using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityAddButton
{
    public class ButtonAddQuestion : ButtonAddEntity
    {
        public int TestId { get; set; }

        public ButtonAddQuestion()
        {
            this.Content = "Добавить вопрос";
        }

        public override bool AddEntity(TestingSystemEntities db)
        {
            db.Database.Log = Console.Write;

            WindowEdit windowAdd = new WindowEdit(this.TestId);
            windowAdd.gridEditQuestion.Visibility = Visibility.Visible;
            windowAdd.buttonOk.Content = "Добавить";
            windowAdd.Title = "Добавление вопроса";

            windowAdd.textBoxQuestionName.MaxLength = 500;

            bool? result = windowAdd.ShowDialog();

            if (result == true)
            {
                Question addQuestion = new Question();
                addQuestion.QuestionText = windowAdd.QuestionName;

                addQuestion.TestId = Convert.ToInt16(this.TestId);

                db.Question.Add(addQuestion);
                db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
