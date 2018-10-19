//////////////////////////////////////////////////////////////////////////////////
//  TrackingData.cs
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.CodeGenExample.Domain.Interfaces;
using Paradigm.ORM.CodeGenExample.Domain.Tables;

namespace Paradigm.ORM.CodeGenExample.Domain.Entities
{
    /// <summary>
    /// Represents a tracking data domain entity.
    /// </summary>
	[DataContract]
	[TableType(typeof(ITrackingDataTable))]
	public partial class TrackingData : ITrackingData, ITrackingDataTable
    {
        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        #endregion
		#region Public Properties

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		[DataMember]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the heat.
		/// </summary>
		[DataMember]
		public double Heat { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		[DataMember]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		[DataMember]
		public string Location { get; set; }

		/// <summary>
		/// Gets or sets the speed.
		/// </summary>
		[DataMember]
		public double Speed { get; set; }

		/// <summary>
		/// Gets or sets the telepathy powers.
		/// </summary>
		[DataMember]
		public int TelepathyPowers { get; set; }

		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		[DataMember]
		public DateTimeOffset Timestamp { get; set; }

		#endregion

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingData"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to the service provider.</param>
		public TrackingData(IServiceProvider serviceProvider)
		{
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            
			this.Initialize();
		}

		#endregion

		#region Public Methods

        /// <summary>
        /// Determines whether this instance is new.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </returns>
		public bool IsNew()
		{
			return this.FirstName == default(String) || this.LastName == default(String);
		}

		#endregion

		#region Partial Methods

        /// <summary>
        /// Initializes the entity right after the instantiation.
        /// </summary>
		partial void Initialize();

		#endregion
    }
}
