using Micro_Gigs.DTOs;
using Micro_Gigs.Models;
using Micro_Gigs.Repositories;

namespace Micro_Gigs.Services
{
    public class UsersServices
    {
        private UsersRepo usersRepo;
        private AuthService authService;

        // Constructor Injection:
        // Inject required services using Dependency Injection
        // Used it to get the instance of UsersRepo and AuthService from the DI container
        public UsersServices(UsersRepo _usersRepo, AuthService _authService)
        {
            usersRepo = _usersRepo;
            authService = _authService;
        }



        // =====================================================
        // Register a new user account
        // =====================================================
        public UsersInputDTOs.UsersResponseDto? Register(UsersInputDTOs.RegisterUserDto dto)
        {
            // Check if email already exists in the database
            if (usersRepo.GetByEmail(dto.Email) != null)
                return null;


            // Create new user entity
            Users user = new Users
            {
                UserName = dto.UserName,
                Email = dto.Email,
                UserType = dto.UserType,

                // Hash password before storing it securely
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };


            // Save user to database
            usersRepo.Add(user);



            // Map User Entity to Response DTO
            UsersInputDTOs.UsersResponseDto response = new UsersInputDTOs.UsersResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                UserType = user.UserType,
                RegistrationDate = user.RegistrationDate
            };


            return response;
        }





        // =====================================================
        // Authenticate user and generate JWT token
        // =====================================================
        public UsersInputDTOs.LoginResponseDto? Login(UsersInputDTOs.LoginUserDto dto)
        {
            // Find user by email
            Users? user = usersRepo.GetByEmail(dto.Email);


            if (user == null)
                return null;



            // Verify entered password with stored password hash
            bool passwordValid =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);



            if (!passwordValid)
                return null;



            // Generate JWT token after successful login
            string token = authService.GenerateToken(user);



            // Return login response
            UsersInputDTOs.LoginResponseDto response =
                new UsersInputDTOs.LoginResponseDto
                {
                    Token = token,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserType = user.UserType
                };


            return response;
        }





        // =====================================================
        // Get user information by ID
        // =====================================================
        public UsersInputDTOs.UsersResponseDto? GetById(int id)
        {
            // Retrieve user by primary key
            Users? user = usersRepo.GetById(id);


            if (user == null)
                return null;



            // Convert Entity to Response DTO
            return new UsersInputDTOs.UsersResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                UserType = user.UserType,
                RegistrationDate = user.RegistrationDate
            };
        }





        // =====================================================
        // Update existing user information
        // =====================================================
        public UsersInputDTOs.UsersResponseDto? Update(
            int id,
            UsersInputDTOs.UpdateUserDto dto)
        {
            // Find existing user
            Users? user = usersRepo.GetById(id);


            if (user == null)
                return null;



            // Update user information
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.UserType = dto.UserType;



            // Save changes
            usersRepo.Update(user);



            // Return updated user data
            return new UsersInputDTOs.UsersResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                UserType = user.UserType,
                RegistrationDate = user.RegistrationDate
            };
        }





        // =====================================================
        // Delete user account
        // =====================================================
        public bool Delete(int id)
        {
            // Find user before deletion
            Users? user = usersRepo.GetById(id);


            if (user == null)
                return false;



            // Remove user from database
            usersRepo.Delete(user);


            return true;
        }

        // =====================================================
        // Get all users information
        // =====================================================
        public List<UsersInputDTOs.UsersResponseDto> GetAll()
        {
            // Retrieve all users from database
            List<Users> users = usersRepo.GetAll();


            // Convert Entities to Response DTOs
            return users.Select(user => new UsersInputDTOs.UsersResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                UserType = user.UserType,
                RegistrationDate = user.RegistrationDate

            }).ToList();
        }
    }

}

