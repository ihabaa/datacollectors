using System;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace mockdotnet
{

    public class Customer
    {
        public string party_id { get; set; }
        public string date_of_birth { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string ssn { get; set; }
        public string last_4_ssn { get; set; }
    }
    public class Account
    {
        public string account_number;
        public string make;
        public string model_description;
        public string model_year;
        public List<Customer> customers { get; set; }
    }
    public class AccountResponse
    {
        public List<Account> Accounts { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();
      
        private static AccountResponse accts;
    
        static AccountResponse GetAccountAsync(string path)
        {
            AccountResponse model = null;
            var client = new HttpClient();
            var task = client.GetAsync(path)
              .ContinueWith((taskwithresponse) =>
              {
                  var response = taskwithresponse.Result;
                  var jsonString = response.Content.ReadAsStringAsync();
                  jsonString.Wait();
                  model = JsonConvert.DeserializeObject<AccountResponse>(jsonString.Result);

              });
            task.Wait();
            return model;
        }
        static void Main()
        {

            RunAsync().GetAwaiter().GetResult();
        }
        static async Task RunAsync()
        {
              
         //   client.BaseAddress = new Uri("https://ihab-ock-api.free.beeceptor.com/details");
            client.BaseAddress = new Uri("http://localhost:3001/details");
        
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
               

                while (true)
                { 

                accts = GetAccountAsync(client.BaseAddress.AbsoluteUri);
                    Console.WriteLine("#of Accounts = {0}",accts.Accounts.Count);
                    Console.WriteLine(accts.Accounts.ToArray()[0].account_number);
                    Console.WriteLine(accts.Accounts.ToArray()[0].customers.ToArray()[0].first_name);
                  
                    System.Threading.Thread.Sleep(60*1000); 


                }

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            // Console.ReadLine();
        }
    }
}


