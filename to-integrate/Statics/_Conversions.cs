
namespace Fairweather.Service
{
    public static class C
    {
        public static float MM_to_Pix(float mm, float dpi) {

            var inches = MM_to_In(mm);
            var ret = inches * dpi;

            return ret;

        }

        public static float Pix_to_MM(float pix, float dpi) {

            var inches = pix / dpi;

            var ret = In_to_MM(inches);

            return ret;

        }

        public static float In_to_MM(float inches) {

            return (float)(inches * 25.4);

        }
        public static float MM_to_In(float mm) {
            return (float)(mm * 0.0393700787);
        }
    }

}