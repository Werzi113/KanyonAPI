using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.Categories;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private MyContext _context = new MyContext();

        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(_context.Categories.ToArray());
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryByID(int id)
        {
            Category category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound("Category doesn't exist");
            }

            return Ok(category);
        }

        [HttpGet("Parent")]
        public IActionResult GetCategoriesByParent([FromQuery]int? id)
        {
            if (id != null && !_context.Categories.Any(Category => Category.CategoryID == id))
            {
                return NotFound("Parent doesn't exist");
            }

            return Ok(_context.Categories.Where(Category => Category.ParentID == id).ToArray());
        }

        [HttpGet("Path/{id}")]
        public IActionResult GetCategoryPath(int id)
        {
            Category dbCategory = _context.Categories.Find(id);

            if (dbCategory == null)
            {
                return NotFound("Category doesn't exist");
            }

            return Ok(_context.Categories.Where(Category => Category.Left < dbCategory.Left && Category.Right > dbCategory.Right).OrderBy(Category => Category.Left));
        }

        [HttpPost]
        //[SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddCategory(CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Categories.Any(Category => Category.CategoryID == categoryDTO.ParentID) && categoryDTO.ParentID != null)
            {
                return NotFound("Parent doesn't exist");
            }

            Category dbCategory = categoryDTO.CreateCategory();

            if (dbCategory.ParentID == null)
            {
                dbCategory.Left = _context.Categories.Max(Category => Category.Right) + 1;
            }
            else
            {
                Category parent = _context.Categories.Find(dbCategory.ParentID);
                dbCategory.Left = parent.Left + 1;
            }
            dbCategory.Right = dbCategory.Left + 1;

            foreach (var item in _context.Categories.Where(Category => Category.Right >= dbCategory.Left))
            {
                item.Right += 2;
            }

            foreach (var item in _context.Categories.Where(Category => Category.Left >= dbCategory.Left))
            {
                item.Left += 2;
            }

            _context.Categories.Add(dbCategory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryByID), new { id = dbCategory.CategoryID }, dbCategory);
        }

        [HttpPut("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult EditCategory(int id, EditCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            //if (!_context.Categories.Any(Category => Category.CategoryID == categoryDTO.ParentID) && categoryDTO.ParentID != null)
            //{
            //    return NotFound("Parent doesn't exist");
            //}
            //if (id == categoryDTO.ParentID)
            //{
            //    return StatusCode(403, "Category can't be a parent of itself");
            //}

            Category dbCategory = _context.Categories.Find(id);

            if (dbCategory == null)
            {
                return NotFound("Category doesn't exist");
            }

            //dbCategory.ParentID = categoryDTO.ParentID;
            dbCategory.Name = categoryDTO.Name;
            dbCategory.Description = categoryDTO.Description;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        //[SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteCategory(int id)
        {
            Category dbCategory = _context.Categories.Find(id);

            if (dbCategory == null)
            {
                return NotFound("Category doesn't exist");
            }
            if (_context.Products.Any(Product => Product.CategoryId == id))
            {
                return StatusCode(403, "Category has products in it");
            }

            int width = dbCategory.Right - dbCategory.Left + 1;

            _context.Categories.Remove(dbCategory);
            _context.SaveChanges();

            foreach (var item in _context.Categories.Where(Category => Category.Left > dbCategory.Left))
            {
                item.Left -= width;
            }
            foreach (var item in _context.Categories.Where(Category => Category.Right > dbCategory.Right))
            {
                item.Right -= width;
            }

            _context.SaveChanges();

            return NoContent();
        }
    }
}