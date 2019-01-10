using iShare.WeChat.WeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iShare.WeChat.WeApi
{
    /// <summary>
    /// 微信授权url地址拼接
    /// </summary>
    public class WeRequestApi
    {
        /// <summary>
        /// 获取code的url地址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static string GetAuthorizeProxyUrl(string appId, string redirectUrl, string state, OAuthScope scope, string responseType = "code")
        {
            var url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect",
                                appId, System.Web.HttpUtility.UrlEncode(redirectUrl), responseType, scope, state);

            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }

        /// <summary>
        /// （网页授权无返回关注等信息）通过code换取网页授权access_token，url
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetAccessTokenUrl(string appid, string secret, string code)
        {
            /*
                首先请注意，这里通过code换取的是一个特殊的网页授权access_token,与基础支持中的access_token（该access_token用于调用其他接口）不同。
                公众号可通过下述接口来获取网页授权access_token。如果网页授权的作用域为snsapi_base，则本步骤中获取到网页授权access_token的同时，
                也获取到了openid，snsapi_base式的网页授权流程即到此为止。
             */
            return $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={secret}&code={code}&grant_type=authorization_code";
        }

        /// <summary>
        /// 刷新access_token
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="refresh_token">请求获取access_token时返回的refresh_token（有效时常为30天）</param>
        /// <returns></returns>
        public static string GetRefreshTokenUrl(string appid, string refresh_token)
        {
            /*
                由于access_token拥有较短的有效期，当access_token超时后，可以使用refresh_token进行刷新，
                refresh_token有效期为30天，当refresh_token失效之后，需要用户重新授权。
             */
            return $"https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={appid}&grant_type=refresh_token&refresh_token={refresh_token}";
        }

        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)，无关注信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetSnsapiUserinfoUrl(string access_token,string openid,string lang)
        {
            /*
                如果网页授权作用域为snsapi_userinfo，则此时开发者可以通过access_token和openid拉取用户信息了。
             */

            return $"https://api.weixin.qq.com/sns/userinfo?access_token={access_token}&openid={openid}&lang={lang}";
        }

        /// <summary>
        /// 检验授权凭证（access_token）是否有效
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string GetCheckAccessTokenUrl(string access_token,string openid)
        {
            return $"https://api.weixin.qq.com/sns/auth?access_token={access_token}&openid={openid}";
        }
    }
}
