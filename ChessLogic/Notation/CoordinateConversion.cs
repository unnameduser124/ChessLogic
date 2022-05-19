using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.Notation
{
    public abstract class CoordinateConversion
    {
        static public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        static public string convertCords(int[] cord)
        {
            return (char)(cord[0] + 97) + (cord[1] + 1).ToString();
        }

        static public char convertCordToChar(int cord)
        {
            return (char)(cord + 97);
        }

        static public int convertCharToCord(char cord)
        {
            return cord - 97;
        }

        static public int[] cordToIntArray(string cord)
        {
            var position = new int[2];
            position[0] = cord[0] - 97;
            position[1] = cord[1] - '0' - 1;

            return position;
        }
    }
}
