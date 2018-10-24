using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityAddButton
{
    public class ButtonAddAnswer : ButtonAddEntity
    {
        public int QuestionId { get; set; }

        public ButtonAddAnswer()
        {
            this.Content = "Добавить ответ";
        }

        public override bool AddEntity(TestingSystemEntities db)
        {
            db.Database.Log = Console.Write;

            WindowEdit windowAdd = new WindowEdit();
            windowAdd.gridEditAnswer.Visibility = Visibility.Visible;
            windowAdd.buttonOk.Content = "Добавить";
            windowAdd.Title = "Добавление ответа";

            windowAdd.textBoxAnswerText.MaxLength = 300;

            // Ставим значение по умолчанию
            windowAdd.comboBoxAnswerValue.SelectedIndex = 1;   // TODO enum or stringValue


            bool? result = windowAdd.ShowDialog();

            if (result == true)
            {
                this.SwitchingOtherAnswersToWrong(db, 
                    windowAdd.comboBoxAnswerValue.SelectedIndex,
                    this.QuestionId);

                Answer addAnswer = new Answer();
                addAnswer.ResponseText = windowAdd.AnswerName;
                addAnswer.CorrectAnswer 
                    = windowAdd
                    .comboBoxAnswerValue.SelectedIndex == 0
                    ? true : false;

                addAnswer.QuestionId = this.QuestionId;

                db.Answer.Add(addAnswer);
                db.SaveChanges();


                // method установки Active
                // TODO >>> попробовать Актив
                // если ответов

                bool active;

                if (db.Answer.Where(x => x.QuestionId == addAnswer.QuestionId).Count() >= 2
                    &&
                    db.Answer
                    .Where(x => x.QuestionId == addAnswer.QuestionId 
                    && x.CorrectAnswer == true).Count() > 0
                    )
                {
                    // то вопрос делаем активным
                    active = true;
                }
                else
                {
                    active = false;
                }

                db.Question
                        .Where(x => x.Id == addAnswer.QuestionId)
                        .FirstOrDefault()
                        .Active
                        = active;
                // если вопросов

                // если тестов

                db.SaveChanges();

                return true;
            }

            return false;
        }

        private void SwitchingOtherAnswersToWrong(TestingSystemEntities db, int selectedIndex, int questionId)
        {
            if (selectedIndex == 0)
            {
                var answers
                    = db.Answer
                    .Where(x => x.QuestionId == questionId)
                    .ToList();

                foreach (var answer in answers)
                {
                    answer.CorrectAnswer = false;
                }
            }
        }
    }
}
