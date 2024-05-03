﻿using CSharpFunctionalExtensions;
using MediatR;
using RideSharing.Application.Abstractions;

namespace RideSharing.Application.Trip.Commands.DriverCancelTrip
{
	public class DriverCancelTripCommandHandler(
		ITripRepository tripRepository,
		IDriverRepository driverRepository,
		ITripEventMessageBus tripHandlerEventBus)
		: IRequestHandler<DriverCancelTripCommandDto, Result<Guid>>
	{
		public async Task<Result<Guid>> Handle(DriverCancelTripCommandDto request, CancellationToken cancellationToken)
		{
			// Step 1: check driver exists
			var driverInDB = await driverRepository.FindByIdAsync(request.DriverId);

			if (driverInDB == null)
			{
				return Result.Failure<Guid>("Driver is not found.");
			}

			// Step 2: check trip request exists
			var activeTrip = await tripRepository.GetActiveTripForDriver(request.DriverId);

			if (activeTrip == null)
			{
				return Result.Failure<Guid>("Driver has no active trip.");
			}

			// Step 3: prepare entity
			var entityResult = activeTrip.CancelByDriver();

			if (entityResult.IsFailure)
			{
				return Result.Failure<Guid>(entityResult.Error);
			}

			// Step 4: perform database operations

			var transaction = await tripRepository.BeginTransactionAsync();

			try
			{
				// Note: log table is inserted from database triggers, not api

				await tripRepository.UpdateAsync(activeTrip);

				await tripRepository.CommitTransactionAsync(transaction);

				tripHandlerEventBus.PublishAsync(activeTrip.GetTripDto());

				// Last Step: return result

				return Result.Success(request.TripId);
			}
			catch (Exception ex)
			{
				await tripRepository.RollBackTransactionAsync(transaction);

				return Result.Failure<Guid>($"Failed with error: {ex.Message}");
			}
		}
	}
}