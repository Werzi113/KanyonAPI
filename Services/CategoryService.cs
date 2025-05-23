using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class CategoryService
    {
        private MyContext _context = new MyContext();

        public Category[] getAllChildrenOfCategory(int id)
        {
            return _context.Categories.FromSqlInterpolated($@"
                WITH recursive Nodes as (
	            select * from Categories where CategoryID = {id}
                UNION all
                select cat.* from Categories as cat 
                INNER JOIN
	            Nodes as n
                WHERE cat.parentID = n.CategoryID
                )
                select * from Nodes").ToArray();
        }
    }
}
