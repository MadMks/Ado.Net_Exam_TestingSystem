using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityEditButton
{
    abstract public class ButtonEditEntity : Button, IEdit
    {
        public ButtonEditEntity()
        {
            this.Content = "Edit";
            this.Margin = new Thickness(5.0);

            Grid.SetColumn(this, 1);
        }

        public ButtonEditEntity(int idEntity) : this()
        {
            this.Tag = idEntity;
        }

        public abstract bool EditingEntity(TestingSystemEntities db);
    }
}
