using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.Entity
{
    public class BaseWeChatUserEntity
    {
        /// <summary>
        /// 是否订阅此公众号，0-未关注，将取不到其余信息。
        /// </summary>
        public int subscribe;

        public string openid;

        public string nickname;

        public string headimgurl;

        public int sex;

        public string language;

        public string city;

        public string province;

        public string country;

        /// <summary>
        /// 关注时间。
        /// </summary>
        public string subscribe_time;

        /// <summary>
        /// 绑定开发平台UnionID.
        /// </summary>
        public string unionid;

        /// <summary>
        /// 对粉丝的备注。
        /// </summary>
        public string remark;

        /// <summary>
        /// 用户所在分组。
        /// </summary>
        public int groupid;
    }
}
