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

        private const double currentPadding = 7.0;

        internal class TextBlockForNumber : TextBlock
        {
            public TextBlockForNumber()
            {
                this.HorizontalAlignment = HorizontalAlignment.Center;
                this.Padding 
                    = new Thickness(
                        0, currentPadding,
                        0, currentPadding);
            }
        }
        internal class TextBlockForText : TextBlock
        {
            public TextBlockForText()
            {
                this.Padding
                    = new Thickness(
                        7.0, currentPadding,
                        0, currentPadding);
            }
        }

        public ButtonEditEntity ButtonEdit { get; set; }
        public ButtonDeleteEntity ButtonDelete { get; set; }

        public ButtonAddEntity ButtonAdd { get; set; }

        public void OpenAReservedPlaceForEditingButtons()
        {
            Grid.SetColumnSpan(button, 1);
        }

        public abstract void AddingAdminButtons(int idEntity);

        public abstract ButtonAddEntity AddingAnAddEntityButton();
    }
}
