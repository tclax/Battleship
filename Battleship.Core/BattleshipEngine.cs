using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Core
{
    public enum LoggingLevel
    {
        Consolidated,
        Detail,
    }

    public enum ComputerAttackStategy
    {
        Random,
        Horizonal,
        Vertical,
        Diagonal
    }

    public class BattleshipEngine
    {
        #region Properties
        private BattleshipBoard _board;
        private Random _random;
        private string _movesString;
        private int _moves;
        private Tuple<int, int> _previousMoveCoordinates; 
        #endregion

        #region Constructor
        public BattleshipEngine(int boardSize = 10)
        {
            _board = new BattleshipBoard(boardSize);
            InitProperties();
            PlaceShips();
        }
        #endregion

        #region Private Methods
        private void InitProperties()
        {
            _random = new Random();
            _movesString = string.Empty;
            _moves = 0;
            _previousMoveCoordinates = null;
        }
        private void PlaceShips()
        {
            //Consider allowing the user to set how many ships and kind of ships to be added to the board
            List<Tuple<char, int>> shipSizeList = new List<Tuple<char, int>>() { new Tuple<char, int>('C', 5), new Tuple<char, int>('B', 4),
                new Tuple<char, int>('S', 3), new Tuple<char, int>('Z', 3), new Tuple<char, int>('D', 2)};

            foreach (var tuple in shipSizeList)
            {
                var placements = _board.GetAvailableShipPlacements(tuple.Item2);
                var boatPlacement = placements[_random.Next(placements.Count)];
                var tiles = new List<Tile>();
                foreach (var placement in boatPlacement)
                {
                    tiles.Add(new Tile(placement.Item1, placement.Item2, tuple.Item1.ToString()));
                }
                _board.PlaceShip(new Ship(tiles));
            }
        }

        private void Log(string message)
        {
            _movesString += message;
        }

        public void StartComputer()
        {
            _movesString = string.Empty;
            _moves = 0;
            while (!_board.AllShipsSunk())
            {
                Log($"Move: {_moves}\n{_board.ToString()}\n");
                //Log($"Move: {_moves}\n");
                int x = -1;
                int y = -1;
                do
                {
                    var coordinates = RandomAttack();
                    //var coordinates = DiagonalRandomAttack();
                    x = coordinates.Item1;
                    y = coordinates.Item2;
                } while (!_board.IsValidAttack(x, y));
                Log($"Making attack at ({x}, {y})");
                _board.Attack(x, y);
                Log($"{_board.GetTileStatebyPosition(x, y).ToString()}\n");
                _moves++;
            }
            Log("################\n");
            Log($"Completed battleship game in {_moves} moves.\n");
        }

        public void StartComputer(ComputerAttackStategy computerAttackStategy)
        {
            _movesString = string.Empty;
            _moves = 0;
            while (!_board.AllShipsSunk())
            {
                Log($"Move: {_moves}\n{_board.ToString()}\n");
                int x = -1;
                int y = -1;
                do
                {
                    Tuple<int, int> coordinates;
                    switch (computerAttackStategy)
                    {
                        case ComputerAttackStategy.Random:
                            coordinates = RandomAttack();
                            break;
                        case ComputerAttackStategy.Horizonal:
                            coordinates = DiagonalRandomAttack();
                            break;
                        case ComputerAttackStategy.Vertical:
                        case ComputerAttackStategy.Diagonal:
                        default:
                            coordinates = RandomAttack();
                            break;
                    }
                    x = coordinates.Item1;
                    y = coordinates.Item2;
                } while (!_board.IsValidAttack(x, y));
                Log($"Making attack at ({x}, {y})");
                _board.Attack(x, y);
                Log($"{_board.GetTileStatebyPosition(x, y).ToString()}\n");
                _moves++;
            }
            Log("################\n");
            Log($"Completed battleship game in {_moves} moves.\n");
        }

        //Randomly chooses an available tile that is empty
        private Tuple<int, int> RandomAttack()
        {
            var availableMoves = _board.GetAvailableMoves();
            var randomMove = availableMoves[_random.Next(availableMoves.Count)];
            return new Tuple<int, int>(randomMove.Tile.X, randomMove.Tile.Y);
        }

        //Picks a random starting point and moves diagonally 
        private Tuple<int, int> DiagonalRandomAttack()
        {
            if(_previousMoveCoordinates == null)
            {
                
                _previousMoveCoordinates = new Tuple<int, int>(_random.Next(_board.GetBoardSize()), _random.Next(_board.GetBoardSize()));
            }
            else
            {
                int x = _previousMoveCoordinates.Item1;
                int y = _previousMoveCoordinates.Item2;
                if(x == 0 && y == _board.GetBoardSize()-1)
                {
                    _previousMoveCoordinates = new Tuple<int, int>(_board.GetBoardSize()-1, 1);
                }
                else if(x == _board.GetBoardSize() - 1 && y == _board.GetBoardSize() - 1)
                {
                    _previousMoveCoordinates = new Tuple<int, int>(0, 0);
                }
                else if(x == 0)
                {
                    _previousMoveCoordinates = new Tuple<int, int>(y+1, 0);
                }
                else if(y == _board.GetBoardSize()-1)
                {
                    _previousMoveCoordinates = new Tuple<int, int>(_board.GetBoardSize()-1, x+1);
                }
                else
                {
                    _previousMoveCoordinates = new Tuple<int, int>(x-1, y+1);
                }
            }

            return _previousMoveCoordinates;
        }

        private Tuple<int, int> HorizonalLinearAttack()
        {
            if(_previousMoveCoordinates == null)
            {
                _previousMoveCoordinates = new Tuple<int, int>(0, 0);
            }
            else
            {
                int x = _previousMoveCoordinates.Item1;
                int y = _previousMoveCoordinates.Item2;
                if(x == _board.GetBoardSize() - 1)
                {
                    _previousMoveCoordinates = new Tuple<int, int>(0, y + 1);
                }
                else
                {
                    _previousMoveCoordinates = new Tuple<int, int>(x + 1, y);
                }
            }

            return _previousMoveCoordinates;
        }

        private Tuple<int, int> VerticalLinearAttack()
        {
            if (_previousMoveCoordinates == null)
            {
                _previousMoveCoordinates = new Tuple<int, int>(0, 0);
            }
            else
            {
                int x = _previousMoveCoordinates.Item1;
                int y = _previousMoveCoordinates.Item2;
                if (y == _board.GetBoardSize() - 1)
                {
                    _previousMoveCoordinates = new Tuple<int, int>(x + 1, 0);
                }
                else
                {
                    _previousMoveCoordinates = new Tuple<int, int>(x, y + 1);
                }
            }

            return _previousMoveCoordinates;
        }

        //Once it find a hit it will seek for the ship until 
        //private Tuple<int, int> SeekAndDestroyAttack()
        //{

        //}

        #endregion

        #region Public Methods
        public string BoardToString()
        {
            return _board.ToString();
        }

        public void Reset(int boardSize = 10)
        {
            _board = new BattleshipBoard(boardSize);
            InitProperties();
            PlaceShips();
        }

        public int GetMoves()
        {
            return _moves;
        }

        public string GetBoardMoves()
        {
            return _movesString;
        }
        #endregion
    }
}
