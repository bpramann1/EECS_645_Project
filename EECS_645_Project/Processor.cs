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
            if (!HasData(traceData.tag[0], traceData.index[0], traceData.offset[0]))//Check to see if the current processor has the data
            {
                if (shared)//if other processors have the data
                {
                    cache.WriteData(computer.bus.GetData(this, traceData.tag[0], traceData.index[0], traceData.offset[0]), traceData.tag[0], traceData.index[0], traceData.offset[0]);//Get that data from other processors and write it
                }
                else
                {
                    cache.WriteData(traceData.timeStamp[0].ToString(), traceData.tag[0], traceData.index[0], traceData.offset[0]);//Write the time stamp as the data
                }
            }
            if (cache.ShouldSendSignal(false, traceData.tag[0], traceData.index[0], traceData.offset[0]))//If this processor should send a signal to other caches
            {
                computer.bus.SendSignal(cache.GetSignalToBeSent(false, traceData.tag[0], traceData.index[0], traceData.offset[0]), this);// Get the signal to be sent and send it on the bus
            }
            cache.ChangeState(true, false, shared, traceData.tag[0], traceData.index[0], traceData.offset[0]);//Change the state of the cache line on the current processor
        }

        void SendSignal(BusSignal outputSignal)
        {
            computer.bus.SendSignal(outputSignal, this);//Send a signal by first sending to the bus
        }

        public void RecieveSignal(BusSignal signal)
        {
            ProcessSignal(signal);//Once the signal has been recieved call the processSignal on that signal
        }

        void ProcessSignal(BusSignal signal)
        {
            cache.ChangeState(false, false, false, signal.tag, signal.index, signal.offset, transaction: signal.transaction);//Change the state of the current processor based off the input signal
        }



        public bool HasData(string tag, string index, string offset)
        {
            return cache.HasData(tag, index, offset);//Check to see if the cache has the data based on the index and tag and return the value as a boolean
        }


        public string GetData(string tag, string index, string offset)
        {
            return cache.GetData(tag, index, offset); //Get the data from the cache and return it as a string.
        }


    }
}
