using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IPickupDropoffRepository
    {
        void Add(PickupDropoff pickupDropoff);
        void Update(PickupDropoff pickupDropoff);
        void Remove(PickupDropoff pickupDropoff);
        PickupDropoff GetById(string pickupDropoffId);
        ICollection<PickupDropoff> GetByUser(User user);
        IList<PickupDropoff> GetRecentPickupsAndDropoffs(User user, DateTime payrollStartTime, DateTime payrollEndTime);
    }
}
