using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public abstract class DataAbstractApi
    {
        public abstract int Width { get; }
        public abstract int Height { get; }

        public abstract IBall CreateBall(int number);
        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
    internal class DataApi : DataAbstractApi
    {
        public override int Width => throw new NotImplementedException();

        public override int Height => throw new NotImplementedException();

        public override IBall CreateBall(int number)
        {
            throw new NotImplementedException();
        }
    }
}
