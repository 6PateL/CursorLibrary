using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using CursorLibrary.Controllers;
using CursorLibrary.Models;
using CursorLibrary.SD;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var cursorApi = CursorApiController.Default;

        cursorApi.OnMouseMoved += CursorApi_OnMouseMoved;
        cursorApi.OnLeftMouseClicked += CursorApi_OnLeftMouseClicked;
        cursorApi.OnRightMouseClicked += CursorApi_OnRightMouseClicked;
        cursorApi.OnKeyPressed += CursorApi_OnKeyPressed;

        Console.WriteLine("натискання клавіши");
        await cursorApi.SimulateKeyPressing(KeyboardKeys.A);

        if(Console.ReadKey(true).Key == ConsoleKey.A)
            Console.WriteLine("клавіша натиснута");

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("переміщення мишки");
        await cursorApi.SetCursorPosition(0, 0);

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("ліва кнопка миші");
        await cursorApi.SimulateLeftMouseClick();

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("права кнопка миші");
        await cursorApi.SimulateRightMouseClick();

        await Task.Delay(TimeSpan.FromSeconds(5));

        Console.WriteLine("ВСЬО!");
        Console.ReadKey(); 
    }

    private static void CursorApi_OnKeyPressed(object? sender, EventArgs e)
    {
        Console.WriteLine("івент при натисканні визвано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnRightMouseClicked(object? sender, MouseInfoModel e)
    {
        Console.WriteLine("івент натискання правої кнопки визвано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnMouseMoved(object? sender, MouseInfoModel e)
    {
        Console.WriteLine("івент руху миши визвано!");
        Console.WriteLine();
    }

    private static void CursorApi_OnLeftMouseClicked(object? sender, MouseInfoModel e)
    {
        Console.WriteLine("івент натискання лівої кнопки визвано!");
        Console.WriteLine();
    }
}