using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    public class GigCategoriesServices
    {
        private GigCategoriesRepo categoriesRepo;

        public GigCategoriesServices(GigCategoriesRepo _categoriesRepo) 
        {
            categoriesRepo = _categoriesRepo;
        }

        public List<CategoryResponseDto> GetAllCategories() 
        {
            var categories = categoriesRepo.GetAll();
            return categories.Select(c => new CategoryResponseDto
            {
                GigCategoryId = c.GigCategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            }).ToList();
        }

        public CategoryResponseDto? GetCategoryById(int id)
        {
            var category = categoriesRepo.GetById(id);
            if (category == null) return null;


            return new CategoryResponseDto
            {
                GigCategoryId = category.GigCategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        } 

        public CategoryResponseDto CreateCategory(CreateCategoryDto dto)
        {
            var category = new GigCategories
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            categoriesRepo.Add(category);

            return new CategoryResponseDto
            {
                GigCategoryId = category.GigCategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public bool UpdateCategory(int id, CreateCategoryDto dto)
        {
            var category = categoriesRepo.GetById(id);
            if (category == null) return false;

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;

            categoriesRepo.Update(category);
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var category = categoriesRepo.GetById(id);
            if (category == null) return false;

            categoriesRepo.Delete(category);
            return true;
        }
    }
}
