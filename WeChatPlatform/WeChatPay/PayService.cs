using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.WeChatPay
{
    public class PayService
    {
        private const string PayKey = "xxxyyy";

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="redUrl"></param>
        /// <returns>支付宝处理结果</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string redUrl)
        {
            var dict = FilterPara(sParaTemp);

            var postXml = DictionaryToXml(dict);
            var sendData = Encoding.UTF8.GetBytes(postXml);

            const string cert = "D:\\xxx\\apiclient_cert.p12";
            var password = Env.MchId;// 证书密码 即商户号
            var cer = new X509Certificate(cert, password);

            #region 该部分是关键，若没有该部分则在IIS下会报 CA证书出错

            var certificate = new X509Certificate2(cert, password);
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Remove(certificate);   // 可省略。
            store.Add(certificate);
            store.Close();

            #endregion

            // 请求远程HTTP.
            // 设置HttpWebRequest基本信息.
            var myReq = (HttpWebRequest)WebRequest.Create(redUrl);
            myReq.Method = "post";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ClientCertificates.Add(cer);

            // 填充POST数据.
            myReq.ContentLength = sendData.Length;
            var requestStream = myReq.GetRequestStream();
            requestStream.Write(sendData, 0, sendData.Length);
            requestStream.Close();

            // 发送POST数据请求服务器.
            var httpWResp = (HttpWebResponse)myReq.GetResponse();
            var myStream = httpWResp.GetResponseStream();

            // 获取服务器返回信息.
            if (myStream == null)
                return null;

            var reader = new StreamReader(myStream, Encoding.GetEncoding("utf-8"));
            var responseData = new StringBuilder();
            String line;
            while ((line = reader.ReadLine()) != null)
            {
                responseData.Append(line);
            }

            // 释放.
            myStream.Close();
            var xmlContent = responseData.ToString();

            return xmlContent;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        private static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            var dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 键值字典转Xml。
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string DictionaryToXml(IDictionary<string, string> dict)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<xml>");
            foreach (var item in dict)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                    continue;

                sb.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value);
            }
            var sign = Sign(dict);
            sb.AppendFormat("<sign>{0}</sign>", sign);
            sb.AppendLine("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 签名。
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static string Sign(IDictionary<string, string> dict)
        {
            var sb = new StringBuilder();
            foreach (var item in dict)
            {
                if (string.IsNullOrEmpty(item.Key) || string.IsNullOrEmpty(item.Value))
                    continue;

                sb.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            sb.Append("key=" + PayKey);
            var bytesToHash = Encoding.UTF8.GetBytes(sb.ToString()); // 注意，必须是UTF-8。
            var hashResult = ComputeMd5Hash(bytesToHash);
            var hash = BytesToString(hashResult, false);
            return hash;
        }

        private static byte[] ComputeMd5Hash(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var result = md5.ComputeHash(input);
                return result;
            }
        }

        private static string BytesToString(byte[] input, bool lowercase = true)
        {
            if (input == null || input.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(input.Length * 2);
            for (var i = 0; i < input.Length; i++)
            {
                sb.AppendFormat(lowercase ? "{0:x2}" : "{0:X2}", input[i]);
            }
            return sb.ToString();
        }
    }
}
