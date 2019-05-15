using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class CacheData
    {
        public string tag;
        public string data;
        public ProcessorStates processorState;
        CacheWay cacheWay;
        public CacheData(CacheWay CacheWay)
        {
            processorState = ProcessorStates.Invalid;
            tag = null;
            data = "";
            cacheWay = CacheWay;
        }

        void ChangeData(BusTransactions transaction)
        {
            switch (processorState)
            {
                case ProcessorStates.Invalid:
                    if (transaction == BusTransactions.ExclusiveRead)
                    {

                    }
                    break;
                case ProcessorStates.Exclusive:
                    break;
                case ProcessorStates.Modified:
                    break;
                case ProcessorStates.Owner:
                    break;
                case ProcessorStates.Shared:
                    break;
                default:
                    break;
            }
        }


        public string GetTag()
        {
            return tag;
        }

        public ProcessorStates GetState()
        {
            return processorState;
        }

        public bool ShouldSendSignal(bool write)
        {
            bool shouldSendSignal = false;
            if (write)
            {
                switch (processorState)
                {
                    case ProcessorStates.Invalid:
                    case ProcessorStates.Owner:
                    case ProcessorStates.Shared:
                        shouldSendSignal = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (processorState)
                {
                    case ProcessorStates.Invalid:
                        shouldSendSignal = true;
                        break;
                    default:
                        break;
                }
            }
            return shouldSendSignal;
        }

        public bool IsFlushNeeded(BusSignal signal)
        {
            bool isFlushNeeded = false;
            switch (processorState)
            {
                case ProcessorStates.Exclusive:
                    isFlushNeeded = isFlushNeeded || (signal.transaction == BusTransactions.ExclusiveRead) || (signal.transaction == BusTransactions.Read);
                    break;
                case ProcessorStates.Modified:
                    isFlushNeeded = isFlushNeeded || (signal.transaction == BusTransactions.ExclusiveRead) || (signal.transaction == BusTransactions.Read);
                    break;
                case ProcessorStates.Owner:
                    isFlushNeeded = isFlushNeeded || (signal.transaction == BusTransactions.ExclusiveRead) || (signal.transaction == BusTransactions.Read);
                    break;
                default:
                    break;
            }
            return isFlushNeeded;
        }

        public BusSignal GetSignalToBeSent(bool write, string tag, string index, string offset)
        {
            BusTransactions transaction = BusTransactions.Flush;
            if (write)
            {
                switch (processorState)
                {
                    case ProcessorStates.Invalid:
                        transaction = BusTransactions.ExclusiveRead;
                        break;
                    case ProcessorStates.Owner:
                        transaction = BusTransactions.Upgrade;
                        break;
                    case ProcessorStates.Shared:
                        transaction = BusTransactions.Upgrade;
                        break;
                }
            }
            else
            {
                switch (processorState)
                {
                    case ProcessorStates.Invalid:
                        transaction = BusTransactions.Read;
                        break;
                }
            }
            if (transaction == BusTransactions.Flush)
            {
                Console.Write("Trying to send a signal without a transaction");
            }
            return new BusSignal(transaction, tag, index, offset);
        }

        public void WriteData(string inputData, string inputTag)
        {
            data = inputData;
            tag = inputTag;
            processorState = ProcessorStates.Invalid;
        }

        public void SetTag(string inputTag)
        {
            tag = inputTag;
        }

        //public void Flush()
        //{
        //    switch (processorState)
        //    {
        //        case ProcessorStates.Invalid:

        //            break;
        //        case ProcessorStates.Exclusive:
        //            break;
        //        case ProcessorStates.Modified:
        //            break;
        //        case ProcessorStates.Owner:
        //            break;
        //        case ProcessorStates.Shared:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public string GetData()
        {
            return data;
        }

        public void ChangeState(bool DataTransactionIsInitiatedByCurrentProcessor, bool ProcessorRead = false, bool DataSharedByOtherProcessors = false, BusTransactions transaction = BusTransactions.ExclusiveRead)
        {
            switch (processorState)
            {
                case ProcessorStates.Invalid:
                    if (DataTransactionIsInitiatedByCurrentProcessor)
                    {
                        if (ProcessorRead)
                        {
                            if (DataSharedByOtherProcessors)
                            {
                                processorState = ProcessorStates.Shared;
                            }
                            else
                            {
                                processorState = ProcessorStates.Exclusive;
                            }
                        }
                        else
                        {
                            processorState = ProcessorStates.Modified;
                        }
                    }
                    else
                    {
                        //No change in state
                    }
                    break;
                case ProcessorStates.Exclusive:
                    if (DataTransactionIsInitiatedByCurrentProcessor)
                    {
                        if (ProcessorRead)
                        {
                            //Leave the processor state the same
                        }
                        else
                        {
                            processorState = ProcessorStates.Modified;
                        }
                    }
                    else
                    {
                        switch (transaction)
                        {
                            case BusTransactions.Read:
                                processorState = ProcessorStates.Shared;
                                break;
                            case BusTransactions.ExclusiveRead:
                                cacheWay.cacheLine.cache.controllingProcessor.invalidationNumber[2]++;
                                processorState = ProcessorStates.Invalid;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case ProcessorStates.Modified:
                    if (DataTransactionIsInitiatedByCurrentProcessor)
                    {
                        if (ProcessorRead)
                        {
                            //Leave the processor state the same
                        }
                        else
                        {
                            //Leave the processor state the same
                        }
                    }
                    else
                    {
                        switch (transaction)
                        {
                            case BusTransactions.Read:
                                processorState = ProcessorStates.Owner;
                                break;
                            case BusTransactions.ExclusiveRead:
                                cacheWay.cacheLine.cache.controllingProcessor.invalidationNumber[0]++;
                                processorState = ProcessorStates.Invalid;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case ProcessorStates.Owner:
                    if (DataTransactionIsInitiatedByCurrentProcessor)
                    {
                        if (ProcessorRead)
                        {
                            //Leave the processor state the same
                        }
                        else
                        {
                            processorState = ProcessorStates.Modified;
                        }
                    }
                    else
                    {
                        switch (transaction)
                        {
                            case BusTransactions.Read:
                                //Stay in the same state
                                break;
                            case BusTransactions.ExclusiveRead:
                            case BusTransactions.Upgrade:
                                cacheWay.cacheLine.cache.controllingProcessor.invalidationNumber[1]++;
                                processorState = ProcessorStates.Invalid;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case ProcessorStates.Shared:
                    if (DataTransactionIsInitiatedByCurrentProcessor)
                    {
                        if (ProcessorRead)
                        {
                            //Leave the processor state the same
                        }
                        else
                        {
                            processorState = ProcessorStates.Modified;
                        }
                    }
                    else
                    {
                        switch (transaction)
                        {
                            case BusTransactions.Read:
                                //Stay in the same state
                                break;
                            case BusTransactions.ExclusiveRead:
                            case BusTransactions.Upgrade:
                                cacheWay.cacheLine.cache.controllingProcessor.invalidationNumber[3]++;
                                processorState = ProcessorStates.Invalid;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    Console.Write("Processor state is not defined!");
                    break;
            }
        }
    }
}
