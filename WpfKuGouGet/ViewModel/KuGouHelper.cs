using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.ClayObject;
using Furion.DataEncryption;
using Furion.RemoteRequest.Extensions;
using GalaSoft.MvvmLight;
using Microsoft.Extensions.DependencyInjection;

namespace WpfKuGouGet.ViewModel
{
    /// <summary>
    /// 歌曲信息
    /// </summary>
    public class SongInfo : ViewModelBase
    {
        private string _songName = "";

        public string SongName
        {
            get => _songName;
            set => Set("SongName", ref _songName, value);
        }

        private string _songAuthor = "";

        public string SongAuthor
        {
            get => _songAuthor;
            set => Set("SongAuthor", ref _songAuthor, value);
        }

        private string _songAlbumId = "";
        public string SongAlbumId
        {
            get => _songAlbumId;
            set => Set("SongAlbumId", ref _songAlbumId, value);
        }

        private string _songFileHash = "";
        public string SongFileHash
        {
            get => _songFileHash;
            set => Set("SongFileHash", ref _songFileHash, value);
        }

        private string _songUrl = "";

        public string SongUrl
        {
            get => _songUrl;
            set => Set("SongUrl", ref _songUrl, value);
        }
    }

    /// <summary>
    /// 酷狗工具类
    /// </summary>
    public partial class KuGouHelper : ViewModelBase
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

        private readonly Dictionary<string, object> _headers = new Dictionary<string, object> {
                { "User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36" }
        };

        //搜索个数
        public int SearchNum = 50;

        //缓存目录
        private string _pathCache = @"D:/kgMusic/";

        public string PathCache
        {
            get => _pathCache;
            set => Set("PathCache", ref _pathCache, value);
        }

        //搜索歌曲信息容器
        private ObservableCollection<SongInfo> _songItemsInfo = new ObservableCollection<SongInfo>();
        public ObservableCollection<SongInfo> SongItemsInfo
        {
            get => _songItemsInfo;
            set => Set("SongItemsInfo", ref _songItemsInfo, value);
        }

        /// <summary>
        /// 搜索功能
        /// </summary>
        /// <param name="searchKeyWord">搜索关键词</param>
        /// <param name="pageIndex">分页</param>
        /// <returns></returns>
        public async Task<bool> Search(string searchKeyWord, int pageIndex)
        {
            bool ok = false;
            var tempSongItemsInfo = new ObservableCollection<SongInfo>();
            try
            {
                string t = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString(CultureInfo.InvariantCulture);

                string[] sign_params = {"NVPh5oo715z5DIWAeQlhMDsWXXQV4hwt", "bitrate=0", "callback=callback123",
                       "clienttime=" + t, "clientver=2000", "dfid=-", "inputtype=0", "iscorrection=1",
                       "isfuzzy=0",
                       "keyword=" + searchKeyWord, "mid=" + t, "page=" + pageIndex, "pagesize=" + SearchNum,
                       "platform=WebFilter", "privilege_filter=0", "srcappid=2919", "token=", "userid=0",
                       "uuid=" + t, "NVPh5oo715z5DIWAeQlhMDsWXXQV4hwt" };
                var signParamStr = string.Join("", sign_params);
                var signature = MD5Encryption.Encrypt(signParamStr);

                var response = await "https://complexsearch.kugou.com/v2/search/song"
                    .SetHeaders(_headers)
                    .SetQueries(new Dictionary<string, object> {
                {"callback","callback123"},
                {"page",pageIndex},
                {"keyword",searchKeyWord??""},
                {"pagesize", SearchNum},
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

                if (string.IsNullOrEmpty(responseJson))
                    return ok;

                var clay = Clay.Parse(responseJson);
                int index = 0;
                foreach (var item in clay.data.lists)
                {
                    index++;
                    var tempSongInfo = new SongInfo
                    {
                        SongAuthor = item.SingerName,
                        SongName = item.FileName,
                        SongFileHash = item.FileHash,
                        SongAlbumId = item.AlbumID
                    };
                    tempSongItemsInfo.Add(tempSongInfo);
                }

                ok = index > 0;
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //throw;
            }

            SongItemsInfo = tempSongItemsInfo;
            return ok;
        }

        /// <summary>
        /// 下载接口
        /// </summary>
        /// <param name="fileHash"></param>
        /// <param name="albumId"></param>
        public async void Download(string fileHash, string albumId)
        {
            //获取文件下载路径
            var respondFileInfo = await "https://wwwapi.kugou.com/yy/index.php"
                .SetHeaders(_headers)
                .SetQueries(new Dictionary<string, object>
                {
                    {"r", "play/getdata" },
                    {"callback", "jQuery191035601158181920933_1653052693184" },
                    {"hash", fileHash },
                    {"dfid", "2mSZvv2GejpK2VDsgh0K7U0O" },
                    {"appid", "1014" },
                    {"mid", "c18aeb062e34929c6e90e3af8f7e2512" },
                    {"platid", "4" },
                    {"album_id", albumId },
                    {"_", "1653050047389" }
                }).GetAsStringAsync();

            var respondFileInfoJson = respondFileInfo.Substring(42).TrimEnd().TrimEnd(';').TrimEnd(')');
            var clay = Clay.Parse(respondFileInfoJson);
            string fileUrl = clay.data.play_url;

            //下载文件
            var bytes = await fileUrl.SetHeaders(_headers).GetAsByteArrayAsync();

            if (!Directory.Exists(PathCache))
            {
                Directory.CreateDirectory(PathCache);
            }

            await using FileStream fs = new FileStream($"{PathCache}{clay.data.audio_name}.mp3", FileMode.Create, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}
