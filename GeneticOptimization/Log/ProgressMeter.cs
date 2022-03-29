using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticOptimization.Log
{
    public class ProgressMeter : IProgressMeter
    {
        private int _currentVal;

        public void Add(int value)
        {
            _currentVal++;
        }

        public int GetCurrent()
        {
            return _currentVal;
        }
    }
}
