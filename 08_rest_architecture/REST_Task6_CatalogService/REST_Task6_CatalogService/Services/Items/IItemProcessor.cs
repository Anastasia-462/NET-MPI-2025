using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using REST_Task6_CatalogService.Services.Items.Models;
using System.Collections.Generic;

namespace REST_Task6_CatalogService.Services.Items
{
    public interface IItemProcessor
    {
        public void AddItem(int categoryId, ItemRequest model);

        public void DeleteItem(int id);

        public List<Response> GetListItems(int page, int? categoryId);

        public void UpdateItem(int id, ItemRequest model);
    }
}
