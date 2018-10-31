using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityAddButton
{
    public interface IAdding
    {
        bool AddEntity(TestingSystemEntities db);
    }
}
