using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyMCP
{
    class Output
    {
        public static void Write(string text)
        {
            try
            {
                Console.Write(DateTime.Now.ToString() + ": " + text);
            }
            catch (Exception ex) 
            { Console.WriteLine(ex.ToString());

            System.Threading.Thread.Sleep(5000);
            }
        }

        public static void WriteLine(string text)
        {
            Write(text + "\n");
        }
    }
}
