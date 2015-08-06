using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAppUninstaller
{
    public struct AppxParameters
    {

        public AppxOptions Options { get; set; }

        public string PackageName { get; set; }

        public string PublisherName { get; set; }

        public string UserName { get; set; }

    }
}
