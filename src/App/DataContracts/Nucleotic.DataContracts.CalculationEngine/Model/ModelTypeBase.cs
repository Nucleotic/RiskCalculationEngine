using System;
using System.Collections.Generic;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class ModelTypeBase<T>
    {
        public List<Exception> Exceptions;

        public ModelTypeBase()
        {
            Exceptions = new List<Exception>();
        }
    }

    public class ListModelTypeBase<T> : List<T>
    {
        public List<Exception> Exceptions;

        public ListModelTypeBase()
        {
            Exceptions = new List<Exception>();
        }
    }
}