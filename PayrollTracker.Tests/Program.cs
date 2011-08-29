using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollTracker.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid guid = new Guid("7a37f1b9886a4a2dadd5c2681440d010");
            Console.WriteLine(guid);

            DateTime dt = DateTime.Now;
            Console.WriteLine(dt);
        }
    }
}
