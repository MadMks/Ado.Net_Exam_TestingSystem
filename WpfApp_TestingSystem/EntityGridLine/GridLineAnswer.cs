using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_TestingSystem.EntityGridLine
{
    public class GridLineAnswer : GridLineEntity
    {
        private Grid gridLineButton = null;

        public GridLineAnswer() {}

        public GridLineAnswer(int number, Answer currentAnswer)
        {
            // Колонки для основного Grid
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
        }

        public override void AddingAdminButtons(int idEntity)
        {
            throw new NotImplementedException();
        }
    }
}
