using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    public class Position
    {
        public int x;
        public int X { get => x; set => x = value; }
        public int y;
        public int Y { get => y; set => y = value; }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Position Add(Position p_1, Position p_2)
        {
            int sumPosition_1 = p_1.X + p_2.X;
            int sumPosition_2 = p_1.Y + p_2.Y;
            return new Position(sumPosition_1, sumPosition_2);
        }

        public static float Distance(Position p_1, Position p_2)
        {
            double distanceX = (double)p_2.X - p_1.X;
            double distanceY = (double)p_2.Y - p_1.Y;

            return (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
        }
    }
}
