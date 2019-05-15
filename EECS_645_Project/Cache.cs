using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class Cache
    {
        public Processor controllingProcessor;
        public CacheLine[] cacheLines;
        public Cache(Processor ControllingProcessor)
        {
            controllingProcessor = ControllingProcessor;
            cacheLines = new CacheLine[(int)Math.Pow(2, 8)];
            for (int i = 0; i < cacheLines.Length; i++)
            {
                cacheLines[i] = new CacheLine(this);
            }
        }

        public void WriteData(string inputData, string tag, string index, string offset)
        {
            cacheLines[Conversions.BinaryToDecimal(index)].WriteData(inputData, tag, offset);
        }

        //public void Flush(string tag, string index, string offset)
        //{
        //    cacheLines[Conversions.BinaryToDecimal(index)].Flush(tag, offset);
        //}

        public bool IsFlushNeeded(BusSignal signal)
        {
            return cacheLines[Conversions.BinaryToDecimal(signal.index)].IsFlushNeeded(signal);
        }

        public void ChangeState(bool DataTransactionIsInitiatedByCurrentProcessor, bool write, bool shared, string tag, string index, string offset, BusTransactions transaction = BusTransactions.ExclusiveRead)
        {
            cacheLines[Conversions.BinaryToDecimal(index)].ChangeState(tag, offset, DataTransactionIsInitiatedByCurrentProcessor, write, shared, transaction);
        }

        public string GetData(string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].GetData(tag, offset);
        }

        public bool HasData(string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].HasTag(tag, offset);
        }

        public bool SharedData(string tag, string index, string offset)
        {          
            return cacheLines[Conversions.BinaryToDecimal(index)].SharedData(tag, offset);
        }

        public bool ShouldSendSignal(bool write, string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].ShouldSendSignal(write, tag, offset);
        }

        public BusSignal GetSignalToBeSent(bool write, string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].GetSignalToBeSent(write, tag, index, offset);
        }
    }
}
