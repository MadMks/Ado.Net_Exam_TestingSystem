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

        private List<Category> categories;

        private List<Question> questions;
        private List<Answer> answers;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // new memory
            this.categories = new List<Category>();

            // await
            this.LoadingCategoriesFromTheDatabase();


            this.numberOfTheCurrentQuestion = 0;
            this.numberOfCorrectAnswersStudent = 0;

            this.buttonTeacher.Click += ButtonTeacher_Click;

            this.buttonCategory.Click += ButtonCategory_Click;

            this.listBoxAllTests.MouseLeftButtonUp += ListBoxAllTests_MouseLeftButtonUp;
            this.buttonPassing_Reply.Click += ButtonPassing_Reply_Click;
        }

        private void LoadingCategoriesFromTheDatabase()
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                this.categories
                    = (
                    from category in db.Category
                    select category
                    )
                    .ToList();
            }
        }

        private void ButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            this.gridCategoryOrAllTest.Visibility = Visibility.Hidden;

            this.gridSelection.Visibility = Visibility.Visible;

            this.ShowAllCategories();
        }

        private void ShowAllCategories()
        {
            for (int i = 0; i < this.categories.Count; i++)
            {
                // новая строка
                //this.gridSelection.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Кнопка для новой строки (для контейнера)
                Button button = new Button { Padding = new Thickness(15.0) };     // TODO в кнопку поместить все элементы
                // контейнер для объектов в новой строке
                Grid gridLine = new Grid();
                gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto});
                gridLine.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto});
                // объекты новой строки
                TextBlock number = new TextBlock { Text = (i + 1).ToString() };
                TextBlock name = new TextBlock { Text = this.categories[i].Name };
                // TODO кол-во тестов

                gridLine.Children.Add(number);
                gridLine.Children.Add(name);

                Grid.SetColumn(number, 0);
                Grid.SetColumn(name, 1);

                button.Content = gridLine;
                this.gridSelection.Children.Add(button);

                //this.gridSelection.Children.Add(gridLine);

                //Grid.SetRow(gridLine, i);
            }

            //foreach (var category in this.categories)
            //{
            //    Grid grid = new Grid();
            //    grid.RowDefinitions.Add(new RowDefinition());
            //    grid.Children.Add(new TextBlock { Text = "1" });
            //    grid.Children.Add(new TextBlock { Text = category.Name});
            //    Grid.SetRow(grid, 0);

            //    this.gridSelection.Children.Add(
            //        grid
            //        );
            //}
        }

        private void ButtonTeacher_Click(object sender, RoutedEventArgs e)
        {
            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridCategoryOrAllTest.Visibility = Visibility.Visible;
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
            this.gridTestSelection.Visibility = Visibility.Hidden;

            this.gridPassingTheTest.Visibility = Visibility.Visible;

            // Title название теста   // TODO название категории
            this.textBlockPassing_Title.Text
                = this.listBoxAllTests.SelectedValue.ToString();

            this.answers = new List<Answer>();

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                //db.Configuration.LazyLoadingEnabled = false;

                // TODO загрузить все вопросы и ответы
                questions = (
                    from question in db.Question.Include("Answer")  // HACK или безотложная (много маленьких запросов)
                    where question.Test.Name == this.listBoxAllTests.SelectedValue.ToString()
                    select question
                    )
                    .ToList();

                // пробую загрузить ответы для одого текущего теста.
                
                foreach (var item in this.questions)
                {
                    this.answers.AddRange(item.Answer.Where(x => x.QuestionId == item.Id).Select(x => x));
                }


                this.ShowQuestionWithAnswers();
            }
        }

        private void ShowQuestionWithAnswers()
        {
            // проверка на кол-во вопросов
            if (this.numberOfTheCurrentQuestion < this.questions.Count)
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
            this.testResultInPercent = (100 / this.questions.Count) * this.numberOfCorrectAnswersStudent;
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

            foreach (var answer in this.questions[this.numberOfTheCurrentQuestion].Answer)
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
                = this.questions[this.numberOfTheCurrentQuestion].QuestionText;

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
