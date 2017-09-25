using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace TestWatson1
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*WatsonQA travel = new WatsonQA("travel", 
                                           "https://gateway.watsonplatform.net/qagw/service", 
                                           "[enter uid here]", 
                                           "[enter pwd here]");

           var answers =  travel.AskQuestion("[enter question here]");
           Console.WriteLine(answers);
           Console.ReadKey();*/

            TalkToWatson().Wait();


        }


        public Program() { }

        public static async Task TalkToWatson()
        {
            var baseurl = "https://gateway.watsonplatform.net/conversation/api";
            //var workspace = "4e612f8f-588f-4ba3-8b77-d7c3db6ec47b";
            var workspace = "f8a11222-610b-4974-9ee7-db35ff9c10ee";
            //var username = "62b81250-24b9-430f-8a41-dd73bd2f519a";
            var username = "c3b01d3d-50fb-4f80-b889-d5b53b49bffd";

            //var password = "A2NVe73JNdKL";
            var password = "rOEOXpytY50p";

            var context = null as object;
            //var input = 
            Console.ReadLine();
            var message = new { input = new { text = "" }, context };


            System.Threading.CancellationToken cantoken = new System.Threading.CancellationToken();

            var resp = await baseurl
                .AppendPathSegments("v1", "workspaces", workspace, "message")
                .SetQueryParam("version", "2016-11-21")
                .WithBasicAuth(username, password)
                .AllowAnyHttpStatus()
                .PostJsonAsync(message, cantoken);

            var json = await resp.Content.ReadAsStringAsync();

            var answer = new
            {
                intents = default(object),
                entities = default(object),
                input = default(object),
                output = new
                {
                    text = default(string[])
                },
                context = default(object)
            };

            answer = JsonConvert.DeserializeAnonymousType(json, answer);
            var elcontextoantiguo = answer.context;

            var output = answer?.output?.text?.Aggregate(
                new StringBuilder(),
                (sb, l) => sb.AppendLine(l),
                sb => sb.ToString());

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{resp.StatusCode}: {output}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(json);
            Console.ResetColor();

            //input = Console.ReadLine();
            message = new { input = new { text = Console.ReadLine() }, context = elcontextoantiguo };

            resp = await baseurl
                .AppendPathSegments("v1", "workspaces", workspace, "message")
                .SetQueryParam("version", "2016-11-21")
                .WithBasicAuth(username, password)
                .AllowAnyHttpStatus()
                .PostJsonAsync(message);

            json = await resp.Content.ReadAsStringAsync();

            answer = new
            {
                intents = default(object),
                entities = default(object),
                input = default(object),
                output = new
                {
                    text = default(string[])
                },
                context = default(object)
            };

            answer = JsonConvert.DeserializeAnonymousType(json, answer);

            output = answer?.output?.text?.Aggregate(
                new StringBuilder(),
                (sb, l) => sb.AppendLine(l),
                sb => sb.ToString());

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{resp.StatusCode}: {output}");
            Console.WriteLine($"{answer.intents}: {output}");


            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(json);
            Console.ResetColor();
            Console.ReadKey();
        }



        public static async Task enviarMensaje(string mensaje)
        {

            var baseurl = "https://gateway.watsonplatform.net/conversation/api";
            //var workspace = "4e612f8f-588f-4ba3-8b77-d7c3db6ec47b";
            var workspace = "f8a11222-610b-4974-9ee7-db35ff9c10ee";
            //var username = "62b81250-24b9-430f-8a41-dd73bd2f519a";
            var username = "c3b01d3d-50fb-4f80-b889-d5b53b49bffd";

            //var password = "A2NVe73JNdKL";
            var password = "rOEOXpytY50p";

            var context = null as object;

            var message = new { input = new { text = mensaje }, context };


            var resp = await baseurl
                .AppendPathSegments("v1", "workspaces", workspace, "message")
                .SetQueryParam("version", "2016-11-21")
                .WithBasicAuth(username, password)
                .AllowAnyHttpStatus()
                .PostJsonAsync(message);

            var json = await resp.Content.ReadAsStringAsync();

            var answer = new
            {
                intents = default(object),
                entities = default(object),
                input = default(object),
                output = new
                {
                    text = default(string[])
                },
                context = default(object)
            };

            answer = JsonConvert.DeserializeAnonymousType(json, answer);
            var elcontextoantiguo = answer.context;

            var output = answer?.output?.text?.Aggregate(
                new StringBuilder(),
                (sb, l) => sb.AppendLine(l),
                sb => sb.ToString());

            mensaje = output;

        }

        public string mensaje
        {
            set; get;

        }
    }
}
