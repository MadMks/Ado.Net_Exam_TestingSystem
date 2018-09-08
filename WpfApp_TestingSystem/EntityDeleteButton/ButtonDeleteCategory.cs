﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteCategory : ButtonDeleteEntity, IDelete
    {
        /// <summary>
        /// Недопустимый конструктор (параметры обязательны).
        /// </summary>
        public ButtonDeleteCategory() { }

        public ButtonDeleteCategory(int idCategory) : base(idCategory) { }

        public bool DeletingEntity(TestingSystemEntities db)
        {
            //int idCategory = Convert.ToInt32((sender as Button).Tag);
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
                                    //where (from test in deleteTests select test.Id).Contains(question.TestId)
                                where (delTestsId).Contains(question.TestId)
                        select question
                        )
                        .ToList();

                    if (deleteQuestions.Count > 0)
                    {
                        int[] delQuestionId = (from question in deleteQuestions select question.Id).ToArray();

                        // answer
                        var deleteAnswer
                            = (
                            from answer in db.Answer
                            where (delQuestionId).Contains(answer.QuestionId)
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

                //this.ShowAllCategories();
                return true;
            }

            return false;
        }
    }
}
