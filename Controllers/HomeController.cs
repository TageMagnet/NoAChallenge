using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoaChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsumingWebAapiRESTinMVC.Controllers
{
    public class HomeController : Controller
    {
        //Instantiate an instance of the client
        static HttpClient client = new HttpClient();

        //The base adress of the api
        string Baseurl = "https://reqres.in/api/";

        public async Task<ActionResult> Index()
        {
            //Instantiate an instance of the ColorModel
            ColorModel ColorInfo = new ColorModel();

            //Declaring the list of all fetched objects
            List<Color> AllItemsFromAPI = new List<Color>();

            //Declaring the lists of pantone numbers, or the first two numbers in the pantone numbers, that is
            List<Color> DivisibleByThree = new List<Color>();
            List<Color> DivisibleByTwo = new List<Color>();
            List<Color> TheRest = new List<Color>();

            using (client)
            {
                //Get the base address of the api
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Fetch objects from all pages
                for (int page = 1; page <= 6; page++)
                {
                    //Get the responsemessage from api for respective page
                    HttpResponseMessage Res = await client.GetAsync($"example?per_page=2&page={page}");
                    
                    //If the response is successful...
                    if (Res.IsSuccessStatusCode)
                    {
                        //...the json-object is stored in a variable...
                        var colorResponse = Res.Content.ReadAsStringAsync().Result;

                        //...that is converted and stored in the ColorInfo object
                        ColorInfo = JsonConvert.DeserializeObject<ColorModel>(colorResponse);

                        foreach (var item in ColorInfo.data)
                        {
                            //And the object is added to the list
                            AllItemsFromAPI.Add(item);
                        }
                        
                    }
                }

                foreach (var item in AllItemsFromAPI)
                {
                    //Check the first two digits in pantone-code
                    int firstTwoDigitsInPantoneCode = int.Parse(item.pantone_value.Substring(0, 2));
                    
                    //Determine divisibility, add to lists accordingly
                    if (firstTwoDigitsInPantoneCode % 3 == 0)
                    {
                        DivisibleByThree.Add(item);
                    }
                    else if (firstTwoDigitsInPantoneCode % 2 == 0 && !DivisibleByThree.Contains(item))
                    {
                        DivisibleByTwo.Add(item);
                    }
                    else
                    {
                        TheRest.Add(item);
                    }
                }

                //Sort the lists by the year-property
                var DivisibleByThreeSorted = DivisibleByThree.OrderBy(o => o.year).ToList();
                var DivisibleByTwoSorted = DivisibleByTwo.OrderBy(o => o.year).ToList();
                var TheRestSorted = TheRest.OrderBy(o => o.year).ToList();

                //Add the lists to ViewBag
                ViewBag.DivisibleByThree = DivisibleByThreeSorted;
                ViewBag.DivisibleByTwo = DivisibleByTwoSorted;
                ViewBag.TheRest = TheRestSorted;

                //Return the complete list to the view
                return View(AllItemsFromAPI);
            }
        }
    }
}