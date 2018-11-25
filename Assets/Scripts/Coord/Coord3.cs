using UnityEngine;
using System.Collections;

namespace Nothke.Math.Coord
{
    public struct Coord3
    {
        public int x;
        public int y;
        public int z;

        public Coord3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Coord3(int x, int y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }

        public Coord3 LeftDir()
        {
            if (this == F) return L;
            if (this == R) return F;
            if (this == B) return R;
            if (this == L) return B;

            return Zero;
        }

        public Coord3 RightDir()
        {
            if (this == F) return R;
            if (this == R) return B;
            if (this == B) return L;
            if (this == L) return F;

            return Zero;
        }

        public static Coord3 F { get { return new Coord3(0, 0, 1); } }
        public static Coord3 B { get { return new Coord3(0, 0, -1); } }
        public static Coord3 L { get { return new Coord3(-1, 0, 0); } }
        public static Coord3 R { get { return new Coord3(1, 0, 0); } }
        public static Coord3 U { get { return new Coord3(0, 1, 0); } }
        public static Coord3 D { get { return new Coord3(0, -1, 0); } }
        public static Coord3 Zero { get { return new Coord3(0, 0, 0); } }

        public static Coord3 RandomHorizontalDir()
        {
            int r = Random.Range(0, 4);

            switch (r)
            {
                case 0: return new Coord3(1, 0, 0);
                case 1: return new Coord3(-1, 0, 0);
                case 2: return new Coord3(0, 0, 1);
                case 3: return new Coord3(0, 0, -1);
            }

            return new Coord3(0, 1, 0);
        }

        public static Coord3 RandomHorizontalDirOtherThan(Coord3 dir)
        {
            Coord3 newDir;

            do
            {
                newDir = RandomHorizontalDir();
            } while (newDir == dir);

            return newDir;
        }

        public static Coord3 RandomDir()
        {
            int r = Random.Range(0, 6);

            switch (r)
            {
                case 0: return new Coord3(1, 0, 0);
                case 1: return new Coord3(-1, 0, 0);

                case 2: return new Coord3(0, 1, 0);
                case 3: return new Coord3(0, -1, 0);

                case 4: return new Coord3(0, 0, 1);
                case 5: return new Coord3(0, 0, -1);
            }

            return new Coord3(0, 1, 0);
        }

        public static Coord3 operator +(Coord3 left, Coord3 right)
        {
            return new Coord3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Coord3 operator -(Coord3 left, Coord3 right)
        {
            return new Coord3(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        public static Coord3 operator *(Coord3 left, int scalar)
        {
            return new Coord3(left.x * scalar, left.y * scalar, left.z * scalar);
        }

        public static bool operator ==(Coord3 c1, Coord3 c2)
        {
            return c1.x == c2.x && c1.y == c2.y && c1.z == c2.z;
        }

        public static bool operator !=(Coord3 c1, Coord3 c2)
        {
            return !(c1.x == c2.x && c1.y == c2.y && c1.z == c2.z);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", x, y, z);
        }
    }
}