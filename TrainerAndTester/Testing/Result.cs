using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerAndTester.Testing
{
    internal class Result
    {
        private float[] results;

        public string Name { get; set; }
        public float[] GetResults{ get { return results; } }

        public Result(int categorycount)
        {
            this.results = new float[categorycount + 1];
            for (int i = 0; i < results.Length; i++)
            {
                this.results[i] = 0;
            }
        }

        public void AddResult(float percent, int i)
        {
            this.results[i] = percent;
        }
    }
}
