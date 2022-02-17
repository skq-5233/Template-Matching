namespace bianli
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
    }
}