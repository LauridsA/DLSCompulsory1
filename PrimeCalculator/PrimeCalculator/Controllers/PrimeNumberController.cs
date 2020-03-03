using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrimeCorrectnessService;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrimeNumberController : ControllerBase
    {
        private string outputFolder = Directory.GetCurrentDirectory();

        [HttpGet]
        public string Get()
        {
            return "Frontpage/health endpoint";
        }

        [HttpGet]
        [Route("{from}/{to}")]
        public ActionResult<string> GetPrime([FromRoute]int from, [FromRoute]int to)
        {
            PrimeCorrectness primeChecker = new PrimeCorrectness();

            //log request
            DumpToFile(from.ToString(), to.ToString(), "Request received, beginning calculation");
            //perform calculation
            string res = primeChecker.CountPrimes(from.ToString(), to.ToString());
            //log result
            DumpToFile(res, null, "Calculation performed. Result below");
            //return result
            return res;
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