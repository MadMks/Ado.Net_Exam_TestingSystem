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
    public class ButtonCategoryLine : Button
    {
        private Grid gridLine = null;

        //private TextBlock tbNameCategory = null;
        // ---
        //public string CategoryName
        //{
        //    get { return tbNameCategory.Text; }
        //    set { tbNameCategory.Text = value; }    // TODO ?!?!?!? времменно написал - нужно правильно присваивать!
        //}

        // +
        public int CategoryID { get; set; }

        public bool IsTeacher { get; set; }


        public ButtonCategoryLine()
        {
            // по умолчанию во все публичные свойства
            // записать no name или 0
        }


        public ButtonCategoryLine(int number, string nameCategory, int quantityTests/*, bool isAdmin*/)
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

            //if (isAdmin)
            //{
            //    gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
            //    gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
            //}

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
            /*TextBlock*/
            TextBlock tbNameCategory = new TextBlock
            {
                Text = nameCategory,
                Background = Brushes.AntiqueWhite,
                //Padding = new Thickness(0.0, 10.0, 0.0, 10.0)
                VerticalAlignment = VerticalAlignment.Center
            };
            //this.CategoryName = nameCategory;
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


            //if (isAdmin)
            //{
            //    Button buttonEdit = new Button { Content = "Edit" };
            //    Button buttonDelete = new Button { Content = "Delete" };

            //    gridLine.Children.Add(buttonEdit);
            //    gridLine.Children.Add(buttonDelete);

            //    Grid.SetColumn(buttonEdit, 3);
            //    Grid.SetColumn(buttonDelete, 4);
            //}

            //buttonForRowCategory.Content = gridLine;
            //buttonForRowCategory.Tag = this.categories[i].Id;
            //buttonForRowCategory.Click += ButtonForRowCategory_Click;

            this.Content = gridLine;
        }
    }
}
