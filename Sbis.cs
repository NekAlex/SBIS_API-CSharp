using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RU.NekAlex.Sbis.Exception;
using RU.NekAlex.Sbis.Model;
using RU.NekAlex.Sbis.SDK;

namespace RU.NekAlex.Sbis
{
    public class Sbis
    {
        protected SbisSettings Settings { get; private set; }
        public string Request { get; private set; }
        public string Response { get; private set; }
        public Sbis(SbisSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Проверить текущую авторизацию
        /// </summary>
        /// <param name="guid">GUID Сессии</param>
        /// <returns></returns>
        protected bool CheckSession()
        {
            var result = new SbisRequestWrapper(Settings).GetData(SBisRequests.ПроверитьСессию());
            try
            {
                var res = SBisRequests.GetResponseInfo(result);
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Авторизоваться на сервере
        /// </summary>
        /// <returns></returns>
        protected void Authorization()
        {
            var result = new SbisRequestWrapper(Settings).GetData(SBisRequests.Авторизация(Settings));
            try
            {
                var res = SBisRequests.GetResponseInfo(result);
                Settings.AuthGuid = res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }

        }
        /// <summary>
        /// Сформировать печатный вариант
        /// </summary>
        /// <param name="docGuid"></param>
        /// <returns></returns>
        protected dynamic FilePechatRequest(string docGuid)
        {
            if (!CheckSession())
                Authorization();
            Request = SBisRequests.СформироватьДокументПакетаВПечатномВиде(docGuid);
            Response= new SbisRequestWrapper(Settings).GetData(Request);
            try
            {
                var res = SBisRequests.GetResponseInfo(Response);
                return res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }

        }
        /// <summary>
        /// Получить документ
        /// </summary>
        /// <param name="docGuid"></param>
        /// <returns></returns>
        protected dynamic GetDocumentRequest(string docGuid)
        {
            if (!CheckSession())
                Authorization();
            var result = new SbisRequestWrapper(Settings).GetData(SBisRequests.ПолучитьДокумент(docGuid));
            try
            {
                var res = SBisRequests.GetResponseInfo(result);
                return res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }

        }
        /// <summary>
        /// Список событий за период
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public dynamic GetChanges(string date)
        {
            if (!CheckSession())
                Authorization();
            Request = SBisRequests.СписокСобытийЗаПериод(date);
            Response = new SbisRequestWrapper(Settings).GetData(Request);
            try
            {
                var res = SBisRequests.GetResponseInfo(Response);
                return res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }
        }
        /// <summary>
        /// Список пакетов документов
        /// </summary>
        /// <returns></returns>
        protected dynamic DocPackList()
        {
            if (!CheckSession())
                Authorization();
            Request = SBisRequests.СписокПакетовДокументов(Settings.LastDocDate);
            Response = new SbisRequestWrapper(Settings).GetData(Request);
            try
            {
                var res = SBisRequests.GetResponseInfo(Response);
                return res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }
        }
        /// <summary>
        /// Получить документы в пакете
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected dynamic GetDocListInPack(string guid)
        {
            if (!CheckSession())
                Authorization();
            Request = SBisRequests.СписокДокументовВПакете(guid);
            Response = new SbisRequestWrapper(Settings).GetData(Request);
            try
            {
                var res = SBisRequests.GetResponseInfo(Response);
                return res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }
        }

        /// <summary>
        /// Человекочитаемые документы
        /// </summary>
        /// <param name="docGuid"></param>
        /// <returns></returns>
        protected dynamic GetHumanReadebleDocument(string docGuid, string package)
        {
            if (!CheckSession())
                Authorization();
            var FI = FilePechatRequest(docGuid);
            string baseDir = System.IO.Path.Combine(Settings.WorkDir, package);
            string fullPath = System.IO.Path.Combine(baseDir, FI["ИмяФайла"]);
            if (!System.IO.Directory.Exists(baseDir))
                System.IO.Directory.CreateDirectory(baseDir);
            System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create);
            fs.Write(Convert.FromBase64String(FI["Данные"]), 0, Convert.FromBase64String(FI["Данные"]).Length);
            fs.Close();
            return FI;
        }
        /// <summary>
        /// Список событий за период
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public dynamic GetDocInfo(string guid)
        {
            if (!CheckSession())
                Authorization();
            Request = SBisRequests.ИнформацияОДокументе(guid);
            Response = new SbisRequestWrapper(Settings).GetData(Request);
            try
            {
                var res = SBisRequests.GetResponseInfo(Response);
                return res;
            }
            catch (SbisJsonErrorException ser)
            {
                Console.WriteLine(ser);
                throw ser;
            }
        }

        /// <summary>
        /// Обработка входящих
        /// </summary>
        public void PrepareInputFolder()
        {
            if (!CheckSession())
                Authorization();
            var res = DocPackList();
            foreach (var pack in res["d"]["СписокПакетовДокументов"]["d"])
            {
                SBiSKonvert sk = new SBiSKonvert();
                sk.Дата = pack[1];
                sk.Номер = pack[2];
                sk.Отправитель.ИНН = pack[3];
                sk.Отправитель.КПП = pack[4];
                sk.Отправитель.Название = pack[5];
                sk.Получатель.ИНН = pack[6];
                sk.Получатель.КПП = pack[7];
                sk.Получатель.Название = pack[8];
                //sk.Примечание = pack[9];
                sk.ТипДокумента = pack[9];

                var docs = GetDocListInPack(pack[0]);
                foreach (var doc in docs["d"]["СписокДокументов"]["d"])
                {
                    if (doc[6] == "Нет")
                    {
                        string HRName = GetDocument(doc[0], pack[0])["ИмяФайла"];
                        if (System.IO.Path.GetExtension(doc[1]) == ".xml")
                        {
                            HRName = GetHumanReadebleDocument(doc[0], pack[0])["ИмяФайла"];
                            
                        }
                        else
                            Console.WriteLine("Не ХМЛ");

                        sk.Вложения.Add(new SBiSKonverAtta { ИдентификаторДокумента = doc[0], ИмяФайла = doc[1], Название = doc[3], Версия = doc[2], Выгружен = doc[5], Служебный = doc[6], ЧислоПодписей = doc[7], ТипДокумента = doc[8],Человечный =HRName });
                    }
                }
                string baseDir = System.IO.Path.Combine(Settings.WorkDir, pack[0]);
                string fullPath = System.IO.Path.Combine(baseDir, "KONVERT.xml");


                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(SBiSKonvert));
                System.Xml.Serialization.XmlSerializerNamespaces namespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                System.IO.TextWriter writer = new System.IO.StreamWriter(fullPath);
                ser.Serialize(writer, sk, namespaces);
                writer.Close();

            }
        }
        /// <summary>
        /// Получить документ
        /// </summary>
        /// <param name="docGuid">ИдентификаторДокумента</param>
        /// <param name="package">ИдентификаторПакета</param>
        /// <returns></returns>
        protected dynamic GetDocument(string docGuid, string package)
        {
            if (!CheckSession())
                Authorization();
            var FI = GetDocumentRequest(docGuid);
            string baseDir = System.IO.Path.Combine(Settings.WorkDir, package);
            string fullPath = System.IO.Path.Combine(baseDir, FI["ИмяФайла"]);
            if (!System.IO.Directory.Exists(baseDir))
                System.IO.Directory.CreateDirectory(baseDir);
            System.IO.FileStream fs = new System.IO.FileStream(fullPath, System.IO.FileMode.Create);
            fs.Write(Convert.FromBase64String(FI["Данные"]), 0, Convert.FromBase64String(FI["Данные"]).Length);
            fs.Close();
            return FI;
        }

    }


    public class SbisSettings
    {
        public string AuthGuid { get; set; }
        public string WorkDir { get; set; }
        public string LastDocDate { get; set; }
        public string SbisUser { get; set; }
        public string SbisPassword { get; set; }
        public string ProxyUrl { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPass { get; set; }
    }
}
