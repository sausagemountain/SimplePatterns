using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    class HighEndServerBuilder: IServerBuilder
    {
        public Server Build()
        {
            var result = new Server() {
                Gpu = MakeGpu(),
                Cpu = MakeCpu(),
                MotherBoard = MakeMotherBoard(),
                Ram = MakeRam(),
                Storage = MakeStorage()
            };
            return result;
        }

        public string MakeMotherBoard() => "High-end Motherboard";
        public string MakeCpu() => "High-end Cpu";
        public string MakeGpu() => "High-end Gpu";
        public string MakeRam() => "High-end Ram";
        public string MakeStorage() => "High-end Storage";
    }
}
