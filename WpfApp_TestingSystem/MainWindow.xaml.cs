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
using WpfApp_TestingSystem.Entity;
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
        private int numberOfCorrectAnswersStudent; // HACK можно поменять на свойство в связи (если писать результат в бд)
        /// <summary>
        /// Результат тестирования в процентах.
        /// </summary>
        private int testResultInPercent;

        private bool isTeacher;

        private List<Category> categories;
        private List<Test> tests;
        private List<Question> questionsOfTheSelectedTest;
        private List<Answer> answersToTheCurrentQuestion;


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
            this.categories = new List<Category>();
            this.tests = new List<Test>();

            this.numberOfTheCurrentQuestion = 0;
            this.numberOfCorrectAnswersStudent = 0;

            this.buttonTeacher.Click += ButtonTeacher_Click;
            this.buttonStudent.Click += ButtonStudent_Click;

            this.buttonAllTests.Click += ButtonAllTests_Click;
            this.buttonCategory.Click += ButtonCategory_Click;
            
            this.buttonPassing_Reply.Click += ButtonPassing_Reply_Click;
        }


        private void ButtonStudent_Click(object sender, RoutedEventArgs e)
        {
            this.level = Level.SelectCategoryOrAllTest;

            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridCategoryOrAllTest.Visibility = Visibility.Visible;

            this.isTeacher = false;
        }


        private void ButtonAllTests_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;
            
            this.gridSelection.Visibility = Visibility.Visible;

            this.ShowTestsOfSelectedCategories(null);
        }


        private void ShowTestsOfSelectedCategories(int? idCategory)
        {
            this.ShowHeaderTest();

            this.ClearTheList();


            using (TestingSystemEntities db = new TestingSystemEntities())
            {
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
                    gridLineTest
                       = new GridLineTest(
                           i,
                           listTestsOfSelectedCategories[i],
                           this.isTeacher
                       );
                    

                    // Установить стиль кнопки внутри grid
                    (gridLineTest.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

                    // Обработчик нажатия на название теста (Test).
                    (gridLineTest.Children[0] as Button).Click += ButtonInGridLineTest_Click;

                    if (this.isTeacher)
                    {
                        this.CreateEditingButtons(gridLineTest, listTestsOfSelectedCategories[i].Id);
                    }

                    this.stackPanelSelection.Children.Add(gridLineTest);
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
                this.currentIdCategory = -1;

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
                this.currentIdCategory = -1;

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

        
        private void ButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

            this.gridSelection.Visibility = Visibility.Visible;

            
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

        private void ShowAllCategories()
        {
            this.ShowHeaderCategory();

            this.ClearTheList();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                this.level = Level.AllCategories;

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
                    gridLineCategory
                        = new GridLineCategory(
                            i,
                            listOfAllCategories[i],
                            this.isTeacher
                        );


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
                if (this.isTeacher)
                {
                    if (gridLineCategory == null)
                    {
                        gridLineCategory = new GridLineCategory();
                    }

                    this.CreateAddButton(gridLineCategory);
                }
            }
        }

        private void ClearTheList()
        {
            if (this.stackPanelSelection.Children.Count > 0)
            {
                this.stackPanelSelection.Children.Clear();
            }
        }

        private void CreateAddButton(GridLineEntity gridLineEntity)
        {
            ButtonAddEntity buttonAddEntity = 
                gridLineEntity.AddingAnAddEntityButton();
            gridLineEntity.ButtonAdd.Click += ButtonAddEntity_Click;

            this.gridForButtonAddEntity.Children.Clear();
            this.gridForButtonAddEntity.Children.Add(buttonAddEntity);
        }

        private void ButtonAddEntity_Click(object sender, RoutedEventArgs e)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                if (sender is ButtonAddEntity)
                {
                    bool isAdded = (sender as ButtonAddEntity).AddEntity(db);

                    if (isAdded)
                    {
                        // Выводим ту сущность, которую добавили
                        this.ShowGridLineEntity(sender);
                    }
                }
            }
        }


        private void CreateEditingButtons(GridLineEntity gridLineEntity, int idEntity)
        {
            // Открытие зарезервированного места для кнопок редактирования
            Grid.SetColumnSpan(this.textBlockHiddenForSizeButtonLine, 1);

            gridLineEntity.OpenAReservedPlaceForEditingButtons();


            // Добавление кнопок админа.
            gridLineEntity.AddingAdminButtons(idEntity);
            gridLineEntity.ButtonEdit.Click += ButtonEditEntity_Click;
            gridLineEntity.ButtonDelete.Click += ButtonDeleteEntity_Click;
        }


        private void ButtonDeleteEntity_Click(object sender, RoutedEventArgs e)
        {

            // Полное удаление категории, и всех зависимостей.

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                if (sender is ButtonDeleteEntity)
                {
                    bool isRemoved = (sender as ButtonDeleteEntity).DeletingEntity(db);

                    if (isRemoved)
                    {
                        // Выводим ту сущность, которую редактировали.
                        this.ShowGridLineEntity(sender);
                    }
                }

            }

            
        }

        private void ButtonEditEntity_Click(object sender, RoutedEventArgs e)
        {

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                if (sender is ButtonEditEntity)
                {
                    bool isEdited = (sender as ButtonEditEntity).EditingEntity(db);
                    
                    if (isEdited)
                    {
                        // Выводим ту сущность, которую редактировали.
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


        private void ButtonCategoryLine_Click(object sender, RoutedEventArgs e)
        {
            if (this.isTeacher)
            {
                this.currentIdCategory = ((sender as Button).Parent as GridLineCategory).CategoryId;
            }

            this.ShowTestsOfSelectedCategories(Convert.ToInt32((sender as Button).Tag));
        }


        private void ButtonInGridLineTest_Click(object sender, RoutedEventArgs e)
        {
            if (this.isTeacher)
            {
                this.currentIdTest = ((sender as Button).Parent as GridLineTest).TestID;

                this.ShowQuestionsOfSelectedOfTest(this.currentIdTest);
            }
            else
            {
                if (this.level == Level.TestsOfTheSelectedCategory)
                {
                    this.currentIdCategory = ((sender as Button).Parent as GridLineTest).CategoryId;
                }

                // При нажатии на тест - запуск вопросов теста
                this.LaunchingTestQuestions(sender as Button);
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

            this.ClearTheList();
            

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
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
                    gridLineQuestion
                        = new GridLineQuestion(
                            i,
                            listOfQuestionsCurrentTest[i]
                        );

                    // Установить стиль кнопки внутри grid
                    (gridLineQuestion.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

                    // Обработчик нажатия на название Вопроса (Question).
                    (gridLineQuestion.Children[0] as Button).Click += ButtonInGridLineQuestion_Click;


                    this.CreateEditingButtons(gridLineQuestion, listOfQuestionsCurrentTest[i].Id);

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

            this.ClearTheList();


            using (TestingSystemEntities db = new TestingSystemEntities())
            {
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
                    gridLineAnswer
                        = new GridLineAnswer(
                            i,
                            listOfAnswersCurrentQuestion[i]
                        );

                    // Установить стиль кнопки внутри grid
                    (gridLineAnswer.Children[0] as Button).Style = (Style)(this.Resources["styleButtonForList"]);

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
            // Загрузить страницу прохождения теста
            this.gridSelection.Visibility = Visibility.Hidden;

            this.gridPassingTheTest.Visibility = Visibility.Visible;

            GridLineTest gridLineTest = senderButton.Parent as GridLineTest;


            this.textBlockPassing_Title.Text
                = gridLineTest.CategoryName
                + " | "
                + gridLineTest.TestName;


            this.answersToTheCurrentQuestion = new List<Answer>();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                // Загружаем все активные вопросы текущего теста.
                this.questionsOfTheSelectedTest = (
                    from question in db.Question.Include("Answer")  // HACK или безотложная (много маленьких запросов)
                    where question.Test.Id == gridLineTest.TestID
                    where question.Active == true
                    select question
                    )
                    .ToList();

                // Загружаем ответы для активных вопросов теста.
                foreach (var item in this.questionsOfTheSelectedTest)
                {
                    this.answersToTheCurrentQuestion
                        .AddRange(item.Answer.Where(x => x.QuestionId == item.Id).Select(x => x));
                }


                this.ShowQuestionWithAnswers();
            }
        }




        private void ButtonTeacher_Click(object sender, RoutedEventArgs e)
        {
            this.level = Level.SelectCategoryOrAllTest;

            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridCategoryOrAllTest.Visibility = Visibility.Visible;

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                if (db.Test.Count() == 0)
                {
                    this.buttonAllTests.IsEnabled = false;
                }
                else
                {
                    this.buttonAllTests.IsEnabled = true;
                }
            }

            this.isTeacher = true;
        }

        private void ButtonPassing_Reply_Click(object sender, RoutedEventArgs e)
        {
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
            this.ComputeOfPercent();

            this.ShowTestResult();

            this.ResetTest();

            // TODO здесь можно записать результат в таблицу,
            // когда результаты будут хранится в бд.

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

            
            Thickness margin = new Thickness(8.0);

            foreach (var answer in this.questionsOfTheSelectedTest[this.numberOfTheCurrentQuestion].Answer)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = answer.ResponseText,
                    TextWrapping = TextWrapping.Wrap
                };

                this.stackPanelPassing_Answer.Children.Add(
                    new RadioButton {
                        Content = textBlock,
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

            this.gridForButtonAddEntity.Children.Clear();

            // Закрытие зарезервированного места для кнопок редактирования
            Grid.SetColumnSpan(this.textBlockHiddenForSizeButtonLine, 3);

            Grid.SetColumnSpan(this.gridHeaderCategory, 2);
            Grid.SetColumnSpan(this.gridHeaderTest, 2);
        }
    }
}
