namespace BuilderPattern
{
    public class Server
    {
        public string MotherBoard { get; set; }
        public string Cpu { get; set; }
        public string Gpu { get; set; }
        public string Ram { get; set; }
        public string Storage { get; set; }

        public override string ToString()
        {
            return $"{MotherBoard}, {Cpu}, {Ram}, {Gpu}, {Storage}";
        }
    }
}