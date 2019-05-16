using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* Cache models a cache block */
    public class Cache
    {
        /* Processor associated with this cache block */
        public Processor controllingProcessor;

	/* Array representation of CacheLines */
        public CacheLine[] cacheLines;

	/* Default Cache Constructor */
        public Cache(Processor ControllingProcessor)
        {
            controllingProcessor = ControllingProcessor;
            cacheLines = new CacheLine[(int)Math.Pow(2, 8)];

            for (int i = 0; i < cacheLines.Length; i++)
            {
                cacheLines[i] = new CacheLine(this);
            }
        }

	/* WriteData converts a binary numbers into its decimal version in order to find
	 * the appropriate line index and then writes the passed data */
        public void WriteData(string inputData, string tag, string index, string offset)
        {
            cacheLines[Conversions.BinaryToDecimal(index)].WriteData(inputData, tag, offset);
        }

	/* ChangeState updates a cacheLine based on the MOESI protocol */
        public void ChangeState(bool DataTransactionIsInitiatedByCurrentProcessor, bool write, bool shared, string tag, string index, string offset, BusTransactions transaction = BusTransactions.ExclusiveRead)
        {
            cacheLines[Conversions.BinaryToDecimal(index)].ChangeState(tag, offset, DataTransactionIsInitiatedByCurrentProcessor, write, shared, transaction);
        }

	/* GetData returns the data stored in a single cacheLine */
        public string GetData(string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].GetData(tag, offset);
        }

	/* HasData returns a bool value representing if a specific cacheLine has
	 * data */
        public bool HasData(string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].HasTag(tag, offset);
        }

	/* ShouldSendSignal returns a bool value representing whether the MOESI protocol
	 * requires a signal be sent */
        public bool ShouldSendSignal(bool write, string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].ShouldSendSignal(write, tag, offset);
        }
	
	/* GetSignalToBeSent returns a BusSignal based on the state of a cache line */
        public BusSignal GetSignalToBeSent(bool write, string tag, string index, string offset)
        {
            return cacheLines[Conversions.BinaryToDecimal(index)].GetSignalToBeSent(write, tag, index, offset);
        }
    }
}
