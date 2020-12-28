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
            mySong = new Audio.Song(@"C:\SpecialFolder\rmx.wav");
            yourSong = new Audio.Song(@"C:\SpecialFolder\rmx.wav"); ;
        }

        private static Audio.Song mySong, yourSong;

        public static void Update() // Update is called every 0.01 seconds
        {
            if (Input.KeyDown(ConsoleKey.Enter))
            {
                mySong.Play();
                Console.WriteLine("Now playing: mySong");
            }
            if (Input.KeyDown(ConsoleKey.Backspace))
            {
                yourSong.Play();
                Console.WriteLine("Now playing: yourSong");
            }
        }

        public static void SlowUpdate() // SlowUpdate is called every ~ 0.5 seconds
        {
            
        }

        public static void LazyUpdate() // LazyUpdate is called every ~ 1 second
        {
            
        }

        // ### Workspace ###
       
    }
}
