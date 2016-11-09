using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform
{
    public class Env
    {
        /// <summary>
        /// 公众账号appid.
        /// </summary>
        public static readonly string AppId = "wxxxxxxxxx";

        /// <summary>
        /// 公众账号秘钥。
        /// </summary>
        public static readonly string Secret = "xxxxxx";

        /// <summary>
        /// 商户号mch_id.
        /// </summary>
        public static readonly string MchId = "1234567890";

        /// <summary>
        /// 支付秘钥。
        /// </summary>
        public static readonly string PayKey = "xxxyyy";

        /// <summary>
        /// 网关地址:微信支付
        /// </summary>
        public static string RedPackUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

        /// <summary>
        /// 网关地址：微信支付查询
        /// </summary>
        public static string RedPackQueryUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gethbinfo";
    }
}
