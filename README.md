
# CursorLibrary

A C# library for simulating and capturing mouse and keyboard actions on Windows. It enables programmatic control over the cursor (movement, clicks, button press/release), keyboard key presses, and real-time detection of user input. Ideal for automation, UI testing, or building tools to interact with the operating system.

## Features

-   **Mouse Simulation**:
    -   Move the cursor to specified screen coordinates.
    -   Simulate left and right mouse clicks.
    -   Simulate mouse button press (PullDown) and release (PullUp).
-   **Keyboard Simulation**:
    -   Simulate key presses using key codes (defined in `KeyboardKeys`).
-   **Input Capture**:
    -   Detect user mouse button presses (left/right) and keyboard key presses in real-time.
-   **Events**:
    -   Event handlers for simulation actions (mouse movement, clicks, button press/release, key presses).
    -   Event handlers for captured user input (mouse button presses, key presses).
-   **Logging**:
    -   All operations are logged via the global `Logger` class, with support for exporting logs to a file.
-   **Asynchronous Operations**:
    -   Uses `SemaphoreSlim` for thread-safe operations in multi-threaded environments.
-   **Singleton Pattern**:
    -   Controllers (`CursorApiController`, `KeyboardApiController`, `InputHookController`) are implemented as singletons.
-   **Error Handling**:
    -   Custom exceptions (`CursorApiException` for mouse operations, `KeyboardApiException` for keyboard operations).

## Installation

1.  Clone the repository or add the project to your solution.
2.  Ensure you have a compatible .NET Framework or .NET Core version installed.
3.  Add dependencies for the following namespaces:
    -   `System.Drawing` (for coordinate handling).
    -   `System.Runtime.InteropServices` (for Windows API calls).
4.  Build the project.

## Usage

The library consists of three main controller classes:

-   `CursorApiController`: Handles mouse simulation.
-   `KeyboardApiController`: Handles keyboard simulation.
-   `InputHookController`: Captures user mouse and keyboard input.

All controllers are accessible via their static `Default` property.

### Example

```csharp
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
        // Set console encoding
        Console.OutputEncoding = Encoding.UTF8;

        // Subscribe to process exit for log export
        AppDomain.CurrentDomain.ProcessExit += (s, e) => 
        {
            Console.WriteLine("Saving logs...");
            Logger.ExportLogs("logs.txt");
        };

        // Initialize controllers
        var cursorApi = CursorApiController.Default;
        var keyboardApi = KeyboardApiController.Default;
        var inputHook = InputHookController.Default;

        // Subscribe to simulation events
        cursorApi.OnMouseMoved += (s, e) => Console.WriteLine($"Cursor moved: X={e.PositionX}, Y={e.PositionY}");
        cursorApi.OnLeftMouseClicked += (s, e) => Console.WriteLine("Left mouse clicked!");
        cursorApi.OnRightMouseClicked += (s, e) => Console.WriteLine("Right mouse clicked!");
        cursorApi.OnMousePulledUp += (s, e) => Console.WriteLine($"Mouse button released: {e.MouseType}");
        cursorApi.OnMousePulledDown += (s, e) => Console.WriteLine($"Mouse button pressed: {e.MouseType}");
        keyboardApi.OnKeyPressed += (s, e) => Console.WriteLine("Key pressed (simulation)!");

        // Subscribe to input capture events
        inputHook.OnMouseButtonPressed += (s, e) => Console.WriteLine($"User pressed mouse button: {e.MouseType} at X={e.PositionX}, Y={e.PositionY}");
        inputHook.OnKeyPressed += (s, keyCode) => Console.WriteLine($"User pressed key: {keyCode}");

        // Start input capture
        Console.WriteLine("Starting mouse and keyboard input capture...");
        await inputHook.StartHooksAsync();

        // Simulate actions
        Console.WriteLine("Simulating key press (A)...");
        await keyboardApi.SimulateKeyPressing(KeyboardKeys.A);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Moving cursor to (0, 0)...");
        await cursorApi.SetCursorPosition(0, 0);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Simulating left mouse click...");
        await cursorApi.SimulateMouseClick(MouseType.LEFTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Simulating right mouse button press...");
        await cursorApi.SimulateMousePullDown(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Simulating right mouse button release...");
        await cursorApi.SimulateMousePullUp(MouseType.RIGHTMOUSE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        // Wait for user input
        Console.WriteLine("Test mouse and keyboard input. Press Enter to stop...");
        Console.ReadLine();

        // Stop input capture
        Console.WriteLine("Stopping input capture...");
        await inputHook.StopHooksAsync();

        Console.WriteLine("Demo completed! Press any key to exit.");
        Console.ReadKey();
    }
}

```

### Key Methods

#### CursorApiController

-   `Task<int> SetCursorPosition(int x, int y)`: Moves the cursor to the specified (x, y) coordinates.
-   `Task<int> SimulateMouseClick(MouseType mouseType)`: Simulates a mouse click (left or right button).
-   `Task<int> SimulateMousePullDown(MouseType mouseType)`: Simulates pressing a mouse button.
-   `Task<int> SimulateMousePullUp(MouseType mouseType)`: Simulates releasing a mouse button.

#### KeyboardApiController

-   `Task<int> SimulateKeyPressing(byte keyCode)`: Simulates pressing and releasing a key by its code.

#### InputHookController

-   `Task<int> StartHooksAsync()`: Starts capturing mouse and keyboard input.
-   `Task<int> StopHooksAsync()`: Stops capturing input.

### Return Values

All methods return `Task<int>`:

-   `1`: Operation successful.
-   `0`: Error occurred (details in logs or console).

### Events

-   `CursorApiController`:
    -   `OnMouseMoved`: Triggered when the cursor moves (simulated).
    -   `OnLeftMouseClicked`, `OnRightMouseClicked`: Triggered on simulated left/right mouse clicks.
    -   `OnMousePulledUp`, `OnMousePulledDown`: Triggered on simulated mouse button release/press.
-   `KeyboardApiController`:
    -   `OnKeyPressed`: Triggered when a key is simulated.
-   `InputHookController`:
    -   `OnMouseButtonPressed`: Triggered when a user presses a mouse button (returns `MouseInfoModel`).
    -   `OnKeyPressed`: Triggered when a user presses a key (returns key code as `byte`).

### Error Handling

-   `CursorApiException`: Thrown for errors in mouse-related operations.
-   `KeyboardApiException`: Thrown for errors in keyboard-related operations.
-   Both exceptions include a message and an inner exception for detailed diagnostics.

## Project Structure

-   **Controllers**:
    -   `CursorApiController.cs`: Handles mouse simulation.
    -   `KeyboardApiController.cs`: Handles keyboard simulation.
    -   `InputHookController.cs`: Captures user mouse and keyboard input.
-   **Models**:
    -   `MouseInfoModel`: Stores cursor position and mouse button type.
-   **SD**:
    -   `MouseType`: Enum for mouse button types (`LEFTMOUSE`, `RIGHTMOUSE`).
    -   `KeyboardKeys`: Key codes for keyboard simulation.
-   **Utilities**:
    -   `Logger`: Global class for logging operations and exporting logs to a file.
-   **Exceptions**:
    -   `CursorApiException`: Exception for mouse operation errors.
    -   `KeyboardApiException`: Exception for keyboard operation errors.

## Requirements

-   **OS**: Windows (due to reliance on `user32.dll` and `kernel32.dll`).
-   **Framework**: .NET Framework or .NET Core (version depends on your project).
-   **Console Encoding**: Set `Console.OutputEncoding = Encoding.UTF8` for proper output in non-English languages.
-   **Permissions**: Input capture may require elevated privileges on some systems.

## Limitations

-   Works only on Windows due to `user32.dll` and `kernel32.dll` dependencies.
-   Multi-threaded operations are synchronized via `SemaphoreSlim`, but require careful handling in complex scenarios.
-   Low-level hooks may impact performance and should be stopped when not needed.
-   Logging relies on the global `Logger` class, which must be implemented in your project.

## Testing

The library includes a test program (`Program.cs`) that demonstrates:

-   Mouse and keyboard simulation.
-   Real-time capture of user mouse and keyboard input.
-   Log export on program exit.

Run the test program to verify functionality.

## Logging

All actions (successful or failed) are logged via the `Logger` class. Logs can be exported to a file using `Logger.ExportLogs(path)`.

## Contributing

1.  Fork the repository.
2.  Create a feature branch (`git checkout -b feature/new-feature`).
3.  Commit your changes (`git commit -m 'Added new feature'`).
4.  Push to the branch (`git push origin feature/new-feature`).
5.  Create a Pull Request.

## License

This project is licensed under the MIT License.

## Contact

For questions or suggestions, please open an issue in the repository.
