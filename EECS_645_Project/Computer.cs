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
        public Computer()       //This is the constructor for the computer object
        {
            memory = new Memory(this);      //Create a new memory for storing data
            processors = new Processor[4] { new Processor(this, 0, ProcessorStates.Invalid), new Processor(this, 1, ProcessorStates.Invalid), new Processor(this, 2, ProcessorStates.Invalid), new Processor(this,  3, ProcessorStates.Invalid) };              //Create an array of four processors
            numberOfCacheToCacheTransfers = new int[processors.Length,processors.Length];//Create a two dimensional array to hold the number of cache to cache transfers
            bus = new Bus(this);        //Create a new bus for sending signals accross

        }

        public void RunSimulation()
        {
            Console.Write("\nSimulation is started..."); //Write to the console that the simulation has started
            int processorIdWithNextInstruction = CalculateProcessorIdWithNextInstruction(); //Calculate the processor id of the processor that has the instruction with the lowest clock cycle
            while (processorIdWithNextInstruction != -1) //While we found a valid processor id
            {
                RunNextInstruction(processorIdWithNextInstruction); //Run the next instruction
                processorIdWithNextInstruction = CalculateProcessorIdWithNextInstruction(); //Calculate the processor id of the processor that has the instruction with the lowest clock cycle
            }

            foreach(Processor processor in processors)//iterate through every processor
            {
                foreach (CacheLine cacheLine in processor.cache.cacheLines)//iterate through every cache line
                {
                    foreach (CacheWay way in cacheLine.ways)// iterate through every way
                    {
                        switch (way.GetState())//branch to the state that matches the current way state
                        {
                            case ProcessorStates.Invalid: //if the state of way is invalid
                                processor.numberInStateI++; //increment the number in the state invalid for the current processor
                                break;
                            case ProcessorStates.Exclusive: //if the state of way is exclusive
                                processor.numberInStateE++; //increment the number in the state exclusive for the current processor
                                break;
                            case ProcessorStates.Modified: //if the state of way is modified
                                processor.numberInStateM++; //increment the number in the state modified for the current processor
                                break;
                            case ProcessorStates.Owner: //if the state of way is owner
                                processor.numberInStateO++; //increment the number in the state owner for the current processor
                                break;
                            case ProcessorStates.Shared: //if the state of way is shared
                                processor.numberInStateS++; //increment the number in the state shared for the current processor
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.Write("\n\nNumber Of Cache To Cache Transfers:"); //Write to the console two new lines followed by "Number Of Cache To Cache Transfers:"
                foreach(Processor processor2 in processors)//iterate through all the processors
                {
                    if (processor.processorId != processor2.processorId)//if the first processor is not the second processor
                    {
                        Console.Write("\nNumber of P" + processor.processorId + "-P" + processor2.processorId + " transfers: " + numberOfCacheToCacheTransfers[processor.processorId, processor2.processorId]);//Write to the console the number of cache to cache transfers
                    }
                }
            }

            Console.Write("\n\nNumber Of Invalidations For Each Processor:");
            for (int i = 0; i < processors.Length; i++)//iterate through all the processors
            {
                Console.Write("\nP" + i + " Invalidation from:   m=" + processors[i].invalidationNumber[0] + " o=" + processors[i].invalidationNumber[1] + " e=" + processors[i].invalidationNumber[2] + " s=" + processors[i].invalidationNumber[3]);// Write the invalidation number from each state
            }

            Console.Write("\n\nNumber Of Dirty Write Backs For Each Processor:");
            for (int i = 0; i < processors.Length; i++)
            {
                Console.Write("\nP" + i + "=" + (processors[i].invalidationNumber[0] + processors[i].invalidationNumber[1]));//write the dirty writebacks of each state
            }

            Console.Write("\n\nNumber Of Cache State Machines In Each State:");
            for (int i = 0; i < processors.Length; i++)
            {
                Console.Write("\nP" + i + " number in state:   m=" + processors[i].numberInStateM + " o=" + processors[i].numberInStateO + " e=" + processors[i].numberInStateE + " s=" + processors[i].numberInStateS + " i=" + processors[i].numberInStateI);//Write the ending number of processors in each state.
            }

            Console.Write("\nThe simulation has finished all the instructions.");

            Console.Write("\nSimulation is over!\nPress any key to exit!");
            Console.Read();
        }

        void RunNextInstruction(int processorIdWithNextInstruction)
        {
                processors[processorIdWithNextInstruction].RunInstruction();//Tell the processor with the correct id to run its front instruction
        }

        int CalculateProcessorIdWithNextInstruction()
        {
            int processorIdWithNextInstruction = -1; //Initial a variable holding the value of the processor id to -1 so that we will know that if the value ends negative one, we did not find a processor with a usable instruction
            int smallestTimeStamp = -1; //Initialize a variable holding the value of the smallest time stamp to -1 so that we will know that if the value is still -1, we have not yet found a processor with a usable instruction
            for (int i = 0; i < processors.Length; i++) //iterate through all the processors
            {
                if (processors[i].traceData.timeStamp.Count != 0) //if there are still some instructions left over
                {
                    if ((smallestTimeStamp == -1) || (processors[i].traceData.timeStamp[0]<smallestTimeStamp)) //if we have found no instructions yet or the time stamp for the instruction we are looking at is less than the one we think is the least
                    {
                        processorIdWithNextInstruction = i; //Set the processor id with next instruction equal to the current processor id
                        smallestTimeStamp = processors[i].traceData.timeStamp[0]; //Set the current least time stamp to the time stamp of the urrent instruction.
                    }
                }
            }
            return processorIdWithNextInstruction; //return the current processor id.
        }
    }
}
