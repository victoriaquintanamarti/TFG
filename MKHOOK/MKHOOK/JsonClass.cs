using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;
using System.Dynamic;

namespace MKHOOK
{
    public class JsonClass
    {
        public string TimeActivityPerSecond { get; set; }
        public string MouseSamplePerSecond { get; set; }
        public List<ActivityStats> Activity { get; set; }

    }

    public class ActivityStats
    {
        public TimeStats Time { get; set; }
        public KeyboardStats Keyboard { get; set; }
        public MouseStats Mouse { get; set; }
    }

    public class TimeStats
    {
        public string TimeElapsed { get; set; }

    }

    public class KeyboardStats
    {
        public int PressedKeys { get; set; }
        public int ScapeKey { get; set; }
        public int TwoPressedKeys { get; set; }
    }

    public class MouseStats
    {
        public int MouseClicks { get; set; }
        public double EuclideanDistance { get; set; }
        public int MouseWheel { get; set; }

    }
}
