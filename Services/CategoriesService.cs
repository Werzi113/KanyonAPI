using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class CategoriesService
    {
        private MyContext _context = new MyContext();

        public Category[] GetCategoryTree(int ParentID)
        {
            Category parent = _context.Categories.Find(ParentID);
            if (parent == null)
            {
                throw new Exception("Parent doesn't exist");
            }

            return _context.Categories.Where(Category => Category.Left < parent.Right && Category.Right > parent.Left).OrderBy(Category => Category.Left).ToArray();
        }
    }
}
