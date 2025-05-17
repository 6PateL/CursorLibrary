<h1 id="cursorlibrary">CursorLibrary</h1>
<p>A C# library for simulating mouse and keyboard actions on Windows. It enables programmatic control over the cursor (movement, clicks, button press/release) and keyboard key presses. Ideal for automation, UI testing, or building tools to interact with the operating system.</p>
<h2 id="features">Features</h2>
<ul>
<li><strong>Mouse Control</strong>:
<ul>
<li>Move the cursor to specified screen coordinates.</li>
<li>Simulate left and right mouse clicks.</li>
<li>Simulate mouse button press (PullDown) and release (PullUp).</li>
</ul>
</li>
<li><strong>Keyboard Control</strong>:
<ul>
<li>Simulate key presses using key codes (defined in <code>KeyboardKeys</code>).</li>
</ul>
</li>
<li><strong>Events</strong>:
<ul>
<li>Event handlers for tracking actions (mouse movement, clicks, button press/release, key presses).</li>
</ul>
</li>
<li><strong>Logging</strong>:
<ul>
<li>All operations are logged via the global <code>Logger</code> class for debugging.</li>
</ul>
</li>
<li><strong>Asynchronous Operations</strong>:
<ul>
<li>Uses <code>SemaphoreSlim</code> for thread-safe operations in multi-threaded environments.</li>
</ul>
</li>
<li><strong>Singleton Pattern</strong>:
<ul>
<li>Both controllers (<code>CursorApiController</code> and <code>KeyboardApiController</code>) are implemented as singletons for easy access.</li>
</ul>
</li>
<li><strong>Error Handling</strong>:
<ul>
<li>Custom exceptions (<code>CursorApiException</code> for mouse operations, <code>KeyboardApiException</code> for keyboard operations).</li>
</ul>
</li>
</ul>
<h2 id="installation">Installation</h2>
<ol>
<li>Clone the repository or add the project to your solution.</li>
<li>Ensure you have a compatible .NET Framework or .NET Core version installed.</li>
<li>Add dependencies for the following namespaces:
<ul>
<li><code>System.Drawing</code> (for coordinate handling).</li>
<li><code>System.Runtime.InteropServices</code> (for Windows API calls).</li>
</ul>
</li>
<li>Build the project.</li>
</ol>
<h2 id="usage">Usage</h2>
<p>The library consists of two main controller classes: <code>CursorApiController</code> (for mouse operations) and <code>KeyboardApiController</code> (for keyboard operations). Both are accessible via their static <code>Default</code> property.</p>
<h3 id="example">Example</h3>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token keyword">using</span> System<span class="token punctuation">;</span>
<span class="token keyword">using</span> System<span class="token punctuation">.</span>Text<span class="token punctuation">;</span>
<span class="token keyword">using</span> System<span class="token punctuation">.</span>Threading<span class="token punctuation">.</span>Tasks<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>Controllers<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>Models<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>SD<span class="token punctuation">;</span>

<span class="token keyword">class</span> <span class="token class-name">Program</span>
<span class="token punctuation">{</span>
    <span class="token keyword">static</span> <span class="token keyword">async</span> Task <span class="token function">Main</span><span class="token punctuation">(</span><span class="token keyword">string</span><span class="token punctuation">[</span><span class="token punctuation">]</span> args<span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        <span class="token comment">// Set console encoding for proper output</span>
        Console<span class="token punctuation">.</span>OutputEncoding <span class="token operator">=</span> Encoding<span class="token punctuation">.</span>UTF8<span class="token punctuation">;</span>

        <span class="token comment">// Initialize controllers</span>
        <span class="token keyword">var</span> cursorApi <span class="token operator">=</span> CursorApiController<span class="token punctuation">.</span>Default<span class="token punctuation">;</span>
        <span class="token keyword">var</span> keyboardApi <span class="token operator">=</span> KeyboardApiController<span class="token punctuation">.</span>Default<span class="token punctuation">;</span>

        <span class="token comment">// Subscribe to events</span>
        cursorApi<span class="token punctuation">.</span>OnMouseMoved <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span>$<span class="token string">"Cursor moved: X={e.PositionX}, Y={e.PositionY}"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnLeftMouseClicked <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Left mouse clicked!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnRightMouseClicked <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Right mouse clicked!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnMousePulledUp <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span>$<span class="token string">"Mouse button released: {e.MouseType}"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnMousePulledDown <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span>$<span class="token string">"Mouse button pressed: {e.MouseType}"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        keyboardApi<span class="token punctuation">.</span>OnKeyPressed <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Key pressed!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token comment">// Simulate actions</span>
        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating key press (A)..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> keyboardApi<span class="token punctuation">.</span><span class="token function">SimulateKeyPressing</span><span class="token punctuation">(</span>KeyboardKeys<span class="token punctuation">.</span>A<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Moving cursor to (100, 100)..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SetCursorPosition</span><span class="token punctuation">(</span><span class="token number">100</span><span class="token punctuation">,</span> <span class="token number">100</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating left mouse click..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMouseClick</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>LEFTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating right mouse button press..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMousePullDown</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>RIGHTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating right mouse button release..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMousePullUp</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>RIGHTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Demo completed! Press any key to exit."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        Console<span class="token punctuation">.</span><span class="token function">ReadKey</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre>
<h3 id="key-methods">Key Methods</h3>
<h4 id="cursorapicontroller">CursorApiController</h4>
<ul>
<li><code>Task&lt;int&gt; SetCursorPosition(int x, int y)</code>: Moves the cursor to the specified (x, y) coordinates.</li>
<li><code>Task&lt;int&gt; SimulateMouseClick(MouseType mouseType)</code>: Simulates a mouse click (left or right button).</li>
<li><code>Task&lt;int&gt; SimulateMousePullDown(MouseType mouseType)</code>: Simulates pressing a mouse button.</li>
<li><code>Task&lt;int&gt; SimulateMousePullUp(MouseType mouseType)</code>: Simulates releasing a mouse button.</li>
</ul>
<h4 id="keyboardapicontroller">KeyboardApiController</h4>
<ul>
<li><code>Task&lt;int&gt; SimulateKeyPressing(byte keyCode)</code>: Simulates pressing and releasing a key by its code.</li>
</ul>
<h3 id="return-values">Return Values</h3>
<p>All methods return <code>Task&lt;int&gt;</code>:</p>
<ul>
<li><code>1</code>: Operation successful.</li>
<li><code>0</code>: Error occurred (details in logs or console).</li>
</ul>
<h3 id="events">Events</h3>
<ul>
<li><code>CursorApiController</code>:
<ul>
<li><code>OnMouseMoved</code>: Triggered when the cursor moves.</li>
<li><code>OnLeftMouseClicked</code>, <code>OnRightMouseClicked</code>: Triggered on left/right mouse clicks.</li>
<li><code>OnMousePulledUp</code>, <code>OnMousePulledDown</code>: Triggered on mouse button release/press.</li>
</ul>
</li>
<li><code>KeyboardApiController</code>:
<ul>
<li><code>OnKeyPressed</code>: Triggered when a key is simulated.</li>
</ul>
</li>
</ul>
<h3 id="error-handling">Error Handling</h3>
<ul>
<li><code>CursorApiException</code>: Thrown for errors in mouse-related operations.</li>
<li><code>KeyboardApiException</code>: Thrown for errors in keyboard-related operations.</li>
<li>Both exceptions include a message and an inner exception for detailed diagnostics.</li>
</ul>
<h2 id="project-structure">Project Structure</h2>
<ul>
<li><strong>Controllers</strong>:
<ul>
<li><code>CursorApiController.cs</code>: Handles mouse operations.</li>
<li><code>KeyboardApiController.cs</code>: Handles keyboard operations.</li>
</ul>
</li>
<li><strong>Models</strong>:
<ul>
<li><code>MouseInfoModel</code>: Stores cursor position and mouse button type.</li>
</ul>
</li>
<li><strong>SD</strong>:
<ul>
<li><code>MouseType</code>: Enum for mouse button types (<code>LEFTMOUSE</code>, <code>RIGHTMOUSE</code>).</li>
<li><code>KeyboardKeys</code>: Key codes for keyboard simulation.</li>
</ul>
</li>
<li><strong>Utilities</strong>:
<ul>
<li><code>Logger</code>: Global class for logging operations.</li>
</ul>
</li>
<li><strong>Exceptions</strong>:
<ul>
<li><code>CursorApiException</code>: Exception for mouse operation errors.</li>
<li><code>KeyboardApiException</code>: Exception for keyboard operation errors.</li>
</ul>
</li>
</ul>
<h2 id="requirements">Requirements</h2>
<ul>
<li><strong>OS</strong>: Windows (due to reliance on <code>user32.dll</code>).</li>
<li><strong>Framework</strong>: .NET Framework or .NET Core (version depends on your project).</li>
<li><strong>Console Encoding</strong>: Set <code>Console.OutputEncoding = Encoding.UTF8</code> for proper output in non-English languages.</li>
</ul>
<h2 id="limitations">Limitations</h2>
<ul>
<li>Works only on Windows due to <code>user32.dll</code> dependency.</li>
<li>Multi-threaded operations are synchronized via <code>SemaphoreSlim</code>, but require careful handling in complex scenarios.</li>
<li>Logging relies on the global <code>Logger</code> class, which must be implemented in your project.</li>
</ul>
<h2 id="testing">Testing</h2>
<p>The library includes a test program (<code>Program.cs</code>) that demonstrates all core functionality:</p>
<ul>
<li>Key presses.</li>
<li>Cursor movement.</li>
<li>Mouse clicks.</li>
<li>Mouse button press/release.</li>
</ul>
<p>Run the test program to verify the library’s functionality.</p>
<h2 id="logging">Logging</h2>
<p>All actions (successful or failed) are logged via the <code>Logger</code> class. Check logs for debugging.</p>
<h2 id="contributing">Contributing</h2>
<ol>
<li>Fork the repository.</li>
<li>Create a feature branch (<code>git checkout -b feature/new-feature</code>).</li>
<li>Commit your changes (<code>git commit -m 'Added new feature'</code>).</li>
<li>Push to the branch (<code>git push origin feature/new-feature</code>).</li>
<li>Create a Pull Request.</li>
</ol>
<h2 id="license">License</h2>
<p>This project is licensed under the MIT License (or specify another if needed).</p>
<h2 id="contact">Contact</h2>
<p>For questions or suggestions, please open an issue in the repository.# CursorLibrary</p>
<p>A C# library for simulating mouse and keyboard actions on Windows. It enables programmatic control over the cursor (movement, clicks, button press/release) and keyboard key presses. Ideal for automation, UI testing, or building tools to interact with the operating system.</p>
<h2 id="features-1">Features</h2>
<ul>
<li><strong>Mouse Control</strong>:
<ul>
<li>Move the cursor to specified screen coordinates.</li>
<li>Simulate left and right mouse clicks.</li>
<li>Simulate mouse button press (PullDown) and release (PullUp).</li>
</ul>
</li>
<li><strong>Keyboard Control</strong>:
<ul>
<li>Simulate key presses using key codes (defined in <code>KeyboardKeys</code>).</li>
</ul>
</li>
<li><strong>Events</strong>:
<ul>
<li>Event handlers for tracking actions (mouse movement, clicks, button press/release, key presses).</li>
</ul>
</li>
<li><strong>Logging</strong>:
<ul>
<li>All operations are logged via the global <code>Logger</code> class for debugging.</li>
</ul>
</li>
<li><strong>Asynchronous Operations</strong>:
<ul>
<li>Uses <code>SemaphoreSlim</code> for thread-safe operations in multi-threaded environments.</li>
</ul>
</li>
<li><strong>Singleton Pattern</strong>:
<ul>
<li>Both controllers (<code>CursorApiController</code> and <code>KeyboardApiController</code>) are implemented as singletons for easy access.</li>
</ul>
</li>
<li><strong>Error Handling</strong>:
<ul>
<li>Custom exceptions (<code>CursorApiException</code> for mouse operations, <code>KeyboardApiException</code> for keyboard operations).</li>
</ul>
</li>
</ul>
<h2 id="installation-1">Installation</h2>
<ol>
<li>Clone the repository or add the project to your solution.</li>
<li>Ensure you have a compatible .NET Framework or .NET Core version installed.</li>
<li>Add dependencies for the following namespaces:
<ul>
<li><code>System.Drawing</code> (for coordinate handling).</li>
<li><code>System.Runtime.InteropServices</code> (for Windows API calls).</li>
</ul>
</li>
<li>Build the project.</li>
</ol>
<h2 id="usage-1">Usage</h2>
<p>The library consists of two main controller classes: <code>CursorApiController</code> (for mouse operations) and <code>KeyboardApiController</code> (for keyboard operations). Both are accessible via their static <code>Default</code> property.</p>
<h3 id="example-1">Example</h3>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token keyword">using</span> System<span class="token punctuation">;</span>
<span class="token keyword">using</span> System<span class="token punctuation">.</span>Text<span class="token punctuation">;</span>
<span class="token keyword">using</span> System<span class="token punctuation">.</span>Threading<span class="token punctuation">.</span>Tasks<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>Controllers<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>Models<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>SD<span class="token punctuation">;</span>

<span class="token keyword">class</span> <span class="token class-name">Program</span>
<span class="token punctuation">{</span>
    <span class="token keyword">static</span> <span class="token keyword">async</span> Task <span class="token function">Main</span><span class="token punctuation">(</span><span class="token keyword">string</span><span class="token punctuation">[</span><span class="token punctuation">]</span> args<span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        <span class="token comment">// Set console encoding for proper output</span>
        Console<span class="token punctuation">.</span>OutputEncoding <span class="token operator">=</span> Encoding<span class="token punctuation">.</span>UTF8<span class="token punctuation">;</span>

        <span class="token comment">// Initialize controllers</span>
        <span class="token keyword">var</span> cursorApi <span class="token operator">=</span> CursorApiController<span class="token punctuation">.</span>Default<span class="token punctuation">;</span>
        <span class="token keyword">var</span> keyboardApi <span class="token operator">=</span> KeyboardApiController<span class="token punctuation">.</span>Default<span class="token punctuation">;</span>

        <span class="token comment">// Subscribe to events</span>
        cursorApi<span class="token punctuation">.</span>OnMouseMoved <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span>$<span class="token string">"Cursor moved: X={e.PositionX}, Y={e.PositionY}"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnLeftMouseClicked <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Left mouse clicked!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnRightMouseClicked <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Right mouse clicked!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnMousePulledUp <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span>$<span class="token string">"Mouse button released: {e.MouseType}"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnMousePulledDown <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span>$<span class="token string">"Mouse button pressed: {e.MouseType}"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        keyboardApi<span class="token punctuation">.</span>OnKeyPressed <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Key pressed!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token comment">// Simulate actions</span>
        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating key press (A)..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> keyboardApi<span class="token punctuation">.</span><span class="token function">SimulateKeyPressing</span><span class="token punctuation">(</span>KeyboardKeys<span class="token punctuation">.</span>A<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Moving cursor to (100, 100)..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SetCursorPosition</span><span class="token punctuation">(</span><span class="token number">100</span><span class="token punctuation">,</span> <span class="token number">100</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating left mouse click..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMouseClick</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>LEFTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating right mouse button press..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMousePullDown</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>RIGHTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Simulating right mouse button release..."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMousePullUp</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>RIGHTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token keyword">await</span> Task<span class="token punctuation">.</span><span class="token function">Delay</span><span class="token punctuation">(</span>TimeSpan<span class="token punctuation">.</span><span class="token function">FromSeconds</span><span class="token punctuation">(</span><span class="token number">2</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Demo completed! Press any key to exit."</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        Console<span class="token punctuation">.</span><span class="token function">ReadKey</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre>
<h3 id="key-methods-1">Key Methods</h3>
<h4 id="cursorapicontroller-1">CursorApiController</h4>
<ul>
<li><code>Task&lt;int&gt; SetCursorPosition(int x, int y)</code>: Moves the cursor to the specified (x, y) coordinates.</li>
<li><code>Task&lt;int&gt; SimulateMouseClick(MouseType mouseType)</code>: Simulates a mouse click (left or right button).</li>
<li><code>Task&lt;int&gt; SimulateMousePullDown(MouseType mouseType)</code>: Simulates pressing a mouse button.</li>
<li><code>Task&lt;int&gt; SimulateMousePullUp(MouseType mouseType)</code>: Simulates releasing a mouse button.</li>
</ul>
<h4 id="keyboardapicontroller-1">KeyboardApiController</h4>
<ul>
<li><code>Task&lt;int&gt; SimulateKeyPressing(byte keyCode)</code>: Simulates pressing and releasing a key by its code.</li>
</ul>
<h3 id="return-values-1">Return Values</h3>
<p>All methods return <code>Task&lt;int&gt;</code>:</p>
<ul>
<li><code>1</code>: Operation successful.</li>
<li><code>0</code>: Error occurred (details in logs or console).</li>
</ul>
<h3 id="events-1">Events</h3>
<ul>
<li><code>CursorApiController</code>:
<ul>
<li><code>OnMouseMoved</code>: Triggered when the cursor moves.</li>
<li><code>OnLeftMouseClicked</code>, <code>OnRightMouseClicked</code>: Triggered on left/right mouse clicks.</li>
<li><code>OnMousePulledUp</code>, <code>OnMousePulledDown</code>: Triggered on mouse button release/press.</li>
</ul>
</li>
<li><code>KeyboardApiController</code>:
<ul>
<li><code>OnKeyPressed</code>: Triggered when a key is simulated.</li>
</ul>
</li>
</ul>
<h3 id="error-handling-1">Error Handling</h3>
<ul>
<li><code>CursorApiException</code>: Thrown for errors in mouse-related operations.</li>
<li><code>KeyboardApiException</code>: Thrown for errors in keyboard-related operations.</li>
<li>Both exceptions include a message and an inner exception for detailed diagnostics.</li>
</ul>
<h2 id="project-structure-1">Project Structure</h2>
<ul>
<li><strong>Controllers</strong>:
<ul>
<li><code>CursorApiController.cs</code>: Handles mouse operations.</li>
<li><code>KeyboardApiController.cs</code>: Handles keyboard operations.</li>
</ul>
</li>
<li><strong>Models</strong>:
<ul>
<li><code>MouseInfoModel</code>: Stores cursor position and mouse button type.</li>
</ul>
</li>
<li><strong>SD</strong>:
<ul>
<li><code>MouseType</code>: Enum for mouse button types (<code>LEFTMOUSE</code>, <code>RIGHTMOUSE</code>).</li>
<li><code>KeyboardKeys</code>: Key codes for keyboard simulation.</li>
</ul>
</li>
<li><strong>Utilities</strong>:
<ul>
<li><code>Logger</code>: Global class for logging operations.</li>
</ul>
</li>
<li><strong>Exceptions</strong>:
<ul>
<li><code>CursorApiException</code>: Exception for mouse operation errors.</li>
<li><code>KeyboardApiException</code>: Exception for keyboard operation errors.</li>
</ul>
</li>
</ul>
<h2 id="requirements-1">Requirements</h2>
<ul>
<li><strong>OS</strong>: Windows (due to reliance on <code>user32.dll</code>).</li>
<li><strong>Framework</strong>: .NET Framework or .NET Core (version depends on your project).</li>
<li><strong>Console Encoding</strong>: Set <code>Console.OutputEncoding = Encoding.UTF8</code> for proper output in non-English languages.</li>
</ul>
<h2 id="limitations-1">Limitations</h2>
<ul>
<li>Works only on Windows due to <code>user32.dll</code> dependency.</li>
<li>Multi-threaded operations are synchronized via <code>SemaphoreSlim</code>, but require careful handling in complex scenarios.</li>
<li>Logging relies on the global <code>Logger</code> class, which must be implemented in your project.</li>
</ul>
<h2 id="testing-1">Testing</h2>
<p>The library includes a test program (<code>Program.cs</code>) that demonstrates all core functionality:</p>
<ul>
<li>Key presses.</li>
<li>Cursor movement.</li>
<li>Mouse clicks.</li>
<li>Mouse button press/release.</li>
</ul>
<p>Run the test program to verify the library’s functionality.</p>
<h2 id="logging-1">Logging</h2>
<p>All actions (successful or failed) are logged via the <code>Logger</code> class. Check logs for debugging.</p>
<h2 id="contributing-1">Contributing</h2>
<ol>
<li>Fork the repository.</li>
<li>Create a feature branch (<code>git checkout -b feature/new-feature</code>).</li>
<li>Commit your changes (<code>git commit -m 'Added new feature'</code>).</li>
<li>Push to the branch (<code>git push origin feature/new-feature</code>).</li>
<li>Create a Pull Request.</li>
</ol>
<h2 id="license-1">License</h2>
<p>This project is licensed under the MIT License (or specify another if needed).</p>
<h2 id="contact-1">Contact</h2>
<p>For questions or suggestions, please open an issue in the repository.</p>

