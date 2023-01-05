using ChatApp.Models.Abstract;
using ChatApp.Services.Global;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChatApp.Models
{
    public class PersonModel : AbstractSqlModel
    {
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        [JsonIgnore]
        public string Avatar => Media is null || string.IsNullOrEmpty(Media.Path) ? "User" : Media.Path;
        public int CredentialsId { get; set; }
        public CredentialsModel Credentials { get; set; }
        public Genders Gender { get; set; }
        public int MediaId { get; set; }
        private MediaModel _media;
        public MediaModel Media
        {
            get => _media;
            set
            {
                _media= value;
                OnPropertyChanged(nameof(Media));
                OnPropertyChanged(nameof(Avatar));
            }
        }
        [JsonIgnore]
        public string Bday => CreatedDate.ToString("dd.MMMM.yyyy");
        [JsonIgnore]
        public string Name => GetName();
        public string GetName()
        {
            return $"{Nachname} {Vorname}";
        }
        public void UpdateMediaUI()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                OnPropertyChanged(nameof(Media));
                OnPropertyChanged(nameof(Avatar));
            });
        }
    }
}
