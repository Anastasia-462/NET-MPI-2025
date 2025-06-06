using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using REST_Task6_CatalogService.Services.Categories;

namespace REST_Task6_CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryProcessor _categoryProcessor;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryProcessor categoryProcessor)
        {
            _logger = logger;
            _categoryProcessor = categoryProcessor;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCategories()
        {
            return Ok(_categoryProcessor.GetListCategories());
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddCategory([FromBody] CategoryRequest category)
        {
            _categoryProcessor.AddCategory(category);
            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryRequest category)
        {
            _categoryProcessor.UpdateCategory(id, category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _categoryProcessor.DeleteCategory(id);
            return NoContent();
        }
    }
}