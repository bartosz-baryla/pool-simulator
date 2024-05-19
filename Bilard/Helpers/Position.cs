using System.ComponentModel;

namespace Helpers
{
    public class Position
    {
        private readonly double x;
        private readonly double y;

        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X => this.x;
        public double Y => this.y;
    }
}
