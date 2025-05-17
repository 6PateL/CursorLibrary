using CursorLibrary.Exceptions;
using CursorLibrary.Models;
using CursorLibrary.SD;
using CursorLibrary.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CursorLibrary.Controllers
{
    /// <summary>
    /// Контролер для перехоплення подій вводу користувача (миша та клавіатура) через Windows API
    /// Увага: цей клас веде журнали, які можна знайти в глобальному класі "Logger"
    /// </summary>
    public class InputHookController
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);

        private const int WH_MOUSE_LL = 14; // Low-level mouse hook
        private const int WH_KEYBOARD_LL = 13; // Low-level keyboard hook

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_KEYDOWN = 0x0100;

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private IntPtr _mouseHookHandle = IntPtr.Zero;
        private IntPtr _keyboardHookHandle = IntPtr.Zero;
        private HookProc? _mouseHookProc;
        private HookProc? _keyboardHookProc;

        public event EventHandler<MouseInfoModel>? OnMouseButtonPressed;
        public event EventHandler<byte>? OnKeyPressed;

        private static InputHookController? _instance;
        public static InputHookController Default => _instance ??= new InputHookController();

        private InputHookController() { }

        /// <summary>
        /// Запускає перехоплення подій миші та клавіатури
        /// </summary>
        /// <returns>1 - успішно, 0 - помилка</returns>
        public async Task<int> StartHooksAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    if (_mouseHookHandle != IntPtr.Zero || _keyboardHookHandle != IntPtr.Zero)
                    {
                        Logger.AddLog("Хуки вже запущено");
                        return 1;
                    }

                    _mouseHookProc = MouseHookCallback;
                    _keyboardHookProc = KeyboardHookCallback;

                    var moduleHandle = GetModuleHandle(null);
                    _mouseHookHandle = SetWindowsHookEx(WH_MOUSE_LL, _mouseHookProc, moduleHandle, 0);
                    if (_mouseHookHandle == IntPtr.Zero)
                        throw new CursorApiException("Не вдалося встановити хук миші", new Exception());

                    _keyboardHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardHookProc, moduleHandle, 0);
                    if (_keyboardHookHandle == IntPtr.Zero)
                        throw new KeyboardApiException("Не вдалося встановити хук клавіатури", new Exception());

                    Logger.AddLog("Хуки миші та клавіатури успішно запущено");
                    return 1;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виняток: {ex.Message}");
                Logger.AddLog(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Зупиняє перехоплення подій миші та клавіатури
        /// </summary>
        /// <returns>1 - успішно, 0 - помилка</returns>
        public async Task<int> StopHooksAsync()
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    bool success = true;
                    if (_mouseHookHandle != IntPtr.Zero)
                    {
                        if (!UnhookWindowsHookEx(_mouseHookHandle))
                            success = false;
                        _mouseHookHandle = IntPtr.Zero;
                    }

                    if (_keyboardHookHandle != IntPtr.Zero)
                    {
                        if (!UnhookWindowsHookEx(_keyboardHookHandle))
                            success = false;
                        _keyboardHookHandle = IntPtr.Zero;
                    }

                    if (!success)
                        throw new CursorApiException("Не вдалося зупинити хуки", new Exception());

                    Logger.AddLog("Хуки миші та клавіатури зупинено");
                    return 1;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виняток: {ex.Message}");
                Logger.AddLog(ex.ToString());
                return 0;
            }
        }

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                MouseType? mouseType = null;

                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                    mouseType = MouseType.LEFTMOUSE;
                else if (wParam == (IntPtr)WM_RBUTTONDOWN)
                    mouseType = MouseType.RIGHTMOUSE;

                if (mouseType.HasValue)
                {
                    var model = new MouseInfoModel
                    {
                        PositionX = hookStruct.pt.X,
                        PositionY = hookStruct.pt.Y,
                        MouseType = mouseType.Value
                    };
                    Logger.AddLog($"Натиснуто кнопку миші: {mouseType}, X={hookStruct.pt.X}, Y={hookStruct.pt.Y}");
                    OnMouseButtonPressed?.Invoke(this, model);
                }
            }

            return CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
        }

        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                var hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                var keyCode = (byte)hookStruct.vkCode;
                Logger.AddLog($"Натиснуто клавішу: {keyCode}");
                OnKeyPressed?.Invoke(this, keyCode);
            }

            return CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
        }
    }
}