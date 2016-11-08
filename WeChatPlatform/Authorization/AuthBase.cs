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
    }
}
