using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    internal class GameLogic
    {
        Game game;
        public GameLogic(Game game)
        {
            this.game = game;
        }
        public void CleanUpGameItems()
        {
            List<GameItem> removables = new();
            foreach (GameItem item in game.Items)
            {
                if (item.Available == false)
                    removables.Add(item);
            }

            foreach (GameItem item in removables)
            {
                game.Items.Remove(item);
            }
        }

        private void CleanUpDemons()
        {
            List<Demon> removables = new();
            foreach (Demon demon in game.Demons)
            {
                if (!demon.Alive)
                    removables.Add(demon);
            }

            foreach (Demon demon in removables)
            {
                game.Demons.Remove(demon);
            }
        }

        private void DemonMoveLogic(Demon demon)
        {
            Random RNG = new();
            int x = demon.Position.X + RNG.Next(-1, 2);
            int y = demon.Position.Y + RNG.Next(-1, 2);
            Position targetPosition = new Position(x, y);
            Move(demon, targetPosition);
        }

        private void UpdateDemons()
        {
            foreach (Demon demon in game.Demons)
            {
                demon.UpdateState(game.Player);
                if (demon.State == DemonStateType.Move)
                {
                    DemonMoveLogic(demon);
                }
                else if (demon.State == DemonStateType.Attack)
                {
                    DemonAttackLogic(demon);
                }
                DemonIndirectInteractionLogic(demon);
            }
        }

        private void DemonAttackLogic(Demon demon)
        {
            double distance = Position.Distance(demon.Position, game.Player.Position);
            Random r = new();
            int generatedDamage = 0;
            switch (demon.Type)
            {
                case DemonType.Zombieman:
                    generatedDamage = r.Next(3, 16);
                    break;
                case DemonType.Imp:
                    generatedDamage = r.Next(3, 25);
                    break;
                case DemonType.Mancubus:
                    generatedDamage = r.Next(8, 65);
                    break;
            }
            int damage = (2 * generatedDamage) / (1 + (int)distance);
            game.Player.TakeDamage(damage);
        }

        private List<GameItem> GetGameItemsWithinDistance(Position p, double distance)
        {
            List<GameItem> list = new List<GameItem>();
            foreach (GameItem item in game.Items)
            {
                if (Position.Distance(p, item.Position) <= distance)
                    list.Add(item);
            }
            return list;
        }

        private List<Demon> GetDemonsWithinDistance(Position position, double distance)
        {
            List<Demon> list = new();
            foreach (Demon demon in game.Demons)
            {
                if (Position.Distance(position, demon.Position) <= distance)
                {
                    list.Add(demon);
                }
            }
            return list;
        }

        private double GetTotalFillingratio(Position p)
        {
            List<GameItem> list = GetGameItemsWithinDistance(p, 0);
            double total = 0.0;
            foreach (GameItem item in list)
            {
                total += item.FillingRatio;
            }
            List<Demon> demonList = GetDemonsWithinDistance(p, 0);
            foreach (Demon demon in demonList)
            {
                total += demon.FillingRatio;
            }
            return total;
        }

        public void Move(Player player, Position vel)
        {
            double sum = GetTotalFillingratio(vel) + player.Fillingratio;
            if (sum <= 1.0)
                player.Position = vel;
        }

        public void Move(Demon demon, Position vel)
        {
            double sum = GetTotalFillingratio(vel) + demon.FillingRatio;
            if (sum <= 1.0)
                demon.Position = vel;
        }

        public void PlayerAttackLogic(Player player)
        {
            if (player.Ammo <= 0)
                return;

            player.Shoot();

            List<Demon> demons = GetDemonsWithinDistance(player.Position, player.SightRange);
            Random r = new();
            foreach (Demon demon in demons)
            {
                double distance = Position.Distance(player.Position, demon.Position);
                int damage = (2 * r.Next(35, 106)) / (1 + (int)distance);

                demon.TakeDamage(damage);

                if (!demon.Alive)
                {
                    switch (demon.Type)
                    {
                        case DemonType.Zombieman:
                            player.AddCombatPoints(1);
                            break;
                        case DemonType.Imp:
                            player.AddCombatPoints(3);
                            break;
                        case DemonType.Mancubus:
                            player.AddCombatPoints(10);
                            break;
                    }
                }
            }
        }

        public void UpdateGameState()
        {
            UpdateDemons();
            CleanUpGameItems();
            CleanUpDemons();
        }

        public void PlayerDirectInteractionLogic()
        {
            List<GameItem> items = GetGameItemsWithinDistance(game.Player.Position, 1);
            foreach(GameItem item in items)
            {
                switch(item.Type)
                {
                    case ItemType.Door:
                        item.Interact();
                        break;
                    case ItemType.LevelExit:
                        item.Interact();
                        game.Exited = true;
                        break;
                }
            }
        }

        public void PlayerIndirectInteractionLogic()
        {
            List<GameItem> items = GetGameItemsWithinDistance(game.Player.Position, 0);
            foreach (GameItem item in items)
            {
                switch (item.Type)
                {
                    case ItemType.Ammo:
                        item.Interact();
                        game.Player.PickUpAmmo(5);
                        break;
                    case ItemType.Medikit:
                        item.Interact();
                        game.Player.PickUpHealth(25);
                        break;
                    case ItemType.ToxicWaste:
                        item.Interact();
                        game.Player.TakeDamage(5);
                        break;
                }
            }
        }

        private void DemonIndirectInteractionLogic(Demon demon)
        {
            List<GameItem> items = GetGameItemsWithinDistance(demon.Position, 0);
            foreach (GameItem item in items)
            {
                switch (item.Type)
                {
                    case ItemType.ToxicWaste:
                        demon.TakeDamage(5);
                        break;
                }
            }
        }
    }
}