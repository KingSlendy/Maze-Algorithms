using System.Collections.Generic;

namespace Maze_Algorithms {
    public abstract class Algorithms {
        public string[] Cardinal { get; } = { "N", "E", "W", "S" };
        public string[] Opposite { get; } = { "S", "W", "E", "N" };

        readonly Dictionary<string, (int row, int col)> locations = new Dictionary<string, (int row, int col)> {
            { "N", (-1, 0) },
            { "E", (0, 1) },
            { "W", (0, -1) },
            { "S", (1, 0) }
        };

        public (int, int) Direction(int row, int col, string cardinal) {
            (var newRow, var newCol) = (row + locations[cardinal].row, col + locations[cardinal].col);

            if (!newRow.IsInRange(0, Mazes.MazeHeight - 1) || !newCol.IsInRange(0, Mazes.MazeWidth - 1)) return (-1, -1);

            return (newRow, newCol);
        }

        public virtual int Adjacent(int row, int col, int[,] sets = null) {
            var start = Mazes.RNG.Next(Cardinal.Length);
            var adjacent = -1;

            for (var i = 0; i < Cardinal.Length; i++) {
                var check = (start + i) % Cardinal.Length;
                (var newRow, var newCol) = Direction(row, col, Cardinal[check]);

                if ((newRow, newCol) != (-1, -1) && !Mazes.Cells[newRow, newCol].Visited) {
                    adjacent = check;

                    break;
                }
            }

            return adjacent;
        }

        public Mazes.Cell Forge(int row, int col, int newRow, int newCol, int point) {
            Mazes.Cells[row, col].Visited = Mazes.Cells[row, col][Cardinal[point]] = true;
            Mazes.Cells[newRow, newCol].Visited = Mazes.Cells[newRow, newCol][Opposite[point]] = true;

            return Mazes.Cells[newRow, newCol];
        }

        public void Union(int row, int col, int num, int[,] sets) {
            foreach (var cardinal in Cardinal) {
                (var newRow, var newCol) = Direction(row, col, cardinal);

                if ((newRow, newCol) != (-1, -1) && Mazes.Cells[row, col][cardinal] && sets[newRow, newCol] != sets[row, col]) {
                    sets[newRow, newCol] = sets[row, col];
                    Union(newRow, newCol, num, sets);
                }
            }
        }

        public abstract void GenerateMaze();
    }
}