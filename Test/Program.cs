using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        cursorApi.OnMouseMoved += (s, e) => Console.WriteLine($"Курсор переміщено: X={e.PositionX}, Y={e.PositionY}");
        cursorApi.OnLeftMouseClicked += (s, e) => Console.WriteLine("Клік лівою кнопкою виконано!");
        cursorApi.OnRightMouseClicked += (s, e) => Console.WriteLine("Клік правою кнопкою виконано!");
        cursorApi.OnMousePulledUp += (s, e) => Console.WriteLine($"Кнопку миші відпущено: {e.MouseType}");
        cursorApi.OnMousePulledDown += (s, e) => Console.WriteLine($"Кнопку миші натиснуто: {e.MouseType}");
        keyboardApi.OnKeyPressed += (s, e) => Console.WriteLine("Клавішу натиснуто!");

        Console.WriteLine("Симуляція натискання клавіші A...");
        await keyboardApi.SimulateKeyPressing(KeyboardKeys.A);

        if (Console.ReadKey(true).Key == ConsoleKey.A)
        {
            Console.WriteLine("кнопку A натиснуто");
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

        Console.WriteLine("Демонстрація завершена! Натисніть будь-яку клавішу для виходу.");
        Console.ReadKey();
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        Console.WriteLine("Логи збережено");
        Logger.ExportLogs("C:\\Users\\максим\\Desktop"); 
    }
}