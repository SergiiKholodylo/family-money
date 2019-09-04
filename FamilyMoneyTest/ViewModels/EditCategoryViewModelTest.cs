using System.ComponentModel;
using System.Linq;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class EditCategoryViewModelTest
    {
        private string _fieldName;

        [TestMethod]
        public void TestConstructor()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());


            var viewModel = new EditCategoryViewModel(storage, null, null);


            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.Name));
            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.Description));
            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.ErrorString));
            Assert.IsNull(viewModel.ParentCategory);
            Assert.IsNotNull(viewModel.Categories);
        }

        [TestMethod]
        public void TestConstructorWithExistCategory()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var category = storage.CreateCategory("Test Category", "Description", 0, null);


            var viewModel = new EditCategoryViewModel(storage, category, null);


            Assert.AreEqual(category.Name, viewModel.Name);
            Assert.AreEqual(category.Description, viewModel.Description);
            Assert.IsNull(viewModel.ParentCategory);
            Assert.IsNotNull(viewModel.Categories);
        }

        [TestMethod]
        public void TestConstructorWithNewAndParentCategory()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);


            var viewModel = new EditCategoryViewModel(storage, null, parentCategory);


            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.Name));
            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.Description));
            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.ErrorString));
            Assert.AreEqual(parentCategory, viewModel.ParentCategory);
            Assert.IsNotNull(viewModel.Categories);
        }

        [TestMethod]
        public void TestConstructorWithExistAndParentCategory1()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);
            var category = storage.CreateCategory("Test Category", "Description", 0, parentCategory);


            var viewModel = new EditCategoryViewModel(storage, category, null);


            Assert.AreEqual(category.Name, viewModel.Name);
            Assert.AreEqual(category.Description, viewModel.Description);
            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.ErrorString));
            Assert.AreEqual(parentCategory, viewModel.ParentCategory);
            Assert.IsNotNull(viewModel.Categories);
        }

        [TestMethod]
        public void TestConstructorWithExistAndParentCategory2()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);
            var category = storage.CreateCategory("Test Category", "Description", 0, parentCategory);


            var viewModel = new EditCategoryViewModel(storage, category, parentCategory);


            Assert.AreEqual(category.Name, viewModel.Name);
            Assert.AreEqual(category.Description, viewModel.Description);
            Assert.IsTrue(string.IsNullOrWhiteSpace(viewModel.ErrorString));
            Assert.AreEqual(parentCategory, viewModel.ParentCategory);
            Assert.IsNotNull(viewModel.Categories);
        }

        [TestMethod]
        public void EventNameTest()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var viewModel = new EditCategoryViewModel(storage,null,null);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Name = "Name";
            Assert.AreEqual("Name", _fieldName);
        }

        [TestMethod]
        public void EventDescriptionTest()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var viewModel = new EditCategoryViewModel(storage, null, null);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Description = "Description";
            Assert.AreEqual("Description", _fieldName);
        }

        [TestMethod]
        public void EventParentCategoryTest()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);
            var viewModel = new EditCategoryViewModel(storage, null, null);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.ParentCategory = parentCategory;
            Assert.AreEqual("ParentCategory", _fieldName);
        }

        [TestMethod]
        public void CreateNewCategoryTest()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);
            var viewModel = new EditCategoryViewModel(storage, null, parentCategory)
            {
                Name = "Name",
                Description = "Description"
            };

            viewModel.CreateNewCategory();
            var savedCategory = storage.GetAllCategories().FirstOrDefault(x => x.Name.Equals("Name"));

            Assert.IsNotNull(savedCategory);
            Assert.AreEqual(parentCategory, viewModel.ParentCategory);
            Assert.AreEqual("Name",savedCategory.Name);
            Assert.AreEqual("Description", savedCategory.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void UpdateCategoryExceptionTest()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);
            var viewModel = new EditCategoryViewModel(storage, null, parentCategory)
            {
                Name = "Name",
                Description = "Description"
            };

            viewModel.UpdateCategory();
        }

        [TestMethod]
        public void UpdateCategoryTest()
        {
            var storage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var parentCategory = storage.CreateCategory("Parent Category", "ParentDescription", 0, null);
            var newParentCategory = storage.CreateCategory("New Parent Category", "New ParentDescription", 0, null);
            var category = storage.CreateCategory("Test Category", "Description", 0, parentCategory);
            var viewModel = new EditCategoryViewModel(storage, category, parentCategory)
            {
                Name = "New Name",
                Description = "New Description",
                ParentCategory = newParentCategory
            };

            viewModel.UpdateCategory();
            var savedCategory = storage.GetAllCategories().FirstOrDefault(x => x.Name.Equals("New Name"));

            Assert.IsNotNull(savedCategory);
            Assert.AreEqual(newParentCategory, viewModel.ParentCategory);
            Assert.AreEqual("New Name", savedCategory.Name);
            Assert.AreEqual("New Description", savedCategory.Description);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _fieldName = e.PropertyName;
        }
    }
}
