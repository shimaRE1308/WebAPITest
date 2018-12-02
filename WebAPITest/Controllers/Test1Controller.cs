using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WebAPITest.Controllers
{
    public class Test1Controller : ApiController
    {


        [HttpPost] // POST 用メソッド
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

        [HttpGet] // GET 用メソッド
        public IEnumerable<Person> Get2()
        {
            //return new string[] { "value1", "value2" };

            System.Threading.Thread.Sleep(2000);

            return new[]
            {
                new Person(){Id = 1, Name ="piyo", Time = DateTime.Now}
            };

        }
    }
}
