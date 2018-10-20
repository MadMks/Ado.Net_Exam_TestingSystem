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
                    //int[] delQuestionsId = (from question in db.Question select question.Id).ToArray();

                    // question
                    var deleteQuestions
                        = (
                        from question in db.Question
                        //where (delQuestionsId).Contains(question.TestId)
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

                    db.Test.Remove(deleteTest);
                    db.SaveChanges();
                }

                return true;
            }

            return false;
        }
    }
}
