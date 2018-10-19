@include "../shared.cs"
@{
	var name = Model.Definition.Name;
	var interfaceName = $"I{name.ToString().Replace("View", "")}";
	var tableName = $"I{name}Table";
	var properties = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Properties;
	var navigationProperties = properties.Where(x => x.Attributes.Any(a => a.Name == "NavigationAttribute")).ToList();
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;
using Microsoft.Extensions.DependencyInjection;
using @Model.Configuration["InterfacesNamespace"];
using @Model.Configuration["TablesNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Represents a @Raw(GetReadableString(Model.Definition.Name)) domain entity.
    /// </summary>
	[DataContract]
	[TableType(typeof(@tableName))]
	public partial class @Raw(name) : @interfaceName, @tableName
    {
@if (navigationProperties.Any())
{
	<text>@Raw(GetPrivateFieldList(Model, navigationProperties, "\t\t"))</text>
}
        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        #endregion
@if (properties.Any())
{
	<text>@Raw(GetPublicPropertyList(Model, properties, "\t\t"))</text>
}
		#region Constructor

        /// @Raw("<summary>")
        /// Initializes a new instance of the @Raw("<see cref=\"" + name + "\"/>") class.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"serviceProvider\">")A reference to the service provider.@Raw("</param>")
		public @(name)(IServiceProvider serviceProvider)
		{
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            @Raw(GetConstructorList(Model, navigationProperties, "\t\t\t"))
			this.Initialize();
		}

		#endregion
@if (ImplementsDomainInterface(Model))
{
<text>
		#region Public Methods

        /// @Raw("<summary>")
        /// Determines whether this instance is new.
        /// @Raw("</summary>")
        /// @Raw("<returns>")
        ///   @Raw("<c>true</c>") if this instance is new; otherwise, @Raw("<c>false</c>").
        /// @Raw("</returns>")
		public bool IsNew()
		{
			return @Raw(GetIsNewCondition(Model.Definition));
		}
</text>
    if (navigationProperties.Any())
    {
		<text>@Raw(GetCrudMethods(Model, navigationProperties, "\t\t"))</text>
    }
<text>
		#endregion
</text>
}

@if (navigationProperties.Any())
{
		<text>@Raw(GetPrivateMethods(Model, navigationProperties, "\t\t"))</text>
}
		#region Partial Methods

        /// @Raw("<summary>")
        /// Initializes the entity right after the instantiation.
        /// @Raw("</summary>")
		partial void Initialize();

		#endregion
    }
}
