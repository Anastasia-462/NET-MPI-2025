using Moq;
using REST_Task6_CatalogService.Context;
using REST_Task6_CatalogService.Controllers.Models;
using REST_Task6_CatalogService.Models;
using REST_Task6_CatalogService.Services.Items.Models;
using REST_Task6_CatalogService.Services.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REST_Task6_CatalogService.Services.Categories;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace REST_Task6_CatalogService.Test
{
    public class CategoryProcessorTests
    {
        private readonly Mock<ILiteContext> _mockContext;
        private readonly ICategoryProcessor _categoryProcessor;

        public CategoryProcessorTests()
        {
            _mockContext = new Mock<ILiteContext>();
            _categoryProcessor = new CategoryProcessor(_mockContext.Object);
        }

        [Fact]
        public void AddCategory_WhenValidRequest_ShouldAddCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category" };
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });

            var categoryRequest = new CategoryRequest { Name = "Category1" };

            // Act
            _categoryProcessor.AddCategory(categoryRequest);

            // Assert
            _mockContext.Verify(c => c.Insert<Category>(It.Is<Category>(i => i.Name == "Category1")), Times.Once);
        }

        [Fact]
        public void AddCategory_WhenTheSameCategoryAlreadyExist_ShouldThrowArgumentException()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category1" };
            var categoryRequest = new CategoryRequest { Name = "Category1" };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _categoryProcessor.AddCategory(categoryRequest));
        }

        [Fact]
        public void AddCategory_WhenInvalidRequest_ShouldThrowArgumentException()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = null };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _categoryProcessor.AddCategory(categoryRequest));
        }

        [Fact]
        public void DeleteCategory_WhenCategoryExists_ShouldDeleteCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category" };
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });
            _mockContext.Setup(c => c.GetCollection<CategoryItem>()).Returns(new List<CategoryItem>());
            _mockContext.Setup(c => c.GetCollection<Item>()).Returns(new List<Item>());

            // Act
            _categoryProcessor.DeleteCategory(category.Id);

            // Assert
            _mockContext.Verify(c => c.Delete<Category>(category.Id), Times.Once);
        }

        [Fact]
        public void DeleteCategory_WhenCategoryExistsAndItemsForThisCategoryExist_ShouldDeleteCategoryAndItems()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category" };
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "TestItem1" },
                new Item { Id = 2, Name = "TestItem2" },
                new Item { Id = 3, Name = "TestItem3" },
                new Item { Id = 4, Name = "TestItem4" },
                new Item { Id = 5, Name = "TestItem5" },
            };
            var categoryItems = new List<CategoryItem>
            {
                new CategoryItem { ItemId = 1, CategoryId = category.Id },
                new CategoryItem { ItemId = 2, CategoryId = category.Id },
                new CategoryItem { ItemId = 3, CategoryId = category.Id },
                new CategoryItem { ItemId = 4, CategoryId = category.Id },
                new CategoryItem { ItemId = 5, CategoryId = category.Id },
            };
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });
            _mockContext.Setup(c => c.GetCollection<CategoryItem>()).Returns(categoryItems);
            _mockContext.Setup(c => c.GetCollection<Item>()).Returns(items);

            // Act
            _categoryProcessor.DeleteCategory(category.Id);

            // Assert
            _mockContext.Verify(c => c.DeleteMany<CategoryItem>(It.IsAny<Expression<Func<CategoryItem, bool>>>()), Times.Once);
            _mockContext.Verify(c => c.DeleteMany<Item>(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
            _mockContext.Verify(c => c.Delete<Category>(category.Id), Times.Once);
        }

        [Fact]
        public void DeleteCategory_WhenCategoryNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" },
                new Category { Id = 3, Name = "Category3" },
                new Category { Id = 4, Name = "Category4" },
                new Category { Id = 5, Name = "Category5" },
            };
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category>(categories));

            // Act
            var result = _categoryProcessor.GetListCategories();

            // Assert
            var expectedResult = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" },
                new Category { Id = 3, Name = "Category3" },
                new Category { Id = 4, Name = "Category4" },
                new Category { Id = 5, Name = "Category5" },
            };
            Assert.Collection(result,
            category =>
            {
                Assert.Equal(expectedResult[0].Id, category.Id);
                Assert.Equal(expectedResult[0].Name, category.Name);
            },
            category =>
            {
                Assert.Equal(expectedResult[1].Id, category.Id);
                Assert.Equal(expectedResult[1].Name, category.Name);
            },
            category =>
            {
                Assert.Equal(expectedResult[2].Id, category.Id);
                Assert.Equal(expectedResult[2].Name, category.Name);
            },
            category =>
            {
                Assert.Equal(expectedResult[3].Id, category.Id);
                Assert.Equal(expectedResult[3].Name, category.Name);
            },
            category =>
            {
                Assert.Equal(expectedResult[4].Id, category.Id);
                Assert.Equal(expectedResult[4].Name, category.Name);
            });
        }

        [Fact]
        public void GetListCategorys_WhenCategoriesDoNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category>());

            // Act
            var result = _categoryProcessor.GetListCategories();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void UpdateCategory_WhenCategoryExists_ShouldUpdateCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "OldName" };
            var categoryRequest = new CategoryRequest { Name = "NewName" };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category> { category });

            // Act
            _categoryProcessor.UpdateCategory(category.Id, categoryRequest);

            // Assert
            _mockContext.Verify(c => c.Update<Category>(It.Is<Category>(i => i.Id == category.Id && i.Name == "NewName")), Times.Once);
        }

        [Fact]
        public void UpdateCategory_WhenCategoryNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = "NewName" };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category>());

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _categoryProcessor.UpdateCategory(1, categoryRequest));
        }

        [Fact]
        public void UpdateCategory_WhenRequestIsInvalid_ShouldArgumentException()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Name = null };

            _mockContext.Setup(c => c.GetCollection<Category>()).Returns(new List<Category>());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _categoryProcessor.UpdateCategory(1, categoryRequest));
        }
    }
}
