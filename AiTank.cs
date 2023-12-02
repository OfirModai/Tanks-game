using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace TanksGame
{
    class AiTank : Tank
    {
        Strategy last;
        PictureBox[] seeing;
        List<PictureBox> permanent;
        Point was_enemy;
        public static Random r = new Random();
        public AiTank(PictureBox px) : base(px, "AiTank" + (count + 1))
        {
            last = Strategy.none;
            permanent = new List<PictureBox>();
        }
        public Strategy CalcNextMove(PictureBox[] seeing,out bool shoot)
        {
            //for not doing anything
            /*shoot = false;
            return Strategy.none;*/
            this.seeing = seeing;
            shoot = false;
            permanent.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (seeing[i] == null) continue;
                else if (seeing[i].Tag.ToString() == "aitank" || seeing[i].Tag.ToString() == "humantank") was_enemy = MyHelper.GetCenter(seeing[i].Bounds);
                bool added = false;
                for (int j = 0; j < permanent.Count&&!added; j++)
                {
                    if (CompareTo(seeing[i], permanent[j]) < 0)
                    {
                        permanent.Insert(j, seeing[i]);
                        added = true;
                    }
                }
                if (!added) permanent.Add(seeing[i]);
            }
            Strategy choice = MakeChoice();
            for (int i = 0; i < permanent.Count; i++)
            {
                if (permanent[i].Tag.ToString() == "aitank" || permanent[i].Tag.ToString() == "humantank")
                {
                    if(GetDirectionOfObject(permanent[i])==(int)last)
                    {
                        shoot = true;
                        break;
                    }
                }
            }
            return choice;
        }
        private Strategy MakeChoice()
        {
            if (permanent.Count == 0) return DoUsual();
            //  דבר ראשון יריות שמופנות אלי או לפנות לכיוון אוייב
            for (int i = 0; i < permanent.Count; i++)
            {
                int direction_of_shot_against_me = GetDirectionOfObject(permanent[i]);
                if (IsAShotAgainstMe(direction_of_shot_against_me))
                {
                    int temp = direction_of_shot_against_me + 1;
                    for (int j = 0; j < 2; j++)
                    {
                        temp = CorrectingDirection(temp);
                        if (IsAShotAgainstMe(GetDirectionOfObject(seeing[temp]))) return MakeChoiceForReal(GetDirectionOfObject(permanent[i]) + 2);
                        temp += 2;
                    }
                    bool one = false, two = false;
                    for (int j = 0; j < 2; j++)
                    {
                        temp = CorrectingDirection(temp);
                        int dis_escapeBullet;
                        if (direction_of_shot_against_me == 1 || direction_of_shot_against_me == 3)
                        {
                            if (temp == 0) dis_escapeBullet = permanent[i].Bounds.Right - Bounds.Left;
                            else dis_escapeBullet = permanent[i].Bounds.Left - Bounds.Right;
                        }
                        else
                        {
                            if (temp == 1) dis_escapeBullet = permanent[i].Bounds.Bottom - Bounds.Top;
                            else dis_escapeBullet = permanent[i].Bounds.Top - Bounds.Bottom;
                        }

                        if (MyHelper.CalcDistance(Bounds, seeing[temp].Bounds) < dis_escapeBullet)
                        {
                            if (j == 0) one = true;
                            else two = true;
                        }
                        else if (permanent[i].Tag.ToString()=="fire"&&dis_escapeBullet > -1 * speed && dis_escapeBullet < 0) 
                            return MakeChoiceForReal(temp);
                        temp += 2;
                    }
                    temp = CorrectingDirection(temp);
                    if (one && two) return MakeChoiceForReal(temp + 1);
                    if (one) return MakeChoiceForReal(temp+2);
                    if (two) return MakeChoiceForReal(temp);
                    Point me = GetCentral();
                    Point other = MyHelper.GetCenter(permanent[i].Bounds);
                    if (Math.Abs(me.Y - other.Y) > Math.Abs(me.X - other.X))
                    {
                        if (me.X > other.X) return MakeChoiceForReal(0);
                        return MakeChoiceForReal(2);
                    }
                    if (me.Y > other.Y) return MakeChoiceForReal(1);
                    return MakeChoiceForReal(3);
                }
                else if (permanent[i].Tag.ToString() == "aitank" || permanent[i].Tag.ToString() == "humantank")
                {
                    was_enemy = MyHelper.GetCenter(permanent[i].Bounds);
                    if(Canfire()) return MakeChoiceForReal(PickForGettingCloseTo(was_enemy));
                    return MakeChoiceForReal(PickForGettingCloseTo(was_enemy)+2);
                }
            }

            int options = 4, reliable = -1, danger = -1;
            // לברוח מיריות קרובות או קירות
            for (int i = 0; i < seeing.Length; i++)
            {
                if (IsAObjectNearMe(i))
                {
                    options--;
                    danger = i;
                }
                else reliable = i;
            }
            if (options == 4)
                return DoUsual();
            int oposit = CorrectingDirection(danger + 2);
            //if (danger == PickForGettingCloseTo(was_enemy)) was_enemy = new Point();
            switch (options)
            {
                case 0:
                    return Strategy.none;
                case 1:
                    return (Strategy)reliable;
                case 3:
                    int x =PickForGettingCloseTo(was_enemy);
                    if(x==-1||IsAObjectNearMe(x)) return MakeChoiceForReal(oposit);
                    return MakeChoiceForReal(x);
                case 4:
                    return DoUsual();
                default:
                    if (IsAObjectNearMe(GetDirectionOfObject(seeing[oposit]))) return MakeChoiceForReal(danger + 1);
                    else return MakeChoiceForReal(oposit);
            }
        }
        private bool IsAShotAgainstMe(int direction)
        {
            if (seeing[direction] == null) return false;
            return (seeing[direction].Tag.ToString() == "fire" && Math.Abs(direction-(int)((Strategy)seeing[direction].Image.Tag)) == 2);
        }
        private bool IsAObjectNearMe(int direction)
        {
            if (seeing[direction] == null) return false;
            if (MyHelper.CalcDistance(Bounds, seeing[direction].Bounds) > 5) return false;
           if (seeing[direction].Tag.ToString() == "fire" && direction-(int)(Strategy)seeing[direction].Image.Tag == 0)
                return false;
            return true;
        }
        private int GetDirectionOfObject(PictureBox px)
        {
            //מה עושים עם אובייקט נכנס פעמיים
            for (int i = 0; i < 4; i++)
            {
                if (seeing[i] == px) return i;
            }
            return -1;
        }
        private int CorrectingDirection(int tocorrect) { if (tocorrect > 3) tocorrect -= 4; return tocorrect; }
        private Strategy MakeChoiceForReal(int choice)
        {
            if (choice != -1)
            {
                choice = CorrectingDirection(choice);
                if (seeing[choice].Tag.ToString() == "fire" && MyHelper.CalcDistance(seeing[choice].Bounds, Bounds) <= 5)
                    choice = -1;
            }
            last = (Strategy)(choice);
            return last;
        }
        private Strategy DoUsual()
        {
            //return Strategy.none;
            if (was_enemy.IsEmpty)
            {
                if (last==Strategy.none||r.Next(0, 15) == 0) last = (Strategy)r.Next(0, 4);
                //while((int)last-1==Convert(cant_move)) last = (Strategy)r.Next(0, 4);
                return last;
            }
            Point my = GetCentral();
            if (MyHelper.CalcDistance(Bounds, was_enemy) < 10)
            {
                was_enemy = new Point();
                return MakeChoiceForReal(-1);
            }
            return MakeChoiceForReal(PickForGettingCloseTo(was_enemy));
        }
        private int PickForGettingCloseTo(Point other)
        {
            if (other.IsEmpty) return -1;
            Point my = GetCentral();
            bool my_top=true, my_left=true;
            if (my.Y > other.Y) my_top = false;
            if (my.X > other.X) my_left = false;
            int dif_x = Math.Abs(other.X - my.X), dif_y = Math.Abs(other.Y - my.Y);

            /*if ( dif_x>dif_y )
            {
                if (my_left) return 0;
                else return 2;
            }
            if (my_top) return 1;
            return 3;*/

            int range = 36;
            if(dif_x<range||dif_y<range)
            {
                if (dif_x<range)
                {
                    if (my_top) return 1;
                    return 3;
                }
                else if (my_left) return 0;
                return 2;
            }
            if (dif_x < dif_y)
            {
                if (my_left) return 0;
                return 2;
            }
            else if (my_top) return 1;
            return 3;

            
        }
        private int CompareTo(PictureBox @base, PictureBox to)
        {
            if (MyHelper.CalcDistance(Bounds, @base.Bounds) < MyHelper.CalcDistance(Bounds, to.Bounds)) return -1;
            return 1;
        }
    }
    enum Strategy
    {
        none=-1, right, down, left, up
    }
}