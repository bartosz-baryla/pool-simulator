using Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        /** Odwołanie do warstwy Modelu w warstwie Prezentacji. */
        private readonly AbstractModelApi modelObj;
        /** Liczba kul. */
        private int ballsCount = 5;

        /** Metoda wywoływana po przyciśnięciu przycisku start. */
        public ICommand StartCommand { get; set; }

        /** Metoda wywoływana po przyciśnięciu przycisku stop. */
        public ICommand StopCommand { get; set; }

        /** Metoda wywoływana po przyciśnięciu przycisku '+1'. */
        public ICommand AddCommand { get; }

        /** Metoda wywoływana po przyciśnięciu przycisku '-1'. */
        public ICommand SubtractCommand { get; }

        private bool isFirstStart = true;

        /** Metoda służąca do tworzenia kul. */
        public ICommand CreateBalls { get; }


        public MainWindowViewModel()
        {
            modelObj = AbstractModelApi.CreateApi();
            StartCommand = new RelayCommand(StartAction);
            StopCommand = new RelayCommand(StopAction);
            AddCommand = new RelayCommand(AddAction);
            SubtractCommand = new RelayCommand(SubtractAction);
            CreateBalls = new RelayCommand(CreateBallsAction);
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
            if(isFirstStart)
            {
                modelObj.CreateBalls(ballsCount);
                isFirstStart = false;
            }
            modelObj.Start();
        }

        private void StopAction()
        {
            modelObj.Stop();
        }

        public Canvas Canvas
        {
            get => modelObj.Canvas;

        }


        private void CreateBallsAction()
        {
            modelObj.CreateBalls(BallsCount);
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
