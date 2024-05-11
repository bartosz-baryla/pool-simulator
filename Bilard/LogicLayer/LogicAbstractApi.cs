using DataLayer;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System;

namespace LogicLayer
{
    public abstract class LogicAbstractApi
    {
        public abstract int GetCount { get; }
        public abstract IList CreateBalls(int count);
        public abstract IBall GetBall(int id);
        public abstract void BallPositionChanged(object sender, PropertyChangedEventArgs args);
        public abstract double DistanceBetweenBalls(double cx1, double cy1, double cx2, double cy2, double x1, double y1, double x2, double y2);
        public abstract void Start();
        public abstract void Stop();
        public static LogicAbstractApi CreateApi()
        {
            return new LogicApi();
        }
    }

    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi dataLayer;
        private readonly Mutex mutex = new Mutex();

        public LogicApi()
        {
            dataLayer = DataAbstractApi.CreateApi();
        }

        public override IList CreateBalls(int number)
        {
            IList list = dataLayer.CreateBalls(number);
            for (int i = 0; i < number; i++)
            {
                dataLayer.GetBall(i).PropertyChanged += BallPositionChanged;
            }
            return list;
        }
        public override void Start()
        {
            for (int i = 0; i < dataLayer.BallsCount; i++)
            {
                dataLayer.GetBall(i).MakeThread(15);

            }
        }

        public override void Stop()
        {
            for (int i = 0; i < dataLayer.BallsCount; i++)
            {
                dataLayer.GetBall(i).Stop();

            }
        }

        public override IBall GetBall(int index)
        {
            return dataLayer.GetBall(index);
        }

        public override int GetCount { get => dataLayer.BallsCount; }

        public override void BallPositionChanged(object sender, PropertyChangedEventArgs args)
        {
            IBall ball = (IBall)sender;
            mutex.WaitOne();        // Sekcja krytyczna - sprawdzanie kolizji (zamek)
            CheckCollisions(ball); 
            mutex.ReleaseMutex();
        }

        void CheckCollisions(IBall ball)
        {
            //Sprawdzam zderzenia ze ścianą:
            int rightWall = 800 - 10 * 2;
            int leftWall = 0;
            int topWall = 0;
            int bottomWall = 400 - 10 * 2;


            if (ball.current_Y <= topWall) //Sprawdzam górną ścianę
            {
                ball.current_Y = -ball.current_Y;
                ball.next_Y = -ball.next_Y;
            }

            if (ball.current_Y >= bottomWall) //Sprawdzam dolną ścianę
            {
                ball.current_Y = bottomWall - (ball.current_Y - bottomWall);
                ball.next_Y = -ball.next_Y;
            }

            if (ball.current_X <= leftWall) // Sprawdzam lewą ścianę
            {
                ball.current_X = -ball.current_X;
                ball.next_X = -ball.next_X;
            }

            if (ball.current_X >= rightWall) //sprawdzam prawą ścianę
            {
                ball.current_X = rightWall - (ball.current_X - rightWall);
                ball.next_X = -ball.next_X;
            }

            // Sprawdzamy, czy nie dochodzi do kolizji między piłkami, zmieniamy pozycje kuli
            for (int i = 0; i < dataLayer.BallsCount; i++)
            {
                IBall secondBall = dataLayer.GetBall(i);

                // Sprawdzam kolizję dwóch różnych piłek
                if (ball.ID != secondBall.ID && DistanceBetweenBalls(ball.current_X, ball.current_Y, secondBall.current_X, secondBall.current_Y, ball.next_X, ball.next_Y, secondBall.next_X, secondBall.next_Y) <= (2 * 10)) // bo 10 to promień
                {
                    double v1x = ball.next_X;
                    double v1y = ball.next_Y;
                    double v2x = secondBall.next_X;
                    double v2y = secondBall.next_Y;

                    //zakładamy że masa każdej kuli wynosi 1
                    int mass = 1;
                    double u1x = (v1x * (1 - mass) + 2 * mass * v2x) / (mass*2);
                    double u1y = (v1y * (1 - mass) + 2 * mass * v2y) / (mass * 2);
                    double u2x = (v2x * (1 - mass) + 2 * mass * v1x) / (mass * 2);
                    double u2y = (v2y * (1 - mass) + 2 * mass * v1y) / (mass * 2);


                    ball.next_X = u1x;
                    ball.next_Y = u1y;
                    secondBall.next_X = u2x;
                    secondBall.next_Y = u2y;
                }
            }
        }

        public override double DistanceBetweenBalls(double cx1, double cy1, double cx2, double cy2, double x1, double y1, double x2, double y2)
        {
            double dx1 = cx1 + 10 + x1;
            double dy1 = cy1 + 10 + y1;
            double dx2 = cx2 + 10 + x2;
            double dy2 = cy2 + 10 + y2;

            return Math.Sqrt(((dx1 - dx2) * (dx1 - dx2)) + ((dy1 - dy2) * (dy1 - dy2)));
        }

    }
}
