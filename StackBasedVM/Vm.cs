using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackBasedVM
{
    public class Vm
    {
        private const int DEFAULT_STACK_SIZE = 1000;
        private const int FALSE              = 0;
        private const int TRUE               = 1;

        private int[] code;
        private int[] data;
        private int[] stack;

        private int sp; // stack pointer
        private int ip; // instruction pointer
        private int fp; // frame pointer

        private int startIp = 0;

        private Dictionary<int, Action> opcodes;
        private void initOpcodes()
        {
            opcodes = new Dictionary<int, Action>()
            {
                { Bytecode.IADD    , iadd   },
                { Bytecode.ISUB    , isub   },
                { Bytecode.IMUL    , imul   },
                { Bytecode.ILT     , ilt    },
                { Bytecode.IEQ     , ieq    },
                { Bytecode.BR      , br     },
                { Bytecode.BRT     , brt    },
                { Bytecode.BRF     , brf    },
                { Bytecode.ICONST  , iconst },
                { Bytecode.LOAD    , this.NotImplemented },
                { Bytecode.GLOAD   , this.NotImplemented },
                { Bytecode.STORE   , this.NotImplemented },
                { Bytecode.GSTORE  , this.NotImplemented },
                { Bytecode.PRINT   , print  },
                { Bytecode.POP     , this.NotImplemented },
                { Bytecode.CALL    , this.NotImplemented },
                { Bytecode.RET     , this.NotImplemented },
                { Bytecode.HALT    , this.NotImplemented },
            };
        }

        public Vm(int[] code, int startIp, int dataSize)
        {
            initOpcodes();

            this.code = code;
            this.data = new int[dataSize];
            this.stack = new int[DEFAULT_STACK_SIZE];

            this.startIp = startIp;
            this.sp = -1;
        }

        public void Exec()
        {
            ip = this.startIp;
            this.cpu();
        }

        private void cpu()
        {
            int opcode = this.code[ip];
            while(opcode != Bytecode.HALT && ip < code.Length)
            {
                ip++;
                // Will change the ip pointer most likely when the function executes
                opcodes[opcode]();
                opcode = code[ip];
            }
        }

        private void iadd()
        {
            int a, b = 0;

            b = stack[sp--];
            a = stack[sp--];

            stack[++sp] = a + b;
        }

        private void isub()
        {
            int a, b = 0;

            b = stack[sp--];
            a = stack[sp--];

            stack[++sp] = a - b;
        }

        private void imul()
        {
            int a, b = 0;

            b = stack[sp--];
            a = stack[sp--];

            stack[++sp] = a * b;
        }

        private void ilt()
        {
            int a, b = 0;

            b = stack[sp--];
            a = stack[sp--];

            stack[++sp] = a < b ? TRUE : FALSE;
        }

        private void ieq()
        {
            int a, b = 0;

            b = stack[sp--];
            a = stack[sp--];

            stack[++sp] = a == b ? TRUE : FALSE;
        }

        private void br()
        {
            int addr = code[ip];
            ip = addr;
        }

        private void brt()
        {
            int addr = code[ip];

            int result = stack[sp--];
            if (result == TRUE)
            {
                ip = addr; 
            }
            else
            {
                // continue onto the next instruction since there is no branch
                ip++;
            }
        }

        private void brf()
        {
            int addr = code[ip];

            int result = stack[sp--];
            if (result == FALSE)
            {
                ip = addr; 
            }
            else
            {
                // continue onto the next instruction since there is no branch
                ip++;
            }
        }

        private void iconst()
        {
            // grab code value and increment ip after
            int val = code[ip++];
            stack[++sp] = val;
        }

        private void print()
        {
            int val = stack[sp--];
            Console.WriteLine(val);
        }

        private void NotImplemented()
        {
            // IP always points to the next instruction when function is called, so IP - 1 is the current opcode
            // Next instruction is usually the arguments to the opcode.
            Console.WriteLine($"{ip - 1} Not Implemented");
        }
    }
}
