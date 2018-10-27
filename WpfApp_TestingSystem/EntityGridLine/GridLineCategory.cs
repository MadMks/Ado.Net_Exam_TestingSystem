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
    public class GridLineCategory : /*Grid*/GridLineEntity
    {
        //private Button button = null;
        private Grid gridLineButton = null;

        //private TextBlock tbNumber = null;

        public int CategoryId { get; set; }

        //public int CategoryID
        //{
        //    get { return Convert.ToInt32(tbNumber.Text); }
        //}

        //public int CategoryID { get; set; } // TODO присваивать в Main или получать в конструкторе (если передавать в него сущность)
        //public bool IsTeacher { get; set; }

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
            /*Button*/
            button = new Button() /*{ Style = (Style)(this.Resources["styleButtonForList"]) }*/;
            // grid внутри главной кнопки
            /*Grid*/
            gridLineButton = new Grid { Background = Brushes.MediumBlue };
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
                Text = (number + 1).ToString(),
                Background = Brushes.AliceBlue
                //, Height = 35
                //, HorizontalAlignment = HorizontalAlignment.Center
                //,
                //Padding = new Thickness(0, 15.0, 0, 15.0)
            };
            TextBlockForText tbNameCategory = new TextBlockForText
            {
                Text = currentCategories.Name,
                Background = Brushes.Aqua
                //,
                //Padding = new Thickness(0, 15.0, 0, 15.0)
            };

            TextBlockForNumber tbQuantityTests = null;
            if (isTeacher)
            {
                tbQuantityTests = new TextBlockForNumber
                {
                    Text = currentCategories.Test.Count().ToString(),
                    Background = Brushes.BurlyWood
                };
            }
            else
            {
                tbQuantityTests = new TextBlockForNumber
                {
                    Text = currentCategories
                    .Test
                    .Where(t => t.Active == true)
                    .Count().ToString(),
                    Background = Brushes.BurlyWood
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
            //button.Height = 25;
            // HACK добавляю в Tag id категории
            button.Tag = currentCategories.Id;

            // TODO возможно вместо верхней (если она еще и не используется).
            // test 2 
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
            //ButtonEditCategory buttonEditCategory = new ButtonEditCategory(idEntity);
            this.ButtonEdit = new ButtonEditCategory(idEntity);
            //this.Children.Add(buttonEditCategory);
            this.Children.Add(this.ButtonEdit);
            // Кнопка "Удалить".
            //ButtonDeleteCategory buttonDeleteCategory = new ButtonDeleteCategory(idEntity);
            this.ButtonDelete = new ButtonDeleteCategory(idEntity);
            //this.Children.Add(buttonDeleteCategory);
            this.Children.Add(this.ButtonDelete);
        }

        public override ButtonAddEntity AddingAnAddEntityButton()
        {
            //Button buttonAddEntity
            //    = base.AddingAnAddEntityButton();
            this.ButtonAdd = new ButtonAddCategory();
            //this.ButtonAdd.Content = "Добавить категорию";
            //this.ButtonAdd.Click += ButtonAddEntity_Click;

            return this.ButtonAdd;
        }

        //private void ButtonAddEntity_Click(object sender, RoutedEventArgs e)
        //{
        //    using (TestingSystemEntities db = new TestingSystemEntities())
        //    {
        //        db.Database.Log = Console.Write;

        //        WindowEdit windowAdd = new WindowEdit();
        //        windowAdd.gridEditCategory.Visibility = Visibility.Visible;
        //        windowAdd.buttonOk.Content = "Добавить";


        //        bool? result = windowAdd.ShowDialog();

        //        if (result == true)
        //        {
        //            // запишем в базу

        //            Category category = new Category();
        //            category.Name = windowAdd.CategoryName;

        //            db.Category.Add(category);
        //            db.SaveChanges();

        //            this.ShowAllCategories();
        //        }
        //    }
        //}


        //public GridLineCategory(int number, string nameCategory, int quantityTests)
        //{
        //    this.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(10.0, GridUnitType.Star)
        //    });
        //    this.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(1.0, GridUnitType.Star)
        //    });
        //    this.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(1.0, GridUnitType.Star)
        //    });

        //    // "Главная " кнопка
        //    /*Button*/ button = new Button { Style = (Style)(this.Resources["styleButtonForList"]) };
        //    // grid внутри главной кнопки
        //    /*Grid*/ gridLineButton = new Grid { Background = Brushes.MediumBlue };
        //    // Колонки главной кнопки.
        //    gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(1.0, GridUnitType.Star)
        //    });
        //    gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(10.0, GridUnitType.Star)
        //    });
        //    gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(1.0, GridUnitType.Star)
        //    });

        //    // Данные главной кнопки
        //    TextBlock tbNumber = new TextBlock
        //    {
        //        Text = (number + 1).ToString(),
        //        Background = Brushes.AliceBlue
        //    };
        //    TextBlock tbNameCategory = new TextBlock
        //    {
        //        Text = nameCategory,
        //        Background = Brushes.Aqua
        //    };
        //    TextBlock tbQuantityTests = new TextBlock
        //    {
        //        Text = quantityTests.ToString(),
        //        Background = Brushes.BurlyWood
        //    };

        //    // Добавление textBox с данными в кнопку.
        //    gridLineButton.Children.Add(tbNumber);
        //    gridLineButton.Children.Add(tbNameCategory);
        //    gridLineButton.Children.Add(tbQuantityTests);

        //    // Расстановка текстБоксов по колонкам
        //    Grid.SetColumn(tbNumber, 0);
        //    Grid.SetColumn(tbNameCategory, 1);
        //    Grid.SetColumn(tbQuantityTests, 2);

        //    // Скрываем зарезервированное место для кнопок админа.
        //    Grid.SetColumnSpan(button, 3);


        //    // Добавим в кнопку grid с текстБлоками (в которых данные).
        //    button.Content = gridLineButton;

        //    // Установка кнопки в первую колонку
        //    Grid.SetColumn(button, 0);

        //    // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
        //    // к доп. невидимому полю в stackPanel
        //    Binding binding = new Binding();
        //    binding.ElementName = "textBlockHiddenForSizeButtonLine";
        //    //binding.ElementName = textBlockHiddenForSizeButtonLine.Name;
        //    binding.Path = new PropertyPath("ActualWidth");
        //    gridLineButton.SetBinding(Grid.WidthProperty, binding);


        //    this.Children.Add(button);
        //}

        //public void OpenAReservedPlaceForEditingButtons()
        //{
        //    Grid.SetColumnSpan(button, 1);
        //}

        //public void SetTheMainButtonGridBinding(TextBlock textBlockHiddenForSizeButtonLine)
        //{
        //    MessageBox.Show(textBlockHiddenForSizeButtonLine.Name);
        //    // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
        //    // к доп. невидимому полю в stackPanel
        //    Binding binding = new Binding();
        //    binding.ElementName = textBlockHiddenForSizeButtonLine.Name;
        //    binding.Path = new PropertyPath("ActualWidth");
        //    gridLineButton.SetBinding(Grid.WidthProperty, binding);
        //}

        /// <summary>
        /// Данное имя уже существует в базе данных.
        /// </summary>
        /// <param name="textBoxText"></param>
        /// <returns></returns>
        internal static bool IsNameAlreadyExists(string categoryName/*, int entityId*/)
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
