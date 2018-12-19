using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Data.SqlClient;
using log4net;

namespace WebAPITest.Controllers
{
    public class Test1Controller : ApiController
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet] // POST 用メソッド
        public Person Post(IEnumerable<Person> persons)
        {
            //return new string[] { "value1", "value2" };

            foreach (var person in persons)
            {
                Debug.WriteLine("id is " + person.Id);
                Debug.WriteLine("name is " + person.Name);
                Debug.WriteLine("time is " + person.Time);
            }

            return new Person() { Id = 999, Name = "return", Time = DateTime.Now.AddDays(10) };
        }

        [HttpGet]
        public IEnumerable<TestData> Get()
        {
            

            logger.Debug("Get method execute");

            var values = Get2("201807");

            logger.Debug("execute end");

            return values;
        }

        [HttpGet] // GET 用メソッド
        public IEnumerable<TestData> Get2(string id)
        {
            //return new string[] { "value1", "value2" };

            //System.Threading.Thread.Sleep(2000);

            var connection = new SqlConnection(GetConnectionString());

            connection.Open();

            var command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"select PUBLISH_DATE, CAR_NAME, MODEL from CAR_PRICE_LIST2 where PUBLISH_DATE='" + id + "' group by PUBLISH_DATE, CAR_NAME, MODEL";

            SqlDataReader reader = command.ExecuteReader();

            List<TestData> list = new List<TestData>();

            while (reader.Read())
            {
                list.Add(new TestData()
                {
                    PublishData = reader["PUBLISH_DATE"].ToString(),
                    CarName = reader["CAR_NAME"].ToString(),
                    Model = reader["MODEL"].ToString()

                });
            }


            return list;

            //return new[]
            //{
            //    new Person(){Id = 1, Name =ss, Time = DateTime.Now}
            //};

        }

        private string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = "172.27.50.254",
                IntegratedSecurity = false,
                UserID = "developer",
                Password = "Passw0rd"
            };

            return builder.ToString();
        }
    }

    public class TestData
    {
        public string PublishData { get; set; }
        public string CarName { get; set; }
        public string Model { get; set; }
    }
}
