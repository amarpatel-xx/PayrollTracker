using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IDaycareRepository
    {
        void Add(Daycare daycare);
        void Update(Daycare daycare);
        void Remove(Daycare daycare);
        Daycare GetById(string daycareId);
        ICollection<Daycare> GetByUser(User user);
        IList<Daycare> GetRecentDaycares(User user, DateTime payrollStartTime, DateTime payrollEndTime);
    }
}
