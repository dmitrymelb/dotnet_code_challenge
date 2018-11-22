using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using dotnet_code_challenge.Model;

namespace dotnet_code_challenge
{
    public static class XmlDataProcessor
    {
        private class Horse
        {
            public int ID;
            public string Name;
        }

        private class Market
        {
            public int HorseID;
            public double Price;
        }


        public static List<HorseBet> ProcessData(XElement xmlRoot)
        {
            IEnumerable<Horse> horses =
                            from el in xmlRoot.Elements("races")
                                    .Elements("race")
                                    .Elements("horses")
                                    .Elements("horse")
                            select new Horse() { ID = Convert.ToInt32(el.Element("number").Value), Name = el.Attribute("name").Value }
            ;

            IEnumerable<Market> prices =
                            from el in xmlRoot.Elements("races")
                                    .Elements("race")
                                    .Elements("prices")
                                    .Elements("price")
                                    .Elements("horses")
                                    .Elements("horse")
                            select new Market() { HorseID = Convert.ToInt32(el.Attribute("number").Value), Price = Convert.ToDouble(el.Attribute("Price").Value) }
            ;


            var allBets = from hrs in horses
                        join prc in prices on hrs.ID equals prc.HorseID
                        orderby prc.Price
                        select new HorseBet() { HorseName = hrs.Name
                                                , Price = prc.Price }
            ;

            return allBets.ToList();
        }
    }
}
