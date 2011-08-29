using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IBoardingRepository
    {
        void Add(Boarding boarding);
        void Update(Boarding boarding);
        void Remove(Boarding boarding);
        Boarding GetById(string boardingId);
        ICollection<Boarding> GetByUser(User user);
        IList<Boarding> GetRecentBoardings(User user, DateTime payrollStartTime, DateTime payrollEndTime);
    }
}
