namespace Maze_Algorithms {
    public class HuntAndKill : Algorithms {
        Mazes.Cell current = Mazes.Current = Mazes.Cells[Mazes.StartRow, Mazes.StartCol];

        public HuntAndKill() => GenerateMaze();

        public override async void GenerateMaze() {
            while (true) {
                await Mazes.PaintUpdate();

                (var row, var col) = (current.Row, current.Col);
                var adjacent = Adjacent(row, col);

                if (adjacent == -1) {
                    for (var r = 0; r < Mazes.MazeHeight; r++) {
                        for (var c = 0; c < Mazes.MazeWidth; c++) {
                            (row, col) = (r, c);

                            if (Mazes.Cells[row, col].Visited) {
                                adjacent = Adjacent(row, col);

                                if (adjacent != -1) goto Path;
                            }
                        }
                    }

                    break;
                }

            Path:
                (var newRow, var newCol) = Direction(row, col, Cardinal[adjacent]);
                Mazes.Current = current = Forge(row, col, newRow, newCol, adjacent);
            }

            await Mazes.PaintUpdate(true);
        }
    }
}