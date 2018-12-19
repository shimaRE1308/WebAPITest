using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using log4net;
using System.Web.Hosting;
using System.Web.Configuration;

namespace WebAPITest.Controllers
{
    [RoutePrefix("pf")]
    public class PerformanceController : ApiController
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region お試しメソッド


        [HttpGet] // POST 用メソッド
        [Route("post")]
        public Person Post(IEnumerable<Person> persons)
        {
            logger.Debug(System.Threading.Thread.CurrentThread.GetHashCode());

            System.Threading.Thread.Sleep(1000);

            return new Person() { Id = 999, Name = "return", Time = DateTime.Now.AddDays(10) };
        }

        [HttpGet]
        [Route("tes")]
        public string Test()
        {
            logger.Debug(System.Threading.Thread.CurrentThread.GetHashCode());

            return "end";
        }


        [HttpGet]
        [Route("sleep")]
        public string Count()
        {
            System.Threading.Thread.Sleep(1000);

            return "Test";
        }

        #endregion


        #region DBアクセスするやつ


        [HttpGet]
        [Route("ssa")] // 車両遡及ありの略
        public IEnumerable<DBData> Get()
        {

            List<DBData> list = null;

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            list = GetData("201907");
            list = GetData("201807");
            list = GetData("201707");
            list = GetData("201607");
            list = GetData("201507");
            list = GetData("201407");
            list = GetData("201307");
            list = GetData("201207");
            list = GetData("201107");
            list = GetData("201007");
            list = GetData("200907");
            list = GetData("200807");
            list = GetData("200707");
            list = GetData("200607");
            list = GetData("200507");
            list = GetData("200407");
            list = GetData("200307");
            list = GetData("200207");


            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            logger.Debug($"{ts.Seconds}.{ts.Milliseconds}");

            return list;

        }

        [HttpGet]
        [Route("ssn")] // 車両遡及なしの略
        public IEnumerable<DBData> Get2()
        {

            List<DBData> list = null;

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            list = GetData("200207");

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            logger.Debug($"{ts.Seconds}.{ts.Milliseconds}");

            return list;

        }

        [HttpGet]
        [Route("new")]  // 新価
        public IEnumerable<DBData> GetShin()
        {

            List<DBData> list = null;

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            list = GetDataShin("201901");

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            logger.Debug($"{ts.Seconds}.{ts.Milliseconds}");

            return list;

        }

        [HttpGet]
        [Route("count2")]
        public IEnumerable<DBData> GetOne()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            var list = Execute2();

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            logger.Debug($"{ts.Seconds}.{ts.Milliseconds}");
            return list;
        }

        #endregion 


        private List<DBData> GetData(string date)
        {
            List<DBData> list = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = @"select * from CAR_PRICE_LIST2 where PUBLISH_DATE=@DATE and CLASS_NUMBER='4' and MODEL='VAY1222'";
                    command.Parameters.Add(new SqlParameter("@DATE", date));

                    SqlDataReader reader = command.ExecuteReader();

                    list = CreateListData(ref reader, out list);

                }
                catch
                {

                }
            }
            return list;
        }

        private List<DBData> GetDataShin(string date)
        {
            List<DBData> list = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = @"select * from CAR_PRICE_LIST2 
where PUBLISH_DATE=@DATE and CLASS_NUMBER=@CLASS and  MODEL=@MODEL and FIRST_REGIST_START='0000' and FIRST_REGIST_END='0000'";
                    command.Parameters.Add(new SqlParameter("@DATE", date));
                    command.Parameters.Add(new SqlParameter("@CLASS", 3));
                    command.Parameters.Add(new SqlParameter("@MODEL", "ZN6"));


                    SqlDataReader reader = command.ExecuteReader();

                    list = CreateListData(ref reader, out list);
                }
                catch
                {

                }
            }
            return list;
        }


        private List<DBData> Execute2()
        {
            List<DBData> list = null;

            using (var connection = new SqlConnection(GetConnectionString()))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();

                    command.CommandText = @"select * from CAR_PRICE_LIST2 
where PUBLISH_DATE = (
    select max(a.PUBLISH_DATE) from (
        select PUBLISH_DATE from CAR_PRICE_LIST2 where PUBLISH_DATE like '%07' and CLASS_NUMBER=@CLASS and  MODEL=@MODEL group by PUBLISH_DATE) a ) 
and CLASS_NUMBER=@CLASS and MODEL=@MODEL";

                    command.Parameters.Add(new SqlParameter("@CLASS", "4"));
                    command.Parameters.Add(new SqlParameter("@MODEL", "VAY1222"));

                    SqlDataReader reader = command.ExecuteReader();

                    list = CreateListData(ref reader, out list);

                }
                catch
                {

                }
            }

            return list;
        }

        private List<DBData> CreateListData(ref SqlDataReader reader, out List<DBData> list)
        {
            list = new List<DBData>();

            while (reader.Read())
            {
                list.Add(new DBData()
                {
                    PublishDate = reader["PUBLISH_DATE"].ToString(),
                    CountryCoce = reader["COUNTRY_CODE"].ToString(),
                    MakerCode = reader["MAKER_CODE"].ToString(),
                    CarNameCode = reader["CAR_NAME_CODE"].ToString(),
                    CarName = reader["CAR_NAME"].ToString(),
                    ClassNumber = reader["CLASS_NUMBER"].ToString(),
                    Model = reader["MODEL"].ToString(),
                    ShapeCode = reader["SHAPE_CODE"].ToString(),
                    ShapeName = reader["SHAPE_NAME"].ToString(),
                    Specification = reader["SPECIFICATION"].ToString(),
                    PublicationDate = reader["PUBLICATION_DATE"].ToString(),
                    DuplicateNumber = reader["DUPLICATE_NUMBER"].ToString(),
                    BodilyInjuryLiability = reader["BODILY_INJURY_LIABILITY"].ToString(),
                    PropertyDamageLiability = reader["PROPERTY_DAMAGE_LIABILITY"].ToString(),
                    Accident = reader["ACCIDENT"].ToString(),
                    Vehicle = reader["VEHICLE"].ToString(),
                    FirstRegistStart = reader["FIRST_REGIST_START"].ToString(),
                    FirstRegistEnd = reader["FIRST_REGIST_END"].ToString(),
                    PriceLowerLimit = reader["PRICE_LOWER_LIMIT"].ToString(),
                    PriceHigherLimit = reader["PRICE_HIGHER_LIMIT"].ToString(),
                    Price = reader["PRICE"].ToString()

                });
            }

            return list;
        }


        private string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = GetConfigValue("Address"),
                IntegratedSecurity = false,
                UserID = GetConfigValue("UserId"),
                Password = GetConfigValue("Password"),
                MaxPoolSize = Int32.Parse(GetConfigValue("MaxPool")),
                MinPoolSize = Int32.Parse(GetConfigValue("MinPool"))

            };

            return builder.ToString();
        }

        private string GetConfigValue(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }

    }

    public class DBData
    {
        public string PublishDate { get; set; }
        public string CountryCoce { get; set; }
        public string MakerCode { get; set; }
        public string CarNameCode { get; set; }
        public string CarName { get; set; }
        public string ClassNumber { get; set; }
        public string Model { get; set; }
        public string ShapeCode { get; set; }
        public string ShapeName { get; set; }
        public string Specification { get; set; }
        public string PublicationDate { get; set; }
        public string DuplicateNumber { get; set; }
        public string BodilyInjuryLiability { get; set; }
        public string PropertyDamageLiability { get; set; }
        public string Accident { get; set; }
        public string Vehicle { get; set; }
        public string FirstRegistStart { get; set; }
        public string FirstRegistEnd { get; set; }
        public string PriceLowerLimit { get; set; }
        public string PriceHigherLimit { get; set; }
        public string Price { get; set; }
    }
}
