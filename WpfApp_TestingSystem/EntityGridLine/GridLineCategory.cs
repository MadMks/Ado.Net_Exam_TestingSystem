using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace WpfApp_TestingSystem
{
    public class GridLineCategory : /*Grid*/GridLineEntity
    {
        //private Button button = null;
        private Grid gridLineButton = null;

        //private TextBlock tbNumber = null;



        //public int CategoryID
        //{
        //    get { return Convert.ToInt32(tbNumber.Text); }
        //}

        //public int CategoryID { get; set; } // TODO присваивать в Main или получать в конструкторе (если передавать в него сущность)


        public GridLineCategory()
        {

        }
        public GridLineCategory(int number, Category currentCategories)
        {
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // "Главная " кнопка
            /*Button*/
            button = new Button { Style = (Style)(this.Resources["styleButtonForList"]) };
            // grid внутри главной кнопки
            /*Grid*/
            gridLineButton = new Grid { Background = Brushes.MediumBlue };
            // Колонки главной кнопки.
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // Данные главной кнопки
            TextBlock tbNumber = new TextBlock
            {
                Text = (number + 1).ToString(),
                Background = Brushes.AliceBlue
            };
            TextBlock tbNameCategory = new TextBlock
            {
                Text = currentCategories.Name,
                Background = Brushes.Aqua
            };
            TextBlock tbQuantityTests = new TextBlock
            {
                Text = currentCategories.Test.Count().ToString(),
                Background = Brushes.BurlyWood
            };

            // Добавление textBlock с данными в кнопку.
            gridLineButton.Children.Add(tbNumber);
            gridLineButton.Children.Add(tbNameCategory);
            gridLineButton.Children.Add(tbQuantityTests);

            // Расстановка textBlock по колонкам
            Grid.SetColumn(tbNumber, 0);
            Grid.SetColumn(tbNameCategory, 1);
            Grid.SetColumn(tbQuantityTests, 2);

            // Скрываем зарезервированное место для кнопок админа.
            Grid.SetColumnSpan(button, 3);


            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;

            // HACK добавляю в Tag id категории
            button.Tag = currentCategories.Id;

            // Установка кнопки в первую колонку
            Grid.SetColumn(button, 0);

            // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
            // к доп. невидимому полю в stackPanel
            Binding binding = new Binding();
            binding.ElementName = "textBlockHiddenForSizeButtonLine";
            //binding.ElementName = textBlockHiddenForSizeButtonLine.Name;
            binding.Path = new PropertyPath("ActualWidth");
            gridLineButton.SetBinding(Grid.WidthProperty, binding);


            this.Children.Add(button);
        }


        public GridLineCategory(int number, string nameCategory, int quantityTests)
        {
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // "Главная " кнопка
            /*Button*/ button = new Button { Style = (Style)(this.Resources["styleButtonForList"]) };
            // grid внутри главной кнопки
            /*Grid*/ gridLineButton = new Grid { Background = Brushes.MediumBlue };
            // Колонки главной кнопки.
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // Данные главной кнопки
            TextBlock tbNumber = new TextBlock
            {
                Text = (number + 1).ToString(),
                Background = Brushes.AliceBlue
            };
            TextBlock tbNameCategory = new TextBlock
            {
                Text = nameCategory,
                Background = Brushes.Aqua
            };
            TextBlock tbQuantityTests = new TextBlock
            {
                Text = quantityTests.ToString(),
                Background = Brushes.BurlyWood
            };

            // Добавление textBox с данными в кнопку.
            gridLineButton.Children.Add(tbNumber);
            gridLineButton.Children.Add(tbNameCategory);
            gridLineButton.Children.Add(tbQuantityTests);

            // Расстановка текстБоксов по колонкам
            Grid.SetColumn(tbNumber, 0);
            Grid.SetColumn(tbNameCategory, 1);
            Grid.SetColumn(tbQuantityTests, 2);

            // Скрываем зарезервированное место для кнопок админа.
            Grid.SetColumnSpan(button, 3);


            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;

            // Установка кнопки в первую колонку
            Grid.SetColumn(button, 0);

            // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
            // к доп. невидимому полю в stackPanel
            Binding binding = new Binding();
            binding.ElementName = "textBlockHiddenForSizeButtonLine";
            //binding.ElementName = textBlockHiddenForSizeButtonLine.Name;
            binding.Path = new PropertyPath("ActualWidth");
            gridLineButton.SetBinding(Grid.WidthProperty, binding);


            this.Children.Add(button);
        }

        //public void OpenAReservedPlaceForEditingButtons()
        //{
        //    Grid.SetColumnSpan(button, 1);
        //}

        //public void SetTheMainButtonGridBinding(TextBlock textBlockHiddenForSizeButtonLine)
        //{
        //    MessageBox.Show(textBlockHiddenForSizeButtonLine.Name);
        //    // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
        //    // к доп. невидимому полю в stackPanel
        //    Binding binding = new Binding();
        //    binding.ElementName = textBlockHiddenForSizeButtonLine.Name;
        //    binding.Path = new PropertyPath("ActualWidth");
        //    gridLineButton.SetBinding(Grid.WidthProperty, binding);
        //}
    }
}
