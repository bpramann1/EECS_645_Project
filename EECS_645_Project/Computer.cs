using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EECS_645_Project
{
    public class Computer
    {
        public Processor[] processors;
        public int[,] numberOfCacheToCacheTransfers;
        public Bus bus;
        public Memory memory;
        public Computer()
        {
            memory = new Memory(this);
            processors = new Processor[4] { new Processor(this, 0, ProcessorStates.Invalid), new Processor(this, 1, ProcessorStates.Invalid), new Processor(this, 2, ProcessorStates.Invalid), new Processor(this,  3, ProcessorStates.Invalid) };//Index will represent processor id
            numberOfCacheToCacheTransfers = new int[processors.Length,processors.Length];
            bus = new Bus(this);

        }

        public void RunSimulation()
        {
            Console.Write("\nSimulation is started...");
            int cycleNumber = 0;
            int processorIdWithNextInstruction = CalculateProcessorIdWithNextInstruction();
            while (processorIdWithNextInstruction != -1)
            {
                if ((processors[processorIdWithNextInstruction].traceData.index.Count == 742) && (processorIdWithNextInstruction == 0)) //
                {
                    int hey = 9;
                }
                RunNextInstruction(processorIdWithNextInstruction);
                cycleNumber++;
                processorIdWithNextInstruction = CalculateProcessorIdWithNextInstruction();
            }

            for (int i = 0; i < processors.Length; i++)
            {
                for (int j = 0; j < processors[i].cache.cacheLines.Length; j++)
                {
                    for (int k = 0; k < processors[i].cache.cacheLines[j].ways.Length; k++)
                    {
                            //if ((processors[i].cache.cacheLines[j].ways[k].tag != null) || (processors[i].cache.cacheLines[j].ways[k].tag == "1"))
                            //{
                            switch (processors[i].cache.cacheLines[j].ways[k].processorState)
                            {
                                case ProcessorStates.Invalid:
                                    processors[i].numberInStateI++;
                                    break;
                                case ProcessorStates.Exclusive:
                                    processors[i].numberInStateE++;
                                    break;
                                case ProcessorStates.Modified:
                                    processors[i].numberInStateM++;
                                    break;
                                case ProcessorStates.Owner:
                                    processors[i].numberInStateO++;
                                    break;
                                case ProcessorStates.Shared:
                                    processors[i].numberInStateS++;
                                    break;
                                default:
                                    break;

                            //}
                        }
                    }
                }
                Console.Write("\nNumber of unique instructions:" + processors[i].numberOfUniqueInstructions + "\nNumber of index:" + processors[i].indexs.Count + "\nNumber of offsets:" + processors[i].offsets.Count);
                Console.Write("\n\nNumber Of Cache To Cache Transfers:");
                for (int j = 0; j < processors.Length; j++)
                {
                    if (i != j)
                    {
                        Console.Write("\nNumber of P" + i + "-P" + j + " transfers: " + numberOfCacheToCacheTransfers[i, j]);
                    }
                }







            }

            Console.Write("\n\nNumber Of Invalidations For Each Processor:");
            for (int i = 0; i < processors.Length; i++)
            {
                Console.Write("\nP" + i + " Invalidation from:   m=" + processors[i].invalidationNumber[0] + " o=" + processors[i].invalidationNumber[1] + " e=" + processors[i].invalidationNumber[2] + " s=" + processors[i].invalidationNumber[3]);
            }

            Console.Write("\n\nNumber Of Dirty Write Backs For Each Processor:");
            for (int i = 0; i < processors.Length; i++)
            {
                Console.Write("\nP" + i + "=" + (processors[i].invalidationNumber[0] + processors[i].invalidationNumber[1]));
            }

            Console.Write("\n\nNumber Of Cache State Machines In Each State:");
            for (int i = 0; i < processors.Length; i++)
            {
                Console.Write("\nP" + i + " number in state:   m=" + processors[i].numberInStateM + " o=" + processors[i].numberInStateO + " e=" + processors[i].numberInStateE + " s=" + processors[i].numberInStateS + " i=" + processors[i].numberInStateI);
            }

            Console.Write("\nThe simulation has finished all the instructions.");

            Console.Write("\nSimulation is over!\nPress any key to exit!");
            Console.Read();
        }

        void RunNextInstruction(int processorIdWithNextInstruction)
        {
                processors[processorIdWithNextInstruction].RunInstruction();
        }

        int CalculateProcessorIdWithNextInstruction()
        {
            int processorIdWithNextInstruction = -1;
            int smallestTimeStamp = -1;
            for (int i = 0; i < processors.Length; i++)
            {
                if (processors[i].traceData.timeStamp.Count != 0)
                {
                    if ((smallestTimeStamp == -1) || (processors[i].traceData.timeStamp[0]<smallestTimeStamp))
                    {
                        processorIdWithNextInstruction = i;
                        smallestTimeStamp = processors[i].traceData.timeStamp[0];
                    }
                }
            }
            return processorIdWithNextInstruction;
        }
    }
}
