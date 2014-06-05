namespace Employees.Web.Models
{
    using System;
    using System.Security.Cryptography;

    using Newtonsoft.Json;

    public static class ETagExtensions
    {
        public static string GenerateETag(this object obj)
        {
            var objJson = JsonConvert.SerializeObject(obj);
            var objJsonBytes = System.Text.Encoding.ASCII.GetBytes(objJson);

            var hashProvider = new MD5CryptoServiceProvider();
            var hash = hashProvider.ComputeHash(objJsonBytes);

            var etagString = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            return string.Format("\"{0}\"", etagString);
        }
    }
}