﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AlarmWorkflow.Shared.Core;

namespace AlarmWorkflow.Windows.Service.WcfServices
{
    /// <summary>
    /// Wraps an Operation for WCF.
    /// </summary>
    [DataContract()]
    public class OperationItem
    {
        #region Fields

        [DataMember()]
        private byte[] _customData;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the unique Id of this operation.
        /// </summary>
        [DataMember()]
        public int Id { get; set; }
        /// <summary>
        /// Gets/sets the date and time when this operation was created.
        /// </summary>
        [DataMember()]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the Einsatznr object.
        /// </summary>
        [DataMember()]
        public string OperationNumber { get; set; }
        /// <summary>
        /// Gets or sets the Mitteiler object.
        /// </summary>
        [DataMember()]
        public string Messenger { get; set; }
        /// <summary>
        /// Gets or sets the Einsatzort object.
        /// </summary>
        [DataMember()]
        public string Location { get; set; }
        /// <summary>
        /// Gets/sets the street. The street may contain the StreetNumber.
        /// </summary>
        [DataMember()]
        public string Street { get; set; }
        /// <summary>
        /// Gets/sets the street number. This may be empty and/or the street number may be merged into the Street-property.
        /// </summary>
        [DataMember()]
        public string StreetNumber { get; set; }
        /// <summary>
        /// Gets or sets the Ort object.
        /// </summary>
        [DataMember()]
        public string City { get; set; }
        /// <summary>
        /// Gets/sets the zip code of the City.
        /// </summary>
        [DataMember()]
        public string ZipCode { get; set; }
        /// <summary>
        /// Gets or sets the Objekt object.
        /// </summary>
        [DataMember()]
        public string Property { get; set; }
        /// <summary>
        /// Gets or sets the Hinweis object.
        /// </summary>
        [DataMember()]
        public string Comment { get; set; }
        /// <summary>
        /// Gets the Stichwort object.
        /// </summary>
        [DataMember()]
        public string Keyword { get; set; }
        /// <summary>
        /// Gets/sets whether or not this operation is acknowledged, that means that this operation is no longer necessary to be displayed in the UI as "fresh".
        /// If this is set to "false" then this operation will always been shown in the UI. By default, an operation is set to "acknowledged"
        /// either if the user manually acknowledges it or after a defined timespan (usually 8 hours).
        /// </summary>
        [DataMember()]
        public bool IsAcknowledged { get; set; }
        /// <summary>
        /// Gets/sets the route plan image data.
        /// </summary>
        [DataMember()]
        public byte[] RouteImage { get; set; }
        /// <summary>
        /// Gets/sets the custom data.
        /// </summary>
        public IDictionary<string, object> CustomData
        {
            get { return Utilities.Deserialize<IDictionary<string, object>>(_customData); }
            set { _customData = Utilities.Serialize(value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationItem"/> class.
        /// </summary>
        public OperationItem()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationItem"/> class.
        /// </summary>
        /// <param name="operation">The <see cref="Operation"/> to clone.</param>
        /// <param name="detailLevel">The level of detail of the copied information.</param>
        public OperationItem(Operation operation, OperationItemDetailLevel detailLevel)
            : this()
        {
            this.Id = operation.Id;
            this.Timestamp = operation.Timestamp;
            this.OperationNumber = operation.OperationNumber;
            this.Messenger = operation.Messenger;
            this.Location = operation.Location;
            this.Street = operation.Street;
            this.StreetNumber = operation.StreetNumber;
            this.City = operation.City;
            this.ZipCode = operation.ZipCode;
            this.Property = operation.Property;
            this.Comment = operation.Comment;
            this.Keyword = operation.Keyword;
            this.IsAcknowledged = operation.IsAcknowledged;

            // Copy the expensive information only if requested
            if (detailLevel == OperationItemDetailLevel.Full)
            {
                this.RouteImage = operation.RouteImage;
                this.CustomData = operation.CustomData;
            }
        }

        #endregion

        #region Factory methods

        /// <summary>
        /// Converts this instance to its corresponding <see cref="Operation"/>-object.
        /// </summary>
        /// <returns></returns>
        public Operation ToOperation()
        {
            Operation operation = new Operation();
            operation.Id = this.Id;
            operation.Timestamp = this.Timestamp;
            operation.OperationNumber = this.OperationNumber;
            operation.Messenger = this.Messenger;
            operation.Location = this.Location;
            operation.Street = this.Street;
            operation.StreetNumber = this.StreetNumber;
            operation.City = this.City;
            operation.ZipCode = this.ZipCode;
            operation.Property = this.Property;
            operation.Comment = this.Comment;
            operation.Keyword = this.Keyword;
            operation.RouteImage = this.RouteImage;
            operation.CustomData = this.CustomData;
            operation.IsAcknowledged = this.IsAcknowledged;
            return operation;
        }

        #endregion
    }
}
