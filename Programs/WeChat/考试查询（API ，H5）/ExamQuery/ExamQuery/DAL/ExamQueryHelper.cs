using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamQuery.Tools;
using ExamQuery.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ExamQuery.DAL
{
    public class ExamQueryHelper
    {
        /// <summary>
        /// 检测该考生是否已存在
        /// </summary>
        /// <param name="Sfz"></param>
        /// <param name="Zkzh"></param>
        /// <returns></returns>
        public static bool IsRepeat(string Sfz,string Zkzh)
        {
            string sql = "select * from StudentInfo where Sfz= @Sfz and Zkzh=@Zkzh";
            SqlParameter[] parameters = {
                new SqlParameter("@Sfz",Sfz),
                new SqlParameter("@Zkzh",Zkzh)
            };
            return SQLHelper.ExcuteScalar(sql, parameters) != null ? true : false;
        }

        /// <summary>
        /// 录入考生信息
        /// </summary>
        /// <param name="StudentName"></param>
        /// <param name="Sfz"></param>
        /// <param name="Zkzh"></param>
        /// <param name="ProvinceKey"></param>
        /// <param name="CityKey"></param>
        public static void InsertStudentInfo(Examinee examinee)
        {
            if (!IsRepeat(examinee.Sfz, examinee.Zkzh))
            {
                string sql = "insert into StudentInfo values(@StudentName,@StudentId,@Sfz,@Zkzh,@ProvinceKey,@CityKey)";
                SqlParameter[] parameters = {
                new SqlParameter("@StudentName",examinee.StudentName),
                new SqlParameter("@StudentId",examinee.StudentId),
                new SqlParameter("@Sfz",examinee.Sfz),
                new SqlParameter("@Zkzh",examinee.Zkzh),
                new SqlParameter("@ProvinceKey",examinee.ProvinceKey),
                new SqlParameter("@CityKey",examinee.CityKey)
                };
                SQLHelper.ExcuteNonQuery(sql, parameters);
            }
        }

        /// <summary>
        /// 查询考生主键Id
        /// </summary>
        /// <param name="Zkzh"></param>
        /// <param name="Sfz"></param>
        /// <returns></returns>
        public static int GetStudentId(string Zkzh, string Sfz)
        {
            string sql = "select Id from StudentInfo where Sfz= @Sfz and Zkzh=@Zkzh";
            SqlParameter[] parameters = {
                new SqlParameter("@Sfz",Sfz),
                new SqlParameter("@Zkzh",Zkzh)
            };
            return (int)SQLHelper.ExcuteScalar(sql, parameters);
        }

        /// <summary>
        /// 录入考生高考成绩
        /// </summary>
        /// <param name="Zkzh"></param>
        /// <param name="Sfz"></param>
        /// <param name="gk"></param>
        public static void InsertGkScore(string Zkzh, string Sfz,GkScoreResult gk)
        {
            //获取考生主键
            int Id = GetStudentId(Zkzh, Sfz);
            string exits = "select count(1) from GkScore where StudentId = @Id";
            SqlParameter[] sqls = {
                new SqlParameter("@Id",Id)
            };
            //检测该表中是否已经存在该考生成绩
            if ((int)SQLHelper.ExcuteScalar(exits, sqls) == 0)
            {
                    string sql = "insert into GkScore values(@StudentId,@SubjectType,@ChineseScore,@MathematicsScore,@EnglishScore,@ComprehensiveScore,@SumScore)";
                    SqlParameter[] parameters = {
                    new SqlParameter("@StudentId",Id),
                    new SqlParameter("@SubjectType",gk.Type),
                    new SqlParameter("@ChineseScore",gk.ChineseScore),
                    new SqlParameter("@MathematicsScore",gk.MathematicsScore),
                    new SqlParameter("@EnglishScore",gk.EnglishScore),
                    new SqlParameter("@ComprehensiveScore",gk.ComprehensiveScore),
                    new SqlParameter("@SumScore",gk.SumScore)
                };
                SQLHelper.ExcuteNonQuery(sql, parameters);
            }
        }

        /// <summary>
        /// 录入高考录取结果
        /// </summary>
        /// <param name="StudentName"></param>
        /// <param name="Sfz"></param>
        /// <param name="Batch"></param>
        /// <param name="Category"></param>
        /// <param name="SchoolName"></param>
        public static void InsertGkAdmissions(string Zkzh, string Sfz,GkAdmissionsResult gk)
        {
            int Id = GetStudentId(Zkzh, Sfz);

            string exits = "select count(1) from GkAdmissions where StudentId = @Id";
            SqlParameter[] sqls = {
                new SqlParameter("@Id",Id)
            };

            if ((int)SQLHelper.ExcuteScalar(exits, sqls) == 0)
            {
                    string sql = "insert into GkAdmissions values(@StudentId,@Batch,@Category,@SchoolName)";
                    SqlParameter[] parameters = {
                    new SqlParameter("@StudentId",Id),
                    new SqlParameter("@Batch",gk.Batch),
                    new SqlParameter("@Category",gk.Category),
                    new SqlParameter("@SchoolName",gk.SchoolName)
                    };
                    SQLHelper.ExcuteNonQuery(sql, parameters);
            }
        }

        /// <summary>
        /// 录入中考成绩
        /// </summary>
        /// <param name="Zkzh"></param>
        /// <param name="Sfz"></param>
        /// <param name="zk"></param>
        public static void InsertZkScore(string Zkzh, string Sfz,ZkScoreResult zk)
        {
            int Id = GetStudentId(Zkzh, Sfz);

            string exits = "select count(1) from ZkScore where StudentId = @Id";
            SqlParameter[] sqls = {
                new SqlParameter("@Id",Id)
            };

            if ((int)SQLHelper.ExcuteScalar(exits, sqls) == 0)
            {
                string sql = "insert into ZkScore values(@StudentId,@ChineseScore,@MathematicsScore,@EnglishScore,@HistoryScore,@GeographyScore,@BiologyScore,@ChemistryScore,@PhysicsScore,@PoliticsScore,@Sports,@SumScore)";
                    SqlParameter[] parameters = {
                    new SqlParameter("@StudentId",Id),
                    new SqlParameter("@ChineseScore",zk.ChineseScore),
                    new SqlParameter("@MathematicsScore",zk.MathematicsScore),
                    new SqlParameter("@EnglishScore",zk.EnglishScore),
                    new SqlParameter("@HistoryScore",zk.HistoryScore),
                    new SqlParameter("@GeographyScore",zk.GeographyScore),
                    new SqlParameter("@BiologyScore",zk.BiologyScore),
                    new SqlParameter("@ChemistryScore",zk.ChemistryScore),
                    new SqlParameter("@PhysicsScore",zk.PhysicsScore),
                    new SqlParameter("@PoliticsScore",zk.PoliticsScore),
                    new SqlParameter("@Sports",zk.Sports),
                    new SqlParameter("@SumScore",zk.SumScore)
                    };
                SQLHelper.ExcuteNonQuery(sql, parameters);
            }
        }

        /// <summary>
        /// 录入中考录取结果
        /// </summary>
        /// <param name="Zkzh"></param>
        /// <param name="Sfz"></param>
        /// <param name="zk"></param>
        public static void InsertZkAdmissions(string Zkzh, string Sfz,ZkAdmissionsResult zk)
        {
            int Id = GetStudentId(Zkzh, Sfz);

            string exits = "select count(1) from ZkAdmissions where StudentId = @Id";
            SqlParameter[] sqls = {
                new SqlParameter("@Id",Id)
            };

            if ((int)SQLHelper.ExcuteScalar(exits, sqls) == 0)
            {
                string sql = "insert into ZkAdmissions values(@StudentId,@SchoolName,@Batch,@Category)";
                SqlParameter[] parameters = {
                new SqlParameter("@StudentId",Id),
                new SqlParameter("@SchoolName",zk.SchoolName),
                new SqlParameter("@Batch",zk.Batch),
                new SqlParameter("@Category",zk.Category)
                 };
                SQLHelper.ExcuteNonQuery(sql, parameters);
            }
        }

        /// <summary>
        /// 临时表数据
        /// </summary>
        public static void InsertTemp(string json,string ProvinceKey)
        {
            string sql = "insert into Temporary values(@ProvinceKey,@data)";
            SqlParameter[] sqls = {
                new SqlParameter("@ProvinceKey",ProvinceKey),
                new SqlParameter("@data",json)
            };
            SQLHelper.ExcuteNonQuery(sql, sqls);
        }

    }
}