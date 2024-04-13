using System.Collections.Generic;

namespace DataLayer
{
    public abstract class DataAbstractApi
    {
        public abstract List<IBall> balls { get; }
        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
    internal class DataApi : DataAbstractApi
    {
        public override List<IBall> balls { get; }
    }
}
