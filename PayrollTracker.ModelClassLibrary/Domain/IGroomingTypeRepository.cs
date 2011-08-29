using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface IGroomingTypeRepository
    {
        void Add(GroomingType groomingType);
        void Update(GroomingType groomingType);
        void Remove(GroomingType groomingType);
        GroomingType GetById(string groomingTypeId);
        IList<GroomingType> GetAllGroomingTypes();
    }
}
