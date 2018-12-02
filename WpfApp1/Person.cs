using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WpfApp1
{
    [DataContract]
    class Person
    {
        [DataMember(Name ="id")]
        public int Key { get; set; }

        [DataMember(Name ="name")]
        public string Namae { get; set; }

        [DataMember(Name="time")]
        public DateTime Zikan { get; set; }
    }
}
