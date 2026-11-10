using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    public class Game
    {
        ConsoleRenderer renderer;
        GameLogic logic;
        Stopwatch stopwatchLogic = new();
        Stopwatch stopwatchRenderer = new();
        Player _player;
        bool _exited;
        List<GameItem> _items;

        public List<GameItem> Items { get => _items; set => _items = value; }
        public List<Demon> _demons;
        public List<Demon> Demons { get => _demons; }
        public Player Player { get => _player; }
        public bool Exited { get => _exited; set => _exited = value;  }
    
        public Game()
        {
            renderer = new ConsoleRenderer(this);
            logic = new GameLogic(this);
            this._player = new(0,0);
            this._exited = false;
            this._items = new();
            this._demons = new List<Demon>();
        }
        
        private void UserAction()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Escape:
                        Exited = true;
                        break;
                    case ConsoleKey.DownArrow:
                        if(_player.Position.Y < Console.WindowHeight-1)
                        {
                            logic.Move(_player, Position.Add(_player.Position, new Position(0, +1)));
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (_player.Position.Y > 0)
                        {
                            logic.Move(_player, Position.Add(_player.Position, new Position(0,-1)));
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (_player.Position.X > 0)
                        {
                            logic.Move(_player, Position.Add(_player.Position, new Position(-1, 0)));
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (_player.Position.X < Console.WindowWidth-1)
                        {
                            logic.Move(_player, Position.Add(_player.Position, new Position(+1, 0)));
                        }
                        break;
                    case ConsoleKey.D:
                        logic.PlayerDirectInteractionLogic();
                        break;
                    case ConsoleKey.A:
                        logic.PlayerAttackLogic(_player);
                        break;
                }
                logic.PlayerIndirectInteractionLogic();
            }
        }

        public void Run()
        {
            while (!this.Exited && Player.Alive)
            {
                stopwatchLogic.Start();
                stopwatchRenderer.Start();
                if(stopwatchLogic.ElapsedMilliseconds > 500)
                {
                    logic.UpdateGameState();
                    stopwatchLogic.Restart();
                }
                if(stopwatchRenderer.ElapsedMilliseconds > 25)
                {
                    renderer.RenderGame();
                    stopwatchRenderer.Restart();
                }
                UserAction();
            }
        }
    }
}