using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IGroomingRepository
    {
        void Add(Grooming grooming);
        void Update(Grooming grooming);
        void Remove(Grooming grooming);
        Grooming GetById(string groomingId);
        ICollection<Grooming> GetByUser(User user);
        IList<Grooming> GetRecentGroomings(User user, DateTime payrollStartTime, DateTime payrollEndTime);
    }
}
