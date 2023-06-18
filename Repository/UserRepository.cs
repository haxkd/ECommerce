using EComm.Interface;
using EComm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EComm.Repository
{
    public class UserRepository : IUsers
    {
        readonly DatabaseContext _dbContext = new();

        public UserRepository(DatabaseContext dbContext) {
            _dbContext = dbContext;
        }

        public void UserSignup(User user)
        {
            try
            {
                User? users = _dbContext.Users.FirstOrDefault(x => x.uemail == user.uemail);
                if (users != null)
                {
                    throw new Exception("User already exist");
                }
                else
                {                
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                }
            }
            catch(Exception  e)
            {
                throw new Exception(e.ToString());
            }
        }

       
    }
}
