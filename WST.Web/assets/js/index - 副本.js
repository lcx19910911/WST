Vue.directive('drag',function(el, binding){//自定义指令 拖拽元素
    el.onmousedown = function(e){
    	window.event? window.event.cancelBubble = true : e.stopPropagation();
    	var target = e.target;
    	// if($(target).parents('.page').siblings('.editor-box').find('.editor').attr('status') == 0) return false; 
    	$('.select').removeClass('select');
    	$('.el-select-dropdown').hide();
    	$(el).parent().addClass('select');
  //   	if(app.temp != null) {
		// 	app.temp.zindex = app.temp.zindex*1 - 100;
  //   	}
		// app.temp = binding.value;
		// app.temp.zindex = app.temp.zindex*1 + 100;
  //   	console.log(binding.value.zindex);

        var disX = e.clientX -$(el).parent()[0].offsetLeft;
        var disY = e.clientY - $(el).parent()[0].offsetTop;
        document.onmousemove = function(e){
        	e.preventDefault();
            var l = e.clientX-disX;
            var t = e.clientY-disY;
            binding.value.positionX = l;
            binding.value.positionY = t;
            // 超出编辑区停止
            // $(el).parents('.pages')[0].onmouseleave = function() {
            // 	document.onmousemove=null;
            // 	document.onmouseup=null;
            // }
        };
        document.onmouseup = function(){
            document.onmousemove=null;
            document.onmouseup=null;
        };
    };
});
Vue.directive('dragtool',function(el, binding){//自定义指令 拖拽工具栏
    el.onmousedown = function(e){
    	window.event? window.event.cancelBubble = true : e.stopPropagation();
        var disX = e.clientX -$(el).parent()[0].offsetLeft;
        var disY = e.clientY - $(el).parent()[0].offsetTop;
        document.onmousemove = function(e){
        	e.preventDefault();
            var l = e.clientX-disX;
            var t = e.clientY-disY;
            // 拖拽区域限制
            if(t <= 50) t=50;
            if(l <= 200) l=200;
            if(t > $(window).height() - $(el).parent().height()) t = $(window).height() - $(el).parent().height();
            if(l > $(window).width() - $(el).parent().width()) l = $(window).width() - $(el).parent().width();
            binding.value.position = [l, t];
        };
        document.onmouseup = function(){
            document.onmousemove=null;
            document.onmouseup=null;
        };
    };
});
Vue.directive('dragnum',function(el, binding){//自定义指令 拖拽修改数据
    el.onmousedown = function(e){
        var obj = binding.value[2];
        var minNum = binding.value[3];
        var maxNum = binding.value[4];
        // console.log(maxNum, minNum)
    	window.event? window.event.cancelBubble = true : e.stopPropagation();
        var disX = e.clientX -$(el).parent()[0].offsetLeft;
        var x = binding.value[1];
        document.onmousemove = function(e){
        	e.preventDefault();
            var l = e.clientX-disX;
            switch(obj) {
            	case 'animateSeed':
            		binding.value[0][obj] = parseFloat(x*1+l*1/6).toFixed(1);
            		if(binding.value[0][obj] <= minNum) binding.value[0][obj] = minNum;
            		if(binding.value[0][obj] >= maxNum) binding.value[0][obj] = maxNum+'.0';
            		break;
            	case 'delay':
            		binding.value[0][obj] = parseFloat(x*1+l*1/12).toFixed(1);
            		if(binding.value[0][obj] <= minNum) binding.value[0][obj] = minNum;
            		if(binding.value[0][obj] >= maxNum) binding.value[0][obj] = maxNum+'.0';
            		break;
                default:
                    binding.value[0][obj] = parseInt(x*1+l*1);
                    if(binding.value[0][obj] <= minNum) binding.value[0][obj] = minNum;
                    if(binding.value[0][obj] >= maxNum) binding.value[0][obj] = maxNum;
            }
        };
        document.onmouseup = function(){
            document.onmousemove=null;
            document.onmouseup=null;
        };
    };
});
Vue.directive('rmclass',function(el, binding) {//自定义指令
	el.onclick = function(e) {
		$('.page-item').removeClass(binding.value);
	}
})
Vue.directive('selvalue',function(el, binding) {//自定义指令
	$(el).on('dblclick', function(e) {
		$(this).select();
	})
})

Vue.directive('uploadbg',function(el, binding) {//自定义指令
	$(el).on('change', function() {
		$(el).parent().submit();
	})
})


// Vue.component('my-component', {
// 	props: ['message'],
// 	template: '<div>{{message}} A custom component!</div>',
// 	data: function() {
// 		return {
// 			msg: "lhzong"
// 		}
// 	}
// })
var app = new Vue({
	el:'#app',
	data:{
        msgInfo: '',
        msgTimer: null,
		pageData: [],//初始化数据
        currentPage: {},//当前页
        currentId: '',
		animates: [//动画效果选项
			{value:"",label:"无动画"},
			{value:"fadeIn",label:"淡入淡出"},
			{value:"fromRight",label:"从右滑入"},
			{value:"fromLeft",label:"从左滑入"},
			{value:"fromTop",label:"从上滑入"},
			{value:"fromBottom",label:"从下滑入"}
		],
        classify: { //分类
            'seckill': '秒杀',
            'group': '拼团',
            'bargainirg': '砍价',
            'help': '助力',
            'coupon': '优惠券',
            'redPaper': '扫码红包'
        },
        classList: [ //分类
            {'name': 'seckill',   'cid': '1'},
            {'name': 'group',     'cid': '2'},
            {'name': 'bargainirg','cid': '3'},
            {'name': 'help',      'cid': '4'},
            {'name': 'coupon',    'cid': '5'},
            {'name': 'redPaper',  'cid': '6'}
        ],
        widgetList: [
            {'type': 'title','name': '标题'},
            {'type': 'slide','name': '轮播图'},
            {'type': 'textarea','name': '文本域'},
            {'type': 'inputAny','name': '输入框'},
            {'type': 'inputNum','name': '数字输入框'},
            {'type': 'button','name': '按钮'}
        ],
        createModuleShow: false,
		editingPage: 0,//当前编辑页
		toolbar: {//工具栏选项
			tab: 0,
			position: [700,143]
		},
		mouseRight: {//初始化右键菜单位置
			position: [100,100]
		},
		win: {//窗口设定
			W: 375,
			H: 667
		},
		template: {//初始化图片/音乐库
			type: 'none',
			imgArr: [],
			musicArr: []
		},
        widgetShow: false
	},
	mounted: function() {
        console.log(l.getCookie('currentId'), 'cookie');
        if(l.getCookie('currentId') != '') {
            this.currentId = l.getCookie('currentId');
            this.getCustomers();
        } else {
    		this.getCustomers(1);
        }
	},
	methods: {
        msg: function(string) {
            $('.msg').addClass('msgShow');
            if(this.msgTimer) clearTimeout(this.msgTimer);
            this.msgInfo = string || 'error';
            this.msgTimer = setTimeout(function() {
                $('.msg').removeClass('msgShow');
            }, 3000)
        },
		getCustomers: function(a) {//获取数据
            var that = this;
            // console.log(that.currentPage)
            this.$http.get('./data.json?v='+Math.random())
            .then(function(res) {
                that.pageData = res.data;
                that.pageData.pages.sort(function(a,b) {
                    return b.create_time - a.create_time;
                })

                if(a) return false;
                $.each(that.pageData.pages, function(i, obj) {
                    // console.log(that)
                    if(obj.id == that.currentId) {
                        that.currentPage = obj;
                        return false;
                    }
                })

            }).catch(function(res) {
                console.log(res)
            })
        },
        getTemplate: function(type) {//获取图片/音乐库
            this.$http.get('./template.json?v='+Math.random())
            .then(function(res) {
                console.log(res);
                this.template = res.data;
                this.template.type = type;
                this.templateShow();
                	// this.create(data, 'img', res.name);
            }).catch(function(res) {
                console.log(res)
            })
        },
        templateHide: function() {//隐藏图片/音乐库
        	$('.template-wrapper').animate({
                	"right":'-301px'
            },200)
        },
        templateShow: function() {//显示图片/音乐库
        	$('.template-wrapper').animate({
                	"right":'0'
            },300)
        },
        setBgMusic: function(item) {//设置背景音乐
        	// this.pageData.BGM = item;
            this.currentPage.BGM = item;
        },
        selText: function(e) {//全选文本内容
        	var el = e.target;
        	// if($(el).parents('.page').prev('.editor-box').find('.editor').attr('status') == 0) return false;
        	
        	if(this.toolbar.tab == 1) {
        		this.toolbar.tab = 0;
        		setTimeout(function() {
        			$(el).parent().next('.tool-bar').find('.e-cont input').focus().select();
        		},150)
        	} else {
	        	$(el).parent().next('.tool-bar').find('.e-cont input').focus().select();
        	}
        },
        create: function(type, cont) {//创建元素
        	var that = this;
        	var newObj = {//初始化元素数据
        		'type': type,
        		'cont': cont || '双击修改',
        		'size': (type == 'img') ? 60 : 24,
                'positionX': 100,
        		'positionY': 100,
        		'opacity': 100,
        		'blur': 0,
        		'zindex': (type == 'img') ? 5 : 10,
        		'color': '#333333',
        		'animate': '',
        		'animateSeed': 1,
        		'delay': 0
        	}
        	this.currentPage.object.push(newObj);
        	setTimeout(function() {// 新建元素后聚焦
	        	$('.select').removeClass('select');
        		$('.pages').find('.page-item').last(0).addClass('select');
        	},100);
        },
        selectImg: function(item, type) {//修改背景图/创建图片元素
        	if(type == 'bg') {
        		if(item == null) {
        			this.currentPage.background = 'none';
        			return false;
        		}
        		this.currentPage.background = item.name;
        	} else {
        		this.create('img', item.name);
        	}
        },
        delEl: function(arr, b) {//删除指定元素
        	l.alertVerify('是否删除所选内容？', {
        		sure: function() {
        			arr.splice(b, 1);
        		}
        	})
        },
        clearItem: function() {//清空页面内容
        	var str = '是否清空该页内容？';
        	var that = this;
            alert('error');
        	// l.alertVerify(str, {
        	// 	sureTxt: "清空",
        	// 	sure: function() {
        	// 		that.pageData.pages[that.editingPage].object = [];
        	// 		that.pageData.pages[that.editingPage].background = 'none';
        	// 	}
        	// })
        },
        fileLoad: function(iframe, data, e) {//上传成功更新图片/音乐库
        	var ele = $(window.frames[iframe].document.body);
        	if(ele.text() == '') return false;
        	var res = JSON.parse(ele.text());
        	if(res.status == 1) {
        		if(res.type == 'img') {
		        	data.imgArr.push({name:res.name,pid:res.pid});
        		} else {
        			data.musicArr.push({name:res.name,mid:res.mid});
        		}
        	} else {
        		alert(res.msg);
        	}
        	$(e.target).prev()[0].reset();
        },
        saveFn: function(obj) {//保存数据

        	$.post("./php/updataJSON.php",{obj:obj},function(res){
        		console.log(JSON.parse(res));
        	});
        },
        add: function(item, obj, num) {//数字加
        	var a;
        	switch(obj) {
        		case 'positionX':
        			if(item[obj] >= num) return false; 
        			var x = item[obj];
        			x = x*1 + 1;
        			item[obj] = x;
        			break;
        		case 'positionY':
        			if(item[obj] >= num) return false; 
        			var y = item[obj];
        			y = y*1 + 1;
        			item[obj] = y;
        			break;
        		case 'animateSeed':
        			if(item[obj] >= num) return false;
        			item[obj] = (item[obj]*1 + 0.1).toFixed(1);
        			break;
        		case 'delay':
        			if(item[obj] >= num) return false;
        			item[obj] = (item[obj]*1 + 0.1).toFixed(1);
        			break;
                default:
                    if(item[obj] >= num) return false; 
                    item[obj]++;
                    // console.log(item[obj])
        	}
        	
        },
        reduce: function(item, obj, num) {//数字减
        	switch(obj) {
        		case 'positionX':
        			var x = item[obj];
        			x = x - 1;
        			item[obj] = parseInt(x);
        			break;
        		case 'positionY':
        			var y = item[obj];
        			y = y - 1;
        			item[obj] = parseInt(y);
        			break;
        		case 'animateSeed':
        			if(item[obj] <= num) return false;
        			item[obj] = (item[obj]*1 - 0.1).toFixed(1);
        			break;
        		case 'delay':
        			if(item[obj] <= num) return false;
        			item[obj] = (item[obj]*1 - 0.1).toFixed(1);
        			break;
                default:
                    if(item[obj] <= num) return false; 
                    item[obj]--;
        	}
        },
        editing: function(dataItem, e) {//选择编辑页
        	e.preventDefault();
            var that = this;
            if(this.currentId == dataItem.id) return false;
            if(this.currentId == '') {
                this.editingPage = dataItem.id;
                this.currentId = dataItem.id;
                this.getCustomers();
            } else {
                l.alertVerify('离开页面前请确认已保存当前数据！', {
                    sureTxt: '确定',
                    sure: function() {
                        that.editingPage = dataItem.id;
                        that.currentId = dataItem.id;
                        that.getCustomers();
                    }
                })
            }
            l.setCookie('currentId', this.currentId, 1)
        },
        delPage: function(data, index, e) {//删除页面
        	e.preventDefault();
        	var that = this;
        	$('.rightMouseMask').hide();
        	var str = '是否删除该页?';
        	l.alertVerify(str, {
        		sureTxt: '删除',
        		sure: function() {
        			// 删除
        			data.pages.splice(index, 1);
        			// 删除后自动选择新的编辑页
        			if(index > that.editingPage) return false;
        			that.editingPage--;
        			if(that.editingPage <= 0) that.editingPage = 0;
        		}
        	})
        },
        rightMouseClick: function(e) {//右键弹出菜单
        	e.preventDefault();
        	this.mouseRight.position = [e.clientX, e.clientY];
        	var el = e.target;
        	$(el).next('.rightMouseMask').show();
        },
        rightMouseHidden: function() {//右键弹出隐藏
        	$('.rightMouseMask').hide();
        },
        copyPage: function(data, item) {//创建 | 复制并生成该页
        	var that = this;
        	var maxNum = 1;
        	$.each(data.pages, function(i, obj) {//获取最高页面序号
        		if(obj.sequence > maxNum) {
        			maxNum = obj.sequence;
        		}
        	});
        	maxNum++;
        	//创建新空白页面
        	var obj = new Object({
        		sequence: maxNum,
        		background: 'none',
        		object: []
        	})
        	//如果存在第二个参数，对新页面赋值(克隆)
        	if(item != null) {
        		obj.background = item.background;
        		$.each(item.object, function(i, v) {
        			var a = that.copyObj(v);
        			obj.object.push(a);
        		})
        	}
        	data.pages.push(obj);
        	$('.rightMouseMask').hide();
        	// 自动选中新增的页面
        	this.editingPage = data.pages.length - 1;
        },
        preventDefault: function(e) {//阻止默认
        	e.preventDefault();
            this.rightMouseHidden();
        },
        copyObj: function(obj){//复制对object
            var newobj = {};
            for ( var attr in obj) {
                newobj[attr] = obj[attr];
            }
            return newobj;
        },
        cancelSel: function() {//关闭工具栏
        	$('.select').removeClass('select');
        },
        moveUpPage: function(data, index, e) {//与页面序号前一个互换
        	data.pages[index] = [
        		data.pages[index-1],
        		data.pages[index-1] = data.pages[index]
        	][0];
        	data.pages[index].sequence = [
        		data.pages[index-1].sequence,
        		data.pages[index-1].sequence = data.pages[index].sequence
        	][0];
        	$('.rightMouseMask').hide();
        	if(this.editingPage == index) {
        		this.editingPage--;
        	} else if(this.editingPage == index-1) {
        		this.editingPage++;
        	}
        },
        moveDownPage: function(data, index, e) {//与页面序号后一个互换
        	data.pages[index] = [
        		data.pages[index+1],
        		data.pages[index+1] = data.pages[index]
        	][0]
        	data.pages[index].sequence = [
        		data.pages[index+1].sequence,
        		data.pages[index+1].sequence = data.pages[index].sequence
        	][0];
        	if(this.editingPage == index) {
        		this.editingPage++;
        	} else if(this.editingPage == index+1) {
        		this.editingPage--;
        	}
        	$('.rightMouseMask').hide();
        },
        toolbarTab: function(index) {//工具栏tab切换
        	this.toolbar.tab = index;
        },
        animate: function(item, e) {//动画
        	if(item.animate == "" || item.animate == null) {
        		this.toolbar.tab = 1;
        		return false;
        	}
        	var el = e.target;
        	$(el).parents('.tool-bar').prev().addClass(item.animate);
        	var timer = setTimeout(function() {
        		$(el).parents('.tool-bar').prev().removeClass(item.animate);
        	}, item.animateSeed*1000+item.delay*1000)
        },
        removeImg: function(item, index) {//删除图片
        	var str = "删除后将无法恢复，是否确认删除？";
        	l.alertVerify(str, {
        		sure: function() {
        			item.splice(index, 1);
        		}
        	})
        },
        createModule: function() {
            this.createModuleShow = true;
        },
        selClassify: function(item) { //创建元素选择分类
            this.msg(item.name)
        },
        showWidget: function() {
            if(this.widgetShow) {
                this.widgetShow = false;
            } else {
                this.widgetShow = true;
            }
        }
	}
})