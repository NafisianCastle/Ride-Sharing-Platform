﻿using Dapper;
using RideSharing.Application.Abstractions;
using RideSharing.Domain.Entities;
using RideSharing.Domain.Enums;
using System.Text;

namespace RideSharing.Infrastructure.Repositories
{
	public class TripRequestRepository : BaseRepository<TripRequestEntity>, ITripRequestRepository
	{
		public TripRequestRepository(
			ApplicationDbContext applicationDbContext,
			DapperContext dapperContext)
			: base(applicationDbContext, dapperContext)
		{

		}

		public async Task<TripRequestEntity> GetActiveTripRequestForCustomer(Guid customerId)
		{
			// If a trip is requested in less than one minute and it is neither canceled nor started, it is considered an active requested trip.
			// If a trip request has no activity within one minute, it is considered auto-canceled.

			DateTime oneMinuteAgo = DateTime.UtcNow.AddMinutes(-1);

			var query = new StringBuilder();

			query.Append($"SELECT * FROM \"TripRequests\"");
			query.Append($" WHERE \"{nameof(TripRequestEntity.Status)}\" = @{nameof(TripRequestEntity.Status)}");
			query.Append($" AND \"{nameof(TripRequestEntity.UpdatedAt)}\" >= @{nameof(oneMinuteAgo)}");
			query.Append($" AND \"{nameof(TripRequestEntity.CustomerId)}\" = @{nameof(TripRequestEntity.CustomerId)}");
			query.Append(" LIMIT 1");

			var parameters = new DynamicParameters();

			parameters.Add(nameof(TripRequestEntity.Status), (int)TripRequestStatus.NoDriverAccepted, System.Data.DbType.Int16);
			parameters.Add(nameof(oneMinuteAgo), oneMinuteAgo, System.Data.DbType.DateTime);
			parameters.Add(nameof(TripRequestEntity.CustomerId), customerId, System.Data.DbType.Guid);

			using (var connection = _dapperContext.CreateConnection())
			{
				var tripRequest = await connection.QueryFirstOrDefaultAsync<TripRequestEntity>(query.ToString(), parameters);
				return tripRequest;
			}
		}

	}
}