using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TanksGame
{
    class Fire : MovingObject
    {
        public static int count = 0;
        public Fire(PictureBox px,Strategy direction) : base(px, direction, "Fire" + (count+1))
        {
            MyHelper.RotateImage(px,direction);
            count += 1;
            speed = 10;
        }
        public override bool IsDoneMoving()
        {
            return die;
        }
    }
}
