using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrimeCorrectnessService;
using Serilog;

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
            var guid = Guid.NewGuid();
            PrimeCorrectness primeChecker = new PrimeCorrectness();

            //log request
            Log.Information($"REQID: {guid} received request with from: {from} and to: {to}. Beginning calculation");
            //perform calculation
            string res = primeChecker.CountPrimes(from.ToString(), to.ToString());
            //log result
            Log.Information($"REQID: {guid} calculated results: {res}");
            //return result
            return res;
        }

    }
}