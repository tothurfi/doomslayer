using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    public enum DemonType
    {
        Zombieman,
        Imp,
        Mancubus
    }

    enum DemonStateType
    {
        Idle,
        Move,
        Attack
    }

    public class Demon
    {
        Position _position;
        public Position Position { get => _position; set => _position = value; }
        ConsoleSprite _sprite;
        public ConsoleSprite Sprite { get => _sprite; }
        DemonType _type;
        public DemonType Type { get => _type; }

        DemonStateType _state;
        internal DemonStateType State { get => _state; set => _state = value; }
        double _fillingratio;
        public double FillingRatio {  get => _fillingratio; }
        int _health;
        public int Health { get => _health;
            set
            {
                if(value >= 0)
                {
                    _health = value;
                }
            }
        }

        bool _alive;
        public bool Alive { get => _alive; }
        int _sightrange;
        public int SightRange { get =>  _sightrange; }
        int _attackrange;
        public int AttackRange { get => _attackrange; }

        public Demon(int x, int y, DemonType type)
        {
            this._position = new Position(x, y);
            this._type = type;
            this._alive = true;
            this._state = DemonStateType.Idle;
            SetInitialProperties();
        }

        public void SetInitialProperties()
        {
            switch (Type)
            {
                case DemonType.Zombieman:
                    SetPropertyValues(0.4, 20, 15, 70, 3, 1);
                    _sprite = new(ConsoleColor.Black, ConsoleColor.White, 'o');
                    break;
                case DemonType.Imp:
                    SetPropertyValues(0.4, 60, 24, 93, 6, 3);
                    _sprite = new(ConsoleColor.Black, ConsoleColor.Red, 'o');
                    break;
                case DemonType.Mancubus:
                    SetPropertyValues(0.96, 600, 64, 70, 9, 6);
                    _sprite = new(ConsoleColor.Black, ConsoleColor.Magenta, 'O');
                    break;
            }
        }

        public void UpdateState(Player player)
        {
            if (Position.Distance(player.Position, this.Position) < this.AttackRange)
                _state = DemonStateType.Attack;
            else if (Position.Distance(player.Position, this.Position) < this.SightRange)
                _state = DemonStateType.Move;
            else
                _state = DemonStateType.Idle;
        }

        private void SetPropertyValues(double fRatio, int hp, int damage, int speed, int viewDistance, int damageDistance)
        {
            _fillingratio = fRatio;
            _health = hp;
            _sightrange = viewDistance;
            _attackrange = damageDistance;
            //damage = damage;
            //speed = speed;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (Health <= 0)
                _alive = false;
        }
    }
}
