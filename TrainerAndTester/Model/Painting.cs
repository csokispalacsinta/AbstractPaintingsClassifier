using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace TrainerAndTester.Model
{
    internal class Painting : IPainting
    {
        private Mat image;

        public Painting(string path)
        {
            image = new Mat(path);
        }

        public Matrix<float> GetDescriptors(int clusternum, int featurenum)
        {
            Matrix<float> descs = new Matrix<float>(1, 3);

            descs[0, 0] = this.GetORB(true, clusternum, featurenum);
            descs[0, 1] = this.GetORB(false, clusternum, featurenum);
            descs[0, 2] = this.GetSIFT(clusternum, featurenum);

            return descs;
        }

        private float GetSIFT(int clusternum, int featurenum)
        {
            UMat uMat = new UMat();
            SIFT sift = new SIFT(featurenum, 3, 0.04, 10, 1.6);
            sift.DetectAndCompute(image, null, new VectorOfKeyPoint(), uMat, false);

            if (uMat.Rows < clusternum)
            {
                return 0f;
            }
            return (float)CvInvoke.Kmeans(uMat, clusternum, new Mat(), new MCvTermCriteria(), 2, 0);
        }

        private float GetORB(bool isHarris, int clusternum, int featurenum)
        {
            UMat uMat = new UMat();
            ORB orb = new ORB(featurenum, 1.2f, 8, 31, 0, 2, isHarris ?
                                                            ORB.ScoreType.Harris : ORB.ScoreType.Fast, 31, 20);
            orb.DetectAndCompute(image, null, new VectorOfKeyPoint(), uMat, false);

            if (uMat.Rows < clusternum)
            {
                return 0f;
            }
            UMat desccv32f = new UMat();
            uMat.ConvertTo(desccv32f, Emgu.CV.CvEnum.DepthType.Cv32F);

            return (float)CvInvoke.Kmeans(desccv32f, clusternum, new Mat(), new MCvTermCriteria(), 2, 0);
        }
    }
}
