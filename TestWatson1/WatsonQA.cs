using System;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace TestWatson1
{
    class WatsonQA
    {
        private string url;
        private string uid;
        private string pwd;

        public WatsonQA(string corpus, string baseURL, string uid, string pwd)
        {
            this.url = baseURL + "/v1/question/" + corpus;
            this.uid = uid;
            this.pwd = pwd;

            Console.WriteLine("url:" + this.url);
            Console.WriteLine("uid:" + this.uid);
            Console.WriteLine("pwd:" + this.pwd);
        }

        public string AskQuestion(string question)
        {
            string answers = null;
            string data = "{\"question\" : { \"evidenceRequest\":{\"profile\":\"NO\"},\"questionText\" : \"" + question + "\"}}";

            var qaCall = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                string auth = string.Format("{0}:{1}", this.uid, this.pwd);
                string auth64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(auth));
                string credentials = string.Format("{0} {1}", "Basic", auth64);

                qaCall.Headers[HttpRequestHeader.Authorization] = credentials;
                qaCall.Method = "POST";
                qaCall.Accept = "application/json";
                qaCall.ContentType = "application/json";
                qaCall.Headers["X-SyncTimeOut"] = "30";

                var encoding = new UTF8Encoding();
                var payload = Encoding.GetEncoding("iso-8859-1").GetBytes(data);
                qaCall.ContentLength = payload.Length;
                using (var callStream = qaCall.GetRequestStream())
                {
                    callStream.Write(payload, 0, payload.Length);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("error:" + e.Message);
                Console.ReadKey();
            }

            try
            {
                WebResponse qaResponse = qaCall.GetResponse();
                Stream requestStream = qaResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(requestStream);
                answers = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (System.Net.WebException e)
            {
                Console.Out.WriteLine("errors:" + e.Message);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("error:" + e.Message);
                Console.ReadKey();
            }

            return answers;
        }


        

    }
}
