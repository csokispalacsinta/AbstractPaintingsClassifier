using Emgu.CV;

namespace TrainerAndTester.Model
{
    internal class Category : ICategory
    {
        private static object lockobj = new object();
        private List<IPainting> paintings;
        private string path;
        private string extension;
        private Style style;
        private int quantity;
        private int n;

        public Style Style => style;

        public Category(int quantity, string path, string extension, Style style, int n)
        {
            this.paintings = new List<IPainting>();
            this.path = path;
            this.extension = extension;
            this.style = style;
            this.quantity = quantity;
            this.n = n;
            this.PaintingsLoader();
        }

        private void PaintingsLoader()
        {
            for (int i = 0; i < this.quantity; i++)
            {
                paintings.Add(new Painting(this.path + (i + this.n) + this.extension));
            }
        }

        public void GetDescriptors(List<Matrix<float>> desclist, List<int> styles, int clusternum, int featurenum, int descssize)
        {
            Parallel.For(0, descssize, i =>
            {
                Matrix<float> descs = paintings[i].GetDescriptors(clusternum, featurenum);
                if (descs[0, 0] != 0f || descs[0, 1] != 0f || descs[0, 2] != 0f)
                {
                    lock (lockobj)
                    {
                        desclist.Add(descs);
                        styles.Add((int)this.Style.SerialNumber);
                    }
                }
            });

        }
    }
}
