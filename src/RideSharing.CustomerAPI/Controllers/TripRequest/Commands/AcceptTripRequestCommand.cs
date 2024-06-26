﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RideSharing.Application.TripRequest.Commands.AcceptTripRequest;
using RideSharing.Common.Entities;

namespace RideSharing.CustomerAPI.Controllers.TripRequest.Commands
{
	[Route("api/external/trip-requests")]
	[ApiController]
	public class AcceptTripRequestCommand : ControllerBase
	{
		private readonly IMediator mediator;

		public AcceptTripRequestCommand(IMediator mediator)
		{
			this.mediator = mediator;
		}

		/// <summary>
		/// Use this endpoint to accept trip request for driver
		/// </summary>
		/// <param name="tripRequestId"></param>
		/// <returns></returns>
		[HttpPut("{tripRequestId}/accept")]
		public async Task<ActionResult<Response<long>>> RequestRide(long tripRequestId)
		{
			var driverId = new long(); // TODO:- get customerId from httpContextAccessor!

			var model = new AcceptTripRequestDto(driverId, tripRequestId);

			var res = await mediator.Send(model);

			if (res.IsFailure) return BadRequest(res.Error);

			return Ok(res.Value);
		}
	}
}
