using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    class ServerDirector
    {
        public IServerBuilder ServerBuilder { get; set; }

        public ServerDirector() { }

        public ServerDirector(IServerBuilder serverBuilder)
        {
            ServerBuilder = serverBuilder ?? throw new ArgumentNullException(nameof(serverBuilder));
        }

        public Server Construct()
        {
            return ServerBuilder.Build();
        }
    }
}
