using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("CategoryParameters")]
    public class CategoryParameter
    {
        public int ParameterID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string ValueType { get; set; }

    }
}
