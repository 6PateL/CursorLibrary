using CursorLibrary.Exceptions;
using CursorLibrary.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CursorLibrary.Controllers
{
    /// <summary>
    /// Клас-контролер API для роботи з клавіатурою
    /// Увага: цей клас веде журнали, які можна знайти в глобальному класі "Logger"
    /// </summary>
    public class KeyboardApiController
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, nuint dwExtraInfo);

        // Клавіатура
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public event EventHandler? OnKeyPressed;

        private static KeyboardApiController? _instance;
        public static KeyboardApiController Default => _instance ??= new KeyboardApiController();

        private KeyboardApiController() { }

        /// <summary>
        /// Симуляція натискання клавіші клавіатури
        /// </summary>
        /// <param name="keyCode">Код клавіші, можна знайти в класі KeyboardKeys</param>
        /// <returns>
        /// Код результату:
        /// 1 - операція успішно завершена.
        /// 0 - сталася помилка.
        /// </returns>
        public async Task<int> SimulateKeyPressing(byte keyCode)
        {
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    keybd_event(keyCode, 0, 0, nuint.Zero);
                    keybd_event(keyCode, 0, KEYEVENTF_KEYUP, nuint.Zero);
                    Logger.AddLog($"Симуляція натискання клавіші: Клавіша {keyCode}");
                    OnKeyPressed?.Invoke(this, EventArgs.Empty);
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