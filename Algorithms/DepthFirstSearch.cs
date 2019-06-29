using System.Collections.Generic;

namespace Maze_Algorithms {
    public class DepthFirstSearch : Algorithms {
        Mazes.Cell current = Mazes.Current = Mazes.Cells[Mazes.StartRow, Mazes.StartCol];
        Stack<Mazes.Cell> cells = new Stack<Mazes.Cell>();

        public DepthFirstSearch() {
            cells.Push(current);
            GenerateMaze();
        }

        public override async void GenerateMaze() {
            while (cells.Count > 0) {
                await Mazes.PaintUpdate();

                (var row, var col) = (current.Row, current.Col);
                var adjacent = Adjacent(row, col);

                if (adjacent != -1) {
                    (var newRow, var newCol) = Direction(row, col, Cardinal[adjacent]);
                    current = Forge(row, col, newRow, newCol, adjacent);
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