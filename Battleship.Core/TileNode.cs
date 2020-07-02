using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Core
{
    public class TileNode
    {
        public Tile Tile { get; set; }

        public TileNode NorthNode { get; set; }
        public TileNode SouthNode { get; set; }
        public TileNode WestNode { get; set; }
        public TileNode EastNode { get; set; }

        public TileNode(Tile tile)
        {
            Tile = tile;
        }

    }
}
