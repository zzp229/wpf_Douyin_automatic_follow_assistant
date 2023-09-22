using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsInput.Native;
using WindowsInput;

namespace 版本1
{
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
            //testThread.IsBackground = true;
            testThread.Start();
        }


        static Color pixelColor;
        public static Dispatcher UiDispatcher { get; set; }    //访问控件使用

        /// <summary>
        /// 循环线程
        /// </summary>
        void forThread()
        {
            //先等几秒先
            Thread.Sleep(4000);

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


                //判断使用什么功能
                KeyToFollow();

                tmp = $"Pixel color at ({x}, {y}): R={r}, G={g}, B={b}";

                // 需要更新txtColor
                UiDispatcher.Invoke(new Action(() =>
                {
                    txtColor.Text = tmp; // 修改TextBox的Text属性
                }));


                Thread.Sleep(2000);
            }
        }


        void KeyToFollow()
        {
            Dispatcher.Invoke(() =>
            {
                // 查找活动窗口
                IntPtr activeWindowHandle = GetForegroundWindow();

                // 获取活动窗口的 AutomationElement
                AutomationElement activeWindowElement = AutomationElement.FromHandle(activeWindowHandle);

                // 查找包含焦点的元素
                AutomationElement focusedElement = activeWindowElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.HasKeyboardFocusProperty, true));

                // 模拟在焦点元素中按下 'G'
                if (focusedElement != null)
                {
                    InputSimulator simulator = new InputSimulator();
                    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_F);
                }
            });
        }


        #region KeyToFollow使用
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]


        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int INPUT_KEYBOARD = 1;


        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const ushort G_KEY = 0x47;
        #endregion


        /// <summary>
        /// 获取颜色方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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
