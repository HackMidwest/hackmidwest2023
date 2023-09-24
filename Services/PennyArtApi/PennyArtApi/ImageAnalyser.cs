using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PennyArtApi
{
    public class ImageAnalyzer
    {
        public ImageAnalyzer() { }

        public async Task<string[]> AnalyzeAsync(string imageUrl)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://pa-cgs.cognitiveservices.azure.com/vision/v3.2/analyze?visualFeatures=Tags");
                request.Headers.Add("Ocp-Apim-Subscription-Key", "9ed82b3050614066a676096ca20a118f");
                var content = new StringContent(JsonConvert.SerializeObject(new AnalyzerPayload() { url = imageUrl }), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var resultObj = JsonConvert.DeserializeObject<AnalyzerResponse>(json);
                return GetTopNTags(resultObj, 3);
            }
            catch (Exception)
            {

                return new string[] { "child art", "drawing" };
            }
           
           
        }

        private string[] GetTopNTags(AnalyzerResponse resultObj, int v)
        {
            if(resultObj!= null && resultObj.tags.Any())
            {
                return resultObj.tags.Where(t=>t.confidence >= 0.85).Select(t=>t.name).ToArray();
            }
            return null;
        }
    }


    public class AnalyzerPayload
    {
        public string url { get; set; }
    }


    public class AnalyzerResponse
    {
        public Tag[] tags { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
        public string modelVersion { get; set; }
    }

    public class Metadata
    {
        public int height { get; set; }
        public int width { get; set; }
        public string format { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public float confidence { get; set; }
    }


}
