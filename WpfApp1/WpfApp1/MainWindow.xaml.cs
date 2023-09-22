using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using System.Windows.Xps;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public MainWindow()
        {
            InitializeComponent();

            UiDispatcher = Dispatcher.CurrentDispatcher;    //实例化


            Thread testThread = new Thread(forThread);
            testThread.IsBackground = true;
            testThread.Start();
        }


        static System.Drawing.Color pixelColor;
        public static Dispatcher UiDispatcher { get; set; }    //访问控件使用

        /// <summary>
        /// 循环线程
        /// </summary>
        void forThread()
        {
            while (true)
            {
                // 获取屏幕指定坐标点的颜色
                int x = 1844;
                int y = 557;
                pixelColor = GetPixelColor(x, y);

                string tmp = $"Pixel color at ({x}, {y}): R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}";

                //转换为int好判断
                int r = int.Parse(pixelColor.R.ToString());
                int g = int.Parse(pixelColor.G.ToString());
                int b = int.Parse(pixelColor.B.ToString());

                tmp = $"Pixel color at ({x}, {y}): R={r}, G={g}, B={b}";

                // 需要更新txtColor
                UiDispatcher.Invoke(new Action(() =>
                {
                    txtColor.Text = tmp; // 修改TextBox的Text属性
                }));


                Thread.Sleep(1000);
            }
        }


        /// <summary>
        /// 获取颜色方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(desktopPtr, x, y);
            ReleaseDC(IntPtr.Zero, desktopPtr);

            byte r = (byte)(pixel & 0x000000FF);
            byte g = (byte)((pixel & 0x0000FF00) >> 8);
            byte b = (byte)((pixel & 0x00FF0000) >> 16);

            System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
            return color;
        }
    }
}
