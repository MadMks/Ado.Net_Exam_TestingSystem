using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfApp_TestingSystem.EntityDeleteButton;
using WpfApp_TestingSystem.EntityEditButton;

namespace WpfApp_TestingSystem
{
    public class GridLineTest : /*Grid*/GridLineEntity
    {
        //private Button button = null;
        private Grid gridLineButton = null;

        private TextBlock textBlockCategory = null;

        public int TestID { get; set; }

        public string CategoryName
        {
            get { return textBlockCategory.Text; }
            set { textBlockCategory.Text = value; }    // TODO ?!?!?!? времменно написал - нужно правильно присваивать!
        }

        public GridLineTest() {}

        public GridLineTest(int number, Test currentTests)
        {
            // Колонки для основного Grid
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
            button = new Button { Style = (Style)(this.Resources["styleButtonForList"]) };
            // grid внутри главной кнопки
            gridLineButton = new Grid { Background = Brushes.MediumBlue };
            // Колонки главной кнопки.
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(9.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // Данные главной кнопки
            TextBlock textBlockNumber = new TextBlock
            {
                Text = (number + 1).ToString(),
                Background = Brushes.AliceBlue
            };
            TextBlock textBlockTestName = new TextBlock
            {
                Text = currentTests.Name,
                Background = Brushes.Aqua
            };
            /*TextBlock*/ textBlockCategory = new TextBlock
            {
                Text = currentTests.Category.Name,
                Background = Brushes.Bisque
            };
            TextBlock textBlockQuantityQuestions = new TextBlock
            {
                Text = currentTests.Question.Count().ToString(),
                Background = Brushes.BurlyWood
            };

            // Добавление textBlock с данными в кнопку.
            gridLineButton.Children.Add(textBlockNumber);
            gridLineButton.Children.Add(textBlockTestName);
            gridLineButton.Children.Add(textBlockCategory);
            gridLineButton.Children.Add(textBlockQuantityQuestions);

            // Расстановка textBlock по колонкам
            Grid.SetColumn(textBlockNumber, 0);
            Grid.SetColumn(textBlockTestName, 1);
            Grid.SetColumn(textBlockCategory, 2);
            Grid.SetColumn(textBlockQuantityQuestions, 3);

            // Скрываем зарезервированное место для кнопок админа.
            Grid.SetColumnSpan(button, 3);

            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;

            // HACK добавляю в Tag id теста
            button.Tag = currentTests.Id;   // TODO ??? убрать
            // заменил нижней строкой.
            this.TestID = currentTests.Id;

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

        public override void AddingAdminButtons(int idEntity)
        {
            // TODO добавить кнопку ButtonEditTest
            this.ButtonEdit = new ButtonEditTest(idEntity);
            this.Children.Add(this.ButtonEdit);
            // TODO добавить кнопку ButtonDeleteTest
            this.ButtonDelete = new ButtonDeleteTest(idEntity);
            this.Children.Add(this.ButtonDelete);
        }
    }
}
