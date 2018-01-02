using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackBasedVM
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = File.ReadAllText("program.asm");
            var assembled = Assembler.assemble(program);
            var vm = new Vm(assembled, 0, 1);

            vm.Exec();
        }
    }
}
