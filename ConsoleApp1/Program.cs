using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
  public class a {
        public virtual string SS () => Console.WriteLine('a');
  }
    public class B : a {
        public override string SS () =>  Console.WriteLine('b');
    }
    class Program
    {

        static void Main(string[] args)
        {

        }
    }
}
