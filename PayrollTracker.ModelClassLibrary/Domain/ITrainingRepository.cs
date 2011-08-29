using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface ITrainingRepository
    {
        void Add(Training training);
        void Update(Training training);
        void Remove(Training training);
        Training GetById(string trainingId);
        ICollection<Training> GetByUser(User user);
        IList<Training> GetRecentTrainings(User user, DateTime payrollStartTime, DateTime payrollEndTime);
    }
}
