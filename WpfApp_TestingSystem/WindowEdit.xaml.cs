﻿using System;
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
            if (true)
            {

            }

            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
