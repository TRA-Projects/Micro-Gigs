using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories
{
    public class GigCategoriesRepo
    {
        private MicroGigsContext context;

        public GigCategoriesRepo(MicroGigsContext _context)
        {
            context = _context;
        }

        public List<GigCategories> GetAll()
        {
            return context.Categories.ToList();
        }

        public void Add(GigCategories category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Update(GigCategories category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void Delete(GigCategories category)
        {
            context.Categories.Remove(category);
            context.SaveChanges() ;
        }
    }

}
