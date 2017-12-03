function BOXOPEN (obj) {
	console.log(obj)
	$('.window-infor').fadeIn(200);
	if (obj.type == 1) {
		$('.prompt-box').show();
		$('.pb-txt').text(obj.txt);
		setTimeout(function() {
			$('.window-infor, .prompt-box').hide();
		},1200);
			
		
	} else if (obj.type == 2) {
		// $('.sb-btn-ok').text('确认支付');
		$('.select-box').show();
		$('.sb-txt').text('');
		console.log(obj.txt);
		$('.sb-txt').append('<div class="sbtxt-blod">'+obj.txt.blod+'</div>');
		$('.sb-txt').append('<div class="sbtxt-normal">'+obj.txt.normal+'</div>');

		
	}
	$(document).on('click', '.pb-btn', function() {
		$('.window-infor, .prompt-box').hide();
		// obj.funOk;
	});

}