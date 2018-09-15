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
            MessageBox.Show("Реализовать удаление вопроса");

            throw new NotImplementedException();
        }
    }
}
