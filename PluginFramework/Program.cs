using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Manager.DirectoryManageOption option = Manager.DirectoryManageOption.Classified | Manager.DirectoryManageOption.Root;

            Console.WriteLine(Enum.GetName(typeof(Manager.DirectoryManageOption), (int) 3));

            Console.ReadKey();
        }
    }

}
