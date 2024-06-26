﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RideSharing.Application.TripRequest.Commands.TripRequest;
using RideSharing.Common.Entities;

namespace RideSharing.CustomerAPI.Controllers.TripRequest.Commands
{
	[Route("api/external/trip-requests")]
	[ApiController]
	public class TripRequestCommand : ControllerBase
	{
		private readonly IMediator _mediator;

		public TripRequestCommand(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Customer will request a trip using this endpoint by selecting source, destination, cabType.
		/// Initially the driverId will be null because driver is not chosen yet.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<Response<long>>> RequestRide(TripRequestCommandDto model)
		{
			var res = await _mediator.Send(model);

			if (res.IsFailure) return BadRequest(res.Error);

			return Created();
		}
	}
}