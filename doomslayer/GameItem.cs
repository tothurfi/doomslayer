using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    public enum ItemType { Ammo, BFGCell, Door, LevelExit, Medikit, ToxicWaste, Wall }
    public class GameItem
    {
        Position position;
        double fillingRatio;
        bool available = true;
        ItemType type;
        ConsoleSprite _sprite;
        public Position Position { get => position; }

        public ConsoleSprite Sprite { get => _sprite; }

        public ItemType Type { get => type; }
        public double FillingRatio { get => fillingRatio; }
        public bool Available { get => available; }

        private void SetInitialProperties()
        {
            switch (type)
            {
                case ItemType.Ammo:
                    fillingRatio = 0.0;
                    _sprite = new ConsoleSprite(ConsoleColor.Red, ConsoleColor.Yellow, 'A');
                    break;
                case ItemType.BFGCell:
                    fillingRatio = 0.0;
                    _sprite = new ConsoleSprite(ConsoleColor.Green, ConsoleColor.White, 'B');
                    break;
                case ItemType.Door:
                    //zárva
                    fillingRatio = 1.0;
                    _sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Yellow, '/');
                    //nyitva
                    //fillingRatio = 0.0;
                    //_sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Gray, '/')
                    break;
                case ItemType.LevelExit:
                    fillingRatio = 1.0;
                    _sprite = new ConsoleSprite(ConsoleColor.Cyan, ConsoleColor.Black, 'E');
                    break;
                case ItemType.Medikit:
                    fillingRatio = 0.0;
                    _sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Red, '+');
                    break;
                case ItemType.ToxicWaste:
                    fillingRatio = 0.0;
                    _sprite = new ConsoleSprite(ConsoleColor.DarkGreen, ConsoleColor.DarkYellow, ':');
                    break;
                case ItemType.Wall:
                    fillingRatio = 1.0;
                    _sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Gray, 'W');
                    break;
            }
        }

        public GameItem(int x, int y, ItemType type)
        {
            this.position = new(x, y);
            this.type = type;

            SetInitialProperties();
        }

        public void Interact()
        {
            //sw -> tab -> type -> lefele nyíl
            switch (type)
            {
                case ItemType.Ammo:
                case ItemType.BFGCell:
                case ItemType.Medikit:
                    this.available = false;
                    break;
                case ItemType.Door:
                    if (this.fillingRatio == 0.0)
                    {
                        fillingRatio = 1.0;
                        _sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Yellow, '/');
                    }
                    else if(fillingRatio == 1.0)
                    {
                        fillingRatio = 0.0;
                        _sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.DarkGray, '/');
                    }
                    break;
            }
        }
    }
}
