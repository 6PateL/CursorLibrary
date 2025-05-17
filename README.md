---


---

<h1 id="cursorlibrary">CursorLibrary</h1>
<p>Бібліотека для симуляції дій миші та клавіатури в Windows за допомогою C#. Дозволяє програмно керувати курсором (переміщення, кліки, натискання/відпускання кнопок) і симулювати натискання клавіш. Підходить для автоматизації, тестування інтерфейсів або створення інструментів для взаємодії з операційною системою.</p>
<h2 id="основні-можливості">Основні можливості</h2>
<ul>
<li><strong>Керування мишею</strong>:
<ul>
<li>Переміщення курсора до заданих координат на екрані.</li>
<li>Симуляція кліків лівою та правою кнопками миші.</li>
<li>Симуляція натискання та відпускання кнопок миші (PullDown/PullUp).</li>
</ul>
</li>
<li><strong>Керування клавіатурою</strong>:
<ul>
<li>Симуляція натискання клавіш за їхніми кодами (з класу <code>KeyboardKeys</code>).</li>
</ul>
</li>
<li><strong>Події</strong>:
<ul>
<li>Підтримка подій для відстеження дій (рух миші, кліки, натискання/відпускання кнопок, натискання клавіш).</li>
</ul>
</li>
<li><strong>Логування</strong>:
<ul>
<li>Усі операції записуються в глобальний клас <code>Logger</code> для діагностики.</li>
</ul>
</li>
<li><strong>Асинхронність</strong>:
<ul>
<li>Використовує <code>SemaphoreSlim</code> для безпечної роботи в багатопотоковому середовищі.</li>
</ul>
</li>
<li><strong>Сінглтон</strong>:
<ul>
<li>Обидва контролери (<code>CursorApiController</code> і <code>KeyboardApiController</code>) реалізовані як сінглтони для зручного доступу.</li>
</ul>
</li>
</ul>
<h2 id="встановлення">Встановлення</h2>
<ol>
<li>Склонуйте репозиторій або додайте проект до вашого рішення.</li>
<li>Переконайтеся, що у вас встановлено .NET Framework або .NET Core, сумісний з вашою версією C#.</li>
<li>Додайте залежності для просторів імен:
<ul>
<li><code>System.Drawing</code> (для роботи з координатами).</li>
<li><code>System.Runtime.InteropServices</code> (для викликів Windows API).</li>
</ul>
</li>
<li>Скомпілюйте проект.</li>
</ol>
<h2 id="використання">Використання</h2>
<p>Бібліотека складається з двох основних класів-контролерів: <code>CursorApiController</code> (для миші) і <code>KeyboardApiController</code> (для клавіатури). Обидва доступні через статичну властивість <code>Default</code>.</p>
<h3 id="приклад-використання">Приклад використання</h3>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token keyword">using</span> System<span class="token punctuation">;</span>
<span class="token keyword">using</span> System<span class="token punctuation">.</span>Text<span class="token punctuation">;</span>
<span class="token keyword">using</span> System<span class="token punctuation">.</span>Threading<span class="token punctuation">.</span>Tasks<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>Controllers<span class="token punctuation">;</span>
<span class="token keyword">using</span> CursorLibrary<span class="token punctuation">.</span>SD<span class="token punctuation">;</span>

<span class="token keyword">class</span> <span class="token class-name">Program</span>
<span class="token punctuation">{</span>
    <span class="token keyword">static</span> <span class="token keyword">async</span> Task <span class="token function">Main</span><span class="token punctuation">(</span><span class="token keyword">string</span><span class="token punctuation">[</span><span class="token punctuation">]</span> args<span class="token punctuation">)</span>
    <span class="token punctuation">{</span>
        Console<span class="token punctuation">.</span>OutputEncoding <span class="token operator">=</span> Encoding<span class="token punctuation">.</span>UTF8<span class="token punctuation">;</span>

        <span class="token keyword">var</span> cursorApi <span class="token operator">=</span> CursorApiController<span class="token punctuation">.</span>Default<span class="token punctuation">;</span>
        <span class="token keyword">var</span> keyboardApi <span class="token operator">=</span> KeyboardApiController<span class="token punctuation">.</span>Default<span class="token punctuation">;</span>

        <span class="token comment">// Підписка на події</span>
        cursorApi<span class="token punctuation">.</span>OnMouseMoved <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Курсор переміщено!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        cursorApi<span class="token punctuation">.</span>OnLeftMouseClicked <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Клік лівою кнопкою!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        keyboardApi<span class="token punctuation">.</span>OnKeyPressed <span class="token operator">+</span><span class="token operator">=</span> <span class="token punctuation">(</span>s<span class="token punctuation">,</span> e<span class="token punctuation">)</span> <span class="token operator">=</span><span class="token operator">&gt;</span> Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Клавішу натиснуто!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

        <span class="token comment">// Симуляція дій</span>
        <span class="token keyword">await</span> keyboardApi<span class="token punctuation">.</span><span class="token function">SimulateKeyPressing</span><span class="token punctuation">(</span>KeyboardKeys<span class="token punctuation">.</span>A<span class="token punctuation">)</span><span class="token punctuation">;</span> <span class="token comment">// Натискання клавіші A</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SetCursorPosition</span><span class="token punctuation">(</span><span class="token number">100</span><span class="token punctuation">,</span> <span class="token number">100</span><span class="token punctuation">)</span><span class="token punctuation">;</span> <span class="token comment">// Переміщення курсора</span>
        <span class="token keyword">await</span> cursorApi<span class="token punctuation">.</span><span class="token function">SimulateMouseClick</span><span class="token punctuation">(</span>MouseType<span class="token punctuation">.</span>LEFTMOUSE<span class="token punctuation">)</span><span class="token punctuation">;</span> <span class="token comment">// Клік лівою кнопкою</span>

        Console<span class="token punctuation">.</span><span class="token function">WriteLine</span><span class="token punctuation">(</span><span class="token string">"Готово!"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
        Console<span class="token punctuation">.</span><span class="token function">ReadKey</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
    <span class="token punctuation">}</span>
<span class="token punctuation">}</span>

</code></pre>
<h3 id="основні-методи">Основні методи</h3>
<h4 id="cursorapicontroller">CursorApiController</h4>
<ul>
<li><code>Task&lt;int&gt; SetCursorPosition(int x, int y)</code>: Переміщує курсор до координат (x, y).</li>
<li><code>Task&lt;int&gt; SimulateMouseClick(MouseType mouseType)</code>: Симулює клік миші (ліва або права кнопка).</li>
<li><code>Task&lt;int&gt; SimulateMousePullDown(MouseType mouseType)</code>: Симулює натискання кнопки миші.</li>
<li><code>Task&lt;int&gt; SimulateMousePullUp(MouseType mouseType)</code>: Симулює відпускання кнопки миші.</li>
</ul>
<h4 id="keyboardapicontroller">KeyboardApiController</h4>
<ul>
<li><code>Task&lt;int&gt; SimulateKeyPressing(byte keyCode)</code>: Симулює натискання та відпускання клавіші за її кодом.</li>
</ul>
<h3 id="повернення-результатів">Повернення результатів</h3>
<p>Усі методи повертають <code>Task&lt;int&gt;</code>:</p>
<ul>
<li><code>1</code>: Операція успішна.</li>
<li><code>0</code>: Сталася помилка (деталі в логах або консолі).</li>
</ul>
<h3 id="події">Події</h3>
<ul>
<li><code>CursorApiController</code>:
<ul>
<li><code>OnMouseMoved</code>: Викликається при переміщенні курсора.</li>
<li><code>OnLeftMouseClicked</code>, <code>OnRightMouseClicked</code>: Кліки лівою/правою кнопкою.</li>
<li><code>OnMousePulledUp</code>, <code>OnMousePulledDown</code>: Відпускання/натискання кнопок.</li>
</ul>
</li>
<li><code>KeyboardApiController</code>:
<ul>
<li><code>OnKeyPressed</code>: Викликається при симуляції натискання клавіші.</li>
</ul>
</li>
</ul>
<h2 id="структура-проекту">Структура проекту</h2>
<ul>
<li><strong>Controllers</strong>:
<ul>
<li><code>CursorApiController.cs</code>: Логіка для роботи з мишею.</li>
<li><code>KeyboardApiController.cs</code>: Логіка для роботи з клавіатурою.</li>
</ul>
</li>
<li><strong>Models</strong>:
<ul>
<li><code>MouseInfoModel</code>: Модель для зберігання даних про позицію курсора та тип кнопки.</li>
</ul>
</li>
<li><strong>SD</strong>:
<ul>
<li><code>MouseType</code>: Перелік для типів кнопок миші (<code>LEFTMOUSE</code>, <code>RIGHTMOUSE</code>).</li>
<li><code>KeyboardKeys</code>: Коди клавіш для симуляції.</li>
</ul>
</li>
<li><strong>Utilities</strong>:
<ul>
<li><code>Logger</code>: Глобальний клас для логування дій.</li>
</ul>
</li>
<li><strong>Exceptions</strong>:
<ul>
<li><code>CursorApiException</code>: Виняток для помилок API.</li>
</ul>
</li>
</ul>
<h2 id="вимоги">Вимоги</h2>
<ul>
<li>ОС: Windows (через використання Windows API <code>user32.dll</code>).</li>
<li>.NET Framework або .NET Core (версія залежить від вашого проєкту).</li>
<li>Кодування консолі: Рекомендується встановити <code>Console.OutputEncoding = Encoding.UTF8</code> для коректного відображення української мови.</li>
</ul>
<h2 id="обмеження">Обмеження</h2>
<ul>
<li>Працює лише на Windows через залежність від <code>user32.dll</code>.</li>
<li>Багатопотокові операції синхронізуються через <code>SemaphoreSlim</code>, але потребують обережного використання в складних сценаріях.</li>
<li>Логування залежить від глобального класу <code>Logger</code>, який має бути реалізований у вашому проєкті.</li>
</ul>
<h2 id="тестування">Тестування</h2>
<p>Бібліотека включає тестовий код (<code>Program.cs</code>), який демонструє всі основні функції:</p>
<ul>
<li>Натискання клавіші.</li>
<li>Переміщення курсора.</li>
<li>Кліки мишею.</li>
<li>Натискання та відпускання кнопок миші.</li>
</ul>
<p>Запустіть тестовий код для перевірки роботи бібліотеки.</p>
<h2 id="логування">Логування</h2>
<p>Усі дії (успішні та помилкові) записуються в <code>Logger</code>. Переглядайте логи для діагностики проблем.</p>
<h2 id="внесок">Внесок</h2>
<ol>
<li>Форкніть репозиторій.</li>
<li>Створіть гілку для нової функції (<code>git checkout -b feature/новий-функціонал</code>).</li>
<li>Закомітьте зміни (<code>git commit -m 'Додано новий функціонал'</code>).</li>
<li>Запуште гілку (<code>git push origin feature/новий-функціонал</code>).</li>
<li>Створіть Pull Request.</li>
</ol>
<h2 id="ліцензія">Ліцензія</h2>
<p>Цей проєкт поширюється за ліцензією MIT (або вкажіть іншу, якщо потрібно).</p>
<h2 id="контакти">Контакти</h2>
<p>Для питань чи пропозицій звертайтеся через Issues у репозиторії.</p>

