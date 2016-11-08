using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeChatPlatform.Entity;

namespace WeChatPlatform.Authorization
{
    public class AuthBase
    {
        /// <summary>
        /// 基础支持：获取access_token。
        /// </summary>
        /// <returns></returns>
        public static BaseAccessTokenEntity GetBaseAccessToken()
        {
            var url
                = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}"
                , Env.AppId, Env.Secret);

            var jsonContent = Encoding.UTF8.GetString(new WebClient().DownloadData(url));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var token = JsonHelper.DeserializeJsonToObject<BaseAccessTokenEntity>(jsonContent);

            return token;
        }

        /// <summary>
        /// 基础支持：获取用户信息。
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static BaseWeChatUserEntity GetBaseWeChatUserInfo(string access_token, string openid)
        {
            var url
                = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN"
                , access_token, openid);

            var jsonContent = Encoding.UTF8.GetString(new WebClient().DownloadData(url));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var user = JsonHelper.DeserializeJsonToObject<BaseWeChatUserEntity>(jsonContent);

            return user;
        }

        /// <summary>
        /// 获取（临时）永久二维码的Ticket。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static QRcodeTicketEntity GetQRcodeTicket(string accessToken, int uid, int type)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", accessToken);
            var postJson = type == 1
                ? string.Format("{{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {{\"scene\": {{\"scene_str\": \"{0}\"}}}}}}", uid)
                : string.Format("{{\"expire_seconds\": 604800,\"action_name\": \"QR_SCENE\", \"action_info\": {{\"scene\": {{\"scene_id\": {0}}}}}}}"
                , uid);
            var sendData = Encoding.UTF8.GetBytes(postJson);

            var jsonContent = Encoding.UTF8.GetString(new WebClient().UploadData(url, "POST", sendData));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var ticket = JsonHelper.DeserializeJsonToObject<QRcodeTicketEntity>(jsonContent);

            return ticket;
        }

        /// <summary>
        /// OAuth2.0鉴权：通过code换取网页授权access_token。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OAuthAccessTokenEntity GetOAuthAccessToken(string code)
        {
            var url
                = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code"
                , Env.AppId, Env.Secret, code);

            var jsonContent = Encoding.UTF8.GetString(new WebClient().DownloadData(url));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var token = JsonHelper.DeserializeJsonToObject<OAuthAccessTokenEntity>(jsonContent);

            return token;
        }

        /// <summary>
        /// OAuth2.0鉴权：根据access_token和openid拉取用户信息(需scope为 snsapi_userinfo)。
        /// </summary>
        /// <param name="access_token">此处的access_token为OAuth授权得到的token，切勿与基础支持内部的access_token混淆</param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static OAuthWeChatUserEntity GetOAuthWeChatUserInfo(string access_token, string openid)
        {
            var url
                = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN"
                , access_token, openid);

            var jsonContent = Encoding.UTF8.GetString(new WebClient().DownloadData(url));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var token = JsonHelper.DeserializeJsonToObject<OAuthWeChatUserEntity>(jsonContent);

            return token;
        }

        /// <summary>
        /// 发送客服消息。【仅对48小时活跃用户有效】
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="content">发送内容</param>
        public static WeChatReturnCodeEntity SendCustomerMsg(string openid, string content)
        {
            var token = GetBaseAccessToken();
            if (token == null || string.IsNullOrEmpty(token.access_token))
                return null;

            string url
                = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", token.access_token);
            string postJson
                = string.Format("{{\"touser\": \"{0}\",\"msgtype\": \"text\", \"text\": {{\"content\": \"{1}\"}}}}"
                , openid, content);
            byte[] sendData = Encoding.UTF8.GetBytes(postJson);

            string jsonContent = Encoding.UTF8.GetString(new WebClient().UploadData(url, "POST", sendData));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var returnCode = JsonHelper.DeserializeJsonToObject<WeChatReturnCodeEntity>(jsonContent);

            return returnCode;
        }

        /// <summary>
        /// 发送消息-模板消息接口
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="templateid">消息模板对应的唯一编号串</param>
        /// <param name="skipUrl">模板点击跳转的链接</param>
        /// <param name="jsonData">模板显示需要的json数据</param>
        /// <returns></returns>
        public static WeChatReturnCodeEntity SendTemplateMsg(string openid, string templateid, string skipUrl, object jsonData)
        {
            var token = GetBaseAccessToken();
            if (token == null || string.IsNullOrEmpty(token.access_token))
                return null;

            string url
                = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", token.access_token);
            var msg = new MagTemplateEntity<object>
            {
                touser = openid,
                template_id = templateid,
                url = skipUrl,
                topcolor = "#FF0000",
                data = jsonData
            };

            var postJson = JsonHelper.SerializeObject(msg);
            byte[] sendData = Encoding.UTF8.GetBytes(postJson);
            string jsonContent = Encoding.UTF8.GetString(new WebClient().UploadData(url, "POST", sendData));
            if (string.IsNullOrEmpty(jsonContent))
                return null;

            var returnCode = JsonHelper.DeserializeJsonToObject<WeChatReturnCodeEntity>(jsonContent);

            return returnCode;
        }
    }
}
