namespace Maze_Algorithms {
    public class AldousBroder {
        Mazes.Cell current = Mazes.Current = Mazes.Cells[Mazes.StartRow, Mazes.StartCol];

        public AldousBroder() {
            GenerateMaze();
        }

        async void GenerateMaze() {
            var totalVisited = 0;
            
            while (totalVisited < Mazes.MazeWidth * Mazes.MazeHeight - 1) {
                await Mazes.PaintUpdate();

                var point = Mazes.RNG.Next(Mazes.Cardinal.Length) % Mazes.Cardinal.Length;
                (var row, var col) = Mazes.Direction(current.Row, current.Col, Mazes.Cardinal[point]);

                if ((row, col) != (-1, -1)) {
                    if (!Mazes.Cells[row, col].Visited) {
                        current = Mazes.Forge(current.Row, current.Col, row, col, point);
                        totalVisited++;
                    } else {
                        current = Mazes.Cells[row, col];
                    }
                }

                Mazes.Current = current;
            }

            await Mazes.PaintUpdate(true);
        }
    }
}