using LogicLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Model
{
    public abstract class AbstractModelApi
    {
        public abstract void Start();
        public abstract void Stop();
        public abstract ObservableCollection<IModelBall> FirstStart(int number);

        public static AbstractModelApi CreateApi()
        {
            return new ModelApi();
        }
    }
    internal class ModelApi : AbstractModelApi
    {
        private readonly LogicAbstractApi LogicLayer;
        ObservableCollection<IModelBall> modelBalls = new ObservableCollection<IModelBall>();

        public ModelApi()
        {
            LogicLayer = LogicAbstractApi.CreateApi();
        }

        public override ObservableCollection<IModelBall> FirstStart(int number)
        {
            IList<ILogicBall> logicBalls = LogicLayer.CreateBalls(number);
            for (int i = 0; i < number; i++)
            {
                ILogicBall ball = LogicLayer.GetLogicBall(i);
                ModelBall ball2 = new ModelBall();
                ball2.X = ball.P.X; 
                ball2.Y = ball.P.Y;
                ball.Subscribe(ball2);
                modelBalls.Add(ball2);
            }
            return modelBalls;
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
