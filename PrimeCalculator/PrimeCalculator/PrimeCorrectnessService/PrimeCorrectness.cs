using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeCorrectnessService
{
    public class PrimeCorrectness
    {
        public string CountPrimes(string from, string to)
        {
            try
            {
                int fromAsInt = int.Parse(from);
                int toAsInt = int.Parse(to);
                string faggot = "";
                while (fromAsInt <= toAsInt)
                {
                    if (IsPrime(fromAsInt.ToString()))
                        faggot += $"{fromAsInt}, ";
                    fromAsInt++;
                }
                return faggot;
            }
            catch (Exception e)
            {
                return "im the nigger";
            }
        }
        public bool IsPrime(string check)
        {
            try
            {
                int checkFor = int.Parse(check);

                for (int counter = 2; counter < checkFor; counter++)
                    if (checkFor % counter == 0)
                        return false;
                return true;
            }
            catch (Exception e)
            {
                // Log.Error("Not a prime");
                return false;

            }
        }
    }
}
