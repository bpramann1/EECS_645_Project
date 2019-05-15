using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class LRU
    {
        List<int> wayNumber;
        public LRU(int NumberOfWays)
        {
            wayNumber = new List<int>();
            for (int i = 0; i < NumberOfWays; i++)
            {
                wayNumber.Add(i);
            }
        }
        
        public int GetLeastRecentlyUsedWayNumber()
        {
            return wayNumber[0];
        }

        public void UpdateLRU(int UsedWay)
        {
            wayNumber.Remove(UsedWay);
            wayNumber.Add(UsedWay);
        }
    }
}
