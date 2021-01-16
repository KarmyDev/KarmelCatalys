using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System;

//Required
using Un4seen.Bass;

// Experimental


// External
using Karmel.Vectors;
using Pastel;
using MessagePack;
using System.Diagnostics;

namespace KarmelCatalys
{
    class Program
    {
        public static int appWidth = 52, appHeight = 52;
        public static int screenWidth, screenHeight;

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

        #region ChangeConsoleFont
        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0, short fontWidth = 0)
        {
            // Console.WriteLine("Set Current Font: " + font);

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };


            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize,
                    FontWidth = fontWidth > 0 ? fontWidth : before.FontWidth,
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }
        #endregion

        public static Workspace.Karmel karmelWorkspace;

        /// <summary>
        /// IF YOU CHANGE THIS TO FALSE THEN THE PROGRAM WILL STOP WORKING
        /// </summary>
        public static bool isProgramRunning = true;
        static void Main(string[] args)
        {

            #region PrepareWindow
            QuickEditMode(false);
#pragma warning disable CA1416 // Weryfikuj zgodność z platformą
            Console.SetWindowSize(appWidth, appHeight);
            Console.SetBufferSize(appWidth, appHeight);
#pragma warning restore CA1416 // Weryfikuj zgodność z platformą
            
            // Now disable resizing
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
            
            Console.Title = "KarmelCatalys Runtime \"Engine\"";

            SetCurrentFont("Terminal", 8, 8);

            #endregion

            #region Prepare Bass (Disabled)
            // Bass.BASS_Init(-1, -1, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            #endregion

            #region Prepare Variables
            screenWidth = appWidth - 1;
            screenHeight = appHeight - 1;
            #endregion

            #region PrepareKarmelVoids
            karmelWorkspace = new Workspace.Karmel();

            karmelWorkspace.Awake();
            karmelWorkspace.Start();
            var normalUpdate = Task.Run(async () =>
            {
                while (isProgramRunning)
                {
                    slowUpdateTime++;
                    lazyUpdateTime++;
                    await Task.Delay(1);
                    var update = Task.Run(karmelWorkspace.Update);
                    if (slowUpdateTime >= 10)
                    {
                        var slowUpdate = Task.Run(karmelWorkspace.SlowUpdate);
                        slowUpdateTime = 0;
                    }
                    if (lazyUpdateTime >= 100)
                    {
                        var lazyUpdate = Task.Run(karmelWorkspace.LazyUpdate);
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
            public static void BOXDRAWER_DRAW(Vec2Int boxSize, string color, string bgcolor, bool fill)
            {
                var cursor = new Vec2Int(Console.CursorLeft, Console.CursorTop);

                string firstPart = "╔";
                string midlePartL = "║";
                string midlePartR = "║";
                string lastPart = "╚";
                string fillPart = "";
                for (int i = 0; i < boxSize.X - 1 ; i++)
                {
                    firstPart += "═";
                    fillPart += " ";
                    lastPart += "═";
                }
                firstPart += "╗";
                lastPart += "╝";

                Console.Write(firstPart.Pastel(color).PastelBg(bgcolor));
                if (!fill)
                {
                    for (int i = 0; i < boxSize.Y; i++)
                    {
                        cursor.Y += 1;
                        Console.SetCursorPosition(cursor.X, cursor.Y);
                        Console.Write(midlePartL.Pastel(color).PastelBg(bgcolor));
                        Console.SetCursorPosition(cursor.X + boxSize.X * 2, cursor.Y);
                        Console.Write(midlePartR.Pastel(color).PastelBg(bgcolor));
                    }
                }
                else
                {
                    for (int i = 0; i < boxSize.Y; i++)
                    {
                        cursor.Y += 1;
                        Console.SetCursorPosition(cursor.X, cursor.Y);
                        Console.Write(midlePartL.Pastel(color).PastelBg(bgcolor));
                        Console.Write(fillPart.Pastel(color).PastelBg(bgcolor));
                        Console.Write(midlePartR.Pastel(color).PastelBg(bgcolor));
                    }
                }
                Console.SetCursorPosition(cursor.X, cursor.Y + 1);
                Console.Write(lastPart.Pastel(color).PastelBg(bgcolor));
            }
        }
        public static class ERROR_BOX
        {
            public static void SHOW(string errorMessage, bool forceStop)
            {
                KarmelCatalys.Program.isProgramRunning = !forceStop;
                Console.SetCursorPosition(0, 0);
                KarmelCatalysEngine.UI.DrawUIBox(new Vec2Int(KarmelCatalys.Program.screenWidth, 1), "#FF0000", "#000000", true);
                Console.SetCursorPosition(2, 0);
                Console.Write("Error".Pastel("#FF0000").PastelBg("#000000"));
                Console.SetCursorPosition(2, 1);
                Console.Write(errorMessage.Pastel("#FFFFFF").PastelBg("#000000"));
            }
        }
    }

}

namespace KarmelCatalysEngine
{
    /* THIS IS BASS CLASS - ITS CURRENTLY DISABLED
    public static class Audio
    {

        public static void PlayData(byte[] data)
        {
            Bass.BASS_ChannelPlay(Bass.BASS_StreamPutData(Bass.BASS_StreamCreatePush(0, 0, BASSFlag.BASS_STREAM_AUTOFREE, IntPtr.Zero), data, 0), false);
        }
    }
    */

    public class IDMap
    {
        public Vec2Int Position { set; get; }
        public int[,] MapObjectData { set; get; }
        public string[] TileList { set; get; }

        public void RenderMap()
        {
            MapRender(new Vec2Int(KarmelCatalys.Program.appWidth, KarmelCatalys.Program.appHeight), TileList);
        }

        public void RenderMap(Vec2Int screenSizeToRender)
        {
            MapRender(screenSizeToRender, TileList);
        }

        private string dataToDisplay_MapRenderer;
        private void MapRender(Vec2Int screenSize, string[] tileList)
        {
            if (tileList != null)
            {
                dataToDisplay_MapRenderer = "";
                for (int y = Position.Y; y  < screenSize.Y + Position.Y; y++)
                {
                    for (int x = Position.X; x < screenSize.X + Position.X; x++)
                    {
                        // Checks if tile is in array, if not returns deafult tile (0)
                        if (y >= 0 && y < screenSize.Y + Position.Y && x >= 0 && x < screenSize.X + Position.X)
                        {
                            if (x >= 0 && x < MapObjectData.GetLength(1) - 1 && y >= 0 && y < MapObjectData.GetLength(0) - 1)
                            {
                                if (0 <= MapObjectData[y, x] && MapObjectData[y, x] <= tileList.Length - 1)
                                {
                                    dataToDisplay_MapRenderer += tileList[MapObjectData[y, x]];
                                }
                                else
                                {
                                    dataToDisplay_MapRenderer += " ";
                                }
                            }
                            else
                            {
                                dataToDisplay_MapRenderer += " ";
                            }
                        }
                        else
                        {
                            dataToDisplay_MapRenderer += " ";
                        }
                    }
                    dataToDisplay_MapRenderer += "\n";
                }
                
                Console.SetCursorPosition(0, 0);
                Console.Write(dataToDisplay_MapRenderer);
            }
            else // Exception
            {
                KarmelCatalys.FUNCTIONS.ERROR_BOX.SHOW("TileList was not set!", true);
            }
        }
    }

    public class Old_Audio
    {
        public class Song
        {
            byte[] songData = null;

            public bool IsPlaying { get; private set; }
            public bool IsLooping { get; set; }

            public Song() { songData = null; }

            public Song(string path)
            {
                if (File.Exists(path))
                {
                    songData = File.ReadAllBytes(path);
                }
            }

            public void Play(bool loop)
            {
                ActualPlayer(loop);

            }
            public void Play()
            {
                ActualPlayer(false);
            }

            private void ActualPlayer(bool loop)
            {
                if (songData != null)
                {

                    IsLooping = loop;
                    IsPlaying = true;
#pragma warning disable CA1416 // Weryfikuj zgodność z platformą
                    var thisTask = Task.Factory.StartNew(() =>
                    {

                        byte[] privateSongData = songData;
                        using var ms = new MemoryStream(privateSongData);
                        var player = new SoundPlayer(ms);

                        player.PlaySync();
                        if (IsLooping)
                        {
                            ActualPlayer(IsLooping);
                        }
                        else
                        {
                            IsPlaying = false;
                        }

                    }
                    );
#pragma warning restore CA1416 // Weryfikuj zgodność z platformą

                }
            }

            public void LoadSong(string path)
            {
                if (File.Exists(path))
                {
                    songData = File.ReadAllBytes(path);
                }
            }

            public void ClearSong()
            {
                songData = null;
            }
        }
    }

    public class Map
    {
        public MapObject[,] objs;
        public Vec2Int Position { set; get; } // Screen position, where currently screen is on the map

        public void LoadFromFile(string path)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

            using var fs = new FileStream(path, FileMode.Open);
            objs = MessagePackSerializer.Deserialize<MapObject[,]>(fs, lz4Options);
        }

        private string renderer_renderedMap;

        public void RenderLastMap()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(renderer_renderedMap);
        }
        public void RenderMap()
        {
            for (int i = Position.X; i < Position.X + KarmelCatalys.Program.appWidth / 2; i++)
            {
                for (int j = Position.Y; j < Position.Y + KarmelCatalys.Program.appHeight; i++)
                {
                    var mapObj = new MapObject();
                    if (i >= 0 && i < objs.GetLength(0) && j >= 0 && j < objs.GetLength(1))
                    {
                        if (objs[i, j] != null)
                        {
                            mapObj = objs[i, j];
                        }
                        else
                        {
                            mapObj = GetDefaultMapObject();
                        }
                    }
                    else
                    {
                        mapObj = GetDefaultMapObject();
                    }
                    renderer_renderedMap += mapObj.character.Pastel(mapObj.color).PastelBg(mapObj.bgcolor);
                }
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(renderer_renderedMap);
        }

        private MapObject GetDefaultMapObject()
        {
            var mapObj = new MapObject();
            mapObj.character = "  ";
            mapObj.color = "#cccccc";
            mapObj.bgcolor = "#0c0c0c";
            mapObj.eventData = "#000000";
            return mapObj;
        }

        public void SaveToFile(string path)
        {
            var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
            byte[] byteObjs = MessagePackSerializer.Serialize(objs, lz4Options);

            using var fs = new FileStream(path, FileMode.Append);
            fs.Write(byteObjs);
        }

        public MapObject GetMapObject(Vec2Int position)
        {
            return objs[position.X, position.Y];
        }

        public Vec2Int ScreenToMapPos(Vec2Int position)
        {
            return new Vec2Int(Position.X + position.X, Position.Y + position.Y);
        }

        public void SetPosition(Vec2Int position)
        {
            Position = position;
        }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class MapObject
    {
        public string character, color, bgcolor, eventData;
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
            if (key != ConsoleKey.NoName)
            {
                KarmelCatalys.Program.pressedKey = ConsoleKey.NoName;
                return true;
            }
            key = ConsoleKey.NoName;
            return false;
        }
        public static bool AnyKey()
        { 
            if (KarmelCatalys.Program.pressedKey != ConsoleKey.NoName)
            {
                KarmelCatalys.Program.pressedKey = ConsoleKey.NoName;
                return true;
            }
            return false;
        }
    }
    public static class UI
    {
        public static void DrawUIBox(Vec2Int boxSize)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, "#cccccc", "#0c0c0c", false);
        }
        public static void DrawUIBox(Vec2Int boxSize, string color)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, color, "#0c0c0c", false);
        }
        public static void DrawUIBox(Vec2Int boxSize, string color, string bgcolor)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, color, bgcolor, false);
        }
        public static void DrawUIBox(Vec2Int boxSize, string color, bool fill)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, color, "#0c0c0c", fill);
        }
        public static void DrawUIBox(Vec2Int boxSize, string color, string bgcolor, bool fill)
        {
            KarmelCatalys.FUNCTIONS.UIDRAWER.BOXDRAWER_DRAW(boxSize, color, bgcolor, fill);
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

    public static class Screen
    {
        /// <summary>
        /// Default is 8 (you can use 16 for big screen)
        /// </summary>
        public static void ChangeScreenZoom(short rate)
        {
            KarmelCatalys.Program.SetCurrentFont("Terminal", rate, rate);
        }
        /// <summary>
        /// Default screen size is 52x52 (you can use 25x25 | 35x35 | 42x42)
        /// </summary>
        public static void ChangeScreenSize(int x, int y)
        {
            KarmelCatalys.Program.appWidth = x;
            KarmelCatalys.Program.appHeight = y;
            KarmelCatalys.Program.screenWidth = KarmelCatalys.Program.appWidth - 1;
            KarmelCatalys.Program.screenHeight = KarmelCatalys.Program.appHeight - 1;
#pragma warning disable CA1416 // Validate platform compatibility
            Console.SetWindowSize(KarmelCatalys.Program.appWidth, KarmelCatalys.Program.appHeight);
            Console.SetBufferSize(KarmelCatalys.Program.appWidth, KarmelCatalys.Program.appHeight);
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }

    public static class Paths
    {
        public static string MainDirectory { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);
    }
}