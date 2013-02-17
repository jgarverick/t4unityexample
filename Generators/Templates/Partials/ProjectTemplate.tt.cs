using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObliteracyDotNet.Generators.Base;

namespace ObliteracyDotNet.Generators.Templates.Projects
{
    public partial class ProjectTemplate : IProjectTemplate
    {
        public Guid ProjectGuid
        {
            get { return this._projectGuidField; }
            set { this._projectGuidField = value; }
        }

        public string Namespace
        {
            get { return this._namespaceField; }
            set { this._namespaceField = value; }
        }

        public List<string> IncludedFiles
        {
            get { return this._includesField; }
            set { this._includesField = value; }
        }


    }
}
