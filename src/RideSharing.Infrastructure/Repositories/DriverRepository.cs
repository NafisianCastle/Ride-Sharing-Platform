﻿using RideSharing.Application.Abstractions;
using RideSharing.Domain.Entities;
using RideSharing.Infrastructure;
using RideSharing.Infrastructure.Repositories;

namespace RideSharing.Application
{
	public class DriverRepository : BaseRepository<DriverEntity>, IDriverRepository
	{
		public DriverRepository(ApplicationDbContext applicationDbContext, DapperContext dapperContext) : base(applicationDbContext, dapperContext)
		{
		}
	}
}