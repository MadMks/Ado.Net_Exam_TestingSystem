using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WpfApp_TestingSystem
{
    public class ButtonLine : Button
    {
        private Grid gridLine = null;

        public ButtonLine()
        {

        }

        public ButtonLine(int number, string nameCategory, int quantityTests)
        {
            //this.Style = (Style)(this.Resources["styleButtonForList"]);   // What #1 ???

            this.gridLine = new Grid();

            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });

            TextBlock textBlockNumber = new TextBlock
            {
                Text = (number + 1).ToString(),

                Background = Brushes.AliceBlue,
                Width = 30,
                //Padding = new Thickness(0.0, 10.0, 0.0, 10.0),
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5.0)
            };
            TextBlock textBlockNameCategory = new TextBlock
            {
                Text = nameCategory,

                Background = Brushes.AntiqueWhite,
                VerticalAlignment = VerticalAlignment.Center
            };
            TextBlock textBlockQuantityTests = new TextBlock
            {
                Text = quantityTests.ToString(),

                Background = Brushes.Azure,
                VerticalAlignment = VerticalAlignment.Center
            };


            gridLine.Children.Add(textBlockNumber);
            gridLine.Children.Add(textBlockNameCategory);
            gridLine.Children.Add(textBlockQuantityTests);

            Grid.SetColumn(textBlockNumber, 0);
            Grid.SetColumn(textBlockNameCategory, 1);
            Grid.SetColumn(textBlockQuantityTests, 2);


            this.Content = this.gridLine;
        }

        public ButtonLine(int number, string nameTest, string nameCategory, int quantityQuestion)
        {
            //this.Style = (Style)(this.Resources["styleButtonForList"]);   // What #1 ???

            this.gridLine = new Grid();

            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2.0, GridUnitType.Star) });
            gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });

            TextBlock textBlockNumber = new TextBlock
            {
                Text = (number + 1).ToString(),

                Background = Brushes.AliceBlue,
                Width = 30,
                //Padding = new Thickness(0.0, 10.0, 0.0, 10.0),
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
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = Brushes.Azure,
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
