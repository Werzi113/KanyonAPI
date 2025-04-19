using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("CategoryParametersValues")]
    [PrimaryKey(nameof(ProductID), nameof(ParameterID))]
    public class CategoryParameterValue
    {
        public int ProductID { get; set; }
        public int ParameterID { get; set; }
        public string Value { get; set; }

    }
}
