using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Core
{
    public enum TileState
    {
        Empty,
        Hit,
        Miss,
        Ship
    }
    public class Tile
    {
        #region Properties
        public int X { get; set; }
        public int Y { get; set; }
        public string Code { get; set; }
        public TileState State { get; set; }
        #endregion

        #region Constructor
        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            State = TileState.Empty;
            Code = SymbolResource.Empty;
        }

        public Tile(int x, int y, string code)
        {
            X = x;
            Y = y;
            State = TileState.Ship;
            Code = code;
        }
        #endregion

        #region Private Methods
        #endregion


        #region Public Methods
        public string CoordinateToString()
        {
            return $"({X}, {Y})";
        }

        //restricted view will hide ships as empty tiles until they are hit
        public string CodeToString(bool restrictedView = false)
        {
            string result;
            if (restrictedView)
            {
                switch (State)
                {
                    case TileState.Empty:
                    case TileState.Ship:
                        result = SymbolResource.Empty;
                        break;
                    case TileState.Hit:
                        result = SymbolResource.Hit;
                        break;
                    case TileState.Miss:
                        result = SymbolResource.Miss;
                        break;
                    default:
                        result = "$";
                        break;
                }
            }
            else
            {
                switch (State)
                {
                    case TileState.Empty:
                        result = SymbolResource.Empty;
                        break;
                    case TileState.Hit:
                        result = SymbolResource.Hit;
                        break;
                    case TileState.Miss:
                        result = SymbolResource.Miss;
                        break;
                    case TileState.Ship:
                        result = Code.ToString();
                        break;
                    default:
                        result = "$";
                        break;
                }
            }

            return result;
        }

        public TileState GetTileState(bool restrictedView = false)
        {
            TileState state;

            if (restrictedView)
            {
                switch (State)
                {
                    case TileState.Empty:
                    case TileState.Ship:
                        state = TileState.Empty;
                        break;
                    case TileState.Hit:
                        state = TileState.Hit;
                        break;
                    case TileState.Miss:
                        state = TileState.Miss;
                        break;
                    default:
                        state = TileState.Empty;
                        break;
                }
            }
            else
            {
                switch (State)
                {
                    case TileState.Empty:
                        state = TileState.Empty;
                        break;
                    case TileState.Ship:
                        state = TileState.Ship;
                        break;
                    case TileState.Hit:
                        state = TileState.Hit;
                        break;
                    case TileState.Miss:
                        state = TileState.Miss;
                        break;
                    default:
                        state = TileState.Empty;
                        break;
                }
            }

            return state;
        }
        #endregion
    }
}
