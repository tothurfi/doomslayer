using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doomslayer
{
    internal class ConsoleRenderer
    {
        Game game;
        public ConsoleRenderer(Game game)
        {
            this.game = game;
        }
        private void RenderSingleSprite(Position p, ConsoleSprite s)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X < Console.WindowWidth && p.Y < Console.WindowHeight)
            {
                Console.BackgroundColor = s.Background;
                Console.ForegroundColor = s.Foreground;
                Console.SetCursorPosition(p.X, p.Y);
                Console.WriteLine(s.Glyph);
            }
        }

        private void RenderHUD()
        {
            Player player = game.Player;
            string hud = $"HP: {player.Health}/{player.MaxHealth} | " +
                $"Lőszer: {player.Ammo}/{player.MaxAmmo} | " +
                $"BFG: {player.BFGCells}/{player.MaxBFGCells} | " +
                $"CP: {player.CombatPoints}";

            Console.SetCursorPosition(0, Console.WindowHeight - 1);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(hud);
        }

        public void RenderGame()
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();
            Player player = game.Player;

            for (int i = 0; i < game.Items.Count; i++)
            {
                double distance = Position.Distance(player.Position, game.Items[i].Position);
                if(distance <= player.SightRange)
                    RenderSingleSprite(game.Items[i].Position, game.Items[i].Sprite);
            }
            for (int i = 0; i < game.Demons.Count; i++)
            {
                double distance = Position.Distance(player.Position, game.Demons[i].Position);
                if (distance <= player.SightRange)
                    RenderSingleSprite(game.Demons[i].Position, game.Demons[i].Sprite);
            }
            RenderSingleSprite(game.Player.Position, game.Player.Sprite);
            RenderHUD();
        }
    }
}
