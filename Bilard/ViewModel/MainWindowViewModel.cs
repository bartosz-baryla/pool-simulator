using Model;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AbstractModelApi modelObj;
        private int ballsCount = 5;
        private IList balls;

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

        public IList Balls
        {
            get => balls;
            set
            {
                if (value.Equals(balls))
                {
                    return;
                }

                balls = value;
                OnPropertyChanged();
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
