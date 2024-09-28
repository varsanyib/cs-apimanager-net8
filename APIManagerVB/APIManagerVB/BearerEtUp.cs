using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIManagerVB
{
    public class BearerEtUp
    {
        /// <summary>
        /// Set Bearer Token for API Request in KeyValuePair
        /// </summary>
        /// <param name="token">Token Text</param>
        /// <returns>Authorization Header in KeyValuePair</returns>
        public static KeyValuePair<string, string> SetBearerToken(string token)
        {
            return new KeyValuePair<string, string>("Authorization", $"Bearer {token}");
        }
    }
}
