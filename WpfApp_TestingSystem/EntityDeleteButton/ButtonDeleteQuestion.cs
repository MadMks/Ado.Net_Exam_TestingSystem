using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

                return true;
            }

            return false;
        }
    }
}
