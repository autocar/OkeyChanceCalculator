using System;
using System.Collections.Generic;
using System.Linq;

namespace OkeyGame
{
    public class Services
    {
        public Services()
        {
        }

        public List<Tile> GenerateTilesList()

        {
            List<Tile> Tiles = new List<Tile>();
            int countId = 0;
            for (int j = 0; j < 2; j++)
            {
                
                for (int i = 0; i < 13; i++)
                {
                    Tiles.Add(new Tile(countId, Colors.Yellow, i + 1));
                    countId++;
                }

                for (int i = 0; i < 13; i++)
                {
                    Tiles.Add(new Tile(countId, Colors.Blue,i + 1 ));
                    countId++;
                }

                for (int i = 0; i < 13; i++)
                {
                    Tiles.Add(new Tile(countId, Colors.Black,i + 1));
                    countId++;

                }

                for (int i = 0; i < 13; i++)
                {
                    Tiles.Add(new Tile(countId, Colors.Red, i + 1));
                    countId++;

                }

                Tiles.Add(new Tile(countId, Colors.FakeJoker, -1));
                countId++;

            }

            return Tiles;
        }

        public List<Tile> ShuffleTiles(List<Tile> Tiles)
        {
            Random r = new Random();
            for (int i = Tiles.Count; i > 0; i--)
            {
                int j = r.Next(i);
                Tile k = Tiles[j];
                Tiles[j] = Tiles[i - 1];
                Tiles[i - 1] = k;
            }
            return Tiles;
        }

        public (List<Player>,List<Tile>) Distribute(List<Player> players, List<Tile> tiles )
        {

            int count = 0;
            for (int j = 0; j < 4; j++)
            {

                for (int i = 0; i < 14; i++)
                {
                    players[j].Tiles.Add(tiles[count]);
                    tiles[count].isUsed = true;
                    count++;
                }

            }

            Random rnd = new Random();
            int number = rnd.Next(0, 3);
            players[number].Tiles.Add(tiles[count]);
            tiles[count].isUsed=true;

            return (players, tiles);
        }

        public (List<Player>, List<Tile>) GenerateFakeandJokerTile(List<Tile> tiles,List<Player> players)
        {
            //Gosterge assigments
            Random rnd = new Random();
            var fakeRemovedTiles = tiles.Where(o=>o.Color!=Colors.FakeJoker & o.isUsed==true).ToList();
            var fakeTile = fakeRemovedTiles[rnd.Next(fakeRemovedTiles.Count)];
            Console.WriteLine(String.Format("Fake Tile Color: {0}  Value: {1}", fakeTile.Color, fakeTile.Value.ToString()));

            if(tiles.Where(o => o.Id == fakeTile.Id).Count() != 0) 
                 tiles.Where(o => o.Id == fakeTile.Id).First().isFakeJoker = true ;
            if(tiles.Where(o =>o.Id!=fakeTile.Id & o.Color==fakeTile.Color & o.Value==fakeTile.Value).Count()!=0)
                tiles.Where(o =>o.Id!=fakeTile.Id & o.Color==fakeTile.Color & o.Value==fakeTile.Value).Last().isFakeJoker = true;

            //Joker assigments
            if(fakeTile.Value==13 & tiles.Where(o => o.Value == 1 & o.Color == fakeTile.Color).Count() != 0)
            {
                tiles.Where(o => o.Value == 1 & o.Color == fakeTile.Color).First().isJoker = true;
                tiles.Where(o => o.Value == 1 & o.Color == fakeTile.Color).Last().isJoker = true;
                Console.WriteLine(String.Format("Joker Tile Color: {0}  Value: 1", fakeTile.Color));


            }
            else 
            {
                if (tiles.Where(o => o.Value == fakeTile.Value + 1 & o.Color == fakeTile.Color).Count() != 0)
                {

                    tiles.Where(o => o.Value == fakeTile.Value + 1 & o.Color == fakeTile.Color).First().isJoker = true;
                    tiles.Where(o => o.Value == fakeTile.Value + 1 & o.Color == fakeTile.Color).Last().isJoker = true;
                    Console.WriteLine(String.Format("Joker Tile Color: {0}  Value: {1}", fakeTile.Color,(fakeTile.Value+1).ToString()));

                }

            }

            //Player Hands reassigment for joker and gosterge
            foreach (Player player in players)
            {
                if (player.Tiles.Where(o => o.Id == fakeTile.Id).Count() != 0)
                {
                    player.Tiles.Where(o => o.Id == fakeTile.Id).First().isFakeJoker = true;
                    player.hasFakeJoker = true;
                }

                //Joker assigments
                if (fakeTile.Value == 13 & player.Tiles.Where(o => o.Value == 1 & o.Color == fakeTile.Color).Count() != 0)
                {
                    player.Tiles.Where(o => o.Value == 1 & o.Color == fakeTile.Color).First().isJoker = true;
                    player.Tiles.Where(o => o.Value == 1 & o.Color == fakeTile.Color).Last().isJoker = true;
                    player.jokerCount += 1;
                    Console.WriteLine(String.Format("Player has Joker Tile , Player Name: {0}  Id: {1}", player.Name,player.Id.ToString()));
                    


                }
                else
                {
                    if (player.Tiles.Where(o => o.Value == fakeTile.Value + 1 & o.Color == fakeTile.Color).Count() != 0)
                    {
                        player.Tiles.Where(o => o.Value == fakeTile.Value + 1 & o.Color == fakeTile.Color).First().isJoker = true;
                        player.Tiles.Where(o => o.Value == fakeTile.Value + 1 & o.Color == fakeTile.Color).Last().isJoker = true;
                        player.jokerCount += 1;
                        Console.WriteLine(String.Format("Player has Joker Tile , Player Name: {0}  Id: {1}", player.Name, player.Id.ToString()));

                    }

                }


            }



            return (players,tiles);

        }

        public List<Player> CalculateChanceToWin(List<Player> players)
        {

            foreach(Player player in players)
            {
                //Calculation of Joker Rank
                player.ChanceToWin += player.jokerCount * 40;

                //Calculation of fakeJoker Rank
                //if (player.hasFakeJoker == true)
                    //player.ChanceToWin += 10;

                //Caclulate Chance up to Gropued by Values like (Y1,B1,R1; B12,Y12,R12)
                var groupByValues = player.Tiles.GroupBy(o=>o.Value);
                foreach(var gr in groupByValues)
                {
                    if(gr.Count()>1)
                    {
                        player.ChanceToWin+=gr.Count()* gr.Count() * 5;
                    }
                }

                //Calculate chance by Grouped Color
                var groupByColor = player.Tiles.GroupBy(o=>o.Color);
                foreach(var gr in groupByColor)
                {
                    if(gr.Count()>1)
                    {
                        player.ChanceToWin += gr.Count() * gr.Count() * 5;

                    }

                }

            }

            return players.OrderBy(o => o.ChanceToWin).ToList();
        }
    }
}
