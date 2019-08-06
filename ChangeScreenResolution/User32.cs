using System;
using System.Runtime.InteropServices;

namespace ChangeScreenResolution
{
    internal class User32
    {
        public const int ENUM_CURRENT_SETTINGS = -1;

        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE1 devMode);

        [DllImport("user32.dll")]
        public static extern DISP_CHANGE ChangeDisplaySettingsEx(
            string lpszDeviceName, ref DEVMODE1 lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevicesW(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);
    }
}