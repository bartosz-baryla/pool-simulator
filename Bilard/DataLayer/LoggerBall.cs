using System;

namespace DataLayer
{
    public class LoggerBall
    {
        private readonly int id;
        private readonly double x;
        private readonly double y;
        private readonly double vx;
        private readonly double vy;
        private readonly DateTime time;

        public LoggerBall(int id, double x, double y, double vx, double vy, System.DateTime time)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
            this.time = time;
        }

        public int ID => id;
        public DateTime Time => time;
        public double X => x;
        public double Y => y;
        public double VX => vx;
        public double VY => vy;
    }
}
