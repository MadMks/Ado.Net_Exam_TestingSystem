using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp_TestingSystem.EntityGridLine
{
    public class GridLineAnswer : GridLineEntity
    {
        private Grid gridLineButton = null;

        public GridLineAnswer() {}

        public GridLineAnswer(int number, Answer currentAnswer)
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
                Width = new GridLength(10.0, GridUnitType.Star)
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
            TextBlock textBlockQuestionName = new TextBlock
            {
                Text = currentAnswer.ResponseText,
                Background = Brushes.Aqua
            };
            //TextBlock textBlockQuantityAnswers = new TextBlock
            //{
            //    Text = currentQuestion.Answer.Count().ToString(),
            //    Background = Brushes.BurlyWood
            //};
            RadioButton radioButton = new RadioButton
            {
                GroupName = "radioButtonGroupAnswer"
            };

            // Добавление элементов с данными в кнопку.
            gridLineButton.Children.Add(textBlockNumber);
            gridLineButton.Children.Add(textBlockQuestionName);
            gridLineButton.Children.Add(radioButton);

            // Расстановка textBlock по колонкам
            Grid.SetColumn(textBlockNumber, 0);
            Grid.SetColumn(textBlockQuestionName, 1);
            Grid.SetColumn(radioButton, 2);

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
            binding.Path = new PropertyPath("ActualWidth");
            gridLineButton.SetBinding(Grid.WidthProperty, binding);

            this.Children.Add(button);
        }

        public override void AddingAdminButtons(int idEntity)
        {
            throw new NotImplementedException();
        }
    }
}
