using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObliteracyDotNet.Generators.Templates.ConfigFiles
{
    public partial class EdmxAppConfig
    {
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
