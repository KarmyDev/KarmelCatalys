using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// REQUIRED!!
using KarmelCatalysEngine;
using Karmel.Vectors;
using Pastel;

using System.IO;

namespace Workspace
{
    public class Karmel
    {

        public void Awake() // Awake is the frist method that's called
        {
            // UI.DrawUIBox(new Vec2Int(KarmelCatalys.Program.screenWidth, KarmelCatalys.Program.screenHeight),"#26FF00");
            Console.SetCursorPosition(0, 0);
            UI.DrawBackground("#40C0DE");

            
        }

        public void Start() // Start is called after Awake
        {
            // DONT TOUCH IT // Audio.PlayData(File.ReadAllBytes(@"C:\SpecialFolder\rmx.wav"));
            mySong = new Old_Audio.Song(@"C:\SpecialFolder\rmx.wav");
            yourSong = new Old_Audio.Song(@"C:\SpecialFolder\rmx.wav");
        }

        private Old_Audio.Song mySong, yourSong;

        public void Update() // Update is called every 0.01 seconds
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(Paths.MainDirectory);
            Console.SetCursorPosition(0, 0);
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
