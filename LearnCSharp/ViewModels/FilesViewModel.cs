using LearnCSharp.UI;
using Services.SourceFiles.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnCSharp.ViewModels
{    
    public class FilesViewModel : INotifyPropertyChanged
    {
        public bool CheckedFilesExists => _files.Any(f => f.IsChecked);        

        private ObservableCollection<CheckedListItem<SourceFileDto>> _files;
        public ObservableCollection<CheckedListItem<SourceFileDto>> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }

        // Add other properties and logic as needed

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
