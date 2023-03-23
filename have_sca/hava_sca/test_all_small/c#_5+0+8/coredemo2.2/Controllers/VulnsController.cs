using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using coredemo2._2.Models;
using MySql.Data.MySqlClient;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Net;
using System.DirectoryServices;
using Newtonsoft.Json;
using System.Configuration;

namespace coredemo2._2.Controllers
{


        class TestTraceListener : TraceListener
    {

        public String path { get; set; }

        public void pathExists(String path){
            if(!File.Exists(path)){
                File.Create(path).Close();
            }
            this.path=path;
        }
        public override void  WriteLine(String s)
        {
            File.AppendAllLinesAsync(path,s.Split(" "));
        }
        public override  void Write(String s)
        {
            File.AppendAllTextAsync(path,s);
        }
    }

    
    public class VulnsController:Controller{

        private string RootPath=System.IO.Directory.GetCurrentDirectory();


        // GET 目录穿越

        public IActionResult dir(){
        
            String path=Request.Query["path"];
            if(path!=null){
                DirectoryInfo root=new DirectoryInfo(RootPath+"/"+path);
                List<String> list=new List<string>();
                foreach(DirectoryInfo d in root.GetDirectories()){
                    String name=d.Name;
                    String fullName=d.FullName;
                    Console.WriteLine("fullName:"+name+fullName);
                    list.Add(name+"/");
                }
                foreach(FileInfo f in root.GetFiles()){
                    String name=f.Name;
                    String fullName=f.FullName;
                    list.Add(name);
                }
                ViewData["files"]=list;
            }

            return View();
        }

        // GET XSS注入反射型
        public IActionResult xss(){
            String name= Request.Query["name"];
            String age=Request.Query["age"];
            String message =Request.Query["message"];
            string mode=Request.Query["mode"];
            if (age != null && name != null) {
                var p=new Userinfo(){
                    Name=name,
                    Age=age
                };
                if (mode != null) {
                    return View(p);
                }
                ViewData["person"] = p;

            }else if(message !=null){
                ViewBag.Message = message;
            }

            return View();
        }
        

        // public IActionResult

        public IActionResult xss2(){
            String name=Request.Query["name"];
            String bz=Request.Query["bz"];
            if(name!=null){
                MysqlConn dbconn=new MysqlConn();
                bool flag=dbconn.OpenConn();
                if(!flag){
                    ViewData["error"]="数据库连接错误!";
                }else{
                    Userinfo rs=dbconn.selectByname(name);
                    if(rs!=null){
                        ViewData["rs"]=rs;
                    }else{
                        ViewData["error"]="不存在";
                    }
                }
            }
            if(bz!=null&name!=null){
                MysqlConn dbconn=new MysqlConn();
                bool flag=dbconn.OpenConn();
                if(!flag){
                    ViewData["error"]="数据库连接错误!";
                }else{
                    Boolean rs=dbconn.updateByname(name,bz);
                }
            }                                                                                          

            return View();
        }

        // GET SQL注入
        public IActionResult sql(){
            String name=Request.Query["name"];
            if(name!=null){
                MysqlConn dbconn=new MysqlConn();
                bool flag=dbconn.OpenConn();
                if(!flag){
                    ViewData["error"]="数据库连接错误！";
                }else{
                    Userinfo rs=dbconn.selectByname(name);
                    if(rs!=null){
                        ViewData["rs"]=rs;
                    }else{
                        ViewData["error"]="该用户不存在";
                    }
                }
            }

            return View();
        }


        public IActionResult log(){
            DateTime date=DateTime.Now;
            String name=Request.Query["guestname"];
            String path=RootPath+"/App_Data/Logs/"+date.ToString("yyyy-MM-dd'.txt'");
            TestTraceListener traceListener = new TestTraceListener();
            traceListener.pathExists(path);

            if(name!=null){
                
                traceListener.Write("Guest :"+name+" visits the log page\n");
                traceListener.Close();
            }
            StreamReader sr=new StreamReader(path);
            string content;
            StringBuilder builder=new StringBuilder();
            while((content=sr.ReadLine())!=null){
                builder.Append(content.ToString());
                builder.Append("<br/>");
            }
            sr.Close();
            ViewData["rs"]=new Microsoft.AspNetCore.Html.HtmlString(builder.ToString());

            return View();
        }

        //Get Xpath注入
        public IActionResult xpath(){
            return View();
        }
        //Post xpath
        [HttpPost]
        public IActionResult xpaths(){
            String XmlString="<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<bookstore>" +
                    "<user role=\"1\" username=\"admin\" password=\"admin888\"></user>" +
                    "<user role=\"1\" username=\"root\" password=\"root123\"></user>" +
                    "<user role=\"2\" username=\"guest\" password=\"123456\"></user>" +
                "</bookstore>";
            String username=Request.Form["username"];
            String password=Request.Form["password"];
            XmlDocument doc=new XmlDocument();
            doc.LoadXml(XmlString);
            XmlElement root=doc.DocumentElement;
            XmlNodeList ParameterNodes=root.SelectNodes("//bookstore/user[@username='" + username + "' and @password='" + password + "']");
            Console.WriteLine("xmlPayload:"+"//bookstore/user[@username='" + username + "' and @password='" + password + "']");
            if(ParameterNodes.Count!=0){
                return View("~/Views/Vulns/Result/Success.cshtml");
            }else{
                return View("~/Views/Vulns/Result/Error.cshtml");
            }
        }

        //ssrf
        public IActionResult ssrf(){
            String url=Request.Query["url"];
            if(url==null){
                return View();
            };

            HttpWebRequest httpWebRequest=(HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.Method="GET";
            httpWebRequest.Timeout=20000;
            HttpWebResponse httpWebResponse=(HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader=new StreamReader(httpWebResponse.GetResponseStream(),Encoding.UTF8);
            ViewData["rs"]=streamReader.ReadToEnd();
            streamReader.Close();
            return View();
        }

        public IActionResult redirect(){
            String url=Request.Query["url"];
            String mode=Request.Query["mode"];
            if(url==null){
                return View();
            }else if(mode!=null&&mode.Equals("resp")){
                Response.Redirect(url);
                return View();
            }
            return Redirect(url);
        }
        // Get ldapLogin 
        public IActionResult ldapLogin(){
            string method = Request.Method;
            if(method.Equals("POST")){
                String username=Request.Form["username"];
                String passwd=Request.Form["password"];
                LdapHelper ldap=new LdapHelper();
                DirectoryEntry de=ldap.OpenConnection("/dc=maxcrc,dc=com");
                if(de!=null){
                    DirectorySearcher deSearch = new DirectorySearcher();
                    deSearch.SearchRoot=de;
                    deSearch.Filter="(&(ou="+username+")(userPassword="+passwd+"))";
                    SearchResultCollection results=deSearch.FindAll();
                    if(results.Count==0){
                        return View("~/View/Vulns/Result/Error.cshtml");
                    }else{
                        return View("~/View/Vulns/Result/Success.cshtml");
                    }
                }else{
                    ViewData["error"]="Ldap连接失败";
                }
            }
            return View();
        }
        // Get xxe
        public IActionResult xxe(){
            String id=Request.Query["id"];
            if(id!=null){
                String querystring = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE foo [ <!ELEMENT foo ANY >  <!ENTITY xxe SYSTEM \"_EXTERNAL_FILE_\" >]><foo>&xxe;</foo>";
                var xml = querystring.Replace("_EXTERNAL_FILE_", Request.Query["id"]);
                XmlReaderSettings settings=new XmlReaderSettings();
                settings.DtdProcessing=DtdProcessing.Parse;
                settings.XmlResolver = new XmlUrlResolver();
                using(MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml))){
                    XmlReader reader = XmlReader.Create(stream, settings);
                    XmlDocument xmlDocument=new XmlDocument();
                    xmlDocument.XmlResolver = new XmlUrlResolver();
                    xmlDocument.Load(reader);
                    XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("foo");
                    String tmp=xmlNodeList.Item(0).InnerText;
                    ViewData["rs"]=tmp;
                    }
            }
            return View();
        }

        //Get 反序列化
        public IActionResult deserialization(){
            // Userinfo user=new Userinfo();
            // user.Name="admin";
            // user.Password="admin123";
            // Object obj= new object();
          
            // String testString=JsonConvert.SerializeObject(user,new JsonSerializerSettings{
            //     NullValueHandling =NullValueHandling.Ignore,
            //     TypeNameAssemblyFormatHandling=TypeNameAssemblyFormatHandling.Full,
            //     TypeNameHandling=TypeNameHandling.All,

            // });
            // Console.WriteLine("Json输出:  {0}",testString);

            // String strContent="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";
            // var obj =new WindowsIdentityTest(strContent);
            // String obj1=JsonConvert.SerializeObject(obj,new JsonSerializerSettings{
            //     TypeNameHandling=TypeNameHandling.All,
            //     TypeNameAssemblyFormatHandling =TypeNameAssemblyFormatHandling.Full,
            // });
            // Console.WriteLine("obj输出:{0}",obj1);

            // Object obj=JsonConvert.DeserializeObject<Object>(Request.Query["id"],new JsonSerializerSettings{
            //     TypeNameHandling=TypeNameHandling.Auto
            // });
            return View();
        }

        //Get 命令执行
        public IActionResult command(){
            String cmd=Request.Query["cmd"];
            if(cmd==null){
                return View();
            }
            Process p=new Process();

            p.StartInfo.FileName="cmd.exe";

            p.StartInfo.UseShellExecute=false;

            p.StartInfo.RedirectStandardInput=true;

            p.StartInfo.RedirectStandardOutput=true;

            p.StartInfo.RedirectStandardError=true;

            p.StartInfo.CreateNoWindow=true;

            p.Start();

            p.StandardInput.WriteLine(cmd+"&exit");

            p.StandardInput.AutoFlush=true;

            String StrOutput=p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            ViewData["rs"]=StrOutput;


            return View();
        }

        //Get 文件上传跳转
        public IActionResult fileUpload(){

            return View();
        }

        
        //Get 文件下载跳转
        public IActionResult fileDownload(){
            return View();
        }

        //Get Header注入
        public IActionResult headerInjection(){
            String name=Request.Query["name"];
            String value=Request.Query["value"];
            if(name!=null&&value!=null){
                Response.Headers[name]="test";
                Response.Headers["Test-Header"]=value;
                ViewData["rs"]="name:"+name+",value: "+value;
            }
            return View();
        }

        public IActionResult gdpr(){
            String name="法外狂徒张三";
            String phone="18137100080";
            String email="zhangsan@qq.com";
            String idacard="621225198610062040";
            String Address="广东省广州市增城区中新镇张三花园小区6栋56号";
            ViewData["name"]=name;
            ViewData["phone"]=phone;
            ViewData["email"]=email;
            ViewData["idcard"]=idacard;
            ViewData["address"]=Address;

            return View();
        }

    }

}