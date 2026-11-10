namespace doomslayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Position p1 = new(1,2);
            //Position p2 = new(3,4);

            //Position p3 = Position.Add(p1, p2);
            //float number = Position.Distance(p1,p2);
            //Console.WriteLine(p3.ToString());
            //Console.WriteLine(number);

            //ConsoleSprite sprite = new(ConsoleColor.Red, ConsoleColor.DarkRed, 'O');
            //Console.BackgroundColor = sprite.Background;
            //Console.ForegroundColor = sprite.Foreground;
            //Console.WriteLine(sprite.Glyph);

            //Player player = new(20,10);
            //Console.SetCursorPosition(player.Position.X, player.Position.Y);
            //Console.BackgroundColor = player.Sprite.Background;
            //Console.ForegroundColor = player.Sprite.Foreground;
            //Console.WriteLine(player.Sprite.Glyph);

            Game game = new Game();
            game.Items.Add(new GameItem(10,5, ItemType.Door));
            game.Items.Add(new GameItem(10,10, ItemType.Medikit));
            game.Items.Add(new GameItem(20,15, ItemType.Ammo));
            game.Items.Add(new GameItem(30,20, ItemType.Wall));
            game.Items.Add(new GameItem(20,0, ItemType.ToxicWaste));
            game._demons.Add(new Demon(5, 10, DemonType.Zombieman));
            game._demons.Add(new Demon(5, 15, DemonType.Imp));
            game._demons.Add(new Demon(5, 20, DemonType.Mancubus));
            game.Run();

            Console.ReadKey();
        }
    }
}
