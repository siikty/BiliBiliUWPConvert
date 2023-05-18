using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BiliBiliUWPConvert
{
    internal class Program
    {
        static string bilibiliDownloadPath = "C:\\Users\\siikt\\AppData\\Local\\Packages\\36699Atelier39.forWin10_pke1vz55rvc1r\\LocalCache\\BilibiliDownload";
        static string newPathParent = "D:\\考研资料\\408视频";
        static List<string> list = new List<string>()
        {
            "70156862",
            "70211798",
            "70228743",
            "92191094"
        };

        static void Main(string[] args)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ConvertMp4(list[i]);
                Console.WriteLine("结束：" + list[i]);
            }
        }

        private static void ConvertMp4(string videopathname)
        {
            var videopathurl = Path.Combine(bilibiliDownloadPath, videopathname, "desktop.ini");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var desktopIni = new INIParser.IniFile(videopathurl, Encoding.GetEncoding("gb2312"));
            var foldname = desktopIni[".ShellClassInfo", "InfoTip"].ToString();
            var newPath = Path.Combine(newPathParent, foldname);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            var orgPath = new DirectoryInfo(Path.Combine(bilibiliDownloadPath, videopathname));
            foreach (var item in orgPath.GetDirectories())
            {
                var itemInfo = item.GetFiles("*.info");
                var itemInfoTxt = File.ReadAllText(itemInfo.First().FullName);
                var jsonObject = JsonObject.Parse(itemInfoTxt);
                var PartName = jsonObject["PartName"].ToString();
                var mp4 = item.GetFiles("*.mp4");
                var newItemPath = Path.Combine(newPath);
                if (!Directory.Exists(newItemPath))
                {
                    Directory.CreateDirectory(newItemPath);
                }
                byte[] bytes = File.ReadAllBytes(mp4.First().FullName);
                File.WriteAllBytes(Path.Combine(newItemPath, PartName + ".mp4"), bytes.Skip(3).ToArray());
                Console.WriteLine(PartName);
            }
        }
    }
}