using System;
using System.Drawing;

namespace duobianxing
{
    internal class Image<T1, T2>
    {
        private Bgr bgr;
        private int v1;
        private int v2;

        public Image(int v1, int v2, Bgr bgr)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.bgr = bgr;
        }

        internal void DrawPolyline(Point[] pt, bool v1, Bgr bgr, int v2)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Image<T1, T2>(Image<Bgr, byte> v)
        {
            throw new NotImplementedException();
        }
    }
}