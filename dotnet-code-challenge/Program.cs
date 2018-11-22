using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using dotnet_code_challenge.Model;

namespace dotnet_code_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            // load data
            XElement raceCaulfield = XElement.Load(@"FeedData\Caulfield_Race1.xml");
            JObject raceWolferhampton = JObject.Parse(File.ReadAllText(@"FeedData\Wolferhampton_Race1.json"));

            var tasks = new List<Task<List<HorseBet>>>();
            tasks.Add(Task.Run( () => XmlDataProcessor.ProcessData(raceCaulfield) ));
            tasks.Add(Task.Run( () => JsonDataProcessor.ProcessData(raceWolferhampton) ));

            // run in parallel
            var continuation = Task.WhenAll(tasks);

            try
            {
                continuation.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine($"Error processing racing data: {e.Message}");
                throw;
            }

            if (continuation.Status == TaskStatus.RanToCompletion)
            {
                // flatten results into one list
                var results = continuation.Result.ToList().SelectMany( r => r);

                // process results as required by business rules
                var resultsForDisplay = results.OrderBy(r => r.Price);

                // output results
                foreach (HorseBet horseBet in resultsForDisplay)
                {
                    Console.WriteLine($"Horse name: {horseBet.HorseName}, Price: {horseBet.Price}");
                }

            }
            else
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"Task {task.Id} has status {task.Status}");
                }
            }

            Console.WriteLine("Hello Racing World!");
        }
    }
}
