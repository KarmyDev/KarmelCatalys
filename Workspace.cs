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
            UI.DrawBackground("#B9B9EC");
            Console.SetCursorPosition(0,0);
            Console.WriteLine("Width/2 :: " + KarmelCatalys.Program.appWidth / 2);
            Console.WriteLine("Height :: " + KarmelCatalys.Program.appHeight);
        }

        public static void Start() // Start is called after Awake
        {
            playerPos = new Vec2Int(0, 0);
        }

        public static void Update() // Update is called every 0.01 seconds
        {
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

        public static void FixedUpdate() // FixedUpdate is called every 0.1 seconds
        {
            
            
        }

        public static void LateUpdate() // LateUpdate is called every 1 second
        {
            
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
