$(function() {
	var css = ['css/style1','css/reset','css/edit','dateselect/dev/css/mobiscroll.core-2.5.2','dateselect/dev/css/mobiscroll.animation-2.5.2','dateselect/dev/css/mobiscroll.android-ics-2.5.2'],
		script = ['js/my','dateselect/dev/js/mobiscroll.core-2.5.2', 'dateselect/dev/js/mobiscroll.core-2.5.2-zh', 'dateselect/dev/js/mobiscroll.datetime-2.5.1', 'dateselect/dev/js/mobiscroll.datetime-2.5.1-zh', 'dateselect/dev/js/mobiscroll.android-ics-2.5.2'];
	var string = 'absdefghijklmnopqrstuvwxyz';
	var version = Math.random().toFixed(7) + string[Math.floor(Math.random() * 25)] + string[Math.floor(Math.random() * 25)] + string[Math.floor(Math.random() * 25)] +  Math.floor(Math.random() * 1000) + '';
	for (var i = 0; i < css.length; i++) {
	    $('head').append('<link rel="stylesheet" type="text/css" href="../../assets/' + css[i] + '.css?v=' + version + '">')
	}
	for(var i = 0; i < script.length; i++) {
	    $('head').append('<script type="text/javascript" src="../../assets/' + script[i] + '.js?v=' + version + '"></script>')
	}
})

