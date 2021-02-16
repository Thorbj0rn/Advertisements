using Advertisements.Interfaces;
using Advertisements.Interfaces.Models.AdvertisementService;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Редактирует объявление
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
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
                throw ex;
            }
        }

        /// <summary>
        /// Добавляет объявление
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add-advertisement")]
        public async Task<ActionResult<int>> AddAdvertisement([FromForm] AddAdvertisementRequest req)
        {
            try
            {
                var res = await _advService.AddAdvertisement(req);
                return new OkObjectResult(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Удаляет объявление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("delete-advertisement")]
        public async Task<ActionResult<bool>> DeleteAdvertisement(Guid id)
        {
            try
            {
                var res = await _advService.DeleteAdvertisement(id);
                return new OkObjectResult(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Возвращает список объявлений
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("get-advertisements")]
        public async Task<ActionResult<List<AdvertisementResponse>>> GetAdvertisements (AdvertisementsRequest req)
        {
            try
            {
                var res = await _advService.GetAdvertisements(req);
                return new OkObjectResult(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
