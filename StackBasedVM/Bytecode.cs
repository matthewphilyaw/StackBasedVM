using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackBasedVM
{
    public static class Bytecode
    {
        public static int IADD   = 1;
        public static int ISUB   = 2;
        public static int IMUL   = 3;
        public static int ILT    = 4;
        public static int IEQ    = 5;
        public static int BR     = 6;
        public static int BRT    = 7;
        public static int BRF    = 8;
        public static int ICONST = 9;
        public static int LOAD   = 10;
        public static int GLOAD  = 11;
        public static int STORE  = 12;
        public static int GSTORE = 13;
        public static int PRINT  = 14;
        public static int POP    = 15;
        public static int CALL   = 16;
        public static int RET    = 17;
        public static int HALT   = 18;
    }
}
