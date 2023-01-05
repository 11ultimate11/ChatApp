using MvvmHelpers;
using System.Collections.Generic;

namespace ChatApp.Models.Interfaces
{
    public interface IChatModel
    {
        int ID { get; }
        int Count { get; set; }
        int OwnerID { get; }
        int TargetID { get; }
        string LastMessage { get; set; }
        List<ChatKeyModel> Keys { get; }
        PersonModel Target { get; }
        ObservableRangeCollection<ChatMessageModel> ChatMessages { get; }
    }
}