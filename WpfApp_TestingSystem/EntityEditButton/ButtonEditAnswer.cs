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
            MessageBox.Show("Реализовать редактирование ответа");

            //int idAnswer = Convert.ToInt32(this.Tag);

            //WindowEdit windowEdit = new WindowEdit();
            //windowEdit
        }
    }
}
