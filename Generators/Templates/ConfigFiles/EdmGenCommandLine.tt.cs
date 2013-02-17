using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObliteracyDotNet.Generators.Templates.ConfigFiles
{
    public partial class EdmGenCommandLine
    {
        public string NameSpace
        {
            get { return this._namespaceField; }
            set { this._namespaceField = value; }
        }

        public string ContainerName
        {
            get { return this._containerNameField; }
            set { this._containerNameField = value; }
        }

        public string DatabaseServer
        {
            get { return this._dbServerField; }
            set { this._dbServerField = value; }
        }

        public string DatabaseName
        {
            get { return this._instanceNameField; }
            set { this._instanceNameField = value; }
        }

        public string ProjectName
        {
            get { return this._projectField; }
            set { this._projectField = value; }
        }

    }
}
