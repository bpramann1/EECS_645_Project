using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_645_Project
{
    /* The conversion class is used to change the base representation
     * of a number */

    public static class Conversions
    {
	/* HexToBinary is a map from hexadecimal characters to their
	 * binary representation */
        public static string HexToBinary(char HexCharacter)
        {
            switch (HexCharacter)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'a':
                    return "1010";
                case 'b':
                    return "1011";
                case 'c':
                    return "1100";
                case 'd':
                    return "1101";
                case 'e':
                    return "1110";
                case 'f':
                    return "1111";
                default:
                    return "2222";
            }
        }

	/* BinaryToDecimal builds and returns an integer from a string
	 * representing a binary number */
        public static int BinaryToDecimal(string BinaryString)
        {
            int decimalValue = 0;
            for (int i = 0; i < BinaryString.Length; i++)
            {
                if (BinaryString[i] == '1')
                {
                    decimalValue += (int) Math.Pow(2, BinaryString.Length - i - 1);
                }else if(BinaryString[i] != '0')
                {
                    return -1;
                }
            }
            return decimalValue;
        }
    }

}
