namespace Maze_Algorithms {
    public class HuntAndKill {
        Mazes.Cell current = Mazes.Current = Mazes.Cells[Mazes.StartRow, Mazes.StartCol];

        public HuntAndKill() {
            GenerateMaze();
        }

        async void GenerateMaze() {
            while (true) {
                await Mazes.PaintUpdate();

                (var row, var col) = (current.Row, current.Col);
                var adjacent = Mazes.Adjacent(row, col);

                if (adjacent == -1) {
                    for (var r = 0; r < Mazes.MazeHeight; r++) {
                        for (var c = 0; c < Mazes.MazeWidth; c++) {
                            (row, col) = (r, c);

                            if (Mazes.Cells[row, col].Visited) {
                                adjacent = Mazes.Adjacent(row, col);

                                if (adjacent != -1) goto Path;
                            }
                        }
                    }

                    break;
                }

            Path:
                (var newRow, var newCol) = Mazes.Direction(row, col, Mazes.Cardinal[adjacent]);
                Mazes.Current = current = Mazes.Forge(row, col, newRow, newCol, adjacent);
            }

            await Mazes.PaintUpdate(true);
        }
    }
}