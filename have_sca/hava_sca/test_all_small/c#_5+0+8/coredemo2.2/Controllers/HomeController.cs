using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using coredemo2._2.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Web;
using System.Xml.XPath;
using System.DirectoryServices;
using System.Xml;
using System.IO;
using System.Text;
namespace coredemo2._2.Controllers
{


    class TestJsonReader : JsonReader
    {
        public override bool Read()
        {
            return false;
        }
    }

    class TestDataType
    {
    
        public string name;
        public int age;
    }

    class UnsafeXmlResolver : XmlResolver
    {
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return null;
        }
    }



    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
