using Micro_Gigs.DTOs;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GigCategoriesController : ControllerBase
    {
        private GigCategoriesServices categoriesServices;

        public GigCategoriesController(GigCategoriesServices _categoriesServices)
        {
            categoriesServices = _categoriesServices;
        }

        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var category = categoriesServices.GetAllCategories();
            return Ok (category);
        }


        [HttpGet("GetById")]
        public ActionResult GetById([FromQuery] int id)
        {
            var category = categoriesServices.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }


        [HttpPost("Create")]
        [Authorize]
        public ActionResult Create([FromBody] CreateCategoryDto dto)
        {
            var category = categoriesServices.CreateCategory(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.GigCategoryId }, category);
        }

        [HttpPut("Update")]
        [Authorize]
        public ActionResult Update([FromQuery] int id , [FromBody] CreateCategoryDto dto)
        {
            var success = categoriesServices.UpdateCategory(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }


        [HttpDelete("Delete")]
        [Authorize]
        public ActionResult Delete([FromQuery] int id)
        {
            var success = categoriesServices.DeleteCategory(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
