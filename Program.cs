using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;

// External
using Karmel.Vectors;
using Pastel;
using System.IO;
using MessagePack;

namespace KarmelCatalys
{
    class Program
    {
        public static int appWidth = 52, appHeight = 25;

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
            Console.SetWindowSize(appWidth, appHeight);
            QuickEditMode(false);
            Console.SetBufferSize(appWidth, appHeight);


            /* Now disable resizing
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
            */
            Console.Title = "KarmelCatalys Runtime \"Engine\"";
            #endregion

            #region PrepareKarmelVoids
            Workspace.Karmel.Awake();
            Workspace.Karmel.Start();
            var normalUpdate = Task.Run(async () =>
            {
                while (true)
                {
                    slowUpdateTime++;
                    lazyUpdateTime++;
                    await Task.Delay(1);
                    var update = Task.Run(Workspace.Karmel.Update);
                    if (slowUpdateTime >= 10)
                    {
                        var slowUpdate = Task.Run(Workspace.Karmel.SlowUpdate);
                        slowUpdateTime = 0;
                    }
                    if (lazyUpdateTime >= 100)
                    {
                        var lazyUpdate = Task.Run(Workspace.Karmel.LazyUpdate);
                        lazyUpdateTime = 0;
                    }
                }
            });
            #endregion

            while (dontCloseApp)
            {
                
                pressedKey = Console.ReadKey(true).Key;
            }
            Environment.Exit(0);
        }
        private static int slowUpdateTime = 0, lazyUpdateTime = 0;

        public static bool dontCloseApp = true;
        public static ConsoleKey pressedKey;

    }

    namespace FUNCTIONS
    {
        public static class UIDRAWER
        {
            public static void BOXDRAWER_DRAW(Vec2Int boxSize, string color, string bgcolor)
            {
                var cursor = new Vec2Int(Console.CursorLeft, Console.CursorTop);

                string firstPart = "╔";
                string midlePart = "║";
                string lastPart = "╚";
                for (int i = 0; i < boxSize.X + 1; i++)
                {
                    firstPart += "══";
                    lastPart += "══";
                }
                firstPart += "╗";
                lastPart += "╝";

                Console.Write(firstPart.Pastel(color).PastelBg(bgcolor));

                for (int i = 0; i < boxSize.Y; i++)
                {
                    cursor.Y += 1;
                    Console.SetCursorPosition(cursor.X, cursor.Y);
                    Console.Write(midlePart.Pastel(color).PastelBg(bgcolor));
                    Console.SetCursorPosition(cursor.X + 3 + boxSize.X * 2, cursor.Y);
                    Console.Write(midlePart.Pastel(color).PastelBg(bgcolor));
                }
                Console.SetCursorPosition(cursor.X, cursor.Y + 1);
                Console.Write(lastPart.Pastel(color).PastelBg(bgcolor));
            }
        }

    }

}

namespace KarmelCatalysEngine
{
    public class Map
    {
        public MapObject[,] objs;
        public Vec2Int Position { set; get; }
        
        public void LoadFromFile(string path)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

            using (var fs = new FileStream(path, FileMode.Open))
            {
                objs = MessagePackSerializer.Deserialize<MapObject[,]>(fs, lz4Options);
            }
            
        }

        private string renderer_renderedMap;
        public void RenderMap()
        {
            for (int i = 0; i < KarmelCatalys.Program.appWidth / 2; i++)
            {
                for (int j = 0; j < KarmelCatalys.Program.appHeight; i++)
                {
                    var mapObj = objs[i - Position.X, j - Position.Y];
                    renderer_renderedMap += mapObj.character.Pastel(mapObj.color).PastelBg(mapObj.bgcolor);
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(renderer_renderedMap);
            Console.SetCursorPosition(51, 24);
        }

        public void SaveToFile(string path)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
            byte[] byteObjs = MessagePackSerializer.Serialize(objs, lz4Options);

            using (var fs = new FileStream(path, FileMode.Append))
            {
                fs.Write(byteObjs);
            }
        }

        public MapObject GetMapObj(Vec2Int position)
        {
            return objs[position.X, position.Y];
        }

        public Vec2Int ScreenToMapPos(Vec2Int position)
        {
            return new Vec2Int(position.X + Position.X, position.Y + Position.Y);
        }

        public void SetPosition(Vec2Int position)
        {
            Position = position;
        }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class MapObject
    {
        public string character, color, bgcolor;
    }

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

        public static bool AnyKey(out ConsoleKey key)
        {
            key = KarmelCatalys.Program.pressedKey;
            return KarmelCatalys.Program.pressedKey != ConsoleKey.NoName;
        }
    }
    public static class UI
    {
        public static void DrawUIBox(Vec2Int boxSize)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, "#cccccc", "#0c0c0c");
        }
        public static void DrawUIBox(Vec2Int boxSize, string color)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, color, "#0c0c0c");
        }
        public static void DrawUIBox(Vec2Int boxSize, string color, string bgcolor)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, color, bgcolor);
        }
        
        private static string drawer_background_text; // MOVE THIS THINGS TO ANOTHER CLASS FOR TEMP VARIABLESS
        public static void DrawBackground(string color)
        {
            
            for (int i = 0; i < KarmelCatalys.Program.appWidth / 2; i++)
            {
                for (int j = 0; j < KarmelCatalys.Program.appHeight; j++)
                {
                    drawer_background_text += "  ";
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(drawer_background_text.PastelBg(color));
        }
    }
}