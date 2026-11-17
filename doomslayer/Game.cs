using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace doomslayer
{
    enum SoundEffectType
    {
        BFG,
        Door,
        ItemPickup,
        Pain,
        PlayerDeath,
        Shotgun
    }
    public class Game
    {
        WindowsMediaPlayer MusicPlayer;
        WindowsMediaPlayer SfxPlayer;
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
            MusicPlayer = new();
            SfxPlayer = new();
            renderer = new ConsoleRenderer(this);
            logic = new GameLogic(this);
            this._player = new(0,0);
            this._exited = false;
            this._items = new();
            this._demons = new List<Demon>();
        }
        
        internal void PlaySoundEffect(SoundEffectType effectType)
        {
            switch(effectType)
            {
                case SoundEffectType.Door:
                    SfxPlayer.URL = Path.GetFullPath(@"../../../sounds/door.mp3");
                    break;
                case SoundEffectType.ItemPickup:
                    SfxPlayer.URL = Path.GetFullPath(@"../../../sounds/item_pickup.mp3");
                    break;
                case SoundEffectType.Pain:
                    SfxPlayer.URL = Path.GetFullPath(@"../../../sounds/pain.mp3");
                    break;
                case SoundEffectType.Shotgun:
                    SfxPlayer.URL = Path.GetFullPath(@"../../../sounds/shotgun.mp3");
                    break;
                case SoundEffectType.PlayerDeath:
                    SfxPlayer.URL = Path.GetFullPath(@"../../../sounds/player_death.mp3");
                    break;
                case SoundEffectType.BFG:
                    SfxPlayer.URL = Path.GetFullPath(@"../../../sounds/bfg.mp3");
                    break;
            }
            SfxPlayer.controls.play();
        }

        public void PlayMusic(string path)
        {
            string fullPath = Path.GetFullPath(path);
            MusicPlayer.URL = fullPath;
            MusicPlayer.controls.play();
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
                    case ConsoleKey.S:
                        logic.PlayerBFGAttackLogic(_player);
                        break;
                }
                logic.PlayerIndirectInteractionLogic();
            }
        }

        public void Run()
        {
            PlayMusic(@"../../../sounds/doom_music.mp3");
            while (!this.Exited && Player.Alive)
            {
                stopwatchLogic.Start();
                stopwatchRenderer.Start();
                long elapsedMilliseconds = stopwatchLogic.ElapsedMilliseconds;
                if(stopwatchLogic.ElapsedMilliseconds > 500)
                {
                    logic.UpdateGameState(elapsedMilliseconds);
                    stopwatchLogic.Restart();
                }
                if(stopwatchRenderer.ElapsedMilliseconds > 25)
                {
                    renderer.RenderGame();
                    stopwatchRenderer.Restart();
                }
                UserAction();
            }
            if (this.Exited)
                PlayMusic(@"../../../sounds/level_complete.mp3");
            if (!Player.Alive)
                Player.Health = 0;
                PlaySoundEffect(SoundEffectType.PlayerDeath);
            Console.ReadLine();
        }

        public void LoadMapFromPlainText(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int x = int.Parse(lines[0].Split(',')[0]);
            int y = int.Parse(lines[0].Split(',')[1]);

            for(int i = 1; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if(lines[i][j] == 'W')
                    {
                        Items.Add(new GameItem(j, i-1, ItemType.Wall));
                    }
                    if (lines[i][j] == 'A')
                    {
                        Items.Add(new GameItem(j, i - 1, ItemType.Ammo));
                    }
                    if (lines[i][j] == 'B')
                    {
                        Items.Add(new GameItem(j, i - 1, ItemType.BFGCell));
                    }
                    if (lines[i][j] == 'D')
                    {
                        Items.Add(new GameItem(j, i - 1, ItemType.Door));
                    }
                    if (lines[i][j] == 'E')
                    {
                        Items.Add(new GameItem(j, i - 1, ItemType.LevelExit));
                    }
                    if (lines[i][j] == 'M')
                    {
                        Items.Add(new GameItem(j, i - 1, ItemType.Medikit));
                    }
                    if (lines[i][j] == 'T')
                    {
                        Items.Add(new GameItem(j, i - 1, ItemType.ToxicWaste));
                    }
                    if (lines[i][j] == 'z')
                    {
                        Demons.Add(new Demon(j, i - 1, DemonType.Zombieman));
                    }
                    if (lines[i][j] == 'i')
                    {
                        Demons.Add(new Demon(j, i - 1, DemonType.Imp));
                    }
                    if (lines[i][j] == 'm')
                    {
                        Demons.Add(new Demon(j, i - 1, DemonType.Mancubus));
                    }
                    if (lines[i][j] == 'p')
                    {
                        _player = new Player(j, i-1);
                    }
                    if (lines[i][j] == '_')
                    {
                        
                    }
                }
            }
        }
    }
}