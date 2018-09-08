using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditCategory : ButtonEditEntity
    {
        /// <summary>
        /// Недопустимый конструктор (параметры обязательны).
        /// </summary>
        public ButtonEditCategory() {}

        public ButtonEditCategory(int idCategory) : base(idCategory) {}
    }
}
