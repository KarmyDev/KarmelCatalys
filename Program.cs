using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;

// External
using Karmel.Vectors;
using Pastel;
using System.IO;

namespace KarmelCatalys
{
    class Program
    {
        #region QuickEditModeDisable
        private static class NativeFunctions
        {
            public enum StdHandle : int
            {
                STD_INPUT_HANDLE = -10,
                STD_OUTPUT_HANDLE = -11,
                STD_ERROR_HANDLE = -12,
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetStdHandle(int nStdHandle); //returns Handle

            public enum ConsoleMode : uint
            {
                ENABLE_ECHO_INPUT = 0x0004,
                ENABLE_EXTENDED_FLAGS = 0x0080,
                ENABLE_INSERT_MODE = 0x0020,
                ENABLE_LINE_INPUT = 0x0002,
                ENABLE_MOUSE_INPUT = 0x0010,
                ENABLE_PROCESSED_INPUT = 0x0001,
                ENABLE_QUICK_EDIT_MODE = 0x0040,
                ENABLE_WINDOW_INPUT = 0x0008,
                ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,

                //screen buffer handle
                ENABLE_PROCESSED_OUTPUT = 0x0001,
                ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
                ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
                DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
                ENABLE_LVB_GRID_WORLDWIDE = 0x0010
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
        }

        public static void QuickEditMode(bool Enable)
        {
            //QuickEdit lets the user select text in the console window with the mouse, to copy to the windows clipboard.
            //But selecting text stops the console process (e.g. unzipping). This may not be always wanted.
            IntPtr consoleHandle = NativeFunctions.GetStdHandle((int)NativeFunctions.StdHandle.STD_INPUT_HANDLE);
            UInt32 consoleMode;

            NativeFunctions.GetConsoleMode(consoleHandle, out consoleMode);
            if (Enable)
                consoleMode |= ((uint)NativeFunctions.ConsoleMode.ENABLE_QUICK_EDIT_MODE);
            else
                consoleMode &= ~((uint)NativeFunctions.ConsoleMode.ENABLE_QUICK_EDIT_MODE);

            consoleMode |= ((uint)NativeFunctions.ConsoleMode.ENABLE_EXTENDED_FLAGS);

            NativeFunctions.SetConsoleMode(consoleHandle, consoleMode);
        }
        #endregion
        #region DisableResizing
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        static void Main(string[] args)
        {
            
            #region PrepareWindow
            Console.SetWindowSize(52, 25);
            QuickEditMode(false);
            Console.SetBufferSize(52, 25);


            // Now disable resizing
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
            Console.Title = "KarmelCatalys Runtime \"Engine\"";
            #endregion

            #region PrepareKarmelVoids
            Workspace.Karmel.Awake();
            Workspace.Karmel.Start();
            var fixedUpdate = Task.Run(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(100);
                    Workspace.Karmel.FixedUpdate();
                }
            });
            var normalUpdate = Task.Run(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(10);
                    Workspace.Karmel.Update();
                }
            });
            #endregion

            while (dontCloseApp)
            {
                
                pressedKey = Console.ReadKey(true).Key;
            }
            Environment.Exit(0);
        }
        public static bool dontCloseApp = true;
        public static ConsoleKey pressedKey;

    }
}

namespace KarmelCatalysEngine
{
    public static class Input
    {
        public static bool KeyDown(ConsoleKey key)
        {
            if (KarmelCatalys.Program.pressedKey == key)
            {
                KarmelCatalys.Program.pressedKey = ConsoleKey.NoName;
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// UI class for drawing UI in the console.
    /// </summary>
    public static class UI
    {
        public static void DrawUIBox(Vec2Int boxSize, string color)
        {
            var cursor = new Vec2Int(Console.CursorLeft, Console.CursorTop);

            string firstPart = "╔";
            string midlePart = "║";
            string lastPart = "╚";
            for (int i = 0; i < boxSize.X + 1; i++)
            {
                firstPart += "══";
                midlePart += "  ";
                lastPart += "══";
            }
            firstPart += "╗";
            midlePart += "║";
            lastPart += "╝";

            Console.Write(firstPart.Pastel(color));

            for (int i = 0; i < boxSize.Y; i++)
            {
                cursor.Y += 1;
                Console.SetCursorPosition( cursor.X , cursor.Y);
                Console.Write(midlePart.Pastel(color));
            }
            Console.SetCursorPosition(cursor.X, cursor.Y + 1);
            Console.Write(lastPart.Pastel(color));
        }
    }
}
