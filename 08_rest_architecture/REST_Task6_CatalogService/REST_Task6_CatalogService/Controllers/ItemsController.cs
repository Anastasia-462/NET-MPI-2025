using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_Task6_CatalogService;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using REST_Task6_CatalogService.Services.Categories;
using REST_Task6_CatalogService.Services.Items;

namespace REST_Task6_CatalogService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IItemProcessor _itemProcessor;

        public ItemsController(ILogger<ItemsController> logger, IItemProcessor itemProcessor)
        {
            _logger = logger;
            _itemProcessor = itemProcessor;
        }

        [HttpGet]
        public IActionResult GetItems([FromQuery] int page, [FromQuery] int? categoryId)
        {
            return Ok(_itemProcessor.GetListItems(page, categoryId));
        }

        [HttpPost]
        public IActionResult AddItem([FromQuery] int categoryId, [FromBody] ItemRequest model)
        {
            _itemProcessor.AddItem(categoryId, model);
            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, [FromBody] ItemRequest model)
        {
            _itemProcessor.UpdateItem(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            _itemProcessor.DeleteItem(id);
            return NoContent();
        }
    }
}