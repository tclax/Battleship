using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Core
{
    public class Ship
    {

        #region Properties
        public List<Tile> Tiles;
        #endregion

        #region Constructor
        public Ship(List<Tile> tiles)
        {
            Tiles = tiles;
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public bool IsSunk()
        {
            return Tiles.All(x => x.State == TileState.Hit);
        }

        public override string ToString()
        {
            return Tiles.Any(x => x.State == TileState.Ship) ? "Alive" : "Sunk";
        }
        #endregion
    }
}
