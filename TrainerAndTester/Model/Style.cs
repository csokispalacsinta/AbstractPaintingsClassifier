

namespace TrainerAndTester.Model
{
    internal class Style
    {
        public string? Name { get; set; }
        public int SerialNumber { get; set; }

        public Style(string name, int serialnumber)
        {
            this.Name = name;
            this.SerialNumber = serialnumber;
        }
    }
}
