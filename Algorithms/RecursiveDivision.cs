using System.Threading.Tasks;

namespace Maze_Algorithms {
    public class RecursiveDivision {
        public RecursiveDivision() {
            for (var r = 0; r < Mazes.MazeHeight; r++) {
                for (var c = 0; c < Mazes.MazeWidth; c++) {
                    Mazes.Cells[r, c].Visited = true;

                    for (var d = 0; d < Mazes.Cardinal.Length; d++)
                        Mazes.Cells[r, c][Mazes.Cardinal[d]] = true;

                    if (r == 0) Mazes.Cells[r, c]["N"] = false;
                    if (c == Mazes.MazeWidth - 1) Mazes.Cells[r, c]["E"] = false;
                    if (c == 0) Mazes.Cells[r, c]["W"] = false;
                    if (r == Mazes.MazeHeight - 1) Mazes.Cells[r, c]["S"] = false;
                }
            }

            GenerateMaze();
        }

        async void GenerateMaze() {
            int ChooseOrientation(int width, int height) {
                return (width < height) ? 0 : (height < width) ? 1 : Mazes.RNG.Next(2);
            }

            async Task Division(int row, int col, int width, int height, int orientation) {
                if (width < 2 || height < 2) return;

                await Mazes.PaintUpdate();

                var horizontal = (orientation == 0);

                var wallRow = row + horizontal.ThenDefault(Mazes.RNG.Next(height - 2));
                var wallCol = col + (!horizontal).ThenDefault(Mazes.RNG.Next(width - 2));
                var passRow = wallRow + (!horizontal).ThenDefault(Mazes.RNG.Next(height));
                var passCol = wallCol + horizontal.ThenDefault(Mazes.RNG.Next(width));
                (var dirRow, var dirCol) = (horizontal) ? (0, 1) : (1, 0);

                var length = (horizontal) ? width : height;
                var dir = (horizontal) ? 3 : 1;

                for (var i = 0; i < length; i++) {
                    Mazes.Current = Mazes.Cells[wallRow, wallCol];
                    Mazes.Current.Visited = true;

                    await Mazes.PaintUpdate();

                    if (wallRow != passRow || wallCol != passCol) {
                        Mazes.Cells[wallRow, wallCol][Mazes.Cardinal[dir]] = false;
                        (var newRow, var newCol) = Mazes.Direction(wallRow, wallCol, Mazes.Cardinal[dir]);
                        Mazes.Cells[newRow, newCol][Mazes.Opposite[dir]] = false;
                    }

                    wallRow += dirRow;
                    wallCol += dirCol;
                }

                (var nextWidth, var nextHeight) = (horizontal) ? (width, wallRow - row + 1) : (wallCol - col + 1, height);

                await Division(row, col, nextWidth, nextHeight, ChooseOrientation(nextWidth, nextHeight));

                (var nextRow, var nextCol) = (horizontal) ? (wallRow + 1, col) : (row, wallCol + 1);
                (nextWidth, nextHeight) = (horizontal) ? (width, row + height - wallRow - 1) : (col + width - wallCol - 1, height);

                await Division(nextRow, nextCol, nextWidth, nextHeight, ChooseOrientation(nextWidth, nextHeight));
            }

            await Division(0, 0, Mazes.MazeWidth, Mazes.MazeHeight, ChooseOrientation(Mazes.MazeWidth, Mazes.MazeHeight));
            await Mazes.PaintUpdate(true);
        }
    }
}