using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace HomeTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string fileFormat = FileFormat();
            string folderPath = FolderPath();
            List<User> userList = new List<User>();
            userList.AddRange(await JsonPlaceholderApi.GetUsersAsync());
            userList.AddRange(await RandomUserApi.GetUsersAsync());
            userList.AddRange(await DummyJsonApi.GetUsersAsync());
            userList.AddRange(await ReqresApi.GetUsersAsync());



            await CreateOutputFile(folderPath,fileFormat,userList);

            Console.WriteLine($"the number of users is: {userList.Count()}");
            Console.ReadKey();
        }


        private static string FolderPath()
        {

            Console.WriteLine("Please enter the folder path:");
            string folderPath = Console.ReadLine();
            while (!System.IO.Directory.Exists(folderPath))
            {
                Console.WriteLine("This directory is not exist. please enter diffrent directory");
                folderPath = Console.ReadLine();
            }

            return folderPath;
        }

        private static string FileFormat()
        {
            Console.WriteLine("Please enter the file format (JSON or CSV):");
            string fileFormat = Console.ReadLine();
            while (fileFormat.ToUpper() != "JSON" && fileFormat.ToUpper() != "CSV")
            {
                Console.WriteLine("The format must be JSON or CSV");
                fileFormat = Console.ReadLine();
            }


            return fileFormat.ToUpper();
        }

        private static async Task CreateOutputFile(string folderPath, string fileFormat, List<User> users)
        {
            string fileName = $"users_{DateTime.Now:yyyyMMddhhmmss}.{fileFormat}";
            string filePath = Path.Combine(folderPath, fileName);

            using var streamWriter = new StreamWriter(filePath);
            if (fileFormat.Equals("JSON"))
            {
                await streamWriter.WriteAsync(JsonConvert.SerializeObject(users, Formatting.Indented));
            }
            else if (fileFormat.Equals("CSV"))
            {
                await streamWriter.WriteLineAsync("First Name,Last Name,Email,Source Id");
                foreach (User user in users)
                {
                    await streamWriter.WriteLineAsync($"{user.FirstName},{user.LastName},{user.Email},{user.SourceId}");
                }
            }
        }
    }
}