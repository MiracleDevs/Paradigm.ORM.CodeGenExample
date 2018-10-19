//////////////////////////////////////////////////////////////////////////////////
//  TrackingData.cs
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;

namespace Paradigm.ORM.CodeGenExample.Domain.Interfaces
{
    /// <summary>
    /// Provides an interface for domain and view objects modeling a tracking data.
    /// </summary>
	public partial interface ITrackingData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
		string FirstName { get; }

        /// <summary>
        /// Gets or sets the heat.
        /// </summary>
		double Heat { get; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
		string LastName { get; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
		string Location { get; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
		double Speed { get; }

        /// <summary>
        /// Gets or sets the telepathy powers.
        /// </summary>
		int TelepathyPowers { get; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
		DateTimeOffset Timestamp { get; }

		#endregion
    }
}