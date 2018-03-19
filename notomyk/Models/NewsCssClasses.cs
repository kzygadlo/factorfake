using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Models
{
    public class NewsCssClasses
    {

        public NewsCssClasses()
        {

        }

        public NewsCssClasses(string basicClass)
        {
            FaktClass = basicClass;
            FaktVotedClass = "";

            ManipulatedClass = basicClass;
            ManipulatedVotedClass = "";

            FakeClass = basicClass;
            FakeVotedClass = "";
        }

        public string FaktClass { get; set; }
        public string ManipulatedClass { get; set; }
        public string FakeClass { get; set; }

        public string FaktVotedClass { get; set; }
        public string ManipulatedVotedClass { get; set; }
        public string FakeVotedClass { get; set; }
    }
}