using dotnet_code_challenge.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace dotnet_code_challenge
{
    public static class JsonDataProcessor
    {
        public static List<HorseBet> ProcessData(JObject jsonRoot)
        {
            var allBets =
                from slctn in jsonRoot["RawData"]["Markets"][0]["Selections"]
                select new HorseBet() { HorseName = Convert.ToString(slctn["Tags"]["name"])
                                        , Price = Convert.ToDouble(slctn["Price"]) }
            ;

            return allBets.ToList();
        }
    }
}
