 (function() {
     function isFirefox() {
         if(isFirefox=navigator.userAgent.indexOf("Firefox")>0) {
             return true;
         }else {
             return false;
         }
     }
     var mousewheel = isFirefox() ? "DOMMouseScroll" : "mousewheel";
    /*
     * 功能：弹窗警告
     * css需增加样式.ban-scroll {overflow:hidden;height:100%;}
     * str: 提示内容,
     * obj: {
     *   //DIY颜色搭配: bgColor, textColor, contColor, contBgColor,
     *   //确认触发函数：sure: function() {},
     *   //取消触发函数：cancel: function() {},
     * }
     */
    function alertVerifyFn(str,obj) {
        var body = document.getElementsByTagName('body')[0];
        // body.style.cssText = "overflow:hidden;height:100%;";
        var oldClass;
        (body.getAttribute('class') == null) ? oldClass = '' : oldClass = body.getAttribute('class');
        body.setAttribute('class', oldClass + ' ban-scroll');
        
        // if(document.getElementById('lVerify')) {
        //     document.getElementById('lVerifyCont').innerText = str;
        //     var ele = document.getElementById('lVerify');
        //     ele.style.display = 'block';
        // } else {
            var alertBox = document.createElement('div');
            var box = document.createElement('div');
            var title = document.createElement('h2');
            var contBox = document.createElement('div');
            var btnBox = document.createElement('div');
            var sureBtn = document.createElement('span');
            var cancelBtn = document.createElement('span');
            var mask = document.createElement('div');

            var bgColor = obj.bgColor || "#3f86d7",
                textColor = obj.textColor || "#fff",
                contColor = obj.contColor || "#333",
                contBgColor = obj.contBgColor || "#fff",
                fontSize = obj.fontSize || "16px",
                sureTxt = obj.sureTxt || "确定",
                cancelTxt = obj.cancelTxt || "取消";

            alertBox.id = 'lVerify';
            alertBox.style.cssText = "position:fixed;top:0;width:100%;max-width:650px;height:100%;z-index:1000;font-size:" + fontSize + ";";
            box.style.cssText = "position:absolute;top:20%;width:400px;text-align:center;font-size:1.2em;border-radius:0.3em 0.3em 0 0;overflow:hidden;";
            // console.log(document.body.offsetWidth * 0.7);

            title.style.cssText = "font-size:1em;background-color:" + bgColor + ";color:" + textColor + ";padding:0.3em 1em;box-sizing:border-box;text-align:left;";
            title.innerText = '警告！';
            box.appendChild(title);

            contBox.style.cssText = "color:#000;padding: 1.5em 1em;text-align:left;text-indent: 1.5em;background:" + contBgColor + ";";
            contBox.id = 'lVerifyCont';
            box.appendChild(contBox);
            
            sureBtn.innerText = sureTxt;
            cancelBtn.innerText = cancelTxt;
            sureBtn.style.cssText = "float:right;width:50%;background:" + bgColor + ";cursor:pointer;";
            cancelBtn.style.cssText = "float:left;width:50%;background:#ccc;cursor:pointer;";
            btnBox.appendChild(sureBtn);
            btnBox.appendChild(cancelBtn);
            btnBox.style.cssText = "overflow:hidden;padding:0;color:" + textColor + ";border-top:1px solid #eee;line-height:2.2em;font-size: 0.9em;";
            box.appendChild(btnBox);

            mask.style.cssText = "position:absolute;top:0;left:0;width:" + document.body.offsetWidth + "px;height:100%;background:rgba(0,0,0,0.6);";

            alertBox.appendChild(mask);
            alertBox.appendChild(box);

            body.appendChild(alertBox);
            contBox.style.color = contColor;
            contBox.innerText = str;

            box.style.left = (document.body.offsetWidth - box.offsetWidth) / 2 + 'px';
            box.style.top  = (window.screen.height - box.offsetHeight) * 0.8 / 2 + 'px';

            sureBtn.onclick = function() {
                body.setAttribute('class', oldClass);
                // alertBox.style.display = 'none';
                // body.style.cssText = "overflow:visible;height:auto;";
                obj.sure();
                alertBox.parentNode.removeChild(alertBox);
            };
            cancelBtn.onclick = function() {
                body.setAttribute('class', oldClass);
                // alertBox.style.display = 'none';
                // body.style.cssText = "overflow:visible;height:auto;";
                if(obj.cancel != null || typeof(obj.cancel) != 'undefined') {
                    obj.cancel();
                }
                alertBox.parentNode.removeChild(alertBox);
            };
        // }
    }
    /*
     * 功能：移动端图片放大、拖拽
     * selector: 通过 id选择器 获取到的 img标签！
     */ 
    var touchScale = function(selector) {
        var startX, endX, scale, x1, x2, y1, y2, imgLeft, imgTop, imgWidth, imgHeight, dragLeft, dragTop,
            one = false,
            $touch = $(selector),
            originalWidth = $touch.width(),
            originalHeight = $touch.height(),
            baseScale = parseFloat(originalWidth/originalHeight),
            imgData = [];
        function siteData(name) {
            imgLeft = parseInt(name.css('left'));
            imgTop = parseInt(name.css('top'));
            imgWidth = name.width();
            imgHeight = name.height();
        }
        $(document).on('touchstart touchmove touchend', '#' + selector.id, function(event){
            event.preventDefault();
            var $me = $(selector),
                touch1 = event.originalEvent.targetTouches[0],  // 第一根手指touch事件
                touch2 = event.originalEvent.targetTouches[1],  // 第二根手指touch事件
                fingers = event.originalEvent.touches.length;   // 屏幕上手指数量
            //手指放到屏幕上的时候，还没有进行其他操作
            if (event.type == 'touchstart') {
                if (fingers == 2) {
                    // 缩放图片的时候X坐标起始值
                    startX = Math.abs(touch1.pageX - touch2.pageX);
                    one = false;
                }
                else if (fingers == 1) {
    //                          预存手指刚点击时的位置
                    x1 = touch1.pageX;
                    y1 = touch1.pageY;
    //                             获取图片当前的位置
                    imgLeft = $me.css('left');
                    imgTop = $me.css('top');
                    one = true;
                }
                siteData($me);
            }
            //手指在屏幕上滑动
            else if (event.type == 'touchmove') {
                if (fingers == 2) {
                    // 缩放图片的时候X坐标滑动变化值
                    endX = Math.abs(touch1.pageX - touch2.pageX);
                    scale = endX - startX;
                    $me.css({
                        'width' : originalWidth + scale,
                        'height' : (originalWidth + scale)/baseScale,
                        'left' : imgLeft,
                        'top' : imgTop
                    });

                }else if (fingers == 1) {
    //                          手指拖动图片实时的位置
                    x2 = touch1.pageX;
                    y2 = touch1.pageY;
                    if (one) {
                        dragLeft = imgLeft + (x2 - x1);
                        dragTop = imgTop + (y2 - y1);
                        $me.css({
                            'left' : dragLeft,
                            'top' : dragTop,
                        });
                    }

                }
            }
            //手指移开屏幕
            else if (event.type == 'touchend') {
                // 手指移开后保存图片的宽
                originalWidth = $touch.width();
                siteData($me);
            }
        });
    };

    /*
     * demo:
     * popUp({
     *    str: "xxx给你发送了消息",
     *    clickFn: function() {},
     * });
     */
    function popup(obj) {
        var time = obj.time || 5000;
        var timer = null;
        var el = document.createElement('div');
        el.id = "popUp";
        el.style = "position:fixed;top:1%;left:0;width:100%;height:0;z-index: 100;";
        var text = document.createElement('div');
        text.style = "width:60%;margin:0 auto;padding:1em;background:#3ce2ff;border-radius:7px;border:1px solid #81b4fe;text-align:center;"
        text.innerText = obj.str || "没有填写提示内容";
        el.appendChild(text);
        document.body.appendChild(el);
        
        timer = setTimeout(function() {
            el.parentNode.removeChild(el);
        }, time)
        text.onclick = function() {
            if(timer) {
                clearTimeout(timer);
            }
            el.parentNode.removeChild(el);
            obj.clickFn();
        }
    };
     //获取标签


     // 设置cookie
    function setCookieFn(c_name,value,min) {
        var exdate=new Date();
        exdate.setTime(exdate.getTime() + min*1000*60);
        document.cookie=c_name+ "=" + escape(value) + ((min==null) ? "" : ";expires="+exdate.toString())+";path=/";
    }
    // 读取cookie
    function getCookieFn(c_name) {
        if (document.cookie.length>0)
        {
            c_start=document.cookie.indexOf(c_name + "=")
            if (c_start!=-1)
            {
                c_start=c_start + c_name.length+1
                c_end=document.cookie.indexOf(";",c_start)
                if (c_end==-1) c_end=document.cookie.length
                // console.log(unescape(document.cookie.substring(c_start,c_end)))
                return unescape(document.cookie.substring(c_start,c_end))
            }
        }
        return ""
    }

    
    // 获取网址上的参数值
    function GetQueryVal(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return  unescape(r[2]); return null;
    }
    //过滤头尾空格
    function trimStr(str) {
        return str.replace(/(^\s*)|(\s*$)/g,"");
    }
    function setCookie(c_name,value,expiredays) {
        return setCookieFn(c_name,value,expiredays);
    }
    function getCookie(name) {
        return getCookieFn(name);
    }
    function popUp(obj) {
        return popup(obj);
    }
    function alertVerify(str, obj) {
        return alertVerifyFn(str, obj);
    }
    function touchMove(selector) {
        return touchScale(selector);
    }


    window.l = new Object;;

    window.l.apiUrl = 'http://wst.fjyouwo.com'

    window.l.trimStr = trimStr;
    window.l.getCookie = getCookie;
    window.l.setCookie = setCookie;
    window.l.alertVerify = alertVerify;
    window.l.touchMove = touchMove;
    window.l.popUp = popUp;
    window.l.GetQueryVal = GetQueryVal;
 })();
$(function() {
   $(window).resize(infinite);
   function infinite() {
       var htmlWidth = $('html').width();
       if (htmlWidth >= 650) {
           $("html").css({
               "font-size" : "24px"
           });
       } else if(htmlWidth <= 320) {
           $("html").css({
               "font-size" : "12px"
           });
       } else {
           $("html").css({
               "font-size" :  12 / 320 * htmlWidth + "px"
           });
       }
   }infinite();
})