using Lab4AntonSmovzhenkoCsharp.Models;
using Lab4AntonSmovzhenkoCsharp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab4AntonSmovzhenkoCsharp.Repository
{
    internal class PersonFileRepository
    {
        private static readonly string MainFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyC#Storage", "MyUsers");
  
        public async Task AddToRepositoryOrUpdateAsync(Person person)
        {
            var personInString = JsonSerializer.Serialize(person);
            using (var writer = new StreamWriter(Path.Combine(MainFolder, person.Email), false))
            {
                await writer.WriteAsync(personInString);
            }

        }

        public async Task<Person> GetPersonAsync(string email)
        {
            string personInString = null;
            string path = Path.Combine(MainFolder, email);
            if (!File.Exists(path))
            {
                return null;
            }
            using (var reader = new StreamReader(path))
            {
                personInString = await reader.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<Person>(personInString);
        }

        public void RemoveFromRepository(Person person)
        {
            File.Delete(Path.Combine(MainFolder, person.Email));
        }
        public List<RedactorViewModel> GetAllPersons(Action gotoInfo)
        {
            List<RedactorViewModel> persons = new List<RedactorViewModel>();
            foreach (var file in Directory.EnumerateFiles(MainFolder))
            {
                string personInString = null;
                using (var reader = new StreamReader(file))
                {
                    personInString = reader.ReadToEnd();
                }
                persons.Add(new RedactorViewModel(JsonSerializer.Deserialize<Person>(personInString), gotoInfo));
            }
            return persons;
        }

    }


}