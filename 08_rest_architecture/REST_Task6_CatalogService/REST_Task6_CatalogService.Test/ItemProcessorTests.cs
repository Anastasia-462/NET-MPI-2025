using Moq;
using REST_Task6_CatalogService.Context;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using REST_Task6_CatalogService.Services.Items;
using REST_Task6_CatalogService.Services.Items.Models;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace REST_Task6_CatalogService.Test
{
    public class ItemProcessorTests
    {
        private readonly Mock<ILiteContext> _mockContext;
        private readonly ItemProcessor _itemProcessor;

        public ItemProcessorTests()
        {
            _mockContext = new Mock<ILiteContext>();
            _itemProcessor = new ItemProcessor(_mockContext.Object);
        }

        [Fact]
        public void AddItem_WhenValidRequest_ShouldAddItem()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "TestCategory" };
            var itemRequest = new ItemRequest { Name = "TestItem", Author = "TestAuthor" };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });

            _mockContext.Setup(c => c.InsertWithId<Item>(It.IsAny<Item>())).Returns(1);

            // Act
            _itemProcessor.AddItem(categoryId, itemRequest);

            // Assert
            _mockContext.Verify(c => c.InsertWithId<Item>(It.Is<Item>(i => i.Name == "TestItem" && i.Author == "TestAuthor")), Times.Once);
            _mockContext.Verify(c => c.Insert<CategoryItem>(It.Is<CategoryItem>(ci => ci.CategoryId == categoryId && ci.ItemId == 1)), Times.Once);
        }

        [Fact]
        public void AddItem_WhenInvalidRequest_ShouldThrowArgumentException()
        {
            // Arrange
            var categoryId = 1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemProcessor.AddItem(categoryId, null));
        }

        [Fact]
        public void AddItem_WhenCategoryDoesNotExist_ShouldKeyNotFoundException()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "TestCategory" };
            var itemRequest = new ItemRequest { Name = "TestItem", Author = "TestAuthor" };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _itemProcessor.AddItem(2, new ItemRequest { Name = "Item" }));
        }

        [Fact]
        public void DeleteItem_WhenItemExists_ShouldDeleteItem()
        {
            // Arrange
            var itemId = 1;

            var category = new Category { Id = 1, Name = "Category" };
            var items = new List<Item> { new Item { Id = itemId, Name = "TestItem" } };
            var categoryItems = new List<CategoryItem> { new CategoryItem { ItemId = itemId, CategoryId = category.Id } };
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });
            _mockContext.Setup(c => c.GetCollection<CategoryItem>()).Returns(categoryItems);
            _mockContext.Setup(c => c.GetCollection<Item>()).Returns(items);

            // Act
            _itemProcessor.DeleteItem(itemId);

            // Assert
            _mockContext.Verify(c => c.DeleteMany<CategoryItem>(It.IsAny<Expression<Func<CategoryItem, bool>>>()), Times.Once);
            _mockContext.Verify(c => c.Delete<Item>(itemId), Times.Once);
        }

        [Fact]
        public void DeleteItem_WhenItemNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var itemId = 1;

            _mockContext.Setup(c => c.GetCollection<Item>()).Returns(new List<Item>());

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _itemProcessor.DeleteItem(itemId));
        }

        [Fact]
        public void GetListItems_WhenPageExceedsTotal_ShouldThrowArgumentException()
        {
            // Arrange
            _mockContext.Setup(c => c.GetCollection<Item>()).Returns(new List<Item>());
            _mockContext.Setup(c => c.GetCollection<CategoryItem>()).Returns(new List<CategoryItem>());
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category>());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemProcessor.GetListItems(2, null));
        }

        [Fact]
        public void GetListItems_WhenSpecifiedCategoryId_ShouldThrowArgumentException()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category" };
            _mockContext.Setup(c => c.GetCollection<Item>()).Returns(new List<Item>());
            _mockContext.Setup(c => c.GetCollection<CategoryItem>()).Returns(new List<CategoryItem>());
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category>() { category });

            // Act & Assert
            var result = _itemProcessor.GetListItems(1, category.Id);
            Assert.Empty(result);
        }

        [Fact]
        public void GetListItems_WhenValidRequest_ShouldItemsList()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category" };

            var item_1 = new Item { Id = 1, Name = "TestItem1" };
            var item_2 = new Item { Id = 2, Name = "TestItem2" };
            var item_3 = new Item { Id = 3, Name = "TestItem3" };
            var item_4 = new Item { Id = 4, Name = "TestItem4" };
            var item_5 = new Item { Id = 5, Name = "TestItem5" };
            var item_6 = new Item { Id = 6, Name = "TestItem6" };
            var item_7 = new Item { Id = 7, Name = "TestItem7" };
            var item_8 = new Item { Id = 8, Name = "TestItem8" };
            var item_9 = new Item { Id = 9, Name = "TestItem9" };
            var item_10 = new Item { Id = 10, Name = "TestItem10" };
            var item_11 = new Item { Id = 11, Name = "TestItem11", Author = "Author" };
            var item_12 = new Item { Id = 12, Name = "TestItem12" };

            var categoryItem_1 = new CategoryItem { ItemId = item_1.Id, CategoryId = category.Id };
            var categoryItem_2 = new CategoryItem { ItemId = item_2.Id, CategoryId = category.Id };
            var categoryItem_3 = new CategoryItem { ItemId = item_3.Id, CategoryId = category.Id };
            var categoryItem_4 = new CategoryItem { ItemId = item_4.Id, CategoryId = category.Id };
            var categoryItem_5 = new CategoryItem { ItemId = item_5.Id, CategoryId = category.Id };
            var categoryItem_6 = new CategoryItem { ItemId = item_6.Id, CategoryId = category.Id };
            var categoryItem_7 = new CategoryItem { ItemId = item_7.Id, CategoryId = category.Id };
            var categoryItem_8 = new CategoryItem { ItemId = item_8.Id, CategoryId = category.Id };
            var categoryItem_9 = new CategoryItem { ItemId = item_9.Id, CategoryId = category.Id };
            var categoryItem_10 = new CategoryItem { ItemId = item_10.Id, CategoryId = category.Id };
            var categoryItem_11 = new CategoryItem { ItemId = item_11.Id, CategoryId = category.Id };
            var categoryItem_12 = new CategoryItem { ItemId = item_12.Id, CategoryId = category.Id };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });
            _mockContext.Setup(c => c.GetCollection<CategoryItem>())
                .Returns(new List<CategoryItem> { categoryItem_1, categoryItem_2, categoryItem_3, categoryItem_4, categoryItem_5,
                               categoryItem_6, categoryItem_7, categoryItem_8, categoryItem_9, categoryItem_10, categoryItem_11, categoryItem_12});
            _mockContext.Setup(c => c.GetCollection<Item>())
                .Returns(new List<Item> { item_1, item_2, item_3, item_4, item_5, item_6, item_7, item_8, item_9, item_10, item_11, item_12 });

            // Act
            var result = _itemProcessor.GetListItems(2, null);

            // Assert
            var expectedResult = new List<Response>
            {
                new Response
                {
                    ItemId = 11,
                    ItemName = "TestItem11",
                    Author = "Author",
                    CategoryName = "Category"
                },
                new Response
                {
                    ItemId = 12,
                    ItemName = "TestItem12",
                    Author = null,
                    CategoryName = "Category"
                }
            };
            Assert.Collection(result,
            item =>
            {
                Assert.Equal(expectedResult[0].ItemId, item.ItemId);
                Assert.Equal(expectedResult[0].ItemName, item.ItemName);
                Assert.Equal(expectedResult[0].Author, item.Author);
                Assert.Equal(expectedResult[0].CategoryName, item.CategoryName);
            },
            item =>
            {
                Assert.Equal(expectedResult[1].ItemId, item.ItemId);
                Assert.Equal(expectedResult[1].ItemName, item.ItemName);
                Assert.Null(item.Author);
                Assert.Equal(expectedResult[1].CategoryName, item.CategoryName);
            });
        }
        
        [Fact]
        public void GetListItems_WhenValidRequestAndProceedCategoryId_ShouldItemsList()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category" };
            var category_2 = new Category { Id = 2, Name = "Category2" };

            var item_1 = new Item { Id = 1, Name = "TestItem1" };
            var item_2 = new Item { Id = 2, Name = "TestItem2" };
            var item_3 = new Item { Id = 3, Name = "TestItem3" };
            var item_4 = new Item { Id = 4, Name = "TestItem4" };
            var item_5 = new Item { Id = 5, Name = "TestItem5" };
            var item_6 = new Item { Id = 6, Name = "TestItem6" };
            var item_7 = new Item { Id = 7, Name = "TestItem7" };
            var item_8 = new Item { Id = 8, Name = "TestItem8" };
            var item_9 = new Item { Id = 9, Name = "TestItem9" };
            var item_10 = new Item { Id = 10, Name = "TestItem10" };
            var item_11 = new Item { Id = 11, Name = "TestItem11", Author = "Author" };
            var item_12 = new Item { Id = 12, Name = "TestItem12" };

            var categoryItem_1 = new CategoryItem { ItemId = item_1.Id, CategoryId = category.Id };
            var categoryItem_2 = new CategoryItem { ItemId = item_2.Id, CategoryId = category.Id };
            var categoryItem_3 = new CategoryItem { ItemId = item_3.Id, CategoryId = category_2.Id };
            var categoryItem_4 = new CategoryItem { ItemId = item_4.Id, CategoryId = category.Id };
            var categoryItem_5 = new CategoryItem { ItemId = item_5.Id, CategoryId = category.Id };
            var categoryItem_6 = new CategoryItem { ItemId = item_6.Id, CategoryId = category_2.Id };
            var categoryItem_7 = new CategoryItem { ItemId = item_7.Id, CategoryId = category.Id };
            var categoryItem_8 = new CategoryItem { ItemId = item_8.Id, CategoryId = category.Id };
            var categoryItem_9 = new CategoryItem { ItemId = item_9.Id, CategoryId = category.Id };
            var categoryItem_10 = new CategoryItem { ItemId = item_10.Id, CategoryId = category.Id };
            var categoryItem_11 = new CategoryItem { ItemId = item_11.Id, CategoryId = category.Id };
            var categoryItem_12 = new CategoryItem { ItemId = item_12.Id, CategoryId = category.Id };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category, category_2 });
            _mockContext.Setup(c => c.GetCollection<CategoryItem>())
                .Returns(new List<CategoryItem> { categoryItem_1, categoryItem_2, categoryItem_3, categoryItem_4, categoryItem_5,
                               categoryItem_6, categoryItem_7, categoryItem_8, categoryItem_9, categoryItem_10, categoryItem_11, categoryItem_12});
            _mockContext.Setup(c => c.GetCollection<Item>())
                .Returns(new List<Item> { item_1, item_2, item_3, item_4, item_5, item_6, item_7, item_8, item_9, item_10, item_11, item_12 });

            // Act
            var result = _itemProcessor.GetListItems(1, category_2.Id);

            // Assert
            var expectedResult = new List<Response>
            {
                new Response
                {
                    ItemId = 3,
                    ItemName = "TestItem3",
                    Author = null,
                    CategoryName = "Category2"
                },
                new Response
                {
                    ItemId = 6,
                    ItemName = "TestItem6",
                    Author = null,
                    CategoryName = "Category2"
                }
            };
            Assert.Collection(result,
            item =>
            {
                Assert.Equal(expectedResult[0].ItemId, item.ItemId);
                Assert.Equal(expectedResult[0].ItemName, item.ItemName);
                Assert.Equal(expectedResult[0].Author, item.Author);
                Assert.Equal(expectedResult[0].CategoryName, item.CategoryName);
            },
            item =>
            {
                Assert.Equal(expectedResult[1].ItemId, item.ItemId);
                Assert.Equal(expectedResult[1].ItemName, item.ItemName);
                Assert.Null(item.Author);
                Assert.Equal(expectedResult[1].CategoryName, item.CategoryName);
            });
        }

        [Fact]
        public void UpdateItem_WhenItemExists_ShouldUpdateItem()
        {
            // Arrange
            var itemId = 1;
            var item = new Item { Id = itemId, Name = "OldName", Author = "OldAuthor" };
            var itemRequest = new ItemRequest { Name = "NewName", Author = "NewAuthor" };

            _mockContext.Setup(c => c.GetCollection<Item>())
                        .Returns(new List<Item> { item });

            // Act
            _itemProcessor.UpdateItem(itemId, itemRequest);

            // Assert
            _mockContext.Verify(c => c.Update<Item>(It.Is<Item>(i => i.Id == itemId && i.Name == "NewName" && i.Author == "NewAuthor")), Times.Once);
        }

        [Fact]
        public void UpdateItem_WhenItemNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var itemId = 1;
            var itemRequest = new ItemRequest { Name = "NewName", Author = "NewAuthor" };

            _mockContext.Setup(c => c.GetCollection<Item>())
                        .Returns(new List<Item>());

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _itemProcessor.UpdateItem(itemId, itemRequest));
        }

        [Fact]
        public void UpdateItem_WhenRequestIsInvalid_ShouldArgumentException()
        {
            // Arrange
            var itemId = 1;
            var itemRequest = new ItemRequest { Name = null, Author = "NewAuthor" };

            _mockContext.Setup(c => c.GetCollection<Item>())
                        .Returns(new List<Item>());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _itemProcessor.UpdateItem(itemId, itemRequest));
        }
    }
}