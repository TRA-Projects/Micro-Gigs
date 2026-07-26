using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories
{
    public class UsersRepo
    {

        private MicroGigsContext _context;


        // Constructor Injection: DbContext from DI Container
        public UsersRepo(MicroGigsContext context)
        {
            _context = context;
        }

        // ============================
        // READ: Get All Users
        // ============================
        public List<Users> GetAll()
        {
            return _context.Users.ToList();
        }


        // ============================
        // READ: Get User By ID
        // ============================
        public Users? GetById(int id)
        {
            return _context.Users
                .FirstOrDefault(u => u.UserId == id);
        }


        // ============================
        // READ: Get User By Email
        // ============================
        public Users? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }


        // ============================
        // CREATE: Add New User
        // ============================
        public void Add(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        // ============================
        // UPDATE: Update Existing User
        // ============================
        public void Update(Users user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }


        // ============================
        // DELETE: Delete User
        // ============================
        public void Delete(Users user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
