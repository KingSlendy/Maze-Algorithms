using System;
using System.Collections.Generic;

namespace Maze_Algorithms {
    public class Sidewinder : Algorithms {
        public Sidewinder() {
            Mazes.Current = Mazes.Cells[0, 0];
            GenerateMaze();
        }

        public override async void GenerateMaze() {
            var choices = new List<Mazes.Cell>();

            for (var r = 0; r < Mazes.MazeHeight; r++) {
                for (var c = 0; c < Mazes.MazeWidth; c++) {
                    await Mazes.PaintUpdate();

                    var cell = Mazes.Current = Mazes.Cells[r, c];
                    cell.Visited = true;
                    var cardinal = "N";

                    if (r == 0) cardinal = "E";
                    else if (c == Mazes.MazeWidth - 1) cardinal = "N";
                    else cardinal = new[] { "N", "E" }.Pick();

                    if (cardinal == "N" && choices.Count > 0) {
                        choices.Add(cell);
                        cell = choices.Pick();
                        choices.Clear();
                    }

                    (var row, var col) = Direction(cell.Row, cell.Col, cardinal);

                    if ((row, col) != (-1, -1)) {
                        if (r != 0 && cardinal != "N") choices.Add(cell);

                        Mazes.Current = Forge(cell.Row, cell.Col, row, col, Array.IndexOf(Cardinal, cardinal));
                    }
                }
            }

            await Mazes.PaintUpdate(true);
        }
    }
}