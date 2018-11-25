using UnityEngine;
using System.Collections;

namespace Nothke.Math.Coord
{
    public struct Coord
    {

        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coord LeftDir()
        {
            if (this == F) return L;
            if (this == R) return F;
            if (this == B) return R;
            if (this == L) return B;

            return Zero;
        }

        public Coord RightDir()
        {
            if (this == F) return R;
            if (this == R) return B;
            if (this == B) return L;
            if (this == L) return F;

            return Zero;
        }

        public static Coord F { get { return new Coord(0, 1); } }
        public static Coord B { get { return new Coord(0, -1); } }
        public static Coord L { get { return new Coord(-1, 0); } }
        public static Coord R { get { return new Coord(1, 0); } }
        public static Coord Zero { get { return new Coord(0, 0); } }

        public static Coord RandomHorizontalDir()
        {
            int r = Random.Range(0, 4);

            switch (r)
            {
                case 0: return new Coord(1, 0);
                case 1: return new Coord(-1, 0);
                case 2: return new Coord(0, 1);
                case 3: return new Coord(0, -1);
            }

            return new Coord(0, 1);
        }

        public static Coord RandomHorizontalDirOtherThan(Coord dir)
        {
            Coord newDir;

            do
            {
                newDir = RandomHorizontalDir();
            } while (newDir == dir);

            return newDir;
        }

        public static Coord operator +(Coord left, Coord right)
        {
            return new Coord(left.x + right.x, left.y + right.y);
        }

        public static Coord operator -(Coord left, Coord right)
        {
            return new Coord(left.x - right.x, left.y - right.y);
        }

        public static Coord operator *(Coord left, int scalar)
        {
            return new Coord(left.x * scalar, left.y * scalar);
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1.x == c2.x && c1.y == c2.y);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }
    }
}