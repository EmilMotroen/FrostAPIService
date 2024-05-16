using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace FrostAPIService
{
    internal class Program
    {
        // 6ca1f2b5-7b4b-4e6f-abe8-d439aa6b0c1c - ClientID
        private readonly static string sisteSyvDager = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        static async Task Main()
        {
            try
            {
                string client_id = "6ca1f2b5-7b4b-4e6f-abe8-d439aa6b0c1c";
                string url = $"https://frost.met.no/observations/v0.jsonld?sources=SN27160&referencetime=R7%2F{sisteSyvDager}T12%2F{sisteSyvDager}T12%2FP1D&elements=air_temperature";
                Uri uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.Accept = "application/json";
                string authInfo = client_id + ":";
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    request.Headers["Authorization"] = "Basic " + authInfo;
                using (StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    JObject obj = JObject.Parse(result);
                    JArray data = (JArray)obj["data"];
                    JArray observations;
                    Console.WriteLine(data);
                }

            } catch (Exception e)
            {
                Console.WriteLine($"Failed retrieving data: \n{e}");
            }




            ////JWT:
            //var myToken = "6ca1f2b5-7b4b-4e6f-abe8-d439aa6b0c1c";
            //var authenticator = new JwtAuthenticator(myToken);
            //var options = new RestClientOptions($"https://frost.met.no/observations/v0.jsonld?sources=SN27160&referencetime=R7%2F{sisteSyvDager}T12%2F{sisteSyvDager}T12%2FP1D&elements=air_temperature")
            //{
            //    Authenticator = authenticator
            //};

            //var jwtClient = new RestClient(options);

            //Console.WriteLine(jwtClient);



            // Original løsning
            //var client = new RestClient($"https://frost.met.no/observations/v0.jsonld?sources=SN27160&referencetime=R7%2F{sisteSyvDager}T12%2F{sisteSyvDager}T12%2FP1D&elements=air_temperature");

            //var request = new RestRequest("", Method.Get);
            //request.AddHeader("Accept", "application/json");
            //request.AddHeader("UserAgent", "HttpClientFactory-Sample");
            //request.AddHeader("Authorization", "Basic YmY5ZjUxYTMtMjZkMi00ZTZlLWFhYmYtMjVhNmU1MmJhYWE3OmQ5OTkyNWU1LTc5NDgtNDMwMy1iMGI2LThiZmFmMmNjZDU3NQ==");

            //var response = await client.ExecuteAsync(request);

            ////Behandling av JSON-respons
            //dynamic obj = JObject.Parse(response.Content);
            //string dataVerdi = obj.data.ToString();

            //Console.WriteLine(dataVerdi);
        }
    }
}
