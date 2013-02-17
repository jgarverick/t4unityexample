using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ObliteracyDotNet.Generators.Structures
{
    public class PatternBuilderConfig
    {
        public string ProjectName { get; set; }
        public string ProjectNamespace { get; set; }
        public string DBServer { get; set; }
        public string DBInstance { get; set; }
        public string ContextContainerName { get; set; }
        public string SaveDirectory { get; set; }
        public string ReturnType { get; set; }
        public Dictionary<string,List<MethodStruct>> MethodStructures { get; set; }
        public PatternBuilderOptions BuilderOptions { get; set; }
        public PatternBuilderTypeCodes PatternType { get; set; }

    }

    
}
