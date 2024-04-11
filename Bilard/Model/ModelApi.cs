using LogicLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Model
{
    public abstract class AbstractModelApi
    {
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ballsCollection { get; }
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
        private LogicAbstractApi LogicLayer;

        /** Lista reprezentacji kul. */
        public override List<Ellipse> ballsCollection { get; }

        /** Reprezentacja planszy, po której poruszają się kule.*/
        public override Canvas Canvas { get; set; }

        public ModelApi()
        {
            LogicLayer = LogicAbstractApi.CreateApi();
            ballsCollection = new List<Ellipse>();
            Canvas = new Canvas();
            Canvas.Width = 330;
            Canvas.Height = 780;
            //LogicLayer.Update += (sender, args) => Move();
        }


        public override void CreateBalls(int ballsCount)
        {
            Random random = new Random();
            LogicLayer.CreateBalls(ballsCount);

            for (int i = LogicLayer.GetCount - ballsCount; i < LogicLayer.GetCount; i++)
            {
                Ellipse ball = new Ellipse
                {
                    Width = LogicLayer.GetR(i) * 2,
                    Height = LogicLayer.GetR(i) * 2,
                    Fill = Brushes.Red,
                };
                Canvas.SetLeft(ball, LogicLayer.GetX(i));
                Canvas.SetTop(ball, LogicLayer.GetY(i));

                ballsCollection.Add(ball);
                Canvas.Children.Add(ball);
            }
        }

        public override void Move()
        {
            /* for (int i = 0; i < LogicLayer.GetCount; i++)
             {
                 Canvas.SetLeft(ellipseCollection[i], LogicLayer.GetX(i));
                 Canvas.SetTop(ellipseCollection[i], LogicLayer.GetY(i));
             }
             for (int i = LogicLayer.balls.Count; i < ellipseCollection.Count; i++)
             {
                 Canvas.Children.Remove(ellipseCollection[ellipseCollection.Count - 1]);
                 ellipseCollection.Remove(ellipseCollection[ellipseCollection.Count - 1]);
             }*/
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
