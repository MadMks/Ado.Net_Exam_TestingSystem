﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfApp_TestingSystem.Entity;
using WpfApp_TestingSystem.EntityAddButton;
using WpfApp_TestingSystem.EntityDeleteButton;
using WpfApp_TestingSystem.EntityEditButton;

namespace WpfApp_TestingSystem.EntityGridLine
{
    public class GridLineAnswer : GridLineEntity
    {
        private Grid gridLineButton = null;

        public int QuestionId { get; set; }

        public GridLineAnswer() {}

        public GridLineAnswer(int number, Answer currentAnswer)
        {
            // Колонки для основного Grid
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            this.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });


            // "Главная " кнопка
            button = new Button { Style = (Style)(this.Resources["styleButtonForList"]) };
            // grid внутри главной кнопки
            gridLineButton = new Grid
            {
                Background = Brushes.White
            };
            // Колонки главной кнопки.
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(10.0, GridUnitType.Star)
            });
            gridLineButton.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            });


            // Данные главной кнопки
            TextBlockForNumber textBlockNumber = new TextBlockForNumber
            {
                Text = (number + 1).ToString()
            };
            TextBlockForText textBlockQuestionName = new TextBlockForText
            {
                Text = currentAnswer.ResponseText,
                TextWrapping = TextWrapping.Wrap
            };

            CheckBox checkBox = new CheckBox
            {
                IsEnabled = false,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            if (currentAnswer.CorrectAnswer == true)
            {
                checkBox.IsChecked = true;
            }
            else
            {
                checkBox.IsChecked = false;
            }

            // Добавление элементов с данными в кнопку.
            gridLineButton.Children.Add(textBlockNumber);
            gridLineButton.Children.Add(textBlockQuestionName);
            gridLineButton.Children.Add(checkBox);

            // Расстановка textBlock по колонкам
            Grid.SetColumn(textBlockNumber, 0);
            Grid.SetColumn(textBlockQuestionName, 1);
            Grid.SetColumn(checkBox, 2);

            // Скрываем зарезервированное место для кнопок админа.
            Grid.SetColumnSpan(button, 3);

            // Добавим в кнопку grid с текстБлоками (в которых данные).
            button.Content = gridLineButton;

            // Для получения id вопроса из кнопки.
            this.QuestionId = currentAnswer.QuestionId;

            // Установка кнопки в первую колонку
            Grid.SetColumn(button, 0);

            // Привяжем grid в кнопке (чтоб растянуть на всю ширину кнопки)
            // к доп. невидимому полю в stackPanel
            Binding binding = new Binding();
            binding.ElementName = "textBlockHiddenForSizeButtonLine";
            binding.Path = new PropertyPath("ActualWidth");
            gridLineButton.SetBinding(Grid.WidthProperty, binding);

            this.Children.Add(button);
        }

        internal static bool IsNameAlreadyExists(string answerName, int entityParentId)
        {
            using (TestingSystemEntities db = new TestingSystemEntities())
            {
                Answer result = db.Answer
                    .Where(a => a.ResponseText == answerName && a.QuestionId == entityParentId)
                    .FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return true;
            }
        }

        public override void AddingAdminButtons(int idEntity)
        {
            this.ButtonEdit = new ButtonEditAnswer(idEntity);
            this.Children.Add(this.ButtonEdit);

            this.ButtonDelete = new ButtonDeleteAnswer(idEntity);
            this.Children.Add(this.ButtonDelete);
        }

        public override ButtonAddEntity AddingAnAddEntityButton()
        {
            this.ButtonAdd = new ButtonAddAnswer();

            (this.ButtonAdd as ButtonAddAnswer).QuestionId
                = this.QuestionId;

            return this.ButtonAdd;
        }
    }
}
