using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteAnswer : ButtonDeleteEntity
    {
        public ButtonDeleteAnswer() {}

        public ButtonDeleteAnswer(int idAnswer) : base(idAnswer) { }

        public override bool DeletingEntity(TestingSystemEntities db)
        {
            int idAnswer = Convert.ToInt32(this.Tag);

            var deleteAnswer = db.Answer
                .Where(x => x.Id == idAnswer)
                .FirstOrDefault();

            MessageBoxResult result = MessageBox.Show(
                $"Вы действительно хотите удалить этот ответ?",
                $"Удаление ответа",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                db.Answer.Remove(deleteAnswer);
                db.SaveChanges();

                return true;
            }


            return false;
        }
    }
}
