using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Square> FieldList = new();

        Game game;

        public MainWindow()
        {
            for (int i = 0; i < 16; i++)
                FieldList.Add(new Square());

            InitializeComponent();

            GameField.ItemsSource = FieldList;
        }

        private void newGame()
        {
            game = new();
            CheckField(game.Board);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isDead = game.Run(e.Key);
            if (isDead == true)
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
            Score.Text = game.Score.ToString();
            GameField.Items.Refresh();
        }

        public void GameOver()
        {
            End.Visibility = Visibility.Visible;
            if (ulong.Parse(Best.Text) < game.Score)
                Best.Text = Score.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            End.Visibility = Visibility.Hidden;
            newGame();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            newGame();
            try
            { Best.Text = Encoding.ASCII.GetString(File.ReadAllBytes("best")); }
            catch { };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameOver();
            File.WriteAllBytes("best", Encoding.ASCII.GetBytes(Best.Text));
        }
    }
}
