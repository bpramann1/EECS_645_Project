using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class Processor
    {
        public Computer computer;

        public Cache cache;
        public TraceData traceData;
        int processorId;
        int missNumber;
        public int[] invalidationNumber = new int[4];
        //List<string> tags;
        public List<string> indexs;
        public List<string> offsets;
        public int numberOfUniqueInstructions;
        public int numberInStateM = 0;
        public int numberInStateO = 0;
        public int numberInStateE = 0;
        public int numberInStateS = 0;
        public int numberInStateI = 0;
        public Processor(Computer Computer, int ProcessorId, ProcessorStates ProcessorState)
        {
            computer = Computer;
            missNumber = 0;
            cache = new Cache(this);
            processorId = ProcessorId;
            traceData = new TraceData("p" + processorId + ".tr");
            //tags = new List<string>();
            indexs = new List<string>();
            offsets = new List<string>();
        }

        public void RunInstruction()
        {
            if (!((indexs.Contains(traceData.index[0]))&&(offsets.Contains(traceData.offset[0]))))
            {
                numberOfUniqueInstructions++;
                if (!indexs.Contains(traceData.index[0]))
                {
                    indexs.Add(traceData.index[0]);
                }
                if (!offsets.Contains(traceData.offset[0]))
                {
                    offsets.Add(traceData.offset[0]);
                }
            }

            if (traceData.shouldWrite[0])
            {
                Write();
            }
            else
            {
                Read();
            }
            traceData.PopQueue();
        }
        
        void Write()
        {
            //bool shared = SharedData(traceData.tag[0], traceData.index[0], traceData.offset[0]);
            cache.WriteData(traceData.timeStamp[0].ToString(), traceData.tag[0], traceData.index[0], traceData.offset[0]);
            //write data to our cache
            if (cache.ShouldSendSignal(true, traceData.tag[0], traceData.index[0], traceData.offset[0]))
            {
                computer.bus.SendSignal(cache.GetSignalToBeSent(true, traceData.tag[0], traceData.index[0], traceData.offset[0]), this);
            }
            cache.ChangeState(true, true, HasData(traceData.tag[0], traceData.index[0], traceData.offset[0]), traceData.tag[0], traceData.index[0], traceData.offset[0]);
            //change state/ invalidate other caches
        }

        void Read()
        {
            //Try to read from this processors own catch
            //bool shared = SharedData(traceData.tag[0], traceData.index[0], traceData.offset[0]);
            //bool ShouldSend
            if (!HasData(traceData.tag[0], traceData.index[0], traceData.offset[0]))
            {
                if (computer.bus.HasData(this, traceData.tag[0], traceData.index[0], traceData.offset[0]))
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
            cache.ChangeState(true, false, HasData(traceData.tag[0], traceData.index[0], traceData.offset[0]), traceData.tag[0], traceData.index[0], traceData.offset[0]);
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
            //if (signal.transaction != BusTransactions.Flush)
            //{
            //    if (IsFlushNeeded(signal))
            //    {
                    cache.ChangeState(false, false, false, signal.tag, signal.index, signal.offset, transaction: signal.transaction);
                    if((signal.tag == "") || (signal.offset == "") || (signal.index == ""))
                    {
                        Console.Write("kl");
                    }
 //                   //ProcessSignal(new BusSignal(BusTransactions.Flush, signal.tag, signal.index, signal.offset));
 //               }
 //               else
 //               {
 //                   cache.ChangeState(false, false, false, signal.tag, signal.index, signal.offset, signal.transaction);
 //               }

 //           }
 //           else
 //           {
 ////               cache.Flush(signal.tag, signal.index, signal.offset);
 //           }

        }

        bool IsFlushNeeded(BusSignal signal)
        {
            return cache.IsFlushNeeded(signal);
        }



        public bool HasData(string tag, string index, string offset)
        {
            return cache.HasData(tag, index, offset);
        }

        //public bool SharedData(string tag, string index, string offset)
        //{
        //    return cache.SharedData(tag, index, offset);
        //}

        public string GetData(string tag, string index, string offset)
        {
            return cache.GetData(tag, index, offset);
        }


    }
}
