using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Model
{
    public class ModelApi
    {
        public int width { get; }
        public int height { get; }
        public int balls { get; set; }

        public ModelApi(int ballsCount)
        {
            balls = ballsCount;
        }
    }
}
