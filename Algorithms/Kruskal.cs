using System.Linq;

namespace Maze_Algorithms {
    public class Kruskal {
        int[,] sets = Enumerable.Range(0, Mazes.MazeWidth * Mazes.MazeHeight).ToArray().ToArray2D(Mazes.MazeHeight, Mazes.MazeWidth);

        public Kruskal() {
            GenerateMaze();
        }

        async void GenerateMaze() {
            while (!sets.ToArray1D().All(x => x == sets[0, 0])) {
                await Mazes.PaintUpdate();

                (var row, var col) = (0, 0);
                var point = -1;

                while (point == -1) {
                    (row, col) = (Mazes.RNG.Next(Mazes.MazeHeight), Mazes.RNG.Next(Mazes.MazeWidth));
                    point = Mazes.Adjacent(row, col, sets);
                }

                (var newRow, var newCol) = Mazes.Direction(row, col, Mazes.Cardinal[point]);
                Mazes.Current = Mazes.Cells[newRow, newCol];
                Mazes.Union(newRow, newCol, sets[newRow, newCol], sets);
            }

            await Mazes.PaintUpdate(true);
        }
    }
}