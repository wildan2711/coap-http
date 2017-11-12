using System;
using System.Collections.Generic;
using System.Text;
using CoAP;
using CoAP.Util;
using CoAP.Server.Resources;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace coap
{
    class Coap
    {
        public static string CoapURI = "coap://192.168.43.100/monitor";
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        
        public static IWebHost BuildWebHost(string[] args) =>
            new WebHostBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();
        public class Startup {
            public void Configure(IApplicationBuilder app) {

                app.Run(async (context) => {
                    try {
                        Request request = new Request(Method.GET);
                        request.URI = new Uri(CoapURI);
                        request.Send();

                        Response coapResponse = request.WaitForResponse();
                        
                        if (coapResponse == null) {
                            Console.WriteLine("Request timed out");
                            byte[] response = Encoding.UTF8.GetBytes("Request timed out");

                            context.Response.StatusCode = 500;
                            await context.Response.Body.WriteAsync(response, 0, response.Length);       
                        }
                        else {
                            string data = Utils.ToString(coapResponse);
                            byte[] response = Encoding.UTF8.GetBytes(data);
                            
                            await context.Response.Body.WriteAsync(response, 0, response.Length);
                        }

                    } 
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                        byte[] response = Encoding.UTF8.GetBytes(e.Message);

                        context.Response.StatusCode = 500;
                        await context.Response.Body.WriteAsync(response, 0, response.Length);
                    }
                });
            }
        }
    }

}
