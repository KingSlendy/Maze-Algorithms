using System;
using System.Windows.Forms;

namespace Maze_Algorithms {
    public static class Program {
        public static Mazes App;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            App = new Mazes();
            Application.Run(App);
        }
    }
}