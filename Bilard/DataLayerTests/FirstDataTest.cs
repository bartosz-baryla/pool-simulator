using DataLayer;
using Xunit;

namespace DataLayerTests
{
    public class FirstDataTest
    {
        [Fact]
        public void AddBall()
        {

            DataAbstractApi dataLayer;
            dataLayer = DataAbstractApi.CreateApi();
            dataLayer.CreateBalls(2);

            IBall ball = dataLayer.GetBall(0);
            IBall ball2 = dataLayer.GetBall(1);

            Assert.NotEqual(ball, ball2);


        }
    }
}
