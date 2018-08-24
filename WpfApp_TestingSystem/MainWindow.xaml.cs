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
        private List<Question> questions;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.numberOfTheCurrentQuestion = 0;

            this.listBoxAllTests.MouseLeftButtonUp += ListBoxAllTests_MouseLeftButtonUp;
            this.buttonPassing_Reply.Click += ButtonPassing_Reply_Click;
        }

        private void ButtonPassing_Reply_Click(object sender, RoutedEventArgs e)
        {
            this.numberOfTheCurrentQuestion++;

            this.ShowCurrentQuestion();
        }

        private void ListBoxAllTests_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // загрузить страницу прохождения теста
            this.gridTestSelection.Visibility = Visibility.Hidden;

            this.gridPassingTheTest.Visibility = Visibility.Visible;

            // Title название теста   // TODO название категории
            this.textBlockPassing_Title.Text
                = this.listBoxAllTests.SelectedValue.ToString();

            

            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                db.Database.Log = Console.Write;

                // TODO загрузить все вопросы -
                questions = (
                    from question in db.Question
                    where question.Test.Name == this.listBoxAllTests.SelectedValue.ToString()
                    select question
                    )
                    .ToList();
                // TODO загрузить все ответы -
                //var answers = from answer in db.Answer
                //                  //join 
                //              where answer.QuestionId

                // Question
                //this.textBlockPassing_Question.Text
                //    = (
                //    from question in db.Question
                //    where question.
                //    select question.QuestionText
                //    )
                //    .FirstOrDefault();


                // Show
                // Вывод на экран 1го вопроса
                this.ShowCurrentQuestion();

                // вывод вопросов
                //for (int i = 0; i < questions.Count; i++)
                //{
                //    MessageBox.Show(questions[i].QuestionText);
                //}
            }
        }

        /// <summary>
        /// Показать текст текущего вопроса.
        /// </summary>
        private void ShowCurrentQuestion()
        {
            // проверка на кол-во вопросов

            this.textBlockPassing_Question.Text
                = questions[this.numberOfTheCurrentQuestion].QuestionText;
        }

        private void buttonStudent_Click(object sender, RoutedEventArgs e)
        {
            this.gridUserTypeSelection.Visibility = Visibility.Hidden;

            this.gridTestSelection.Visibility = Visibility.Visible;


            using (TestingSystemEntities db = new TestingSystemEntities())
            {
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
