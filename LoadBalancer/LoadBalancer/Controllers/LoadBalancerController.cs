using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadBalancerController : ControllerBase
    {
        private string outputFolder = Directory.GetCurrentDirectory();
        private List<string> services = new List<string>()
        {
            "Service1",
            "Service2",
            "Service3"
        };
        private int counter = 0;
        public LoadBalancerController()
        {
            //empty lol
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
            if (counter == services.Count)
                counter = 0;
            else
                counter++;
            var serviceToChoose = services[counter];
            DumpToFile(serviceToChoose, null, "Service Chosen:");
            //Passthrough request

            //get result

            //log result

            //return result
            return "Frontpage/health endpoint";
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
                return currentContent;
            }
            return currentContent;
        }
    }
}