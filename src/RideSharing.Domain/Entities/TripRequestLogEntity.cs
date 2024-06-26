﻿using NetTopologySuite.Geometries;

namespace RideSharing.Domain.Entities
{
	public class TripRequestLogEntity : BaseEntity
	{
		public TripRequestLogEntity()
		{

		}
		public TripRequestLogEntity(TripRequestEntity tripRequest)
		{
			TripRequestId = tripRequest.Id;
			CustomerId = tripRequest.CustomerId;
			Source = tripRequest.Source;
			Destination = tripRequest.Destination;
			CabType = tripRequest.CabType;
			PaymentMethod = tripRequest.PaymentMethod;
			Status = tripRequest.Status;
		}

		public long TripRequestId { get; set; }
		public virtual TripRequestEntity TripRequest { get; set; }
		public long CustomerId { get; set; }
		public virtual CustomerEntity Customer { get; set; }
		public long? DriverId { get; set; }
		public virtual DriverEntity? Driver { get; set; }
		public Point Source { get; set; }
		public Point Destination { get; set; }
		public CabType CabType { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public TripRequestStatus Status { get; set; }
	}
}
