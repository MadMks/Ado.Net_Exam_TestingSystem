using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_TestingSystem;

namespace WpfApp_TestingSystem.EntityEditButton
{
    public class ButtonEditTest : ButtonEditEntity
    {
        public ButtonEditTest() {}

        public ButtonEditTest(int idTest) : base(idTest) { }


        public override bool EditingEntity(TestingSystemEntities db)
        {
            int idTest = Convert.ToInt32(this.Tag);

            var editTest = db.Test
                .Where(x => x.Id == idTest)
                .FirstOrDefault();

            WindowEdit windowEdit = new WindowEdit(editTest.Name);
            windowEdit.gridEditTest.Visibility = Visibility.Visible;

            var categoriesTest
                = (
                from category in db.Category
                select category/*.Name*/
                )
                .ToList();

            // Заполняем название теста.
            windowEdit.TestName = editTest.Name;
            // Заполняем названия доступных категорий.
            windowEdit.comboBoxTestCategories.ItemsSource
                = categoriesTest;
            // Выводим только имена категорий.
            windowEdit.comboBoxTestCategories.DisplayMemberPath
                = "Name";
            // Выбираем категорию соответствующую 
            // тесту который редактируем. 
            windowEdit.comboBoxTestCategories.SelectedItem
                = editTest.Category;

            bool? result = windowEdit.ShowDialog();

            if (result == true)
            {
                editTest.Name = windowEdit.TestName;
                editTest.CategoryId
                    = (windowEdit.comboBoxTestCategories
                    .SelectedItem as Category).Id;

                db.SaveChanges();

                return true;
            }
            
            return false;
        }
    }
}
