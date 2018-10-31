using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteCategory : ButtonDeleteEntity
    {
        /// <summary>
        /// Недопустимый конструктор (параметры обязательны).
        /// </summary>
        public ButtonDeleteCategory() { }

        public ButtonDeleteCategory(int idCategory) : base(idCategory) { }

        public override bool DeletingEntity(TestingSystemEntities db)
        {
            int idCategory = Convert.ToInt32((this).Tag);

            var deleteCategory = db.Category.Where(x => x.Id == idCategory).FirstOrDefault();

            int testsCount = deleteCategory.Test.Count();

            MessageBoxResult result = MessageBox.Show(
                $"Категория {deleteCategory.Name} содержит {testsCount} тестов."
                + " \nУдалить?",
                $"Удаление категории {deleteCategory.Name}",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (testsCount > 0)
                {
                    var deleteTests
                        = (
                        from test in db.Test
                        where test.CategoryId == deleteCategory.Id
                        select test
                        )
                        .ToList();

                    Int16[] delTestsId = (from test in deleteTests select test.Id).ToArray();

                    var deleteQuestions
                        = (
                        from question in db.Question
                        where (delTestsId).Contains(question.TestId)
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

                    db.Test.RemoveRange(deleteTests);
                }

                db.Category.Remove(deleteCategory);
                db.SaveChanges();
                
                return true;
            }

            return false;
        }
    }
}
