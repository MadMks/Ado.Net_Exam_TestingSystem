using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp_TestingSystem.EntityEditButton;
using WpfApp_TestingSystem.EntityDeleteButton;
using System.Windows;
using WpfApp_TestingSystem.EntityAddButton;

namespace WpfApp_TestingSystem
{
    abstract public class GridLineEntity : Grid
    {
        internal Button button = null;

        //internal ButtonEditEntity buttonEdit = null;
        //internal ButtonDeleteEntity buttonDelete = null;

        public ButtonEditEntity ButtonEdit { get; set; }
        public ButtonDeleteEntity ButtonDelete { get; set; }

        public ButtonAddEntity ButtonAdd { get; set; }

        public void OpenAReservedPlaceForEditingButtons()
        {
            Grid.SetColumnSpan(button, 1);
        }

        public abstract void AddingAdminButtons(int idEntity);

        public abstract ButtonAddEntity AddingAnAddEntityButton();
        //{
            //Button buttonAddEntity = new Button
            //{
            //    Content = "no name button",
            //    Margin = new Thickness(10.0),
            //    Padding = new Thickness(10.0, 5.0, 10.0, 5.0),
            //    HorizontalAlignment = HorizontalAlignment.Center
            //};

            //return buttonAddEntity;
        //}
    }
}
