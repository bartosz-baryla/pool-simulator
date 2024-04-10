using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel;

namespace ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AbstractModelApi modelObj;
        private int ballsCount = 5;
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SubtractCommand { get; }

        public MainWindowViewModel()
        {
            modelObj = AbstractModelApi.CreateApi();
            StartCommand = new RelayCommand(StartAction);
            StopCommand = new RelayCommand(StopAction);
            AddCommand = new RelayCommand(AddAction);
            SubtractCommand = new RelayCommand(SubtractAction);
        }

        public int BallsCount
        {
            get => ballsCount;
            set
            {
                ballsCount = value;
                OnPropertyChanged();
            }
        }

        private void StartAction()
        {
            modelObj.Start(BallsCount);
        }

        private void StopAction()
        {
            modelObj.Stop();
        }


        private void AddAction()
        {
            if (BallsCount < 15)
            {
                BallsCount++;
                OnPropertyChanged(nameof(BallsCount));
            }
        }

        private void SubtractAction()
        {
            if (BallsCount > 5)
            {
                BallsCount--;
                OnPropertyChanged(nameof(BallsCount));
            }
        }

    }


    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
