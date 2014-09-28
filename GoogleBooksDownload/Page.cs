using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooksDownload
{
    public class Page
    {
        public string pid { get; set; }
        public string src { get; set; }
        public int flags { get; set; }
        public int order { get; set; }
        public string uf { get; set; }
    }

    public class RootObject
    {
        public List<Page> page { get; set; }
    }
}
