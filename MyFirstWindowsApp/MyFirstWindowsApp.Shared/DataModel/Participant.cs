using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyFirstWindowsApp.DataModel
{
    public class Participant : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        private bool _checkedIn;

        public bool CheckedIn
        {
            get { return _checkedIn; }
            set
            {
                if (_checkedIn == value) return;
                _checkedIn = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
