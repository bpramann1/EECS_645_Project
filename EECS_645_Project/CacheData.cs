using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* CacheData holds a string representation of cache data */
    public class CacheData
    {
	/* String object representing cache data */
        public string data;

        CacheWay cacheWay;

	/* Default CacheData constructor */
        public CacheData(CacheWay CacheWay)
        {
            data = "";
            cacheWay = CacheWay;
        }

	/* WriteData stores new input data */
        public void WriteData(string inputData)
        {
            data = inputData;
        }
	
	/* GetData returns the string representation of a cache's data */
        public string GetData()
        {
            return data;
        }


    }
}
