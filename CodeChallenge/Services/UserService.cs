using CodeChallenge.Constants;
using CodeChallenge.Persistence;
using CodeChallenge.Persistence.DBModel;
using CodeChallenge.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Services;

public class UserService : IUserService
{
    public SchedulingContext _dbContext;

    public UserService(
        SchedulingContext schedulingContext
        ) 
    {
        _dbContext = schedulingContext;
    }

    public async Task<User> GetUserById(Guid id) 
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<User> AddUser(UserRequestModel model)
    {
        var user = new User
        {
            UserId = Guid.NewGuid(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Type = model.UserType == UserConstants.UserTypeProvider ? UserConstants.UserTypeProvider : UserConstants.UserTypePatient
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;

    }
}
