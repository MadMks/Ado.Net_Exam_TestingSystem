﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

            WindowEdit windowEdit = new WindowEdit(editAnswer.ResponseText);
            windowEdit.gridEditAnswer.Visibility = Visibility.Visible;

            // Заполняем поле ответа
            windowEdit.textBoxAnswerText.Text = editAnswer.ResponseText;
            // Ставим значение
            windowEdit.comboBoxAnswerValue.SelectedIndex
                = editAnswer.CorrectAnswer == true
                ? 0 : 1 ;   // TODO enum or stringValue

            bool? result = windowEdit.ShowDialog();

            if (result == true)
            {
                this.SwitchingOtherAnswersToWrong(db,
                    windowEdit.comboBoxAnswerValue.SelectedIndex,
                    editAnswer.QuestionId);

                editAnswer.ResponseText = windowEdit.textBoxAnswerText.Text;
                editAnswer.CorrectAnswer
                    = windowEdit.comboBoxAnswerValue.SelectedIndex == 0
                    ? true : false;

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
