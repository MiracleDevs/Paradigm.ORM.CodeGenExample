using Microsoft.Extensions.DependencyInjection;
using Paradigm.ORM.CodeGenExample.Domain;
using Paradigm.ORM.CodeGenExample.Domain.DataAccess;
using Paradigm.ORM.CodeGenExample.Domain.Entities;
using Paradigm.ORM.CodeGenExample.Domain.Interfaces;
using Paradigm.ORM.CodeGenExample.Domain.Mappers;
using Paradigm.ORM.Data.DatabaseAccess.Generic;
using Paradigm.ORM.Data.Mappers.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigm.ORM.CodeGenExample.App
{
    /// <summary>
    /// 
    /// </summary>
    internal class Startup
    {
        #region Properties

        /// <summary>
        /// Gets the random generator.
        /// </summary>
        /// <value>
        /// The random generator.
        /// </value>
        private Random Random { get; }

        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        private IServiceProvider ServiceProvider { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            this.Random = new Random();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Configures the startup instance.
        /// </summary>
        public void Configure()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<MutantRepository>();
            serviceCollection.AddSingleton<ICassandraConnector, CassandraConnector>();
            serviceCollection.AddTransient<ITrackingData, TrackingData>();
            serviceCollection.AddTransient<TrackingData>();
            serviceCollection.AddTransient<ITrackingDataDatabaseAccess, TrackingDataDatabaseAccess>();
            serviceCollection.AddTransient<ITrackingDataDatabaseReaderMapper, TrackingDataDatabaseReaderMapper>();
            serviceCollection.AddTransient<IDatabaseAccess<TrackingData>, TrackingDataDatabaseAccess>();
            serviceCollection.AddTransient<IDatabaseReaderMapper<TrackingData>, TrackingDataDatabaseReaderMapper>();

            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Runs the application asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            using (var connector = this.ServiceProvider.GetRequiredService<ICassandraConnector>())
            {
                connector.Initialize("Contact Points=192.168.2.221;Port=9042");

                await connector.OpenAsync();

                if (!connector.IsOpen())
                {
                    Console.WriteLine("Couldn't connect to the server.");
                    return;
                }

                Console.WriteLine("We are connected ...");
                var databaseAccess = this.ServiceProvider.GetRequiredService<ITrackingDataDatabaseAccess>();

                // 1. Insert testing data.
                await InsertDataAsync(databaseAccess);

                // 2. Getting readings from the database.
                await ReadDataAsync(databaseAccess);

                // 3. Deleting readings.
                await DeleteDataAsync(databaseAccess);

                await connector.CloseAsync();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates testing data.
        /// </summary>
        /// <returns>A list of <see cref="TrackingData"/></returns>
        private IEnumerable<TrackingData> GenerateData()
        {
            var result = new List<TrackingData>();
            var mutantRepository = this.ServiceProvider.GetRequiredService<MutantRepository>();

            for (var i = 0; i < 1000; i++)
            {
                var trackingData = this.ServiceProvider.GetRequiredService<TrackingData>();
                var mutant = mutantRepository.GetById(this.Random.Next(0, 4));

                trackingData.FirstName = mutant.FirstName;
                trackingData.LastName = mutant.LastName;
                trackingData.Location = "Canada";
                trackingData.Heat = 10 * Random.NextDouble();
                trackingData.Speed = 10 * Random.NextDouble();
                trackingData.TelepathyPowers = Random.Next(0, 10);
                trackingData.Timestamp = DateTimeOffset.Now.AddMilliseconds(Random.Next(0, 10000));

                result.Add(trackingData);
            }

            return result;
        }

        /// <summary>
        /// Inserts the data asynchronously.
        /// </summary>
        /// <param name="databaseAccess">The database access.</param>
        /// <returns></returns>
        private async Task InsertDataAsync(ITrackingDataDatabaseAccess databaseAccess)
        {
            Console.WriteLine("Inserting mutant readings...");
            await databaseAccess.InsertAsync(this.GenerateData());
        }

        /// <summary>
        /// Reads the data asynchronously.
        /// </summary>
        /// <param name="databaseAccess">The database access.</param>
        /// <returns></returns>
        private static async Task ReadDataAsync(ITrackingDataDatabaseAccess databaseAccess)
        {
            Console.WriteLine("Getting mutant readings...");
            var mutantReadings = await databaseAccess.SelectAsync();

            foreach (var tracking in mutantReadings)
            {
                Console.WriteLine($"{tracking.FirstName} {tracking.LastName} ({tracking.Location}) : [speed: {tracking.Speed}, heat: {tracking.Heat}]");
            }
        }

        /// <summary>
        /// Deletes the data asynchronously.
        /// </summary>
        /// <param name="databaseAccess">The database access.</param>
        /// <returns></returns>
        private static async Task DeleteDataAsync(ITrackingDataDatabaseAccess databaseAccess)
        {
            Console.WriteLine("Deleting mutant readings...");

            var mutantReadings = await databaseAccess.SelectAsync();
            var mutants = mutantReadings.GroupBy(x => new { x.FirstName, x.LastName }).Select(x => x.First()).ToList();

            foreach (var tracking in mutants)
            {
                await databaseAccess.DeleteAsync(tracking);
            }

            if (!(await databaseAccess.SelectAsync()).Any())
            {
                Console.WriteLine("All entries have been deleted");
            }
        }

        #endregion
    }
}
