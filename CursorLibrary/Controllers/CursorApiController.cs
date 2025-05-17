using CursorLibrary.Exceptions;
using CursorLibrary.Models;
using CursorLibrary.SD;
using CursorLibrary.Utilities;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CursorLibrary.Controllers
{
    /// <summary>
    /// Основний клас - контролер API для роботи з мишею
    /// Увага: цей клас веде журнали, які можна знайти в глобальному класі "Logger"
    /// </summary>
    public class CursorApiController
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, nuint dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public event EventHandler<MouseInfoModel>? OnMouseMoved;
        public event EventHandler<MouseInfoModel>? OnLeftMouseClicked;
        public event EventHandler<MouseInfoModel>? OnRightMouseClicked;
        public event EventHandler<MouseInfoModel>? OnMousePulledUp;
        public event EventHandler<MouseInfoModel>? OnMousePulledDown;

        // Миша
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        private static CursorApiController? _instance;
        public static CursorApiController Default => _instance ??= new CursorApiController();

        private CursorApiController() { }

        private static MouseInfoModel GetCurrentMouseInfo()
        {
            var model = new MouseInfoModel();
            if (GetCursorPos(out POINT point))
            {
                model.PositionX = point.X;
                model.PositionY = point.Y;
            }
            return model;
        }

        /// <summary>
        /// Встановити позицію курсора на екрані
        /// </summary>
        /// <param name="x">X позиція на екрані</param>
        /// <param name="y">Y позиція на екрані</param>
        /// <returns>
        /// Код результату:
        /// 1 - операція успішно завершена.
        /// 0 - сталася помилка.
        /// </returns>
        public async Task<int> SetCursorPosition(int x, int y)
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    var result = SetCursorPos(x, y);
                    if (!result)
                        throw new CursorApiException("Помилка SetCursorPos.", new Exception());

                    var infoModel = GetCurrentMouseInfo();
                    infoModel.PositionX = x;
                    infoModel.PositionY = y;

                    Logger.AddLog($"Курсор переміщено: X:{x}, Y:{y}");

                    OnMouseMoved?.Invoke(this, infoModel);
                    return 1;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Виняток: {ex.Message}");
                Logger.AddLog(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Симуляція відпускання кнопки миші
        /// </summary>
        /// <param name="mouseType">Ліва або права кнопка миші</param>
        /// <returns>
        /// Код результату:
        /// 1 - операція успішно завершена.
        /// 0 - сталася помилка.
        /// </returns>
        public async Task<int> SimulateMousePullUp(MouseType mouseType)
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    var infoModel = GetCurrentMouseInfo();
                    infoModel.MouseType = mouseType;

                    switch (mouseType)
                    {
                        case MouseType.LEFTMOUSE:
                            {
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                                Logger.AddLog("Симуляція відпускання лівої кнопки миші");
                                OnMousePulledUp?.Invoke(this, infoModel);
                                break;
                            }
                        case MouseType.RIGHTMOUSE:
                            {
                                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
                                Logger.AddLog("Симуляція відпускання правої кнопки миші");
                                OnMousePulledUp?.Invoke(this, infoModel);
                                break;
                            }
                    }
                    return 1;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Виняток: {ex.Message}");
                Logger.AddLog(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Симуляція натискання кнопки миші
        /// </summary>
        /// <param name="mouseType">Ліва або права кнопка миші</param>
        /// <returns>
        /// Код результату:
        /// 1 - операція успішно завершена.
        /// 0 - сталася помилка.
        /// </returns>
        public async Task<int> SimulateMousePullDown(MouseType mouseType)
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    var infoModel = GetCurrentMouseInfo();
                    infoModel.MouseType = mouseType;

                    switch (mouseType)
                    {
                        case MouseType.LEFTMOUSE:
                            {
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                                Logger.AddLog("Симуляція натискання лівої кнопки миші");
                                OnMousePulledDown?.Invoke(this, infoModel);
                                break;
                            }
                        case MouseType.RIGHTMOUSE:
                            {
                                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
                                Logger.AddLog("Симуляція натискання правої кнопки миші");
                                OnMousePulledDown?.Invoke(this, infoModel);
                                break;
                            }
                    }
                    return 1;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Виняток: {ex.Message}");
                Logger.AddLog(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Симуляція кліку миші на екрані
        /// </summary>
        /// <param name="mouseType">Ліва або права кнопка миші</param>
        /// <returns>
        /// Код результату:
        /// 1 - операція успішно завершена.
        /// 0 - сталася помилка.
        /// </returns>
        public async Task<int> SimulateMouseClick(MouseType mouseType)
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    var infoModel = GetCurrentMouseInfo();
                    infoModel.MouseType = mouseType;

                    switch (mouseType)
                    {
                        case MouseType.LEFTMOUSE:
                            {
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, nuint.Zero);
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, nuint.Zero);
                                Logger.AddLog("Симуляція кліку лівої кнопки миші");
                                OnLeftMouseClicked?.Invoke(this, infoModel);
                                break;
                            }
                        case MouseType.RIGHTMOUSE:
                            {
                                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, nuint.Zero);
                                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, nuint.Zero);
                                Logger.AddLog("Симуляція кліку правої кнопки миші");
                                OnRightMouseClicked?.Invoke(this, infoModel);
                                break;
                            }
                    }
                    return 1;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (CursorApiException ex)
            {
                Console.WriteLine($"Виняток: {ex.Message}");
                Logger.AddLog(ex.ToString());
                return 0;
            }
        }
    }
}