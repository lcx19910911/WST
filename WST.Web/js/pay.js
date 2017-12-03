function WxPay(orderId)
{
    $.ajax({
        type: "GET",
        url: "/WxPay/GetPayParms",
        success: function (data) {
            $(".pay-btny").removeAttr("disabled");//获取到配置，打开付款按钮
            wx.config({
                debug: true, // 开启调试模式,成功失败都会有alert框
                appId: data.appId, // 必填，公众号的唯一标识
                timestamp: data.timeStamp, // 必填，生成签名的时间戳
                nonceStr: data.nonceStr, // 必填，生成签名的随机串
                signature: data.signature,// 必填，签名
                jsApiList: ['chooseWXPay'] // 必填，需要使用的JS接口列表
            });
            wx.ready(function () {

                wx.chooseWXPay({
                    "timestamp": data.timeStamp, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
                    "nonceStr": data.nonceStr, // 支付签名随机串，不长于 32 位
                    "package": data.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                    "signType": data.signType, // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                    "paySign": data.paySign, // 支付签名
                    success: function (res) {
                        // 支付成功后的回调函数
                        if (res.err_msg == "get_brand_wcpay_request：ok") {
                            // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。
                            window.location.href = window.location.host + '/order/detail?orderId=' + orderId;
                        } else {
                            var boxObj1 = BOXOPEN({
                                type: 1,
                                txt: res.err_msg
                            });
                            return false;
                        }
                    }
                });

                // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
            });
            wx.error(function (res) {

                var boxObj1 = BOXOPEN({
                    type: 1,
                    txt: res.err_msg
                });
                // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
            });
        }
    });
}