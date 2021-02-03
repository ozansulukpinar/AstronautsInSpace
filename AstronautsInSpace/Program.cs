using System;
using RestSharp;
using Newtonsoft.Json;

namespace AstronautsInSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            //Number and names of astronauts in space
            IRestResponse response = GetResponse("http://api.open-notify.org/astros.json");
            AstronautResponse result = JsonConvert.DeserializeObject<AstronautResponse>(response.Content.ToString());

            //Current location of ISS
            IRestResponse responseII = GetResponse("http://api.open-notify.org/iss-now.json");
            IssResponse resultII = JsonConvert.DeserializeObject<IssResponse>(responseII.Content.ToString());

            DateTime date = new DateTime(1970,1,1,0,0,0,0).AddSeconds(resultII.Timestamp).ToLocalTime();
            string currentDate = date.ToString("dd MMMM yyyy HH:mm:ss");

            string astronauts = "";
            int numberOfAstronaut = result.Number;

            double latitude = resultII.Iss_Position.Latitude;
            double longitude = resultII.Iss_Position.Longitude;

            string directionOfLatitude = "N";
            string directionOfLongitude = "E";
            
            string currentSituation = "";

            if (numberOfAstronaut != 0)
            {
                for(int i = 0; i < numberOfAstronaut; i++)
                {
                    if(i == (numberOfAstronaut - 1))
                        astronauts += " and ";

                    astronauts += result.People[i].Name + "(" + result.People[i].Craft + ")";

                    if ( i < (numberOfAstronaut - 2))
                        astronauts += ", ";
                }

                currentSituation = "There are " + numberOfAstronaut + " astronauts in space on " + currentDate
                + ". Their names are " + astronauts + ". They are currently over " + latitude +
                "° " + directionOfLatitude + ", " + longitude + "° " + directionOfLongitude + ".";
            }
            else
            {
                currentSituation = "There is no astronaut in space right now. Is everything OK?";
            }

            Console.WriteLine(currentSituation);
        }

        private static IRestResponse GetResponse(string endpoint)
        {
            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            return response;
        }
    }
}