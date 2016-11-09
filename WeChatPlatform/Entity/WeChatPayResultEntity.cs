using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.Entity
{
    public class WeChatPayResultEntity
    {
        public string return_code;

        public string return_msg;

        public string result_code;

        public string err_code;

        public string err_code_des;

        public string mch_billno;

        public string mch_id;

        public string wxappid;

        public string re_openid;

        public string total_amount;

        public string send_listid;

        public string send_time;
    }

    public class WeChatPayQueryResultEntity
    {
        public string return_code;

        public string return_msg;

        public string result_code;

        public string mch_billno;

        public string mch_id;

        public string detail_id;

        public string status;

        public string reason;

        public string err_code;

        public string err_code_des;
    }
}
