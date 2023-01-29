using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace _2048
{
    public class Square : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int number = 0;
        public int fontSize = 48;
        public Thickness margin = new(27, 0, 0, 0);
        public string color = "#776e65";
        public int Number { get => number; set { SetNumber(value); PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Number))); } }
        public string Color { get => color; set { color = value; } }
        public int FontSize { get => fontSize; set { fontSize = value; } }
        public Thickness Margin { get => margin; set { margin = value; } }

        private readonly Dictionary<int, string> NumberColorMap = new()
            {
                {0, "#776e65"},
                {2, "#eee4da"},
                {4, "#ece0ca"},
                {8, "#f2b179"},
                {16, "#ec8d53"},
                {32, "#f77c5f"},
                {64, "#f65e3b"},
                {128, "#edcf73"},
                {256, "#edcc62"},
                {512, "#edc850"},
                {1024, "#edc53f"},
                {2048, "#edc22d"},
            };


        public void SetNumber(int newNum)
        {
            number = newNum;
            try
            {
                color = NumberColorMap[number];
            }
            catch
            {
                color = "#7d89a9";
            }
            if (number < 10)
            {
                fontSize = 48;
                margin = new(27, 0, 0, 0);
            }
            else if (number < 100)
            {
                fontSize = 48;
                margin = new(14, 0, 0, 0);
            }
            else if (number < 1000)
            {
                fontSize = 40;
                margin = new(7, 0, 0, 0);
            }
            else if (number < 10000)
            {
                fontSize = 36;
                margin = new(0, 0, 0, 0);
            }
            else
            {
                fontSize = 28;
                margin = new(0, 0, 0, 0);
            }
        }
    }
}
