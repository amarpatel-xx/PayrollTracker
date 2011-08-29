using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IDogRepository
    {
        void Add(Dog dog);
        void Update(Dog dog);
        void Remove(Dog dog);
        Dog GetById(string dogId);
        ICollection<Dog> GetByFirstName(string firstName);
        ICollection<Dog> GetByLastName(string lastName);
        IList<Dog> GetAllDogs();
    }
}
