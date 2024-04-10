using DataLayer;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogicLayer
{
    public abstract class LogicAbstractApi
    {
        public abstract int width { get; }
        public abstract int height { get; }
        public abstract IList CreateBalls(int ballCount);
        public abstract void Start();
        public abstract void Stop();
        public static LogicAbstractApi CreateApi()
        {
            return new LogicApi();
        }
    }

    internal class LogicApi : LogicAbstractApi
    {
        public override int width { get; }

        public override int height { get; }
        private readonly DataAbstractApi dataLayer;

        public override IList CreateBalls(int ballCount)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }

}