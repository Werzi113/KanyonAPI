using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("CategoryParametersValues")]
    public class CategoryParameterValue
    {
        public int ProductID { get; set; }
        public int ParameterID { get; set; }
        public string Value { get; set; }

    }
}
