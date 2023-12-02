using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace TanksGame
{
    class Tank : MovingObject
    {
        protected PictureBox toright;
        protected int energy;
        public static int count = 0;
        public Tank(PictureBox px):base(px,Strategy.none,("HumanTank"+(count+1)))
        {
            toright = new PictureBox();
            toright.Size = px.Size;
            toright.Image = new Bitmap(px.Image);
            speed = 5;
            energy = 0;
            count++;
            die = false;
        }
        protected Tank(PictureBox px,string name): base(px, Strategy.none, name)
        {
            toright = new PictureBox();
            toright.Size = px.Size;
            toright.Image = new Bitmap(px.Image);
            speed = 5;
            energy = 0;
            count++;
            die = false;
        }
        public bool IsDie() { return die; }
        public void FillEnergy() { energy++; }
        public bool Canfire() { return energy >= 30; }
        public void ResetEnergy() { energy = 0; }
        public override bool IsDoneMoving()
        {
            return die || direction==Strategy.none; 
        }
        public override void RemoveHimself(Control.ControlCollection collection)
        {
            if (die) base.RemoveHimself(collection);
            else BeacomeDead();
        }
        public virtual void ChangeDirectionTo(Strategy s) 
        {
            Strategy temp = direction;
            direction = s;
            if (temp == direction|| direction==Strategy.none) return;
            picturebox.Image = new Bitmap(toright.Image);
            //picturebox.Size = toright.Size;
            MyHelper.RotateImage(picturebox, direction);
            picturebox.Image.Tag = direction;
        }
        public Point GetPlaceForFire()
        {
            Point p = new Point();
            p = GetCentral();
            switch ((Strategy)picturebox.Image.Tag)
            {
                case Strategy.right:
                    {
                        p.X = p.X + 33;
                        p.Y = p.Y - 7;
                        break;
                    }
                case Strategy.down:
                    {
                        p.X = p.X - 8;
                        p.Y = p.Y + 33;
                        break;
                    }
                case Strategy.left:
                    {
                        p.X = p.X - 65;
                        p.Y = p.Y - 7;
                        break;
                    }
                case Strategy.up:
                    {
                        p.X = p.X - 6;
                        p.Y = p.Y - 64;
                        break;
                    }
            }
            return p;
        }
        public void BeacomeDead()
        {
            die = true;
            picturebox.Image = Properties.Resources.Expload_fixed;
            picturebox.Tag = "expload";
            speed = 0;
            energy = 0;
        }
        public void FixLocation(Rectangle damaged,Strategy d)
        {
            switch ((int)d)
            {
                case 0: picturebox.Location = new Point(damaged.Left - picturebox.Width, picturebox.Location.Y); break;
                case 1: picturebox.Location = new Point(picturebox.Location.X, damaged.Top - picturebox.Height); break;
                case 2: picturebox.Location = new Point(damaged.Right, picturebox.Location.Y); break;
                case 3: picturebox.Location = new Point(picturebox.Location.X, damaged.Bottom); break;
            }
        }
    }
}
