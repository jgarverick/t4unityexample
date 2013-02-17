using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObliteracyDotNet.Generators.Structures;

namespace ObliteracyDotNet.Generators
{
    public interface IPatternBuilder
    {
        void Execute();
        string ContractSource { get; set; }
        string ServiceSource { get; set; }
    }
}
