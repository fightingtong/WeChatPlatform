using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.Entity
{
    /// <summary>
    /// OAuth2.0鉴权：通过code换取网页授权access_token，参数说明。
    /// </summary>
    public class OAuthAccessTokenEntity
    {
        /// <summary>
        /// 网页授权接口调用凭证（【注意】此access_token与基础支持的access_token不同）。
        /// </summary>
        public string access_token;

        public int expires_in;

        public string refresh_token;

        /// <summary>
        /// 用户唯一标识（【注意】在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID）。
        /// </summary>
        public string openid;

        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔。
        /// </summary>
        public string scope;

    }

    /// <summary>
    /// 获取基础支持的access_token。
    /// </summary>
    public class BaseAccessTokenEntity
    {
        /// <summary>
        /// 基础支持的access_token不同。
        /// </summary>
        public string access_token;

        public int expires_in;
    }

    /// <summary>
    /// 创建二维码ticket。
    /// </summary>
    public class QRcodeTicketEntity
    {
        public string ticket;

        public int expire_seconds;

        public string url;
    }
}
