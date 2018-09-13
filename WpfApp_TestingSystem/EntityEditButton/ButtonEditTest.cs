using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditTest : ButtonEditEntity
    {
        public ButtonEditTest() {}

        public ButtonEditTest(int idTest) : base(idTest) { }

        public override bool EditingEntity(TestingSystemEntities db)
        {
            throw new NotImplementedException();
        }
    }
}
