using System;
using System.Drawing;

namespace Rect
{
    internal class Image<T1, T2>
    {
        internal void Draw(Rectangle rect, Bgr bgr, int v)
        {
            throw new NotImplementedException();
        }

        internal void Draw(CircleF circlef, Bgr bgr, int v)
        {
            throw new NotImplementedException();
        }

        internal void Draw(string str, Point str_location, object hersheyComolexSmall)
        {
            throw new NotImplementedException();
        }

        internal void Draw(string str, Point str_location, object hersheyComolexSmall, int v1, Bgr bgr, int v2)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Image<T1,T2>(Image<Bgr, byte> v)
       { 
            throw new NotImplementedException();
        }
    }
}