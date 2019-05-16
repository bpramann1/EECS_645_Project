using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class Processor
    {
        public Computer computer; //This is needed to be a reference to the computer that owns the processor
        public Cache cache; //This will be the cache associated with this processor
        public TraceData traceData; //This will be the data for the trace associated with this processor
        public int processorId; //This will be the processor id
        public int[] invalidationNumber = new int[4]; //This is an array for keeping track how many times each state has be invalidated
        public int numberInStateM = 0; //This is for keeping track how many cache lines are in state M
        public int numberInStateO = 0; //This is for keeping track how many cache lines are in state O
        public int numberInStateE = 0; //This is for keeping track how many cache lines are in state E
        public int numberInStateS = 0; //This is for keeping track how many cache lines are in state S
        public int numberInStateI = 0; //This is for keeping track how many cache lines are in state I
        public Processor(Computer Computer, int ProcessorId, ProcessorStates ProcessorState) //This is the constructor to create a processor
        {
            computer = Computer; //Set the computer to the one that created the processor
            cache = new Cache(this);  //Create the cache for this processor
            processorId = ProcessorId; //Set the processor id to the one that the controlling computer wants it to be
            traceData = new TraceData("p" + processorId + ".tr"); //Create the trace data for this processor
        }

        public void RunInstruction()
        { 
            if (traceData.shouldWrite[0]) //Check the trace data to see if the instruction is a processor write
            {
                Write();//Run the function for a processor write
            }
            else
            {
                Read();//Run the function for a processor read
            }
            traceData.PopQueue();//Pop the current instruction of the queue of instructions
        }
        
        void Write()
        {
            bool shared = computer.bus.HasData(this, traceData.tag[0], traceData.index[0], traceData.offset[0]); //Check to see if another processors cache has the data that is about to be written by the processor and store the value in a boolean variable
            cache.WriteData(traceData.timeStamp[0].ToString(), traceData.tag[0], traceData.index[0], traceData.offset[0]);//Write the data
            if (cache.ShouldSendSignal(true, traceData.tag[0], traceData.index[0], traceData.offset[0]))//if you should send a signal on the bus
            {
                computer.bus.SendSignal(cache.GetSignalToBeSent(true, traceData.tag[0], traceData.index[0], traceData.offset[0]), this);//Figure out what signal needs to be sent and send it on the bus
            }
            cache.ChangeState(true, true, shared, traceData.tag[0], traceData.index[0], traceData.offset[0]);//change the state of the cache line
        }

        void Read()
        {
            bool shared = computer.bus.HasData(this, traceData.tag[0], traceData.index[0], traceData.offset[0]);//Check to see if another processors cache has the data that is about to be read by the processor and store the value in a boolean variable
            if (!HasData(traceData.tag[0], traceData.index[0], traceData.offset[0]))
            {
                if (shared)
                {
                    cache.WriteData(computer.bus.GetData(this, traceData.tag[0], traceData.index[0], traceData.offset[0]), traceData.tag[0], traceData.index[0], traceData.offset[0]);
                }
                else
                {
                    cache.WriteData(traceData.timeStamp[0].ToString(), traceData.tag[0], traceData.index[0], traceData.offset[0]);
                }
            }
            if (cache.ShouldSendSignal(false, traceData.tag[0], traceData.index[0], traceData.offset[0]))
            {
                computer.bus.SendSignal(cache.GetSignalToBeSent(false, traceData.tag[0], traceData.index[0], traceData.offset[0]), this);
            }
            cache.ChangeState(true, false, shared, traceData.tag[0], traceData.index[0], traceData.offset[0]);
            //if(!successful){try to read from others catch
            //if(!successful){read from memory}}
            //if(we grabed the data from someone else){write to our own processor)
        }

        void SendSignal(BusSignal outputSignal)
        {
            computer.bus.SendSignal(outputSignal, this);
        }

        public void RecieveSignal(BusSignal signal)
        {
            ProcessSignal(signal);
        }

        void ProcessSignal(BusSignal signal)
        {
            ProcessorStates beforeChangeStateProcessorState = cache.cacheLines[Conversions.BinaryToDecimal(signal.index)].ways[cache.cacheLines[Conversions.BinaryToDecimal(signal.index)].GetWayNumber(signal.tag, signal.offset)].GetState();
            cache.ChangeState(false, false, false, signal.tag, signal.index, signal.offset, transaction: signal.transaction);
            ProcessorStates afterChangeStateProcessorState = cache.cacheLines[Conversions.BinaryToDecimal(signal.index)].ways[cache.cacheLines[Conversions.BinaryToDecimal(signal.index)].GetWayNumber(signal.tag, signal.offset)].GetState();
            if ((beforeChangeStateProcessorState != ProcessorStates.Invalid) && (afterChangeStateProcessorState == ProcessorStates.Invalid))
            {
                cache.cacheLines[Conversions.BinaryToDecimal(signal.index)].ways[cache.cacheLines[Conversions.BinaryToDecimal(signal.index)].GetWayNumber(signal.tag, signal.offset)].SetTag(null);
            }
            if ((signal.tag == "") || (signal.offset == "") || (signal.index == ""))
            {
                Console.Write("kl");
            }

        }



        public bool HasData(string tag, string index, string offset)
        {
            return cache.HasData(tag, index, offset);
        }


        public string GetData(string tag, string index, string offset)
        {
            return cache.GetData(tag, index, offset);
        }


    }
}
