SBIS_EDO_Net
============

C# реализация интерфейса API СБИ ЭДО http://help.sbis.ru/exchange/integration


##########Объект параметров
SbisSettings ss = new SbisSettings();
ss.AuthGuid = "004f7bdf-0066e085-00ba-8042c152a6c74422";
ss.LastDocDate = "01-01-2010";
ss.ProxyLogin = "proxyUser";
ss.ProxyPass = "proxyPass";
ss.ProxyUrl = "http://proxyUrl:3128;
ss.SbisUser = "sbisUser";
ss.SbisPassword = "sbisPassword";
ss.WorkDir = System.IO.Path.Combine("C:\\", "sbis");
Sbis sm = new Sbis(ss);
sm.PrepareInputFolder();
