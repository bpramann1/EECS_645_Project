using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* Program is used to launch the simulator */
    public class Program
    {
        static void Main(string[] args)
        {
	    /* Our simulation is ran by first creating a virtual
	     * computer and then running the simulation from within
	     * that virtual computer */
            Computer computer = new Computer();
            computer.RunSimulation(); //Run the Simulation      -Hint Visual Studio allows you to press F12 to go to function definition
        }
    }
}
