using LearnCSharp.ViewModels;
using Services.SourceFiles;
using Services.SourceFiles.Dto;
using System;
using System.Collections;
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

namespace LearnCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FilesViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            using var service = new SourceFilesService();

            _viewModel = new FilesViewModel();
            _viewModel.Files = service.GetFiles();

            FilesView.DataContext = _viewModel;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter file path:", "Input String", "", -1, -1);
            if (string.IsNullOrEmpty(input))
                return;

            using var service = new SourceFilesService();
            service.Add(input);
            _viewModel.Files = service.GetFiles();
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FilesView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select line(s)");
                return;
            }

            //var ids = FilesView.SelectedItems.Cast<SourceFileDto>().Select(s => s.Id);

            IList selectedItems = FilesView.SelectedItems;
            List<int> ids = new List<int>();
            foreach (object item in selectedItems)
            {
                SourceFileDto dto = (SourceFileDto)item;
                ids.Add(dto.Id);
            }

            using var service = new SourceFilesService();
            service.Remove(ids);
            _viewModel.Files = service.GetFiles();
        }

        //private void removeBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (FilesView.SelectedItems.Count == 0)
        //    {
        //        MessageBox.Show("Select line(s)");
        //        return;
        //    }

        //    var ids = FilesView.SelectedItems.Cast<SourceFileDto>().Select(s => s.Id);

        //    using var service = new SourceFilesService();
        //    service.Remove(ids);
        //    _viewModel.Files = service.GetFiles();
        //}
    }
}
