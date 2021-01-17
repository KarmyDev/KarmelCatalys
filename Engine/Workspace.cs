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

            // UI.DrawUIBox(new Vec2Int(KarmelCatalys.Program.screenWidth, KarmelCatalys.Program.screenHeight),"#26FF00");
            Console.SetCursorPosition(0, 0);
            UI.DrawBackground("#000000");

            map01 = new IDMap();
            map01.MapObjectData = new int[,] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            
            map01.TileList = new string[]
            { " ", " ".PastelBg("#C816CB") };
            map01.RenderMap();

            //vv ERROR TEST vv
            //map01.TileList = null;
            //map01.RenderMap();

            //KarmelCatalys.FUNCTIONS.ERROR_BOX.SHOW("My personal error", false);


        }

        public void Start() // Start is called after Awake
        {
            // DONT TOUCH IT // Audio.PlayData(File.ReadAllBytes(@"C:\SpecialFolder\rmx.wav"));
            mySong = new Audio.Song(@"C:\SpecialFolder\rmx.wav");
            yourSong = new Audio.Song(@"C:\SpecialFolder\rmx.wav");
            posX = -10;
            posY = -10;
            RenderFrame();
        }
        private IDMap map01;
        private Audio.Song mySong, yourSong;
        private bool consoleActive = false;
        private int posX, posY;
        public void QuickUpdate() // QuickUpdate is called every ~ 0.5 seconds (or 0.001)
        {

        }

        public void RenderFrame()
        {
            map01.Position = new Vec2Int(posX, posY);
            Console.SetCursorPosition(0, 0);
            map01.RenderMap();
            Console.SetCursorPosition(0, 0);
            UI.DrawUIBox(new Vec2Int(KarmelCatalys.Program.screenWidth, 3), "#16FF00", true);
            Console.SetCursorPosition(2, 2);
            Console.Write("            Lives: 4   |   Keys: 0");
            Console.SetCursorPosition(0, 0);
        }

        public void Update() // Update is called every ~ 0.1 seconds
        {
            if (!consoleActive)
            {
                if (Input.KeyDown(ConsoleKey.RightArrow))
                {
                    posX++;
                    RenderFrame();
                }
                if (Input.KeyDown(ConsoleKey.LeftArrow))
                {
                    posX--;
                    RenderFrame();
                }
                if (Input.KeyDown(ConsoleKey.UpArrow))
                {
                    posY--;
                    RenderFrame();
                }
                if (Input.KeyDown(ConsoleKey.DownArrow))
                {
                    posY++;
                    RenderFrame();
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
                    RenderFrame();
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
                    RenderFrame();
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

        // ### Workspace ###
       
    }
}
