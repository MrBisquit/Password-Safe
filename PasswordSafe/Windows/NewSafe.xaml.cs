using Microsoft.Win32;
using PasswordSafe.Functions;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PasswordSafe.Windows
{
    /// <summary>
    /// Interaction logic for NewSafe.xaml
    /// </summary>
    public partial class NewSafe : Window
    {
        public NewSafe()
        {
            InitializeComponent();
        }

        public string FileLocationString = "";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileLocation.Text = "No file";
        }

        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = ".wps|WTDawson Password Safe file",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WTDawson", "PasswordSafe")
            };

            if((bool)sfd.ShowDialog())
            {
                FileLocationString = new FileInfo(sfd.FileName).FullName.EndsWith(".wps") ? new FileInfo(sfd.FileName).FullName : new FileInfo(sfd.FileName).FullName + ".wps";
                FileLocation.Text = new FileInfo(sfd.FileName).Name.EndsWith(".wps") ? new FileInfo(sfd.FileName).Name : new FileInfo(sfd.FileName).Name + ".wps";
                File.WriteAllText(FileLocationString, "");
            }
        }

        private void CSB_Click(object sender, RoutedEventArgs e)
        {
            if(!File.Exists(FileLocationString))
            {
                ChooseFileButton.Focus();
                return;
            }else if(string.IsNullOrEmpty(SafePassword.Password))
            {
                SafePassword.Focus();
                return;
            } else
            {
                Safe.CreateSafe(FileLocationString, SafePassword.Password);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }
    }
}
