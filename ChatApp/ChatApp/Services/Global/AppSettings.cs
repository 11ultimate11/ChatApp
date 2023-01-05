using ChatApp.Models;
using ChatApp.Models.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace ChatApp.Services.Global
{
    public static class AppSettings
    {
        public static PersonModel Person { get; set; }
        public static IChatModel ChatModel { get; set; }
        public static string APItoken { get; set; }


        /// <summary>
        /// Decode an image from path and resize it , then write the new byte on the cache dir with the specific key 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MediaModel GetMediaModelFromPath(string path)
        {
            SKBitmap sKBitmap = SKBitmap.Decode(path).Resize(new SKImageInfo(300, 300), SKFilterQuality.High);
            var encode = sKBitmap.Encode(SKEncodedImageFormat.Jpeg, 100).ToArray();
            string key = Guid.NewGuid().ToString() + ".jpg";
            MediaModel model = new MediaModel()
            {
                Key = key,
                Data = encode
            };
            return model; 
        }
    }
    public enum Genders
    {
        männlich, weiblich
    }
    public enum ChatType
    {
        single, groupped
    }
}
