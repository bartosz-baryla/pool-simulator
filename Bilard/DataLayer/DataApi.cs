using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer
{
    public abstract class DataAbstractApi
    {
        public abstract int BallsCount { get; }
        public abstract IList CreateBalls(int number);
        public abstract IBall GetBall(int id);
        public abstract Task CreateLoggingTask(BoundedConcurrentQueue<LoggerBall> logQueue);
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
        private object locker = new object();


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

        public override Task CreateLoggingTask(BoundedConcurrentQueue<LoggerBall> queue)
        {
            return CallLogger(queue);
        }

        internal async Task CallLogger(BoundedConcurrentQueue<LoggerBall> queue) // Logger ball jest immutable
        {
            while (true)
            {
                if (queue.TryDequeue(out LoggerBall logObject)) //Jeśli kolejka jest pusta, TryDequeue zwróci false
                {
                    if (logObject != null)
                    {
                        string data = JsonSerializer.Serialize(logObject);
                        string log = "\n" + data + "\n";
                        string directory = Directory.GetCurrentDirectory();
                        string filePath = Path.Combine(directory, "Loggs.json");

                        lock (locker)
                        {
                            File.AppendAllText(filePath, log);
                        }
                    }
                } 
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1)); // wprowadzam w stan suspended jeżeli kolejka jest pusta
                }

            }
        }

        public override int BallsCount { get => balls.Count; }

        public override IBall GetBall(int id)
        {
            return balls[id];
        }
    }
}
