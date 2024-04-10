using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal class Ball : IBall
    {
        public double x0 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double y0 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double x1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double y1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int r => throw new NotImplementedException();

        public double m => throw new NotImplementedException();

        public int Identifier => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Move(double time, ConcurrentQueue<IBall> queue)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public interface IBall : INotifyPropertyChanged
    {
        double x0 { get; set; }
        double y0 { get; set; }
        double x1 { get; set; }
        double y1 { get; set; }
        int r { get; }
        double m { get; } // masa
        int Identifier { get; }

        void Move(double time, ConcurrentQueue<IBall> queue);
        void Stop();
    }
}
