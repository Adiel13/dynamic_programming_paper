using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculo_polinomios
{
    internal class principal
    {
        static void Main(string[] args)
        {
            karatsuba k = new karatsuba(2048);
            k.fillKaratsuba();
            Console.WriteLine(k.switchcode(2048));

            striding s = new striding(16);
            s.fillStriding();
            //s.generateCode();

            hibrido h = new hibrido(2048);
            h.fillHibrido();
            //h.generateCode();



            // Display the number of command line arguments.
            Console.WriteLine("hola mundo");
        }
    }
}
