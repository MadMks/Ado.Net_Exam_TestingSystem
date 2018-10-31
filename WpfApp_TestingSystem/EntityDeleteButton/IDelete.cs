using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    public interface IDelete
    {
        bool DeletingEntity(TestingSystemEntities db);
    }
}
