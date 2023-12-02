using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TanksGame
{
    abstract class MovingObject
    {
        protected PictureBox picturebox;
        protected int speed;
        public Strategy direction;
        public readonly string name;
        protected bool die;
        public Rectangle Bounds
        {
            get { return picturebox.Bounds; }
        }
        public MovingObject(PictureBox px,Strategy direction,string name)
        {
            die = false;
            this.name = name;
            picturebox = px;
            this.direction = direction;
            speed = 0;
        }
        public virtual void MoveOneStep(Rectangle intersect)
        {
            if (intersect.IsEmpty)
            {
                picturebox.Location = GetLocationAfterOneStep(speed);
                return;
            }
            for (int i = 0; i < 5 && picturebox.Bounds.IntersectsWith(intersect)==false ; i++)
            {
                Point p = picturebox.Location; 
                picturebox.Location = GetLocationAfterOneStep(1);
                if (picturebox.Bounds.IntersectsWith(intersect))
                {
                    picturebox.Location = p;
                    break;
                }
            }
        }
        public virtual void MoveOneStep()
        {
            if (direction == Strategy.none) return;
            picturebox.Location = GetLocationAfterOneStep(speed);
        }
        public Rectangle SpeculateOneStep()
        {
            return new Rectangle(GetLocationAfterOneStep(speed), picturebox.Bounds.Size);
        }
        public Strategy GetDiffrentFrom(Control p)
        {
            if (!(p is PictureBox) || ((PictureBox)p).Image.Tag == null) return Strategy.none;
            Strategy other = (Strategy)((PictureBox)p).Image.Tag;
            Strategy me = (Strategy)picturebox.Image.Tag;
            int x = me - other;
            if (x < 0) x += 4;
            return (Strategy)x;
        }
        private Point GetLocationAfterOneStep(int speed)
        {
            if (direction == Strategy.right) return new Point(picturebox.Location.X + speed, picturebox.Location.Y);
            else if (direction == Strategy.down) return new Point(picturebox.Location.X, picturebox.Location.Y + speed);
            else if (direction == Strategy.left) return new Point(picturebox.Location.X - speed, picturebox.Location.Y);
            else if (direction == Strategy.up) return new Point(picturebox.Location.X, picturebox.Location.Y - speed);
            else return picturebox.Location;
        }
        public abstract bool IsDoneMoving();
        public Point GetCentral()
        {
            Point p = new Point();
            p.X = picturebox.Location.X + picturebox.Width / 2;
            p.Y = picturebox.Location.Y + picturebox.Height / 2;
            return p;
        }        
        public virtual void RemoveHimself(Control.ControlCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i] == picturebox) { collection.RemoveAt(i); picturebox.Dispose(); return; }
            }
            picturebox.Dispose();
        }
        public Strategy GetDirection() { return (Strategy)picturebox.Image.Tag; }
    }
}
