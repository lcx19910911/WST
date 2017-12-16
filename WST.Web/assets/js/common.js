$(function() {
	var clickJudge = true,
		timer = null;
	
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
		$('.player-btn').removeClass('load');
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
	// 
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
})