﻿using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerAndTester.Model
{
    internal interface IPainting
    {
        Matrix<float> GetDescriptors(int clusternum, int deaturenum);
    }
}
