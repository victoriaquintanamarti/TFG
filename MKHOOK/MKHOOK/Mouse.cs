using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKHOOK
{
    public class Mouse
    {
        private int clicks;
        private int mouseWheel;
        private int prevMouseWheel = 0;
        private int changeMouseWheel = 0;
        private int moveMouse;
        private double sumDistances = 0;
        private int previousX = 0;
        private int previousY = 0;
        private int x = 0;
        private int y = 0;

        public Mouse()
        {
            clicks = 0;
            mouseWheel = 1;
            moveMouse = 0;
        }

        public void setClicks(int clicks)
        {
            this.clicks = clicks;
        }
        public void setMouseWheel(int mouseWheel)
        {
            if ((mouseWheel <= 0 && prevMouseWheel <= 0) || (mouseWheel >= 0 && prevMouseWheel >= 0)) {}
            else
            {
                changeMouseWheel++;
            }
            this.mouseWheel = changeMouseWheel;
            prevMouseWheel = mouseWheel;
        }

        public void setChangeMouseWheel(int changeMouseWheel)
        {
            this.changeMouseWheel = changeMouseWheel;
        }

        public void setMoveMouse(int moveMouse)
        {
            this.moveMouse = moveMouse;
        }
        public int getClicks()
        {
            return clicks;
        }
        public int getMouseWheel()
        {
            return changeMouseWheel;
        }
        public int getMoveMouse()
        {
            return moveMouse;
        }
        public Double getSumDistances()
        {
            return sumDistances;
        }
        public void setSumDistances(Double sumDistances)
        {
            this.sumDistances = sumDistances;
        }
        public void setPositionMouse(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public int getPreviousX()
        {
            return previousX;
        }
        public int getPreviousY()
        {
            return previousY;
        }
        public void setPreviousPositionMouse(int previousX, int previousY)
        {
            this.previousX = previousX;
            this.previousY = previousY;
        }
        public void euclideanDistance()
        {
            sumDistances += Math.Sqrt(Math.Pow((x - previousX), 2) + Math.Pow((y - previousY), 2));

        }
        public void stats()
        {
            Console.WriteLine("Mouse clicks: " + clicks);
            Console.WriteLine("Euclidean distance: " + sumDistances);
            Console.WriteLine("Wheel: " + getMouseWheel());
        }
    }
}
