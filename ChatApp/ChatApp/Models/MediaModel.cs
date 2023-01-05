using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Essentials;

namespace ChatApp.Models
{
    public class MediaModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public byte[] Data { get; set; }
        [JsonIgnore]
        public string Path => GetFilePath();

        private string GetFilePath()
        {
            var path = System.IO.Path.Combine(FileSystem.CacheDirectory, Key);
            if(File.Exists(path))
                return path;
            else
            {
                if (Data != null && Data.Length > 0)
                {
                    File.WriteAllBytes(path, Data);
                    return path;
                }
                else return string.Empty;
            }
                
        }
    }
}
