using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TanksGame
{
    static class MyHelper
    {
        static SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\Documents\Visual Studio 2019\Projects\TanksGame\TanksGame\Database.mdf;Integrated Security=True");
        public static DataTable GetLocationsInformation()
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from Locations";
            command.ExecuteNonQuery();
            System.Data.DataTable table = new System.Data.DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter(command);
            adpt.Fill(table);
            connection.Close();
            return table;
        }
        public static void CommandOnDatabase(string user_command)
        {
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = user_command;
            command.ExecuteNonQuery();
            connection.Close();
        }
        public static PictureBox CreateFire(Tank t)
        {
            PictureBox px = new PictureBox();
            px.Image = Properties.Resources.Fire_fixed;
            px.Image.Tag = t.GetDirection();
            px.Location = t.GetPlaceForFire();
            px.Name = "Fire" + (Fire.count + 1);
            px.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            px.Size = new Size(32, 13);
            if (!Form1.realplaying) px.BorderStyle = BorderStyle.FixedSingle;
            px.TabStop = false;
            px.Tag = "fire";
            return px;
        }
        public static PictureBox CreateTank(Point location, string type)
        {
            PictureBox tank_img = new PictureBox();
            if (type == "HumanTank") tank_img.Image = Properties.Resources.Tank_fixed;
            else tank_img.Image = Properties.Resources.EnemyTank_fixed;
            tank_img.Image.Tag = Strategy.right;
            tank_img.Location = location;
            tank_img.Name = type + (Tank.count + 1);
            tank_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            tank_img.TabStop = false;
            if (!Form1.realplaying) tank_img.BorderStyle = BorderStyle.FixedSingle;
            tank_img.Tag = type.ToLower();
            return tank_img;
        }
        public static PictureBox CreateBlock(Point location)
        {
            PictureBox px = new PictureBox();
            px.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            px.Image = global::TanksGame.Properties.Resources.Block_fixed;
            px.Location = location;
            px.Name = "Block";
            px.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            px.TabStop = false;
            px.Tag = "block";
            return px;
        }
        public static int GetDiffrentAngle(RotateFlipType from, RotateFlipType to)
        {
            int angle1, angle2;
            if (from == RotateFlipType.RotateNoneFlipNone) angle1 = 0;
            else if (from == RotateFlipType.Rotate90FlipNone) angle1 = 90;
            else if (from == RotateFlipType.Rotate180FlipNone) angle1 = 180;
            else angle1 = 270;
            if (to == RotateFlipType.RotateNoneFlipNone) angle2 = 0;
            else if (to == RotateFlipType.Rotate90FlipNone) angle2 = 90;
            else if (to == RotateFlipType.Rotate180FlipNone) angle2 = 180;
            else angle2 = 270;
            return angle1 - angle2;
        }
        public static int GetDiffrentAngle(Strategy from, RotateFlipType to)
        {
            switch ((int)from)
            {
                case 0: return GetDiffrentAngle(RotateFlipType.RotateNoneFlipNone, to);
                case 1: return GetDiffrentAngle(RotateFlipType.Rotate90FlipNone, to);
                case 2: return GetDiffrentAngle(RotateFlipType.Rotate180FlipNone, to);
                case 3: return GetDiffrentAngle(RotateFlipType.Rotate270FlipNone, to);
                default: throw new Exception("get diffrent angle is wrong");
            }
        }
        public static Point GetCenter(Rectangle c)
        {
            return new Point(c.Location.X + c.Width / 2, c.Location.Y + c.Height / 2);
        }
        public static Strategy WhereMeet(Rectangle me,Rectangle damaged)
        {
            if (!me.IntersectsWith(damaged)) return Strategy.none;
            // all wrong, write again
            int[] difs = new int[] { me.Right- damaged.Left, me.Bottom - damaged.Top, damaged.Right - me.Left, damaged.Bottom - me.Top  };
            int size = me.Width / 2 + damaged.Width / 2;
            bool[] intersect = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                intersect[i] = difs[i] < size;
            }
            int min=difs[0], minindx = 0;
            for (int i = 1; i < 4; i++)
            {
                if(min>difs[i])
                {
                    min = difs[i];
                    minindx = i;
                }
            }
            return (Strategy)minindx;
        }
        public static double CalcDistance(Rectangle c1, Rectangle c2)
        {
            if (c1.IntersectsWith(c2)) return -1;
            if (c1.Top < c2.Bottom && c1.Bottom > c2.Top)
                return Math.Max(c1.Left - c2.Right, c2.Left - c1.Right);
            if (c1.Left < c2.Right && c1.Right > c2.Left)
                return Math.Max(c1.Top - c2.Bottom, c2.Top - c1.Bottom);
            Point p1 = GetCenter(c1), p2 = GetCenter(c2);
            double m = -1 * (p1.Y - p2.Y) / (double)(p1.X - p2.X);
            if (m > 0)
            {
                if (p1.X < p2.X) return CalcDistance(new Point(c1.Right, c1.Top), new Point(c2.Left, c2.Bottom));
                else return CalcDistance(new Point(c2.Right, c2.Top), new Point(c1.Left, c1.Bottom));
            }
            else
            {
                if (p1.X < p2.X) return CalcDistance(new Point(c1.Right, c1.Bottom), new Point(c2.Left, c2.Top));
                else return CalcDistance(new Point(c2.Right, c2.Bottom), new Point(c1.Left, c1.Top));
            }
        }
        public static double CalcDistance(Rectangle r, Point p)
        {
            if (r.Contains(p)) return 0;
            Rectangle p_rect = Rectangle.FromLTRB(p.X, p.Y, p.X, p.Y);
            return CalcDistance(r, p_rect);
        }
        public static double CalcDistance(Point p1,Point p2)
        {
            double s = Math.Pow((p1.X - p2.X), 2) + Math.Pow(p1.Y - p2.Y, 2);
            return Math.Sqrt(s);
        }
        public static Strategy Convert(RotateFlipType rotateFlipType)
        {
            switch (rotateFlipType)
            {
                case RotateFlipType.RotateNoneFlipNone: return Strategy.right;
                case RotateFlipType.Rotate90FlipNone: return Strategy.down;
                case RotateFlipType.Rotate180FlipNone: return Strategy.left;
                case RotateFlipType.Rotate270FlipNone: return Strategy.up;
                default: return Strategy.none;
            }
        }
        public static RotateFlipType Convert(Strategy s)
        {
            switch ((int)s)
            {
                case 0: return RotateFlipType.RotateNoneFlipNone;
                case 1: return RotateFlipType.Rotate90FlipNone;
                case 2: return RotateFlipType.Rotate180FlipNone;
                case 3: return RotateFlipType.Rotate270FlipNone;
                default: return RotateFlipType.RotateNoneFlipX;
            }
        }
        public static int CorrectingDirection(int tocorrect) { if (tocorrect > 3) tocorrect -= 4; return tocorrect; }
        public static void RotateImage(PictureBox px, Strategy direction)
        {
            Bitmap temp = (Bitmap)px.Image;
            temp.RotateFlip(Convert(direction));
            //if(temp.Tag==RotateFlipType.RotateNoneFlipNone)
            px.Image = temp;
            if (direction == Strategy.left || direction == Strategy.right) return;
            px.Size = new Size(px.Height, px.Width);
        }
        public static Bitmap RotateImage(Bitmap bmp, float angle)
        {
            float height = bmp.Height;
            float width = bmp.Width;
            int hypotenuse = System.Convert.ToInt32(System.Math.Floor(Math.Sqrt(height * height + width * width)));
            Bitmap rotatedImage = new Bitmap(hypotenuse, hypotenuse);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform((float)rotatedImage.Width / 2, (float)rotatedImage.Height / 2); //set the rotation point as the center into the matrix
                g.RotateTransform(angle); //rotate
                g.TranslateTransform(-(float)rotatedImage.Width / 2, -(float)rotatedImage.Height / 2); //restore rotation point into the matrix
                g.DrawImage(bmp, (hypotenuse - width) / 2, (hypotenuse - height) / 2, width, height);
            }
            return rotatedImage;
        }
    }
}
