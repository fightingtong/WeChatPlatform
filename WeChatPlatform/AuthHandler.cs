using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeChatPlatform.Authorization;

namespace WeChatPlatform
{
    public class AuthHandler
    {
        /// <summary>
        /// 生成（临时）永久二维码。
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type">0-临时，1-永久</param>
        /// <returns></returns>
        public string CreateQRcode(int uid, int type)
        {
            var token = AuthBase.GetBaseAccessToken();
            if (token == null || string.IsNullOrEmpty(token.access_token))
                return null;

            var ticket = AuthBase.GetQRcodeTicket(token.access_token, uid, type);
            if (ticket == null || string.IsNullOrEmpty(ticket.ticket))
                return null;

            return string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", HttpUtility.UrlEncode(ticket.ticket));
        }
    }
}
