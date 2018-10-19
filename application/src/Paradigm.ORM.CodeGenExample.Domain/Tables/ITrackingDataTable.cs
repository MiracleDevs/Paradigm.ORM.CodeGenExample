//////////////////////////////////////////////////////////////////////////////////
//  ITrackingDataTable.cs
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.CodeGenExample.Domain.Tables
{
	[Table("tracking", "", "tracking_data")]
	public interface ITrackingDataTable
    {
        #region Properties
	
		[Column("first_name", "text")]
		[NotNullable]
		[PrimaryKey]
		string FirstName { get; }
	
		[Column("heat", "double")]
		[NotNullable]
		double Heat { get; }
	
		[Column("last_name", "text")]
		[NotNullable]
		[PrimaryKey]
		string LastName { get; }
	
		[Column("location", "text")]
		[NotNullable]
		string Location { get; }
	
		[Column("speed", "double")]
		[NotNullable]
		double Speed { get; }
	
		[Column("telepathy_powers", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		int TelepathyPowers { get; }
	
		[Column("timestamp", "timestamp")]
		[NotNullable]
		DateTimeOffset Timestamp { get; }

		#endregion
    }
}