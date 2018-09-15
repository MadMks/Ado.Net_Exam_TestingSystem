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


        public WindowEdit()
        {
            InitializeComponent();

            this.Loaded += WindowEdit_Loaded;
        }

        private void WindowEdit_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO ? delete
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
