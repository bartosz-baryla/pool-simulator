using LogicLayer;
using Xunit;

namespace LogicLayerTests
{
    public class FirstLogicTest
    {
        [Fact]
        public void GetBalls()
        {
            LogicAbstractApi lLayer;
            lLayer = LogicAbstractApi.CreateApi();

            double x1 = 10;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;

            double result = lLayer.DistanceBetweenBalls(x1, y1, x2, y2);

            Assert.Equal(10, result);

        }
    }
}
