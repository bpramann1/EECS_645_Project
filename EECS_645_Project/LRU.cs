using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* LRU models a structure used to determine
     * the least recently used page */
    public class LRU
    {
	/* wayNumber is a list of integers used
	 * to store the path history of ways taken, 
	 * pushing the LRU towards the wrong
	 * of the list */
        List<int> wayNumber;

	/* Default LRU constructor */
        public LRU(int NumberOfWays)
        {
            wayNumber = new List<int>();
            for (int i = 0; i < NumberOfWays; i++)
            {
                wayNumber.Add(i);
            }
        }
        
	/* GetLeastRecentlyUsedWayNumber returns the first element
	 * in our list, which represents the LRU way number */
        public int GetLeastRecentlyUsedWayNumber()
        {
            return wayNumber[0];
        }

	/* UpdateLRU replaced a wayNumber from our list
	 * of values */
        public void UpdateLRU(int UsedWay)
        {
            wayNumber.Remove(UsedWay);
            wayNumber.Add(UsedWay);
        }
    }
}
