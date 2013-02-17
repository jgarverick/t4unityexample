using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObliteracyDotNet.Generators
{
    public enum PatternBuilderTypeCodes
    {
        Custom,
        FullDataModel,
        Factory,
        Adapter,
        Composite,
    }

    [Flags]
    public enum PatternBuilderOptions
    {
        CreateVSProjFile,
        CompileCodeAfterGeneration,
    }
}
