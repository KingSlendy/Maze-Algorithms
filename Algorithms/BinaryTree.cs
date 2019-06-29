using System;

namespace Maze_Algorithms {
    public class BinaryTree : Algorithms {
        public BinaryTree() {
            Mazes.Current = Mazes.Cells[0, 0];
            GenerateMaze();
        }

        public override async void GenerateMaze() {
            for (var r = 0; r < Mazes.MazeHeight; r++) {
                for (var c = 0; c < Mazes.MazeWidth; c++) {
                    await Mazes.PaintUpdate();

                    var cell = Mazes.Cells[r, c];
                    var cardinal = "N";

                    if (r == 0) cardinal = "W";
                    else if (c == 0) cardinal = "N";
                    else cardinal = new[] { "N", "W" }.Pick();

                    (var row, var col) = Direction(cell.Row, cell.Col, cardinal);

                    if ((row, col) != (-1, -1)) {
                        Forge(cell.Row, cell.Col, row, col, Array.IndexOf(Cardinal, cardinal));
                        Mazes.Current = cell;
                    }
                }
            }

            await Mazes.PaintUpdate(true);
        }
    }
}