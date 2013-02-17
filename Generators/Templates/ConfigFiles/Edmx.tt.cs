using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObliteracyDotNet.Generators.Templates.ConfigFiles
{
    public partial class EdmxFile
    {
        public string CSDLText
        {
            get { return this._CSDLField; }
            set { this._CSDLField = value; }
        }

        public string SSDLText
        {
            get { return this._SSDLField; }
            set { this._SSDLField = value; }
        }

        public string MSLText
        {
            get { return this._MSLField; }
            set { this._MSLField = value; }
        }
    }
}
