using System;
using System.Text;
using System.Threading.Tasks;
using CursorLibrary.Controllers;
using CursorLibrary.Models;
using CursorLibrary.SD;
using CursorLibrary.Utilities;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

        var cursorApi = CursorApiController.Default;
        var keyboardApi = KeyboardApiController.Default;
        var inputHook = InputHookController.Default;

        cursorApi.OnMouseMoved += (s, e) => Console.WriteLine($"Курсор переміщено: X={e.PositionX}, Y={e.PositionY}");
        cursorApi.OnLeftMouseClicked += (s, e) => Console.WriteLine("Клік лівою кнопкою виконано!");
        cursorApi.OnRightMouseClicked += (s, e) => Console.WriteLine("Клік правою кнопкою виконано!");
        cursorApi.OnMousePulledUp += (s, e) => Console.WriteLine($"Кнопку миші відпущено: {e.MouseType}");
        cursorApi.OnMousePulledDown += (s, e) => Console.WriteLine($"Кнопку миші натиснуто: {e.MouseType}");
        keyboardApi.OnKeyPressed += (s, e) => Console.WriteLine("Клавішу натиснуто (симуляція)!");

        inputHook.OnMouseButtonPressed += (s, e) => Console.WriteLine($"Користувач натиснув кнопку миші: {e.MouseType} на X={e.PositionX}, Y={e.PositionY}");
        inputHook.OnKeyPressed += (s, keyCode) => Console.WriteLine($"Користувач натиснув клавішу: {keyCode}");

        Console.WriteLine("Запуск перехоплення подій миші та клавіатури...");
        await inputHook.StartHooksAsync();

        Console.WriteLine("Симуляція натискання клавіші A...");
        await keyboardApi.SimulateKeyPressing(KeyboardKeys.A);

        if (Console.ReadKey(true).Key == ConsoleKey.A)
        {
            Console.WriteLine("Клавішу A натиснуто");
        }

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Переміщення курсора до позиції (0, 0)...");
        await cursorApi.SetCursorPosition(0, 0);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Симуляція кліку лівою кнопкою...");
        await cursorApi.SimulateMouseClick(MouseType.LEFTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Симуляція натискання правої кнопки...");
        await cursorApi.SimulateMousePullDown(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Симуляція відпускання правої кнопки...");
        await cursorApi.SimulateMousePullUp(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Тестуйте натискання кнопок миші та клавіш. Натисніть Enter для завершення...");
        Console.ReadLine();

        Console.WriteLine("Зупинка перехоплення подій...");
        await inputHook.StopHooksAsync();

        Console.WriteLine("Демонстрація завершена! Натисніть будь-яку клавішу для виходу.");
        Console.ReadKey();
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        Console.WriteLine("Збереження логів...");
        Logger.ExportLogs("logs.txt"); 
    }
}