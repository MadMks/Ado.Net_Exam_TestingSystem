using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public class ButtonDeleteTest : ButtonDeleteEntity
    {
        /// <summary>
        /// Недопустимый конструктор (параметры обязательны).
        /// </summary>
        public ButtonDeleteTest() {}

        public ButtonDeleteTest(int idTest) : base(idTest) { }


        public override bool DeletingEntity(TestingSystemEntities db)
        {
            throw new NotImplementedException();
        }
    }
}
