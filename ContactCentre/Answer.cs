using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Vonage.Voice.Nccos;

namespace ContactCentre
{
public static class Answer
{
    [FunctionName("Answer")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("Phone call answered");

        var ncco = new Ncco(new NccoAction[] {
            new TalkAction
            {
                Text = "Welcome to the Contact Centre. Press 1 for order information. Press 2 to speak to an operator.",
                Language = "en-GB",
                Style = 2
            },
            new MultiInputAction
            {
                Dtmf = new DtmfSettings{MaxDigits = 1},
                EventUrl = new []{ "https://3d9a-62-31-58-129.ngrok.io/api/menu" }
            }
        });

        return new OkObjectResult(ncco);
    }
}
}
