using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace iShare.Logs
{
    /// <summary>
    /// IO文件操作
    /// </summary>
    public static class FileOperation
    {
        /// <summary>
        /// 文件写入(文件地址：D:\Logs)
        /// </summary>
        /// <param name="ErrorAddr">错误地址</param>
        /// <param name="ErrorMsg">错误信息</param>
        public static void FileWrite(string ErrorMsg,string ErrorAddr,string Remark)
        {
            string DirectoryPath = @"D:\Logs";

            //检测该文件夹是否存在
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            //txt文件名称
            string FilePath = DirectoryPath + "/"+DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()+"x.txt";
            //检测文件是否存在
            if (!System.IO.File.Exists(FilePath))
            {
                FileStream stream = System.IO.File.Create(FilePath);
                stream.Close();
                stream.Dispose();
            }

            using (FileStream fileStream = new FileStream(FilePath,FileMode.Append,FileAccess.Write))
            {
                string WriterContent = "\r\n\r\n写入时间："+DateTime.Now;
                WriterContent += "\r\n备注：" + Remark;
                WriterContent += "\r\n错误地址：" + ErrorAddr;
                WriterContent +="\r\n错误描述：" + ErrorMsg;

                StreamWriter stream = new StreamWriter(fileStream);
                stream.Write(WriterContent);

                stream.Close();
                stream.Dispose();
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public static string GetContentByFile(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            using (FileStream fileSteam = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                StreamReader streamReader = new StreamReader(fileSteam);
                string str = streamReader.ReadToEnd();

                streamReader.Close();
                streamReader.Dispose();

                return str;
            }
        }

    }
}
