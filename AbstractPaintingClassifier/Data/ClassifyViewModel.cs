using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractPaintingClassifier.Data
{
    public class ClassifyViewModel
    {
        public string Style { get; set; }

        public float[] Descriptors { get; set; }
    }
}
