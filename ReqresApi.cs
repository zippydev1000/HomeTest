using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTest
{
    public class ReqresApi
    {
        private const string ApiUrl = "https://reqres.in/api/users";
        public static async Task<List<User>> GetUsersAsync()
        {
            List<User> userList = new List<User>();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);
            string json = await response.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(json);
            JArray data = (JArray)responseJson["data"];
            foreach (JObject element in data)
            {
                User user = new User
                {
                    FirstName = (string)element["first_name"],
                    LastName = (string)element["last_name"],
                    Email = (string)element["email"],
                    SourceId = (string)element["id"],
                };
                userList.Add(user);
            }
            return userList;
        }
    }
}
