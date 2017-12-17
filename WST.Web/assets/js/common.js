$(function() {
	var clickJudge = true,
		timer = null;

	// function loadAudio(src, callback) {
	//     var audio = new Audio(src);
	//     audio.onloadedmetadata = callback;
	//     audio.src = src;
	// }

	// var audioUrl = "http://wst.fjyouwo.com/Upload/music/cace311f7c834e96b57dd4796d1dd7f9.mp3";//音频路径
	// //调用方法
	// loadAudio(audioUrl,function(){
	//     $('#music').attr("src",audioUrl);
	//     alert("加载完成");
	// });
	
	function audioAutoPlay(id){  
	    var audio = document.getElementById(id);  
	    audio.play();  
	    document.addEventListener("WeixinJSBridgeReady", function () {  
	            audio.play();  
	    }, false);  
	    document.addEventListener('YixinJSBridgeReady', function() {  
	        audio.play();  
	    }, false);  
	}
	audioAutoPlay('audio');


	$('#audio')[0].onloadedmetadata = function() {
		$('.player-btn').addClass('play');
		$('.player-btn').attr('data-status', 'play');
	}
	


	$(document).on('click', '.amask', function() {
		$(this).parent().addClass('hidden');
	})
	// 音乐暂停/播放
	.on('click', '.player-btn', function() {
		if($(this).attr('data-status') == 'play') {
			$(this).attr('data-status', 'pause');
			$(this).addClass('pause').removeClass('play');
			$('#audio')[0].pause();
		} else {
			$(this).attr('data-status', 'play');
			$(this).addClass('play').removeClass('pause');
			$('#audio')[0].play();
		}
	})
	// 分享
	.on('click', '.share-info', function() {
		if(!clickJudge) return false;
		clickJudge = false;
		setTimeout(function() {
			clickJudge = true;
		}, 1000); 
		$('.share-page-wrapepr').removeClass('hidden');
		if(timer) clearTimeout(timer);
		timer = setTimeout(function() {
			$('.share-page-wrapepr').addClass('hidden');
		}, 7000);
	})
	// 
	// 显示音乐列表
	.on('click', '.music-btn', function() {
		$('.music-component-wrapper').removeClass('hidden')
	})
	// 选择音乐
	.on('click', '.music-component-wrapper .music-list h3', function() {

	    if($(this).parent().hasClass('open')) {
	        $(this).parent().removeClass('open');
	    } else {
	        $(this).parent().addClass('open');
	    }
	})
	// 
	.on('click', '.music-item', function() {
	    $('.music-item').removeClass('play');
	    $(this).addClass('play');
	    $('#audio').attr('src', $(this).attr('data-music'));
	    audioAutoPlay('audio');
	})
	// 取消
	.on('click', '.music-component-wrapper .cancel', function() {
		$('#audio').attr('src', $('#audio').attr('oldsrc'));
		$('.music-component-wrapper').addClass('hidden');
	})
	// 确认选择音乐
	.on('click', '.music-component-wrapper .sure', function() {
		$('#audio').attr('oldsrc', $('#audio').attr('src'));
		$('.music-component-wrapper').addClass('hidden');
	})
})