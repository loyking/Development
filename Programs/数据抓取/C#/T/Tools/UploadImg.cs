using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;

namespace Tools
{
    public class UploadImg
    {
        #region 上传图片
        //public static bool Upload(ICollection<HttpPostedFileBase> files, string path, string tpath, ref Hashtable hash)
        //{

        //    string filePath = string.Empty;
        //    int i = 1;

        //    if (files == null || files.Count < 1)
        //    {
        //        hash.Add("Error", "未添加图片");
        //        return false;
        //    }

        //    if (Directory.Exists(path) == true)//如果存在就删除file文件夹
        //    {
        //        DirectoryInfo dir = new DirectoryInfo(path);
        //        dir.Delete(true);
        //    }

        //    //创建file文件夹
        //    Directory.CreateDirectory(path);
        //    Directory.CreateDirectory(path + tpath);

        //    foreach (HttpPostedFileBase file in files)
        //    {
        //        if (file != null)
        //        {
        //            string pictureName = (i++).ToString() + Path.GetExtension(file.FileName);
        //            filePath = Path.Combine(path, pictureName);
        //            try
        //            {
        //                file.SaveAs(filePath);
        //            }
        //            catch
        //            {
        //                hash.Add("Error", "IDCard image upload failed");
        //                return false;
        //            }
        //            string resizeFilePath = Path.Combine(path + tpath, pictureName);
        //            if (!MakeThumbnail(filePath, resizeFilePath, 654, 654, ref hash))
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}
        #endregion

        #region 生成缩略图
        private static bool MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        {
            Image originalImage;
            try
            {
                originalImage = System.Drawing.Image.FromFile(originalImagePath);
            }
            catch
            {
                
                return false;
            }
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            if (ow >= oh)
            {
                towidth = originalImage.Width * height / originalImage.Height;
            }
            else
            {
                toheight = originalImage.Height * width / originalImage.Width;
            }

            Image bitmap = new Bitmap(towidth, toheight);

            Graphics g = Graphics.FromImage(bitmap);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);

            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);
            originalImage.Dispose();
            bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Dispose();
            g.Dispose();
            return true;
        }
        #endregion

        #region 获取图片路径
        //public static string[] GetFileChildName(string path, Guid familyID, string resize = null)
        //{
        //    List<string> filePath = new List<string>();
        //    DirectoryInfo folder = new DirectoryInfo(path + familyID.ToString() + "/" + resize);            
        //    DateTime applyTime = DAL.App.FamilyDAL.GetApplyTime(familyID);
        //    foreach (FileInfo file in folder.GetFiles("*"))
        //    {
        //        if (!file.FullName.Contains("Thumbs.db"))
        //        {
        //            filePath.Add("https://cdn.ac.sxkid.com/img/Uploads/" + familyID.ToString() + "/" + resize + file.Name + "?t=" + applyTime.Ticks);
        //            //filePath.Add("https://cdn.cdsng.sxkid.com/Img/Uploads/" + entryID.ToString() + "/" + resize + file.Name);}=
        //        }
        //    }
        //    return filePath.ToArray();
        //}
        #endregion

        // if (UploadImg.Upload(files, HttpContext.Server.MapPath("/Img/Uploads/" + userID.ToString() + "/"), "Resize\\", ref hash))

        public static bool UploadImgBase64(string source, string path)
        {
            if (Directory.Exists(path) == true)//如果存在就删除file文件夹
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                dir.Delete(true);
            }

            string[] sources = source.Split(',');

            //创建file文件夹
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "Resize\\");
            bool isEmpty = true;
            for (int i = 0; i < sources.Length; i++)
            {
                if (!string.IsNullOrEmpty(sources[i]))
                {
                    isEmpty = false;
                    string newPath = Path.Combine(path, (i + 1).ToString() + ".jpg");

                    byte[] buffer = Convert.FromBase64String(sources[i]);
                    MemoryStream ms = new MemoryStream(buffer);
                    Bitmap bmp = new Bitmap(ms);
                    Image returnImage = bmp;
                    try
                    {
                        returnImage.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                        ms.Close();
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                    finally
                    {
                        returnImage.Dispose();
                        ms.Dispose();
                        bmp.Dispose();
                    }
                    string resizeFilePath = Path.Combine(path + "Resize\\", (i + 1).ToString() + ".jpg");
                    if (!MakeThumbnail(newPath, resizeFilePath, 654, 654))
                    {
                        return false;
                    }
                }
            }
            if (isEmpty)
            {
                return false;
            }
            return true;
        }
    }


}
