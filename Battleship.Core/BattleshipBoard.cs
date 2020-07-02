using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Core
{
    public enum Player
    {
        Player1,
        Player2
    }
    public class BattleshipBoard
    {
        #region Properties
        private List<TileNode> _board;
        private int _size;
        private List<Ship> _playerShipsList;
        #endregion

        #region Properties
        public BattleshipBoard(int size = 8)
        {
            //Init common variables
            _size = size;
            
            //Reset the board as new
            ResetBoard();
        }
        #endregion

        #region Private Methods
        private void ResetBoard()
        {
            _board = new List<TileNode>();
            _playerShipsList = new List<Ship>();

            //Initialize TileNodes and Tiles
            for (int row = 0; row < _size; row++)
            {
                for (int column = 0; column < _size; column++)
                {
                    _board.Add(new TileNode(new Tile(row, column)));
                }
            }

            //Link TileNodes to each other
            foreach (TileNode node in _board)
            {
                //Link North
                var link = _board.Where(x => x.Tile.X == node.Tile.X - 1 && x.Tile.Y == node.Tile.Y);
                node.NorthNode = link.FirstOrDefault();

                //Link South
                link = _board.Where(x => x.Tile.X == node.Tile.X + 1 && x.Tile.Y == node.Tile.Y);
                node.SouthNode = link.FirstOrDefault();

                //Link West
                link = _board.Where(x => x.Tile.X == node.Tile.X && x.Tile.Y == node.Tile.Y - 1);
                node.WestNode = link.FirstOrDefault();

                //Link East
                link = _board.Where(x => x.Tile.X == node.Tile.X && x.Tile.Y == node.Tile.Y + 1);
                node.EastNode = link.FirstOrDefault();
            }
        }
        #endregion

        #region Public Methods    
        public string BoardCoordinatesString()
        {
            string result = string.Empty;
            foreach(var node in _board)
            {
                result += $"{node.Tile.CoordinateToString()}\n\tNorth:{node.NorthNode?.Tile.CoordinateToString()}\n\tSouth:{node.SouthNode?.Tile.CoordinateToString()}\n\tWest:{node.WestNode?.Tile.CoordinateToString()}\n\tEast{node.EastNode?.Tile.CoordinateToString()}\n\n";
            }

            return result;
        }

        public override string ToString()
        {
            string result = string.Empty;

            for(int i = 0; i < _size; i++)
            {
                for(int j = 0; j < _size; j++)
                {
                    result += $"{_board.First(x => x.Tile.X == j && x.Tile.Y == i).Tile.Code} ";
                }
                result += "\n";
            }

            return result;
        }

        public string CoordinatesToString()
        {
            string result = string.Empty;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    result += $"{_board.First(x => x.Tile.X == j && x.Tile.Y == i).Tile.CoordinateToString()} ";
                }
                result += "\n";
            }

            return result;
        }

        public string TileStateToString()
        {
            string result = string.Empty;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    result += $"{_board.First(x => x.Tile.X == j && x.Tile.Y == i).Tile.State.ToString()} ";
                }
                result += "\n";
            }

            return result;
        }

        public List<List<Tuple<int, int>>> GetAvailableShipPlacements(int boatSize)
        {
            List<List<Tuple<int, int>>> placements = new List<List<Tuple<int, int>>>(); 
            foreach (var node in _board)
            {
                List<Tuple<int, int>> northCoordinates = new List<Tuple<int, int>>();
                List<Tuple<int, int>> southCoordinates = new List<Tuple<int, int>>();
                List<Tuple<int, int>> eastCoordinates = new List<Tuple<int, int>>();
                List<Tuple<int, int>> westCoordinates = new List<Tuple<int, int>>();

                northCoordinates.Add(new Tuple<int, int>(node.Tile.X, node.Tile.Y));
                southCoordinates.Add(new Tuple<int, int>(node.Tile.X, node.Tile.Y));
                eastCoordinates.Add(new Tuple<int, int>(node.Tile.X, node.Tile.Y));
                westCoordinates.Add(new Tuple<int, int>(node.Tile.X, node.Tile.Y));

                TileNode currentNorthNode = node;
                TileNode currentSouthNode = node;
                TileNode currentEastNode = node;
                TileNode currentWestNode = node;
                for (int i = 1; i < boatSize; i++)
                {
                    //Check North
                    if (currentNorthNode.NorthNode != null && currentNorthNode.NorthNode.Tile.Code.ToString() == SymbolResource.Empty && currentNorthNode.NorthNode.Tile.State == TileState.Empty)
                    {
                        currentNorthNode = currentNorthNode.NorthNode;
                        northCoordinates.Add(new Tuple<int, int>(currentNorthNode.Tile.X, currentNorthNode.Tile.Y));
                    }
                    else
                    {
                        northCoordinates.Clear();
                    }

                    //Check South
                    if (currentSouthNode.SouthNode != null && currentSouthNode.SouthNode.Tile.Code.ToString() == SymbolResource.Empty && currentSouthNode.SouthNode.Tile.State == TileState.Empty)
                    {
                        currentSouthNode = currentSouthNode.SouthNode;
                        southCoordinates.Add(new Tuple<int, int>(currentSouthNode.Tile.X, currentSouthNode.Tile.Y));
                    }
                    else
                    {
                        southCoordinates.Clear();
                    }

                    //Check East
                    if (currentEastNode.EastNode != null && currentEastNode.EastNode.Tile.Code.ToString() == SymbolResource.Empty && currentEastNode.EastNode.Tile.State == TileState.Empty)
                    {
                        currentEastNode = currentEastNode.EastNode;
                        eastCoordinates.Add(new Tuple<int, int>(currentEastNode.Tile.X, currentEastNode.Tile.Y));
                    }
                    else
                    {
                        eastCoordinates.Clear();
                    }

                    //Check West
                    if (currentWestNode.WestNode != null && currentWestNode.WestNode.Tile.Code.ToString() == SymbolResource.Empty && currentWestNode.WestNode.Tile.State == TileState.Empty)
                    {
                        currentWestNode = currentWestNode.WestNode;
                        westCoordinates.Add(new Tuple<int, int>(currentWestNode.Tile.X, currentWestNode.Tile.Y));
                    }
                    else
                    {
                        westCoordinates.Clear();
                    }
                }

                if (northCoordinates.Count == boatSize)
                    placements.Add(northCoordinates);
                if (southCoordinates.Count == boatSize)
                    placements.Add(southCoordinates);
                if (eastCoordinates.Count == boatSize)
                    placements.Add(eastCoordinates);
                if (westCoordinates.Count == boatSize)
                    placements.Add(westCoordinates);
            }

            return placements;
        }

        public void PlaceShip(Ship ship)
        {
            _playerShipsList.Add(ship);
            foreach(var shipTile in ship.Tiles)
            {
                _board.First(x => x.Tile.X == shipTile.X && x.Tile.Y == shipTile.Y).Tile.Code = shipTile.Code;
                _board.First(x => x.Tile.X == shipTile.X && x.Tile.Y == shipTile.Y).Tile.State = TileState.Ship;
            }
        }

        public bool IsValidAttack(int x, int y)
        {
            return _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.State == TileState.Empty || _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.State == TileState.Ship;
        }

        public void Attack(int x, int y)
        {
           
            if (_board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.State == TileState.Ship)
            {
                _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.Code = SymbolResource.Hit;
                _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.State = TileState.Hit;
            }
            else
            {
                _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.Code = SymbolResource.Miss;
                _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.State = TileState.Miss;
            }

            foreach (var ship in _playerShipsList)
            {
                foreach (var tile in ship.Tiles)
                {
                    if (tile.X == x && tile.Y == y)
                    {
                        tile.State = TileState.Hit;
                    }
                }
            }

        }

        public bool AllShipsSunk()
        {
            return _playerShipsList.All(x => x.IsSunk());
        }

        public int MaximumBoats()
        {
            return _size / 2;
        }

        public List<TileNode> GetAvailableMoves()
        {
            return _board.Where(x => x.Tile.GetTileState(true) == TileState.Empty).ToList();
        }

        public TileState GetTileStatebyPosition(int x, int y)
        {
            return _board.First(t => t.Tile.X == x && t.Tile.Y == y).Tile.State;
        }

        public int GetBoardSize()
        {
            return _size;
        }
        #endregion


    }
}
