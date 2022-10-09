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
        private enum BoardCellType
        {
            FULL,
            EMPTY,

            NOT_BOARD
        }

        private class cell
        {
            public Board parent;
            public int x, y;
            public Button button = null;
            public BoardCellType type;

            public cell(Board parent, int x, int y)
            {
                this.parent = parent;
                this.x = x;
                this.y = y;
            }

            public void setType(BoardCellType type) 
            {
                this.type = type;
                if (button != null)
                {
                    if (type == BoardCellType.FULL) button.Background = Brushes.Coral;
                    else if (type == BoardCellType.EMPTY) button.Background = Brushes.Gray;
                }
            }

            public void setButton(Button button) { this.button = button; }

            public void clickAction(object sender, RoutedEventArgs e)
            {
                int result = parent.clickHandler(this);
                if (result >= 0)
                    parent.window.endScreen(result);
            }
        }

        private int width;
        private int height;

        private cell[,] board;
        private cell clicked = null;

        MainWindow window;
        public Grid Grid;

        public Board(MainWindow window, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.window = window;

            board = new cell[width, height];

            for(int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    board[i, j] = new cell(this, i, j);
                    if (!isBoard(i, j))
                        board[i, j].setType(BoardCellType.NOT_BOARD);
                    else if (isEmpty(i, j))
                        board[i, j].setType(BoardCellType.EMPTY);
                    else
                        board[i, j].setType(BoardCellType.FULL);
                }
            }


            Grid grid = createGrid(width, height);
            createButtons(grid, width, height);
        }


        private Grid createGrid(int x, int y)
        {
            int cell_width = 50;
            int cell_heigth = 50;

            Grid = new Grid();
            window.grid.Children.Add(Grid);

            Grid.Width = cell_width * x;
            Grid.Height = cell_heigth * y;

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
                for (int j = 0; j < y; ++j)
                {
                    if (board[i, j].type == BoardCellType.NOT_BOARD)
                        continue;

                    Button button = new Button()
                    {
                        Height = 50,
                        Width = 50,

                        Tag = i
                    };

                    button.Background = board[i, j].type == BoardCellType.EMPTY ? Brushes.Gray : Brushes.Coral;

                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    grid.Children.Add(button);

                    board[i, j].setButton(button);

                    button.Click += new RoutedEventHandler(board[i, j].clickAction);
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

        private int clickHandler(cell c)
        {
            int x = c.x;
            int y = c.y;

            switch (board[x, y].type)
            {
                case BoardCellType.FULL:
                    {
                        if (clicked != null) clicked.button.Background = Brushes.Coral;
                        clicked = c;
                        c.button.Background = Brushes.Red;
                        break;
                    }
                case BoardCellType.EMPTY:
                    {
                        if (clicked != null)
                        {
                            int dx = c.x - clicked.x;
                            int dy = c.y - clicked.y;

                            if ((Math.Abs(dx) == 2 && dy == 0) ||
                                (Math.Abs(dy) == 2 && dx == 0))
                            {
                                cell middle = board[(c.x + clicked.x) / 2, (c.y + clicked.y) / 2];
                                if (middle.type == BoardCellType.FULL)
                                {
                                    c.setType(BoardCellType.FULL);
                                    clicked.setType(BoardCellType.EMPTY);
                                    middle.setType(BoardCellType.EMPTY);
                                    clicked = c;
                                    c.button.Background = Brushes.Red;

                                    int result = checkResult();
                                    return result;
                                }
                            }
                        }
                        break;
                    }
            }

            return -1;
        }

        private int checkResult()
        {
            BoardCellType[] pattern = { BoardCellType.FULL,
                                        BoardCellType.FULL,
                                        BoardCellType.EMPTY };


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (findHorisontal      (i, j, pattern)) return -1;
                    if (findHorisontal_b    (i, j, pattern)) return -1;
                    if (findVertical        (i, j, pattern)) return -1;
                    if (findVertical_b      (i, j, pattern)) return -1;
                }
            }

            int count = 0;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (board[i, j].type == BoardCellType.FULL)
                        count++;

            return count;
        }


        private bool findHorisontal(int x, int y, BoardCellType[] pattern)
        {
            if (x + pattern.Length - 1 >= width)
                return false;

            for (int k = 0; k < pattern.Length; k++)
                if (board[x + k, y].type != pattern[k])
                    return false;
            return true;
        }
        private bool findHorisontal_b(int x, int y, BoardCellType[] pattern)
        {
            if (x + pattern.Length - 1 >= width)
                return false;

            for (int k = 0; k < pattern.Length; k++)
                if (board[x + k, y].type != pattern[pattern.Length - k - 1])
                    return false;
            return true;
        }
        private bool findVertical(int x, int y, BoardCellType[] pattern)
        {
            if (y + pattern.Length - 1 >= height)
                return false;

            for (int k = 0; k < pattern.Length; k++)
                if (board[x, y + k].type != pattern[k])
                    return false;
            return true;
        }
        private bool findVertical_b(int x, int y, BoardCellType[] pattern)
        {
            if (y + pattern.Length - 1 >= height)
                return false;

            for (int k = 0; k < pattern.Length; k++)
                if (board[x, y + k].type != pattern[pattern.Length - k - 1])
                    return false;
            return true;
        }
    }
}
