﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditAnswer : ButtonEditEntity
    {
        public ButtonEditAnswer() {}

        public ButtonEditAnswer(int idAnswer) : base(idAnswer) { }
        public override bool EditingEntity(TestingSystemEntities db)
        {
            int idAnswer = Convert.ToInt32(this.Tag);
            
            var editAnswer = db.Answer
                .Where(x => x.Id == idAnswer)
                .FirstOrDefault();

            WindowEdit windowEdit = new WindowEdit(editAnswer);
            windowEdit.gridEditAnswer.Visibility = Visibility.Visible;
            windowEdit.textBoxAnswerText.Focus();

            windowEdit.textBoxAnswerText.MaxLength = 300;

            // Заполняем поле ответа
            windowEdit.textBoxAnswerText.Text = editAnswer.ResponseText;
            // Ставим значение
            windowEdit.comboBoxAnswerValue.SelectedIndex
                = editAnswer.CorrectAnswer == true
                ? (int)Result.Correct : (int)Result.NotCorrect;

            bool? result = windowEdit.ShowDialog();

            if (result == true)
            {
                this.SwitchingOtherAnswersToWrong(db,
                    windowEdit.comboBoxAnswerValue.SelectedIndex,
                    editAnswer.QuestionId);

                editAnswer.ResponseText = windowEdit.textBoxAnswerText.Text;
                editAnswer.CorrectAnswer
                    = windowEdit.comboBoxAnswerValue.SelectedIndex == (int)Result.Correct
                    ? true : false;

                db.SaveChanges();


                this.EntityActivitySwitching(db, editAnswer);

                return true;
            }

            return false;
        }

        private void EntityActivitySwitching(TestingSystemEntities db, Answer editAnswer)
        {

            // =====
            // Вопрос.

            bool active;

            if (
                db.Answer
                .Where(x => x.QuestionId == editAnswer.QuestionId).Count() > 1
                &&
                (db.Answer
                .Where(x => x.QuestionId == editAnswer.QuestionId
                && x.CorrectAnswer == true).Count() > 0)
                )
            {
                active = true;
            }
            else
            {
                active = false;
            }

            db.Question
                    .Where(x => x.Id == editAnswer.QuestionId)
                    .FirstOrDefault()
                    .Active
                    = active;

            db.SaveChanges();


            // =====
            // Тест

            int deleteAnswerTestId
                = db.Question.Where(q => q.Id == editAnswer.QuestionId)
                .Select(q => q.TestId).FirstOrDefault();

            // Если есть активные вопросы у теста
            if (db.Question
                .Where(q => q.TestId == deleteAnswerTestId && q.Active == true)
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
                .Where(t => t.Id == deleteAnswerTestId)
                .FirstOrDefault()
                .Active = active;

            db.SaveChanges();


            // =====
            // Категория

            int deleteAnswerCategoryId
                = db.Test
                .Where(t => t.Id == deleteAnswerTestId)
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
