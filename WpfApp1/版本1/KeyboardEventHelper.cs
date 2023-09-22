using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 版本1
{
    public static class KeyboardEventHelper
    {
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void PressKey(Key key)
        {
            byte keyByte = (byte)KeyInterop.VirtualKeyFromKey(key);
            keybd_event(keyByte, 0, 0, 0);
        }

        public static void ReleaseKey(Key key)
        {
            byte keyByte = (byte)KeyInterop.VirtualKeyFromKey(key);
            keybd_event(keyByte, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
