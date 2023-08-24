﻿using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace AbstractPaintingClassifier.Data
{
    public enum Style
    {
        AbstractExpressionism, ArtInformel, ActionPainting, Minimalism, OpArt
    }


    internal class Predicter
    {
        RTrees rTrees;
        string modelname = "model.xml";

        public Predicter()
        {
            rTrees = new RTrees();
            rTrees.Load(modelname);
        }


        public string GetStyle(Mat image)
        {
            int result = (int)rTrees.Predict(GetDescriptors(image));

            return ((Style) result).ToString();
        }

        private Matrix<float> GetDescriptors(Mat image)
        {
            Matrix<float> descs = new Matrix<float>(1, 3);

            descs[0, 0] = GetSIFT(image);
            descs[0, 1] = GetORB(false, image);
            descs[0, 2] = GetORB(true, image);

            return descs;
        }

        private float GetORB(bool isHarris, Mat image)
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
