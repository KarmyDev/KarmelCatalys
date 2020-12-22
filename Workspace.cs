using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// REQUIRED!!
using KarmelCatalysEngine;
using Karmel.Vectors;
using Pastel;

namespace Workspace
{
    public static class Karmel
    {
        public static Map map01;

        public static void Awake() // Awake is the frist method that's called
        {
            Console.SetCursorPosition(0, 0);
            UI.DrawBackground("#B9B9EC");
            Console.SetCursorPosition(0, 0);
            UI.DrawUIBox(new Vec2Int(8, 3));
        }

        public static void Start() // Start is called after Awake
        {
            playerPos = new Vec2Int(0, 0);
        }

        private static bool mUpdate, mSlow, mLazy;
        private static int iUpdate, iSlow, iLazy;

        public static void Update() // Update is called every 0.01 seconds
        {
            iUpdate++;
            if (iUpdate >= 9) iUpdate = 0;

            Console.SetCursorPosition(1, 1);
            Console.Write("Update     :: " + iUpdate + "   ");
            Console.SetCursorPosition(1, 2);
            Console.Write("SlowUpdate :: " + iSlow + "   ");
            Console.SetCursorPosition(1, 3);
            Console.Write("LazyUpdate :: " + iLazy + "   ");
            
            Console.SetCursorPosition(KarmelCatalys.Program.appWidth -1 , KarmelCatalys.Program.appHeight - 1);

            if (Input.KeyDown(ConsoleKey.W))
            {
                playerPos.Y--;
                RenderMap();

            }
            else if (Input.KeyDown(ConsoleKey.A))
            {
                playerPos.X -= 2;
                RenderMap();

            }
            else if (Input.KeyDown(ConsoleKey.S))
            {
                playerPos.Y++;
                RenderMap();

            }
            else if (Input.KeyDown(ConsoleKey.D))
            {
                playerPos.X += 2;
                RenderMap();
            }
            else if (Input.KeyDown(ConsoleKey.L))
            {
                map01.LoadFromFile(@"C:\SpecialFolder\MAPS\MAP01.KCM");
            }
            else if (Input.KeyDown(ConsoleKey.S))
            {
                map01.SaveToFile(@"C:\SpecialFolder\MAPS\MAP01.KCM");
            }



        }

        public static void SlowUpdate() // SlowUpdate is called every ~ 0.5 seconds
        {
            iSlow++;
            if (iSlow >= 9) iSlow = 0;
        }

        public static void LazyUpdate() // LazyUpdate is called every ~ 1 second
        {
            iLazy++;
            if (iLazy >= 9) iLazy = 0;
        }

        // ### Workspace ###
        public static Vec2Int playerPos;

       

        public static void RenderMap()
        {
            Console.SetCursorPosition(0, 0);
            map01.SetPosition(playerPos);
            map01.RenderMap();
        }
    }
}
