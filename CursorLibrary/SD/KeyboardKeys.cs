using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorLibrary.SD
{
    public static class KeyboardKeys
    {
        // Letters
        public const byte A = 0x41;
        public const byte B = 0x42;
        public const byte C = 0x43;
        public const byte D = 0x44;
        public const byte E = 0x45;
        public const byte F = 0x46;
        public const byte G = 0x47;
        public const byte H = 0x48;
        public const byte I = 0x49;
        public const byte J = 0x4A;
        public const byte K = 0x4B;
        public const byte L = 0x4C;
        public const byte M = 0x4D;
        public const byte N = 0x4E;
        public const byte O = 0x4F;
        public const byte P = 0x50;
        public const byte Q = 0x51;
        public const byte R = 0x52;
        public const byte S = 0x53;
        public const byte T = 0x54;
        public const byte U = 0x55;
        public const byte V = 0x56;
        public const byte W = 0x57;
        public const byte X = 0x58;
        public const byte Y = 0x59;
        public const byte Z = 0x5A;

        // Digits
        public const byte D0 = 0x30;
        public const byte D1 = 0x31;
        public const byte D2 = 0x32;
        public const byte D3 = 0x33;
        public const byte D4 = 0x34;
        public const byte D5 = 0x35;
        public const byte D6 = 0x36;
        public const byte D7 = 0x37;
        public const byte D8 = 0x38;
        public const byte D9 = 0x39;

        // Function Keys
        public const byte F1 = 0x70;
        public const byte F2 = 0x71;
        public const byte F3 = 0x72;
        public const byte F4 = 0x73;
        public const byte F5 = 0x74;
        public const byte F6 = 0x75;
        public const byte F7 = 0x76;
        public const byte F8 = 0x77;
        public const byte F9 = 0x78;
        public const byte F10 = 0x79;
        public const byte F11 = 0x7A;
        public const byte F12 = 0x7B;

        // Control Keys
        public const byte Backspace = 0x08;
        public const byte Tab = 0x09;
        public const byte Enter = 0x0D;
        public const byte Shift = 0x10;
        public const byte Ctrl = 0x11;
        public const byte Alt = 0x12;
        public const byte CapsLock = 0x14;
        public const byte Esc = 0x1B;
        public const byte Space = 0x20;

        // Navigation Keys
        public const byte PageUp = 0x21;
        public const byte PageDown = 0x22;
        public const byte End = 0x23;
        public const byte Home = 0x24;
        public const byte LeftArrow = 0x25;
        public const byte UpArrow = 0x26;
        public const byte RightArrow = 0x27;
        public const byte DownArrow = 0x28;
        public const byte Insert = 0x2D;
        public const byte Delete = 0x2E;

        // Numpad
        public const byte NumPad0 = 0x60;
        public const byte NumPad1 = 0x61;
        public const byte NumPad2 = 0x62;
        public const byte NumPad3 = 0x63;
        public const byte NumPad4 = 0x64;
        public const byte NumPad5 = 0x65;
        public const byte NumPad6 = 0x66;
        public const byte NumPad7 = 0x67;
        public const byte NumPad8 = 0x68;
        public const byte NumPad9 = 0x69;
        public const byte Multiply = 0x6A;
        public const byte Add = 0x6B;
        public const byte Subtract = 0x6D;
        public const byte Decimal = 0x6E;
        public const byte Divide = 0x6F;
        public const byte NumLock = 0x90;

        // Symbols & others
        public const byte Semicolon = 0xBA;     // ;
        public const byte Plus = 0xBB;          // =
        public const byte Comma = 0xBC;         // ,
        public const byte Minus = 0xBD;         // -
        public const byte Period = 0xBE;        // .
        public const byte Slash = 0xBF;         // /
        public const byte Backtick = 0xC0;      // `
        public const byte LeftBracket = 0xDB;   // [
        public const byte Backslash = 0xDC;     // \
        public const byte RightBracket = 0xDD;  // ]
        public const byte Apostrophe = 0xDE;    // '

        // Modifier keys
        public const byte LShift = 0xA0;
        public const byte RShift = 0xA1;
        public const byte LCtrl = 0xA2;
        public const byte RCtrl = 0xA3;
        public const byte LAlt = 0xA4;
        public const byte RAlt = 0xA5;
    }
}
