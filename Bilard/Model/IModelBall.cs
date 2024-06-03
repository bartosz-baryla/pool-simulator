using LogicLayer;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public interface IModelBall : INotifyPropertyChanged, IObserver<ILogicBall>
    {
        double X { get; set; }
        double Y { get; set; }
    }

    internal class ModelBall : IModelBall
    {
        private double x; 
        private double y;
        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double X
        {
            get => x;
            set
            {
                if (value.Equals(x))
                {
                    return;
                }

                x = value;
                RaisePropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get => y;
            set
            {
                if (value.Equals(y))
                {
                    return;
                }

                y = value;
                RaisePropertyChanged(nameof(Y));
            }
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(ILogicBall value)
        {
            X = value.X * 1.0; 
            Y = value.Y * 1.0;
        }

    }
}
