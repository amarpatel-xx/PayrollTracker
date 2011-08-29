using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface ICostTypeRepository
    {
        void Add(CostType costType);
        void Update(CostType costType);
        void Remove(CostType costType);
        CostType GetById(string costTypeId);
        CostType GetByCostName(string costName);
        IList<CostType> GetAllSimilarCostTypes(string partialCostName);
    }
}
