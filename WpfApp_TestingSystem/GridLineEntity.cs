using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp_TestingSystem
{
    abstract public class GridLineEntity : Grid
    {
        internal Button button = null;

        public void OpenAReservedPlaceForEditingButtons()
        {
            Grid.SetColumnSpan(button, 1);
        }
    }
}
