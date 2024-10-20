using Microsoft.VisualStudio.TestPlatform.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace RestSharpTests
{
    [TestClass]
    public class ApiFunctionalTests
    {
        private RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("https://api.restful-api.dev");
        }

        [TestMethod]
        [TestCategory("API")]
        [DataRow("Apple MacBook Pro 16", 2019, 1849.99, "Intel Core i9", "1 TB", "Patched New Product Name")]
        public void Validate_PATCH_request_to_updateName(String productName, int year, double price, String cpuModel, String hardDiskSize, String newName)
        {   
            // Creating Product Entity
            var request = new RestRequest("/objects", Method.Post);   
            var payload = new
            {
                name = productName,
                data = new
                {
                    year = year,
                    price = price,
                    CPUmodel = cpuModel,
                    Harddisksize = hardDiskSize
                }
            };
            request.AddJsonBody(payload);
            var response = client.Execute(request);
          
            // Asserting Product Entity Creation 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
         
            var jsonResponse = JObject.Parse(response.Content);
            var productID = (string)jsonResponse.SelectToken("$.id");

            // Patch update request
            var patchRequest = new RestRequest("/objects/"+productID, Method.Patch);

            var patchPayload = new
            {
                name = newName,
            };

            patchRequest.AddBody(patchPayload);

            var patchResponse = client.Execute(patchRequest);
            Console.WriteLine(patchResponse.Content);
            var patchJsonResponse = JObject.Parse(patchResponse.Content);
            var updatedName = (string)patchJsonResponse.SelectToken("$.name");

            Assert.AreEqual(HttpStatusCode.OK, patchResponse.StatusCode);
            Assert.AreNotEqual(productName, newName);
            Assert.AreEqual(updatedName, newName);
            Assert.IsNotNull(response.Content);

        }
    }
}
