using System;

namespace Fairweather.Service
{
    public static partial class ES
    {
        public static int ToArg(this Rect_Edge edge) {

            return (int)edge;

        }

        static public bool Same_Or_Opposite(this Direction_LURD dir1, Direction_LURD dir2) {

            if (dir1 == dir2)
                return true;

            return Opposite(dir1, dir2);

        }
        static bool Opposite(Direction_LURD dir1, Direction_LURD dir2) {

            for (int ii = 0; ii < 2; ++ii) {

            if ((dir1 == Direction_LURD.Left && dir2 == Direction_LURD.Right) ||
                (dir1 == Direction_LURD.Up && dir2 == Direction_LURD.Down))

               return true;

                H.Swap(ref dir1, ref dir2);

            }

            return false;
        }

        static Direction_LURD Get_Opposite(Direction_LURD dir1) {

            var d1 = Pair.Make(Direction_LURD.Left, Direction_LURD.Right);
            var d2 = Pair.Make(Direction_LURD.Up, Direction_LURD.Down);

            for (int ii = 0; ii < 2; ++ii) {

                if (dir1 == d1.First)
                    return d1.Second;

                if (dir1 == d2.First)
                    return d2.Second;

                d1 = d1.Flip();
                d2 = d2.Flip();

            }

            throw new InvalidOperationException();

        }


        static public bool UD(this Direction_LURD dir1) {

            switch (dir1) {
                case Direction_LURD.Up:
                case Direction_LURD.Down:
                    return true;
                default:
                    return false;
            }
        }
        static public bool LR(this Direction_LURD dir1) {

            switch (dir1) {
                case Direction_LURD.Left:
                case Direction_LURD.Right:
                    return true;
                default:
                    return false;
            }
        }

        static public int As_Int(this Direction_LURD dir1) {

            switch (dir1) {
                case Direction_LURD.Left:
                    return 0;
                case Direction_LURD.Up:
                    return 1;

                case Direction_LURD.Right:
                    return 2;

                case Direction_LURD.Down:
                    return 3;

                default:
                    true.tift();
                    return 0;
            }
        }
    }
}