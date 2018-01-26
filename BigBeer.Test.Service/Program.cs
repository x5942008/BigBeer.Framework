using BigBeer.Framework.Service.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Test.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServiceHost()
            {

            }.UseServiceCollection(service =>
            {
                service.Add<BigBeerTestService>();
            })
            .Run(args);
        }
    }
}
