function renderPage(filePath) {
    $.ajax({
        url: filePath+'/model.json',
        dataType: 'json',
        type: 'GET',    
        error: function(msg) {
            alert("加载模板文件出错！");
        },
        success: function(msg) {
        	console.log(msg);
            var len = $('.app').find('.sub-intro').length;
            var data = msg;
            $('#audio').attr('oldsrc', data.music);
        	$('#audio').attr('src', data.music);
            $('#banner').attr('src', data.banner);
            $('#title').html(data.title);

            var goodsObj = data.goodsObj;
            $.each(goodsObj, function(i, obj) {
                var html;
                // console.log(obj);
                if(obj.type == 'txt') {
                    html = '<div class="uploadImgLabel">'+
                                '<div id="subIntro'+len+'" contenteditable="true" data-type="txt" class="sub-intro textarea e-textarea" placeholder="请输入文本">'+obj.cont+'</div>'+
                                '<span class="close">x</span>'+
                            '</div>';
                } else if(obj.type == 'img') {
                    html = '<label for="uploadImg" data-target="subIntro'+len+'" class="uploadImgLabel">'+
                                '<img id="subIntro'+len+'" class="sub-intro" data-type="img" src="'+obj.cont+'" alt="图片缺失">'+
                                '<span class="close">x</span>'+
                            '</label>';
                }
                $('.goodsDescribe-item .model-list-body .add-child').before(html);
            });
            $('#rules').html(data.rules);
            $('#connact').html(data.connact);
            
            var introduceObj = data.introduceObj;
            $.each(introduceObj, function(i, obj) {
                var html;
                // console.log(obj);
                if(obj.type == 'txt') {
                    html = '<div class="uploadImgLabel">'+
                                '<div id="subIntro'+len+'" contenteditable="true" data-type="txt" class="sub-intro textarea e-textarea" placeholder="请输入文本">'+obj.cont+'</div>'+
                                '<span class="close">x</span>'+
                            '</div>';
                } else if(obj.type == 'img') {
                    html = '<label for="uploadImg" data-target="subIntro'+len+'" class="uploadImgLabel">'+
                                '<img id="subIntro'+len+'" class="sub-intro" data-type="img" src="'+obj.cont+'" alt="图片缺失">'+
                                '<span class="close">x</span>'+
                            '</label>';
                }
                $('.introduce-item .model-list-body .add-child').before(html);
            });
            
            // $('#phoneNumber').val(data.phoneNumber);
            

            // $('#banner').attr('src', data.banner);
            // $('#banner').attr('src', data.banner);
            // $('#banner').attr('src', data.banner);
            // $('#banner').attr('src', data.banner);
		}
    });
}