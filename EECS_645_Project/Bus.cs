using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class Bus
    {
        Computer computer;  //This is the computer the owns the bus

        public Bus(Computer Computer)   //Constructor for the bus
        {
            computer = Computer;    //Set the computer to the one that created the bus
        }

        public void SendSignal(BusSignal signal, Processor sendingProcessor) //Function to send a signal to all the processors but the sending processor
        {
            foreach (Processor processor in computer.processors)
            {
                if ((processor != sendingProcessor) && (processor.HasData(signal.tag, signal.index, signal.offset)))
                {
                    processor.RecieveSignal(signal);
                }
            }
        }

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

        public void SendData(int Data, Processor recievingProcessor)
        {
            foreach (Processor processor in computer.processors)
            {
                //if (processor != sendingProcessor)
                //{
                //    processor.RecieveSignal(new BusSignal(Transaction));
                //}
            }
        }
    }
}
