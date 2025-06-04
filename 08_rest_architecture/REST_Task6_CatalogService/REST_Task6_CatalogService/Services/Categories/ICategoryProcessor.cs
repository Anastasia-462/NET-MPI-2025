using Microsoft.AspNetCore.Mvc;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;

namespace REST_Task6_CatalogService.Services.Categories
{
    public interface ICategoryProcessor
    {
        public List<Category> GetListCategories();

        public void AddCategory(CategoryRequest model);

        public void UpdateCategory(int id, CategoryRequest model);

        public void DeleteCategory(int id);
    }
}
