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
        public static string[] CoapServerIPs = {
            "127.0.0.1:5683",
            "127.0.0.1:5684",
            "127.0.0.1:5685"
        };
        public static string Endpoint = "/monitor";
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
                        int n;
                        string route = context.Request.Path.ToString().Substring(1);
                        bool isNumber = int.TryParse(route, out n);
                        if (isNumber) {
                            int id = Convert.ToInt32(route) - 1;
                            Request request = new Request(Method.GET);
                            request.URI = new Uri("coap://"+CoapServerIPs[id]+Endpoint);
                            request.Send();

                            Response coapResponse = request.WaitForResponse();
                            
                            if (coapResponse == null) {
                                Console.WriteLine("Request timed out");
                                byte[] response = Encoding.UTF8.GetBytes("{\"error\": \"Invalid device ID or device not connected\"");

                                context.Response.StatusCode = 500;
                                context.Response.ContentType = "application/json";
                                await context.Response.Body.WriteAsync(response, 0, response.Length);       
                            }
                            else {
                                context.Response.ContentType = "application/json";
                                await context.Response.Body.WriteAsync(coapResponse.Payload, 0, coapResponse.Payload.Length);
                            }
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
