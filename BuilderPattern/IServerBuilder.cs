using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    interface IServerBuilder
    {
        Server Build();

        string MakeMotherBoard();
        string MakeCpu();
        string MakeGpu();
        string MakeRam();
        string MakeStorage();
    }
}
