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
            MessageBox.Show("Реализовать удаление ответа");

            throw new NotImplementedException();
        }
    }
}
