using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatPlatform.Authorization;
using WeChatPlatform.Entity;

namespace WeChatPlatform
{
    /// <summary>
    /// 四种基于任务奖励与用户提现到账情况的模板案例
    /// </summary>
    public class PushMsgHandler
    {
        /// <summary>
        /// 任务完成通知。
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="openid"></param>
        /// <param name="taskName">任务名称</param>
        /// <param name="money">任务奖励</param>
        /// <returns></returns>
        public static void SendTaskDoneMsg(string title, string openid, string taskName, float money)
        {
            var skipUrl = string.Format("http://www.baidu.com");

            var data = new DrawNoticeData
            {
                first = new MagTemplateValue
                {
                    value = string.Format("{0}", title),
                    color = "#173177"
                },
                keyword1 = new MagTemplateValue
                {
                    value = string.Format("{0}", taskName),
                    color = "#173177"
                },
                keyword2 = new MagTemplateValue
                {
                    value = string.Format("{0}元", money.ToString("F2")),
                    color = "#173177"
                },
                keyword3 = new MagTemplateValue
                {
                    value = string.Format("{0}", DateTime.Now),
                    color = "#173177"
                },
                remark = new MagTemplateValue
                {
                    value = string.Format("点击查看我的收入明细"),
                    color = "#173177"
                }
            };
            const string templateid = "aaaaa";

            AuthBase.SendTemplateMsg(openid, templateid, skipUrl, data);
        }

        /// <summary>
        /// 提现申请通知。
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static void SendDrawApplyMsg(string openid, decimal money)
        {
            const decimal fee = 0;
            var cashMoney = money - fee;
            var skipUrl = string.Format("http://www.baidu.com");

            var data = new DrawNoticeData
            {
                first = new MagTemplateValue
                {
                    value = string.Format("您于{0}成功申请提现。", DateTime.Now),
                    color = "#173177"
                },
                keyword1 = new MagTemplateValue
                {
                    value = string.Format("{0}元", money.ToString("F2")),
                    color = "#173177"
                },
                keyword2 = new MagTemplateValue
                {
                    value = string.Format("{0}元", fee.ToString("F2")),
                    color = "#173177"
                },
                keyword3 = new MagTemplateValue
                {
                    value = string.Format("{0}元", cashMoney.ToString("F2")),
                    color = "#173177"
                },
                remark = new MagTemplateValue
                {
                    value = string.Format("预计在1-2个工作日内给您的提现账号存入{0}元，请您注意查收。", cashMoney),
                    color = "#173177"
                }
            };
            const string templateid = "bbbbb";

            AuthBase.SendTemplateMsg(openid, templateid, skipUrl, data);
        }

        /// <summary>
        /// 提现成功通知。
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="drawWay"></param>
        /// <returns></returns>
        public static void SendDrawSucceedMsg(string openid, int drawWay, decimal money)
        {
            var skipUrl = string.Format("http://www.baidu.com");

            var data = new DrawResultData
            {
                first = new MagTemplateValue
                {
                    value = string.Format("提现金额已到账，请及时查收。"),
                    color = "#173177"
                },
                keyword1 = new MagTemplateValue
                {
                    value = string.Format("{0}", "此处为提现账号实名"),
                    color = "#173177"
                },
                keyword2 = new MagTemplateValue
                {
                    value = string.Format("{0}元", money.ToString("F2")),
                    color = "#173177"
                },
                keyword3 = new MagTemplateValue
                {
                    value = string.Format("{0}", drawWay == 1
                    ? string.Format("xx宝：{0}", "此处为提现账号") : "yy红包"),
                    color = "#173177"
                },
                keyword4 = new MagTemplateValue
                {
                    value = string.Format("{0}", DateTime.Now),
                    color = "#173177"
                },
                remark = new MagTemplateValue
                {
                    value = string.Format("哈哈哈，xxxx！"),
                    color = "#173177"
                }
            };
            const string templateid = "ccccc";

            AuthBase.SendTemplateMsg(openid, templateid, skipUrl, data);
        }

        /// <summary>
        /// 提现失败通知。
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static void SendDrawFailedMsg(string openid, decimal money)
        {
            var skipUrl = string.Format("http://www.baidu.com");

            var data = new DrawFailedData
            {
                first = new MagTemplateValue
                {
                    value = string.Format("提现金额失败，资金将退回xx账户。"),
                    color = "#173177"
                },
                money = new MagTemplateValue
                {
                    value = string.Format("{0}元", money.ToString("F2")),
                    color = "#173177"
                },
                time = new MagTemplateValue
                {
                    value = string.Format("{0}", DateTime.Now),
                    color = "#173177"
                },
                remark = new MagTemplateValue
                {
                    value = string.Format("如有疑问请联系客服"),
                    color = "#173177"
                },
            };
            const string templateid = "ddddd";

            AuthBase.SendTemplateMsg(openid, templateid, skipUrl, data);
        }

    }
}
