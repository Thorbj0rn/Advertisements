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
        public async Task<ActionResult<bool>> Update([FromForm] UpdateAdvertisementRequest req)
        {
            try
            {                
                var res = await _advService.Update(req);
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
        public async Task<ActionResult<int>> Add([FromForm] AddAdvertisementRequest req)
        {
            try
            {
                var res = await _advService.Add(req);
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
        [HttpDelete("delete-advertisement/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                var res = await _advService.Delete(id);
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
        public async Task<ActionResult<List<AdvertisementResponse>>> Get(AdvertisementsRequest req)
        {
            try
            {
                var res = await _advService.Get(req);
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
