using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfApp_TestingSystem.EntityAddButton;
using WpfApp_TestingSystem.EntityDeleteButton;
using WpfApp_TestingSystem.EntityEditButton;

namespace WpfApp_TestingSystem.EntityGridLine
{
    public class GridLineQuestion : GridLineEntity
    {
        private Grid gridLineButton = null;

        public int QuestionID { get; set; }

        public GridLineQuestion() {}

        public GridLineQuestion(int number, Question currentQuestion)
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
                Text = currentQuestion.QuestionText,
                Background = Brushes.Aqua
            };
            TextBlock textBlockQuantityAnswers = new TextBlock
            {
                Text = currentQuestion.Answer.Count().ToString(),
                Background = Brushes.BurlyWood
            };

            // Добавление textBlock с данными в кнопку.
            gridLineButton.Children.Add(textBlockNumber);
            gridLineButton.Children.Add(textBlockQuestionName);
            gridLineButton.Children.Add(textBlockQuantityAnswers);

            // Расстановка textBlock по колонкам
            Grid.SetColumn(textBlockNumber, 0);
            Grid.SetColumn(textBlockQuestionName, 1);
            Grid.SetColumn(textBlockQuantityAnswers, 2);

            // Скрываем зарезервированное место для кнопок админа.
            Grid.SetColumnSpan(button, 3);

            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;

            this.QuestionID = currentQuestion.Id;

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
            this.ButtonEdit = new ButtonEditQuestion(idEntity);
            this.Children.Add(this.ButtonEdit);

            this.ButtonDelete = new ButtonDeleteQuestion(idEntity);
            this.Children.Add(this.ButtonDelete);
        }

        public override ButtonAddEntity AddingAnAddEntityButton()
        {
            throw new NotImplementedException();
        }
    }
}
