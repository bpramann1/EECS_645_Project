using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* Memory is an object representing cache memory */
    public class Memory
    {
	/* Create a computer and associate it with
	 * a memory object */
        Computer computer;

	/* Default Memory constructor */
        public Memory(Computer Computer)
        {
            computer = Computer;
        }
    }
}
