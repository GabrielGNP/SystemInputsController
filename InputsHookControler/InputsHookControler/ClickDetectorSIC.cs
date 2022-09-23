using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ClickDetectorSIC
{
    
    public class ClassClickDetectorSIC
    {

        public enum MouseMessage
        {
            MouseMove = 0x200,
            LButtonDown = 0x201,
            LButtonUp = 0x202,
            RButtonDown = 0x204,
            RButtonUp = 0x205,
            MouseWheel = 0x20a,
            MouseHWheel = 0x20e
        }

        [StructLayout(LayoutKind.Sequential)]


        public struct Point
        {
            public int x;
            public int y;
        }
        internal struct MouseLowLevelHookStruct
        {
            public Point pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, int wParam, IntPtr lParam);
        private LowLevelMouseProc MouseProcess;

        public static IntPtr msHook;

        public event MouseEventHandler ClickUp;
        public event MouseEventHandler ClickDown;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
           LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            int wParam, IntPtr lParam);


        public ClassClickDetectorSIC()
        {
            Hook();
        }
        ~ClassClickDetectorSIC()
        {
            UnHook();
        }

        public IntPtr MouseHook { get; private set; } = IntPtr.Zero;


        public void Hook()
        {
            MouseProcess = new LowLevelMouseProc(CaptureClick);
            if (MouseHook == IntPtr.Zero)
                MouseHook = SetWindowsHookEx(14, MouseProcess, IntPtr.Zero, 0);
        }
        public void UnHook()
        {
            UnhookWindowsHookEx(MouseHook);
        }



        public class NewMouseMessageEventArgs : EventArgs
        {
            public Point Position { get; private set; }
            public MouseMessage MessageType { get; private set; }

            public NewMouseMessageEventArgs(Point position, MouseMessage msg)
            {
                Position = position;
                MessageType = msg;
            }
        }

        public int ClI = 0;
        public int ClD = 0;
        public int CMR = 0;
        public int RdR = 0;
        public int BX1 = 0;
        public int BX2 = 0;

        const int WM_MOUSEMOVE = 0x200;

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_LBUTTONDBLCLK = 0x203;

        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;
        const int WM_RBUTTONDBLCLK = 0x206;

        const int WM_MBUTTONDOWN = 0x207;
        const int WM_MBUTTONUP = 0x208;
        const int WM_MBUTTONDBLCLK = 0x209;

        const int WM_MOUSEWHEEL = 0x020a;
        const int WM_MOUSEHWHEEL = 0x020e;

        const int WM_XBUTTONDOWN = 0x020B;
        const int WM_XBUTTONUP = 0x020C;
        const int WM_XBUTTONDBLCLK = 0x020D;

        const int MK_XBUTTON1 = 0x0020;
        const int MK_XBUTTON2 = 0x0040;

        private enum MouseEventType
        {
            None,
            MouseDown,
            MouseUp,
            DoubleClick,
            MouseWheel,
            MouseMove
        }

        private IntPtr CaptureClick(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {

                var st = Marshal.PtrToStructure<MouseLowLevelHookStruct>(lParam);

                MouseLowLevelHookStruct ClickInfo = (MouseLowLevelHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLowLevelHookStruct));

                MouseButtons button = GetButton(wParam, ClickInfo);
                MouseEventType eventType = GetEventType(wParam);

                MouseEventArgs stev = new MouseEventArgs(button,
                    (eventType == MouseEventType.DoubleClick ? 2 : 1),
                    ClickInfo.pt.x,
                    ClickInfo.pt.y,
                    (eventType == MouseEventType.MouseWheel ? (short)((ClickInfo.mouseData >> 16) & 0xffff) : 0));
            

                if (wParam == WM_MOUSEWHEEL)
                {
                    RdR++;
                }
                else
                {
                    if (wParam == WM_MOUSEHWHEEL)
                    {
                        RdR++;
                    }
                }
              
                if (eventType == MouseEventType.MouseUp)
                {
                    //MessageBox.Show(" BX2=" + BX2);
                    ClickUp(this, stev);
                }

              
            }
            return CallNextHookEx(MouseHook, nCode, wParam, lParam);
        }

        private MouseButtons GetButton(int wParam, MouseLowLevelHookStruct clickInfo)
        {
            switch (wParam)
            {

                case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:
                case WM_LBUTTONDBLCLK:
                    return MouseButtons.Left;
                case WM_RBUTTONDOWN:
                case WM_RBUTTONUP:
                case WM_RBUTTONDBLCLK:
                    return MouseButtons.Right;
                case WM_MBUTTONDOWN:
                case WM_MBUTTONUP:
                case WM_MBUTTONDBLCLK:
                    return MouseButtons.Middle;

                case WM_XBUTTONUP:
                case WM_XBUTTONDOWN:
                case WM_XBUTTONDBLCLK:
                    {

                        if (wParam == WM_XBUTTONUP && clickInfo.mouseData == 131072) // codigo de XButton1
                        {

                            //BX1++;
                            //MessageBox.Show(clickInfo.mouseData.ToString() + " BX1=" + BX1);
                            return MouseButtons.XButton1;
                        }
                        else
                        {
                            if (wParam == WM_XBUTTONUP && clickInfo.mouseData == 65536)// codigo de XButton2
                            {
                                //BX2++;
                                //MessageBox.Show(clickInfo.mouseData.ToString() + " BX2=" + BX2);
                                return MouseButtons.XButton2;

                            }
                            else
                            {
                                return MouseButtons.None;
                            }
                        }
                        break;
                    }
                default:
                    return MouseButtons.None;

            }
        }

        private MouseEventType GetEventType(Int32 wParam)
        {
            switch (wParam)
            {
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                case WM_XBUTTONDOWN:
                    return MouseEventType.MouseDown;
                case WM_LBUTTONUP:
                case WM_RBUTTONUP:
                case WM_MBUTTONUP:
                case WM_XBUTTONUP:
                    return MouseEventType.MouseUp;
                case WM_LBUTTONDBLCLK:
                case WM_RBUTTONDBLCLK:
                case WM_MBUTTONDBLCLK:
                case WM_XBUTTONDBLCLK:
                    return MouseEventType.DoubleClick;
                case WM_MOUSEWHEEL:
                    return MouseEventType.MouseWheel;
                case WM_MOUSEMOVE:
                    return MouseEventType.MouseMove;
                default:
                    return MouseEventType.None;
            }
        }
    }
    
}
