Vue.directive('drag',function(el, binding){//自定义指令 拖拽元素
    el.onmousedown = function(e){
    	window.event? window.event.cancelBubble = true : e.stopPropagation();
    	var target = e.target;
    	$('.select').removeClass('select');
    	$(el).parents('.page-item').addClass('select');

        var disX = e.clientX -$(el)[0].offsetLeft;
        var disY = e.clientY - $(el)[0].offsetTop;
        document.onmousemove = function(e){
        	e.preventDefault();
            var l = e.clientX-disX;
            var t = e.clientY-disY;
            binding.value.left = l;
            binding.value.top = t;
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



var app = new Vue({
	el:'#app',
	data:{
        msgInfo: '',
        msgTimer: null,
        sectionStyle: false,
		pageData: [],//初始化数据
        currentPage: {},//当前页
        currentId: '',
        img: { //上传图片配套1/4
            obj: {},
            name: ''
        },
        imgList: {},
        textList: {},
		// animates: [//动画效果选项
		// 	{value:"",label:"无动画"},
		// 	{value:"fadeIn",label:"淡入淡出"},
		// 	{value:"fromRight",label:"从右滑入"},
		// 	{value:"fromLeft",label:"从左滑入"},
		// 	{value:"fromTop",label:"从上滑入"},
		// 	{value:"fromBottom",label:"从下滑入"}
		// ],
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
            // {'type': 'title','name': '标题'},
            // {'type': 'logo','name': 'logo'},
            {'type': 'DIYModules', 'name': '自定义模块'},
            {'type': 'slide','name': '轮播图'},
            {'type': 'image','name': '图片'},
            // {'type': 'textarea','name': '文本域'},
            // {'type': 'inputAny','name': '输入框'},
            // {'type': 'userInfoModel','name': '用户信息'},
            {'type': 'button','name': '按钮'},
            {'type': 'activityTime','name': '活动时间'},
        ],
        createModuleShow: false,
		editingPage: 0,//当前编辑页
		toolbar: {//工具栏选项
			tab: 0,
			position: [700,230]
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
        widgetShow: false, //显示控件窗口
        diyModelTemp: {
            titleBg: '',
            bg: ''
        }
	},
	mounted: function() {
      //   console.log(l.getCookie('currentId'), 'cookie');
      //   if(l.getCookie('currentId') != '') {
      //       this.currentId = l.getCookie('currentId');
            this.getCustomers();
      //   } else {
    		// this.getCustomers(1);
      //   }
	},
	methods: {
        // sss(item) {
        //     var that = this;
        //     // console.log(item);
        //     $('#export').load(function() {
        //         that.diyModelTemp.titleBg = item.title.bg;
        //         // console.log(item);
        //     })
        // },
        clearFontStyle: function(list) {
            list.weight = 'false';
            list.italic = 'false';
            list.underline = 'false';
            list.color = '#333';
            list.align = 'left';
            list.size = 'n';
        },
        setValue: function(item, key, value) {
            item[key] = value;
        },
        checkJudge: function(obj, name) {
            if(obj[name] == "true") {
                obj[name] = "false";
            } else {
                obj[name] = "true";
            }
        },
        setSection: function(list) {
            this.sectionStyle = true;
            this.textList = list;
        },
        uploadImg: function(list, name) { //上传图片配套2/4
            // this.imgList = list;
            this.img = {
                obj: list,
                name: name
            }
            this.$refs.fileImg.click();
        },
        imgChange: function(e) { //上传图片配套3/4
            // if(!/^[(a-z)(A-Z)(0-9)]+\..+/g.test($(e.target)[0].files[0].name)) {
            //     console.log($(e.target)[0].files[0].name)
            //     this.msg('请上传名字为数字与字母组合的文件');
            //     return false;
            // }
            $(e.target).parent().submit();
        },
        fileLoad: function(iframe, data, e) { //上传图片配套4/4
            var ele = $(window.frames[iframe].document.body);
            if(ele.text() == '') return false;
            var res = JSON.parse(ele.text());
            console.log(res);
            if(res.status == 1) {
                this.$set(this.img.obj,this.img.name.toString(),res.cont);
                // this.imgList.cont = res.cont;
            } else {
                this.msg(res.msg);
            }
            $(e.target).prev()[0].reset();
        },
        removeModuleList: function(item, index) { //模板中删除当前list
            console.log(item);
            item.splice(index, 1);
        },
        addModelList: function(item, type) { //模板中创建一条list
            console.log(item.cont);
            var listObj = {
                type: type, //类型
                cont: '', //内容
                align: 'left', //对齐方式
                size: '', 
                color: '#333',
                weight: 'false',
                italic: 'false',
                underline: 'false',
                lock: 'false', //是否锁定
            }
            item.cont.push(listObj);
        },
        delModel: function(obj, index) { //删除模块
            obj.splice(index, 1);
        },
        addModel: function(type) { //添加模块
            var that = this;
            var obj = {
                type: type, //类型
                bg: '', //背景
                // color: '#333', //字体颜色
                width: '92', //宽
                borderRadius: '0', //圆角
                marginTop: '25', //上间距(1 => 0.1em)
                size: '16', //字体大小(1 => 0.1em)
                opacity: '100', //不透明度(1 => 0.1)
                borderWidth: '0', //边框大小
                borderColor: '#fff', //边框颜色
                blur: '0', //模糊度(1 => 0.1)
                lock: 'false' //是否锁定
            };
            switch(type) {
                case 'DIYModules': //自定义模块
                    for(var i = this.currentPage.object.length-1; i >= 0; i--) {
                        if(this.currentPage.object[i].type == 'DIYModules') {
                            this.diyModelTemp = {
                                titleBg: this.currentPage.object[i].title.bg,
                                bg: this.currentPage.object[i].bg
                            };
                            break;
                        }
                    }
                    obj.title = { //标题
                        cont: '', //标题内容
                        left: '50', //位置left、center、right
                        color: '#f00', //颜色
                        show: "true", //是否显示标题
                        bg: this.diyModelTemp.titleBg, //背景
                        lock: 'false' //是否锁定
                    };
                    obj.bg = this.diyModelTemp.bg;
                    obj.cont = [];
                    break;
                case 'slide': //轮播图
                    obj.width = '100';
                    obj.marginTop = '0';
                    obj.cont = [];
                    console.log(obj);
                    break;
                case 'activityTime': //活动时间
                    obj.width = '100';
                    obj.marginTop = '10';
                    obj.startTime = (new Date().getTime()).toString();
                    obj.endTime = (new Date().getTime()+1000*60*60*24).toString();
                    obj.color = '#ff973b';
                    obj.size = '12';
                    obj.timesColor = '#f00';
                    // obj.height = '5';
                    obj.lineHeight = '1';
                    break;
                case 'image': //图片
                    obj.left = '50';
                    obj.top = '50';
                    // obj.cont = 'http://img.zcool.cn/community/01080755c1edaf32f87528a18e9840.jpg@900w_1l_2o_100sh.jpg';
                    obj.width = '50';
                    obj.marginTop = '0';
                    that.uploadImg(obj, 'cont');
                    break;
                case 'button': //按钮
                    obj.left = '50';
                    obj.top = '50';
                    obj.cont = '按钮';
                    obj.width = '50';
                    obj.marginTop = '0';
                    obj.color = '#fff'
                    obj.bg = '#ff973b';
                    break;
            }
            if(this.currentPage.object == null) {
                this.currentPage.object = new Array();
                // console.log(this.currentPage.object)
            }
            this.currentPage.object.push(obj)
            // console.log(this.currentPage)
        },
        createModuleB: function(type) {
            this.currentPage = {
                type: type,
                background: 'none',
                BGM: {
                    mid: '',
                    name: ''
                },
                object: [{
                    type: 'DIYModules',
                    bg: '',
                    marginTop: '20',
                    size: '15',
                    width: '100',
                    borderRadius: '0',
                    borderWidth: '0',
                    borderColor: '',
                    opacity: '100',
                    blur: '0',
                    lock: 'false',
                    title: {
                        cont: '',
                        left: '50',
                        color: '#f00',
                        bg: 'none',
                        show: 'false',
                        lock: 'false',
                    },
                    cont: [{
                        type: 'text',
                        cont: '这个可以是标题',
                        align: 'center',
                        size: 'l',
                        weight: 'true',
                        italic: 'false',
                        underline: 'false',
                        color: '#f00',
                        lock: 'false'
                    }]
                }]
            }
            this.createModuleShow = false;
        },
        selectItem: function(item, e) {
            // console.log(e.target);
            if(item.type != 'DIYModules') {
                this.sectionStyle = false;
                console.log(item);
            }
            $('.select').removeClass('select');
            $(e.target).parents('.page-item').addClass('select');
        },
        msg: function(string) {//提示信息
            $('.msg').addClass('msgShow');
            if(this.msgTimer) clearTimeout(this.msgTimer);
            this.msgInfo = string || 'error';
            this.msgTimer = setTimeout(function() {
                $('.msg').removeClass('msgShow');
            }, 3000);
        },
		getCustomers: function(a) {//获取数据
            var that = this;
            // console.log(that.currentPage)
            this.$http.get('./data.json?v='+Math.random())
            .then(function(res) {
                // that.pageData = res.data;
                // that.pageData.pages.sort(function(a,b) {
                //     return b.create_time - a.create_time;
                // })
                that.currentPage = res.data;

                // if(a) return false;
                // $.each(that.pageData.pages, function(i, obj) {
                //     // console.log(that)
                //     if(obj.id == that.currentId) {
                //         that.currentPage = obj;
                //         return false;
                //     }
                // })

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
        	l.alertVerify(str, {
        		sureTxt: "清空",
        		sure: function() {
        			// that.pageData.pages[that.editingPage].object = [];
        			// that.pageData.pages[that.editingPage].background = 'none';
                    that.currentPage.background = 'none';
                    that.currentPage.BGM = {
                        mid: '',
                        name: ''
                    };
                    that.currentPage.object = [];
        		}
        	})
        },
        saveFn: function() {//保存数据
            // this.currentPage.save_time = new Date().getTime().toString();
            // console.log(JSON.stringify(this.currentPage));
            // return false;
            // $.post('http://192.168.2.14/Api/Micromode/EditTemplate', {data: this.currentPage}, function(res) {
            //     var data = JSON.parse(res);
            //     console.log(data);
            // })
            console.log(this.currentPage);
        	$.post("./php/updataJSON.php",{obj:JSON.stringify(this.currentPage)},function(res){
                console.log(res);
        	});
            // console.log()
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
        // reduce: function(item, obj, num) {//数字减
        // 	switch(obj) {
        // 		case 'positionX':
        // 			var x = item[obj];
        // 			x = x - 1;
        // 			item[obj] = parseInt(x);
        // 			break;
        // 		case 'positionY':
        // 			var y = item[obj];
        // 			y = y - 1;
        // 			item[obj] = parseInt(y);
        // 			break;
        // 		case 'animateSeed':
        // 			if(item[obj] <= num) return false;
        // 			item[obj] = (item[obj]*1 - 0.1).toFixed(1);
        // 			break;
        // 		case 'delay':
        // 			if(item[obj] <= num) return false;
        // 			item[obj] = (item[obj]*1 - 0.1).toFixed(1);
        // 			break;
        //         default:
        //             if(item[obj] <= num) return false; 
        //             item[obj]--;
        // 	}
        // },
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
        // delPage: function(data, index, e) {//删除页面
        // 	e.preventDefault();
        // 	var that = this;
        // 	$('.rightMouseMask').hide();
        // 	var str = '是否删除该页?';
        // 	l.alertVerify(str, {
        // 		sureTxt: '删除',
        // 		sure: function() {
        // 			// 删除
        // 			data.pages.splice(index, 1);
        // 			// 删除后自动选择新的编辑页
        // 			if(index > that.editingPage) return false;
        // 			that.editingPage--;
        // 			if(that.editingPage <= 0) that.editingPage = 0;
        // 		}
        // 	})
        // },
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
        moveItem: function(index, dir, e) {//修改顺序dir: up | down
            var that = this;
            var n = 1,
                s = 1;
            var a,b;
            if(dir == 'up') {
                s=-1;
            } else {
                s=1
            }
            while(this.currentPage.object[index+n*s].type == 'image' || this.currentPage.object[index+n*s].type == 'button') {
                ++n;
                if(index+n*s < 0 || index+n*s > this.currentPage.object.length-1) {
                    that.msg('无法继续移动');
                    return false;
                }
            }
            b = this.currentPage.object[index];
            this.currentPage.object[index] = this.currentPage.object[index+n*s];
            this.currentPage.object[index+n*s] = b;
            a = this.currentPage.object;
            this.currentPage.object = ''; //vue数组只改变顺序页面不重新渲染
            this.currentPage.object = a;
            setTimeout(function() {
                $('.page-item').eq(index+n*s).addClass('select').siblings().removeClass('select');
            }, 100)

        },
        // moveDownItem: function(data, index) {//与页面序号后一个互换
        // 	data.pages[index] = [
        // 		data.pages[index+n],
        // 		data.pages[index+n] = data.pages[index]
        // 	][0]
        // 	data.pages[index].sequence = [
        // 		data.pages[index+n].sequence,
        // 		data.pages[index+n].sequence = data.pages[index].sequence
        // 	][0];
        // },
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
        createModuleP: function() {
            var that = this;
            l.alertVerify('创建新模板前确认当前模板已保存', {
                sure: function() {
                    that.createModuleShow = true;
                }
            })
        },
        selClassify: function(item) { //创建元素选择分类
            this.createModuleB(item.name);
        },
        showWidget: function() {
            if(this.widgetShow) {
                this.widgetShow = false;
            } else {
                this.widgetShow = true;
            }
        },
        getBase64Image: function(img) {
           var canvas = document.createElement("canvas");
           canvas.width = img.width;
           canvas.height = img.height;
           var ctx = canvas.getContext("2d");
           ctx.drawImage(img, 0, 0, img.width, img.height);
           var dataURL = canvas.toDataURL("image/png");
           return dataURL;
           // return dataURL.replace("data:image/png;base64,", "");
        }
	}
})