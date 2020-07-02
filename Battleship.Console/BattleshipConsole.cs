using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Core;

namespace Battleship.Console
{
    public class BattleshipConsole
    {
        static void Main(string[] args)
        {
            int boardSize = 10;
            BattleshipEngine engine = new BattleshipEngine(boardSize);
            int iterations = 1000;
            var movesList = new List<int>();
            for(int i = 0; i < iterations; i++)
            {
                engine.Reset(boardSize);
                engine.StartComputer(ComputerAttackStategy.Random);
                System.Console.WriteLine(engine.GetBoardMoves());
                movesList.Add(engine.GetMoves());
            }
            System.Console.WriteLine("Results");
            System.Console.WriteLine($"Iterations: {iterations}");
            System.Console.WriteLine($"Average moves: {movesList.Average()}");
            System.Console.WriteLine($"Max moves: {movesList.Max()}");
            System.Console.WriteLine($"Min moves: {movesList.Min()}");
                      
            System.Console.ReadLine();
        }
    }
}

//Ideas for future
//
