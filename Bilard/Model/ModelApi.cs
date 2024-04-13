using LogicLayer;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Model
{
    public abstract class AbstractModelApi
    {
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ballsList { get; }
        public abstract void CreateBalls(int ballVal);
        public abstract void Move();
        public abstract void Start();
        public abstract void Stop();


        public static AbstractModelApi CreateApi()
        {
            return new ModelApi();
        }
    }
    internal class ModelApi : AbstractModelApi
    {
        /** Odwołanie do warstwy Logiki. */
        private readonly LogicAbstractApi LogicLayer;

        /** Lista reprezentacji kul. */
        public override List<Ellipse> ballsList { get; }

        /** Reprezentacja planszy, po której poruszają się kule.*/
        public override Canvas Canvas { get; set; }

        public ModelApi()
        {
            LogicLayer = LogicAbstractApi.CreateApi();
            ballsList = new List<Ellipse>();
            Canvas = new Canvas();
            Canvas.Width = 400;
            Canvas.Height = 800;
            LogicLayer.Update += (sender, args) => Move();
        }


        public override void CreateBalls(int ballsCount)
        {
            LogicLayer.CreateBalls(ballsCount);

            for (int i = 0; i < LogicLayer.GetCount; i++)
            {
                Ellipse ball = new Ellipse
                {
                    Width = LogicLayer.GetR(i) * 2,
                    Height = LogicLayer.GetR(i) * 2,
                    Fill = Brushes.Red,
                };
                Canvas.SetLeft(ball, LogicLayer.GetX(i));
                Canvas.SetTop(ball, LogicLayer.GetY(i));

                ballsList.Add(ball);
                Canvas.Children.Add(ball);
            }
        }

        public override void Move()
        {
             for (int i = 0; i < LogicLayer.GetCount; i++)
             {
                 Canvas.SetLeft(ballsList[i], LogicLayer.GetX(i));
                 Canvas.SetTop(ballsList[i], LogicLayer.GetY(i));
             }
             for (int i = LogicLayer.GetCount; i < ballsList.Count; i++)
             {
                 Canvas.Children.Remove(ballsList[ballsList.Count - 1]);
                ballsList.Remove(ballsList[ballsList.Count - 1]);
             }
        }

        public override void Start()
        {
            LogicLayer.Start();
        }

        public override void Stop()
        {
            LogicLayer.Stop();
        }
    }
}
