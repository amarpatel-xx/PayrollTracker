using System;
using System.Collections.Generic;

namespace PayrollTracker.ModelClassLibrary.Domain
{
    public interface ICostRepository
    {
        void Add(Cost cost);
        void Update(Cost cost);
        void Remove(Cost cost);
        Cost GetById(string costId);
    }
}
