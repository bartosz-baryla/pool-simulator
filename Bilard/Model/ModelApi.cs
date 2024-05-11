using LogicLayer;
using System.Collections;

namespace Model
{
    public abstract class AbstractModelApi
    {
        public abstract void Start();
        public abstract void Stop();
        public abstract IList FirstStart(int number);


        public static AbstractModelApi CreateApi()
        {
            return new ModelApi();
        }
    }
    internal class ModelApi : AbstractModelApi
    {
        private readonly LogicAbstractApi LogicLayer;

        public ModelApi()
        {
            LogicLayer = LogicAbstractApi.CreateApi();
        }

        public override IList FirstStart(int number) => LogicLayer.CreateBalls(number);

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
