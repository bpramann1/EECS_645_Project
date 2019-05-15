using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    public class TraceData
    {
        string filename;
        public List<int> timeStamp;
        public List<bool> shouldWrite;
        public List<string> memoryAddress;
        public List<string> binaryMemoryAddress;
        public List<string> tag;
        public List<string> index;
        public List<string> offset;
        public TraceData(string Filename)
        {
            filename = Filename;
            timeStamp = new List<int>();
            shouldWrite = new List<bool>();
            memoryAddress = new List<string>();
            binaryMemoryAddress = new List<string>();
            tag = new List<string>();
            index = new List<string>();
            offset = new List<string>();
            ExtractData();
        }

        public void PopQueue()
        {
            timeStamp.RemoveAt(0);         
            shouldWrite.RemoveAt(0);
            memoryAddress.RemoveAt(0);
            binaryMemoryAddress.RemoveAt(0);
            tag.RemoveAt(0);
            index.RemoveAt(0);
            offset.RemoveAt(0);
        }

        void ExtractData()
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString() + "\\" + filename);          
            foreach (string line in lines)
            {
                timeStamp.Add(int.Parse(line.Split(' ')[0]));
                shouldWrite.Add((int.Parse(line.Split(' ')[1]) == 1));
                memoryAddress.Add(line.Split(' ')[2]);                      
            }

            bool pastX;
            string currentBinaryMemoryAddress;
            for (int i = 0; i < memoryAddress.Count; i++)
            {
                pastX = false;
                currentBinaryMemoryAddress = "";
                for (int j = 0; j < memoryAddress[i].Length; j++)
                {
                    if (pastX)
                    {
                        currentBinaryMemoryAddress = currentBinaryMemoryAddress + Conversions.HexToBinary(memoryAddress[i][j]);
                    }
                    else
                    {
                        pastX = pastX || (memoryAddress[i][j] == 'x');
                    }
                }
                binaryMemoryAddress.Add(currentBinaryMemoryAddress);
                tag.Add(currentBinaryMemoryAddress.Substring(0, 18));
                index.Add(currentBinaryMemoryAddress.Substring(18, 8));
                offset.Add(currentBinaryMemoryAddress.Substring(26, 6));
            }

        }




        

    }
}
