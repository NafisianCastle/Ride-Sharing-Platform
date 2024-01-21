﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RideSharing.Application.TripUseCase.Commands.TripUpdateCommand;
using RideSharing.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace RideSharing.CustomerAPI.Controllers.Trip.Commands
{
	[Route("api/external/trips")]
	[ApiController]
	public class TripUpdateCommand : ControllerBase
	{
		private readonly IMediator _mediator;

		public TripUpdateCommand(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPut("{tripId}/update")]
		public async Task<ActionResult<Response<TripUpdateCommandResponseDto>>> Update([Required] Guid tripId, TripUpdateCommandDto model)
		{
			model.TripId = tripId;
			var res = await _mediator.Send(model);
			if (res.IsFailure) return BadRequest(res.Error);
			return Ok($"Ride request {res.Value.TripId} has been canceled.");
		}
	}
}