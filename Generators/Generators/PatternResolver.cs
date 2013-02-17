using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using ObliteracyDotNet.Generators.Templates;
using ObliteracyDotNet.Generators.Structures;
using ObliteracyDotNet.Generators.Templates.Objects;

namespace ObliteracyDotNet.Generators
{
    public class PatternResolver
    {
        private static UnityContainer Resolver;

        static PatternResolver()
        {
            Resolver = new UnityContainer();
            Resolver.RegisterType(typeof(IPatternTemplate), typeof(Contract), "CustomContract");
            Resolver.RegisterType(typeof(IPatternTemplate), typeof(BusinessObject), "CustomBusinessObject");
        }

        public static List<IPatternTemplate> Resolve(string className, PatternBuilderConfig options)
        {
            List<IPatternTemplate> templates = new List<IPatternTemplate>();

            switch (options.PatternType)
            {
                case PatternBuilderTypeCodes.Custom:
                    templates = ResolveCustomPattern();
                    break;
                case PatternBuilderTypeCodes.Adapter:
                    break;
                case PatternBuilderTypeCodes.Composite:
                    break;
                case PatternBuilderTypeCodes.Factory:
                    break;
            }
            
            // Now get the template generators
            templates.ForEach(template =>
            {
                template.ClassName = className;
                template.NameSpace = options.ProjectNamespace;
                template.Methods = options.MethodStructures.ContainsKey(className) ? options.MethodStructures[className] : new List<MethodStruct>();
                template.ReturnType = className;
                
            });
            return templates;
        }

        private static List<IPatternTemplate> ResolveCustomPattern()
        {
            List<IPatternTemplate> templates = new List<IPatternTemplate>();
            templates.Add(Resolver.Resolve<IPatternTemplate>("CustomContract"));
            templates.Add(Resolver.Resolve<IPatternTemplate>("CustomBusinessObject"));
            return templates;
        }
    }
}
