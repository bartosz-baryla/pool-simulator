using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace ViewModel
{
    public class SecondWindowViewModel
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(DisplayValue));
            }
        }

        public string DisplayValue => $"Symulacja dla {Value} kul.";

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public SecondWindowViewModel(int value)
        {
            Value = value;
            StartCommand = new RelayCommand(StartAction);
            StopCommand = new RelayCommand(StopAction);
        }

        private void StartAction()
        {
            MessageBox.Show("Wystartowało");
            // odpowiednie akcje wykonywane
        }

        private void StopAction()
        {
            MessageBox.Show("Zatrzymano!");
            // odpowiednie akcje wykonywane
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
