using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditTest : ButtonEditEntity
    {
        public ButtonEditTest() {}

        public ButtonEditTest(int idTest) : base(idTest) { }

        public override bool EditingEntity(TestingSystemEntities db)
        {
            MessageBox.Show("Реализовать редактирование теста");

            throw new NotImplementedException();
        }
    }
}
