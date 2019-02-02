using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeanHashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            SeanHashTable table = new SeanHashTable();

            table.Add("pants");
            table.Add("dog");
            table.Add("Greg");
            table.Add("Daniel Avidan");
            table.Add("Greg Smith");
            table.Add("");

            bool nameFound = table.Search("Greg");
            nameFound = table.Search("weggie");
            nameFound = table.Search("Daniel Avidan");

            table.Delete("Greg");
            table.Delete("wow");

            nameFound = table.Search("Greg");

            string name = "human ";
            for(int x = 0; x < 5000; x++)
            {
                name = "human " + x.ToString();
                table.Add(name);
            }

            nameFound = table.Search("pants");
        }
    }
}
