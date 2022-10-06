using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    class Board
    {
        private int width;
        private int height;
        MainWindow window;

        public Board(MainWindow window, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.window = window;

            int[,] cells = new int[width, height];


            Grid grid = createGrid(width, height);
            createButtons(grid, width, height);
        }


        private Grid createGrid(int x, int y)
        {
            int cell_width = 50;
            int cell_heigth = 50;

            Grid Grid = new Grid();
            window.grid.Children.Add(Grid);

            Grid.Width = cell_width * x;
            Grid.Height = cell_heigth * y;
            //Grid.ShowGridLines = true;

            // Create Columns
            for (int i = 0; i < x; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(cell_width);
                Grid.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < x; i++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(cell_heigth);
                Grid.RowDefinitions.Add(gridRow);
            }

            return Grid;
        }

        private void createButtons(Grid grid, int x, int y)
        {
            for (int i = 0; i < x; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (!isBoard(i, j))
                        continue;

                    Button button = new Button()
                    {
                        Height = 50,
                        Width = 50,

                        Tag = i
                    };

                    button.Background = isEmpty(i, j) ? Brushes.Gray : Brushes.Coral;

                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    grid.Children.Add(button);

                    //button.Click += new RoutedEventHandler(button_Click);
                };
            };
        }

        private bool isBoard(int x, int y)
        {
            return (x >= 2 && x <= 4) ||
                   (y >= 2 && y <= 4);
        }

        private bool isEmpty(int x, int y)
        {
            return x == 3 && y == 3;
        }
    }
}
