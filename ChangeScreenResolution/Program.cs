using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace ChangeScreenResolution
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            int width = int.Parse(args[0]);
            int height = int.Parse(args[1]);
            int fontScale = int.Parse(args[2]);
    
            ChangeFontScale(fontScale);
            ChangeResolution(width, height);
        }

        private static void ChangeResolution(int width, int height)
        {
            ChangeDisplayResolution(@"\\.\DISPLAY1", width, height);
            ChangeDisplayResolution(@"\\.\DISPLAY2", width, height);
        }

        private static void ChangeDisplayResolution(string deviceName, int width, int height)
        {
            DEVMODE1 devMode = new DEVMODE1();
            if (User32.EnumDisplaySettings(deviceName, 0, ref devMode) == 0)
                throw new InvalidOperationException("Unable to enumerate display settings");

            devMode.dmPelsWidth = width;
            devMode.dmPelsHeight = height;
            if (User32.ChangeDisplaySettingsEx(deviceName, ref devMode, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_NONE, IntPtr.Zero)
             != DISP_CHANGE.Successful)
                throw new Win32Exception();
        }

        private static void ChangeFontScale(int fontSize)
        {
            using (var controlPanel = Registry.CurrentUser.OpenSubKey("Control Panel", true))
            using (var desktop = controlPanel.OpenSubKey("Desktop", true))
            using (var perMonitorSettings = desktop.OpenSubKey("PerMonitorSettings", true))
            {
                foreach (var subKeyName in perMonitorSettings.GetSubKeyNames())
                {
                    using (var screen = perMonitorSettings.OpenSubKey(subKeyName, true))
                    {
                        switch (fontSize)
                        {
                            case 100:
                                screen.SetValue("DpiValue", -2);
                                break;

                            case 125:
                                screen.SetValue("DpiValue", -1);
                                break;

                            case 150:
                                screen.SetValue("DpiValue", 0);
                                break;

                            case 175:
                                screen.SetValue("DpiValue", 1);
                                break;

                            case 200:
                                screen.SetValue("DpiValue", 2);
                                break;

                            default:
                                throw new InvalidOperationException("Invalid font scale, only support 100, 125, 150, 175, and 200");
                        }
                    }
                }
            }
        }
    }
}