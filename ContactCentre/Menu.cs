using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vonage.Voice.Nccos;
using Vonage.Voice.Nccos.Endpoints;

namespace ContactCentre
{
    public static class Menu
    {
        [FunctionName("Menu")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Menu event triggered");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            var ncco = new Ncco();
            
            var selectedOption = data.dtmf.digits;
            switch (selectedOption.ToString())
            {
                case "1":
                    // LOOK UP CUSTOMER SPECIFIC DATA
                    ncco.Actions.Add(
                        new TalkAction
                        {
                            Text = "Your order is on it's way.",
                            Language = "en-GB",
                            Style = 2
                        });
                    break;
                case "2":
                    // CONNECT TO TELEPHONE AGENT
                    ncco.Actions.Add(
                        new TalkAction
                        {
                            Text = "Please wait while we connect you to the next available operator.",
                            Language = "en-GB",
                            Style = 2
                        });

                    ncco.Actions.Add(
                        new ConnectAction
                        {
                            Endpoint = new Endpoint[]
                            {
                                new PhoneEndpoint
                                {
                                    Number = "44123456789"
                                }
                            }
                        });
                    break;
            }

            return new OkObjectResult(ncco);
        }
    }
}
