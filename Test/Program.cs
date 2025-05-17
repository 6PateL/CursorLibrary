using System;
using System.Text;
using System.Threading.Tasks;
using CursorLibrary.Controllers;
using CursorLibrary.Models;
using CursorLibrary.SD;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var cursorApi = CursorApiController.Default;

        cursorApi.OnMouseMoved += CursorApi_OnMouseMoved;
        cursorApi.OnLeftMouseClicked += CursorApi_OnLeftMouseClicked;
        cursorApi.OnRightMouseClicked += CursorApi_OnRightMouseClicked;
        cursorApi.OnMousePulledUp += CursorApi_OnMousePulledUp;
        cursorApi.OnMousePulledDown += CursorApi_OnMousePulledDown;
        cursorApi.OnKeyPressed += CursorApi_OnKeyPressed;

        Console.WriteLine("Натискання клавіші...");
        await cursorApi.SimulateKeyPressing(KeyboardKeys.A);

        if (Console.ReadKey(true).Key == ConsoleKey.A)
            Console.WriteLine("Клавішу натиснуто!");

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Переміщення миші...");
        await cursorApi.SetCursorPosition(0, 0);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Клік лівою кнопкою миші...");
        await cursorApi.SimulateMouseClick(MouseType.LEFTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Клік правою кнопкою миші...");
        await cursorApi.SimulateMouseClick(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Натискання лівої кнопки миші (Pull Down)...");
        await cursorApi.SimulateMousePullDown(MouseType.LEFTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Відпускання лівої кнопки миші (Pull Up)...");
        await cursorApi.SimulateMousePullUp(MouseType.LEFTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Натискання правої кнопки миші (Pull Down)...");
        await cursorApi.SimulateMousePullDown(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Відпускання правої кнопки миші (Pull Up)...");
        await cursorApi.SimulateMousePullUp(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("Готово!");
        Console.ReadKey();
    }

    private static void CursorApi_OnKeyPressed(object? sender, EventArgs e)
    {
        Console.WriteLine("Подію натискання клавіші викликано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnRightMouseClicked(object? sender, MouseInfoModel e)
    {
        Console.WriteLine("Подію кліку правою кнопкою миші викликано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnMouseMoved(object? sender, MouseInfoModel e)
    {
        Console.WriteLine("Подію руху миші викликано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnLeftMouseClicked(object? sender, MouseInfoModel e)
    {
        Console.WriteLine("Подію кліку лівою кнопкою миші викликано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnMousePulledUp(object? sender, MouseInfoModel e)
    {
        Console.WriteLine($"Подію відпускання кнопки миші викликано! Тип: {e.MouseType}");
        Console.WriteLine();
    }

    private static void CursorApi_OnMousePulledDown(object? sender, MouseInfoModel e)
    {
        Console.WriteLine($"Подію натискання кнопки миші викликано! Тип: {e.MouseType}");
        Console.WriteLine();
    }
}