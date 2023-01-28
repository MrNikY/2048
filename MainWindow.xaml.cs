using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Square> FieldList;

        Game game;

        public MainWindow()
        {
            FieldList = new();
            for (int i = 0; i < 16; i++)
                FieldList.Add(new Square());

            InitializeComponent();

            ObjectDataProvider provider = new()
            { ObjectInstance = FieldList, };
            Binding binding = new()
            {
                Source = provider,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            GameField.SetBinding(ListBox.ItemsSourceProperty, binding);

            game = new();

            //GameField.ItemsSource = FieldList;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (game.Run(e.Key) == 1)
                GameOver();
            CheckField(game.Board);
        }

        public void CheckField(ulong[,] board)
        {
            int i = 0;
            foreach (var element in board)
            {
                FieldList[i].Number = (int)element;
                i++;
            }
            GameField.Items.Refresh();
        }

        public void GameOver()
        {
            End.Visibility = Visibility.Visible;
        }
    }
}
