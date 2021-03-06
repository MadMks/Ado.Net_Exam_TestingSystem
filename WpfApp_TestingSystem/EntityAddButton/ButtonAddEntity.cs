﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp_TestingSystem.Entity;

namespace WpfApp_TestingSystem.EntityAddButton
{
    abstract public class ButtonAddEntity : Button, IAdding
    {
        public ButtonAddEntity()
        {
            this.Content = "no name button";
            this.Margin = new Thickness(8.0);
            this.Padding = new Thickness(10.0, 5.0, 10.0, 5.0);
            this.HorizontalAlignment = HorizontalAlignment.Right;
        }
        
        public abstract bool AddEntity(TestingSystemEntities db);
    }
}
