using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp_TestingSystem.EntityAddButton;
using WpfApp_TestingSystem.EntityDeleteButton;
using WpfApp_TestingSystem.EntityEditButton;
using WpfApp_TestingSystem.EntityGridLine;

namespace WpfApp_TestingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfTheCurrentQuestion;
        /// <summary>
        /// Кол-во правильных ответов студента.
        /// </summary>
        private int numberOfCorrectAnswersStudent; // HACK можно поменять на свойство в связи
        /// <summary>
        /// Результат тестирования в процентах.
        /// </summary>
        private int testResultInPercent;

        private bool isTeacher;

        private List<Category> categories;
        private List<Test> tests;
        private List<Question> questionsOfTheSelectedTest;
        private List<Answer> answersToTheCurrentQuestion;

        //private TextBlock textBlockHiddenForSizeButtonLine = null;

        //public TextBlock TextBlockHiddenForSizeButtonLine
        //{
        //    get { return textBlockHiddenForSizeButtonLine; }
        //}

        private Level level;
        private int currentIdTest;
        private int currentIdCategory;
        private int currentIdQuestion;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // new memory
            this.categories = new List<Category>();
            this.tests = new List<Test>();

            // await
            //this.LoadingCategoriesFromTheDatabase();


            this.numberOfTheCurrentQuestion = 0;
            this.numberOfCorrectAnswersStudent = 0;

            this.buttonTeacher.Click += ButtonTeacher_Click;
            this.buttonStudent.Click += ButtonStudent_Click;

            this.buttonAllTests.Click += ButtonAllTests_Click;
            this.buttonCategory.Click += ButtonCategory_Click;

            //this.listBoxAllTests.MouseLeftButtonUp += ListBoxAllTests_MouseLeftButtonUp;
            this.buttonPassing_Reply.Click += ButtonPassing_Reply_Click;
        }

        // +
        private void ButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            this.level = Level.SelectCategoryOrAllTest;

            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridCategoryOrAllTest.Visibility = Visibility.Visible;

            this.isTeacher = false;
        }

        // +
        private void ButtonAllTests_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

            //this.stackPanelSelection.Visibility = Visibility.Visible;
            this.gridSelection.Visibility = Visibility.Visible;

            this.ShowTestsOfSelectedCategories(null);
        }
        // +
        private void ShowTestsOfSelectedCategories(int? idCategory)
        {
            this.ShowHeaderTest();

            if (this.stackPanelSelection.Children.Count > 0)
            {
                this.stackPanelSelection.Children.Clear();
            }

            //this.LoadingCategoriesFromTheDatabase();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                List<Test> listTestsOfSelectedCategories = null;
                if (this.isTeacher)
                {
                    listTestsOfSelectedCategories
                        = this.GetListTestsOfSelectedCategories(idCategory, db);
                }
                else
                {
                    listTestsOfSelectedCategories
                        = this.GetListActiveTestsOfSelectedCategories(idCategory, db);
                }

                GridLineTest gridLineTest = null;

                for (int i = 0; i < listTestsOfSelectedCategories.Count(); i++)
                {
                    /*GridLineTest*/
                    gridLineTest
                       = new GridLineTest(
                           i,
                           listTestsOfSelectedCategories[i]
                       );

                    gridLineTest.IsTeacher = this.isTeacher;

                    // Установить стиль кнопки внутри grid
                    (gridLineTest.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

                    // TODO написать обработчик нажатия на название теста (Test).
                    //(gridLineTest.Children[0] as Button).Click += ButtonCategoryLine_Click;   //TODO !!! обработчик
                    (gridLineTest.Children[0] as Button).Click += ButtonInGridLineTest_Click;

                    if (this.isTeacher)
                    {
                        this.CreateEditingButtons(gridLineTest, listTestsOfSelectedCategories[i].Id);
                    }

                    this.stackPanelSelection.Children.Add(gridLineTest);


                    //buttonTestLine.Click += ButtonTestLine_Click;

                    // TODO CREATE method "button ADD Test"
                }

                if (this.isTeacher)
                {
                    if (gridLineTest == null)
                    {
                        gridLineTest = new GridLineTest();
                        gridLineTest.CategoryId = this.currentIdCategory;
                    }
                    this.CreateAddButton(gridLineTest);
                }
            }
        }

        private List<Test> GetListTestsOfSelectedCategories(int? idCategory, TestingSystemEntities db)
        {
            List<Test> listTestsOfSelectedCategories;

            if (idCategory == null)
            {
                this.level = Level.AllTests;
                this.currentIdCategory = -1;    // TODO HACK ??? #14.

                listTestsOfSelectedCategories
                    = (
                    from test in db.Test
                    select test
                    )
                    .ToList();
            }
            else
            {
                this.level = Level.TestsOfTheSelectedCategory;

                listTestsOfSelectedCategories
                    = (
                    from test in db.Test
                    where test.CategoryId == idCategory
                    select test
                    )
                    .ToList();
            }

            return listTestsOfSelectedCategories;
        }

        private List<Test> GetListActiveTestsOfSelectedCategories(int? idCategory, TestingSystemEntities db)
        {
            List<Test> listTestsOfSelectedCategories;

            if (idCategory == null)
            {
                this.level = Level.AllTests;
                this.currentIdCategory = -1;    // TODO HACK ??? #14.

                listTestsOfSelectedCategories
                    = (
                    from test in db.Test
                    where test.Active == true
                    select test
                    )
                    .ToList();
            }
            else
            {
                this.level = Level.TestsOfTheSelectedCategory;

                listTestsOfSelectedCategories
                    = (
                    from test in db.Test
                    where test.CategoryId == idCategory
                    where test.Active == true
                    select test
                    )
                    .ToList();
            }

            return listTestsOfSelectedCategories;
        }

        // +
        private void ButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

            //this.stackPanelSelection.Visibility = Visibility.Visible;
            this.gridSelection.Visibility = Visibility.Visible;

            //this.ShowHeaderCategory();
            
            this.ShowAllCategories();
        }

        private void ShowHeaderCategory()
        {
            this.gridHeaderCategory.Visibility = Visibility.Visible;

            this.gridHeaderTest.Visibility = Visibility.Hidden;
            this.gridHeaderQuestion.Visibility = Visibility.Hidden;
            this.gridHeaderAnswer.Visibility = Visibility.Hidden;

            if (this.isTeacher)
            {
                Grid.SetColumnSpan(this.gridHeaderCategory, 1);
            }
        }

        // +
        private void ShowAllCategories()
        {
            this.ShowHeaderCategory();

            // TODO clear stackP
            if (this.stackPanelSelection.Children.Count > 0)
            {
                this.stackPanelSelection.Children.Clear();
            }

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                this.level = Level.AllCategories;

                //var listOfAllCategories
                //        = (
                //        from category in db.Category.Include("Test")
                //        select category
                //        )
                //        .ToList();

                List<Category> listOfAllCategories = null;

                if (this.isTeacher)
                {
                    listOfAllCategories
                        = (
                        from category in db.Category.Include("Test")
                        select category
                        )
                        .ToList();
                }
                else
                {
                    listOfAllCategories
                        = (
                        from category in db.Category.Include("Test")
                        where category.Active == true
                        select category
                        )
                        .ToList();
                }


                GridLineCategory gridLineCategory = null;

                for (int i = 0; i < listOfAllCategories.Count(); i++)
                {
                    // TODO new line fo list category - method

                    // TEST создаю gridLine и передаю в него сущность
                    /*GridLineCategory */gridLineCategory
                        = new GridLineCategory(
                            i,
                            listOfAllCategories[i]
                        );

                    // HACK ?! - добавляю id категории - чтоб при нажатии на кнопку (грид)
                    // вывести тесты данной категории - по добавленному id (в свойство gridLineButton).
                    //gridLineCategory.CategoryID = listOfAllCategories[i].Id;
                    gridLineCategory.IsTeacher = this.isTeacher;

                    // Установить стиль кнопки внутри grid
                    (gridLineCategory.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

                    (gridLineCategory.Children[0] as Button).Click += ButtonCategoryLine_Click;

                    if (this.isTeacher)
                    {
                        this.CreateEditingButtons(gridLineCategory, listOfAllCategories[i].Id);
                    }

                    this.stackPanelSelection.Children.Add(gridLineCategory);
                }

                // Если Учитель, то добавим кнопку "Добавить" категорию.
                // TODO ! метод - проверка (если учитель) -> то создать кнопку Add (abstract)
                if (this.isTeacher)
                {
                    this.CreateAddButton(gridLineCategory);
                }
            }
        }

        private void CreateAddButton(GridLineEntity gridLineEntity)
        {
            ButtonAddEntity buttonAddEntity = 
                gridLineEntity.AddingAnAddEntityButton();
            gridLineEntity.ButtonAdd.Click += ButtonAddEntity_Click;

            //this.stackPanelSelection.Children.Add(buttonAddEntity);

            // test 20.10
            this.gridForButtonAddEntity.Children.Clear();
            this.gridForButtonAddEntity.Children.Add(buttonAddEntity);
        }

        private void ButtonAddEntity_Click(object sender, RoutedEventArgs e)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                if (sender is ButtonAddEntity)
                {
                    bool isAdded = (sender as ButtonAddEntity).AddEntity(db);

                    if (isAdded)
                    {
                        // TODO HACK должна выводится та сущность, которую добавили!

                        this.ShowGridLineEntity(sender);
                    }
                }
            }
        }

        // TODO перместить реализацию в класс добавления для каждой сущности.
        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                WindowEdit windowAdd = new WindowEdit();
                windowAdd.gridEditCategory.Visibility = Visibility.Visible;
                windowAdd.buttonOk.Content = "Добавить";


                bool? result = windowAdd.ShowDialog();

                if (result == true)
                {
                    // запишем в базу

                    Category category = new Category();
                    category.Name = windowAdd.CategoryName;

                    db.Category.Add(category);
                    db.SaveChanges();

                    this.ShowAllCategories();
                }
            }
        }

        private void CreateEditingButtons(GridLineEntity gridLineEntity, int idEntity)
        {
            // открытие зарезервированного места для кнопок редактирования
            Grid.SetColumnSpan(this.textBlockHiddenForSizeButtonLine, 1);

            gridLineEntity.OpenAReservedPlaceForEditingButtons();


            // Добавление кнопок админа.

            #region ForDelete_workingCode_addButtonEditAndDelete

            
            //if (gridLineEntity is GridLineCategory)
            //{
            //    //// Кнопка "Редактирровать".
            //    //Button buttonEditCategory = new Button { Content = "Edit", Tag = idEntity };
            //    //gridLineEntity.Children.Add(buttonEditCategory);
            //    //Grid.SetColumn(buttonEditCategory, 1);
            //    //// Кнопка "Удалить".
            //    //Button buttonDelCategory = new Button { Content = "Del", Tag = idEntity };
            //    //gridLineEntity.Children.Add(buttonDelCategory);
            //    //Grid.SetColumn(buttonDelCategory, 2);

            //    //buttonEditCategory.Click += ButtonEditCategory_Click;
            //    //buttonDelCategory.Click += ButtonDelCategory_Click;



            //    // Кнопка "Редактировать".
            //    ButtonEditCategory buttonEditCategory = new ButtonEditCategory(idEntity);
            //    gridLineEntity.Children.Add(buttonEditCategory);
            //    // Кнопка "Удалить".
            //    ButtonDeleteCategory buttonDeleteCategory = new ButtonDeleteCategory(idEntity);
            //    gridLineEntity.Children.Add(buttonDeleteCategory);

            //    buttonEditCategory.Click += ButtonEditEntity_Click;
            //    buttonDeleteCategory.Click += ButtonDeleteEntity_Click;
            //}
            //else if (gridLineEntity is GridLineTest)
            //{
            //    // TODO добавить в каждый gridLineTest кнопки редактирования тестов.
            //}
            #endregion

            // проба общего кода (абстракция, интерфейс).
            gridLineEntity.AddingAdminButtons(idEntity);
            gridLineEntity.ButtonEdit.Click += ButtonEditEntity_Click;
            gridLineEntity.ButtonDelete.Click += ButtonDeleteEntity_Click;
        }

        //private void AddingEditingButtons(GridLineCategory gridLineCategory, int idCategory)
        //{
        //    // открытие зарезервированного места для кнопок редактирования
        //    Grid.SetColumnSpan(this.textBlockHiddenForSizeButtonLine, 1);

        //    gridLineCategory.OpenAReservedPlaceForEditingButtons();

        //    // Добавление кнопок редактирования.

        //    // Кнопка "Редактирровать".
        //    Button buttonEditCategory = new Button { Content = "Edit", Tag = idCategory };
        //    gridLineCategory.Children.Add(buttonEditCategory);
        //    Grid.SetColumn(buttonEditCategory, 1);
        //    // Кнопка "Удалить".
        //    Button buttonDelCategory = new Button { Content = "Del", Tag = idCategory };
        //    gridLineCategory.Children.Add(buttonDelCategory);
        //    Grid.SetColumn(buttonDelCategory, 2);

        //    buttonEditCategory.Click += ButtonEditCategory_Click;
        //    buttonDelCategory.Click += ButtonDelCategory_Click;
        //}

        private void ButtonDeleteEntity_Click(object sender, RoutedEventArgs e)
        {

            // Полное удаление категории, и всех зависимостей.

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                #region ForDelete_DeletingFullCategory

                //if (sender is ButtonDeleteCategory)
                //{
                //    int idCategory = Convert.ToInt32((sender as Button).Tag);

                //    var deleteCategory = db.Category.Where(x => x.Id == idCategory).FirstOrDefault();

                //    int testsCount = deleteCategory.Test.Count();

                //    MessageBoxResult result = MessageBox.Show(
                //        $"Категория {deleteCategory.Name} содержит {testsCount} тестов."
                //        + " \nУдалить?",
                //        $"Удаление категории {deleteCategory.Name}",
                //        MessageBoxButton.YesNo);

                //    if (result == MessageBoxResult.Yes)
                //    {
                //        if (testsCount > 0)
                //        {
                //            var deleteTests
                //                = (
                //                from test in db.Test
                //                where test.CategoryId == deleteCategory.Id
                //                select test
                //                )
                //                .ToList();

                //            Int16[] delTestsId = (from test in deleteTests select test.Id).ToArray();

                //            var deleteQuestions
                //                = (
                //                from question in db.Question
                //                    //where (from test in deleteTests select test.Id).Contains(question.TestId)
                //            where (delTestsId).Contains(question.TestId)
                //                select question
                //                )
                //                .ToList();

                //            if (deleteQuestions.Count > 0)
                //            {
                //                int[] delQuestionId = (from question in deleteQuestions select question.Id).ToArray();

                //                // answer
                //                var deleteAnswer
                //                    = (
                //                    from answer in db.Answer
                //                    where (delQuestionId).Contains(answer.QuestionId)
                //                    select answer
                //                    )
                //                    .ToList();

                //                if (deleteAnswer.Count > 0)
                //                {
                //                    db.Answer.RemoveRange(deleteAnswer);
                //                }

                //                db.Question.RemoveRange(deleteQuestions);
                //            }

                //            db.Test.RemoveRange(deleteTests);
                //        }

                //        db.Category.Remove(deleteCategory);
                //        db.SaveChanges();

                //        this.ShowAllCategories();
                //    }
                //}
                #endregion

                if (sender is ButtonDeleteEntity)
                {
                    bool isRemoved = (sender as ButtonDeleteEntity).DeletingEntity(db);

                    if (isRemoved)
                    {
                        // TODO HACK должна выводится та сущность, которую редактировали!

                        this.ShowGridLineEntity(sender);
                    }
                }

            }

            
        }

        private void ButtonEditEntity_Click(object sender, RoutedEventArgs e)
        {
            #region ForrDelete_EditingCategory

            
            // TODO new form

            //int idCategory = Convert.ToInt32((sender as Button).Tag);

            //using (TestingSystemEntities db = new TestingSystemEntities())
            //{
            //    db.Database.Log = Console.Write;

            //    WindowEdit windowEdit = new WindowEdit();
            //    windowEdit.gridEditCategory.Visibility = Visibility.Visible;

            //    var editCategory = db.Category
            //                        .Where(x => x.Id == idCategory)
            //                        //.Select(x => x.Name)
            //                        .FirstOrDefault();

            //    windowEdit.CategoryName = editCategory.Name;

            //    bool? result = windowEdit.ShowDialog();

            //    if (result == true)
            //    {
            //        // запишем в базу
            //        editCategory.Name = windowEdit.CategoryName;
            //        db.SaveChanges();

            //        this.ShowAllCategories();
            //    }
            //}
            #endregion

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                if (sender is ButtonEditEntity)
                {
                    bool isEdited = (sender as ButtonEditEntity).EditingEntity(db);
                    
                    if (isEdited)
                    {
                        // TODO HACK должна выводится та сущность, которую редактировали!

                        this.ShowGridLineEntity(sender);
                    }
                }
            }
        }

        private void ShowGridLineEntity(object sender)
        {
            if (sender is ButtonEditCategory || sender is ButtonDeleteCategory
                || sender is ButtonAddCategory)
            {
                this.ShowAllCategories();
            }
            else if (sender is ButtonEditTest || sender is ButtonDeleteTest
                || sender is ButtonAddTest)
            {
                if (level == Level.AllTests)
                {
                    this.ShowTestsOfSelectedCategories(null);
                }
                else if (level == Level.TestsOfTheSelectedCategory)
                {
                    int idCategory;

                    if ((sender as Button).Parent is GridLineTest)
                    {
                        idCategory = ((sender as Button).Parent as GridLineTest).CategoryId;
                    }
                    else
                    {
                        idCategory = this.currentIdCategory;
                    }
                    

                    this.ShowTestsOfSelectedCategories(idCategory);
                }
            }
            else if (sender is ButtonEditQuestion || sender is ButtonDeleteQuestion
                || sender is ButtonAddQuestion)
            {
                this.ShowQuestionsOfSelectedOfTest(this.currentIdTest);
            }
            else if (sender is ButtonEditAnswer || sender is ButtonDeleteAnswer
                || sender is ButtonAddAnswer)
            {
                int idQuestion;
                // TODO возможно сравнение заменить строкой:
                // >>> idQuestion = this.currentIdCategory;
                if ((sender as Button).Parent is GridLineAnswer)
                {
                    idQuestion = ((sender as Button).Parent as GridLineAnswer).QuestionId;
                }
                else
                {
                    idQuestion = this.currentIdQuestion;
                }

                this.ShowAnswersForSelectedOfQuestion(idQuestion);
            }
        }

        // TODO +
        private void ButtonCategoryLine_Click(object sender, RoutedEventArgs e)
        {
            //this.ShowHeaderTest();
            //this.ShowHeaderEntity(sender);

            // TODO показать в стекПанел тесты данной категории.

            //this.ShowTestsOfCurrentCategory(sender as ButtonCategoryLine);

            if (this.isTeacher)
            {
                this.currentIdCategory = ((sender as Button).Parent as GridLineCategory).CategoryId;
            }

            this.ShowTestsOfSelectedCategories(Convert.ToInt32((sender as Button).Tag));
        }

        //private void ShowHeaderEntity(object sender)
        //{
        //    if (sender is GridLineCategory)
        //    {
        //        this.ShowHeaderTest();
        //    }
        //    else if (sender is GridLineTest)
        //    {
                
        //    }
        //}

        private void ButtonInGridLineTest_Click(object sender, RoutedEventArgs e)
        {

            //Level tempLevel = this.level;
            //int tempCurrentIdCategory = this.currentIdCategory;
            // если учитель
            // то загружаем кнопки Вопросов

            // если студент
            // то загружаем Вопросы теста (сдача теста).

            if (this.isTeacher)
            {
                // TODO CREATE method

                //int idTest = ((sender as Button).Parent as GridLineTest).TestID;
                this.currentIdTest = ((sender as Button).Parent as GridLineTest).TestID;

                this.ShowQuestionsOfSelectedOfTest(this.currentIdTest);
            }
            else
            {
                if (this.level == Level.TestsOfTheSelectedCategory)
                {
                    this.currentIdCategory = ((sender as Button).Parent as GridLineTest).CategoryId;
                }

                // TODO при нажатии на тест - запуск вопросов теста
                this.LaunchingTestQuestions(sender as Button);

                //this.level = tempLevel;
                //this.currentIdCategory = tempCurrentIdCategory;
            }
        }

        private void ShowHeaderTest()
        {
            this.gridHeaderTest.Visibility = Visibility.Visible;

            this.gridHeaderCategory.Visibility = Visibility.Hidden;
            this.gridHeaderQuestion.Visibility = Visibility.Hidden;
            this.gridHeaderAnswer.Visibility = Visibility.Hidden;

            if (this.isTeacher)
            {
                Grid.SetColumnSpan(this.gridHeaderTest, 1);
            }
        }

        /// <summary>
        /// Показать вопросы выбранного теста.
        /// </summary>
        /// <param name="idTest">Id выбранного теста.</param>
        private void ShowQuestionsOfSelectedOfTest(int idTest)
        {
            this.ShowHeaderQuestion();

            if (this.stackPanelSelection.Children.Count > 0)
            {
                this.stackPanelSelection.Children.Clear();
            }
            

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                this.level = Level.QuestionsOfTheSelectedTest;

                var listOfQuestionsCurrentTest
                    = (
                    from question in db.Question.Include("Answer")
                    where question.TestId == idTest
                    select question
                    )
                    .ToList();

                GridLineQuestion gridLineQuestion = null;

                for (int i = 0; i < listOfQuestionsCurrentTest.Count(); i++)
                {
                    /*GridLineQuestion*/ gridLineQuestion
                        = new GridLineQuestion(
                            i,
                            listOfQuestionsCurrentTest[i]
                        );

                    // Установить стиль кнопки внутри grid
                    (gridLineQuestion.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

                    // TODO написать обработчик нажатия на название Вопроса (Question).
                    (gridLineQuestion.Children[0] as Button).Click += ButtonInGridLineQuestion_Click;

                    //if (this.isTeacher)
                    //{
                        this.CreateEditingButtons(gridLineQuestion, listOfQuestionsCurrentTest[i].Id);
                    //}

                    this.stackPanelSelection.Children.Add(gridLineQuestion);
                }

                if (gridLineQuestion == null)
                {
                    gridLineQuestion = new GridLineQuestion();
                    gridLineQuestion.TestId = this.currentIdTest;
                }

                if (this.isTeacher)
                {
                    this.CreateAddButton(gridLineQuestion);
                }
            }
        }

        private void ShowHeaderQuestion()
        {
            this.gridHeaderQuestion.Visibility = Visibility.Visible;

            this.gridHeaderCategory.Visibility = Visibility.Hidden;
            this.gridHeaderTest.Visibility = Visibility.Hidden;
            this.gridHeaderAnswer.Visibility = Visibility.Hidden;

            if (this.isTeacher)
            {
                Grid.SetColumnSpan(this.gridHeaderQuestion, 1);
            }
        }

        private void ButtonInGridLineQuestion_Click(object sender, RoutedEventArgs e)
        {
            // TODO показать Ответы выбранного вопроса

            this.currentIdQuestion = ((sender as Button).Parent as GridLineQuestion).QuestionID;

            this.ShowAnswersForSelectedOfQuestion(this.currentIdQuestion);
        }

        /// <summary>
        /// Показать ответы выбранного вопроса.
        /// </summary>
        /// <param name="idQuestion">Id выбранного вопроса.</param>
        private void ShowAnswersForSelectedOfQuestion(int idQuestion)
        {
            this.ShowHeaderAnswer();

            if (this.stackPanelSelection.Children.Count > 0)
            {
                this.stackPanelSelection.Children.Clear();
            }


            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                this.level = Level.AnswersForSelectedOfQuestion;

                var listOfAnswersCurrentQuestion
                    = (
                    from answer in db.Answer
                    where answer.QuestionId == idQuestion
                    select answer
                    )
                    .ToList();

                GridLineAnswer gridLineAnswer = null;

                for (int i = 0; i < listOfAnswersCurrentQuestion.Count(); i++)
                {
                    /*GridLineAnswer*/ gridLineAnswer
                        = new GridLineAnswer(
                            i,
                            listOfAnswersCurrentQuestion[i]
                        );

                    // Установить стиль кнопки внутри grid
                    (gridLineAnswer.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

                    // TODO написать обработчик нажатия на Ответ
                    //

                    this.CreateEditingButtons(gridLineAnswer, listOfAnswersCurrentQuestion[i].Id);

                    this.stackPanelSelection.Children.Add(gridLineAnswer);
                }

                if (gridLineAnswer == null)
                {
                    gridLineAnswer = new GridLineAnswer();
                    gridLineAnswer.QuestionId = this.currentIdQuestion;
                }

                if (this.isTeacher)
                {
                    this.CreateAddButton(gridLineAnswer);
                }
            }
            
        }

        private void ShowHeaderAnswer()
        {
            this.gridHeaderAnswer.Visibility = Visibility.Visible;

            this.gridHeaderCategory.Visibility = Visibility.Hidden;
            this.gridHeaderTest.Visibility = Visibility.Hidden;
            this.gridHeaderQuestion.Visibility = Visibility.Hidden;

            if (this.isTeacher)
            {
                Grid.SetColumnSpan(this.gridHeaderAnswer, 1);
            }
        }

        private void LaunchingTestQuestions(Button senderButton)
        {
            MessageBox.Show("запуск вопросов теста");
            // загрузить страницу прохождения теста
            this.gridSelection.Visibility = Visibility.Hidden;

            this.gridPassingTheTest.Visibility = Visibility.Visible;

            // Title название теста   // TODO название категории
            //this.textBlockPassing_Title.Text
            //    = this.listBoxAllTests.SelectedValue.ToString();

            //ButtonTestLine buttonTestLine = sender as ButtonTestLine;
            GridLineTest gridLineTest = senderButton.Parent as GridLineTest;

            this.textBlockPassing_Title.Text
                = gridLineTest.CategoryName;


            this.answersToTheCurrentQuestion = new List<Answer>();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                //db.Configuration.LazyLoadingEnabled = false;

                // TODO загрузить все вопросы и ответы
                this.questionsOfTheSelectedTest = (
                    from question in db.Question.Include("Answer")  // HACK или безотложная (много маленьких запросов)
                    where question.Test.Id == gridLineTest.TestID
                    where question.Active == true
                    select question
                    )
                    .ToList();

                // пробую загрузить ответы для одого текущего теста.

                foreach (var item in this.questionsOfTheSelectedTest)
                {
                    this.answersToTheCurrentQuestion
                        .AddRange(item.Answer.Where(x => x.QuestionId == item.Id).Select(x => x));
                }


                this.ShowQuestionWithAnswers();
            }
        }









        //
        //
        //
        // Work code - #1
        //
        //
        //


        private void ButtonTeacher_Click(object sender, RoutedEventArgs e)
        {
            this.level = Level.SelectCategoryOrAllTest;

            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridCategoryOrAllTest.Visibility = Visibility.Visible;

            this.isTeacher = true;
        }

        private void ButtonPassing_Reply_Click(object sender, RoutedEventArgs e)
        {
            // TODO подсчет правильных ответов - метод
            this.CheckingTheCorrectAnswers();

            this.numberOfTheCurrentQuestion++;

            this.ShowQuestionWithAnswers();
        }

        /// <summary>
        /// Проверка правильных ответов (подсчет).
        /// </summary>
        private void CheckingTheCorrectAnswers()
        {
            if (this.stackPanelPassing_Answer.Children.Count > 0)
            {
                foreach (RadioButton item in this.stackPanelPassing_Answer.Children)
                {
                    if (this.IsCorrectAnswerIsSelected(item))
                    {
                        this.numberOfCorrectAnswersStudent++;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Выбран правильный ответ.
        /// </summary>
        /// <param name="item">radioButton</param>
        /// <returns>true если выбран правильный ответ.</returns>
        private bool IsCorrectAnswerIsSelected(RadioButton item)
        {
            return item.IsChecked == true && Convert.ToBoolean(item.Tag) == true;
        }

        private void ListBoxAllTests_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // загрузить страницу прохождения теста
            //this.gridTestSelection.Visibility = Visibility.Hidden;

            //this.gridPassingTheTest.Visibility = Visibility.Visible;

            //// Title название теста   // TODO название категории
            //this.textBlockPassing_Title.Text
            //    = this.listBoxAllTests.SelectedValue.ToString();

            //this.answers = new List<Answer>();

            //using (TestingSystemEntities db = new TestingSystemEntities())
            //{
            //    db.Database.Log = Console.Write;

            //    //db.Configuration.LazyLoadingEnabled = false;

            //    // TODO загрузить все вопросы и ответы
            //    questions = (
            //        from question in db.Question.Include("Answer")  // HACK или безотложная (много маленьких запросов)
            //        where question.Test.Name == this.listBoxAllTests.SelectedValue.ToString()
            //        select question
            //        )
            //        .ToList();

            //    // пробую загрузить ответы для одого текущего теста.
                
            //    foreach (var item in this.questions)
            //    {
            //        this.answers.AddRange(item.Answer.Where(x => x.QuestionId == item.Id).Select(x => x));
            //    }


            //    this.ShowQuestionWithAnswers();
            //}
        }

        private void ShowQuestionWithAnswers()
        {
            // Проверка на кол-во вопросов (номер вопроса)
            if (this.numberOfTheCurrentQuestion < this.questionsOfTheSelectedTest.Count())
            {
                this.ShowCurrentQuestion();

                this.ShowCurrentAnswers();
            }
            else
            {
                this.ComputeOfResults();
            }
        }

        /// <summary>
        /// Подсчет результатов.
        /// </summary>
        private void ComputeOfResults()
        {
            // TODO подсчет
            this.ComputeOfPercent();

            this.ShowTestResult();

            this.ResetTest();

            // TODO здесь можно записать результат в таблицу.

            this.gridPassingTheTest.Visibility = Visibility.Hidden;
            this.gridSelection.Visibility = Visibility.Visible;

            this.ShowTests();
        }

        private void ShowTestResult()
        {
            MessageBox.Show(
                            $"Результат прохождения теста: {this.testResultInPercent}%",
                            "Результат",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                            );
        }

        private void ResetTest()
        {
            this.numberOfTheCurrentQuestion = 0;
            this.testResultInPercent = 0;
            this.questionsOfTheSelectedTest.Clear();
            this.numberOfCorrectAnswersStudent = 0;
        }

        private void ComputeOfPercent()
        {
            this.testResultInPercent = (100 / this.questionsOfTheSelectedTest.Count) * this.numberOfCorrectAnswersStudent;
        }

        /// <summary>
        /// Показать ответы для вопроса.
        /// </summary>
        private void ShowCurrentAnswers()
        {
            if (this.stackPanelPassing_Answer.Children.Count > 0)
            {
                this.stackPanelPassing_Answer.Children.Clear();
            }

            // TODO динамическое создание элементов

            Thickness margin = new Thickness(5.0);

            foreach (var answer in this.questionsOfTheSelectedTest[this.numberOfTheCurrentQuestion].Answer)
            {
                this.stackPanelPassing_Answer.Children.Add(
                    new RadioButton {
                        Content = answer.ResponseText,
                        Tag = answer.CorrectAnswer,
                        Margin = margin
                        }
                    );
            } 
        }

        /// <summary>
        /// Показать текст текущего вопроса.
        /// </summary>
        private void ShowCurrentQuestion()
        {
            this.textBlockPassing_Question.Text
                = this.questionsOfTheSelectedTest[this.numberOfTheCurrentQuestion].QuestionText;

        }

        //private void buttonStudent_Click(object sender, RoutedEventArgs e)
        //{
        //    this.gridUserTypeSelection.Visibility = Visibility.Hidden;

        //    this.gridTestSelection.Visibility = Visibility.Visible;


        //    using (TestingSystemEntities db = new TestingSystemEntities())
        //    {
        //        db.Database.Log = Console.Write;

        //        this.listBoxAllTests.ItemsSource
        //            = (
        //            from test in db.Test
        //            select test.Name
        //            )
        //            .ToList();
        //    }
        //}

        private void buttonMenuBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.level == Level.SelectCategoryOrAllTest)
            {
                this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

                this.gridUserTypeSelection.Visibility = Visibility.Visible;

                // Закрытие зарезервированного места для кнопок редактирования
                Grid.SetColumnSpan(this.textBlockHiddenForSizeButtonLine, 3);

                Grid.SetColumnSpan(this.gridHeaderCategory, 2);
                Grid.SetColumnSpan(this.gridHeaderTest, 2);

                // test 20.10
                this.gridForButtonAddEntity.Children.Clear();
            }
            else if (this.level == Level.AllTests)
            {
                this.level = Level.SelectCategoryOrAllTest;

                this.gridSelection.Visibility = Visibility.Hidden;
                this.gridCategoryOrAllTest.Visibility = Visibility.Visible;
            }
            else if (this.level == Level.AllCategories)
            {
                this.level = Level.SelectCategoryOrAllTest;

                this.gridSelection.Visibility = Visibility.Hidden;
                this.gridCategoryOrAllTest.Visibility = Visibility.Visible;
            }
            else if (this.level == Level.TestsOfTheSelectedCategory)
            {
                //this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

                this.ShowAllCategories();
            }
            else if (this.level == Level.QuestionsOfTheSelectedTest)
            {
                this.ShowTests();
            }
            else if (this.level == Level.AnswersForSelectedOfQuestion)
            {
                this.ShowQuestionsOfSelectedOfTest(this.currentIdTest);
            }

        }

        private void ShowTests()
        {
            if (this.currentIdCategory != -1)
            {
                this.ShowTestsOfSelectedCategories(this.currentIdCategory);
            }
            else
            {
                this.ShowTestsOfSelectedCategories(null);
            }
        }

        private void buttonMenuUsersExit_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;
            this.gridHeaderCategory.Visibility = Visibility.Hidden;
            this.gridHeaderTest.Visibility = Visibility.Hidden;
            this.gridHeaderQuestion.Visibility = Visibility.Hidden;
            this.gridHeaderAnswer.Visibility = Visibility.Hidden;
            this.gridSelection.Visibility = Visibility.Hidden;

            this.gridUserTypeSelection.Visibility = Visibility.Visible;

            // test 20.10
            this.gridForButtonAddEntity.Children.Clear();

            // Закрытие зарезервированного места для кнопок редактирования
            Grid.SetColumnSpan(this.textBlockHiddenForSizeButtonLine, 3);

            Grid.SetColumnSpan(this.gridHeaderCategory, 2);
            Grid.SetColumnSpan(this.gridHeaderTest, 2);
        }
    }
}
