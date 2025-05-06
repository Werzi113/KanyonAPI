using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers.Attributes;
using WebApplication1.Models;
using WebApplication1.Enums;
using WebApplication1.Models.DTO.DiscountCodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController : ControllerBase
    {

        private MyContext _context = new MyContext();

        [HttpGet("{id}")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetCodeByID(int id)
        {
            DiscountCode code = _context.DiscountCodes.Find(id);

            if (code == null)
            {
                return NotFound("Code doesn't exist");
            }

            return Ok(code);
        }

        [HttpGet("Valid/{code}")]
        public IActionResult GetValidCodeByCode(string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DiscountCode dbCode = _context.DiscountCodes.FirstOrDefault(Code => Code.Code == code && Code.ExpiryDate > DateTime.Now);
            
            if (dbCode == null)
            {
                return NotFound("Code doesn't exist or is expired");
            }

            return Ok(dbCode);  
        }

        [HttpGet("Codes")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetAllCodes()
        {
            return Ok(_context.DiscountCodes.ToArray());
        }

        [HttpGet("ValidCodes")]
        [SecuredRight(UserRightType.Admin_Read)]
        public IActionResult GetValidCodes()
        {
            return Ok(_context.DiscountCodes.Where(Code => Code.ExpiryDate > DateTime.Now).ToArray());
        }

        [HttpPost]
        [SecuredRight(UserRightType.Admin_Add)]
        public IActionResult AddCode(CreateDiscountCodeDTO codeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.DiscountCodes.Any(Code => Code.Code == codeDTO.Code &&  codeDTO.ExpiryDate > DateTime.Now))
            {
                return StatusCode(403, "There is already an unxpired code with this value");
            }

            DiscountCode code = codeDTO.CreateDiscountCode();

            _context.Add(code);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCodeByID), new { id = code.CodeID }, code);
        }

        [HttpPut("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult EditCode(int id, EditDiscountCodeDTO codeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DiscountCode dbCode = _context.DiscountCodes.Find(id);

            if (dbCode == null)
            {
                return NotFound("Code doesn't exist");
            }
            if (_context.DiscountCodes.Any(Code => Code.Code == codeDTO.Code && codeDTO.ExpiryDate > DateTime.Now && Code.CodeID != dbCode.CodeID))
            {
                return StatusCode(403, "There is already an unxpired code with this value");
            }

            dbCode.ExpiryDate = codeDTO.ExpiryDate;
            dbCode.Discount = codeDTO.Discount;
            dbCode.Code = codeDTO.Code;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SecuredRight(UserRightType.Admin_Edit)]
        public IActionResult DeleteCode(int id)
        {
            DiscountCode dbCode = _context.DiscountCodes.Find(id);
            
            if (dbCode == null)
            {
                return NotFound("Code doesn't exist");
            }

            _context.DiscountCodes.Remove(dbCode);
            _context.SaveChanges();

            return NoContent();
        }
    }
}