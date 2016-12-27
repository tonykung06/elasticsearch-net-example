using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elasticsearch_2._3._5_net_example
{
    public class Post
    {
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public string PostText { get; set; }
    }
}
