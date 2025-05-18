using MvcWhatsUp.Models;

namespace MvcWhatsUp.Services.Interfaces
{
    public interface IUsersService
    {
        //NOTE the difference from the sheets!!!
        List<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetByLoginCredentials(string username, string password);
        void AddUser(User user);

        //Should be!!
        //User UpdateUser(User user);

        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
