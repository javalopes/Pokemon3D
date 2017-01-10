using System;

namespace Pokemon3D.Scripting
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new ScriptProcessor();

            while (true)
            {
                Console.Write("< ");
                string input = Console.ReadLine();
                
                var result = processor.Run(input);

                if (result.TypeOf() == Types.SObject.LITERAL_TYPE_ERROR)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Types.SError error = (Types.SError)result;

                    Console.WriteLine("x " + ((Types.SString)error.Members["type"].Data).Value + ": " +
                                      ((Types.SString)error.Members["message"].Data).Value);

                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("> " + result.ToScriptSource());
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
