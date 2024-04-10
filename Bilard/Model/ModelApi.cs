using LogicLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class AbstractModelApi
    {
        public abstract int balls { get; set; }
        public abstract void StartMoving();
        public abstract IList Start(int ballVal);
        public abstract void Stop();


        public static AbstractModelApi CreateApi()
        {
            return new ModelApi();
        }

    }

    public class ModelApi : AbstractModelApi
    {

        public override int balls { get; set; }

        private readonly LogicAbstractApi LogicLayer;


        public ModelApi()
        {
            LogicLayer = LogicAbstractApi.CreateApi();
        }

        public ModelApi(int ballVal)
        {
            LogicLayer = LogicAbstractApi.CreateApi();
            balls = ballVal;
        }

        public override void StartMoving()
        {
            LogicLayer.Start();
        }


        public override void Stop()
        {
            LogicLayer.Stop();
        }

        public override IList Start(int ballVal) => LogicLayer.CreateBalls(ballVal);
    }
}
