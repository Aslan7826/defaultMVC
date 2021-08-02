using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoUse.CustomAction
{
    public class TxtEditer
    {
        private string PATH;
        public TxtEditer(string path)
        {
            PATH = path;
        }

        /// <summary>
        /// 取得文字檔資料
        /// </summary>
        /// <returns></returns>
        public string GetTxtData()
        {
            var result = string.Empty;
            using (StreamReader reader = new StreamReader(PATH, System.Text.Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        /// <summary>
        /// 完全覆蓋文字檔
        /// </summary>
        /// <returns></returns>
        public void EditTxtData(string txt)
        {
            using (StreamWriter writer = new StreamWriter(PATH, false, System.Text.Encoding.Default))
            {
                writer.Write(txt);
            }
        }
    }
}
