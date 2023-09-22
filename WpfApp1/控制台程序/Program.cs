
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace 控制台程序
{
    internal class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        static void Main(string[] args)
        {
            // 获取屏幕指定坐标点的颜色
            int x = 802;
            int y = 50;
            Color pixelColor = GetPixelColor(x, y);

            // 输出颜色信息
            Console.WriteLine("Pixel color at ({0}, {1}): R={2}, G={3}, B={4}",
                x, y, pixelColor.R, pixelColor.G, pixelColor.B);

            Console.ReadLine();
        }

        static Color GetPixelColor(int x, int y)
        {
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(desktopPtr, x, y);
            ReleaseDC(IntPtr.Zero, desktopPtr);

            byte r = (byte)(pixel & 0x000000FF);
            byte g = (byte)((pixel & 0x0000FF00) >> 8);
            byte b = (byte)((pixel & 0x00FF0000) >> 16);

            Color color = Color.FromRgb(r, g, b);
            return color;
        }
    }
}
