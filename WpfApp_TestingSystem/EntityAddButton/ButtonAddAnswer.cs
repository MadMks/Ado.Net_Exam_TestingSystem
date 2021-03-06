﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_TestingSystem.Entity;

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
            WindowEdit windowAdd = new WindowEdit(this.QuestionId);
            windowAdd.gridEditAnswer.Visibility = Visibility.Visible;
            windowAdd.textBoxAnswerText.Focus();
            windowAdd.buttonOk.Content = "Добавить";
            windowAdd.Title = "Добавление ответа";

            windowAdd.textBoxAnswerText.MaxLength = 300;

            // Ставим значение по умолчанию
            windowAdd.comboBoxAnswerValue.SelectedIndex = (int)Result.NotCorrect;


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
                    .comboBoxAnswerValue.SelectedIndex == (int)Result.Correct
                    ? true : false;

                addAnswer.QuestionId = this.QuestionId;

                db.Answer.Add(addAnswer);
                db.SaveChanges();


                // Установка Active
                this.EntityActivitySwitching(db, addAnswer);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Переключение активности сущностей.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="addAnswer"></param>
        private void EntityActivitySwitching(TestingSystemEntities db, Answer addAnswer)
        {
            // =====
            // Вопрос.

            bool active;

            if (db.Answer.Where(x => x.QuestionId == addAnswer.QuestionId).Count() >= 2
                &&
                db.Answer
                .Where(x => x.QuestionId == addAnswer.QuestionId
                && x.CorrectAnswer == true).Count() > 0
                )
            {
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

            db.SaveChanges();


            // =====
            // Тест.
            
            if (db.Question
                .Where(q => q.TestId == addAnswer.Question.TestId && q.Active == true)
                .Count() > 0)
            {
                active = true;
            }
            else
            {
                active = false;
            }

            db.Test
                .Where(t => t.Id == addAnswer.Question.TestId)
                .FirstOrDefault()
                .Active = active;

            db.SaveChanges();


            // =====
            // Категория.

            int deleteAnswerCategoryId
                = db.Test
                .Where(t => t.Id == addAnswer.Question.TestId)
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
