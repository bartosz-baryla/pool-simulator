using Model;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AbstractModelApi modelObj;
        private int ballsCount = 5;
        private ObservableCollection<IModelBall> balls; // ObservableCollection<> piłek każda piłka subskrybuje osobną piłkę warstwy niżej itd.
        private bool is_first = true;

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
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

        public ObservableCollection<IModelBall> Balls
        {
            get => balls;
            set
            {
                if (value == balls)
                {
                    return;
                }

                balls = value;
                OnPropertyChanged(nameof(Balls));
            }
        }

        public int BallsCount
        {
            get => ballsCount;
            set
            {
                ballsCount = value;
                OnPropertyChanged(nameof(BallsCount));
            }
        }

        private void StartAction()
        {
            if (is_first)
            {
                Balls = modelObj.FirstStart(BallsCount);
                is_first = false;
            }
            modelObj.Start();
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
            }
        }

        private void SubtractAction()
        {
            if (BallsCount > 5)
            {
                BallsCount--;
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
