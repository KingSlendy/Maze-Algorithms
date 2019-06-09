using System.Collections.Generic;

namespace Maze_Algorithms {
    public class GrowingTree {
        List<Mazes.Cell> cells = new List<Mazes.Cell>();

        public GrowingTree() {
            cells.Add(Mazes.Cells[Mazes.StartRow, Mazes.StartCol]);
            GenerateMaze();
        }

        async void GenerateMaze() {
            while (true) {
                await Mazes.PaintUpdate();

                (var choice, var adjacent) = (-1, -1);
                (var row, var col) = (0, 0);

                while (adjacent == -1) {
                    if (choice != -1) cells.RemoveAt(choice);
                    if (cells.Count <= 0) goto GenerationEnd;

                    choice = (Mazes.RNG.Next(2) == 0) ? 0 : cells.Count - 1;
                    var cell = cells[choice];
                    (row, col) = (cell.Row, cell.Col);
                    adjacent = Mazes.Adjacent(row, col);
                }

                (var newRow, var newCol) = Mazes.Direction(row, col, Mazes.Cardinal[adjacent]);
                var current = Mazes.Forge(row, col, newRow, newCol, adjacent);
                cells.Add(current);
                Mazes.Current = current;
            }

        GenerationEnd:
            await Mazes.PaintUpdate(true);
        }
    }
}