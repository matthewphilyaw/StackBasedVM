using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackBasedVM
{
    public static class Assembler
    {
        private class Instruction
        {
            public int Opcode;
            public int NumArgs;
            
            public Instruction(int opcode, int numArgs)
            {
                Opcode = opcode;
                NumArgs = numArgs;
            }
        }

        private class Line
        {
            public Instruction Instruction;
            public string[] Tokens;
            public int Address;
        }

        private static Dictionary<string, Instruction> keywords = new Dictionary<string, Instruction> {
            { "iadd", new Instruction(1, 0) },
            { "isub", new Instruction(2, 0) },
            { "imul", new Instruction(3, 0) },
            { "ilt", new Instruction(4, 0) },
            { "ieq", new Instruction(5, 0) },
            { "br", new Instruction(6, 1) },
            { "brt", new Instruction(7, 1) },
            { "brf", new Instruction(8, 1) },
            { "iconst", new Instruction(9, 1) },
            { "load", new Instruction(10, 0) },
            { "gload", new Instruction(11, 1) },
            { "store", new Instruction(12, 0) },
            { "gstore", new Instruction(13, 1) },
            { "print", new Instruction(14, 0) },
            { "pop", new Instruction(15, 0) },
            { "call", new Instruction(16, 0) },
            { "ret", new Instruction(17, 0) },
            { "halt", new Instruction(18, 0) },
        };

        public static int[] assemble(string program)
        {
            List<int> code = new List<int>();

            List<Line> lines = new List<Line>();

            int currentAddr = -1;
            foreach (var line in program.Split(new[] { Environment.NewLine}, StringSplitOptions.None ))
            {
                if (String.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                {
                    continue;
                }

                currentAddr++;
                var tokens = line.Split(' ');

                if (tokens.Length < 1)
                {
                    throw new Exception("Invalid program");
                }

                var keyword = tokens[0];
                var instr = keywords[keyword.ToLower()];

                lines.Add(new Line()
                {
                    Instruction = instr,
                    Tokens = tokens.Skip(1).ToArray(), // throw away keyword
                    Address = currentAddr
                });

                // Each arg bumps the current addr
                currentAddr += instr.NumArgs;
            }


            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                var opCode = line.Instruction.Opcode;
                code.Add(opCode);

                for (int j = 0; j < line.Instruction.NumArgs; j++)
                {
                    var token = line.Tokens[j];
                    // If branch instruction, support relative or asbolutely line numbers
                    if (opCode == Bytecode.BR ||
                        opCode == Bytecode.BRF ||
                        opCode == Bytecode.BRT)
                    {
                        int addr = -1;
                        if (token[0] == '+')
                        {
                            int jump = int.Parse(token.Substring(1));
                            addr = calcJump(lines, i + jump);
                        }
                        else if (token[0] == '-')
                        {
                            int jump = int.Parse(token.Substring(1));
                            addr = calcJump(lines, i - jump);
                        }
                        else // absolute jump subtract one memory starts at 0
                        {
                            int jump = int.Parse(token);
                            addr = calcJump(lines, jump - 1);
                        }

                        code.Add(addr);
                    }
                    else
                    {
                        // All tokens are ints for now, since it's not a branch nothing special to do for now.
                        code.Add(int.Parse(token));
                    }
                }
            }

            return code.ToArray();
        }

        private static int calcJump(List<Line> lines, int jump)
        {
            if (jump < 0 || jump > lines.Count)
            {
                throw new Exception("Tried to jump outside of code space");
            }

            int addr = lines[jump].Address;
            return addr;
        }
    }
}
