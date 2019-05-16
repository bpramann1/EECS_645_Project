using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* Bus models a memory bus */
    public class Bus
    {
        Computer computer;  //This is the computer the owns the bus
        

        /* Default Bus Constructor */
        public Bus(Computer Computer)
        {
            computer = Computer;
        }

        /* SendSignal broadcasts a signal from one processor to all others */
        public void SendSignal(BusSignal signal, Processor sendingProcessor)
        {
            foreach (Processor processor in computer.processors)
            {
                if ((processor != sendingProcessor) && (processor.HasData(signal.tag, signal.index, signal.offset)))
                {
                    processor.RecieveSignal(signal);
                }
            }
        }



	/* GetData iterates through an array of processors stored in a computer and increases 
	 * the number of Cache to Cache transfers based on the MOESI protocol */
        public string GetData(Processor askingProcessor, string tag, string index, string offset)
        {
            string data = "";
            int askingProcessorId = -1;
            for (int i = 0; i < computer.processors.Length; i++)
            {
                if (computer.processors[i] == askingProcessor)
                {
                    askingProcessorId = i;
                }
            }

            for (int i = 0; i < computer.processors.Length; i++)
            {
                if (i != askingProcessorId)
                {
                    if (computer.processors[i].HasData(tag, index, offset))
                    {
                        data = computer.processors[i].GetData(tag, index, offset);
                        computer.numberOfCacheToCacheTransfers[i, askingProcessorId]++;
                     }
                }
            }
            return data;
        }

	/* HasData iterates through the processors owned by this computer and returns a bool
	 * value representing whether there is data stored in any processor other than
	 * the askingProcessor */
        public bool HasData(Processor askingProcessor, string tag, string index, string offset)
        {
            bool hasData = false;
            foreach (Processor processor in computer.processors)
            {
                if (processor != askingProcessor)
                {
                    hasData = hasData || processor.HasData(tag, index, offset);
                }
            }
            return hasData;
        }
    }
}
