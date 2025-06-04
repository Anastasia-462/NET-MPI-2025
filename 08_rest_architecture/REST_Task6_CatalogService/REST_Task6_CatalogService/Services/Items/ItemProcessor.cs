using REST_Task6_CatalogService.Context;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using REST_Task6_CatalogService.Services.Items.Models;
using System.Collections.Generic;

namespace REST_Task6_CatalogService.Services.Items
{
    public class ItemProcessor : IItemProcessor
    {
        private const int Limit = 10;

        private readonly ILiteContext _context;

        public ItemProcessor(ILiteContext context)
        {
            _context = context;
        }

        public void AddItem(int categoryId, ItemRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Invalid parameter.");
            }

            var name = model.Name;

            var category = _context.GetCollection<Category>().FirstOrDefault(x => x.Id == categoryId);

            if (category == null)
            {
                throw new KeyNotFoundException("Category with this name already exist.");
            }

            var id = _context.InsertWithId<Item>(new Item { Name = name, Author = model.Author });
            _context.Insert<CategoryItem>(new CategoryItem { CategoryId = categoryId, ItemId = id });
        }

        public void DeleteItem(int id)
        {
            var item = _context.GetCollection<Item>().FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new KeyNotFoundException("Such item doesn't exist.");
            }

            _context.DeleteMany<CategoryItem>(x => x.ItemId == id);
            _context.Delete<Item>(id);
        }

        public List<Response> GetListItems(int page, int? categoryId)
        {
            Func<Category, bool> predicate = categoryId != null
                                            ? x => x.Id == categoryId
                                            : x => true;

            var categories = _context.GetCollection<Category>();

            if (categoryId != null && !categories.Any(x => x.Id == categoryId))
            {
                throw new KeyNotFoundException("Such category doesn't exist.");
            }

            var itemModels = (from item in _context.GetCollection<Item>()
                              join categoryItem in _context.GetCollection<CategoryItem>()
                                   on item.Id equals categoryItem.ItemId
                              join category in _context.GetCollection<Category>().Where(predicate)
                                   on categoryItem.CategoryId equals category.Id
                              select new Response
                              {
                                  CategoryName = category.Name,
                                  ItemId = item.Id,
                                  ItemName = item.Name,
                                  Author = item.Author
                              }).DistinctBy(x => x.ItemId).ToList();

            var currentPage = page;
            var totalPages = (int)Math.Ceiling((double)itemModels.Count / Limit);
            totalPages = itemModels.Any() ? totalPages : 1;

            if (currentPage > totalPages || currentPage <= 0)
            {
                throw new ArgumentException("Invalid page.");
            }

            var itemsToSkip = (currentPage - 1) * Limit;

            return itemModels.Skip(itemsToSkip).Take(Limit).ToList();
        }

        public void UpdateItem(int id, ItemRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Invalid parameter.");
            }

            var item = _context.GetCollection<Item>().FirstOrDefault(x => x.Id == id);
            var name = model.Name;

            if (item == null)
            {
                throw new KeyNotFoundException("Such item doesn't exist.");
            }

            item.Name = name;
            item.Author = model.Author;
            _context.Update<Item>(item);
        }
    }
}
