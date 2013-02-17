using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObliteracyDotNet.Generators.Base
{
    public interface IProjectTemplate
    {
        string TransformText();

        string Namespace { get; set; }
        List<string> IncludedFiles { get; set; }
        Guid ProjectGuid { get; set; }
    }
}
