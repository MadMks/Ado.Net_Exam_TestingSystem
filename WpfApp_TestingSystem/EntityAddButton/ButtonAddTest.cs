using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp_TestingSystem.EntityAddButton
{
    public class ButtonAddTest : ButtonAddEntity
    {
        // HACK предположительно
        // если обьединить кнопки add edit и del
        // наследниками одного класса, то это свойство
        // унаследуется.
        public int CategoryId { get; set; }

        public ButtonAddTest()
        {
            this.Content = "Добавить тест";
        }

        public override bool AddEntity(TestingSystemEntities db)
        {
            db.Database.Log = Console.Write;

            WindowEdit windowAdd = new WindowEdit(this.CategoryId);
            windowAdd.gridEditTest.Visibility = Visibility.Visible;

            windowAdd.buttonOk.Content = "Добавить";
            windowAdd.Title = "Добавление теста";


            // Настройка comboBox названия категорий.

            // узнаем существующие категории
            var categoriesTest
                = (
                from category in db.Category
                select category
                )
                .ToList();
            // Заполняем названия доступных категорий.
            windowAdd.comboBoxTestCategories.ItemsSource
                = categoriesTest;
            // Выводим только имена категорий.
            windowAdd.comboBoxTestCategories.DisplayMemberPath
                = "Name";
            // Выбираем категорию соответствующую 
            // тесту который редактируем. 
            windowAdd.comboBoxTestCategories.SelectedItem
                = categoriesTest
                .Where(x => x.Id == this.CategoryId)
                .FirstOrDefault();
            windowAdd.comboBoxTestCategories.IsEnabled
                = false;

            windowAdd.textBoxTestName.MaxLength = 60;

            bool? result = windowAdd.ShowDialog();

            if (result == true)
            {
                Test addTest = new Test();
                addTest.Name = windowAdd.TestName;
                addTest.CategoryId
                    = (windowAdd.comboBoxTestCategories
                    .SelectedItem as Category).Id;

                db.Test.Add(addTest);
                db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
