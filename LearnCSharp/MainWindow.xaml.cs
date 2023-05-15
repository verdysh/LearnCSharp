using DataAccessLayer;
using LearnCSharp.ViewModels;
using Services.Extensions;
using Services.SourceFiles;
using Services.SourceFiles.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
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
            using (var db = new FilesMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FilesMonitorDbContext)].ConnectionString))
            {
                db.Database.EnsureCreated();
            }

            InitializeComponent();
            
            using var service = new SourceFilesService();

            _viewModel = new FilesViewModel();
            _viewModel.Files = new ObservableCollection<SourceFileDto>(service.GetFiles());

            FilesView.DataContext = _viewModel;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter file path:", "Input String", "", -1, -1);
            if (string.IsNullOrEmpty(input))
                return;

            using var service = new SourceFilesService();
            var addedItem = service.Add(input);
            _viewModel.Files.Add(addedItem);
        }

        /*
        private void removeBtn_Click_SimplerCode(object sender, RoutedEventArgs e)
        {
            if (FilesView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select line(s)");
                return;
            }
            List<SourceFileDto> itemsToRemove = new List<SourceFileDto>();
            foreach (object item in FilesView.SelectedItems)
            {
                SourceFileDto dto = (SourceFileDto)item;
                itemsToRemove.Add(dto);
            }

            List<int> ids = new();
            foreach(var item in itemsToRemove)
            {
                ids.Add(item.Id);
                _viewModel.Files.Remove(item);
            }

            using var service = new SourceFilesService();
            service.Remove(ids);
        }
        */

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FilesView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select line(s)");
                return;
            }

            var itemsToRemove = FilesView.SelectedItems.Cast<SourceFileDto>().ToList();
            _viewModel.Files.RemoveRange(itemsToRemove);

            using var service = new SourceFilesService();
            service.Remove(itemsToRemove.Select(i=>i.Id));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as CheckBox).DataContext as SourceFileDto;
        }
    }
}
