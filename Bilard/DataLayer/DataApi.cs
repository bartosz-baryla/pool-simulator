using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;

namespace DataLayer
{
    public abstract class DataAbstractApi
    {
        public abstract int GetCount { get; }
        public abstract IList CreateBalls(int number);
        public abstract IBall GetBall(int id);
        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
    internal class DataApi : DataAbstractApi
    {
        private ObservableCollection<IBall> balls { get; }
        private readonly Mutex mutex = new Mutex();


        public DataApi()
        {
            balls = new ObservableCollection<IBall>();
        }

        public override IList CreateBalls(int number)
        {
            Random random = new Random();
            if (number > 0)
            {
                int ballsCount = balls.Count;
                for (int i = 0; i < number; i++)
                {
                    mutex.WaitOne(); // Sekcja krytyczna - tylko jedna piłka jest tworzona na raz (zamek)
                    int xc = random.Next(10, 800 - 10);
                    int yc = random.Next(10, 400 - 10);// bo przyjmujemy r = 10
                    int xn = random.Next(-3, 3);
                    int yn = random.Next(-3, 3);

                    Ball ball = new Ball(i + ballsCount, xc, yc, xn, yn);
                    balls.Add(ball);
                    mutex.ReleaseMutex();
                }
            }
            return balls;
        }

        public override int GetCount { get => balls.Count; }

        public override IBall GetBall(int index)
        {
            return balls[index];
        }
    }
}
