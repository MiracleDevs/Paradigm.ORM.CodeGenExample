//////////////////////////////////////////////////////////////////////////////////
//  RazorLight.Text.RawString.cs
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Paradigm.ORM.Data.DatabaseAccess.Generic;
using Paradigm.ORM.CodeGenExample.Domain.Entities;

namespace Paradigm.ORM.CodeGenExample.Domain.DataAccess
{
    /// <summary>
    /// Represents a database access object
    /// that allows to create, update, delete and select tracking data from the database.
    /// </summary>
	public partial class TrackingDataDatabaseAccess : DatabaseAccess<TrackingData>, ITrackingDataDatabaseAccess
	{
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingDataDatabaseAccess"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        public TrackingDataDatabaseAccess(IServiceProvider serviceProvider, ICassandraConnector connector) : base(serviceProvider, connector)
		{
		}

		#endregion
	}
}