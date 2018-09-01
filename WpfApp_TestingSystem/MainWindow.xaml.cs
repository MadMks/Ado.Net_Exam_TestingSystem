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
            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridCategoryOrAllTest.Visibility = Visibility.Visible;

            this.isTeacher = false;
        }
        // +
        private void ButtonAllTests_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

            this.stackPanelSelection.Visibility = Visibility.Visible;

            this.ShowAllTests();
        }
        // +
        private void ShowAllTests()
        {
            // TODO clear stackP

            //this.LoadingCategoriesFromTheDatabase();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                var allTests
                    = (
                    from test in db.Test
                    select test
                    )
                    .ToList();

                for (int i = 0; i < allTests.Count(); i++)
                {
                    // TODO new line fo list category - method
                    // TODO LineButtonForCategory - class

                    //Button button = CreateaButtonForTheRow(i);

                    ButtonTestLine buttonTestLine
                        = new ButtonTestLine(
                            i,
                            allTests[i].Name,
                            allTests[i].Category.Name,
                            allTests[i].Question.Count
                        );
                    buttonTestLine.TestID = allTests[i].Id;

                    buttonTestLine.Style = (Style)(this.Resources["styleButtonForList"]); // What #1 ???
                    buttonTestLine.Click += ButtonTestLine_Click;
                    this.stackPanelSelection.Children.Add(buttonTestLine);
                }
            }
        }
        
        // +
        private void ButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

            this.stackPanelSelection.Visibility = Visibility.Visible;

            this.ShowAllCategories();
        }
        // +
        private void ShowAllCategories()
        {
            // TODO clear stackP

            //this.LoadingCategoriesFromTheDatabase();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                var listOfAllCategories
                    = (
                    from category in db.Category
                    select category
                    )
                    .ToList();

                for (int i = 0; i < listOfAllCategories.Count(); i++)
                {
                    // TODO new line fo list category - method

                    ButtonCategoryLine buttonCategoryLine
                        = new ButtonCategoryLine(
                            i,
                            listOfAllCategories[i].Name,
                            listOfAllCategories[i].Test.Count()
                            //, this.isTeacher
                            );
                    buttonCategoryLine.CategoryID = listOfAllCategories[i].Id;

                    buttonCategoryLine.Style = (Style)(this.Resources["styleButtonForList"]);
                    buttonCategoryLine.Click += ButtonCategoryLine_Click;

                    //if (this.isTeacher)
                    //{
                    //    //Binding binding = new Binding();
                    //    //binding.ElementName = "buttonCategoryLine";
                    //    //binding.Path = new PropertyPath("ActualWidth");
                    //    //gridLine.SetBinding(Grid.WidthProperty, binding);

                    //    buttonCategoryLine.IsTeacher = true;

                    //    Grid gridButtons = new Grid();
                    //    gridButtons.ColumnDefinitions.Add(new ColumnDefinition
                    //    {
                    //        Width = new GridLength(10.0, GridUnitType.Star)
                    //    });
                    //    gridButtons.ColumnDefinitions.Add(new ColumnDefinition
                    //    {
                    //        Width = new GridLength(1.0, GridUnitType.Star)
                    //    });
                    //    gridButtons.ColumnDefinitions.Add(new ColumnDefinition
                    //    {
                    //        Width = new GridLength(1.0, GridUnitType.Star)
                    //    });


                    //    Button buttonEdit = new Button { Content = "Edit" };
                    //    Button buttonDelete = new Button { Content = "Delete" };

                    //    gridButtons.Children.Add(buttonCategoryLine);
                    //    gridButtons.Children.Add(buttonEdit);
                    //    gridButtons.Children.Add(buttonDelete);

                    //    Grid.SetColumn(buttonCategoryLine, 0);
                    //    Grid.SetColumn(buttonEdit, 1);
                    //    Grid.SetColumn(buttonDelete, 2);


                    //    this.stackPanelSelection.Children.Add(gridButtons);
                    //}
                    //else
                    //{
                    //    this.stackPanelSelection.Children.Add(buttonCategoryLine);
                    //}


                    this.stackPanelSelection.Children.Add(buttonCategoryLine);
                }
            }
        }
        // TODO +
        private void ButtonCategoryLine_Click(object sender, RoutedEventArgs e)
        {
            // TODO показать в стекПанел тесты данной категории.

            this.ShowTestsOfCurrentCategory(sender as ButtonCategoryLine);
        }

        private void ShowTestsOfCurrentCategory(ButtonCategoryLine buttonCategoryLine)
        {
            if (this.stackPanelSelection.Children.Count > 0)
            {
                this.stackPanelSelection.Children.Clear();
            }



            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                //int tempCategoryID = buttonCategoryLine.CategoryID;

                var testsOfTheSelectedCategory
                    = (
                    from test in db.Test
                        //where test.CategoryId == (sender as ButtonCategoryLine).CategoryID    // What #2 ???
                    where test.CategoryId == buttonCategoryLine.CategoryID
                    //where test.CategoryId == tempCategoryID
                    select test
                    )
                    //.Where(x => x.CategoryId == 1)
                    .ToList();

                for (int i = 0; i < testsOfTheSelectedCategory.Count(); i++)
                {

                    ButtonTestLine buttonTestLine
                        = new ButtonTestLine(
                            i,
                            testsOfTheSelectedCategory[i].Name,
                            testsOfTheSelectedCategory[i].Category.Name,
                            testsOfTheSelectedCategory[i].Question.Count()
                        );
                    buttonTestLine.TestID = testsOfTheSelectedCategory[i].Id;

                    buttonTestLine.Style = (Style)(this.Resources["styleButtonForList"]); // What #1 ???
                    buttonTestLine.Click += ButtonTestLine_Click;
                    this.stackPanelSelection.Children.Add(buttonTestLine);
                }
            }
        }

        private void ButtonTestLine_Click(object sender, RoutedEventArgs e)
        {
            // TODO при нажатии на тест - запуск вопросов теста
            MessageBox.Show("запуск вопросов теста");
            // загрузить страницу прохождения теста
            this.gridTestSelection.Visibility = Visibility.Hidden;

            this.gridPassingTheTest.Visibility = Visibility.Visible;

            // Title название теста   // TODO название категории
            //this.textBlockPassing_Title.Text
            //    = this.listBoxAllTests.SelectedValue.ToString();

            ButtonTestLine buttonTestLine = sender as ButtonTestLine;

            this.textBlockPassing_Title.Text
                = buttonTestLine.CategoryName;


            this.answersToTheCurrentQuestion = new List<Answer>();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                //db.Configuration.LazyLoadingEnabled = false;

                // TODO загрузить все вопросы и ответы
                this.questionsOfTheSelectedTest = (
                    from question in db.Question.Include("Answer")  // HACK или безотложная (много маленьких запросов)
                    //where question.Test.Name == this.listBoxAllTests.SelectedValue.ToString()
                    where question.Test.Id == buttonTestLine.TestID
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
            // проверка на кол-во вопросов (номер вопроса)
            if (this.numberOfTheCurrentQuestion < this.questionsOfTheSelectedTest.Count)
            {
                // Show
                // Вывод на экран 1го вопроса
                this.ShowCurrentQuestion();
                // ответов для 1го вопроса
                this.ShowCurrentAnswers();
            }
            else
            {
                // TODO подсчет результатов
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

            // TODO вывод результата на экран - метод
            

            // TODO запись результата в таблицу.

            throw new NotImplementedException();
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

        private void buttonStudent_Click(object sender, RoutedEventArgs e)
        {
            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridTestSelection.Visibility = Visibility.Visible;


            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                this.listBoxAllTests.ItemsSource
                    = (
                    from test in db.Test
                    select test.Name
                    )
                    .ToList();
            }
        }

    }
}
