namespace Maze_Algorithms {
    public class Wilson {
        readonly int[,] directions = new int[Mazes.MazeHeight, Mazes.MazeWidth];
        Mazes.Cell current = new Mazes.Cell(-1, -1);

        public Wilson() {
            GenerateMaze();
        }

        async void GenerateMaze() {
            Mazes.Cell Choose(bool visited) {
                var chosen = new Mazes.Cell(-1, -1);

                do {
                    chosen = Mazes.Cells[Mazes.RNG.Next(Mazes.MazeHeight), Mazes.RNG.Next(Mazes.MazeWidth)];
                } while (chosen.Visited == visited || chosen == current);

                return chosen;
            }

            var totalVisited = 0;
            current = Choose(true);
            current.Visited = true;
            var initial = new Mazes.Cell(-1, -1);

            while (totalVisited < Mazes.MazeWidth * Mazes.MazeHeight - 1) {
                await Mazes.PaintUpdate();

                var chosen = Choose(true);
                var first = Mazes.Cells[chosen.Row, chosen.Col];

                while (!chosen.Visited) {
                    await Mazes.PaintUpdate();

                    (var row, var col) = (-1, -1);

                    do {
                        (row, col) = Mazes.Direction(chosen.Row, chosen.Col, Mazes.Cardinal[directions[chosen.Row, chosen.Col] = Mazes.RNG.Next(Mazes.Cardinal.Length)]);
                    } while ((row, col) == (-1, -1));

                    Mazes.Current = initial = chosen = Mazes.Cells[row, col];
                }

                chosen = Mazes.Cells[first.Row, first.Col];

                while (chosen != initial) {
                    await Mazes.PaintUpdate();

                    (var row, var col) = Mazes.Direction(chosen.Row, chosen.Col, Mazes.Cardinal[directions[chosen.Row, chosen.Col]]);
                    Mazes.Current = chosen = Mazes.Forge(chosen.Row, chosen.Col, row, col, directions[chosen.Row, chosen.Col]);
                    totalVisited++;
                }
            }

            await Mazes.PaintUpdate(true);
        }
    }
}