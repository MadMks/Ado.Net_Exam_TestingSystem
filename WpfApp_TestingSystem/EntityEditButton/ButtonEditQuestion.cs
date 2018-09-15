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

            throw new NotImplementedException();
        }
    }
}
