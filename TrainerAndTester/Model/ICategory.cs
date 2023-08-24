using Emgu.CV;

namespace TrainerAndTester.Model
{
    internal interface ICategory
    {
        Style Style { get; }
        void GetDescriptors(List<Matrix<float>> descs, List<int> styles, int clusternum, int featurenum, int descssize);
    }
}
