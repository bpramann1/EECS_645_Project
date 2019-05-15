using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class CacheData
    {
        public string data;
        CacheWay cacheWay;
        public CacheData(CacheWay CacheWay)
        {
            data = "";
            cacheWay = CacheWay;
        }



        public void WriteData(string inputData, string inputTag)
        {
            data = inputData;
        }

        public string GetData()
        {
            return data;
        }


    }
}
