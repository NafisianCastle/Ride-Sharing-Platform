﻿using AuthService.Entity;
using Microsoft.AspNetCore.Mvc;
using RideSharing.Common.Entities;
using RideSharing.Entity;
using RideSharing.Entity.Entities;
using RideSharing.Entity.Enums;
using RideSharing.Infrastructure;
using RideSharing.Service;
using Sayeed.Generic.OnionArchitecture.Controller;

namespace RideSharing.API
{
    [Route("api/v1/trips")]
    [ApiController]
    public class TripController : BaseController<Trip>
    {

        private readonly ApplicationDbContext _dbContext;

        public TripController(ITripService baseService) : base(baseService)
        {
        }

        [HttpPost("request")]
        public async Task<ActionResult<Response<RideRequest>>> RequestRide(RideRequest model)
        {
            if (!ModelState.IsValid) // this one line with check "are all required fields of registerDto provided or not"
                throw new CustomException("Model is not valid!", 400);
           
            model.Status = RideStatusEnum.Pending;
            model.RequestTime = DateTime.UtcNow;

            _dbContext.RideRequests.Add(model);
            await _dbContext.SaveChangesAsync();

            return Ok($"Ride request {model.Id} submitted successfully.");

        }


        [HttpPut("cancel/{requestId}")]
        public async Task<ActionResult<Response<RideRequest>>> CancelRide(int requestId)
        {
            var rideRequest = await _dbContext.RideRequests.FindAsync(requestId);

            if (rideRequest == null)
                return NotFound($"Ride request {requestId} not found.");

            if (rideRequest.Status == RideStatusEnum.Canceled)
                return BadRequest($"Ride request {requestId} is already canceled.");

            rideRequest.Status = RideStatusEnum.Canceled;
            await _dbContext.SaveChangesAsync();

            return Ok($"Ride request {requestId} has been canceled.");
        }


        [HttpGet("status/{requestId}")]
        public async Task<ActionResult<Response<RideRequest>>> GetRideStatus(int requestId)
        {
            var rideRequest = await _dbContext.RideRequests.FindAsync(requestId);

            if (rideRequest == null)
                return NotFound($"Ride request {requestId} not found.");

            return Ok($"Ride request {requestId} status: {rideRequest.Status}");
        }

        [HttpPut("accept/{requestId}/{driverId}")]
        public async Task<ActionResult<Response<RideRequest>>> AcceptRide(int requestId, int driverId)
        {
            var rideRequest = await _dbContext.RideRequests.FindAsync(requestId);
            var driver = await _dbContext.Drivers.FindAsync(driverId);

            if (rideRequest == null)
                return NotFound($"Ride request {requestId} not found.");

            if (driver == null)
                return NotFound($"Driver {driverId} not found.");

            if (rideRequest.Status != RideStatusEnum.Pending)
                return BadRequest($"Ride request {requestId} is not pending.");

            rideRequest.Status = RideStatusEnum.Accepted;

            // Associate the driver with the ride request (if needed)
            rideRequest.Driver = driver;

            await _dbContext.SaveChangesAsync();

            return Ok($"Ride request {requestId} has been accepted by driver {driver.Name}.");
        }
    }
}