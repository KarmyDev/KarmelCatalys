using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karmel
{
    namespace Vectors
    {
        public struct Vec2
        {
            public float X { get; set; }
            public float Y { get; set; }

            public Vec2(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
        public struct Vec3
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public Vec3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        public struct Vec2Int
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Vec2Int(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        public struct Vec3Int
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public Vec3Int(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }

}
