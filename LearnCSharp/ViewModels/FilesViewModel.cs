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
        private ObservableCollection<SourceFileDto> _files;
        public ObservableCollection<SourceFileDto> Files
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
