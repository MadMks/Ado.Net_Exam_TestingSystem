using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace WpfApp_TestingSystem
{
    public class ButtonLine : Button
    {
        private Grid gridLine = null;

        private TextBlock tbNameCategory;


        public string CategoryName
        {
            get { return tbNameCategory.Text; }
            set { tbNameCategory.Text = value; }    // TODO ?!?!?!? времменно написал - нужно правильно присваивать!
        }


        public ButtonLine()
        {
            // по умолчанию во все публичные свойства
            // записать no name или 0
        }

        public ButtonLine(int number, string nameCategory, int quantityTests)
        {
            //this.Style = (Style)(this.Resources["styleButtonForList"]);   // What #1 ???

            this.gridLine = new Grid { Background = Brushes.Red };

            // Растягиваем Grid в кнопке - на всю ширину Button.
            Binding binding = new Binding();
            binding.ElementName = "stackPanelSelection";
            binding.Path = new PropertyPath("ActualWidth");
            gridLine.SetBinding(Grid.WidthProperty, binding);

            //gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50.0, GridUnitType.Pixel) });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(11.0, GridUnitType.Star) });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
            // объекты новой строки
            TextBlock tbNumber = new TextBlock
            {
                Text = (number + 1).ToString(),
                Background = Brushes.AliceBlue,
                Width = 30,
                //Height = 25,
                Padding = new Thickness(0.0, 10.0, 0.0, 10.0),
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5.0)
            };
            /*TextBlock*/ tbNameCategory = new TextBlock
            {
                Text = nameCategory,
                Background = Brushes.AntiqueWhite,
                //Padding = new Thickness(0.0, 10.0, 0.0, 10.0)
                VerticalAlignment = VerticalAlignment.Center
            };
            TextBlock tbQuantityTests = new TextBlock
            {
                Text = quantityTests.ToString(),
                Background = Brushes.Green,
                //Padding = new Thickness(0.0, 10.0, 0.0, 10.0)
                VerticalAlignment = VerticalAlignment.Center
            };

            // TODO кол-во тестов

            gridLine.Children.Add(tbNumber);
            gridLine.Children.Add(tbNameCategory);
            gridLine.Children.Add(tbQuantityTests);

            Grid.SetColumn(tbNumber, 0);
            Grid.SetColumn(tbNameCategory, 1);
            Grid.SetColumn(tbQuantityTests, 2);

            //buttonForRowCategory.Content = gridLine;
            //buttonForRowCategory.Tag = this.categories[i].Id;
            //buttonForRowCategory.Click += ButtonForRowCategory_Click;

            this.Content = gridLine;
        }

        public ButtonLine(int number, string nameTest, string nameCategory, int quantityQuestion)
        {
            //this.Style = (Style)(this.Resources["styleButtonForList"]);   // What #1 ???

            this.gridLine = new Grid { Background = Brushes.Red };

            // Растягиваем Grid в кнопке - на всю ширину Button.
            Binding binding = new Binding();
            binding.ElementName = "stackPanelSelection";
            binding.Path = new PropertyPath("ActualWidth");
            gridLine.SetBinding(Grid.WidthProperty, binding);

            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8.0, GridUnitType.Star) });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });

            TextBlock textBlockNumber = new TextBlock
            {
                Text = (number + 1).ToString(),

                Background = Brushes.AliceBlue,
                Width = 30,
                Padding = new Thickness(0.0, 10.0, 0.0, 10.0),
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5.0)
            };
            TextBlock textBlockNameTest = new TextBlock
            {
                Text = nameTest,

                Background = Brushes.AntiqueWhite,
                VerticalAlignment = VerticalAlignment.Center
            };
            TextBlock textBlockNameCategory = new TextBlock
            {
                Text = nameCategory,

                Background = Brushes.AntiqueWhite,
                VerticalAlignment = VerticalAlignment.Center
            };
            TextBlock textBlockQuantityQuestion = new TextBlock
            {
                Text = quantityQuestion.ToString(),
                //HorizontalAlignment = HorizontalAlignment.Right,
                Background = Brushes.Green,
                VerticalAlignment = VerticalAlignment.Center
            };


            gridLine.Children.Add(textBlockNumber);
            gridLine.Children.Add(textBlockNameTest);
            gridLine.Children.Add(textBlockNameCategory);
            gridLine.Children.Add(textBlockQuantityQuestion);

            Grid.SetColumn(textBlockNumber, 0);
            Grid.SetColumn(textBlockNameTest, 1);
            Grid.SetColumn(textBlockNameCategory, 2);
            Grid.SetColumn(textBlockQuantityQuestion, 3);

            this.Content = this.gridLine;
        }

        public ButtonLine(int number, int quantityAnswers, string nameQuestion)
        {

        }
    }
}
