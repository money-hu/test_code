using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace coredemo2._2.Controllers
{


    public class FileController:Controller{
        private string RootPath=System.IO.Directory.GetCurrentDirectory()+"/App_Data/Files/";

        //Post 文件上传
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file){
            
            if(file.Length>0){
                String filePath=RootPath+file.FileName;
                Console.WriteLine("显示文件名称:{0}",filePath);
                using(var stream=System.IO.File.Create(filePath)){
                    await file.CopyToAsync(stream);
                }
                ViewData["rs"]=filePath;
                return Json("Upload success");
            }

        return Json("Upload error");
        }
    
        
        //Get 文件下载
        public IActionResult Download(String FileName){
            String path=RootPath+FileName;
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs,"text/plain",FileName);
        }

    
    }

    
    

}