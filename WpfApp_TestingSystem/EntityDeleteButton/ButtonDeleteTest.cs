using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteTest : ButtonDeleteEntity
    {
        /// <summary>
        /// Недопустимый конструктор (параметры обязательны).
        /// </summary>
        public ButtonDeleteTest() {}

        public ButtonDeleteTest(int idTest) : base(idTest) { }


        public override bool DeletingEntity(TestingSystemEntities db)
        {
            int idTest = Convert.ToInt32((this).Tag);

            var deleteTest = db.Test.Where(x => x.Id == idTest).FirstOrDefault();

            int questionsCount = deleteTest.Question.Count();

            MessageBoxResult result = MessageBox.Show(
                $"Тест {deleteTest.Name} содержит {questionsCount} вопросов."
                + " \nУдалить?",
                $"Удаление теста {deleteTest.Name}",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (questionsCount > 0)
                {
                    // question
                    var deleteQuestions
                        = (
                        from question in db.Question
                        where question.TestId == deleteTest.Id
                        select question
                        )
                        .ToList();

                    if (deleteQuestions.Count > 0)
                    {
                        int[] delQuestionsId = (from question in deleteQuestions select question.Id).ToArray();

                        // answer
                        var deleteAnswer
                            = (
                            from answer in db.Answer
                            where (delQuestionsId).Contains(answer.QuestionId)
                            select answer
                            )
                            .ToList();

                        if (deleteAnswer.Count > 0)
                        {
                            db.Answer.RemoveRange(deleteAnswer);
                        }

                        db.Question.RemoveRange(deleteQuestions);
                    }

                }

                db.Test.Remove(deleteTest);
                db.SaveChanges();

                // Установка Active после удаления.
                this.EntityActivitySwitching(db, deleteTest);

                return true;
            }

            return false;
        }

        private void EntityActivitySwitching(TestingSystemEntities db, Test deleteTest)
        {
            // =====
            // Категория.

            bool active;

            int deleteAnswerCategoryId
                = db.Test
                .Where(t => t.Id == deleteTest.CategoryId)
                .Select(t => t.CategoryId).FirstOrDefault();

            // Если есть активные тесты у категории
            if (db.Test
                .Where(t => t.CategoryId == deleteTest.CategoryId && t.Active == true)
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
                .Where(c => c.Id == deleteTest.CategoryId)
                .FirstOrDefault()
                .Active = active;

            db.SaveChanges();
        }
    }
}
