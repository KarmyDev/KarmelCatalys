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

        public static void Awake() // Awake is the frist method that's called
        {
            
        }

        public static void Start() // Start is called after Awake
        {
            playerPos = new Vec2Int(2, 2);
            RenderMap();
            RenderPlayerPos();
        }

        public static void Update() // Update is called every 0.01 seconds
        {
            if (Input.KeyDown(ConsoleKey.W))
            {
                playerPos.Y--;
                RenderMap();
                RenderPlayerPos();
            }
            else if (Input.KeyDown(ConsoleKey.A))
            {
                playerPos.X-=2;
                RenderMap();
                RenderPlayerPos();
            }
            else if (Input.KeyDown(ConsoleKey.S))
            {
                playerPos.Y++;
                RenderMap();
                RenderPlayerPos();
            }
            else if (Input.KeyDown(ConsoleKey.D))
            {
                playerPos.X+=2;
                RenderMap();
                RenderPlayerPos();
            }
            

        }

        public static void FixedUpdate() // FixedUpdate is called every 0.1 seconds
        {

            
        }

        // ### Workspace ###

        public static Vec2Int playerPos;

        public static void RenderPlayerPos()
        {
            Console.SetCursorPosition(playerPos.X, playerPos.Y);
            Console.Write("██".Pastel("#fcba03"));
            Console.SetCursorPosition(51, 24);
        }

        public static void RenderMap()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n                                                    \n".PastelBg("#999999"));
            // Console.SetCursorPosition(0, 0);
            // UI.DrawUIBox(new Vec2Int(26-2, 25-2), "#44a832");
        }
    }
}
