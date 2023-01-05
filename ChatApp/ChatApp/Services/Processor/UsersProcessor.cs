using ChatApp.Models;
using ChatApp.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChatApp.Services.Processor
{
    class UsersProcessor : IUsersProcessor
    {
        private readonly IApiProcessor api;
        public List<PersonModel> Persons { get; set; } = new List<PersonModel>();
        public UsersProcessor()
        {
            api = DependencyService.Get<IApiProcessor>();
        }
        public async ValueTask<PersonModel> GetPerson(int id)
        {
            var find = Persons.Find(x => x.ID == id);
            if (find != null)
                return find;
            else
            {
                var result = await api.GetStringAsync($"Account/GetAccount/{id}");
                if (!string.IsNullOrEmpty(result))
                {
                    var model = JsonConvert.DeserializeObject<PersonModel>(result);
                    if (!Persons.Contains(model))
                    {
                        Persons.Add(model);
                        return model;
                    }
                    else
                    {
                        return Persons.Find(x => x.ID == id);
                    }
                }
                return null;
            }

        }
    }
}
