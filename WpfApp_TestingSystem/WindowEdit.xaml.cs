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
using WpfApp_TestingSystem.EntityGridLine;

namespace WpfApp_TestingSystem
{
    /// <summary>
    /// Interaction logic for WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {
        /// <summary>
        /// Текст имени редактируемой сущности
        /// (чтоб можно было сохранять редактируемую сущность
        /// с тем же именем).
        /// </summary>
        private string editableField = "";

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

            //this.Loaded += WindowEdit_Loaded;
        }

        public WindowEdit(string editableField)
        {
            InitializeComponent();

            //this.Loaded += WindowEdit_Loaded;

            this.editableField = editableField;
        }

        //private void WindowEdit_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (this.gridEditCategory.IsVisible == true)
        //    {
        //        //this.textBoxCategoryName.Max
        //    }
        //    else if (this.gridEditTest.IsVisible == true)
        //    {
        //        isEdit = this.textBoxTestName.Text == this.editableField ? true : false;
        //    }
        //    else if (this.gridEditQuestion.IsVisible == true)
        //    {
        //        isEdit = this.textBoxQuestionName.Text == this.editableField ? true : false;
        //    }
        //    else if (this.gridEditAnswer.IsVisible == true)
        //    {
        //        isEdit = this.textBoxAnswerText.Text == this.editableField ? true : false;
        //    }
        //}

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsTextBoxFieldFilled())
            {
                if (!this.IsNameAlreadyExists() || this.IsLeftEditedValue())
                {
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Такое имя уже существует!",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Поле не заполнено!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            
        }

        private bool IsLeftEditedValue()
        {
            bool isEdit = false;

            if (this.gridEditCategory.IsVisible == true)
            {
                isEdit = this.textBoxCategoryName.Text == this.editableField ? true : false;
            }
            else if (this.gridEditTest.IsVisible == true)
            {
                isEdit = this.textBoxTestName.Text == this.editableField ? true : false;
            }
            else if (this.gridEditQuestion.IsVisible == true)
            {
                isEdit = this.textBoxQuestionName.Text == this.editableField ? true : false;
            }
            else if (this.gridEditAnswer.IsVisible == true)
            {
                isEdit = this.textBoxAnswerText.Text == this.editableField ? true : false;
            }

            return isEdit;
        }

        private bool IsNameAlreadyExists()
        {
            bool isExists = false;

            if (this.gridEditCategory.IsVisible == true)
            {
                isExists = GridLineCategory.IsNameAlreadyExists(this.textBoxCategoryName.Text);
            }
            else if (this.gridEditTest.IsVisible == true)
            {
                isExists = GridLineTest.IsNameAlreadyExists(this.textBoxTestName.Text);
            }
            else if (this.gridEditQuestion.IsVisible == true)
            {
                isExists = GridLineQuestion.IsNameAlreadyExists(this.textBoxQuestionName.Text);
            }
            else if (this.gridEditAnswer.IsVisible == true)
            {
                isExists = GridLineAnswer.IsNameAlreadyExists(this.textBoxAnswerText.Text);
            }

            return isExists;
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
