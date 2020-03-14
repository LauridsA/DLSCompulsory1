using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Serilog;

namespace LoadBalancer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoadBalancerController : ControllerBase
    {
        private string outputFolder = Directory.GetCurrentDirectory();
        IRestClient restClient;
        private List<string> services = new List<string>()
        {
            @"http://localhost:51294/",
            @"http://localhost:51295/",
            @"http://localhost:51296/"
        };
        private static int counter = 0;
        public LoadBalancerController()
        {
            restClient = new RestClient();
        }
        public string RoundRobin()
        {
            counter++;
            if (counter == services.Count)
            {
                counter = 0;
                return services[services.Count - 1];
            }
            return services[counter-1];
        }

        [HttpGet]
        public string Get()
        {
            return "Frontpage/health endpoint";
        }

        [HttpGet]
        [Route("{from}/{to}")]
        public string HandleRequest([FromRoute]int from, [FromRoute]int to)
        {
            string guid = Guid.NewGuid().ToString();
            //log request
            //choose next in line -- round robin
            
            var serviceToChoose = RoundRobin();
            Log.Information($"REQID: {guid} {Environment.NewLine} fromInput: {from.ToString()} {Environment.NewLine} toinput: {to.ToString()} {Environment.NewLine} Request received. Sent to service {serviceToChoose}.");

            //Passthrough request
            restClient.BaseUrl = new Uri(serviceToChoose);
            var request = new RestRequest("primenumber/" + from + "/" + to, Method.GET);
            //get result
            var res = restClient.Execute(request);
            //log result
            Log.Information($"REQID: {guid} response received: {res.Content} from service {serviceToChoose}.");

            //return result
            return res.Content;
        }
    }
}