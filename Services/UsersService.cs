using MvcWhatsUp.Models;
using MvcWhatsUp.Repositories.Interfaces;
using MvcWhatsUp.Services.Interfaces;
using System.Security.Cryptography;

namespace MvcWhatsUp.Services
{
    public class UsersService : IUsersService
    {
        private IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public void AddUser(User user)
        {
            if (_usersRepository.EmailAddressExists(user.EmailAddress))
            {
                throw new Exception("Email address already exists.");
            }

            User copyUser = new User(user);
            copyUser.Password = HashPassword(user.Password);

            
            _usersRepository.AddUser(copyUser);

            if(user.UserID != copyUser.UserID)
            {
                user.UserID = copyUser.UserID;
            }
        }

        public void DeleteUser(User user)
        {
            _usersRepository.DeleteUser(user);
        }

        public List<User> GetAllUsers()
        {
            return _usersRepository.GetAllUsers();
        }

        public User? GetByLoginCredentials(string username, string password)
        {
            return _usersRepository.GetUserByLoginCredentials(username, HashPassword(password));
        }

        public User? GetUserById(int id)
        {
            return _usersRepository.GetUserByID(id);
        }

        public void UpdateUser(User user)
        {
            _usersRepository.UpdateUser(user);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashbytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashbytes);
            }
        }
    }
}
