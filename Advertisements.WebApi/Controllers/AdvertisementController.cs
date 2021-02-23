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
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult<bool>> Update([FromForm] UpdateAdvertisementRequest request)
        {
            try
            {                
                var res = await _advService.Update(request);
                return new OkObjectResult(res);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Добавляет объявление
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<int>> Add([FromForm] AddAdvertisementRequest request)
        {
            try
            {
                var res = await _advService.Add(request);
                return new OkObjectResult(res);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Удаляет объявление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                var res = await _advService.Delete(id);
                return new OkObjectResult(res);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Возвращает список объявлений
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("get")]
        public async Task<ActionResult<List<AdvertisementResponse>>> Get(AdvertisementsRequest request)
        {
            try
            {
                var res = await _advService.Get(request);
                return new OkObjectResult(res);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Возвращает ссылку на изображения заданного размера
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("get-image-url")]
        public async Task<ActionResult<List<string>>> GetImageUrl(AdvertisementImageUrlRequest request)
        {
            try
            {
                var res = await _advService.GetImageUrl(request);
                return new OkObjectResult(res);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

    }
}
