/**
  * ajax简单的封装
  */
function ajax_request(url, deal) {
	var http_request = null;
	try {
		http_request = new ActiveXObject("Microsoft.XMLHTTP");
	}
	catch (e) {
		try {
			http_request = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			alert(e);
		}
	}
	if (!http_request) {
		return;
	}
	var arr = url.split("?");
	if (arr.length == 1) {
		url = url + "?timestamp=" + new Date().getTime();
	} else {
		url = url + "&timestamp=" + new Date().getTime();
	}
	http_request.open("get", url, "false");
              //setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=gbk');
	http_request.onreadystatechange = function () {
		if (4 == http_request.readyState) {
			if (200 == http_request.status) {
				deal(http_request.responseText);
			} else {
				if (500 == http_request.status) {
					alert("\u8fde\u63a5\u4e0d\u4e0a\u6570\u636e\u5e93\u6216\u7cfb\u7edf\u5185\u90e8\u53d1\u751f\u9519\u8bef\uff01");
				} else {
					if (400 == http_request.status) {
						alert("\u627e\u4e0d\u5230\u8def\u5f84");
					} else {
						alert(http_request.status);
					}
				}
			}
		}
	};
	http_request.setRequestHeader("If-Modifyed-Since", "0");
	http_request.send(null);
}
/**
* select 元素的param属性与option中有匹配，让其选中
*/
function load() {
	var sel = document.getElementsByTagName("select");
	if (sel) {
		for (var j = 0; j < sel.length; j++) {
			if (sel[j].param) {
				for (var i = 0; i < sel[j].options.length; i++) {
					if (sel[j].options[i].value == sel[j].param) {
						sel[j].options[i].selected = true;
					}
				}
			}
		}
	}
}
function $sel() {
	var sel = $(arguments[0]);
	for (var i = 0; i < sel.options.length; i++) {
		if (sel.options[i].selected == true) {
			return sel.options[i].value;
		}
	}
}
/**
 * basic js
 */
/**
 * get element by id
 */
function $() {
	var results = [];
	var element = null;
	for (var i = 0; i < arguments.length; i++) {
		element = arguments[i];
		if (typeof (element) == "string") {
			element = document.getElementById(element);
		}
		results.push(element);
	}
	return results.length < 2 ? results[0] : results;
}
/**
 * create element
 * @param elementType
 * @param className
 */
function $c() {
	if (arguments.length == 1) {
		return document.createElement(arguments[0]);
	} else {
		var ele = document.createElement(arguments[0]);
		ele.className = arguments[1];
		return ele;
	}
}
/**
 * get element value
 */
function $v() {
	return $(arguments[0]).value;
}
/**
 * get elements by tagName
 */
function $t() {
	return document.getElementsByTagName(arguments[0]);
}
function $n() {
	return document.getElementsByName(arguments[0]);
}
String.prototype.trim = function () {
	var reExtraSpace = /^\s+(.*?)\s+$/;
	return this.replace(reExtraSpace, "$1");
};
/*
 * window.showModalDialog
 */
function windowShow(s_Url, n_Width, n_Height) {
	var o_Ret, n_W_Width, n_W_Height, v_W_TOP, v_W_LEFT;
	if (n_Width <= 0) {
		n_W_Width = screen.availWidth;
		v_W_LEFT = "";
	} else {
		n_W_Width = n_Width;
		v_W_LEFT = "";
	}
	if (n_Height <= 0) {
		n_W_Height = screen.availHeight;
		v_W_TOP = "";
	} else {
		n_W_Height = n_Height;
		v_W_TOP = "";
	}
	if (s_Url) {
		var arr = s_Url.split("?");
		if (arr.length == 1) {
			s_Url = s_Url + "?timestamp=" + new Date().getTime();
		} else {
			s_Url = s_Url + "&timestamp=" + new Date().getTime();
		}
	}
	o_Ret = showModalDialog(s_Url, "newWindow", "dialogWidth:" + n_W_Width + "px;dialogHeight:" + n_W_Height + "px;" + v_W_LEFT + v_W_TOP + "center:yes;help:no;resizable:no;scroll:yes;status:no");
	return o_Ret;
}
/*
 * window.Open
 */
function windowOpen(url, width, height) {
 
	var w = 300;
	var h = 400;

	if (width) {
		w = width;
	}
	if (height) {
		h = height;
	}
	if (url) {
		var arr = url.split("?");
		if (arr.length == 1) {
			url = url + "?timestamp=" + new Date().getTime();
		} else {
			url = url + "&timestamp=" + new Date().getTime();
		}
	}
	
	window.open(url, "newwindow", "height=" + h + ",width=" + w + ",top=150,left=400,center=yes,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no");
}

function Menu_HoverStatic(item) {
	item.className = "ToolbarButtonHover";
}
function Menu_Unhover(item) {
	item.className = "ToolbarButton";
}
function getRequestParam(paraname) {
	var strURL = window.location.href;
	var returnValue = new Object();
	if (strURL.split("?").length > 0) {
		var strTemp = strURL.split("?")[1];
		var lstTemp = strTemp.split("&");
		for (var i = 0; i < lstTemp.length; i++) {
			var item = lstTemp[i];
			var itemName = item.split("=")[0];
			var itemValue = item.split("=")[1];
			if (itemName == paraname) {
				returnValue = itemValue;
				break;
			}
		}
	} else {
		returnValue = "";
	}
	return returnValue;
}
//给table奇数列td添加样式
function addBg(){
    var t = $('itable');
    if(t){
	    var cols;
	    for(var i=0;i<t.rows.length;i++){
	        cols = t.rows[i].getElementsByTagName('td');
	        if(cols){
	            for(var j=0;j<cols.length/2;j++){
	                cols[2*j].style.background = '#E7E7E7';
	                cols[2*j].style.color = 'black';
	            }
	        }
	    }
	}else{
	    alert('请为输入表格与显示表格加上id');
	}
}