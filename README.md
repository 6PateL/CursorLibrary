
# CursorLibrary

A C# library for simulating mouse and keyboard actions on Windows. It enables programmatic control over the cursor (movement, clicks, button press/release) and keyboard key presses. Ideal for automation, UI testing, or building tools to interact with the operating system.

## Features

-   **Mouse Control**:
    -   Move the cursor to specified screen coordinates.
    -   Simulate left and right mouse clicks.
    -   Simulate mouse button press (PullDown) and release (PullUp).
-   **Keyboard Control**:
    -   Simulate key presses using key codes (defined in `KeyboardKeys`).
-   **Events**:
    -   Event handlers for tracking actions (mouse movement, clicks, button press/release, key presses).
-   **Logging**:
    -   All operations are logged via the global `Logger` class for debugging.
-   **Asynchronous Operations**:
    -   Uses `SemaphoreSlim` for thread-safe operations in multi-threaded environments.
-   **Singleton Pattern**:
    -   Both controllers (`CursorApiController` and `KeyboardApiController`) are implemented as singletons for easy access.
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

The library consists of two main controller classes: `CursorApiController` (for mouse operations) and `KeyboardApiController` (for keyboard operations). Both are accessible via their static `Default` property.

### Example

```csharp
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
        // Set console encoding for proper output
        Console.OutputEncoding = Encoding.UTF8;

        // Initialize controllers
        var cursorApi = CursorApiController.Default;
        var keyboardApi = KeyboardApiController.Default;

        // Subscribe to events
        cursorApi.OnMouseMoved += (s, e) => Console.WriteLine($"Cursor moved: X={e.PositionX}, Y={e.PositionY}");
        cursorApi.OnLeftMouseClicked += (s, e) => Console.WriteLine("Left mouse clicked!");
        cursorApi.OnRightMouseClicked += (s, e) => Console.WriteLine("Right mouse clicked!");
        cursorApi.OnMousePulledUp += (s, e) => Console.WriteLine($"Mouse button released: {e.MouseType}");
        cursorApi.OnMousePulledDown += (s, e) => Console.WriteLine($"Mouse button pressed: {e.MouseType}");
        keyboardApi.OnKeyPressed += (s, e) => Console.WriteLine("Key pressed!");

        // Simulate actions
        Console.WriteLine("Simulating key press (A)...");
        await keyboardApi.SimulateKeyPressing(KeyboardKeys.A);

        await Task.Delay(TimeSpan.FromSeconds(2));

        Console.WriteLine("Moving cursor to (100, 100)...");
        await cursorApi.SetCursorPosition(100, 100);

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

### Return Values

All methods return `Task<int>`:

-   `1`: Operation successful.
-   `0`: Error occurred (details in logs or console).

### Events

-   `CursorApiController`:
    -   `OnMouseMoved`: Triggered when the cursor moves.
    -   `OnLeftMouseClicked`, `OnRightMouseClicked`: Triggered on left/right mouse clicks.
    -   `OnMousePulledUp`, `OnMousePulledDown`: Triggered on mouse button release/press.
-   `KeyboardApiController`:
    -   `OnKeyPressed`: Triggered when a key is simulated.

### Error Handling

-   `CursorApiException`: Thrown for errors in mouse-related operations.
-   `KeyboardApiException`: Thrown for errors in keyboard-related operations.
-   Both exceptions include a message and an inner exception for detailed diagnostics.

## Project Structure

-   **Controllers**:
    -   `CursorApiController.cs`: Handles mouse operations.
    -   `KeyboardApiController.cs`: Handles keyboard operations.
-   **Models**:
    -   `MouseInfoModel`: Stores cursor position and mouse button type.
-   **SD**:
    -   `MouseType`: Enum for mouse button types (`LEFTMOUSE`, `RIGHTMOUSE`).
    -   `KeyboardKeys`: Key codes for keyboard simulation.
-   **Utilities**:
    -   `Logger`: Global class for logging operations.
-   **Exceptions**:
    -   `CursorApiException`: Exception for mouse operation errors.
    -   `KeyboardApiException`: Exception for keyboard operation errors.

## Requirements

-   **OS**: Windows (due to reliance on `user32.dll`).
-   **Framework**: .NET Framework or .NET Core (version depends on your project).
-   **Console Encoding**: Set `Console.OutputEncoding = Encoding.UTF8` for proper output in non-English languages.

## Limitations

-   Works only on Windows due to `user32.dll` dependency.
-   Multi-threaded operations are synchronized via `SemaphoreSlim`, but require careful handling in complex scenarios.
-   Logging relies on the global `Logger` class, which must be implemented in your project.

## Testing

The library includes a test program (`Program.cs`) that demonstrates all core functionality:

-   Key presses.
-   Cursor movement.
-   Mouse clicks.
-   Mouse button press/release.

Run the test program to verify the library's functionality.

## Logging

All actions (successful or failed) are logged via the `Logger` class. Check logs for debugging.

## Contributing

1.  Fork the repository.
2.  Create a feature branch (`git checkout -b feature/new-feature`).
3.  Commit your changes (`git commit -m 'Added new feature'`).
4.  Push to the branch (`git push origin feature/new-feature`).
5.  Create a Pull Request.

## License

This project is licensed under the MIT License (or specify another if needed).

## Contact

For questions or suggestions, please open an issue in the repository.# CursorLibrary
