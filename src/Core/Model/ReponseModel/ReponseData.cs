using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.ReponseModel
{
    public class ReponseData
    {
        public int status { get; set; }
        public dynamic result { get; set; }
        public string Message { get; set; }
    }
}
