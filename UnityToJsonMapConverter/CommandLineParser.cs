using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityToJsonMapConverter
{
    class CommandLineParser
    {
        public string UnityFilePath { get; private set; }

        public string JsonMapFilePath{ get; private set; }

        public CommandLineParser(string[] args)
        {
            if (args.Length != 2)
            {
                throw new InvalidOperationException("Usage is:\n UnityToJsonMapConverter.exe unityscene targetjsonfile");
            }

            UnityFilePath = args[0];
            JsonMapFilePath = args[1];
        }
    }
}
