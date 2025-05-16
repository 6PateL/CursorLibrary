using CursorLibrary.Exceptions;
using CursorLibrary.Models;
using CursorLibrary.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CursorLibrary.Controllers
{
    /// <summary>
    /// Main class - api controller 
    /// Attention: this class keeps logs, logs can be found in the global "Logger" class  
    /// </summary>
    public class CursorApiController
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, nuint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, nuint dwExtraInfo);

        private readonly object _lockObject = new();

        public event EventHandler<MouseInfoModel>? OnMouseMoved;
        public event EventHandler<MouseInfoModel>? OnLeftMouseClicked;
        public event EventHandler<MouseInfoModel>? OnRightMouseClicked;
        public event EventHandler? OnKeyPressed; 

        //mouse
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        //keyboard 
        const uint KEYEVENTF_KEYUP = 0x0002;

        private static CursorApiController? _instance;
        public static CursorApiController Default => _instance ??= new CursorApiController();

        private static MouseInfoModel InfoModel = new(); 

        private CursorApiController() { }

        /// <summary>
        /// Set cursor position on screen
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>
        /// result code:
        /// 1 - operation was completed successfully.
        /// 0 - an error was occured. 
        /// </returns>
        public async Task<int> SetCursorPosition(int x, int y)
        {
            await Task.Delay(100);
            try
            {
                lock (_lockObject)
                {
                    SetCursorPos(x, y);

                    InfoModel.PositionX = x;
                    InfoModel.PositionY = y;

                    Logger.AddLog($"Mouse moved: X:{x}, Y:{y}"); 

                    OnMouseMoved?.Invoke(this, InfoModel);
                }
                return 1;
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Logger.AddLog(ex.ToString()); 

                return 0;
            }
        }

        /// <summary>
        /// Simulate left click 
        /// </summary>
        /// <returns>
        /// result code:
        /// 1 - operation was completed successfully.
        /// 0 - an error was occured. 
        /// </returns>
        public async Task<int> SimulateLeftMouseClick()
        {
            await Task.Delay(100);
            try
            {
                lock (_lockObject)
                {
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, nuint.Zero);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, nuint.Zero);

                    Logger.AddLog("Left mouse was simulated!");

                    OnLeftMouseClicked?.Invoke(this, InfoModel);
                }
                return 1;
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Logger.AddLog(ex.ToString());

                return 0;
            }
        }

        /// <summary>
        /// Simulate right click 
        /// </summary>
        /// <returns>
        /// result code:
        /// 1 - operation was completed successfully.
        /// 0 - an error was occured. 
        /// </returns>
        public async Task<int> SimulateRightMouseClick()
        {
            await Task.Delay(100);
            try
            {
                lock (_lockObject)
                {
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, nuint.Zero);
                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, nuint.Zero);

                    Logger.AddLog("Right mouse was simulated!"); 

                    OnRightMouseClicked?.Invoke(this, InfoModel);
                }
                return 1;
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Logger.AddLog(ex.ToString());

                return 0;
            }
        }

        /// <summary>
        /// Simulate keyboard key  
        /// </summary>
        /// <param name="keyCode">key code, you can find in class KeyboardKeys</param>
        /// <returns>
        /// result code:
        /// 1 - operation was completed successfully.
        /// 0 - an error was occured. 
        /// </returns>
        public async Task<int> SimulateKeyPressing(byte keyCode)
        {
            await Task.Delay(100); 

            try
            {
                lock (_lockObject)
                {
                    keybd_event(keyCode, 0, 0, nuint.Zero);
                    keybd_event(keyCode, 0, KEYEVENTF_KEYUP, nuint.Zero);

                    Logger.AddLog($"Key was simulated!: Key {keyCode}"); 

                    OnKeyPressed?.Invoke(this, EventArgs.Empty);
                }
            }
            catch(CursorApiException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Logger.AddLog(ex.ToString()); 

                return 0; 
            }

            return 1; 
        }
    }
}
