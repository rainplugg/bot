using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
   public class Http
   {
      public static HttpListener listener;
      public static string endPoint = string.Empty;
      public static async void StartHTTPServer(int port)
      {
         try {
            listener = new HttpListener();
            listener.Prefixes.Add("http://+:" + port + "/");
            listener.Start();
            await Task.Run(() => StartServer());
         } catch (Exception ex) { Console.WriteLine(ex.Message); }
      }

      /// <summary>
      /// Обработчик поступающих команд по локальной сети
      /// </summary>
      /// <returns></returns>
      public static async Task StartServer()
      {
         try {
            while (true) {
               HttpListenerContext context = await listener.GetContextAsync();
               HttpListenerRequest request = context.Request;
               HttpListenerResponse response = context.Response;
               context.Response.StatusCode = 200;
               context.Response.StatusDescription = "OK";

               endPoint = request.RemoteEndPoint.Address.ToString();
               try {
                  byte[] buffer = Encoding.UTF8.GetBytes("OK");
                  response.ContentLength64 = buffer.Length;
                  Stream output = response.OutputStream;
                  output.Write(buffer, 0, buffer.Length);
                  output.Close();
               } catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
         } catch (Exception ex) { Console.WriteLine(ex.Message); }
      }
   }
}
