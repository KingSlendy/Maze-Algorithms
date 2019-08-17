using System.Linq;

namespace Maze_Algorithms {
    public class Kruskal : Algorithms {
        int[,] sets = Enumerable.Range(0, Mazes.MazeWidth * Mazes.MazeHeight).ToArray().ToArray2D(Mazes.MazeHeight, Mazes.MazeWidth);

        public Kruskal() => GenerateMaze();

        public override int Adjacent(int row, int col, int[,] sets) {
            var start = Mazes.RNG.Next(Cardinal.Length);
            var path = -1;

            for (var i = 0; i < Cardinal.Length; i++) {
                var check = (start + i) % Cardinal.Length;
                (var newRow, var newCol) = Direction(row, col, Cardinal[check]);

                if ((newRow, newCol) != (-1, -1) && sets[newRow, newCol] != sets[row, col]) {
                    sets[newRow, newCol] = sets[row, col];
                    Forge(row, col, newRow, newCol, check);
                    path = check;

                    break;
                }
            }

            return path;
        }

        public override async void GenerateMaze() {
            while (!sets.ToArray1D().All(x => x == sets[0, 0])) {
                await Mazes.PaintUpdate();

                (var row, var col) = (0, 0);
                var point = -1;

                while (point == -1) {
                    (row, col) = (Mazes.RNG.Next(Mazes.MazeHeight), Mazes.RNG.Next(Mazes.MazeWidth));
                    point = Adjacent(row, col, sets);
                }

                (var newRow, var newCol) = Direction(row, col, Cardinal[point]);
                Mazes.Current = Mazes.Cells[newRow, newCol];
                Union(newRow, newCol, sets[newRow, newCol], sets);
            }

            await Mazes.PaintUpdate(true);
        }
    }
}