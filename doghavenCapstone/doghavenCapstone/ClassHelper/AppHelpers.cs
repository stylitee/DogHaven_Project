using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace doghavenCapstone.ClassHelper
{
    public class AppHelpers
    {
        
        public static void checkConnection(ContentPage mainpage, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess.ToString() != "Internet")
            {
                mainpage.Navigation.PushAsync(new InternetChecker());
            }
        }

        public static string PasswordEncryption(string value)
        {
            string hash = "d0gh@vEn";
            string _result = "";
            byte[] data = UTF8Encoding.UTF8.GetBytes(value);
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using(TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    _result = Convert.ToBase64String(results, 0, results.Length);
                }
            }
            return _result;
        }

        public static string PasswordDecrypt(string value)
        {
            string hash = "d0gh@vEn";
            string _result = "";
            byte[] data = Convert.FromBase64String(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    _result = UTF8Encoding.UTF8.GetString(results);
                }
            }
            return _result;
        }

        public async static void PushNotificationInit()
        {
            var checker = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
            foreach(var c in checker)
            {
                var matches = await App.client.GetTable<dogMatches>().Where(x => x.dog2 == c.id && x.markAsDone == "False").ToListAsync();
                if (matches.Count != 0)
                {
                    if(App.isAlreadyRead == false)
                    {
                        Random random = new Random();
                        int i = random.Next();
                        var notification = new NotificationRequest
                        {
                            BadgeNumber = 1,
                            Description = "You've got a match, check i@t out!",
                            Title = "Match Notification!",
                            NotificationId = i,
                            ReturningData = "Youve been match recently go check it out!",
                            NotifyTime = DateTime.Now.AddSeconds(1)
                        };

                        NotificationCenter.Current.Show(notification);
                        dogMatches match = new dogMatches()
                        {
                            id = matches[0].id,
                            dog1 = matches[0].dog1,
                            dog2 = matches[0].dog2,
                            markAsDone = "True"
                        };

                        await App.client.GetTable<dogMatches>().UpdateAsync(match);
                        
                        App.isAlreadyRead = true;
                    }
                }
                else
                {
                    App.isAlreadyRead = false;
                }
            }
        }
    }
}
