using Emgu.CV.ML;
using Emgu.CV;
using System.Runtime.InteropServices;
using TrainerAndTester.Model;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace TrainerAndTester.Testing
{
    internal class Tester
    {
        private List<ICategory> categories = new List<ICategory>();
        private int quantity;
        private List<Result> results = new List<Result>();
        private Model.Style[] styles;

        public Tester(int quantity, int trainsize, string[] categoriespath, Model.Style[] styles)
        {
            this.quantity = quantity;
            this.styles = styles;
            for (int i = 0; i < categoriespath.Length; i++)
            {
                categories.Add(new Category(quantity, categoriespath[i], ".jpg", styles[i], trainsize + 1));
            }
        }

        public void Testing(string rtreesname, int clusternum, int featurenum)
        {
            List<Matrix<float>> alldescriptor = new List<Matrix<float>>();
            List<int> expected = new List<int>();

            foreach (var category in this.categories)
            {
                category.GetDescriptors(alldescriptor, expected, clusternum, featurenum, this.quantity);
            }

            RTrees rTrees = new RTrees();
            rTrees.Load(rtreesname);


            int[] results = new int[categories.Count + 1];
            for (int i = 0; i < expected.Count(); i++)
            {
                int rst = (int)rTrees.Predict(alldescriptor[i]);

                if (rst == expected[i])
                {
                    results[categories.Count]++;
                    results[rst]++;
                }
            }

            this.WriteOut(results, expected, rtreesname);
        }

        public void Save(string filename)
        {
            Application xlApp = new Application();

            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            for (int i = 0; i < styles.Length; i++)
            {
                xlWorkSheet.Cells[i + 2, 1] = styles[i].Name;
            }

            xlWorkSheet.Cells[styles.Length + 2, 1] = "Final Accuracy";

            for (int i = 0; i < results.Count; i++)
            {
                for (int j = 0; j < categories.Count + 1; j++)
                {
                    xlWorkSheet.Cells[j + 2, i + 2] = results[i].GetResults[j];
                }
            }

            xlWorkSheet.Cells[styles.Length + 4, 1] = "Best Accuracy";
            xlWorkSheet.Cells[styles.Length + 4, 2] = results.Max(x => x.GetResults[categories.Count]);

            xlWorkBook.SaveAs(@"E:\Egyetem\Projektmunka\SzakdolgozatProgramok\" + filename,
                XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }

        private void WriteOut(int[] sum, List<int> expected, string rtreesname)
        {
            Result result = new Result(categories.Count);
            result.Name = rtreesname;

            for (int i = 0; i < categories.Count + 1; i++)
            {
                result.AddResult((float)(sum[i] 
                    / (float)expected.Count(x => x == i)) * 100, i);
                Console.WriteLine(sum[i]);
            }

            result.AddResult((float)(sum[categories.Count] 
                / (float)expected.Count()) * 100, categories.Count);

            this.results.Add(result);
        }
    }
}
