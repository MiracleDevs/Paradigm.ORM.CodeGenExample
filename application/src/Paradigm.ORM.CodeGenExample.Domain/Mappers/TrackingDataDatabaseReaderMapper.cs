//////////////////////////////////////////////////////////////////////////////////
//  TrackingDataDatabaseReaderMapper.cs
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Mappers.Generic;
using Paradigm.ORM.CodeGenExample.Domain.Entities;

namespace Paradigm.ORM.CodeGenExample.Domain.Mappers
{
    /// <summary>
    /// Maps a tracking data from the database to a <see cref="TrackingData"/> class.
    /// </summary>
	public class TrackingDataDatabaseReaderMapper : DatabaseReaderMapper<TrackingData>, ITrackingDataDatabaseReaderMapper
    {
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingDataDatabaseReaderMapper"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to the service provider.</param>
        /// <param name="connector">The database connector.</param>
		public TrackingDataDatabaseReaderMapper(IServiceProvider serviceProvider, ICassandraConnector connector) : base(serviceProvider, connector)
        {
        }

		#endregion

		#region Protected Methods
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Maps one row to an object.
        /// </summary>
        /// <param name="reader">An open database reader cursor.</param>
        /// <returns>A new instance.</returns>
		protected override object MapRow(IDatabaseReader reader)
		{
			var entity = new TrackingData(this.ServiceProvider);
			
			entity.FirstName =  reader.IsDBNull(0) ? default(string) : reader.GetString(0);
			entity.Heat = reader.GetDouble(1);
			entity.LastName =  reader.IsDBNull(2) ? default(string) : reader.GetString(2);
			entity.Location =  reader.IsDBNull(3) ? default(string) : reader.GetString(3);
			entity.Speed = reader.GetDouble(4);
			entity.TelepathyPowers = reader.GetInt32(5);
			entity.Timestamp = reader.GetFieldValue<DateTimeOffset>(6);

			return entity;
		}

        /// <summary>
        /// Maps one row to an object.
        /// </summary>
        /// <param name="reader">An open database reader cursor.</param>
        /// <returns>A new instance.</returns>
		protected override async System.Threading.Tasks.Task<object> MapRowAsync(IDatabaseReader reader)
		{
			var entity = new TrackingData(this.ServiceProvider);
			
			entity.FirstName = await reader.IsDBNullAsync(0) ? default(string) : reader.GetString(0);
			entity.Heat = reader.GetDouble(1);
			entity.LastName = await reader.IsDBNullAsync(2) ? default(string) : reader.GetString(2);
			entity.Location = await reader.IsDBNullAsync(3) ? default(string) : reader.GetString(3);
			entity.Speed = reader.GetDouble(4);
			entity.TelepathyPowers = reader.GetInt32(5);
			entity.Timestamp = reader.GetFieldValue<DateTimeOffset>(6);

			return entity;
		}

        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		#endregion
    }
}