namespace doomslayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.LoadMapFromPlainText(@"../../../maps/pmp_arena.txt");
            game.Run();

            Console.ReadKey();
        }
    }
}
