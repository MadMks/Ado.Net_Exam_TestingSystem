using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditCategory : ButtonEditEntity
    {
        /// <summary>
        /// Недопустимый конструктор (параметры обязательны).
        /// </summary>
        public ButtonEditCategory() {}

        public ButtonEditCategory(int idCategory) : base(idCategory) {}

        public override bool EditingEntity(TestingSystemEntities db)
        {
            int idCategory = Convert.ToInt32(this.Tag);

            var editCategory = db.Category
                .Where(x => x.Id == idCategory)
                //.Select(x => x.Name)
                .FirstOrDefault();

            WindowEdit windowEdit = new WindowEdit(editCategory);
            windowEdit.gridEditCategory.Visibility = Visibility.Visible;

            windowEdit.CategoryName = editCategory.Name;

            windowEdit.textBoxCategoryName.MaxLength = 30;

            bool? result = windowEdit.ShowDialog();

            if (result == true)
            {
                // запишем в базу
                editCategory.Name = windowEdit.CategoryName;
                db.SaveChanges();

                //this.ShowAllCategories();
                return true;
            }

            return false;
        }
    }
}
