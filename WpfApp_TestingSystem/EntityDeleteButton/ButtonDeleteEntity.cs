using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_TestingSystem.EntityDeleteButton
{
    abstract public class ButtonDeleteEntity : Button, IDelete
    {
        public ButtonDeleteEntity()
        {
            this.Content = "Del";

            Grid.SetColumn(this, 2);
        }

        public ButtonDeleteEntity(int idEntity) : this()
        {
            this.Tag = idEntity;
        }

        public abstract bool DeletingEntity(TestingSystemEntities db);


    }
}
