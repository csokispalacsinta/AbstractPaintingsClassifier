using TrainerAndTester.Testing;
using TrainerAndTester.Training;
using TrainerAndTester.Model;


string[] pathes =
{
    @"E:\Bow\Abstract\absz",
    @"E:\Bow\Baroque\bar",
    @"E:\Bow\Cubism\cub",
    @"E:\Bow\Renaissance\ren",
    @"E:\Bow\Expressionism\exp",
    @"E:\Bow\Impressionism\imp",
    @"E:\Bow\Pop_Art\pop"
};

Style[] styles = new Style[]
{
    new Style("Abstract",0),
    new Style("Baroque",1),
    new Style("Cubism",2),
    new Style("Renaissance",3),
    new Style("Expressionism",4),
    new Style("Impressionism",5),
    new Style("Pop Art",7)
};


int[] clusternums = { 20, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
int[] featurenums = { 500, 500, 300, 400, 500, 600, 700, 1000 };
int[] trainsize = { 170, 100, 110, 120, 130, 140, 150, 160, 170 };

Trainer trainer = new Trainer(170, pathes, styles);
Tester tester = new Tester(30, 170,  pathes, styles);


for (int i = 0; i < 1; i++)
{
    for (int j = 0; j < 1; j++)
    {
        for (int k = 0; k < 1; k++)
        {
            string rtreesname = "rtrees" + trainsize[i] + featurenums[j] + clusternums[k] + ".xml";
            Console.WriteLine(rtreesname);
            trainer.Train(rtreesname, clusternums[k], featurenums[j], trainsize[i]);
            tester.Testing(rtreesname, clusternums[k], featurenums[j]);
        }
    }
}

tester.Save("test17020.xls");

