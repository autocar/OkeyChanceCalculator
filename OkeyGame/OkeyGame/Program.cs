using System;
using System.Collections.Generic;
using System.Linq;
namespace OkeyGame
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Services services = new Services();
            List<Player> Players = new List<Player>();
            List<Tile> Tiles = new List<Tile>();

            //Generate Players
            Players.Add(new Player(1,"Ali"));
            Players.Add(new Player(2,"Veli"));
            Players.Add(new Player(3,"Fatma"));
            Players.Add(new Player(4,"Ayşe"));

            //Generate Tiles
            Tiles = services.GenerateTilesList();

            //Shuffle Tiles to Distribute
            Tiles = services.ShuffleTiles(Tiles);

            //Distribute tiles to players
            (Players,Tiles)= services.Distribute(Players, Tiles);

            // Generate FakeJoker Tile,assig Joker to tiles and players
            (Players, Tiles) = services.GenerateFakeandJokerTile(Tiles,Players);

            //Calculation of Chance to WIN
            Players = services.CalculateChanceToWin(Players);

            Console.WriteLine("*********  Score of Players  *********\r\n");
            foreach(Player player in Players)
            {
                Console.WriteLine(String.Format("{0} has a Chance with Score: {1}",player.Name,player.ChanceToWin));
            }

            Console.WriteLine(String.Format("\r\n******** The Winner is {0} with Score: {1}  ********", Players[3].Name,Players[3].ChanceToWin));
        }
    }
}
