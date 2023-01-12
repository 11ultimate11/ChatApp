using GhostLibrary.Models;

namespace ChatAppServer.Models
{
    public class ExtendedChat : ChatModel
    {
        public PersonModel Target { get; set; }
    }
}
