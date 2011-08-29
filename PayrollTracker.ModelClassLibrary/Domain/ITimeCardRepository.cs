using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface ITimeCardRepository
    {
        void Add(TimeCard timeCard);
        void Update(TimeCard timeCard);
        void Remove(TimeCard timeCard);
        TimeCard GetById(string timeCardId);
        ICollection<TimeCard> GetByUser(User user);
        TimeCard GetMostRecentTimeIn(User user);
        TimeCard GetMostRecentTimeOut(User user);
        TimeCard GetMostRecentTimeIn(User user, DateTime payrollStartTime, DateTime payrollEndTime);
        IList<TimeCard> GetCurrentTimeCards(User user, DateTime payrollStartTime, DateTime payrollEndTime);
    }
}
