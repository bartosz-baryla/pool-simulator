using System;
using System.Collections;
using System.Collections.Generic;
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
        private List<IBall> balls { get; }
        private readonly Mutex mutex = new Mutex();
        private readonly int height = 400;
        private readonly int width = 800;


        public DataApi()
        {
            balls = new List<IBall>();
        }

        public override IList CreateBalls(int number)
        {
            Random random = new Random();

            for (int i = 0; i < number; i++)
            {
                mutex.WaitOne(); // Sekcja krytyczna - tylko jedna piłka jest tworzona na raz (zamek)
                int x = random.Next(10, width - 20);
                int y = random.Next(10, height - 20);
                int vx = random.Next(50, 100);
                int vy = random.Next(50, 100);

                Ball ball = new Ball(i + 1, x, y, vx, vy);
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
