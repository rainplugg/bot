using System;
using System.Threading;

namespace Server
{
   internal class Program
   {
      static void Main(string[] args)
      {
         StartServer(80);
         Console.ReadKey();
      }

      private static void StartServer(int port)
      {
         try {
            new Thread(() => {
               Http.StartHTTPServer(port);
            }).Start();
         } catch (Exception ex) { Console.WriteLine(ex.Message); }
      }
   }
}
