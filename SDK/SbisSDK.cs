using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RU.NekAlex.Sbis.SDK
{
    [System.Xml.Serialization.XmlRoot("Конверт")]
    public class SBiSKonvert
    {
        public string ТипДокумента { get; set; }
        public SBiSKonvertWho Отправитель { get; set; }
        public SBiSKonvertWho Получатель { get; set; }
        public string Дата { get; set; }
        public string Номер { get; set; }
        public string Примечание { get; set; }
        [System.Xml.Serialization.XmlArray]
        public List<SBiSKonverAtta> Вложения { get; set; }
        public SBiSKonvert()
        {
            Отправитель = new SBiSKonvertWho();
            Получатель = new SBiSKonvertWho();
            Вложения = new List<SBiSKonverAtta>();
        }
    }
    public class SBiSKonvertWho
    {
        [System.Xml.Serialization.XmlAttribute]
        public string ИНН { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string КПП { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Название { get; set; }
    }
    [System.Xml.Serialization.XmlType("Вложение")]
    public class SBiSKonverAtta
    {
        public string ИдентификаторДокумента { get; set; }
        public string ИмяФайла { get; set; }
        public string Название { get; set; }
        public string Версия { get; set; }
        public string Выгружен { get; set; }
        public string Служебный { get; set; }
        public string ЧислоПодписей { get; set; }
        public string ТипДокумента { get; set; }
        public string Человечный { get; set; }
    }
}
