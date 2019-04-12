using DataLayer;
using DbEntities.Models;
using GPGDesign.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPGDesign.Components
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly IRepository<Category> categoryRepository;

        public CategoryListViewComponent(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = GetCategories();

            return await Task.FromResult((IViewComponentResult)View("_CategoriesPartial", model));
        }

        private IEnumerable<CategoryViewModel> GetCategories()
        {
            var result = this.categoryRepository.All()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    EnName = c.EnName
                })
                .ToList();

            return result;
        }
    }
}
