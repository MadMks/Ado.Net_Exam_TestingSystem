using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteAnswer : ButtonDeleteEntity
    {
        public ButtonDeleteAnswer() {}

        public ButtonDeleteAnswer(int idAnswer) : base(idAnswer) { }

        public override bool DeletingEntity(TestingSystemEntities db)
        {
            int idAnswer = Convert.ToInt32(this.Tag);

            var deleteAnswer = db.Answer
                .Where(x => x.Id == idAnswer)
                .FirstOrDefault();

            MessageBoxResult result = MessageBox.Show(
                $"Вы действительно хотите удалить этот ответ?",
                $"Удаление ответа",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                db.Answer.Remove(deleteAnswer);
                db.SaveChanges();


                // method установки Active после удаления.
                this.EntityActivitySwitching(db, deleteAnswer);

                return true;
            }


            return false;
        }

        private void EntityActivitySwitching(TestingSystemEntities db, Answer deleteAnswer)
        {
            // TODO >>> попробовать Актив
            // если ответов
            if (db.Answer.Where(x => x.QuestionId == deleteAnswer.QuestionId).Count() < 2)
            {
                // то вопрос делаем не активным
                db.Question
                    .Where(x => x.Id == deleteAnswer.QuestionId)
                    .FirstOrDefault()
                    .Active
                    = false;
            }

            db.SaveChanges();

            
            // Тест.

            int deleteAnswerTestId
                = db.Question.Where(q => q.Id == deleteAnswer.QuestionId)
                .Select(q => q.TestId).FirstOrDefault();

            bool active;
            // Если есть активные вопросы у теста
            if (db.Question
                .Where(q => q.TestId == deleteAnswerTestId && q.Active == true)
                //.Where(q => q.Active == true)
                .Count() > 0)
            {
                active = true;
                //MessageBox.Show("1");
            }
            else
            {
                active = false;
            }
            // Переключаем Тест
            db.Test
                .Where(t => t.Id == deleteAnswerTestId)
                .FirstOrDefault()
                .Active = active;
            // если тестов

            db.SaveChanges();
        }
    }
}
