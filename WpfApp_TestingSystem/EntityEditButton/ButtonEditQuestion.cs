using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditQuestion : ButtonEditEntity
    {
        public ButtonEditQuestion() {}

        public ButtonEditQuestion(int idQuestion) : base(idQuestion) { }

        public override bool EditingEntity(TestingSystemEntities db)
        {
            MessageBox.Show("Реализовать редактирование вопроса");

            int idQuestion = Convert.ToInt32(this.Tag);

            WindowEdit windowEdit = new WindowEdit();
            windowEdit.gridEditQuestion.Visibility = Visibility.Visible;

            var editQuestion = db.Question
                .Where(x => x.Id == idQuestion)
                .FirstOrDefault();

            // Заполняем поле вопроса
            windowEdit.QuestionName = editQuestion.QuestionText;

            bool? result = windowEdit.ShowDialog();

            if (result == true)
            {
                editQuestion.QuestionText = windowEdit.QuestionName;

                db.SaveChanges();

                return true;
            }


            return false;
        }
    }
}
