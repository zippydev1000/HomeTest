using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HomeTest
{
    public class RandomUserApi
    {
        private const string ApiUrl = "https://randomuser.me/api/";
        public static async Task<List<User>> GetUsersAsync()
        {   
            List<User> userList = new List<User>();

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);
            string json = await response.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(json);
            JArray data = (JArray)responseJson["results"];
            foreach (JObject element in data)
            {
                User user = new User
                {
                    FirstName = (string)element["name"]["first"],
                    LastName = (string)element["name"]["last"],
                    Email= (string)element["email"],
                    SourceId = (string)element["login"]["uuid"],
                };
                userList.Add(user);
            }
            return userList;
        }
       

    }
}
