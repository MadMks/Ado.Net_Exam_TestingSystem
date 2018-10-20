using System;
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

            WindowEdit windowEdit = new WindowEdit();
            windowEdit.gridEditAnswer.Visibility = Visibility.Visible;

            var editAnswer = db.Answer
                .Where(x => x.Id == idAnswer)
                .FirstOrDefault();

            // Заполняем поле ответа
            windowEdit.textBoxAnswerText.Text = editAnswer.ResponseText;
            // Ставим значение
            windowEdit.comboBoxAnswerValue.SelectedIndex
                = editAnswer.CorrectAnswer == true
                ? 0 : 1 ;   // TODO enum or stringValue

            bool? result = windowEdit.ShowDialog();

            if (result == true)
            {
                editAnswer.ResponseText = windowEdit.textBoxAnswerText.Text;
                editAnswer.CorrectAnswer
                    = windowEdit.comboBoxAnswerValue.SelectedIndex == 0
                    ? true : false;

                db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
