# Paradigm.ORM Codegen Example
In this repository will try to explain how to fully configure a .net core solution to use Paradigm.ORM dbfirst approach, and communicate with ScyllaDb as our data backend. We'll go over both the configuration and the development required to communicate with scylla.

## 1. Prequisites


### 1.1. ScyllaDb MMS Example
For this tutorial, we'll be using the MMS example from ScyllaDb  https://www.scylladb.com/2018/08/07/mutant-monitoring-system-training-series/

We'll expect that you read the document hand have a basic understanding of what scylla is, how to create a cluster, and how to create keyspaces and column families.

> We'll take the creation scripts from the article and create a database project using DbPublisher, as an example of how Paradigm.ORM helps to manage db projects.

### 1.2. Download Paradigm.ORM tools
Write about downloading tools and placing them in the PATH, both in windows and unix like systems.

## 2. Configure DbPublisher and the db project
Explain briefly why we use dbpublisher, to allow better source control, individual file versioning, etc.

1. Create a directory called `database`.
2. Inside `database` create two other directories `build` `src` `dist`.

```
databases
├── build
├── dist
└── src
```

3. Inside `src` directory, create two directories called `scripts` and `tables`
4. Inside `src` create a file called `project.json`
5. Inside `scripts` directory create a file called `create_keyspace.cql`, with the following content:
```cql
CREATE KEYSPACE IF NOT EXISTS tracking WITH REPLICATION = {
    'class' : 'SimpleStrategy',
    'replication_factor' : 3
};
```

6. Inside `scripts` directory create another file called `use_keyspace.cql`, with the following content:
```cql
USE tracking;
```

This two files are used to create the keyspace if not exists already, and then select the `tracking` keyspace as the active keyspace. We need to separate this two instructions because the DataStax connector only allows one instruction per command, and the dbpublisher creates a command per file.

7. Inside `tables` we'll create our `tracking_data` column family:
```cql
CREATE TABLE IF NOT EXISTS tracking_data (
       first_name       text,
       last_name        text,
       timestamp        timestamp,
       location         varchar,
       speed            double,
       heat             double,
       telepathy_powers int,
       primary key((first_name, last_name), timestamp))
       WITH CLUSTERING ORDER BY (timestamp DESC)
       AND COMPACTION = {'class': 'DateTieredCompactionStrategy',
           'base_time_seconds': 3600,
           'max_sstable_age_days': 1};
```

8. In the `src` finally, we'll create a file called `project.json`, this file will tell the dbpublisher tool how and in what order the execution of the cql files needs to go.
We need to provide the type of database to use, in this case `cassandra`, Paradigm supports other databases like `mysql`, `postgresql`, `sql server`.
We also need to provide the connection string.
The dbpublisher comes with two modes that are not exclusive. One will run each file on a command against a database, and the other one will compile all the files in one big file, to let you run it by hand later if you want. In this case, we'll activate both modes, the `executeScript` and `generateScript` just so you can see both and how they work. After that, we need to write the files we want to run in order. Your file should look something like:

```json
{
       "databaseType" : "cassandra",
       "connectionString": "Contact Points=192.168.2.221;Port=9042",
       "executeScript": true,
       "generateScript": true,
       "outputFileName" : "../dist/tracking.cql",
       "files" : [
               "./scripts/create_keyspace.cql",
               "./scripts/use_keyspace.cql",
               "./tables/tracking_data.cql"
       ]
}
```
9. We can now test if the configuration is correct, and publish the database. Open a terminal or command prompt, and type:

```shell
$ dbpublisher  -c ./project.json -v

------------------------------------------------------------------------------------------------------
Miracle Devs - Paradigm.ORM
DbPublisher Tool

Started at: 17/10/2018 15:25:01
------------------------------------------------------------------------------------------------------
Discovering Files
   Directory:       [~/repositories/github/paradigm-example/database/src]

Individual Files:
   ./src/scripts/create_keyspace.cql
   ./src/scripts/use_keyspace.cql
   ./src/tables/tracking_data.cql
3 script files discovered.
Generating script
Script generated to:
../dist/tracking.cql
Executing scripts
Running script: create_keyspace.cql
Running script: use_keyspace.cql
Running script: tracking_data.cql
Scripts Executed Successfully.
------------------------------------------------------------------------------------------------------
Ended at: 17/10/2018 15:25:01
Elapsed: 0,5341666 sec
------------------------------------------------------------------------------------------------------
```

## 3. Create .net core console application
Add explanation of what are we gonna do.

1. Create a new directory called `application` at the same level as the `database` directory.
2. Inside `application`, create two directories called `build` and `src`.
3. Lets create the solution and projects for this example:

```bash
# creates the application project
dotnet new console -n Paradigm.ORM.CodeGenExample.App

# creates a new solution file
dotnet new sln -n Paradigm.ORM.CodeGenExample

# adds the console application to the solution file
dotnet sln Paradigm.ORM.CodeGenExample.sln add Paradigm.ORM.CodeGenExample.App/

# creates a new standard library for the domain and db access classes.
dotnet new classlib -n Paradigm.ORM.CodeGenExample.Domain

# adds the library to the solution file.
dotnet sln Paradigm.ORM.CodeGenExample.sln add Paradigm.ORM.CodeGenExample.Domain/
```

After creating the solution, your `application` folder you should have a structure like:

```
application
   ├── build
   └── src
       ├── Paradigm.ORM.CodeGenExample.App
       │   ├── Paradigm.ORM.CodeGenExample.App.csproj
       │   └── Program.cs
       ├── Paradigm.ORM.CodeGenExample.Domain
       │   ├── Class1.cs
       │   └── Paradigm.ORM.CodeGenExample.Domain.csproj
       └── Paradigm.ORM.CodeGenExample.sln
```


## 4. Configure Paradigm to create your domain layer
We'll use razor templates already created by us, but you can fill free to modify the templates as see fit.

### 1.1. Configure the DbFirst model collection
We need to tell DbFirst, what we want to map. DbFirst connects to a database, and extract scehma information, plus user configuration, to create a OOP model, with relationships and extra configurations. The DbFirst configuration is similar to a edmx file from the old Entity Framework.

1. Lets create a directory called `configuration` inside `apllication/build/`
2. Inside configuration add these 3 directories: `dbfirst` `codegen` and `templates`
3. Inside `dbfirst` lets create a file called `configuration.json`
   This file will tell DbFirst which connector type is required, the connection string, where to output the model, and which tables, views and stored procedures to map. There are lots of mapping options that we won't see here, that are a hole new subject on itself.
   The file should look something like:

```json
{
       "databaseType" : "cassandra",
       "databaseName" : "tracking",
       "connectionString" : "Contact Points=192.168.2.221;Port=9042",
       "outputFileName" : "../codegen/configuration.dbf",
       "tables": [
               {"name":"tracking_data", "newName": "TrackingData" }
       ],
       "storedProcedures" : [],
       "views" : []
}
```
4. Now we should be able to run the dbfirst tool:

```bash
$ dbfirst -f configuration.json

------------------------------------------------------------------------------------------------------
Miracle Devs - Paradigm.ORM
DbFirst Tool

Started at: 17/10/2018 16:10:17
------------------------------------------------------------------------------------------------------

Individual Files:
   ~/repositories/github/paradigm-example/application/build/configuration/dbfirst/configuration.json
Opening configuration file [configuration.json]
------------------------------------------------------------------------------------------------------
Ended at: 17/10/2018 16:10:18
Elapsed: 0,833207 sec
------------------------------------------------------------------------------------------------------
```

If the tool finished without errors, a new file should be located at the `codegen` directory.
This file contains all the required data that CodeGen requires in order to generate the boilerplate code.

### 1.2. Configure the CodeGen tool
1. Create a file called `configuration.json` inside `application/build/configuration/codegen`.
   You should have two files now, `configuration.json` and `configuration.dbf`. The dbf file will act as an input for the code gen tool, providing the model classes, relationships and other decorations.
2. The CodeGen has two main configuration areas: the input, and the output. CodeGen let you choose which input and    otuput plugins to use on each execution. In this case, will use the json input plugin, and the razor output
   plugin. Basically, the tool will receive the dbf file (dbf file is a json file), and will execute razor templates to produce our final code.

   We need to tell the CodeGen which type of objects inside the dbf models we want to use, and which template to use. We use the type matchers to select objects from the model list.

   We are gonna generate a domain interface, a domain class, a table mapping interface, a database access internface and implementation, and a domain mapper and its interface. Our configuration file for the tool should look something like:

```json

{
    /* will use the json input plugin, to open the model generated with the DbFirst tool */
    "input": {
        "inputType": "Json",
        "parameters": [
            {
                "name": "jsonFiles",
                "value": "configuration.dbf"
            }
        ]
    },

    /* will use the razor template engine as an output plugin */
    "output": {
        "outputType": "Razor",
        "parameters": [
            {
                "name": "nativeTypeTranslatorsPath",
                "value": "../templates/orm/translations.json"
            }
        ],
        "outputFiles": [
            /* DOMAIN INTERFACE */
            {
                "name": "DomainInterface",
                "templatePath": "../templates/orm/domaininterfaces/domaininterface.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/Interfaces/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "I{0}.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Interfaces"
                    }
                ]
            },
            /* TABLE INTERFACE */
            {
                "name": "TableInterface",
                "templatePath": "../templates/orm/tables/tableinterface.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/Tables/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "I{0}Table.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Tables"
                    },
                    {
                        "name": "InterfacesNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Interfaces"
                    }
                ]
            },
            /* DOMAIN ENTITY */
            {
                "name": "DomainEntity",
                "templatePath": "../templates/orm/domainentities/domainentity.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/Entities/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "{0}.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Entities"
                    },
                    {
                        "name": "InterfacesNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Interfaces"
                    },
                    {
                        "name": "TablesNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Tables"
                    },
                    {
                        "name": "IgnoreProperties",
                        "value": ""
                    }
                ]
            },
            /* DATABASE ACCESS INTERFACE */
            {
                "name": "DatabaseAccessInterface",
                "templatePath": "../templates/orm/databaseaccess/idatabaseaccess.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/DatabaseAccess/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "I{0}DatabaseAccess.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.DatabaseAccess"
                    },
                    {
                        "name": "DomainNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Entities"
                    }
                ]
            },
            /* DATABASE ACCESS */
            {
                "name": "DatabaseAccess",
                "templatePath": "../templates/orm/databaseaccess/databaseaccess.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/DatabaseAccess/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "{0}DatabaseAccess.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.DatabaseAccess"
                    },
                    {
                        "name": "DomainNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Entities"
                    },
                    {
                        "name": "Connector",
                        "value": "ICassandraConnector"
                    }
                ]
            },
            /* DATA READER MAPPER INTERFACE */
            {
                "name": "DatabaseReaderMapperInterface",
                "templatePath": "../templates/orm/mappers/imapper.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/Mappers/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "I{0}DatabaseReaderMapper.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Mappers"
                    },
                    {
                        "name": "DomainNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Entities"
                    }
                ]
            },
            /* DATA READER MAPPER */
            {
                "name": "DatabaseReaderMapper",
                "templatePath": "../templates/orm/mappers/mapper.tpl",
                "outputPath": "../../../src/Paradigm.ORM.CodeGenExample.Domain/Mappers/",
                "typeMatchers": [
                    {
                        "name": "ContainsAttribute",
                        "parameters": [
                            "TableAttribute"
                        ]
                    }
                ],
                "namingRules": [
                    {
                        "name": "Format",
                        "parameters": [
                            "{0}DatabaseReaderMapper.cs"
                        ]
                    }
                ],
                "parameters": [
                    {
                        "name": "Namespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Mappers"
                    },
                    {
                        "name": "DomainNamespace",
                        "value": "Paradigm.ORM.CodeGenExample.Domain.Entities"
                    },
                    {
                        "name": "Connector",
                        "value": "ICassandraConnector"
                    }
                ]
            }
        ]
    }
}
```
3. Now we are ready to generate the our application domain code, by typing:
```bash
codegen -f application/build/configuration/codegen/configuration.json

------------------------------------------------------------------------------------------------------
Miracle Devs
Code Generation Tool

Started at: 18/10/2018 09:21:49
------------------------------------------------------------------------------------------------------
Discovering Files
   Directory:       [~/repositories/github/paradigm-example]

Individual Files:
   ~/repositories/github/paradigm-example/database/src/application/build/configuration/codegen/configuration.json

Starting to read configuration file [configuration.json]
Max Parallelism: 4
Input  Type:     Json
Output Type:     Razor
Begining Code Generation...
Registering Templates:
- Template [domaininterface.tpl]
- Template [tableinterface.tpl]
- Template [domainentity.tpl]
- Template [idatabaseaccess.tpl]
- Template [databaseaccess.tpl]
- Template [imapper.tpl]
- Template [mapper.tpl]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]

Requesting assembly [Microsoft.AspNetCore.Razor.Language.resources]
- Starting output configuration file [TableInterface]: 1 files generated.
- Starting output configuration file [DomainInterface]: 1 files generated.
- Starting output configuration file [DatabaseAccessInterface]: 1 files generated.
- Starting output configuration file [DomainEntity]: 1 files generated.
- Starting output configuration file [DatabaseAccess]: 1 files generated.
- Starting output configuration file [DatabaseReaderMapperInterface]: 1 files generated.
- Starting output configuration file [DatabaseReaderMapper]: 1 files generated.

------------------------------------------------------------------------------------------------------
Ended at: 18/10/2018 09:21:54
Elapsed: 4,7440401 sec
------------------------------------------------------------------------------------------------------
```
4. If the tool executed successfully, you should now see your domain project already populated with
   all the different classes.

> We are creating all the different layers in one library, just for the sake of the example, but you
  will probably want to separate the files in different layers, like Data, Domain, Interfaces, Mappings, etc.

5. If you open the sln file into vscode or visual studio, you'll see that some references are missing,
   we need to add the proper nuget packages. First, navigate to `Paradigm.ORM.CodeGenExample/Paradigm.ORM.CodeGenExample.Domain` and then type:
```bash
dotnet add package Paradigm.ORM.Data
```

6. Your domain project is almost done. If you compile, you'll se an error that ICassandraConnector is
   missing. We are using one of the signatures of the paradigm orm classes that requires a explicit connector to be passed. This allow us to have different database connectors, even to a different databases like scylla, mysql or postgresql, in the same injection container.

   We need to add a new interface called `ICassandraConnector.cs` to the domain project, with the following code:
```csharp
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.CodeGenExample.Domain
{
   public interface ICassandraConnector: IDatabaseConnector
   {
   }
}
```

The project should compile without errors now. Your project tree should look like:

```
.
├── DataAccess
│   ├── ITrackingDataDatabaseAccess.cs
│   └── TrackingDataDatabaseAccess.cs
├── Entities
│   └── TrackingData.cs
├── Interfaces
│   └── ITrackingData.cs
├── Mappers
│   ├── ITrackingDataDatabaseReaderMapper.cs
│   └── TrackingDataDatabaseReaderMapper.cs
├── Tables
│   └── ITrackingDataTable.cs
├── ICassandraConnector.cs
├── Paradigm.ORM.CodeGenExample.Domain.csproj
```

## 6. Generate the application code
1. First of all, lets add the reference to the domain project in the application console project. Go to `Paradigm.ORM.CodeGenExample/Paradigm.ORM.CodeGenExample.App` and write:
```bash
dotnet add reference ../Paradigm.ORM.CodeGenExample.Domain/
```
2. Now we need to add the nuget packet with the right connector. In the domain layer, we used the core abstract library, but the application itself should known to which database or databases should connect:
```bash
dotnet add package Paradigm.ORM.Data.Cassandra
```
3. We need to implement `ICassandraConnector`, so we'll create a `CassandraConnector.cs` file, and write the following code:
4. Now we'll add a referece to `Microsoft.Extensions.DependencyInjection` because we want to use dependency injection. So let's write:
```bash
dotnet add package Microsoft.Extensions.DependencyInjection
```
5. Lets add now a new class called `Startup`.
6. Add a property`:
```csharp
private IServiceProvider ServiceProvider { get; set; }
```
7. Add a new method:
```csharp
public void Configure()
{
}
```
   In this method, we'll register all our classes, services, etc, and we'll create our injection container, the service provider in .net core terms.

8. Let's register all our classes:
```csharp
var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<ICassandraConnector, CassandraConnector>();
serviceCollection.AddTransient<ITrackingData, TrackingData>();
serviceCollection.AddTransient<TrackingData>();
serviceCollection.AddTransient<ITrackingDataDatabaseAccess, TrackingDataDatabaseAccess>();
serviceCollection.AddTransient<ITrackingDataDatabaseReaderMapper, TrackingDataDatabaseReaderMapper>();

this.ServiceProvider = serviceCollection.BuildServiceProvider();
```

9. Add a new async method:
```csharp
public async Task RunAsync()
{
}
```


## 7. Connecting to the MMS database (CRUD)


# Final Thoughts


