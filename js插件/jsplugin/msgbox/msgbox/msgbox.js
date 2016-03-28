(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // CommonJS
        factory(require('jquery'));
    } else {
        // Browser globals
        factory();
    }
}(function () {
    /*        
        参数列表说明:                                                            
        text  :弹出对话框的内容,可以使用HTML代码,例如<font color='red'>删除么?</font>,如果直接带入函数,注意转义
        func  :弹出对话框点击确认后执行的函数,需要写全函数的引用,例如add(),如果直接带入函数,注意转义。
        Author:Msh
    */
    
    $.msgbox = {
        
    };

    $.extend($.msgbox, {
        //创建遮罩层
        create_mask : function () {
            var mask = document.createElement('div');
            mask.id = 'mask';
            mask.style.position = 'fixed';
            mask.style.left = '0px';
            mask.style.top = '0px';
            mask.style.width = window.screen.width + 'px';
            mask.style.height = window.screen.height + 'px';
            mask.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(style=4,opacity=25)';
            mask.style.opacity = 0.4;
            mask.style.background = 'black';
            mask.style.zIndex = 1000;
            document.body.appendChild(mask);
        },
        //创建弹出对话框
        create_msgbox : function (w,h,t) {
            var box = document.createElement('div');
            box.id = 'msgbox';
            box.style.position = 'fixed';
            box.style.width = w + 'px';
            box.style.height = h + 'px';
            box.innerHTML = t;
            box.style.top = (window.screen.height - h) / 2 + 'px';
            box.style.left = (window.screen.width -w) / 2 + 'px';
            box.style.zIndex = 1001;
            box.style.overflow = 'hidden';
            document.body.appendChild(box);
        },
        //取消弹出层
        remove_msg: function () {
            var mask = document.getElementById("mask");
            var msgbox = document.getElementById("msgbox");
            if (null == mask && null == msgbox) return;
            document.body.removeChild(mask);
            document.body.removeChild(msgbox);
        },
        //公共
        common_msg: function (title, icon, content, func) {
            //创建遮罩层
            this.create_mask();
            var temp = "<div style=\"width:300px;border: 2px solid #37B6D1;background-color: #fff; font-weight: bold;font-size: 12px;\" >"
                + "<div style=\"line-height:25px; padding:0px 5px;    background-color: #37B6D1;\">" + title + "</div>"
                + "<table  cellspacing=\"0\" border=\"0\"><tr><td style=\" padding:0px 0px 0px 20px; \"><img src=\"" + icon + "\" width=\"64\" height=\"64\"></td>"
                + "<td ><div style=\"background-color: #fff; font-weight: bold;font-size: 12px;padding:20px 0px ; text-align:left;\">" + content
                + "</div></td></tr></table>";
            if (func && typeof func == 'function') { //确认框
                temp += "<div style=\"text-align:center; padding:0px 0px 20px;background-color: #fff;\"><input type='button'  style=\"border:1px solid #CCC; background-color:#CCC; width:50px; height:25px;\" value='确定'id=\"msgconfirmb\"   onclick=\"$.msgbox.remove_msg();$.msgbox.confirm_event();\">";
                temp += "   <input type='button' style=\"border:1px solid #CCC; background-color:#CCC; width:50px; height:25px;\" value='取消'  id=\"msgcancelb\" onClick='$.msgbox.remove_msg();'>";
            } else {
                temp += "<div style=\"text-align:center; padding:0px 0px 20px;background-color: #fff;\"><input type='button'  style=\"border:1px solid #CCC; background-color:#CCC; width:50px; height:25px;\" value='确定'id=\"msgconfirmb\"   onclick=\"$.msgbox.remove_msg();\">";
            }
            temp += "</div></div>";
            //创建弹出对话框
            this.create_msgbox(400, 200, temp);
        },
        //成功
        success : function(content){

            this.common_msg('提示', 'msgbox/images/msgbox_true.png', content);
        },
        //错误
        error: function (content) {

            this.common_msg('提示', 'msgbox/images/msgbox_error.png', content);
        },
        //警告
        warn: function (content) {

            this.common_msg('提示', 'msgbox/images/msgbox_warn.png', content);
        },
        //确认框
        confirm: function (content,func) {
            this.confirm_event = func;
            this.common_msg('提示', 'msgbox/images/msgbox_warn.png', content, this.confirm_event);
        }
    });
}));