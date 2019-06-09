using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze_Algorithms {
    public partial class Mazes : Form {
        public sealed class Cell {
            public int Row { get; }
            public int Col { get; }
            public bool Visited {
                get { return visited; }

                set {
                    if (!visited) {
                        Design = new Color().ToHSV(Hue * 255, 1, 1);
                        Hue += 2d / (MazeWidth * MazeHeight);
                        visited = value;
                    }
                }
            }

            public Color Design { get; set; }
            public bool this[string direction] {
                get { return directions[direction]; }
                set { directions[direction] = value; }
            }

            readonly Dictionary<string, bool> directions = new Dictionary<string, bool> { { "N", false }, { "E", false }, { "W", false }, { "S", false } };

            int GridRow { get; }
            int GridCol { get; }
            bool visited;

            public Cell(int row, int col) {
                (Row, Col) = (row, col);
                (GridRow, GridCol) = (Row * CellLength, Col * CellLength);
            }

            public void Draw(Graphics graphics) {
                var cellSquare = new Rectangle(GridCol, GridRow, CellLength, CellLength);
                graphics.FillRectangle(new SolidBrush((Visited) ? Color.White : Color.Black), cellSquare);
                var walls = new Pen(Color.Black, 2);

                if (!this["N"]) graphics.DrawLine(walls, GridCol, GridRow, GridCol + CellLength, GridRow);
                if (!this["E"]) graphics.DrawLine(walls, GridCol + CellLength, GridRow, GridCol + CellLength, GridRow + CellLength);
                if (!this["W"]) graphics.DrawLine(walls, GridCol, GridRow, GridCol, GridRow + CellLength);
                if (!this["S"]) graphics.DrawLine(walls, GridCol, GridRow + CellLength, GridCol + CellLength, GridRow + CellLength);
                if (Visualization && Current == this) graphics.FillEllipse(new SolidBrush(Color.Red), cellSquare);
            }
        }

        public static readonly Random RNG = new Random();
        public static readonly int MazeWidth = 20, MazeHeight = 15;
        public static readonly int StartRow = RNG.Next(MazeHeight), StartCol = RNG.Next(MazeWidth);
        public static readonly int CellLength = 32;
        public static readonly Cell[,] Cells = new Cell[MazeHeight, MazeWidth];
        public static readonly string[] Cardinal = { "N", "E", "W", "S" };
        public static readonly string[] Opposite = { "S", "W", "E", "N" };
        public static readonly Dictionary<string, (int row, int col)> Locations = new Dictionary<string, (int row, int col)> { { "N", (-1, 0) }, { "E", (0, 1) }, { "W", (0, -1) }, { "S", (1, 0) } };
        public static readonly int VisualSpeed = 20;

        public static bool Visualization = true;
        public static Cell Current;
        public static double Hue;

        public Mazes() {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            InitializeComponent();

            for (var r = 0; r < MazeHeight; r++) {
                for (var c = 0; c < MazeWidth; c++) {
                    Cells[r, c] = new Cell(r, c);
                }
            }

            PickAlgorithm();
        }

        async void PickAlgorithm() {
            await Task.Delay(1000);

            switch (RNG.Next(11)) {
                case 0: new AldousBroder(); break;
                case 1: new BinaryTree(); break;
                case 2: new DepthFirstSearch(); break;
                case 3: new Eller(); break;
                case 4: new GrowingTree(); break;
                case 5: new HuntAndKill(); break;
                case 6: new Kruskal(); break;
                case 7: new Prim(); break;
                case 8: new RecursiveDivision(); break;
                case 9: new Sidewinder(); break;
                case 10: new Wilson(); break;
            }
        }

        public static (int, int) Direction(int row, int col, string cardinal) {
            (var newRow, var newCol) = (row + Locations[cardinal].row, col + Locations[cardinal].col);

            if (!newRow.IsInRange(0, MazeHeight - 1) || !newCol.IsInRange(0, MazeWidth - 1)) return (-1, -1);

            return (newRow, newCol);
        }

        public static int Adjacent(int row, int col) {
            var start = RNG.Next(Cardinal.Length);
            var adjacent = -1;

            for (var i = 0; i < Cardinal.Length; i++) {
                var check = (start + i) % Cardinal.Length;
                (var newRow, var newCol) = Direction(row, col, Cardinal[check]);

                if ((newRow, newCol) != (-1, -1) && !Cells[newRow, newCol].Visited) {
                    adjacent = check;

                    break;
                }
            }

            return adjacent;
        }

        public static int Adjacent(int row, int col, int[,] sets) {
            var start = RNG.Next(Cardinal.Length);
            var path = -1;

            for (var i = 0; i < Cardinal.Length; i++) {
                var check = (start + i) % Cardinal.Length;
                (var newRow, var newCol) = Direction(row, col, Cardinal[check]);

                if ((newRow, newCol) != (-1, -1) && sets[newRow, newCol] != sets[row, col]) {
                    sets[newRow, newCol] = sets[row, col];
                    Forge(row, col, newRow, newCol, check);
                    path = check;

                    break;
                }
            }

            return path;
        }

        public static Cell Forge(int row, int col, int newRow, int newCol, int point) {
            Cells[row, col].Visited = Cells[row, col][Cardinal[point]] = true;
            Cells[newRow, newCol].Visited = Cells[newRow, newCol][Opposite[point]] = true;

            return Cells[newRow, newCol];
        }

        public static void Union(int row, int col, int num, int[,] sets) {
            foreach (var cardinal in Cardinal) {
                (var newRow, var newCol) = Direction(row, col, cardinal);

                if ((newRow, newCol) != (-1, -1) && Cells[row, col][cardinal] && sets[newRow, newCol] != sets[row, col]) {
                    sets[newRow, newCol] = sets[row, col];
                    Union(newRow, newCol, num, sets);
                }
            }
        }

        void Mazes_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue == (char)Keys.F2) {
                Application.Restart();
            }
        }

        void Mazes_Paint(object sender, PaintEventArgs e) {
            for (var r = 0; r < MazeHeight; r++) {
                for (var c = 0; c < MazeWidth; c++) {
                    Cells[r, c].Draw(e.Graphics);
                }
            }
        }

        public static async Task PaintUpdate(bool end = false) {
            if (Visualization) {
                if (end) Current = null;

                Program.App.Invalidate();

                await Task.Delay(VisualSpeed);
            }
        }
    }
}