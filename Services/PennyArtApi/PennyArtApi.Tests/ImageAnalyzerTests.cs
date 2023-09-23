namespace PennyArtApi.Tests
{
    [TestClass]
    public class ImageAnalyzerTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var a = new ImageAnalyzer();
            var resultObj = await a.AnalyzeAsync("https://content.active.com/Assets/Active.com+Content+Site+Digital+Assets/Kids/Articles/Kid%24!e2%24!80%24!99s+Art/kids+art-carousel.jpg");

            Assert.IsNotNull(resultObj);
        }
    }
}