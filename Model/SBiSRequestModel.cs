using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace RU.NekAlex.Sbis
{
    /// <summary>
    /// Класс-обертка для веб запросов
    /// </summary>
    public class SbisRequestWrapper
    {
        /// <summary>
        /// Веб-запрос
        /// </summary>
        private WebRequest httpRequest;
        /// <summary>
        /// Ответ сервера
        /// </summary>
        private WebResponse httpResponse;
        /// <summary>
        /// Тело запроса
        /// </summary>
        private string JSONRequest;
        /// <summary>
        /// GUID авторизации
        /// </summary>
        private SbisSettings _settings ;
        public SbisRequestWrapper(SbisSettings ss)
        {

            _settings = ss;
            
        }
        /// <summary>
        /// Получить данные запроса
        /// </summary>
        /// <returns></returns>
        public dynamic GetData(string request)
        {
            this.JSONRequest = request;
            GetRequest();
            GetResponse();
            return GetResponseData();
            //return ResponseDataWrapper(GetResponseData());
        }

        /// <summary>
        /// Отправить запрос на сервер
        /// </summary>
        protected void GetRequest()
        {
            string url = "https://online.sbis.ru/service/?srv=1";
            if (this.JSONRequest.Contains("САП.")) url = "https://online.sbis.ru/auth/service/sbis-rpc-service300.dll";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json;charset utf-8";
            if (_settings.AuthGuid != string.Empty)
                httpWebRequest.Headers["X-SBISSessionID"] = _settings.AuthGuid;
            httpWebRequest.Method = "POST";
            WebProxy myProxy = new WebProxy();
            Uri newUri = new Uri(_settings.ProxyUrl);
            myProxy.Address = newUri;
            myProxy.Credentials = CredentialCache.DefaultCredentials;
              new NetworkCredential(_settings.ProxyLogin, _settings.ProxyPass);
            httpWebRequest.Proxy = myProxy;

            this.httpRequest =  httpWebRequest;
        }
        /// <summary>
        /// Получить ответ на запрос
        /// </summary>
        protected void GetResponse()
        {
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(JSONRequest);
                streamWriter.Flush();
                streamWriter.Close();
                try
                {
                    httpResponse = httpRequest.GetResponse();
                }
                catch (System.Net.WebException wex)
                {
                    httpResponse = wex.Response;
                }
            }
        }

        /// <summary>
        /// Получить результат запроса
        /// </summary>
        /// <returns></returns>
        protected string GetResponseData()
        {
            string result = string.Empty;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        protected dynamic ResponseDataWrapper(string data)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer(); 
            return jss.Deserialize<dynamic>(data);
        }
    }
}
