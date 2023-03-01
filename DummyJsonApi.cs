using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTest
{
    public class DummyJsonApi
    {
        private const string ApiUrl = "https://dummyjson.com/users";
        public static async Task<List<User>> GetUsersAsync()
        {
            List<User> userList = new List<User>();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);
            string json = await response.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(json);
            JArray data = (JArray)responseJson["users"];
            foreach (JObject element in data)
            {
                User user = new User
                {
                    FirstName = (string)element["firstName"],
                    LastName = (string)element["lastName"],
                    Email = (string)element["email"],
                    SourceId = (string)element["id"],
                };
                userList.Add(user);
            }
            return userList;
        }
    }
}
