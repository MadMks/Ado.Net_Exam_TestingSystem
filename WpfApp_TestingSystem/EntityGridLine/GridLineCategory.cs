using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfApp_TestingSystem.EntityAddButton;
using WpfApp_TestingSystem.EntityDeleteButton;
using WpfApp_TestingSystem.EntityEditButton;

namespace WpfApp_TestingSystem
{
    public class GridLineCategory : GridLineEntity
    {
        private Grid gridLineButton = null;
        

        public int CategoryId { get; set; }

       

        public GridLineCategory() {}
        public GridLineCategory(int number, Category currentCategories, bool isTeacher)
        {
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

            // "Главная " кнопка
            button = new Button();
            // grid внутри главной кнопки
            gridLineButton = new Grid
            {
                Background = Brushes.White
            };
            // Колонки главной кнопки.
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });

            // Данные главной кнопки
            TextBlockForNumber tbNumber = new TextBlockForNumber
            {
                Text = (number + 1).ToString()
            };
            TextBlockForText tbNameCategory = new TextBlockForText
            {
                Text = currentCategories.Name
            };

            TextBlockForNumber tbQuantityTests = null;
            if (isTeacher)
            {
                tbQuantityTests = new TextBlockForNumber
                {
                    Text = currentCategories.Test.Count().ToString()
                };
            }
            else
            {
                tbQuantityTests = new TextBlockForNumber
                {
                    Text = currentCategories
                        .Test
                        .Where(t => t.Active == true)
                        .Count().ToString()
                };
            }

            // Добавление textBlock с данными в кнопку.
            gridLineButton.Children.Add(tbNumber);
            gridLineButton.Children.Add(tbNameCategory);
            gridLineButton.Children.Add(tbQuantityTests);

            // Расстановка textBlock по колонкам
            Grid.SetColumn(tbNumber, 0);
            Grid.SetColumn(tbNameCategory, 1);
            Grid.SetColumn(tbQuantityTests, 2);

            // Скрываем зарезервированное место для кнопок админа.
            Grid.SetColumnSpan(button, 3);


            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;
            // HACK добавляю в Tag id категории
            button.Tag = currentCategories.Id;

            // TODO возможно вместо верхней (если она еще и не используется).
            this.CategoryId = currentCategories.Id;

            // Установка кнопки в первую колонку
            Grid.SetColumn(button, 0);

            // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
            // к доп. невидимому полю в stackPanel
            Binding binding = new Binding();
            binding.ElementName = "textBlockHiddenForSizeButtonLine";
            //binding.ElementName = textBlockHiddenForSizeButtonLine.Name;
            binding.Path = new PropertyPath("ActualWidth");
            gridLineButton.SetBinding(Grid.WidthProperty, binding);


            this.Children.Add(button);
        }


        public override void AddingAdminButtons(int idEntity)
        {
            // Кнопка "Редактировать".
            this.ButtonEdit = new ButtonEditCategory(idEntity);
            this.Children.Add(this.ButtonEdit);
            // Кнопка "Удалить".
            this.ButtonDelete = new ButtonDeleteCategory(idEntity);
            this.Children.Add(this.ButtonDelete);
        }

        public override ButtonAddEntity AddingAnAddEntityButton()
        {
            this.ButtonAdd = new ButtonAddCategory();

            return this.ButtonAdd;
        }

        
        /// <summary>
        /// Данное имя уже существует в базе данных.
        /// </summary>
        /// <param name="textBoxText"></param>
        /// <returns></returns>
        internal static bool IsNameAlreadyExists(string categoryName)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                Category result = db.Category.Where(x => x.Name == categoryName).FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
