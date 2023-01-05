using ChatApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Services.Processor
{
    internal interface IUsersProcessor
    {
        List<PersonModel> Persons { get; set; }

        ValueTask<PersonModel> GetPerson(int id);
    }
}