using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.Entity
{
    /// <summary>
    /// 微信全局返回码。
    /// </summary>
    public class WeChatReturnCodeEntity
    {
        /// <summary>
        /// 返回码。
        /// </summary>
        public int errcode;

        /// <summary>
        /// 说明。
        /// </summary>
        public string errmsg;

        public int msgid;
    }
}
