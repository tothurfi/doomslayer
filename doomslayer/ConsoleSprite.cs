using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    public class ConsoleSprite
    {
        ConsoleColor background;
        ConsoleColor foreground;
        char glyph;
        public ConsoleColor Background { get => background; }
        public ConsoleColor Foreground { get => foreground; }
        public char Glyph { get => glyph; }

        public ConsoleSprite(ConsoleColor background, ConsoleColor foreground, char glyph)
        {
            this.background = background;
            this.foreground = foreground;
            this.glyph = glyph;
        }


    }
}
