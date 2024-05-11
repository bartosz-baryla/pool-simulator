using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;

namespace DataLayer
{
    public abstract class DataAbstractApi
    {
        public abstract int BallsCount { get; }
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
        private readonly int height = 400;
        private readonly int width = 800;


        public DataApi()
        {
            balls = new ObservableCollection<IBall>(); // klasa dziedziczy po IList, ale dodaje funkcjonalność związaną z powiadamianiem o zmianach.
        }

        public override IList CreateBalls(int number)
        {
            Random random = new Random();

            for (int i = 0; i < number; i++)
            {
                mutex.WaitOne(); // Sekcja krytyczna - tylko jedna piłka jest tworzona na raz (zamek)
                int xc = random.Next(10, width - 10);
                int yc = random.Next(10, height - 10);// bo przyjmujemy r = 10
                int xn = 3;
                int yn = 3;

                Ball ball = new Ball(i + 1, xc, yc, xn, yn);
                balls.Add(ball);
                mutex.ReleaseMutex();
            }

            return balls;
        }

        public override int BallsCount { get => balls.Count; }

        public override IBall GetBall(int id)
        {
            return balls[id];
        }
    }
}
