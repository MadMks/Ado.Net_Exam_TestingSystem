using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            db.Database.Log = Console.Write;

            WindowEdit windowAdd = new WindowEdit();
            windowAdd.gridEditAnswer.Visibility = Visibility.Visible;
            windowAdd.buttonOk.Content = "Добавить";
            windowAdd.Title = "Добавление ответа";


            // Ставим значение по умолчанию
            windowAdd.comboBoxAnswerValue.SelectedIndex = 1;   // TODO enum or stringValue


            bool? result = windowAdd.ShowDialog();

            if (result == true)
            {
                Answer addAnswer = new Answer();
                addAnswer.ResponseText = windowAdd.AnswerName;
                addAnswer.CorrectAnswer 
                    = windowAdd
                    .comboBoxAnswerValue.SelectedIndex == 0
                    ? true : false;

                addAnswer.QuestionId = Convert.ToInt16(this.QuestionId);

                db.Answer.Add(addAnswer);
                db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
