﻿using Game.Objects;
using Game.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings
{
    public class QFunction
    {
        static readonly double DEAFULT_VALUE = 0.6;
        public static List<QValue> TabluarQFunction { get; set; } = new List<QValue>();

        public static void GenerateTabularQFunction()
        { 
            var b = new Board();
            TabluarQFunction.Add(new QValue
            {
                State = b.GetHashCode(),
                Actions = InitializeActions(b.GetAvailableMoves())
            });
            ExploreStates(b);    
        }
        public static void ExploreStates(Board board)
        {   
            if (!(board.GetGameState().Item1 == Board.GameState.Finished))
            {
                foreach (var move in board.GetAvailableMoves())
                {
                    var copy = board.GetBoardCopy();
                    copy.MakeMove(move);
                    var hash = copy.GetHashCode();
                    if(!TabluarQFunction.Any(x=> x.State==hash))
                    {
                        TabluarQFunction.Add( new QValue
                        {
                            State = hash,
                            Actions = InitializeActions(copy.GetAvailableMoves())

                        });
                    }
                    ExploreStates(copy);
                }
            }
        }

        static int GetIndexOfQValue(Tuple<int, int> move)
        {
            return move.Item2 + (move.Item1 * Board.DEFAULT_SIZE);
        }

        static double?[] InitializeActions(IList<Tuple<int, int>> moves)
        {
            var actions = new double?[Board.DEFAULT_SIZE * Board.DEFAULT_SIZE];
            foreach(var m in moves)
            {
                var index = GetIndexOfQValue(m);
                actions[index] = DEAFULT_VALUE;
            }
            return actions;
        }
    }
}