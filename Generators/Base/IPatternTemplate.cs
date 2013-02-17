using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObliteracyDotNet.Generators.Structures;

namespace ObliteracyDotNet.Generators
{
    public interface IPatternTemplate
    {
        string NameSpace { get; set; }
        string ClassName { get; set; }
        List<MethodStruct> Methods { get; set; }
        string ReturnType { get; set; }
        string FileName { get; }
        string TransformText();
    }
}
