using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Spire.Doc.Documents;
using Spire.Doc;
using System.IO;
using System;
using System.Text;

namespace Swagger2Docx.Helper
{
    public class SpireDocHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SpireDocHelper(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 靜態頁面轉文件
        /// </summary>
        /// <param name="html">靜態頁面html</param>
        /// <param name="type">檔案類型</param>
        /// <param name="contenttype">上下文類型</param>
        /// <returns></returns>
        public Stream SwaggerConversHtml(string html, string type, out string contenttype)
        {
            string fileName = Guid.NewGuid().ToString() + type;
            //檔存放路徑
            string webRootPath = _hostingEnvironment.WebRootPath;
            string path = webRootPath + @"\Files\TempFiles\";
            var addrUrl = path + $"{fileName}";
            FileStream fileStream = null;
            var provider = new FileExtensionContentTypeProvider();
            contenttype = provider.Mappings[type];
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var data = Encoding.Default.GetBytes(html);
                var stream = ByteHelper.BytesToStream(data);
                //創建Document實例
                Document document = new Document();
                //載入HTML文檔
                document.LoadFromStream(stream, FileFormat.Html, XHTMLValidationType.None);

                switch (type)
                {
                    case ".docx":
                        document.SaveToFile(addrUrl, FileFormat.Docx);
                        break;
                    case ".pdf":
                        document.SaveToFile(addrUrl, FileFormat.PDF);
                        break;
                    case ".html":
                        //document.SaveToFile(addrUrl, FileFormat.Html);
                        //當然了，html 如果不用spire，也可以直接生成
                        FileStream fs = new FileStream(addrUrl, FileMode.Append, FileAccess.Write, FileShare.None);//html直接寫入不用spire.doc
                        StreamWriter sw = new StreamWriter(fs); // 創建寫入流
                        sw.WriteLine(html); // 寫入Hello World
                        sw.Close(); //關閉文件
                        fs.Close();
                        break;
                    case ".xml":
                        document.SaveToFile(addrUrl, FileFormat.Xml);
                        break;
                    case ".svg":
                        document.SaveToFile(addrUrl, FileFormat.SVG);
                        break;
                    default:
                        //保存為Word
                        document.SaveToFile(addrUrl, FileFormat.Docx);
                        break;
                }
                document.Close();
                fileStream = File.Open(addrUrl, FileMode.OpenOrCreate);
                var filedata = ByteHelper.StreamToBytes(fileStream);
                var outdata = ByteHelper.BytesToStream(filedata);
                return outdata;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
                if (File.Exists(addrUrl))
                    File.Delete(addrUrl);//刪掉文件
            }
        }
    }
}
