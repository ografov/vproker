using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using vproker.Models;

namespace vproker.Services
{
    public class ReadClients
    {
        private static string Win1251ToUTF8(string source)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(source);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            source = win1251.GetString(win1251Bytes);
            return source;
        }

        public static async void ReadFromFile(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //ApplicationDbContext context = serviceScope.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
                ClientService clientService = serviceScope.ServiceProvider.GetService(typeof(ClientService)) as ClientService;

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.csv");
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found by path: " + filePath);
                }

                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Client client = ReadClientFromTextLine(line, ';');
                        if(client == null)
                        {
                            continue;
                        }

                        Client existingClient = null;
                        if (!String.IsNullOrEmpty(client.PhoneNumber))
                        {
                            existingClient = clientService.GetByPhone(null, client.PhoneNumber);
                        }
                        else
                        {
                            existingClient = clientService.GetByName(null, client.Name);
                        }

                        if(existingClient != null)
                        {
                            if (existingClient.Passport != client.Passport || existingClient.DateOfBirth != client.DateOfBirth)
                            {
                                existingClient.Passport = client.Passport;
                                existingClient.DateOfBirth = client.DateOfBirth;
                                await clientService.Store(existingClient);
                            }
                        }
                        else
                        {
                            await clientService.Store(client);
                        }
                    }
                }
            }
        }

        private static Client ReadClientFromTextLine(string textLine, char delimiter)
        {
            string[] parts = textLine.Split(new char[] { delimiter });
            Client client = new Client();
            if(String.IsNullOrEmpty(parts[0]))
            {
                // name is required
                return null;
            }
            client.Name = parts[0];
            client.PhoneNumber = parts[1];

            //client.PhoneNumber1 = parts[2];
            //client.EmailAddress = parts[3];
            if (DateTime.TryParse(parts[4], out DateTime dob))
            {
                client.DateOfBirth = dob;
            }
            client.Passport = parts[5];
            //client.Address = parts[6];
            return client;
        }
    }
}
