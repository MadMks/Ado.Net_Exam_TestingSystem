using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityAddButton
{
    public class ButtonAddCategory : ButtonAddEntity
    {
        public ButtonAddCategory()
        {
            this.Content = "Добавить категорию";
        }

        public override bool AddEntity(TestingSystemEntities db)
        {
            db.Database.Log = Console.Write;

            WindowEdit windowAdd = new WindowEdit();
            windowAdd.gridEditCategory.Visibility = Visibility.Visible;
            windowAdd.buttonOk.Content = "Добавить";
            windowAdd.Title = "Добавление категории";

            windowAdd.textBoxCategoryName.MaxLength = 30;

            bool? result = windowAdd.ShowDialog();

            if (result == true)
            {
                Category category = new Category();
                category.Name = windowAdd.CategoryName;

                db.Category.Add(category);
                db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
