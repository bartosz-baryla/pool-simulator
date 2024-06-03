
namespace DataLayer
{
    public abstract class IVector
    {
        public abstract double X { get; }
        public abstract double Y { get; }

        public static IVector CreateVector(double x, double y)
        {
            return new Vector(x, y);
        }
    }

    internal class Vector : IVector
    {
        private readonly double x;
        private readonly double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override double X => this.x;
        public override double Y => this.y;
    }

}
