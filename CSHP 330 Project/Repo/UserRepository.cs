using CSHP_330_Project.model;

namespace CSHP_330_Project.Repo;

public interface IUserRepository
{
    IEnumerable<User> Users { get; }
    int Add(User user);
    int DeleteId(int userId);
    User Find(int userId);
    User? Update(int userId, User user);
}

public class UserRepository : IUserRepository
{
    private static List<User> _users = new List<User>(){
        new User(101, "blah@email.com", "pass1"),
        new User(102, "foo@email.com", "pass2"),
        new User(103, "bar@email.com", "pass3"),
        new User(104, "admin@email.com", "adminadmin")
    };

    public IEnumerable<User> Users => _users;

    public int Add(User user)
    {
        user.createdDate = DateTime.UtcNow;
        user.Id = _users.Select(u => u.Id).Max() + 1;

        _users.Add(user);

        return user.Id;
    }

    public int DeleteId(int userId)
    {
        return _users.RemoveAll(u => u.Id == userId);
    }

    public User Find(int userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }

    public User? Update(int userId, User user)
    {
        var getUser = Find(userId);

        if (getUser == null)
        {
            return null;
        }

        getUser.email = user.email;
        getUser.password = user.password;

        return getUser;
    }
}