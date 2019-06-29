namespace Maze_Algorithms {
    public class AldousBroder : Algorithms {
        Mazes.Cell current = Mazes.Current = Mazes.Cells[Mazes.StartRow, Mazes.StartCol];

        public AldousBroder() {
            GenerateMaze();
        }

        public override async void GenerateMaze() {
            var totalVisited = 0;
            
            while (totalVisited < Mazes.MazeWidth * Mazes.MazeHeight - 1) {
                await Mazes.PaintUpdate();

                var point = Mazes.RNG.Next(Cardinal.Length) % Cardinal.Length;
                (var row, var col) = Direction(current.Row, current.Col, Cardinal[point]);

                if ((row, col) != (-1, -1)) {
                    if (!Mazes.Cells[row, col].Visited) {
                        current = Forge(current.Row, current.Col, row, col, point);
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