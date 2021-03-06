﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> InstagramHelper.cs </Name>
//         <Created> 17/06/2017 2:07:18 AM </Created>
//         <Key> 3faa58e0-4566-4dd8-b606-06e6d194e9e0 </Key>
//     </File>
//     <Summary>
//         InstagramHelper.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Puppy.Instagram
{
    public static class InstagramHelper
    {
        /// <summary>
        ///     Get Maximum 20 recent feeds of user 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId">     </param>
        /// <returns></returns>
        public static async Task<InstagramUserFeeds> GetUserFeedsAsync(string accessToken, string userId = "self")
        {
            try
            {
                var endpoint = $"https://api.instagram.com/v1/users/{userId}/media/recent?access_token={accessToken}";
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(endpoint).ConfigureAwait(true);

                    var buffer = await result.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                    var byteArray = buffer.ToArray();
                    var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

                    var instagramUserFeeds = JsonConvert.DeserializeObject<InstagramUserFeeds>(responseString);

                    return instagramUserFeeds;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}