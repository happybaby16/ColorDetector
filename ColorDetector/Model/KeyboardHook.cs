using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace ColorDetector.Model
{
    public static class KeyboardHook
    {
        private static bool IsAppStart { get; set; } = false;//Свойство, которое показывает запущено ли приложение
        public static event EventHandler KeyboardStartApplication = delegate { };//Приложение будет запускаться по сочетанию клавиш CTRL+SHIFT+c
        public static event EventHandler KeyboardStopApplication = delegate { };//Приложение будет останавливаться по сочетанию клавиш CTRL+SHIFT+c
        public static event DependencyPropertyChangedEventHandler KeyboardShowMenu = delegate { };//Приложение будет запускаться по сочетанию клавиш CTRL+SHIFT+c
        private const int WH_KEYBOARD_LL = 13;//Код хука
        private const int WM_KEYDOWN = 0x0100;//Код нажатия клавиши
        private const int VK_SHIFT = 0X10;//Виртуальная клавиша шифта
        private const int VK_CONTROL = 0x11;//Виртуальная клавиша контрола
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void Start()
        {
            _hookID = SetHook(_proc);
        }


        public static void Stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (GetKeyState(VK_SHIFT)&GetKeyState(VK_CONTROL)&vkCode == 86)
                {
                    if (!IsAppStart)
                    {
                        KeyboardStartApplication(null, new EventArgs());
                        IsAppStart = true;
                    }
                    else
                    {
                        KeyboardStopApplication(null, new EventArgs());
                        IsAppStart = false;
                    }
                    
                }
                if (GetKeyState(VK_SHIFT) & GetKeyState(VK_CONTROL) & vkCode == 77)
                {
                    KeyboardShowMenu(null, new DependencyPropertyChangedEventArgs());
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetKeyState(int nVirtKey);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
