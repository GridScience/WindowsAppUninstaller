using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAppUninstaller
{
    public enum AppxOptions
    {

        AllUsers = 0x01,
        PackageNameFilter = 0x02,
        PublisherFilter = 0x04,
        UserFilter = 0x08,
        Confirm = 0x10,
        DisplayWhatIf = 0x20

    }
}
