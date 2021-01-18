using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// REQUIRED!!
using KarmelCatalysEngine;
using Karmel.Vectors;
using Pastel;

// WORKSPACE
using System.IO;

namespace Workspace
{
    public class Karmel
    {

        public void Awake() // Awake is the frist method that's called
        {
            Console.Title = "Sokoban 1.0";
            DiscordRpc.GameName = "Sokoban 1.0";
            DiscordRpc.Show();

            // UI.DrawUIBox(new Vec2Int(KarmelCatalys.Program.screenWidth, KarmelCatalys.Program.screenHeight),"#26FF00");
            Console.SetCursorPosition(0, 0);

            map01 = new IDMap();
            map01.MapObjectData = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }

        };

            map01.TileList = new string[]
            { " ", " ".PastelBg("#C816CB"), " ".PastelBg("#ECF100"), "■".Pastel("#1BA833"), "■".Pastel("#AAAE00").PastelBg("#ECF100") };
            map01.RenderMap();

            //vv ERROR TEST vv
            //map01.TileList = null;
            //map01.RenderMap();

            //KarmelCatalys.FUNCTIONS.ERROR_BOX.SHOW("My personal error", false);


        }

        public void Start() // Start is called after Awake
        {
            // DONT TOUCH IT // Audio.PlayData(File.ReadAllBytes(@"C:\SpecialFolder\rmx.wav"));
            //mySong = new Audio.Song(@"C:\SpecialFolder\rmx.wav");
            //yourSong = new Audio.Song(@"C:\SpecialFolder\rmx.wav");
            posX = -10;
            posY = -17;
            RenderFrame();
        }
        private IDMap map01;
        //private Audio.Song mySong, yourSong;
        private bool consoleActive = false;
        private bool isGameEnd = false;
        private int posX, posY, playerPosX, playerPosY;
        public void QuickUpdate() // QuickUpdate is called every ~ 0.5 seconds (or 0.001)
        {

        }

        public void Update() // Update is called every ~ 0.1 seconds
        {
            if (!consoleActive && !isGameEnd)
            {
                if (Input.KeyDown(ConsoleKey.RightArrow))
                {
                    MovePlayerByShift(1, 0);
                }
                if (Input.KeyDown(ConsoleKey.LeftArrow))
                {
                    MovePlayerByShift(-1, 0);
                }
                if (Input.KeyDown(ConsoleKey.UpArrow))
                {
                    MovePlayerByShift(0, -1);
                }
                if (Input.KeyDown(ConsoleKey.DownArrow))
                {
                    MovePlayerByShift(0, 1);
                }

            }
            var key = new ConsoleKey();
            if (Input.KeyDown(ConsoleKey.Oem3))
            {
                if (!consoleActive)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write("»");
                }
                else
                {
                    if (!isGameEnd)
                    {
                        RenderFrame();
                    }
                    else
                    {
                        UI.DrawBackground("#3FA4B3");
                        Console.SetCursorPosition(KarmelCatalys.Program.screenWidth / 2 - 5, KarmelCatalys.Program.screenHeight / 2);
                        Console.Write(" You win! ".Pastel("#FAFF00").PastelBg("#000000"));
                    }
                }
                consoleActive = !consoleActive;
            }
            if (Input.AnyKey(out key) && consoleActive)
            {
                if (key == ConsoleKey.Spacebar)
                {
                    Console.Write(" ");
                }
                else if (key == ConsoleKey.Backspace)
                {
                    Console.SetCursorPosition(Console.CursorLeft <= 1 ? 1 : Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft <= 1 ? 1 : Console.CursorLeft - 1, Console.CursorTop);
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (!isGameEnd)
                    {
                        RenderFrame();
                    }
                    else
                    {
                        UI.DrawBackground("#3FA4B3");
                        Console.SetCursorPosition(KarmelCatalys.Program.screenWidth / 2 - 5, KarmelCatalys.Program.screenHeight / 2);
                        Console.Write(" You win! ".Pastel("#FAFF00").PastelBg("#000000"));
                    }
                    Console.SetCursorPosition(0, 0);
                    Console.Write("»");
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    Console.SetCursorPosition(Console.CursorLeft <= 1 ? 1 : Console.CursorLeft - 1, Console.CursorTop);
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    Console.SetCursorPosition(Console.CursorLeft >= KarmelCatalys.Program.appWidth - 1 ? KarmelCatalys.Program.appWidth - 1 : Console.CursorLeft + 1, Console.CursorTop);
                }
                else
                {
                    Console.Write(key.ToString().ToLower());
                }

            }

        }

        public void SlowUpdate() // SlowUpdate is called every ~ 0.5 seconds
        {  
           
        }

        public void LazyUpdate() // LazyUpdate is called every ~ 1 second
        {
            
        }

        public void OnExit() // OnExit is called when you before you close console
        { 
        
        }

        // ### Workspace ###
        // -- Game Vars
        public static int currentLevel = 0;
        public static int boxesInRow = 0;



        public void MovePlayerByShift(int x, int y)
        {
            switch (GetTileIDByPlayerShift(x, y))
            {
                case 0:
                case 3:
                    posY += y;
                    posX += x;
                    RenderFrame();
                    break;

                case 2:
                    switch (GetTileIDByPlayerShift(x + x, y + y))
                    {
                        case 0:
                            posY += y;
                            posX += x;
                            Vec2Int pos = map01.ScreenToMapIDPosition(new Vec2Int(playerPosX + x, playerPosY + y));
                            map01.SetMapObject(pos.X, pos.Y, 0);
                            map01.SetMapObject(pos.X + x, pos.Y + y, 2);
                            RenderFrame();

                            CheckLevelScore();
                            break;
                        case 3:
                            posY += y;
                            posX += x;
                            Vec2Int pos2 = map01.ScreenToMapIDPosition(new Vec2Int(playerPosX + x, playerPosY + y));
                            map01.SetMapObject(pos2.X, pos2.Y, 0);
                            map01.SetMapObject(pos2.X + x, pos2.Y + y, 4);
                            boxesInRow++;
                            RenderFrame();

                            CheckLevelScore();
                            break;
                    }
                    break;

                case 4:
                    switch (GetTileIDByPlayerShift(x + x, y + y))
                    {
                        case 0:
                            posY += y;
                            posX += x;
                            Vec2Int pos = map01.ScreenToMapIDPosition(new Vec2Int(playerPosX + x, playerPosY + y));
                            map01.SetMapObject(pos.X, pos.Y, 3);
                            map01.SetMapObject(pos.X + x, pos.Y + y, 2);
                            boxesInRow--;
                            RenderFrame();
                            break;
                        case 3:
                            posY += y;
                            posX += x;
                            Vec2Int pos2 = map01.ScreenToMapIDPosition(new Vec2Int(playerPosX + x, playerPosY + y));
                            map01.SetMapObject(pos2.X, pos2.Y, 3);
                            map01.SetMapObject(pos2.X + x, pos2.Y + y, 4);
                            RenderFrame();
                            break;
                    }
                    break;
            }
        }

        public void CheckLevelScore()
        {
            switch (currentLevel)
            {
                case 0:
                    if (boxesInRow == 6)
                    {
                        currentLevel++;
                        UI.DrawBackground("#3FA4B3");
                        Console.SetCursorPosition(KarmelCatalys.Program.screenWidth/2 - 5, KarmelCatalys.Program.screenHeight /2);
                        Console.Write(" You win! ".Pastel("#FAFF00").PastelBg("#000000"));
                        isGameEnd = true;
                    }
                    break;
            }
        }


        public int GetTileIDByPlayerShift(int xShift, int yShift)
        { 
            return map01.GetMapIDFromTile(map01.ScreenToMapIDPosition(new Vec2Int(playerPosX + xShift, playerPosY + yShift)));
        }

        public void RenderFrame()
        {
            map01.Position = new Vec2Int(posX, posY);
            playerPosX = KarmelCatalys.Program.screenWidth / 2;
            playerPosY = KarmelCatalys.Program.screenHeight / 2;
            Console.SetCursorPosition(0, 0);
            
            map01.RenderMap();
            Console.SetCursorPosition(0, 0);
            UI.DrawUIBox(new Vec2Int(KarmelCatalys.Program.screenWidth, 3), "#16FF00", true);
            Console.SetCursorPosition(KarmelCatalys.Program.screenWidth/4, 2);
            Console.Write(("Level: " + currentLevel + "   |   Boxes in a row: " + boxesInRow).ToString().Pastel("#FFFFFF"));

            Console.SetCursorPosition(playerPosX, playerPosY);
            Console.Write("@".Pastel("#00FF8D"));
            Console.SetCursorPosition(0, 0);

        }

    }
}
