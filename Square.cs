using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace _2048
{
    public class Square : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int number = 0;
        public string color = "#776e65";
        public int Number { get => number; set { SetNumber(value); PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Number))); } }
        public string Color { get => color; set { color = value; PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Number))); } }

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
                color = "#242424";
            }
        }
    }
}
