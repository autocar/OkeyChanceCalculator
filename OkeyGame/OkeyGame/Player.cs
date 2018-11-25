using System;
using System.Collections.Generic;
namespace OkeyGame
{
    public class Player
    {
        public int Id;
        public string Name;
        public Player(int Id,string Name)
        {
            this.Name = Name;
            this.Id = Id;
        }

        public List<Tile> Tiles = new List<Tile>();
        public int ChanceToWin { get; set; } = 0;
        public int jokerCount { get; set; } =0;
        public bool hasFakeJoker { get; set; } = false;
    }
}
