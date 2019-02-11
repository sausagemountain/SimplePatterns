namespace BuilderPattern
{
    public class MidRangeServerBuilder: IServerBuilder
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

        public string MakeMotherBoard() => "Mid-Range Motherboard";
        public string MakeCpu() => "Mid-Range Cpu";
        public string MakeGpu() => "Mid-Range Gpu";
        public string MakeRam() => "Mid-Range Ram";
        public string MakeStorage() => "Mid-Range Storage";
    }
}