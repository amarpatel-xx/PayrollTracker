using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IRoleRepository
    {
        void Add(Role role);
        void Update(Role role);
        void Remove(Role role);
        Role GetById(string roleId);
        IList<Role> GetAllRoles();
    }
}
