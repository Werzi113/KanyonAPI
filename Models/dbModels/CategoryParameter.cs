using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.dbModels
{
    [Table("CategoryParameters")]
    public class CategoryParameter
    {
        [Key]
        public int ParameterID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string ValueType { get; set; }
    }
}
