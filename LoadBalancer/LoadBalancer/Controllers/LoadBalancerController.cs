using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace LoadBalancer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadBalancerController : ControllerBase
    {
        private string outputFolder = Directory.GetCurrentDirectory();
        IRestClient restClient;
        private List<string> services = new List<string>()
        {
            @"https://localhost:44331/",
            @"https://localhost:44331/",
            @"https://localhost:44331/"
        };
        private int counter = 0;
        public LoadBalancerController()
        {
            restClient = new RestClient();
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
            //log request
            DumpToFile(from.ToString(), to.ToString(), "Request Recieved. Passing it along to next service.");
            //choose next in line -- round robin
            var serviceToChoose = RoundRobin();
            DumpToFile(serviceToChoose, null, "Service Chosen:");
            //Passthrough request
            restClient.BaseUrl = new Uri(serviceToChoose);
            var request = new RestRequest("primenumber/" + from+ "/" + to, Method.GET);
            //get result
            var res = restClient.Execute(request);
            //log result
            DumpToFile(res.Content, null, "Service Response:");
            //return result
            return res.Content;
        }

        public string RoundRobin()
        {
            counter++;
            if (counter == services.Count)
                counter = 0;
            return services[counter];
        }

        private void DumpToFile(string parameter1, string parameter2, string message = null)
        {
            string currentContent = ReadFromFile();
            using (StreamWriter sw = new StreamWriter(outputFolder + "Failsafe.txt"))
            {
                string logstring = currentContent + "\n" + "TimeStamp: " + "\n" + DateTime.Now.ToString() + "\n";
                if (message != null)
                    logstring += "message: " + message + "\n";
                if (parameter1 != null)
                    logstring += "parameter 1: " + parameter1 + "\n";
                if (parameter2 != null)
                    logstring += "parameter 2: " + parameter2 + "\n";
                sw.Write(logstring);
            }
        }

        private string ReadFromFile()
        {
            string currentContent = "";
            if (System.IO.File.Exists(outputFolder + "Failsafe.txt"))
            {
                using (StreamReader sr = new StreamReader(outputFolder + "Failsafe.txt"))
                {
                    currentContent = sr.ReadToEnd();
                }
            }
            return currentContent;
        }
    }
}