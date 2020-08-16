using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace WPFTwilioExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region  Constructor
        public MainWindow()
        {
            InitializeComponent();

            //Initialize the comboboxes
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 0, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Common method for the CanExecute of commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Called when the main text box changes its selection.
        /// Synchronizes the combo boxes, and  bold/italic buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = MyTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;

            temp = MyTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
            if(temp is Double)
                cmbFontSize.Text = temp.ToString();

            temp = MyTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            
            temp = MyTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            
            string info = string.Format($"{MyTextBox.Selection.Text}");
            StatusBlock.Text = info;
        }
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                MyTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }
        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }



        #endregion

        #region Command Handlers
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MyTextBox.Document.Blocks.Clear();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(MyTextBox.Document.ContentStart, MyTextBox.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(MyTextBox.Document.ContentStart, MyTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }
        #endregion
    }
}
