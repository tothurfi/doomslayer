using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    public class Player
    {
        Position _playerPosition;
        double fillingratio;
        public double Fillingratio { get => fillingratio; }
        public Position Position { get => _playerPosition; set => _playerPosition = value; }
        ConsoleSprite _sprite;
        public ConsoleSprite Sprite { get => _sprite;}

        int _health;
        public int Health { get => _health;
            set
            {
                if(value >= 0 && value <= MaxHealth)
                    _health = value;
            }
        }
        int _ammo;
        public int Ammo { get => _ammo;
            set
            {
                if (value >= 0 && value <= MaxAmmo)
                    _ammo = value;
            }
        }

        int _sightrange;
        public int SightRange { get => _sightrange; }

        bool _alive;
        public bool Alive { get =>  _alive; }

        int _points;
        public int CombatPoints { get => _points; }

        public int MaxHealth
        {
            get
            {
                return 100 + CombatPoints / 10;
            }
        }

        public int MaxAmmo
        {
            get
            {
                return 10 + CombatPoints / 50;
            }
        }

        public Player(int x, int y)
        {
            _playerPosition = new(x,y);
            _sprite = new(ConsoleColor.Black, ConsoleColor.Green, 'O');
            fillingratio = 0.4;
            _health = 100;
            _sightrange = 8;
            _alive = true;
            _ammo = 50;
        }

        public void Shoot()
        {
            _ammo--;
        }

        public void PickUpAmmo(int ammo)
        {
            Ammo += ammo;
        }

        public void PickUpHealth(int hp)
        {
            Health += hp;
        }

        public void AddCombatPoints(int points)
        {
            _points += points;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (Health <= 0)
                _alive = false;
        }
    }
}
