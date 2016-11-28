# WeChatPlatform
微信公众号API开发

## GetBaseAccessToken
基础支持：获取access_token

## GetBaseWeChatUserInfo
基础支持：获取用户信息

## GetQRcodeTicket
获取（临时）永久二维码的Ticket

## GetOAuthAccessToken
OAuth2.0鉴权：通过code换取网页授权access_token，此处的access_token为OAuth授权得到的token，切勿与基础支持内部的access_token混淆

## GetOAuthWeChatUserInfo
OAuth2.0鉴权：根据access_token和openid拉取用户信息(需scope为 snsapi_userinfo)

## SendCustomerMsg
发送客服消息。【仅对48小时活跃用户有效】

## SendTemplateMsg
发送消息-模板消息接口

### 四种基于任务奖励与用户提现到账情况的模板案例

#### SendTaskDoneMsg
任务完成通知

#### SendDrawApplyMsg
提现申请通知

#### SendDrawSucceedMsg
提现成功通知

#### SendDrawFailedMsg
提现失败通知

## WeChatAccountPay
【微信商户平台】账号支付

## WeChatAccountPayQuery
【微信商户平台】账号支付查询
