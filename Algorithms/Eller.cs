using System.Linq;

namespace Maze_Algorithms {
    public class Eller : Algorithms {
        readonly int[,] sets = Enumerable.Range(0, Mazes.MazeWidth * Mazes.MazeHeight).ToArray().ToArray2D(Mazes.MazeHeight, Mazes.MazeWidth);

        public Eller() {
            Mazes.Current = Mazes.Cells[0, 0];
            GenerateMaze();
        }

        public override async void GenerateMaze() {
            void JoinSet(int row, int col, int newRow, int newCol, ref bool joined) {
                Forge(row, col, newRow, newCol, 0);
                Union(row, col, sets[row, col], sets);
                joined = true;
            }

            for (var r = 0; r < Mazes.MazeHeight; r++) {
                if (r != 0) {
                    var joined = false;

                    for (var c = Mazes.MazeWidth - 1; c >= 0; c--) {
                        await Mazes.PaintUpdate();

                        Mazes.Current = Mazes.Cells[r, c];
                        (var setRow, var setCol) = Direction(r, c, "N");
                        (var checkRow, var checkCol) = Direction(r, c - 1, "N");

                        if (c == Mazes.MazeWidth - 1) sets[r, c] = sets[setRow, setCol];
                        if (c == 0 && !joined || (checkRow, checkCol) != (-1, -1) && !joined && sets[checkRow, checkCol] != sets[r, c] || Mazes.RNG.Next(5) == 0)
                            JoinSet(r, c, setRow, setCol, ref joined);
                        
                        if ((checkRow, checkCol) != (-1, -1) && sets[checkRow, checkCol] != sets[setRow, setCol]) joined = false;
                    }
                }

                for (var c = 0; c < Mazes.MazeWidth; c++) {
                    Mazes.Cells[r, c].Visited = true;

                    await Mazes.PaintUpdate();

                    if (Mazes.RNG.Next(2) == 0 || r == Mazes.MazeHeight - 1) {
                        (var row, var col) = Direction(r, c, "E");

                        if ((row, col) != (-1, -1) && sets[row, col] != sets[r, c]) {
                            Forge(r, c, row, col, 1);
                            Union(row, col, sets[row, col] = sets[r, c], sets);
                        }
                    }

                    Mazes.Current = Mazes.Cells[r, c];
                }
            }

            await Mazes.PaintUpdate(true);
        }
    }
}