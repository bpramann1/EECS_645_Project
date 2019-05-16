using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Namespace */
namespace EECS_645_Project
{
    public class BusSignal
    {
	 /* Bus Transaction is used to represent the MOESI cache transaction */
        public BusTransactions transaction;

	/* A string representation of a cache tag, index, and offset */
        public string tag; 
        public string index;
        public string offset;

	/* BusSignal is used to pass messages around that represent bus requests */
        public BusSignal(BusTransactions Transaction, string Tag = null, string Index = "", string Offset = "")
        {
            tag = Tag;
            index = Index;
            offset = Offset;
            transaction = Transaction;
        }
    }
}
