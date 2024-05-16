using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder Path

            //string folderPath = "D:\\Route_C41_Back\\Assignments solution\\MVC_Ass02\\Demo.PL\\wwwroot\\Files\\" + folderName;
            //string folderPath = Directory.GetCurrentDirectory() + "wwwroot\\Files" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            //-------------------------------------------------
            // 2. Get File Name and Make it Unique

            string fileName = $"{Guid.NewGuid()}{file.FileName}"; // to Make it Unique
            //-------------------------------------------------
            // 3. Get File Path [Folder Path + File Name]

            string filePath = Path.Combine(folderPath, fileName);
            //-------------------------------------------------
            // 4. save file as streams

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            // 1. get file path

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);

            //----------------------------------------------------------

            // 2. check if file exists or not if exists remove it

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
