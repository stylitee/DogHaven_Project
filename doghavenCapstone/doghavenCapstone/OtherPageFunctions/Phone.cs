using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.OtherPageFunctions
{
    public class Phone
    {
        public static void CloseApplication()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
