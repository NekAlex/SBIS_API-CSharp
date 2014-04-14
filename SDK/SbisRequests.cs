using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using RU.NekAlex.Sbis.Model;
using RU.NekAlex.Sbis.Exception;

namespace RU.NekAlex.Sbis.SDK
{


    /// <summary>
    /// Типовые запросы
    /// </summary>
    public static class SBisRequests
    {
        /// <summary>
        /// Метод ВИ.СформироватьДокументПакетаВПечатномВиде
        /// </summary>
        /// <param name="docGuid">Строка GUID-идентификатора документа</param>
        /// <returns></returns>
        public static string СформироватьДокументПакетаВПечатномВиде(string docGuid)
        {
            SBiSJsonRequest sbr = new SBiSJsonRequest("ВИ.СформироватьДокументПакетаВПечатномВиде");
            sbr.Params.SetS("ИдентификаторДокумента", "Строка");
            sbr.Params.SetD("ИдентификаторДокумента", docGuid);
            return sbr.Serialize();
        }
        /// <summary>
        /// Метод ВИ.ПолучитьДокумент
        /// </summary>
        /// <param name="docGuid">Строка GUID-идентификатора документа</param>
        /// <returns></returns>
        public static string ПолучитьДокумент(string docGuid)
        {
            SBiSJsonRequest sbr = new SBiSJsonRequest("ВИ.ПолучитьДокумент");
            sbr.Params.SetS("ИдентификаторДокумента", "Строка");
            sbr.Params.SetD("ИдентификаторДокумента", docGuid);
            return sbr.Serialize();
        }
        /// <summary>
        ///Авторизация на сервере
        /// </summary>
        /// <returns></returns>
        public static string Авторизация(RU.NekAlex.Sbis.SbisSettings _settings)
        {
            //string json = "{\"jsonrpc\":\"2.0\", \"method\":\"САП.Аутентифицировать\", \"params\":{	\"login\": \"nekrasov_aa@wscb.ru\",	\"password\":\"Njyec123\"},\"id\":0	}";
            SBiSJsonRequest sbr = new SBiSJsonRequest("САП.Аутентифицировать") { Params = new SBiSJsonAuthWrapper() };
            sbr.Params.Set(_settings.SbisUser, _settings.SbisPassword);
            return sbr.Serialize();
        }

        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <returns>JSON Запрос для СБиС</returns>
        public static string ПроверитьСессию()
        {
            return new SBiSJsonRequest("САП.ПроверитьСессию").Serialize();
        }
        /// <summary>
        /// Возвращает список документов и ЭП в пакете, имеющихся на текущий момент времени
        /// </summary>
        /// <param name="guid">GUID пакета</param>
        /// <returns>JSON Запрос для СБиС</returns>
        public static string СписокДокументовВПакете(string guid)
        {
            SBiSJsonRequest sbr = new SBiSJsonRequest("ВИ.СписокДокументовВПакете");
            sbr.Params.SetS("ИдентификаторПакетаДокументов", "Строка");
            sbr.Params.SetD("ИдентификаторПакетаДокументов", guid);
            return sbr.Serialize();
        }
        /// <summary>
        /// Получить список событий с даты date
        /// </summary>
        /// <param name="date">ФильтрДатаВремяС</param>
        /// <returns></returns>
        public static string СписокСобытийЗаПериод(string date)
        {
            SBiSJsonRequest sbr = new SBiSJsonRequest("ВИ.СписокСобытийЗаПериод");
            sbr.Params.SetS("ФильтрДатаВремяС", "Строка");
            sbr.Params.SetS("ФильтрНаправлениеПакета", "Строка");
            sbr.Params.SetD("ФильтрДатаВремяС", date);
            sbr.Params.SetD("ФильтрНаправлениеПакета", "Входящие");
            return sbr.Serialize();
        }

        public static string СписокПакетовДокументов(string date)
        {
            SBiSJsonRequest sbr = new SBiSJsonRequest("ВИ.СписокПакетовДокументов");
            sbr.Params.SetS("ФильтрДатаС", "Строка");
            sbr.Params.SetD("ФильтрДатаС", date);
            return sbr.Serialize();
        }
        public static string ИнформацияОДокументе(string guid)
        {
            SBiSJsonRequest sbr = new SBiSJsonRequest("ВИ.ИнформацияОДокументе");
            sbr.Params.SetS("ИдентификаторПакетаДокументов", "Строка");
            sbr.Params.SetD("ИдентификаторПакетаДокументов", guid);
            return sbr.Serialize();
        }

        /// <summary>
        /// Получение ошибки
        /// </summary>
        /// <param name="response">JSON ответ сервера</param>
        /// <returns>Объект ошибка</returns>
        public static SBiSJsonErrorWrapper GetErrorInfo(string response)
        {
            if (!response.Contains("error"))
                throw new ApplicationException("JSON ответ не содержит ошибку");
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var res = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<SBiSJsonResponse>(response);
            return res.error;
        }
        /// <summary>
        /// Возвращает результат запроса
        /// </summary>
        /// <param name="response">JSON ответ сервера</param>
        /// <returns>Объект результат</returns>
        public static dynamic GetResponseInfo(string response)
        {
            if (response.Contains("error"))
                throw new SbisJsonErrorException(response);
                
           /* if (response.Contains("\"result\":\""))
            {
                //результр какой-то

            }
            else*/
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            jss.MaxJsonLength = 36700160;
                var res = jss.Deserialize<SBiSJsonResponse>(response);
                return res.result;
        }
    }




}
