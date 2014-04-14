using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RU.NekAlex.Sbis.Model
{
    /// <summary>
    /// Запрос СБиС
    /// </summary>
    public sealed class SBiSJsonRequest
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public ISBiSParam Params { get; set; }
        public string id { get; set; }
        public SBiSJsonRequest(string Method)
        {
            jsonrpc = "2.0";
            id = "0";
            method = Method;
            Params = new SBiSJsonRecordWrapper();
        }
        public string Serialize()
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this).Replace("\"Params\":", "\"params\":").Replace("{\"ВходныеДанные\":{\"s\":{},\"d\":{}}}", "{}");
        }

    }
    /// <summary>
    /// Обертка для авторизации
    /// </summary>
    public sealed class SBiSJsonAuthWrapper : ISBiSParam
    {
        public string login { get; set; }
        public string password { get; set; }

        public void Set(string login, string password)
        {
            this.login = login;
            this.password = password;
        }


        public void SetS(string key, dynamic value)
        {
            throw new NotImplementedException();
        }

        public void SetD(string key, dynamic value)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Обертка для типа "Входные данные" / Запись
    /// </summary>
    public sealed class SBiSJsonRecordWrapper : ISBiSParam
    {
        public SBiSJsonRecord ВходныеДанные { get; set; }
        public SBiSJsonRecordWrapper()
        {
            this.ВходныеДанные = new SBiSJsonRecord();
        }
        /// <summary>
        /// Объект описание
        /// </summary>
        /// <param name="Key">Атрибут</param>
        /// <param name="Value">Значение</param>
        protected void SetSAttrib(string Key, dynamic Value)
        {
            this.ВходныеДанные.s.Add(Key, Value);
        }
        /// <summary>
        /// Объект
        /// </summary>
        /// <param name="Key">Атрибут</param>
        /// <param name="Value">Значение</param>
        protected void SetDAttrib(string Key, dynamic Value)
        {
            this.ВходныеДанные.d.Add(Key, Value);
        }

        public void Set(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void SetS(string key, dynamic value)
        {
            this.SetSAttrib(key, value);
        }

        public void SetD(string key, dynamic value)
        {
            this.SetDAttrib(key, value);
        }
    }
    /// <summary>
    /// Интерфейс params
    /// </summary>
    public interface ISBiSParam
    {
        void Set(string login, string password);
        void SetS(string key, dynamic value);
        void SetD(string key, dynamic value);
    }
    /// <summary>
    /// Тип данных "Запись"
    /// </summary>
    public sealed class SBiSJsonRecord
    {
        public Dictionary<string, object> s { get; set; }
        public Dictionary<string, object> d { get; set; }
        public SBiSJsonRecord()
        {
            s = new Dictionary<string, object>();
            d = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// Тип данных "Запись"
    /// </summary>
    public sealed class SBiSJsonResultWrapper
    {
        //"{\"jsonrpc\":\"2.0\",\"result\":{\"s\":{\"ДатаВремяЗапроса\":\"Строка\",\"ВерсияИнтерфейса\":\"Строка\",\"СостояниеИнтерфейса\":\"Строка\"},\"d\":{\"ДатаВремяЗапроса\":\"31-03-2014 10:11:14\",\"ВерсияИнтерфейса\":\"1.16.7\",\"СостояниеИнтерфейса\":\"Готов\"}},\"id\":0}"
        public Dictionary<string, dynamic> s { get; set; }
        public Dictionary<string, dynamic> d { get; set; }
        public string ИмяФайла { get; set; }
        public string Ид { get; set; }
        public string Данные { get; set; }

        public SBiSJsonResultWrapper()
        {
            s = new Dictionary<string, dynamic>();
            d = new Dictionary<string, dynamic>();
        }
        public SBiSJsonResultWrapper(string text):base()
        {
            ИмяФайла = text;
        }
        public void SetS(string key, dynamic value)
        {
            s.Add(key, value);
        }

        public void SetD(string key, dynamic value)
        {
            d.Add(key, value);
        }
    }
    /// <summary>
    /// Обертка типа "Файл"
    /// </summary>
    public sealed class SBiSJsonFileWrapper
    {
        public SBiSJsonFile ВходнойФайл { get; private set; }
        public SBiSJsonFileWrapper()
        {
            this.ВходнойФайл = new SBiSJsonFile();
        }
        public void SetId(string id) { this.ВходнойФайл.Ид = id; }
        public void SetName(string name) { this.ВходнойФайл.ИмяФайла = name; }
        public void SetData(string data) { this.ВходнойФайл.Данные = data; }
    }
    /// <summary>
    /// Тип данных "Файл"
    /// </summary>
    public sealed class SBiSJsonFile
    {
        public string ИмяФайла { get; set; }
        public string Ид { get; set; }
        public string Данные { get; set; }
        public SBiSJsonFile()
        {

        }

    }
    /// <summary>
    /// Обертка "Ответ СБиС на Запрос"
    /// </summary>
    public sealed class SBiSJsonResponse
    {
        //"{\"jsonrpc\":\"2.0\",\"result\":\"0066e085\",\"id\":\"0\"}"
        public string jsonrpc { get; set; }
        public SBiSJsonErrorWrapper error { get; set; }
        public dynamic result { get; set; }
        public string id { get; set; }
    }
    public class SBiSJsonErrorWrapper
    {
        //{\"jsonrpc\":\"2.0\",\"error\":{\"code\":-32000,\"message\":\"\",\"details\":\"Пользователь не аутентифицирован!\",\"type\":\"warning\",\"data\":{\"classid\":\"{afd28339-dc44-4ad9-96dc-55a9789c743a}\",\"addinfo\":null}},\"id\":\"0\"}"
        public int code { get; set; }
        public string details { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public Dictionary<string, dynamic> data { get; set; }
        public override string ToString()
        {
            return string.Format("Ошибка № {0}. Тип ошибки {1}. Описание {2}. Соощение {3}.", this.code, this.type, this.details,this.message);
            //return base.ToString();
        }
    }


 }
