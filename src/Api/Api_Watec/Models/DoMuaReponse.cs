using System.Collections.Generic;

namespace ApiWatec.Models
{   
    public class DoMuaReponse
    {
        public Data data { get; set; }
        public string rid { get; set; }
    }

    public class Data
    {
        public List<float> id1 { get; set; }        
    }

}
