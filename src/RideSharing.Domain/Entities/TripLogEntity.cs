﻿using NetTopologySuite.Geometries;

namespace RideSharing.Domain.Entities
{
	public class TripLogEntity : BaseEntity
	{
		public TripLogEntity()
		{

		}

		public TripLogEntity(TripEntity trip) : base()
		{
			TripId = trip.Id;
			TripRequestId = trip.TripRequestId;
			CustomerId = trip.CustomerId;
			DriverId = trip.DriverId;
			PaymentMethod = trip.PaymentMethod;
			TripStatus = trip.TripStatus;
			Source = trip.Source;
			Destination = trip.Destination;
			CabType = trip.CabType;
		}

		public long TripId { get; set; }
		public virtual TripEntity Trip { get; set; }
		public long TripRequestId { get; set; }
		public virtual TripRequestEntity TripRequest { get; set; }
		public long CustomerId { get; set; }
		public virtual CustomerEntity Customer { get; set; }
		public long DriverId { get; set; }
		public virtual DriverEntity Driver { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public TripStatus TripStatus { get; set; }
		public Point Source { get; set; }
		public Point Destination { get; set; }
		public CabType CabType { get; set; }
	}
}
