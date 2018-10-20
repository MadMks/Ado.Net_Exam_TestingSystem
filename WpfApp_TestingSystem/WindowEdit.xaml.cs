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
using System.Windows.Shapes;

namespace WpfApp_TestingSystem
{
    /// <summary>
    /// Interaction logic for WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {
        public string CategoryName
        {
            get { return this.textBoxCategoryName.Text; }
            set { this.textBoxCategoryName.Text = value; }
        }
        
        public string TestName
        {
            get { return this.textBoxTestName.Text; }
            set { this.textBoxTestName.Text = value; }
        }

        public string QuestionName
        {
            get { return this.textBoxQuestionName.Text; }
            set { this.textBoxQuestionName.Text = value; }
        }

        public string AnswerName
        {
            get { return this.textBoxAnswerText.Text; }
            set { this.textBoxAnswerText.Text = value; }
        }


        public WindowEdit()
        {
            InitializeComponent();

            this.Loaded += WindowEdit_Loaded;
        }

        private void WindowEdit_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO ? delete
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsTextBoxFieldFilled())
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Поле не заполнено!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            
        }

        private bool IsTextBoxFieldFilled()
        {
            bool isFilled = false;

            if (this.gridEditCategory.IsVisible == true)
            {
                isFilled = this.IsTextFieldFilled(this.textBoxCategoryName);
            }
            else if (this.gridEditTest.IsVisible == true)
            {
                isFilled = this.IsTextFieldFilled(this.textBoxTestName);
            }
            else if (this.gridEditQuestion.IsVisible == true)
            {
                isFilled = this.IsTextFieldFilled(this.textBoxQuestionName);
            }
            else if (this.gridEditAnswer.IsVisible == true)
            {
                isFilled = this.IsTextFieldFilled(this.textBoxAnswerText);
            }

            return isFilled;
        }

        private bool IsTextFieldFilled(TextBox textBoxField)
        {
            if (textBoxField.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
