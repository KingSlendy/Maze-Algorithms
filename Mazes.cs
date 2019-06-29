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
                get { return paths[direction]; }
                set { paths[direction] = value; }
            }

            readonly Dictionary<string, bool> paths = new Dictionary<string, bool> { { "N", false }, { "E", false }, { "W", false }, { "S", false } };

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
        public static readonly int VisualSpeed = 20;

        public static bool Visualization = true;
        public static Cell Current;
        public static double Hue;

        public Mazes() {
            DoubleBuffered = true;
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