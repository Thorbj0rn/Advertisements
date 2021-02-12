using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advertisements.WebApi.Controllers
{
    [ApiController]
    [Route("user-controller")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost(Name = "update-user")]
        public async Task<ActionResult<bool>> UpdateUser(UpdateUserRequest request)
        {
            try 
            {
                var res = await _userService.UpdateUser(request);
                return new ActionResult<bool>(res);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResult<bool>(false);
            }
        }
    }
}
