using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPITest.Controllers
{
    public class Test1Controller : ApiController
    {


        [HttpPost] // POST 用メソッド
        public IEnumerable<Person> Get()
        {
            //return new string[] { "value1", "value2" };

            return new[]
            {
                new Person(){Id = 1, Name ="hoge", Time = DateTime.Now},
                new Person(){Id = 2, Name ="fuga", Time = DateTime.Now.AddDays(1)}
            };

        }

        [HttpGet] // GET 用メソッド
        public IEnumerable<Person> Get2()
        {
            //return new string[] { "value1", "value2" };

            System.Threading.Thread.Sleep(5000);

            return new[]
            {
                new Person(){Id = 1, Name ="piyo", Time = DateTime.Now}
            };

        }


    }
}
