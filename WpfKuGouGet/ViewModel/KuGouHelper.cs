using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.ClayObject;
using Furion.DataEncryption;
using Furion.RemoteRequest.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace WpfKuGouGet.ViewModel
{
    public class KuGouHelper
    {
        private static readonly Lazy<KuGouHelper>
           Lazy = new Lazy<KuGouHelper>(() => new KuGouHelper());

        public static KuGouHelper Instance => Lazy.Value;

        public KuGouHelper()
        {
            // 注册服务
            var services = Inject.Create();
            services.AddRemoteRequest();
            services.Build();
        }

        private Dictionary<string, object> _headers = new Dictionary<string, object> {
                { "User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36" }
        };

        /// <summary>
        /// 搜索功能
        /// </summary>
        /// <param name="searchKeyWord">搜索关键词</param>
        /// <param name="pageIndex">分页</param>
        /// <returns></returns>
        public async Task<List<dynamic>> Search(string searchKeyWord, int pageIndex)
        {
            List<dynamic> singerlist = new List<dynamic>();
            try
            {
                string t = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString();

                string[] sign_params = {"NVPh5oo715z5DIWAeQlhMDsWXXQV4hwt", "bitrate=0", "callback=callback123",
                       "clienttime=" + t, "clientver=2000", "dfid=-", "inputtype=0", "iscorrection=1",
                       "isfuzzy=0",
                       "keyword=" + searchKeyWord, "mid=" + t, "page=" + pageIndex, "pagesize=20",
                       "platform=WebFilter", "privilege_filter=0", "srcappid=2919", "token=", "userid=0",
                       "uuid=" + t, "NVPh5oo715z5DIWAeQlhMDsWXXQV4hwt" };
                var sign_paramStr = string.Join("", sign_params);
                var signature = MD5Encryption.Encrypt(sign_paramStr);

                var response = await "https://complexsearch.kugou.com/v2/search/song"
                    .SetHeaders(_headers)
                    .SetQueries(new Dictionary<string, object> {
                {"callback","callback123"},
                {"page",pageIndex},
                {"keyword",searchKeyWord??""},
                {"pagesize","20"},
                {"bitrate","0"},
                {"isfuzzy","0"},
                {"inputtype","0"},
                {"platform","WebFilter"},
                {"userid","0"},
                {"clientver","2000"},
                {"iscorrection","1"},
                {"privilege_filter","0"},
                {"token",""},
                {"srcappid","2919"},
                {"clienttime",t},
                {"mid",t},
                {"uuid",t},
                {"dfid","-"},
                {"signature",signature}
                }).GetAsStringAsync();

                //返回值为JsonpCallback
                var responseJson = response.Replace("callback123(", "").TrimEnd().TrimEnd(')');

                var clay = Clay.Parse(responseJson);
                int index = 0;
                foreach (var item in clay.data.lists)
                {
                    index++;
                    singerlist.Add(item);
                    Console.WriteLine($"{index.ToString().PadLeft(2, '0')}  {item.FileName}");
                }
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //throw;
            }
            return singerlist;
        }
    }
}
