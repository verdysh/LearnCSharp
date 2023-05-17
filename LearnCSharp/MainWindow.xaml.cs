using DataAccessLayer;
using LearnCSharp.UI;
using LearnCSharp.ViewModels;
using Services.Extensions;
using Services.Helpers;
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
            using (var db = DbHelper.CreateInstance())
            {
                db.Database.EnsureCreated();
            }

            InitializeComponent();
            
            using var service = new SourceFilesService(DbHelper.CreateSourceFileRepositoryInstance());

            _viewModel = new FilesViewModel();
            
            var fileItems = service.GetFiles();
            _viewModel.Files = new ObservableCollection<CheckedListItem<SourceFileDto>>(
                fileItems
                .Select(
                    f => new CheckedListItem<SourceFileDto>()
                    {
                        IsChecked = false,
                        Item = f
                    }
                )
            );

            _viewModel.Files.CollectionChanged += Files_CollectionChanged;

            FilesView.DataContext = _viewModel;
            removeBtn.DataContext = _viewModel;
        }

        private void Files_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _viewModel.OnPropertyChanged(nameof(FilesViewModel.CheckedFilesExists));
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter file path:", "Input String", "", -1, -1);
            if (string.IsNullOrEmpty(input))
                return;

            using var service = new SourceFilesService(DbHelper.CreateSourceFileRepositoryInstance());
            var addedItem = service.Add(input);
            _viewModel.Files.Add(new CheckedListItem<SourceFileDto>
            {
                IsChecked=false,
                Item = addedItem
            });
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!_viewModel.CheckedFilesExists)
            {
                MessageBox.Show("Check some line(s)");
                return;
            }

            var itemsToRemove = FilesView.ItemsSource.Cast<CheckedListItem<SourceFileDto>>().Where(c => c.IsChecked).ToList();
            _viewModel.Files.RemoveRange(itemsToRemove);

            using var service = new SourceFilesService(DbHelper.CreateSourceFileRepositoryInstance());
            service.Remove(itemsToRemove.Select(i=>i.Item.Id));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as CheckBox).DataContext as CheckedListItem<SourceFileDto>;
            _viewModel.OnPropertyChanged(nameof(FilesViewModel.CheckedFilesExists));
        }
    }
}
