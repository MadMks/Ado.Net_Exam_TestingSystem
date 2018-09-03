using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace WpfApp_TestingSystem
{
    public class GridLineCategory : Grid
    {
        private Button button = null;

        public GridLineCategory()
        {

        }

        public GridLineCategory(int number, string nameCategory, int quantityTests, bool isTeacher)
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
            Grid gridLineButton = new Grid { Background = Brushes.MediumBlue };
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

            //if (!isTeacher)
            //{
                // Если студент, растянем кнопку на три колонки
                // (скроем место для кнопок админа/учителя).
                //Grid.SetColumnSpan(textBlockHiddenForSizeButtonLine, 3);

            // Скрываем зарезервированное место для кнопок админа.
                Grid.SetColumnSpan(button, 3);
            //}

            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;

            // Установка кнопки в первую колонку
            Grid.SetColumn(button, 0);

            // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
            // к доп. невидимому полю в stackPanel
            Binding binding = new Binding();
            binding.ElementName = "textBlockHiddenForSizeButtonLine";
            binding.Path = new PropertyPath("ActualWidth");
            gridLineButton.SetBinding(Grid.WidthProperty, binding);


            this.Children.Add(button);
        }

        public void ShowButtonsForEditing()
        {
            Grid.SetColumnSpan(button, 1);
        }

    }
}
