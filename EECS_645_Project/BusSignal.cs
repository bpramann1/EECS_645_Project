using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class BusSignal
    {
        public BusTransactions transaction;
        public string tag;
        public string index;
        public string offset;
        public BusSignal(BusTransactions Transaction, string Tag = null, string Index = "", string Offset = "")
        {
            tag = Tag;
            index = Index;
            offset = Offset;
            transaction = Transaction;
        }
    }
}
