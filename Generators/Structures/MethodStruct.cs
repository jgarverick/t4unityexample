using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObliteracyDotNet.Generators.Structures
{
    public class MethodStruct
    {
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public List<MethodParam> Parameters { get; set; }
        public Type ReturnType { get; set; }
        public string Description { get; set; }

        public MethodStruct()
        {
            Parameters = new List<MethodParam>();
        }
    }
}
