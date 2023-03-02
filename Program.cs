using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Diagnostics;


namespace HomeTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string fileFormat = FileFormat();
            string folderPath = FolderPath();
            List<User> userList = new List<User>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var usersTasks = new List<Task<List<User>>>
            {
                 JsonPlaceholderApi.GetUsersAsync(),
                 RandomUserApi.GetUsersAsync(),
                 DummyJsonApi.GetUsersAsync(),
                 ReqresApi.GetUsersAsync()

            };
            List< Task <List < User >>> tasks = new List<Task<List<User>>>();
            tasks.Add(Task.Run(() => JsonPlaceholderApi.GetUsersAsync()));
            tasks.Add(Task.Run(() => RandomUserApi.GetUsersAsync()));
            tasks.Add(Task.Run(() => DummyJsonApi.GetUsersAsync()));
            tasks.Add(Task.Run(() => ReqresApi.GetUsersAsync()));
            await Task.WhenAll(tasks);
            tasks.ForEach(x =>
            {
                userList.AddRange(x.Result);
            });

            await CreateOutputFile(folderPath, fileFormat, userList);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            
            //userList.Clear();
            //stopwatch.Start();
            //userList.AddRange(await JsonPlaceholderApi.GetUsersAsync());
            //userList.AddRange(await RandomUserApi.GetUsersAsync());
            //userList.AddRange(await DummyJsonApi.GetUsersAsync());
            //userList.AddRange(await ReqresApi.GetUsersAsync());

            //await CreateOutputFile(folderPath, fileFormat, userList);

            //stopwatch.Stop();
            //ts = stopwatch.Elapsed;
            //elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //Console.WriteLine("RunTime " + elapsedTime);


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