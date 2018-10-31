using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteQuestion : ButtonDeleteEntity
    {
        public ButtonDeleteQuestion() {}

        public ButtonDeleteQuestion(int idQuestion) : base(idQuestion) { }


        public override bool DeletingEntity(TestingSystemEntities db)
        {
            int idQuestion = Convert.ToInt32(this.Tag);

            var deleteQuestion = db.Question
                .Where(x => x.Id == idQuestion)
                .FirstOrDefault();

            int answersCount = deleteQuestion.Answer.Count();

            MessageBoxResult result = MessageBox.Show(
                $"Вопрос {deleteQuestion.QuestionText}" +
                $" содержит {answersCount} ответов."
                + " \nУдалить?",
                $"Удаление вопроса {deleteQuestion.QuestionText}",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (answersCount > 0)
                {
                    // answers
                    var deleteAnswers
                        = (
                        from answer in db.Answer
                        where answer.QuestionId == idQuestion
                        select answer
                        )
                        .ToList();

                    // deleting answers
                    if (deleteAnswers.Count > 0)
                    {
                        db.Answer.RemoveRange(deleteAnswers);
                    }
                }

                db.Question.Remove(deleteQuestion);
                db.SaveChanges();

                // Установка Active после удаления.
                this.EntityActivitySwitching(db, deleteQuestion);

                return true;
            }

            return false;
        }

        private void EntityActivitySwitching(TestingSystemEntities db, Question deleteQuestion)
        {
            // =====
            // Тест.

            bool active;
            // Если есть активные вопросы у теста
            if (db.Question
                .Where(q => q.TestId == deleteQuestion.TestId && q.Active == true)
                .Count() > 0)
            {
                active = true;
            }
            else
            {
                active = false;
            }
            // Переключаем Тест
            db.Test
                .Where(t => t.Id == deleteQuestion.TestId)
                .FirstOrDefault()
                .Active = active;

            db.SaveChanges();


            // =====
            // Категория.

            int deleteAnswerCategoryId
                = db.Test
                .Where(t => t.Id == deleteQuestion.TestId)
                .Select(t => t.CategoryId).FirstOrDefault();

            // Если есть активные тесты у категории
            if (db.Test
                .Where(t => t.CategoryId == deleteAnswerCategoryId && t.Active == true)
                .Count() > 0)
            {
                active = true;
            }
            else
            {
                active = false;
            }
            // Переключаем Тест
            db.Category
                .Where(c => c.Id == deleteAnswerCategoryId)
                .FirstOrDefault()
                .Active = active;

            db.SaveChanges();
        }
    }
}
