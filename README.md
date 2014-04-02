SBIS_EDO_Net
============

C# реализация интерфейса API СБИ ЭДО http://help.sbis.ru/exchange/integration


##########Объект параметров<br/>
SbisSettings ss = new SbisSettings();<br/>
ss.AuthGuid = "004f7bdf-0066e085-00ba-8042c152a6c74422";<br/>
ss.LastDocDate = "01-01-2010";<br/>
ss.ProxyLogin = "proxyUser";<br/>
ss.ProxyPass = "proxyPass";<br/>
ss.ProxyUrl = "http://proxyUrl:3128;<br/>
ss.SbisUser = "sbisUser";<br/>
ss.SbisPassword = "sbisPassword";<br/>
ss.WorkDir = System.IO.Path.Combine("C:\\", "sbis");<br/>
Sbis sm = new Sbis(ss);<br/>
sm.PrepareInputFolder();<br/>
