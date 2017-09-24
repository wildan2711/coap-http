using System;
using System.Collections.Generic;
using System.Text;
using CoAP.Server;
using CoAP.Server.Resources;
using Newtonsoft.Json;

namespace coap
{
    class Coap
    {
        public static SensorResult currentCondition;
        static void Main(string[] args)
        {
            CoapServer server = new CoapServer();

            server.Add(new SensorResource("monitor"));

            try
            {
                server.Start();

                Console.Write("CoAP server [{0}] is listening on", server.Config.Version);

                foreach (var item in server.EndPoints)
                {
                    Console.Write(" ");
                    Console.Write(item.LocalEndPoint);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            // server.Add(new SensorResource());
        }

        class SensorResource : Resource
        {
            public SensorResource(String name)
                : base(name)
            {

            }
            protected override void DoPut(CoapExchange exchange)
            {
                String payload = exchange.Request.PayloadString;
                SensorResult dict = JsonConvert.DeserializeObject<SensorResult>(payload);
                
                Console.WriteLine("id: {0}", dict.id);
                Console.WriteLine("suhu: {0}", dict.suhu);
                Console.WriteLine("lembab: {0}", dict.lembab);
                currentCondition = dict;
            }
        }

        public class SensorResult
        {
            public int id { get; set; }
            public int suhu { get; set; }
            public int lembab { get; set; }
            public SensorResult(int id, int suhu, int lembab)
            {
                this.id = id;
                this.suhu = suhu;
                this.lembab = lembab;
            }
        }
    }
}
