using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace KeyDetectorSIC
{
    
    public class ClassKeyDetectorSIC
    {
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, IntPtr lParam);
        private LowLevelKeyboardProc keyboardProcess;

        public static IntPtr ptrHook { get; private set; } = IntPtr.Zero;

        public event KeyEventHandler KeyUp;
        public event KeyEventHandler KeyDown;

        // Captura la tecla presionada
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);

        // Captura la siguiente tecla
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, int wp, IntPtr lp);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);

        //Quita el gancho a la tecla presionada
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookwindowsHookEx(IntPtr hook);

        public ClassKeyDetectorSIC()
        {
            Hook();
        }
        ~ClassKeyDetectorSIC()
        {
            UnHook();
        }

        public void Hook()
        {
            ProcessModule processModule = Process.GetCurrentProcess().MainModule;
            keyboardProcess = new LowLevelKeyboardProc(CaptureKey);
            if (ptrHook == IntPtr.Zero)
                ptrHook = SetWindowsHookEx(13, keyboardProcess, GetModuleHandle(processModule.ModuleName), 0);
        }
        public void UnHook()
        {
            UnhookwindowsHookEx(ptrHook);
        }

        private IntPtr CaptureKey(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT keyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                KeyEventArgs eventArgs = new KeyEventArgs(keyInfo.key);
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && KeyDown != null)
                {
                    //MessageBox.Show(eventArgs.KeyCode.ToString());
                    KeyDown(this, eventArgs);
                }
                else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && KeyUp != null)
                {
                    KeyUp(this, eventArgs);
                }
                if (eventArgs.Handled) return (IntPtr)1;
            }
            return CallNextHookEx(ptrHook, nCode, wParam, lParam);
        }
    }

}
