using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FrostAPIService
{
    internal class Program
    {
        private List<Observasjon> observasjoner = new List<Observasjon>();
        private readonly string sisteSyvDager = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

        static async Task Main(string[] args)
        {
            Program pr = new Program();
            var obs = await pr.HentObservasjonerAsync();

            foreach (var ob in obs)
            {
                Console.WriteLine(ob);
            }
        }

        public async Task<List<Observasjon>> HentObservasjonerAsync()
        {
            var client = new RestClient($"https://frost.met.no/observations/v0.jsonld?sources=SN27160&referencetime=R7%2F{sisteSyvDager}T12%2F{sisteSyvDager}T12%2FP1D&elements=air_temperature");

            var request = new RestRequest("", Method.Get);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("UserAgent", "HttpClientFactory-Sample");
            request.AddHeader("Authorization", "Basic YmY5ZjUxYTMtMjZkMi00ZTZlLWFhYmYtMjVhNmU1MmJhYWE3OmQ5OTkyNWU1LTc5NDgtNDMwMy1iMGI2LThiZmFmMmNjZDU3NQ==");

            var response = await client.ExecuteAsync(request);

            //Behandling av JSON-respons
            dynamic obj = JObject.Parse(response.Content);
            string dataVerdi = obj.data.ToString();

            var deserializedDataValue = JsonConvert.DeserializeObject<List<Rootobject>>(dataVerdi);

            observasjoner.Clear();

            foreach (var o in deserializedDataValue)
            {
                var nyObservasjon = new Observasjon();
                nyObservasjon.Dato = o.ReferenceTime;
                nyObservasjon.Temperatur = o.Observations[0].Value;
                observasjoner.Add(nyObservasjon);
            }
            return observasjoner;
        }
    }
    public class Rootobject
    {
        public string SourceId { get; set; }
        public DateTime ReferenceTime { get; set; }
        public Observation[] Observations { get; set; }
    }

    public class Observation
    {
        public string ElementId { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }
        public Level Level { get; set; }
        public string TimeOffset { get; set; }
        public string TimeResolution { get; set; }
        public int TimeSeriesId { get; set; }
        public string PerformanceCategory { get; set; }
        public string ExposureCategory { get; set; }
        public int QualityCode { get; set; }
    }

    public class Level
    {
        public string LevelType { get; set; }
        public string Unit { get; set; }
        public int Value { get; set; }
    }
}
