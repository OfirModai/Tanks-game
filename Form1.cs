using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Media;

namespace TanksGame
{
    public partial class Form1 : Form
    {
        List<Tank> tanks;
        List<MovingObject> movingcurrently;
        int lastkey;
        private string state;
        public static bool realplaying=true;
        SoundPlayer s_shot, s_explosion, s_winning;
        DataTable dt = null;
        public Form1()
        {
            Constract();
        }
        private void Play(System.IO.Stream stream)
        {
            SoundPlayer s = new SoundPlayer(stream);
            s.Play();
        }
        private void Constract()
        {
            InitializeComponent();
            Form1_ClientSizeChanged(this, new EventArgs());
            //if playing
            if (realplaying)
            {
                Controls.Remove(testingTable);
                testingTable.Visible = false;
                rightBorder.BorderStyle = BorderStyle.None;
                leftBorder.BorderStyle = BorderStyle.None;
                topBorder.BorderStyle = BorderStyle.None;
                bottomBorder.BorderStyle = BorderStyle.None;
            }
            WindowState = FormWindowState.Maximized;
            s_shot = new SoundPlayer(TanksGame.Properties.Resources.ShotSound);
            s_explosion = new SoundPlayer(TanksGame.Properties.Resources.ExplosionSound);
            s_winning = new SoundPlayer(TanksGame.Properties.Resources.CheeringSound);
            state = "";
            lastkey = 0;
            dt = new DataTable();
            tanks = new List<Tank>();
            dt.Columns.Add("controls");
            dt.Columns.Add("movingcurrently");
            DataTable data = MyHelper.GetLocationsInformation();
            if (data.Rows.Count == 0)
            {
                state = "humanplayer";
                resetBtn.Text = "add human player or q for moving to AI players";
            }
            else for (int i = 0; i < data.Rows.Count; i++)
                {
                    string type = data.Rows[i][1].ToString().Trim();
                    Point location = new Point((int)data.Rows[i][2], (int)data.Rows[i][3]);
                    CreateObject(type, location);
                }
        }
        private void ResetForm()
        {
            Controls.Clear();
            Constract();
        }
        private void startBtn_MouseClick(object sender, MouseEventArgs e)
        {
            if (state == "ongame") return;
            state = "ongame";
            testingTable.ForeColor = Color.Black;
            movingcurrently = new List<MovingObject>();
            foreach (Tank t in tanks) movingcurrently.Add(t);
            if (tanks[0] is AiTank) KeyPreview = false;
            startBtn.Text = "";
            resetBtn.Text = "start over";
            UpdateDataTest();
        }
        private void resetBtn_MouseClick(object sender, MouseEventArgs e)
        {
            if (state == "ongame" || state == "game_ended")
            {
                //ResetForm();
                
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Application.Exit();
                
            }
            else
            {
                MyHelper.CommandOnDatabase("delete from Locations");
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Application.Exit();
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (state == "" || state == "ongame" || state == "game_ended") return;
            string type = null;
            int editor = 0;
            DataTable dt = MyHelper.GetLocationsInformation();
            if (state == "humanplayer")
            {
                type = "HumanTank";
                editor = 30;
            }
            else if (state == "aiplayer")
            {
                type = "AiTank";
                editor = 30;
            }
            else
            {
                type = "Block";
                editor = 17;
            }
            Point location = new Point(e.X - editor, e.Y - editor);
            location = CreateObject(type, location);
            if (location.IsEmpty) return;
            MyHelper.CommandOnDatabase(string.Format("insert into Locations values('{0}','{1}','{2}','{3}')", dt.Rows.Count, type, location.X, location.Y));
        }
        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            int widthUnit = ClientSize.Width / 20, heightUnit = ClientSize.Height / 20; 
            resetBtn.Size = new Size(widthUnit*10, heightUnit*2);
            startBtn.Size = new Size(widthUnit * 10, heightUnit * 2);
            startBtn.Location = new Point(widthUnit*10, 0);
            testingTable.Location = new Point(widthUnit*16, heightUnit*2);
            testingTable.Size = new Size(widthUnit*4,heightUnit*18);
            leftBorder.Location = new Point(0, heightUnit * 2);
            leftBorder.Size = new Size(widthUnit, heightUnit * 18);

            rightBorder.Size = new Size(widthUnit, heightUnit * 18);
            topBorder.Location = new Point(widthUnit, heightUnit * 2);
            bottomBorder.Location = new Point(widthUnit, heightUnit * 19);
            if (realplaying)
            {
                rightBorder.Location = new Point(widthUnit * 19, heightUnit * 2);
                topBorder.Size = new Size(widthUnit * 18, heightUnit);
                bottomBorder.Size = new Size(widthUnit * 18, heightUnit);
            }
            else
            {
                rightBorder.Location = new Point(widthUnit * 15, heightUnit * 2);
                topBorder.Size = new Size(widthUnit * 14, heightUnit);
                bottomBorder.Size = new Size(widthUnit * 14, heightUnit);
            }
            //playerProgressBar.Location = new Point(0, ClientSize.Height / 10);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Y)
                lastkey = 'y';
            if (state != "ongame" || tanks[0] is AiTank) return;
            if (e.KeyValue == 68) tanks[0].ChangeDirectionTo(Strategy.right);
            else if (e.KeyValue == 83) tanks[0].ChangeDirectionTo(Strategy.down);
            else if (e.KeyValue == 65) tanks[0].ChangeDirectionTo(Strategy.left);
            else if (e.KeyValue == 87) tanks[0].ChangeDirectionTo(Strategy.up);
            if (e.KeyValue != 32) lastkey = e.KeyValue;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (state != "ongame" && e.KeyValue == 81)
            {
                if (state == "humanplayer")
                {
                    resetBtn.Text = "add enemy or press q for moving to blocks";
                    state = "aiplayer";
                }
                else if (state == "aiplayer")
                {
                    resetBtn.Text = "add block or press start for starting the game";
                    state = "blocks";
                }
            }
            if (state != "ongame"|| tanks[0] is AiTank) return;
            if (e.KeyValue == 32)
            {
                if (tanks[0].Canfire())
                {
                    PictureBox fire_img = MyHelper.CreateFire(tanks[0]);
                    Fire fire = new Fire(fire_img, tanks[0].GetDirection());
                    Controls.Add(fire_img);
                    movingcurrently.Add(fire);
                    tanks[0].ResetEnergy();
                    playerProgressBar.Value = 0;
                    s_shot.Play();
                    UpdateDataTest();
                    return;
                }
            }
            else if (e.KeyValue == lastkey) tanks[0].ChangeDirectionTo(Strategy.none);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (state != "ongame") return;
            playerProgressBar.PerformStep();
            //destroy tanks if needed
            if (!(tanks[0] is AiTank) && tanks[0].IsDie()) KeyPreview = false;
            for (int i = 0; i < tanks.Count; i++)
            {
                tanks[i].FillEnergy();
                if (tanks[i].IsDie() && tanks[i].Canfire())
                {
                    tanks[i].RemoveHimself(Controls);
                    movingcurrently.Remove(tanks[i]);
                    tanks.RemoveAt(i);
                    i--;
                    UpdateDataTest();
                }
            }
            if (tanks.Count == 1)
            {
                if (tanks[0].name == "HumanTank1")
                {
                    startBtn.Text = "you win, click start over if you wish to play again";
                    s_winning.Play();
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    startBtn.Text = tanks[0].name + " win, click start over if you wish to play again";
                }
                state = "game_ended";
            }

            //Ai observe
            foreach (Tank t in tanks)
            {
                if (!(t is AiTank) || t.IsDie()) continue;
                // get information for t
                PictureBox[] info = new PictureBox[4] { rightBorder,bottomBorder,leftBorder,topBorder};
                double[] mins = new double[4] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue };
                Point tank_center = t.GetCentral();
                int addedRange = 5;
                List<Rectangle> rects = new List<Rectangle>();
                rects.Add(Rectangle.FromLTRB(t.Bounds.Right, t.Bounds.Top - addedRange, ClientSize.Width, t.Bounds.Bottom + addedRange));
                rects.Add(Rectangle.FromLTRB(t.Bounds.Left-addedRange, t.Bounds.Bottom, t.Bounds.Right+addedRange, ClientSize.Height));
                rects.Add(Rectangle.FromLTRB(0, t.Bounds.Top-addedRange, t.Bounds.Left, t.Bounds.Bottom+addedRange));
                rects.Add(Rectangle.FromLTRB(t.Bounds.Left-addedRange,0, t.Bounds.Right+addedRange, t.Bounds.Top));
                foreach (Control c in Controls)
                {
                    if (!(c is PictureBox)||c.Tag.ToString()=="border") continue;
                    for (int i = 0; i < 4; i++)
                    {
                        if (c.Bounds.IntersectsWith(rects[i]))
                        {
                            double d = MyHelper.CalcDistance(t.Bounds,c.Bounds);
                            if (d < mins[i])
                            {
                                mins[i] = d;
                                info[i] = (PictureBox)c;
                            }
                        } 
                    }
                }
                bool shoot;
                Strategy s = ((AiTank)t).CalcNextMove(info, out shoot);
                t.ChangeDirectionTo(s);
                if (shoot && t.Canfire())
                {
                    PictureBox fire_img = MyHelper.CreateFire(t);
                    Fire fire = new Fire(fire_img,t.GetDirection());
                    Controls.Add(fire_img);
                    movingcurrently.Add(fire);
                    s_shot.Play();
                    t.ResetEnergy();
                    UpdateDataTest();
                }

            }

            //make things move
            for (int i = 0; i < movingcurrently.Count; i++)
            {
                if (movingcurrently[i].IsDoneMoving()) continue;
                bool intersected = false;
                foreach (Control c in Controls)
                {
                    if (!(c is PictureBox)) continue;
                    intersected = true;
                    string result = GetRelationship(movingcurrently[i], (PictureBox)c);
                    if (result == "intersect") movingcurrently[i].MoveOneStep(c.Bounds);
                    else if (result == "destroy" || result == "extinct")
                    {
                        if (result == "destroy") s_explosion.Play();
                        if (GetIndexInMovingCurrently(c) == -1)
                        {
                            movingcurrently[i].RemoveHimself(Controls);
                            movingcurrently.RemoveAt(i);
                            i--;
                            UpdateDataTest();
                        }
                        else RemoveThemAll(ref i, c);
                    }
                    else intersected = false;
                    if (intersected) break;
                }
                if (intersected == false) movingcurrently[i].MoveOneStep();
            }
        }
        private string GetRelationship(MovingObject o, PictureBox px)
        {
            if (o.Bounds.Equals(px.Bounds)) return "equals";
            if (!o.SpeculateOneStep().IntersectsWith(px.Bounds)) return "seperate";
            if(o is Fire || px.Tag.ToString() == "fire")
            {
                // extend the scenerio which shot kills its shooter
                if (o.GetDiffrentFrom(px) == 0&&o is Tank) return "seperate";
                if (o is Tank || px.Tag.ToString() == "humantank" || px.Tag.ToString() == "aitank") return "destroy";
                return "extinct";
                
            }
            return "intersect";
        }
        private Point CreateObject(string type,Point location)
        {
            PictureBox px = null;
            Tank tank = null;
            if (type == "HumanTank")
            {
                px = MyHelper.CreateTank(location, "HumanTank");
                tank = new Tank(px);
                tanks.Add(tank);
            }
            else if (type == "AiTank")
            {
                px = MyHelper.CreateTank(location, "AiTank");
                tank = new AiTank(px);
                tanks.Add(tank);
            }
            else if (type == "Block")
            {
                px = MyHelper.CreateBlock(location);
            }
            foreach (Control c in Controls) 
            {
                Strategy s = MyHelper.WhereMeet(px.Bounds, c.Bounds);
                while (s != Strategy.none)
                {
                    if (s == Strategy.right) px.Bounds = new Rectangle(new Point(px.Bounds.X - 1, px.Bounds.Y), px.Bounds.Size);
                    else if (s == Strategy.down) px.Bounds = new Rectangle(new Point(px.Bounds.X, px.Bounds.Y-1), px.Bounds.Size);
                    else if (s == Strategy.left) px.Bounds = new Rectangle(new Point(px.Bounds.X+1, px.Bounds.Y), px.Bounds.Size);
                    else if (s == Strategy.up) px.Bounds = new Rectangle(new Point(px.Bounds.X, px.Bounds.Y+1), px.Bounds.Size);
                    s = MyHelper.WhereMeet(px.Bounds, c.Bounds);
                }
            }
            foreach (Control c in Controls)
                if (MyHelper.WhereMeet(px.Bounds, c.Bounds) != Strategy.none)
                {
                    tanks.Remove(tank);
                    return new Point();
                }
            Controls.Add(px);
            return px.Location;
        }
        private void RemoveThemAll(ref int i,Control c)
        {
            movingcurrently[GetIndexInMovingCurrently(c)].RemoveHimself(Controls);
            movingcurrently[i].RemoveHimself(Controls);
            int k;
            for (k = 0; k < movingcurrently.Count; k++)
            {
                if (movingcurrently[k].name == c.Name) break;
            }
            if (k == movingcurrently.Count) throw new Exception("something is wrong");
            if (k > i)
            {
                movingcurrently.RemoveAt(k);
                movingcurrently.RemoveAt(i);
                i--;
            }
            else
            {
                movingcurrently.RemoveAt(i);
                movingcurrently.RemoveAt(k);
                i -= 2;
            }
            UpdateDataTest();
        }
        private int GetIndexInMovingCurrently(Control c)
        {
            for (int i = 0; i < movingcurrently.Count; i++)
            {
                if (c.Name == movingcurrently[i].name) return i;
            }
            return -1;
        }
        private void UpdateDataTest()
        {
            //return;
            if (!testingTable.Visible) return;
            dt.Clear();
            int max = Math.Max(Controls.Count, movingcurrently.Count);
            object[,] mat = new object[max, 2];
            for (int i = 0; i < Controls.Count; i++)
                mat[i, 0] = Controls[i].Name;
            for (int i = 0; i < movingcurrently.Count; i++)
                mat[i, 1] = movingcurrently[i].name;
            dt.BeginLoadData();
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                dt.LoadDataRow(new object[] { mat[i, 0], mat[i, 1] }, true);
            }
            dt.EndLoadData();
            testingTable.DataSource = dt;
        }
    }
}
