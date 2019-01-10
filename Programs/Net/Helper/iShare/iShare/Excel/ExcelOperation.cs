using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using NPOI.XSSF.UserModel;

namespace iShare.Excel
{
    /// <summary>
    /// Excel数据导入导出
    /// </summary>
    public static class ExcelOperation
    {
        /// <summary>
        /// 数据导出至Excel（1.0）已废弃
        /// </summary>
        [Obsolete]
        public static void Export<T>(DataTable DataTable) where T : new()
        {
            T model = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();
            string TableName = typeof(T).Name;

            //创建一个工作簿
            HSSFWorkbook workbook = new HSSFWorkbook();
            //工作簿中必须带有一个sheet
            ISheet sheet = workbook.CreateSheet(TableName);

            //列名
            IRow cellsOne = sheet.CreateRow(0);
            for (int k = 0; k < properties.Length; k++)
            {
                ICell cell1 = cellsOne.CreateCell(k);
                //获取属性的描述
                DescriptionAttribute desc = (DescriptionAttribute)properties[k].GetCustomAttribute(typeof(DescriptionAttribute));
                cell1.SetCellValue(desc.Description);
                cell1.SetCellType(CellType.String);
            }
            DataTable table = DataTable;
            int RowIndex = table.Rows.Count;
            int ColIndex = table.Columns.Count;
            for (int i = 0; i < RowIndex; i++)
            {
                //创建一行
                IRow cells = sheet.CreateRow(i + 1);
                for (int j = 0; j < ColIndex; j++)
                {
                    sheet.SetColumnWidth(j, 15 * 256);
                    ICell cell = cells.CreateCell(j);
                    cell.SetCellValue(table.Rows[i][j].ToString());
                    //cell.SetCellType(CellType.Numeric);       设置单元格的类型
                    //cell.CellStyle
                }
            }
            using (FileStream fs = File.OpenWrite(@"E:\MVC\excel导入导出\NPOI\NPOI\Content\"+typeof(T).Name+".xls")) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            {
                workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
            }
        }

        /// <summary>
        /// 数据导出至Excel（1.1）不需要导出的字段从最后一个秒的后面存放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="CenterName"></param>
        /// <returns></returns>
        public static bool Export<T>(System.Data.DataTable table, string FilePath) where T : new()
        {
            try
            {
                T model = new T();
                PropertyInfo[] properties = typeof(T).GetProperties();
                string TableName = typeof(T).Name;

                //创建一个工作簿
                HSSFWorkbook workbook = new HSSFWorkbook();
                //工作簿中必须带有一个sheet
                ISheet sheet = workbook.CreateSheet(TableName);

                //列名
                IRow cellsOne = sheet.CreateRow(0);

                //排除所有不需要导出的字段
                List<int> Barring = new List<int>();

                for (int k = 0; k < properties.Length; k++)
                {
                    ICell cell1 = cellsOne.CreateCell(k);
                    //获取属性的描述
                    DescriptionAttribute desc = (DescriptionAttribute)properties[k].GetCustomAttribute(typeof(DescriptionAttribute));
                    if (desc != null)
                    {
                        cell1.SetCellValue(desc.Description);
                        cell1.SetCellType(CellType.String);
                    }
                    else
                    {
                        //字段没带描述（无需导出）
                        Barring.Add(k);
                    }
                }

                int RowIndex = table.Rows.Count;
                int ColIndex = table.Columns.Count;
                for (int i = 0; i < RowIndex; i++)
                {
                    //创建一行
                    IRow cells = sheet.CreateRow(i + 1);
                    for (int j = 0; j < ColIndex; j++)
                    {
                        //当前列，不在排除导出之外
                        if (!Barring.Contains(j))
                        {
                            sheet.SetColumnWidth(j, 15 * 256);
                            ICell cell = cells.CreateCell(j);
                            cell.SetCellValue(table.Rows[i][j].ToString());
                            //cell.SetCellType(CellType.Numeric);       设置单元格的类型
                            //cell.CellStyle
                        }
                    }
                }
                //string excelName = DateTime.Now.ToString("yyyyMMddhhmmss");

                using (FileStream fs = File.OpenWrite(FilePath)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    workbook.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public static void Import<T>(string path) where T:new() 
        {
            
        }

        /// <summary>
        /// Excel转list集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string path) where T : new()
        {
            string Extension = Path.GetExtension(path);
            IWorkbook hSSFWorkbook = null;
            //错误信息
            StringBuilder ErrorBuilder = new StringBuilder();
            //结果
            List<T> TModelList = new List<T>();
            //Excel中的表头信息
            Dictionary<int, string> SheetDictionary = new Dictionary<int, string>();
            //model中的信息
            Dictionary<int, ExcelModel> TModelDictionary = new Dictionary<int, ExcelModel>();

            using (FileStream fs = File.OpenRead(path))
            {
                if (Extension == ".xls")
                {
                    hSSFWorkbook = new HSSFWorkbook(fs);
                }
                else if (Extension == ".xlsx")
                {
                    hSSFWorkbook = new XSSFWorkbook(fs);
                }
                else
                {
                    return null;
                }
                ISheet sheet = hSSFWorkbook.GetSheetAt(0);
                List<int> CellNumber = new List<int>();
                //获取列头
                IRow HeadRow = sheet.GetRow(0);
                int EmptyCell = 0;
                for (int i = 0; i < HeadRow.LastCellNum; i++)
                {
                    if (HeadRow.GetCell(i) == null)
                    {
                        EmptyCell = 1;
                        continue;
                    }
                    if (EmptyCell != 0)
                    {
                        SheetDictionary.Add(i-1, HeadRow.GetCell(i).StringCellValue);
                        EmptyCell = 0;
                    }
                    else
                    {
                        SheetDictionary.Add(i, HeadRow.GetCell(i).StringCellValue);
                    }
                }
                //获取对象中所有描述、字段信息
                MemberInfo[] propertyInfos = typeof(T).GetProperties();
                for (int k = 0; k < propertyInfos.Length; k++)
                {
                    ExcelModel excel = new ExcelModel();
                    //获取描述
                    DescriptionAttribute desc = (DescriptionAttribute)propertyInfos[k].GetCustomAttribute(typeof(DescriptionAttribute));
                    if (desc != null)
                    {
                         excel.Description = desc.Description;
                        //获取属性
                         excel.Property = (PropertyInfo)propertyInfos[k];
                         TModelDictionary.Add(k, excel);
                    }
                }
                //获取处头部之外的所有行
                for (int j = 1; j <= sheet.LastRowNum; j++)
                {
                    T model = new T(); ;
                    IRow row = sheet.GetRow(j);
                    for (int p = 0; p < row.PhysicalNumberOfCells; p++)
                    {
                        ICell cell = row.GetCell(p);
                        //查找Excel字典中与model字典中对应的字段
                        ExcelModel excelModel = TModelDictionary.Where(x => x.Value.Description == SheetDictionary[p]).Select(x => x.Value).FirstOrDefault();
                        if (excelModel != null && cell != null)
                        {
                            //Numeric(int，double，datetime)
                            if (cell.CellType == CellType.Numeric)
                             {
                                if (HSSFDateUtil.IsCellDateFormatted(cell) && excelModel.Property.PropertyType == typeof(DateTime))
                                {
                                    excelModel.Property.SetValue(model,cell.DateCellValue);
                                }
                                else if(excelModel.Property.PropertyType == typeof(double))
                                {
                                    excelModel.Property.SetValue(model, Double.Parse(cell.NumericCellValue.ToString()));
                                }
                                else
                                {
                                    excelModel.Property.SetValue(model, int.Parse(cell.NumericCellValue.ToString()));
                                }
                             }
                             else if (cell.CellType == CellType.String && excelModel.Property.PropertyType == typeof(string))
                             {
                                  excelModel.Property.SetValue(model, cell.StringCellValue);
                             }
                             else
                             {
                                ErrorBuilder.Append("第" + j + "行的第" + p + "列数据格式不正确！");
                             }
                        }
                    }
                    if (model!= null)
                    {
                        TModelList.Add(model);
                    }
                }
            }
            return TModelList.Count == 0 ? null : TModelList;
        }
    }
    //泛型对象信息存储
    public class ExcelModel
    {
        public string Description { get; set; }

        public PropertyInfo Property { get; set; }
    }
}