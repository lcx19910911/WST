$(function () {
	var currYear = (new Date()).getFullYear();	
	var opt={};
	opt.date = {preset : 'date'};
	opt.datetime = {preset : 'datetime'};
	opt.time = {preset : 'time'};
	opt.default = {
		theme: 'android-ics light', //皮肤样式
        display: 'modal', //显示方式 
        mode: 'scroller', //日期选择模式
		lang:'zh',
        startYear:currYear - 10, //开始年份
        endYear:currYear + 10 //结束年份
	};

  	var optDateTime = $.extend(opt['datetime'], opt['default']);
    $("#startTime").mobiscroll(optDateTime).datetime(optDateTime);
    $("#endTime").mobiscroll(optDateTime).datetime(optDateTime);

    var judge = true;
    $(document).on('click', '.view-btn', function() {
    	if(!judge) {
    		console.log('操作过于频繁');
    		return false;
    	}
    	if($(this).attr('data-status') == 'view') {
    		$('.view-btn').text('返回编辑');
    		$(this).attr('data-status', 'edit');
    		$('.app').removeClass('edit').addClass('view');
    		setValue()
    	} else {
    		$('.view-btn').text('预览活动');
    		$(this).attr('data-status', 'view');
    		$('.app').removeClass('view').addClass('edit');;
    	}
    	judge = false;
    	setTimeout(function() {
    		judge = true;
    	}, 1000);
    }).on('click', '.music-btn', function() {
    	
    })


    function setValue() {
    	$.each($('.e-input'), function(i, obj) {
    		$(this).next('.v-span').text($(this).val());
    	})
    	$.each($('.e-textarea'), function(i, obj) {
    		$(this).next('.v-p').html($(this).val());
    	})
    }

});