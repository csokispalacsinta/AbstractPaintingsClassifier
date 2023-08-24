using Emgu.CV.ML.MlEnum;
using Emgu.CV.ML;
using Emgu.CV;
using TrainerAndTester.Model;

namespace TrainerAndTester.Training
{
    internal class Trainer
    {
        private List<ICategory> categories = new List<ICategory>();
        private int quantity;

        public Trainer(int quantity, string[] categoriespath, Style[] styles)
        {
            this.quantity = quantity;
            for (int i = 0; i < categoriespath.Length; i++)
            {
                categories.Add(new Category(quantity, categoriespath[i], ".jpg", styles[i], 1));
            }
        }


        public void Train(string rtreesname, int clusternum, int featurenum, int descsize)
        {
            List<Matrix<float>> alldescriptor = new List<Matrix<float>>();
            List<int> styles = new List<int>();

            foreach (var category in this.categories)
            {
                category.GetDescriptors(alldescriptor, styles, 
                    clusternum, featurenum, descsize < this.quantity ? descsize : quantity);
            }

            RTrees rTrees = new RTrees();
            rTrees.MaxCategories = categories.Count + 1;

            rTrees.Train(new TrainData(this.GetMatrix(alldescriptor),
                DataLayoutType.RowSample, new Matrix<int>(styles.ToArray())));

            rTrees.Save(rtreesname);

            Console.WriteLine("Training was successful!");
        }


        private Matrix<float> GetMatrix(List<Matrix<float>> descriptors)
        {
            int listcount = descriptors.Count;
            int matrixsize = descriptors[0].Width;

            Matrix<float> matrix = new Matrix<float>(listcount, matrixsize);
            for (int i = 0; i < listcount; i++)
            {
                for (int y = 0; y < matrixsize; y++)
                {
                    matrix[i, y] = descriptors[i][0, y];
                }
            }

            return matrix;
        }
    }
}
