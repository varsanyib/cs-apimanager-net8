using APIManagerVB.Request;
using APIManagerVB;
using Newtonsoft.Json.Linq;

namespace APISyncTestCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Synchronous API Test:\n\n");

            //GET API Test
            Console.WriteLine("GET API Test: ");
            TestCall();

            //POST API Test
            Console.WriteLine("\n\nPOST API Test: ");
            TestPostCall();

            //PUT API Test
            Console.WriteLine("\n\nPUT API Test: ");
            TestPutCall();

            //DELETE API Test
            Console.WriteLine("\n\nDELETE API Test: ");
            TestDeleteCall();
        }
        static void TestDeleteCall()
        {
            //5 seconds delay on API
            Synchronous req = new Synchronous(RequestMethod.DELETE, "https://reqres.in/api", "/users/3?delay=3");
            //Max. Request timeout (def: 10 sec)
            req.AnswerTimeout = 5;
            //Add a header
            req.Headers.Add("User-Agent", "APIManagerVB Test");
            //Disable response processing
            req.JsonAnswerProcessing = false;

            //Execute the request
            req.Execute();

            //Print the logs
            foreach (var log in req.Logs)
            {
                Console.WriteLine(log.Time + " - " + log.Type + " - " + log.Title);
            }

            //Print the response in text (request answer full empty)
            Console.WriteLine("Response: " + req.ResponseText);

            //Print the response in JSON (processing disabled (null check))
            Console.WriteLine("Response JSON: " + (req.ResponseJson?.ToString() ?? "n/a"));

            //Print the status code
            Console.WriteLine("\nStatus Code: " + req.StatusCode);
        }
        static void TestPutCall()
        {
            //5 seconds delay on API
            Synchronous req = new Synchronous(RequestMethod.PUT, "https://reqres.in/api", "/users/3");
            //Max. Request timeout (def: 10 sec)
            req.AnswerTimeout = 4;
            //Add a header
            req.Headers.Add("User-Agent", "APIManagerVB Test");
            //Add a body
            req.BodyData = BodyType.JSON;
            req.Body = new UserDTO("TestUserModified", "Software Tester");

            //Execute the request
            req.Execute();

            //Print the logs
            foreach (var log in req.Logs)
            {
                Console.WriteLine(log.Time + " - " + log.Type + " - " + log.Title);
            }

            //Print the response
            Console.WriteLine("Response: " + (req.ResponseJson?.ToString() ?? "n/a"));

            //Print the status code
            Console.WriteLine("\nStatus Code: " + req.StatusCode);

            //Print JSON response example
            Console.WriteLine("Name: " + (req.ResponseJson?["name"]?.Value<string?>() ?? "N/A"));
            Console.WriteLine("CreatedAt: " + (req.ResponseJson?["createdAt"]?.Value<DateTime>() ?? DateTime.MinValue).ToString("G")); //This request not contains createdAt field, null check test :)
        }
        static void TestPostCall()
        {
            //5 seconds delay on API
            Synchronous req = new Synchronous(RequestMethod.POST, "https://reqres.in/api", "/users");
            //Max. Request timeout (def: 10 sec)
            req.AnswerTimeout = 4;
            //Add a header
            req.Headers.Add("User-Agent", "APIManagerVB Test");
            //Add a body
            req.BodyData = BodyType.JSON;
            req.Body = new UserDTO("TestUser", "Software Tester");

            //Execute the request
            req.Execute();

            //Print the logs
            foreach (var log in req.Logs)
            {
                Console.WriteLine(log.Time + " - " + log.Type + " - " + log.Title);
            }

            //Print the response
            Console.WriteLine("Response: " + (req.ResponseJson?.ToString() ?? "n/a"));

            //Print the status code
            Console.WriteLine("\nStatus Code: " + req.StatusCode);

            //Print JSON response example
            Console.WriteLine("ID: " + (req.ResponseJson?["id"]?.Value<int?>() ?? 0));
            Console.WriteLine("Name: " + (req.ResponseJson?["name"]?.Value<string?>() ?? "N/A"));
            Console.WriteLine("CreatedAt: " + (req.ResponseJson?["createdAt"]?.Value<DateTime>() ?? DateTime.MinValue).ToString("G"));
        }
        static void TestCall()
        {
            //5 seconds delay on API
            Synchronous req = new Synchronous(RequestMethod.GET, "https://reqres.in/api", "/users/1?delay=2");
            //Max. Request timeout (def: 10 sec)
            req.AnswerTimeout = 4;
            //Add a header
            req.Headers.Add("User-Agent", "APIManagerVB Test");

            //Execute the request
            req.Execute();

            //Print the logs
            foreach (var log in req.Logs)
            {
                Console.WriteLine(log.Time + " - " + log.Type + " - " + log.Title);
            }

            //Print the response
            //Console.WriteLine("Response: " + (req.ResponseJson?.ToString() ?? "n/a"));

            //Print the status code
            Console.WriteLine("\nStatus Code: " + req.StatusCode);

            //Print first name and last name from the response
            Console.WriteLine("ID: " + (req.ResponseJson?["data"]?["id"]?.Value<int?>() ?? 0));
            Console.WriteLine("First Name: " + (req.ResponseJson?["data"]?["first_name"]?.Value<string?>() ?? "N/A"));
            Console.WriteLine("Last Name: " + (req.ResponseJson?["data"]?["last_name"] ?? "N/A"));
        }
    }
}
