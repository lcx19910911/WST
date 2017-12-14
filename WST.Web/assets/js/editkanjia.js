$(function () {
    // 日期时间选择
    var currYear = (new Date()).getFullYear();
    var opt = {};
    opt.date = { preset: 'date' };
    opt.datetime = { preset: 'datetime' };
    opt.time = { preset: 'time' };
    opt.default = {
        theme: 'android-ics light', //皮肤样式
        display: 'modal', //显示方式 
        mode: 'scroller', //日期选择模式
        lang: 'zh',
        startYear: currYear - 10, //开始年份
        endYear: currYear + 10 //结束年份
    };

    var optDateTime = $.extend(opt['datetime'], opt['default']);
    $("#startTime").mobiscroll(optDateTime).datetime(optDateTime);
    $("#endTime").mobiscroll(optDateTime).datetime(optDateTime);


    // 上传图片全局变量
    var uploadImgTarget;


    // 预览活动
    var judge = true;
    function audioAutoPlay(id) {
        var audio = document.getElementById(id);
        audio.play();
        document.addEventListener("WeixinJSBridgeReady", function () {
            audio.play();
        }, false);
        document.addEventListener('YixinJSBridgeReady', function () {
            audio.play();
        }, false);
    }
    audioAutoPlay('audio');

    $(document).on('click', '.view-btn', function () {
        if (!judge) {
            console.log('操作过于频繁');
            return false;
        }
        if ($(this).attr('data-status') == 'view') {
            $('.view-btn').text('返回编辑');
            $(this).attr('data-status', 'edit');
            $('.app').removeClass('edit').addClass('view');
            // setValue()
            $('.l .input').attr('disabled', true)
            $('.l .textarea').attr('contenteditable', false)
        } else {
            $('.view-btn').text('预览活动');
            $(this).attr('data-status', 'view');
            $('.app').removeClass('view').addClass('edit');
            $('.uploadImgLabel').removeClass('hidden');
            $('.l .input').attr('disabled', false)
            $('.l .textarea').attr('contenteditable', true)
        }
        judge = false;
        setTimeout(function () {
            judge = true;
        }, 1000);
    })
    // 显示音乐列表
    .on('click', '.music-btn', function () {
        $('.music-component-wrapper').removeClass('hidden')
    })
    // 选择音乐
    .on('click', '.music-component-wrapper .music-list', function () {
        $(this).addClass('play').siblings().removeClass('play');

        $('#audio').attr('src', $(this).attr('data-music'));
        // audioAutoPlay('audio')
    })
    // 取消
    .on('click', '.music-component-wrapper .cancel', function () {
        $('#audio').attr('src', $('#audio').attr('oldsrc'));
        $('.music-component-wrapper').addClass('hidden');
    })
    // 确认选择音乐
    .on('click', '.music-component-wrapper .sure', function () {
        $('#audio').attr('oldsrc', $('.play').attr('data-music'));
        $('.music-component-wrapper').addClass('hidden');
    })
    // 显示图片
    .on('click', '.change-bg-btn', function () {
        $('.head-bg-standby').removeClass('hidden');
    })
    // 添加子项
    .on('click', '.add-child span', function () {
        var type = $(this).attr('add-type');
        var html = '';
        var len = $('.app').find('.sub-intro').length;
        switch (type) {
            case 'txt':
                html = '<div class="uploadImgLabel"><div contenteditable="true" id="subIntro' + len + '" data-type="txt" class="sub-intro textarea e-textarea"  placeholder="请输入文本"></div><p class="v-p"></p><span class="close">x</span></div>';
                break;
            case 'img':
                html = '<label for="uploadImg" data-target="subIntro' + len + '" class="uploadImgLabel"><img id="subIntro' + len + '" class="sub-intro nosrc" data-type="img" src="" alt="上传图片"><span class="close">x</span></label>'
                break;
                // case 'video':
                // 	html = '';
                // 	break;
        }
        console.log(html);
        $(this).parent().before(html)

    })
    // 移除子项
    .on('click', '.uploadImgLabel .close', function (e) {
        e.preventDefault();
        $(this).parent().remove();
    })


    // 保存活动
    .on('click', '.save-btn', function () {
        setFormValue();

        if (!formRules()) {
            return false;
        }

        console.log('pass');
        var $form = $('#submit-form');
        var jsonData = serialize($form);
        setTimeout(function () {
            $form.ajaxSubmit({
                url: '/kanjia/manage',
                method: 'Post',
                data: { jsonData },
                success: function (data) {
debugger
                if(data.Code == 0)
                {
                    if (jsonData.ID != "") {
                        window.location.href = "/user/kanjia?id=" + jsonData.ID;
                    }
                    else {
                        alert("生成成功");
                        window.location.href = "/user/kanjia?id=" + data.Result;
                    }
                }
                else
                    alert(data.ErrorDesc);
               }
            })
        }, 800);

    })

    // 上传图片
        .on('click', '.uploadImgLabel', function () {
            uploadImgTarget = $(this).attr('data-target');
            console.log(uploadImgTarget)
        })
        .on('change', '#uploadImg', function () {
            $('#uploadImgForm').submit();
            console.log('submit complete!');
        });
    document.getElementById('exportimg').onload = function () {
        // $(document).on('load', '', function() {
        var ele = $(window.frames['exportimg'].document.body);
        console.log(ele);
        if (ele.text() == '') return false;
        //var res = JSON.parse(ele.text());
        var res = ele.text();
        console.log(res);
        console.log($("#" + uploadImgTarget))
        $("#" + uploadImgTarget).attr('src', res);
        $("#" + uploadImgTarget).removeClass('nosrc');
        $('#uploadImgForm')[0].reset();
    }



    function setFormValue() {

        // 大图
        var Picture = $('.head-bg-wrapper img').attr('src');
        $('.submit-form .Picture').val(Picture);
        console.log(Picture, '-----Picture')

        // 活动名称(大标题)
        // var name = $('#title').val();
        var name = $('#title').html();
        $('.submit-form .Name').val(name);
        console.log(name, '-----name');

        // 开始时间
        var startTime = $('#startTime').val();
        $('.submit-form .StartTime').val(startTime);
        console.log(startTime, '------startTime');

        // 结束时间
        var endTime = $('#endTime').val();
        $('.submit-form .EndTime').val(endTime);
        console.log(endTime, '------endTime');

        // 原价
        var oldPrice = $('#oldPrice').val();
        $('.submit-form .OldPrice').val(oldPrice)
        console.log(oldPrice, '-----oldPrice');

        // 最低价
        var lessPrice = $('#lessPrice').val();
        $('.submit-form .LessPrice').val(lessPrice)
        console.log(lessPrice, '-----lessPrice');

        // 单次砍价价格
        var oncePrice = $('#oncePrice').val();
        $('.submit-form .OncePrice').val(oncePrice)
        console.log(oncePrice, '-----oncePrice');

        // 砍价间隔时间
        var limitHour = $('#limitHour').val();
        $('.submit-form .LimitHour').val(limitHour)
        console.log(limitHour, '-----limitHour');

        // 奖品数量
        var prizeCount = $('#prizeCount').val();
        $('.submit-form .PrizeCount').val(prizeCount)
        console.log(prizeCount, '-----prizeCount');

        // // 拼团描述
        // var name = $('#title').html();
        // $('.submit-form .PinTuanItemInfo').val( $('#startTime').val() )

        // 商品json集合(商品描述)
        var goodsObj = [];
        $.each($('.goodsDescribe-item .sub-intro'), function (i, obj) {
            var t = $(obj).attr('data-type');
            var v = (t == 'img' ? $(obj).attr('src') : $(obj).html());
            if (v.length > 0) {
                var o = {
                    Type: t,
                    Content: v
                };
                goodsObj.push(o);
            }
        })
        goodsObj = JSON.stringify(goodsObj);
        $('.submit-form .GoodsItemsJson').val(goodsObj);
        console.log(goodsObj, '-----goodsObj');

        // 规则
        var rules = $('#rules').html();
        $('.submit-form .Rules').val(rules)
        console.log(rules, '-----rules');

        // 商家介绍集合
        var introduceObj = [];
        $.each($('.introduce-item .sub-intro'), function (i, obj) {
            var t = $(obj).attr('data-type');
            var v = (t == 'img' ? $(obj).attr('src') : $(obj).html());
            if (v.length > 0) {
                var o = {
                    Type: t,
                    Content: v
                };
                introduceObj.push(o);
            }
        })
        introduceObj = JSON.stringify(introduceObj);

        $('.submit-form .IntroduceTxtJson').val(introduceObj);
        console.log(introduceObj, '------introduceObj');


        // // 商家介绍图片集合
        // var name = $('#title').html();
        // $('.submit-form .StartTime').val( $('#startTime').val() )
        // // 商家介绍视频集合
        // var name = $('#title').html();
        // $('.submit-form .StartTime').val( $('#startTime').val() )

        // 联系
        var connact = $('#connact').html();
        $('.submit-form .Connact').val(connact)
        console.log(connact, '-----connact')

        // 咨询电话
        var phoneNumber = $('#phoneNumber').val();
        $('.submit-form .phoneNumber').val(phoneNumber)
        console.log(phoneNumber, '-----phoneNumber')

        // 背景音乐
        var audio = $('#audio').attr('src');
        $('.submit-form .MusicUrl').val(audio);
        console.log(audio, '-----audio')

        // // 特效名称
        // var name = $('#title').html();
        // $('.submit-form .StartTime').val( $('#startTime').val() )
        // // 特效方向
        // var name = $('#title').html();
        // $('.submit-form .StartTime').val( $('#startTime').val() )
    }

    // 预览赋值
    // function setValue() {
    // 	$.each($('.e-input'), function(i, obj) {
    // 		$(this).next('.v-span').text($(this).val());
    // 	})
    // 	$.each($('.e-textarea'), function(i, obj) {
    // 		$(this).next('.v-p').html($(this).val());
    // 	})
    // 	// validImgSrc()
    // 	$.each($('.uploadImgLabel').find('img'), function(i, obj) {
    // 		if($(obj).attr('src') == '') {
    // $(obj).parent('.uploadImgLabel').addClass('hidden')
    // 		}
    // 	})
    // }



    //表单序列化
    function serialize(formObj) {
        var map = {};
        formObj.find("input[name]:not(input[type='file']),select[name],textarea[name]").each(function (index, item) {

            var $item = $(item);
            var name = $item.attr("name");
            var value = $item.val();
            if ($item.is("input[type='radio'],input[type='checkbox']")) {
                if ($item.prop("checked")) {
                    if (map[name]) {
                        map[name] += "," + value;
                    } else {
                        map[name] = value;
                    }
                }
            } else {
                map[name] = value;
            }

        });
        return map;
    }


    function formRules() {


        var len = $('#submit-form .required').length;
        for (var i = 0; i < len; i++) {
            if ($('#submit-form .required').eq(i).val().length < 1) {
                alert($('#submit-form .required').eq(i).attr('data-required'));
                return false;
            }
        }

        var GoodsItemsJson = JSON.parse($('#submit-form .GoodsItemsJson').val());
        if (GoodsItemsJson.length <= 0 || GoodsItemsJson[0].cont == '') {
            alert('请填写第一条商品描述');
            return false;
        }

        var IntroduceTxtJson = JSON.parse($('#submit-form .IntroduceTxtJson').val());
        if (IntroduceTxtJson.length <= 0 || IntroduceTxtJson[0].cont == '') {
            alert('请填写第一条商家介绍信息');
            return false;
        }

        return true;

    }




    //   function validImgSrc(src) {
    //   	var img = new Image();
    //   	img.src = src;
    //   	console.log(img.fileSize);
    //   	if (img.fileSize > 0 || (img.width > 0 && img.height > 0)) { 
    //     return true;  
    // } else {
    //     return false;
    // }
    // img.remove();
    //   }

});