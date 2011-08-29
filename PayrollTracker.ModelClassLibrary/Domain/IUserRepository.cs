using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        void Remove(User user);
        User GetById(string userId);
        User GetByUsername(string username);
        ICollection<User> GetByFirstName(string firstName);
        ICollection<User> GetByLastName(string lastName);
    }
}
