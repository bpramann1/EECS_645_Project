using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class CacheWay
    {
        public CacheLine cacheLine;
        public CacheData[] cacheData; 
        public CacheWay(CacheLine CacheLine)
        {
            cacheLine = CacheLine;
            cacheData = new CacheData[64];
            for (int i = 0; i < cacheData.Length; i++)
            {
                cacheData[i] = new CacheData(this);
            }
        }



        public string GetTag(string offset)
        {
            return cacheData[Conversions.BinaryToDecimal(offset)].GetTag();
        }

        public ProcessorStates GetState(string offset)
        {
            return cacheData[Conversions.BinaryToDecimal(offset)].processorState;
        }

        public bool ShouldSendSignal(bool write, string offset)
        {
            return cacheData[Conversions.BinaryToDecimal(offset)].ShouldSendSignal(write);
        }

        public bool IsFlushNeeded(BusSignal signal)
        {
            return cacheData[Conversions.BinaryToDecimal(signal.offset)].IsFlushNeeded(signal);
        }

        //public void Flush(string offset)
        //{
        //    cacheData[offset].Flush();
        //}
        
        public BusSignal GetSignalToBeSent(bool write, string tag, string index, string offset)
        {
            return cacheData[Conversions.BinaryToDecimal(offset)].GetSignalToBeSent(write, tag, index, offset);
        }

        public void WriteData(string inputData, string inputTag, string offset)
        {
            cacheData[Conversions.BinaryToDecimal(offset)].WriteData(inputData, inputTag);
        }

        public void SetTag(string inputTag, string offset)
        {
            cacheData[Conversions.BinaryToDecimal(offset)].SetTag(inputTag);
        }

        public string GetData(string offset)
        {
            return cacheData[Conversions.BinaryToDecimal(offset)].GetData();
        }

        public void ChangeState(string offset, bool DataTransactionIsInitiatedByCurrentProcessor, bool ProcessorRead = false, bool DataSharedByOtherProcessors = false, BusTransactions transaction = BusTransactions.ExclusiveRead)
        {
            cacheData[Conversions.BinaryToDecimal(offset)].ChangeState(DataTransactionIsInitiatedByCurrentProcessor, ProcessorRead, DataSharedByOtherProcessors, transaction);
        }

    }
}
