using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeChatPlatform.Entity;
using WeChatPlatform.WeChatPay;

namespace WeChatPlatform
{
    public class PayHandler
    {
        /// <summary>
        /// 【微信商户平台】账号支付。
        /// </summary>
        /// <param name="drawlogid">提现id</param>
        /// <param name="openid"></param>
        /// <param name="money">提现金额</param>
        /// <param name="pubIp">公网ip</param>
        /// <returns>返回成功状态</returns>
        public int WeChatAccountPay(int drawlogid, string openid, float money, string pubIp)
        {
            // 商户号.
            var mch_id = Env.MchId;

            // 商户订单号（每个订单号必须唯一）组成：mch_id+yyyymmdd+10位一天内不能重复的数字。 .
            var mch_billno = string.Format("{0}{1}{2}{3}", mch_id, DateTime.Now.ToString("yyyyMMdd")
                , DateTime.Now.ToString("HHmmss"), new Random().Next(1000, 9999));

            // 此处省略：可以根据生成的商户订单号在自己的数据库插入（或更新）一条提现记录：insert or update

            ////////////////////////////////////////////////////////////////////////////////////////////////

            // 把请求参数打包成数组
            var sParaTemp = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                {"nonce_str", Guid.NewGuid().ToString("N").Substring(0, 30)},
                {"mch_id", mch_id},
                {"mch_billno", mch_billno},
                {"wxappid", Env.AppId},
                {"send_name", "XX提现"},
                {"re_openid", openid},
                {"total_amount", Convert.ToInt32(money*100).ToString(CultureInfo.InvariantCulture)},
                {"total_num", "1"},
                {"wishing", "感谢您的参与"},
                {"client_ip", pubIp},
                {"act_name", "XX提现申请"},
                {"remark", "点击消息拆开红包即可获得现金"}
            };

            // 建立请求.
            var content = PayService.BuildRequest(sParaTemp, Env.RedPackUrl);
            if (string.IsNullOrEmpty(content))
                return 0;

            var result = AnalyticXmlDataOfPay(content);

            if (result == null || string.IsNullOrEmpty(result.mch_billno) || string.IsNullOrEmpty(result.re_openid) ||
                string.IsNullOrEmpty(result.total_amount))
                return 0;

            // 此处省略：可以根据微信支付返回的结果，更新提现记录的状态：update
            return 0;
        }

        /// <summary>
        /// 【微信商户平台】账号支付查询。
        /// </summary>
        /// <param name="drawlogid"></param>
        /// <param name="mch_billno">商户订单号</param>
        /// <returns></returns>
        public int WeChatAccountPayQuery(int drawlogid, string mch_billno)
        {
            // 把请求参数打包成数组
            var sParaTemp = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                {"nonce_str", Guid.NewGuid().ToString("N").Substring(0, 30)},
                {"mch_id", Env.MchId},
                {"mch_billno", mch_billno},
                {"appid", Env.AppId},
                {"bill_type", "MCHT"}
            };

            // 建立请求.
            var content = PayService.BuildRequest(sParaTemp, Env.RedPackQueryUrl);
            if (string.IsNullOrEmpty(content))
                return 0;

            var result = AnalyticXmlDataOfPayQuery(content);
            if (string.IsNullOrEmpty(result.return_code) || string.IsNullOrEmpty(result.result_code) || string.IsNullOrEmpty(result.mch_billno))
                return 0;

            if (result.return_code != "SUCCESS" || result.result_code != "SUCCESS")
                return 0;

            // 此处省略：可以根据微信支付查询返回的结果，更新提现记录的状态：update
            return 0;
        }

        /// <summary>
        /// 解析【微信支付】返回的结果内容。
        /// </summary>
        /// <param name="xmlContent">微信支付返回结果内容</param>
        /// <returns></returns>
        private static WeChatPayResultEntity AnalyticXmlDataOfPay(string xmlContent)
        {
            xmlContent = xmlContent.Replace("<xml>", 
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><wechat>").Replace("</xml>", "</wechat>");

            var doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            var xn = doc.SelectSingleNode("wechat");
            if (xn == null)
                return null;

            var result = new WeChatPayResultEntity();

            var returnCode = xn.SelectSingleNode("return_code");
            result.return_code = returnCode == null
                ? "" 
                : returnCode.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var returnMsg = xn.SelectSingleNode("return_msg");
            result.return_msg = returnMsg == null
                ? "" 
                : returnMsg.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var resultCode = xn.SelectSingleNode("result_code");
            result.result_code = resultCode == null
                ? "" 
                : resultCode.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var errCode = xn.SelectSingleNode("err_code");
            result.err_code = errCode == null
                ? "" 
                : errCode.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var errCodeDes = xn.SelectSingleNode("err_code_des");
            result.err_code_des = errCodeDes == null
                ? "" 
                : errCodeDes.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var mchBillno = xn.SelectSingleNode("mch_billno");
            result.mch_billno = mchBillno == null
                ? "" 
                : mchBillno.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var mchId = xn.SelectSingleNode("mch_id");
            result.mch_id = mchId == null
                ? "" 
                : mchId.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var wxappid = xn.SelectSingleNode("wxappid");
            result.wxappid = wxappid == null
                ? "" 
                : wxappid.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var reOpenid = xn.SelectSingleNode("re_openid");
            result.re_openid = reOpenid == null
                ? "" : reOpenid.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var totalAmount = xn.SelectSingleNode("total_amount");
            result.total_amount = totalAmount == null
                ? "" 
                : totalAmount.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var sendListid = xn.SelectSingleNode("send_listid");
            result.send_listid = sendListid == null
                ? "" 
                : sendListid.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var sendTime = xn.SelectSingleNode("send_time");
            result.send_time = sendTime == null
                ? "0" 
                : sendTime.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            return result;
        }

        /// <summary>
        /// 解析【微信支付查询】返回的结果内容。
        /// </summary>
        /// <param name="xmlContent">微信支付返回结果内容</param>
        /// <returns></returns>
        private static WeChatPayQueryResultEntity AnalyticXmlDataOfPayQuery(string xmlContent)
        {
            xmlContent = xmlContent.Replace("<xml>",
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><wechat>").Replace("</xml>", "</wechat>");

            var doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            var xn = doc.SelectSingleNode("wechat");
            if (xn == null)
                return null;

            var result = new WeChatPayQueryResultEntity();

            var returnCode = xn.SelectSingleNode("return_code");
            result.return_code = returnCode == null
                ? ""
                : returnCode.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var returnMsg = xn.SelectSingleNode("return_msg");
            result.return_msg = returnMsg == null
                ? ""
                : returnMsg.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var resultCode = xn.SelectSingleNode("result_code");
            result.result_code = resultCode == null
                ? ""
                : resultCode.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var mchBillno = xn.SelectSingleNode("mch_billno");
            result.mch_billno = mchBillno == null
                ? "" : mchBillno.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var mchId = xn.SelectSingleNode("mch_id");
            result.mch_id = mchId == null
                ? "" : mchId.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var detailId = xn.SelectSingleNode("detail_id");
            result.detail_id = detailId == null
                ? "" : detailId.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var status = xn.SelectSingleNode("status");
            result.status = status == null
                ? "" : status.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var reason = xn.SelectSingleNode("reason");
            result.reason = reason == null
                ? "" : reason.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var errCode = xn.SelectSingleNode("err_code");
            result.err_code = errCode == null
                ? "" : errCode.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            var errCodeDes = xn.SelectSingleNode("err_code_des");
            result.err_code_des = errCodeDes == null
                ? "" : errCodeDes.InnerText.ToUpper().Replace("<![CDATA[", "").Replace("]]>", "");

            return result;
        }
    }
}
