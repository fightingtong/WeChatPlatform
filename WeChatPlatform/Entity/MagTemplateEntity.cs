using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatPlatform.Entity
{
    /// <summary>
    /// 消息模板。
    /// </summary>
    public class MagTemplateEntity<T>
    {
        public string touser;

        public string template_id;

        public string url;

        public string topcolor;

        public T data;
    }

    public class DrawNoticeData
    {
        public MagTemplateValue first;

        public MagTemplateValue keyword1;

        public MagTemplateValue keyword2;

        public MagTemplateValue keyword3;

        public MagTemplateValue remark;
    }

    public class DrawResultData
    {
        public MagTemplateValue first;

        public MagTemplateValue keyword1;

        public MagTemplateValue keyword2;

        public MagTemplateValue keyword3;

        public MagTemplateValue keyword4;

        public MagTemplateValue remark;
    }

    public class DrawFailedData
    {
        public MagTemplateValue first;

        public MagTemplateValue money;

        public MagTemplateValue time;

        public MagTemplateValue remark;
    }

    public class MagTemplateValue
    {
        public string value;

        public string color;
    }
}
