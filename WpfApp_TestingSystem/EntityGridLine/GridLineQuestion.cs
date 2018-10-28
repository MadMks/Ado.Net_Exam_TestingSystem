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

        public int TestId { get; set; }
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
            TextBlockForNumber textBlockNumber = new TextBlockForNumber
            {
                Text = (number + 1).ToString(),
                Background = Brushes.AliceBlue
            };
            TextBlockForText textBlockQuestionName = new TextBlockForText
            {
                Text = currentQuestion.QuestionText,
                Background = Brushes.Aqua,
                TextWrapping = TextWrapping.Wrap
            };
            TextBlockForNumber textBlockQuantityAnswers = new TextBlockForNumber
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
            this.TestId = currentQuestion.TestId;

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

        internal static bool IsNameAlreadyExists(string questionName, int entityParentId)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                Question result = db.Question
                    .Where(q => q.QuestionText == questionName && q.TestId == entityParentId)
                    .FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return true;
            }
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
            this.ButtonAdd = new ButtonAddQuestion();

            (this.ButtonAdd as ButtonAddQuestion).TestId
                = this.TestId;

            return this.ButtonAdd;
        }
    }
}
