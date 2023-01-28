using System;
using System.Collections.Generic;

namespace _2048
{
    class Game
    {
        public ulong Score { get; private set; }
        public ulong[,] Board { get; private set; }

        private readonly int nRows;
        private readonly int nCols;
        private readonly Random random = new Random();

        public Game()
        {
            Board = new ulong[4, 4];
            nRows = Board.GetLength(0);
            nCols = Board.GetLength(1);
            Score = 0;
            PutNewValue();
            PutNewValue();
        }

        enum Direction
        {
            Up = 0,
            Down,
            Right,
            Left,
        }
        public int Run(System.Windows.Input.Key key)
        {
            bool hasUpdated;

            //while (true)
            //{
                //ConsoleKeyInfo input = Console.ReadKey(true); // BLOCKING TO WAIT FOR INPUT

            switch (key)
            {
                case System.Windows.Input.Key.Up:
                    hasUpdated = Update(Direction.Up);
                    break;

                case System.Windows.Input.Key.Down:
                    hasUpdated = Update(Direction.Down);
                    break;

                case System.Windows.Input.Key.Left:
                    hasUpdated = Update(Direction.Left);
                    break;

                case System.Windows.Input.Key.Right:
                    hasUpdated = Update(Direction.Right);
                    break;

                default:
                    hasUpdated = false;
                    break;
            }

            if (hasUpdated)
            {
                PutNewValue();
            }

            if (IsDead())
            {
                return 1;
            }
            return 0;
            //}
        }

        private static bool Update(ulong[,] board, Direction direction, out ulong score)
        {
            int nRows = board.GetLength(0);
            int nCols = nRows;

            score = 0;
            bool hasUpdated = false;

            // You shouldn't be dead at this point. We always check if you're dead at the end of the Update()

            // Drop along row or column? true: process inner along row; false: process inner along column
            bool isAlongRow = direction == Direction.Left || direction == Direction.Right;

            // Should we process inner dimension in increasing index order?
            bool isIncreasing = direction == Direction.Left || direction == Direction.Up;

            int outterCount = isAlongRow ? nRows : nCols;
            int innerCount = isAlongRow ? nCols : nRows;
            int innerStart = isIncreasing ? 0 : innerCount - 1;
            int innerEnd = isIncreasing ? innerCount - 1 : 0;

            Func<int, int> drop = isIncreasing
                ? new Func<int, int>(innerIndex => innerIndex - 1)
                : new Func<int, int>(innerIndex => innerIndex + 1);

            Func<int, int> reverseDrop = isIncreasing
                ? new Func<int, int>(innerIndex => innerIndex + 1)
                : new Func<int, int>(innerIndex => innerIndex - 1);

            Func<ulong[,], int, int, ulong> getValue = isAlongRow
                ? new Func<ulong[,], int, int, ulong>((x, i, j) => x[i, j])
                : new Func<ulong[,], int, int, ulong>((x, i, j) => x[j, i]);

            Action<ulong[,], int, int, ulong> setValue = isAlongRow
                ? new Action<ulong[,], int, int, ulong>((x, i, j, v) => x[i, j] = v)
                : new Action<ulong[,], int, int, ulong>((x, i, j, v) => x[j, i] = v);

            Func<int, bool> innerCondition = index => Math.Min(innerStart, innerEnd) <= index && index <= Math.Max(innerStart, innerEnd);

            for (int i = 0; i < outterCount; i++)
            {
                for (int j = innerStart; innerCondition(j); j = reverseDrop(j))
                {
                    if (getValue(board, i, j) == 0)
                    {
                        continue;
                    }

                    int newJ = j;
                    do
                    {
                        newJ = drop(newJ);
                    }
                    // Continue probing along as long as we haven't hit the boundary and the new position isn't occupied
                    while (innerCondition(newJ) && getValue(board, i, newJ) == 0);

                    // We did not hit the canvas boundary (we hit a node) AND no previous merge occurred AND the nodes' values are the same
                    if (innerCondition(newJ) && getValue(board, i, newJ) == getValue(board, i, j))
                    {
                        // Let's merge
                        ulong newValue = getValue(board, i, newJ) * 2;
                        setValue(board, i, newJ, newValue);
                        setValue(board, i, j, 0);

                        hasUpdated = true;
                        score += newValue;
                    }
                    else
                    {
                        // Reached the boundary OR...
                        // we hit a node with different value OR...
                        // we hit a node with same value BUT a previous merge had occurred
                        // 
                        // Simply stack along
                        newJ = reverseDrop(newJ); // reverse back to its valid position
                        if (newJ != j)
                        {
                            // there's an update
                            hasUpdated = true;
                        }

                        //prover'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ulong value = getValue(board, i, j);
                        setValue(board, i, j, 0);
                        setValue(board, i, newJ, value);
                    }
                }
            }

            return hasUpdated;
        }

        private bool Update(Direction dir)
        {
            ulong score;
            bool isUpdated = Update(Board, dir, out score);
            Score += score;
            return isUpdated;
        }

        private bool IsDead()
        {
            ulong score;
            foreach (Direction dir in new Direction[] { Direction.Down, Direction.Up, Direction.Left, Direction.Right })
            {
                ulong[,] clone = (ulong[,])Board.Clone();
                if (Game.Update(clone, dir, out score))
                {
                    return false;
                }
            }

            return true;
        }

        private void PutNewValue()
        {
            // Find all empty slots
            List<Tuple<int, int>> emptySlots = new();
            for (int iRow = 0; iRow < nRows; iRow++)
            {
                for (int iCol = 0; iCol < nCols; iCol++)
                {
                    if (Board[iRow, iCol] == 0)
                    {
                        emptySlots.Add(new Tuple<int, int>(iRow, iCol));
                    }
                }
            }

            // We should have at least 1 empty slot. Since we know the user is not dead
            int iSlot = random.Next(0, emptySlots.Count); // randomly pick an empty slot
            ulong value = random.Next(0, 100) < 95 ? (ulong)2 : (ulong)4; // randomly pick 2 (with 95% chance) or 4 (rest of the chance)
            Board[emptySlots[iSlot].Item1, emptySlots[iSlot].Item2] = value;
        }
    }
}