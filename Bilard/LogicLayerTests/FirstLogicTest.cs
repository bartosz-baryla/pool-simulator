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

            double cx1 = 10;
            double cy1 = 0;
            double cx2 = 0;
            double cy2 = 0;
            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;

            double result = lLayer.DistanceBetweenBalls(cx1, cy1, cx2, cy2, x1, y1, x2, y2);

            Assert.Equal(10, result);

        }
    }
}
