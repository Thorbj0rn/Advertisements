using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.AdvertisementService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advertisements.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advService;
        private readonly ILogger<AdvertisementController> _logger;

        public AdvertisementController(IAdvertisementService advService, ILogger<AdvertisementController> logger)
        {
            _advService = advService;
            _logger = logger;
        }

        [HttpPost("update-advertisement")]

        public async Task<ActionResult<bool>> UpdateAdvertisement([FromForm] UpdateAdvertisementRequest req)
        {
            try
            {
                var res = await _advService.UpdateAdvertisement(req);
                return new OkObjectResult(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("get-advertisements")]
        public async Task<ActionResult<List<AdvertisementResponse>>> GetAdvertisements ([FromForm]AdvertisementsRequest req)
        {
            try
            {
                var res = await _advService.GetAdvertisements(req);
                return new OkObjectResult(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
