﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharing.Entity;
using RideSharing.Service;
using Sayeed.Generic.OnionArchitecture.Controller;

namespace RideSharing.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : BaseController<User>
    {
        public UserController(IUserService userService) : base(userService)
        {
        }
    }
}