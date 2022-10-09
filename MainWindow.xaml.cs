using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int cols = 7;
        private int rows = 7;

        Board board;
        Grid result_screen_grid;

        public MainWindow()
        {
            InitializeComponent();
            board = new Board(this, cols, rows);

        }

        public void endScreen(int result)
        {
            this.grid.Children.Remove(board.Grid);

            TextBlock msg = new TextBlock();
            msg.Text = "Your result: " + result.ToString();
            msg.VerticalAlignment = VerticalAlignment.Center;
            msg.TextAlignment = TextAlignment.Center;

            Button restart = new Button();
            restart.Content = "Restart";
            restart.Width = 100;
            restart.Height = 50;
            restart.Click += new RoutedEventHandler(RestartGame);


            Grid grid = new Grid();
            result_screen_grid = grid;
            this.grid.Children.Add(grid);
            for (int i = 0; i < 2; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(100);
                grid.RowDefinitions.Add(gridRow);
            }

            Grid.SetRow(msg, 0);
            grid.Children.Add(msg);
            Grid.SetRow(restart, 1);
            grid.Children.Add(restart);
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            this.grid.Children.Remove(result_screen_grid);
            board = new Board(this, cols, rows);
        }
    }
}
