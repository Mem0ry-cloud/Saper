using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool[] isBomb = new bool[100];
        private int bombs = 0;
        private int poles = 0;
        private int width = 10;
        private int height = 10;
        private List<Button> buttons = new List<Button>();
        private List<TextBlock> textBlocks = new List<TextBlock>();
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
            //генерация кнопок
            //Draw?
            Build();
        }

        private void Bomb_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Бум!");
           Rebuild();
        }

        private void Pole_Click(object sender, RoutedEventArgs e)
        {
            poles--;
            Poles.Text = "Полей осталось: " + poles;
            if (poles == 0)
            {
                MessageBox.Show(":D ты победил");
            }
            Open(sender as Button);
        }
        private void Open(Button b)
        {
            if (b.Visibility == Visibility.Hidden)
            {
                return;
            }
            b.Visibility = Visibility.Hidden;
            poles--;
            Poles.Text = "Полей осталось: " + poles;
            int c = gridMain.Children.IndexOf(b) / 2 - 2;
            if (textBlocks[c].Text == "")
            {
                if (c / width != 0)
                {
                    Open(buttons[c - width]);
                }
                if (c % width != height - 1)
                {
                    Open(buttons[c + 1]);
                }
                if (c / width != height - 1)
                {
                    Open(buttons[c + width]);
                }
                if (c % width != 0)
                {
                    Open(buttons[c - 1]);
                }
            }

        }
        private void Build()
        {
            int percentage = 14;
            for (int i = 0; i < width * height; i++) //создание полей
            {
                Button b = new Button();
                TextBlock t = new TextBlock();
                percentage = random.Next(0, 100);
                if (percentage < 15)
                {
                    b.Click += Bomb_Click;
                    isBomb[i] = true;
                    bombs++;
                }
                else
                {
                    b.Click += Pole_Click;
                    isBomb[i] = false;
                    poles++;
                }
                b.Background = Brushes.LightGray; //по мне так этот цвет подходит
                buttons.Add(b);
                textBlocks.Add(t);
            }
            Bombs.Text = "Кол-во мин:" + bombs;
            Poles.Text = "Полей осталось: " + poles;
            int f = 0;
            for (int i = 0; i < width * height; i++)
            {
                if (!isBomb[i])
                {
                    f = 0;

                    if (i % width != width - 1)
                    {
                        f += Check(i + 1);
                    }
                    if (i % width != 0)
                    {
                        f += Check(i - 1);
                    }
                    if (i / width != 0)
                    {
                        f += Check(i - width);
                        if (i % width != width - 1)
                        {
                            f += Check(i -  width + 1);
                        }
                        if (i % width != 0)
                        {
                            f += Check(i - width - 1);
                        }
                    }
                    if (i / width != height - 1)
                    {
                        f += Check(i + width);
                        if (i % width != height - 1)
                        {
                            f += Check(i + width + 1);
                        }
                        if (i % width != 0)
                        {
                            f += Check(i + width - 1);
                        }
                    }
                    textBlocks[i].Background = Brushes.LightGray;
                    if (f != 0)
                    {
                        textBlocks[i].Text = Convert.ToString(f);
                    }
                    Brush b = Brushes.Black;
                    switch (f)
                    {
                        case 1:
                            b = Brushes.Blue;
                            break;
                        case 2:
                            b = Brushes.Green;
                            break;
                        case 3:
                            b = Brushes.Red;
                            break;
                        case 4:
                            b = Brushes.DarkRed;
                            break;
                    }
                    textBlocks[i].Foreground = b;
                    textBlocks[i].FontSize = 25;
                }
            }
            for (int j = 0; j < buttons.Count(); j++) //мда...
            {
                Grid.SetColumn(textBlocks[j], j % width);
                Grid.SetRow(textBlocks[j], j / height + 1);

                Grid.SetColumn(buttons[j], j % width);
                Grid.SetRow(buttons[j], j / height + 1);
                gridMain.Children.Add(textBlocks[j]);
                gridMain.Children.Add(buttons[j]);
            }
        }
        private int Check(int number)
        {
            if (isBomb[number]){return 1;}
            return 0;
        }
        private void Rebuild()
        {
            bombs = 0;
            poles = 0;
            for (int i = 0; i < width * height; i++)
            {
                gridMain.Children.Remove(buttons[i]);
                gridMain.Children.Remove(textBlocks[i]);
            }
            buttons.Clear();
            textBlocks.Clear();
            Build();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Rebuild();
        }
    }
}
