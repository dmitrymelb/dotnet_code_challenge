using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using Xunit.Sdk;
using dotnet_code_challenge;
using dotnet_code_challenge.Model;
using Newtonsoft.Json.Linq;

namespace dotnet_code_challenge.Test
{
    public class DataProcessorTests
    {
        [Fact]
        public void XmlDataProcessor_returns_2_bets()
        {
            XElement raceCaulfield = XElement.Load(@"FeedData\Caulfield_Race1.xml");

            List<HorseBet> actual = XmlDataProcessor.ProcessData(raceCaulfield);

            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public void JsonDataProcessor_returns_horseName_Metallist_price_9_38()
        {
            // Arrange
            string race_1_horse = @"{
                'RawData': {
                    'Markets': [{
                        'Id': 'anything',
                        'Selections': [
                            {
                                'Id': 'horse ID',
                                'Price': '9.38',
                                'Tags': {
                                    'participant': '1',
                                    'name': 'Metallist'
                                }
                            }
                        ]
                    }]
                }
            }";

            JObject oneHorseRace = JObject.Parse(race_1_horse);

            string expectedHorseName = "Metallist";
            double expectedPrice = 9.38;

            // Act
            List<HorseBet> actual = JsonDataProcessor.ProcessData(oneHorseRace);

            // Assert
            Assert.Single(actual);
            Assert.Equal(expectedHorseName, actual[0].HorseName);
            Assert.Equal(expectedPrice, actual[0].Price);
        }



    }
}
