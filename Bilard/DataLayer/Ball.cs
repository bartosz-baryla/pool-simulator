using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DataLayer
{
    public interface IBall : INotifyPropertyChanged
    {
        void Move();
        void Stop();
        void MakeThread(int period);
        double current_Y { get; set; }
        double next_Y { get; set; }
        double current_X { get; set; }
        double next_X { get; set; }
        int ID { get; set; }

    }

    internal class Ball : IBall // zakładamy że każda kula ma masę 1 i promień 10.
    {
        private double current_y;
        private double next_y;
        private double current_x;
        private double next_x;
        private int id;

        private bool stop = false;
        private Thread thread;

        public Ball(int id, double current_X, double current_Y, double next_X, double next_Y)
        {
            this.id = id;
            this.current_x = current_X;
            this.current_y = current_Y;
            this.next_x = next_X;
            this.next_y = next_Y;
        }

        public int ID
        {
            get => id;
            set
            {
                id = value;
            }
        }

        public double current_X
        {
            get => current_x;
            set
            {
                if (value.Equals(current_x))
                {
                    return;
                }

                current_x = value;
                RaisePropertyChanged(nameof(current_X));
            }
        }

        public double current_Y
        {
            get => current_y;
            set
            {
                if (value.Equals(current_y))
                {
                    return;
                }

                current_y = value;
                RaisePropertyChanged(nameof(current_Y));
            }
        }

        public double next_X
        {
            get => next_x;
            set
            {
                if (value.Equals(next_x))
                {
                    return;
                }

                next_x = value;
            }
        }
        public double next_Y
        {
            get => next_y;
            set
            {
                if (value.Equals(next_y))
                {
                    return;
                }

                next_y = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Move()
        {
            current_X += next_X;
            current_Y += next_Y;
        }

        public void MakeThread(int period)
        {
            stop = false;
            thread = new Thread(() => Run(period));
            thread.Start();
        }

        private void Run(int period)
        {
            while (!stop)
            {
                if (!stop)
                {
                    Move();
                }

                Thread.Sleep(period);
            }
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
