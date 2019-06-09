using System.Collections.Generic;

namespace Maze_Algorithms {
    public class DepthFirstSearch {
        Mazes.Cell current = Mazes.Current = Mazes.Cells[Mazes.StartRow, Mazes.StartCol];
        Stack<Mazes.Cell> cells = new Stack<Mazes.Cell>();

        public DepthFirstSearch() {
            cells.Push(current);
            GenerateMaze();
        }

        async void GenerateMaze() {
            while (cells.Count > 0) {
                await Mazes.PaintUpdate();

                (var row, var col) = (current.Row, current.Col);
                var adjacent = Mazes.Adjacent(row, col);

                if (adjacent != -1) {
                    (var newRow, var newCol) = Mazes.Direction(row, col, Mazes.Cardinal[adjacent]);
                    current = Mazes.Forge(row, col, newRow, newCol, adjacent);
                    cells.Push(current);
                } else {
                    current = cells.Pop();
                }

                Mazes.Current = current;
            }

            await Mazes.PaintUpdate(true);
        }
    }
}