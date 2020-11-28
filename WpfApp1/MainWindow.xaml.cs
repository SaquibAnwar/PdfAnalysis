using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BrowsePdfFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fxFileDialog = new OpenFileDialog();
            fxFileDialog.DefaultExt = ".pdf";
            fxFileDialog.Filter = @"PDF Files (*.pdf)|*.pdf";
            if (fxFileDialog.ShowDialog() == true)
            {
                txtBoxInputFilePath.Text = File.ReadAllText(fxFileDialog.FileName);
            }
        }

        private void BrowseOutputFolder(object sender, RoutedEventArgs e)
        {
            //using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            //{
            //    dlg.Description = Description;
            //    dlg.SelectedPath = Text;
            //    dlg.ShowNewFolderButton = true;
            //    DialogResult result = dlg.ShowDialog();
            //    if (result == System.Windows.Forms.DialogResult.OK)
            //    {
            //        Text = dlg.SelectedPath;
            //        BindingExpression be = GetBindingExpression(TextProperty);
            //        if (be != null)
            //            be.UpdateSource();
            //    }
            //}
        }

        private void btnSubmitClick(object sender, RoutedEventArgs e)
        {
            bool isSuccess = true;
            try
            {
                if(!ValidateDetails())
                {
                    isSuccess = false;
                    return;
                }

                if(GlobalModule.IsPdfSharp)
                {
                    if(!PdfSharp.RunPdfSharp())
                    {
                        isSuccess = false;
                        MessageBox.Show("Process Failed " + GlobalModule.ErrorMessage, "Process Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    if(!ITextSharp.RunITextSharp())
                    {
                        isSuccess = false;
                        MessageBox.Show("Process Failed " + GlobalModule.ErrorMessage, "Process Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Process Failed", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            finally
            {
                //if(!isSuccess)
                //{
                //    this.ShowDialog();
                //}
                //else
                //{
                //    this.Close();
                //}
            }
            isSuccess = true;
        }

        private bool ValidateDetails()
        {
            if(!GlobalModule.IsPdfSharp && !GlobalModule.IsITextSharp)
            {
                MessageBox.Show("Please Select a library", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }

            if(string.IsNullOrEmpty(txtBoxInputFilePath.Text))
            {
                MessageBox.Show("Input File Path Cannot be empty", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                GlobalModule.InputFilePath = txtBoxInputFilePath.Text;
            }

            if (string.IsNullOrEmpty(txtBoxOutputFolderPath.Text))
            {
                MessageBox.Show("Output Folder Path Cannot be empty", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                GlobalModule.OutputFolderPath = txtBoxOutputFolderPath.Text;
            }

            if (!string.IsNullOrEmpty(txtBoxPassword.Text))
            {
                GlobalModule.Password = txtBoxPassword.Text;
            }


            if (chkRead.IsChecked.Value)
            {
                GlobalModule.isReadChecked = true;
            }

            if(chkWrite.IsChecked.Value)
            {
                GlobalModule.IsWriteChecked = true;
                if (string.IsNullOrEmpty(txtBoxInputText.Text))
                {
                    MessageBox.Show("Input Text Cannot be empty", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return false;
                }
                else
                {
                    GlobalModule.InputText = txtBoxInputText.Text;
                }
            }

            if (chkEncrypt.IsChecked.Value)
            {
                GlobalModule.IsEncryptChecked = true;
                if (string.IsNullOrEmpty(txtBoxEncryptPassword.Text))
                {
                    MessageBox.Show("Encryption Password Cannot be empty", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return false;
                }
                else
                {
                    GlobalModule.EncryptionPassword = txtBoxEncryptPassword.Text;
                }
            }

            if (chkDecrypt.IsChecked.Value)
            {
                GlobalModule.IsDecryptChecked = true;
                if (string.IsNullOrEmpty(txtBoxDecryptPassword.Text))
                {
                    MessageBox.Show("Decryption Password Cannot be empty", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return false;
                }
                else
                {
                    GlobalModule.DecryptionPassword = txtBoxDecryptPassword.Text;
                    if (string.IsNullOrEmpty(GlobalModule.Password))
                    {
                        GlobalModule.Password = GlobalModule.DecryptionPassword;
                    }
                }
            }

            if (!GlobalModule.isReadChecked && !GlobalModule.IsWriteChecked && !GlobalModule.IsEncryptChecked && !GlobalModule.IsDecryptChecked)
            {
                MessageBox.Show("Please Select any one of the functionality", "Validation Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void PdfSharpCheck_Checked(object sender, RoutedEventArgs e)
        {
            if(PdfSharpCheck.IsChecked == true)
            {
                GlobalModule.IsPdfSharp = true;
                if (!string.IsNullOrEmpty(txtBoxPassword.Text))
                {
                    txtBoxDecryptPassword.Text = txtBoxPassword.Text;
                }
            }
            else
            {
                GlobalModule.IsPdfSharp = false;
            }
        }

        private void ITextSharpCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (ITextSharpCheck.IsChecked == true)
            {
                GlobalModule.IsPdfSharp = false;
                GlobalModule.IsITextSharp = true;
                if (!string.IsNullOrEmpty(txtBoxPassword.Text))
                {
                    txtBoxDecryptPassword.Text = txtBoxPassword.Text;
                }
            }
            else
            {
                GlobalModule.IsITextSharp = false;
            }
        }
    }
}
