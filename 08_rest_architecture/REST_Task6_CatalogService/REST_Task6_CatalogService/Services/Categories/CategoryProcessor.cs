using REST_Task6_CatalogService.Context;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using System.Xml.Linq;

namespace REST_Task6_CatalogService.Services.Categories
{
    public class CategoryProcessor : ICategoryProcessor
    {
        private readonly ILiteContext _context;

        public CategoryProcessor(ILiteContext context)
        {
            _context = context;
        }

        public void AddCategory(CategoryRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Invalid parameter.");
            }

            var name = model.Name;

            var categories = _context.GetCollection<Category>();

            if (categories.Any(x => x.Name == name))
            {
                throw new ArgumentException("Category with this name already exist.");
            }

            _context.Insert<Category>(new Category { Name = name });
        }

        public void DeleteCategory(int id)
        {
            var categories = _context.GetCollection<Category>();

            if (!categories.Any(x => x.Id == id))
            {
                throw new KeyNotFoundException("Such category doesn't exist.");
            }

            var categoryItems = _context.GetCollection<CategoryItem>().Where(x => x.CategoryId == id).ToList();
            var items = (from categoryItem in categoryItems
                         join item in _context.GetCollection<Item>()
                              on categoryItem.ItemId equals item.Id
                         select item).ToList();

            if (items.Any())
            {
                _context.DeleteMany<Item>(x => items.Contains(x));
                _context.DeleteMany<CategoryItem>(x => categoryItems.Contains(x));
            }

            _context.Delete<Category>(id);
        }

        public List<Category> GetListCategories()
        {
            var categories = _context.GetCollection<Category>();
            return categories;
        }

        public void UpdateCategory(int id, CategoryRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Invalid parameter.");
            }

            var category = _context.GetCollection<Category>().FirstOrDefault(x => x.Id == id);
            var name = model.Name;

            if (category == null)
            {
                throw new KeyNotFoundException("Such category doesn't exist.");
            }

            category.Name = name;
            _context.Update<Category>(category);
        }
    }
}
