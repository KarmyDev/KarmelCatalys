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
            var mapObj = new MapObject();

            /*
            mapObj.character = "[]";
            mapObj.color = "#c521db";
            mapObj.bgcolor = "#661e70";
            mapObj.x = 0;
            mapObj.y = 0;
            */
            map01 = new Map();
            map01.SetPosition(new Vec2Int(0, 0));
            /*
            map01.objs = new MapObject[650];
            for (int i = 0; i < 650; i++)
            {
                map01.objs[i] = mapObj;
            }
            map01.RenderMap(true);
            map01.SaveToFile(@"C:\SpecialFolder\MAPS\map01.kcm");
            */
            map01.LoadFromFile(@"C:\SpecialFolder\MAPS\map01.kcm");
            map01.RenderMap(false);
            
            
        }

        public static void Start() // Start is called after Awake
        {
            playerPos = new Vec2Int(2, 2);
            // RenderMap();
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
            Console.Write("██".Pastel("#fcba03").PastelBg("#0c0c55"));
            Console.SetCursorPosition(51, 24);
        }

        public static void RenderMap()
        {
            UI.DrawBackground("#0c0c55");
            
            Console.SetCursorPosition(0, 0);
            UI.DrawUIBox(new Vec2Int(26 - 2, 25 - 2), "#44a832", "#0c0c55");
            Console.SetCursorPosition(4, 2);
            UI.DrawUIBox(new Vec2Int(26 - 6, 25 - 6), "#990544", "#0c0c55");
        }
    }
}
