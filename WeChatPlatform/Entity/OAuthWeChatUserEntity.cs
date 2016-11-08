using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.Entity
{
    /// <summary>
    /// OAuth鉴权：微信用户信息定义。
    /// </summary>
    public class OAuthWeChatUserEntity
    {
        public string openid;

        public string nickname;

        public string headimgurl;

        public int sex;

        public string language;

        public string city;

        public string province;

        public string country;

        public IList<string> privilege;
    }
}
