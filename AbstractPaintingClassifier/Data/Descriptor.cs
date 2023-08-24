using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractPaintingClassifier.Data
{
    public class Descriptor
    {
        public float GetORB(bool isHarris, Mat image)
        {
            UMat uMat = new UMat();
            ORB orb = new ORB(500, 1.2f, 8, 31, 0, 2, isHarris ? ORB.ScoreType.Harris : ORB.ScoreType.Fast, 31, 20);
            orb.DetectAndCompute(image, null, new VectorOfKeyPoint(), uMat, false);

            if (uMat.Rows == 0)
            {
                return 0f;
            }
            UMat desccv32f = new UMat();
            uMat.ConvertTo(desccv32f, Emgu.CV.CvEnum.DepthType.Cv32F);
            if (uMat.Rows < 6)
            {
                if (uMat.Rows > 0)
                {
                    return (float)CvInvoke.Kmeans(desccv32f, uMat.Rows, new Mat(), new MCvTermCriteria(), 2, 0);
                }
            }

            return (float)CvInvoke.Kmeans(desccv32f, 6, new Mat(), new MCvTermCriteria(), 2, 0);
        }

        public float GetSIFT(Mat image)
        {
            UMat uMat = new UMat();
            SIFT sift = new SIFT(500, 3, 0.04, 10, 1.6);
            sift.DetectAndCompute(image, null, new VectorOfKeyPoint(), uMat, false);

            if (uMat.Rows == 0)
            {
                return 0f;
            }
            if (uMat.Rows < 6)
            {
                if (uMat.Rows > 0)
                {
                    return (float)CvInvoke.Kmeans(uMat, uMat.Rows, new Mat(), new MCvTermCriteria(), 2, 0);
                }
            }
            return (float)CvInvoke.Kmeans(uMat, 6, new Mat(), new MCvTermCriteria(), 2, 0);
        }
    }
}
