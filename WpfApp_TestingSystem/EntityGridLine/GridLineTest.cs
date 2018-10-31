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

namespace WpfApp_TestingSystem
{
    public class GridLineTest : GridLineEntity
    {
        private Grid gridLineButton = null;

        private TextBlockForText textBlockTestName = null;
        private TextBlockForNumber textBlockCategory = null;

        public int TestID { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName
        {
            get { return textBlockCategory.Text; }
            set { textBlockCategory.Text = value; }
        }

        public string TestName
        {
            get { return textBlockTestName.Text; }
        }


        public GridLineTest() {}

        public GridLineTest(int number, Test currentTest, bool isTeacher)
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
            gridLineButton = new Grid
            {
                Background = Brushes.White
            };
            // Колонки главной кнопки.
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(8.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(2.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // Данные главной кнопки
            TextBlockForNumber textBlockNumber = new TextBlockForNumber
            {
                Text = (number + 1).ToString()
            };
            textBlockTestName = new TextBlockForText
            {
                Text = currentTest.Name
            };
            textBlockCategory = new TextBlockForNumber
            {
                Text = currentTest.Category.Name
            };

            TextBlockForNumber textBlockQuantityQuestions = null;
            if (isTeacher)
            {
                textBlockQuantityQuestions = new TextBlockForNumber
                {
                    Text = currentTest.Question.Count().ToString()
                };
            }
            else
            {
                textBlockQuantityQuestions = new TextBlockForNumber
                {
                    Text = currentTest.Question
                    .Where(q => q.Active == true)
                    .Count().ToString()
                };
            }

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
            button.Tag = currentTest.Id;   // TODO ??? убрать
            // заменил нижней строкой.
            this.TestID = currentTest.Id;


            this.CategoryId = currentTest.CategoryId;

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

        internal static bool IsNameAlreadyExists(string testName, int entityParentId)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                Test result = db.Test
                    .Where(t => t.Name == testName && t.CategoryId == entityParentId)
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
            this.ButtonEdit = new ButtonEditTest(idEntity);
            this.Children.Add(this.ButtonEdit);

            this.ButtonDelete = new ButtonDeleteTest(idEntity);
            this.Children.Add(this.ButtonDelete);
        }

        public override ButtonAddEntity AddingAnAddEntityButton()
        {
            this.ButtonAdd = new ButtonAddTest();
            
            (this.ButtonAdd as ButtonAddTest).CategoryId
                = this.CategoryId;

            return this.ButtonAdd;
        }
    }
}
