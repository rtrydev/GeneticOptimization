using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticOptimization.Log
{
    public interface IProgressMeter
    {
        public int GetCurrent();
        public void Add(int value);
    }
}
