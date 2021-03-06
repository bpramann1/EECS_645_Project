﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class CacheLine
    {
        public LRU LRU;
        public CacheWay[] ways;
        public Cache cache;
        public CacheLine(Cache Cache)
        {
            cache = Cache;
            ways = new CacheWay[2] { new CacheWay(this), new CacheWay(this) };
            LRU = new LRU(2);
        }

        public bool HasTag(string tag, string offset)
        {
            bool hasTag = false;
            foreach (CacheWay way in ways)
            {
                if (way.GetState()!= ProcessorStates.Invalid)
                {
                    hasTag = hasTag || (way.GetTag() == tag);
                }
            }
            return hasTag;
        }

        public bool ShouldSendSignal(bool write, string tag, string offset)
        {
                return ways[GetWayNumber(tag, offset)].ShouldSendSignal(write);
        }

        public BusSignal GetSignalToBeSent(bool write, string tag, string index, string offset)
        {
                return ways[GetWayNumber(tag, offset)].GetSignalToBeSent(write, tag, index, offset);
        }

        public int GetWayNumber(string tag, string offset)
        {
            for (int i = 0; i < ways.Length; i++)
            {
                if (ways[i].GetTag() == tag)
                {
                    return i;
                }
            }
            return -1;
        }

        public void WriteData(string inputData, string tag, string offset)
        {
            if (HasTag(tag, offset))
            {
                ways[GetWayNumber(tag, offset)].WriteData(inputData, tag, offset);
            }
            else
            {
                ways[LRU.GetLeastRecentlyUsedWayNumber()].WriteData(inputData, tag, offset);
                LRU.UpdateLRU(LRU.GetLeastRecentlyUsedWayNumber());
            }
        }

        public void ChangeState(string tag, string offset, bool DataTransactionIsInitiatedByCurrentProcessor, bool ProcessorRead = false, bool DataSharedByOtherProcessors = false, BusTransactions transaction = BusTransactions.ExclusiveRead)
        {
            ways[GetWayNumber(tag, offset)].ChangeState(DataTransactionIsInitiatedByCurrentProcessor, ProcessorRead, DataSharedByOtherProcessors, transaction);
        }

        public string GetData(string tag, string offset)
        {
                return ways[GetWayNumber(tag, offset)].GetData(offset);
        }
    }
}
