using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.Categories;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private MyContext _context = new MyContext();
        private CategoryService _categoryService = new CategoryService();

        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(_context.Categories.ToArray());
        }

        [HttpGet("Previews")]
        public IActionResult GetCategoryPreviews()
        {
            var res = _context.Categories.ToArray()
                .GroupJoin(_context.Categories, cat => cat.ParentID, parent => parent.CategoryID, (cat, parent) => new { category = cat, parent = parent })
                .SelectMany(item => item.parent.DefaultIfEmpty(),
                (x, y) => new { category = x.category, parent = y })
                .Select(item => new CategoryPreviewDTO()
                {
                    Name = item.category.Name,
                    CategoryID = item.category.CategoryID,
                    Parent = item.parent?.Name,
                    ParentID = item.category.ParentID
                });

            return Ok(res);
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

        //Gets DIRECT children of category if id == null returns categories without a parent
        [HttpGet("Parent")]
        public IActionResult GetCategoriesByParent([FromQuery]int? id)
        {
            if (id != null && !_context.Categories.Any(Category => Category.CategoryID == id))
            {
                return NotFound("Parent doesn't exist");
            }

            return Ok(_context.Categories.Where(Category => Category.ParentID == id).ToArray());
        }

        //Gets ALL children of a category
        [HttpGet("All/{id}")]
        public IActionResult GetAllChildrenByID(int id)
        {
            if (_context.Categories.Find(id) == null)
            {
                return NotFound("Category doesn't exist");
            }

            var res = _categoryService.getAllChildrenOfCategory(id);

            return Ok(res);
        }

        //Gets the path leading to a category the category is the final element of the returned array
        [HttpGet("Path/{id}")]
        public IActionResult GetPath(int id)
        {
            if (_context.Categories.Find(id) == null)
            {
                return NotFound("Category doesn't exist");
            }

            var res = _context.Categories.FromSqlInterpolated($@"
                WITH recursive Nodes as (
	            select *, 0 as position from Categories where CategoryID = {id}
                UNION all
                select cat.*, n.position + 1 as position from Categories as cat 
                INNER JOIN
	            Nodes as n
                WHERE cat.CategoryID = n.ParentID
                )
                select * from Nodes ORDER BY position desc").ToArray();

            return Ok(res);
        }

        [HttpPost]
        [SecuredRight(UserRightType.Admin_Add)]
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
            if (!_context.Categories.Any(Category => Category.CategoryID == categoryDTO.ParentID) && categoryDTO.ParentID != null)
            {
                return NotFound("Parent doesn't exist");
            }
            if (id == categoryDTO.ParentID)
            {
                return StatusCode(403, "Category can't be a parent of itself");
            }

            Category dbCategory = _context.Categories.Find(id);

            if (dbCategory == null)
            {
                return NotFound("Category doesn't exist");
            }

            var children = _categoryService.getAllChildrenOfCategory(id);
            if (children.Any(item => item.CategoryID == categoryDTO.ParentID))
            {
                return Forbid("Category can't have its child as a parent");
            }

            dbCategory.ParentID = categoryDTO.ParentID;
            dbCategory.Name = categoryDTO.Name;
            dbCategory.Description = categoryDTO.Description;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
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

            _context.Categories.Remove(dbCategory);
            _context.SaveChanges();

            return NoContent();
        }


    }
}