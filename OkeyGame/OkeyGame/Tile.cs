using System;
using System.Collections;
namespace OkeyGame
{
    public class Tile
    {
        public int Id;
        public Colors Color;
        public int Value;
        public bool isJoker = false;
        public bool isFakeJoker=false;
        public bool isUsed = false;

        public Tile(int Id,Colors Color,int Value)
        {
            this.Id = Id;
            this.Color = Color;
            this.Value = Value;

        }

        public int getId(Tile t)
        {
            return t.Id;
        }
        public int getValue(Tile t)
        {
            return t.Value;
        }

        public Colors getColor(Tile t)
        {
            return t.Color;
        }

      

      
    }
}
