namespace BuilderPattern
{
    public class BaselineServerBuilder: IServerBuilder
    {
        public Server Build()
        {
            var result = new Server()
            {
                Cpu = MakeCpu(),
                Gpu = MakeGpu(),
                MotherBoard = MakeMotherBoard(),
                Ram = MakeRam(),
                Storage = MakeStorage()
            };
            return result;
        }

        public string MakeMotherBoard() => "Baseline Motherboard";
        public string MakeCpu() => "Baseline Cpu";
        public string MakeGpu() => "Baseline Gpu";
        public string MakeRam() => "Baseline Ram";
        public string MakeStorage() => "Baseline Storage";
    }
}