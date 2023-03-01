using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeTest
{
    public class JsonPlaceholderApi
    {
        private const string ApiUrl = "https://jsonplaceholder.typicode.com/users";
        public static async Task<List<User>> GetUsersAsync()
        {
            List<User> userList = new List<User>();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);
            string json = await response.Content.ReadAsStringAsync();
            List<dynamic> data = JsonConvert.DeserializeObject<List<dynamic>>(json);

            foreach (var element in data)
            {
                string name = element.name;
                string firstName = name.Split(' ')[0];
                string lastName = name.Split(' ')[1];
                User user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = element.email,
                    SourceId = element.id
                };
                userList.Add(user);
            }
               
                return userList;
        }

    }
}
