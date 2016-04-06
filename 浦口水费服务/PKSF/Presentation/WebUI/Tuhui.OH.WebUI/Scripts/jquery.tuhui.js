/*! jquery.tuhui.js v1.0.0 */
/*常量*/
(function () {
    $.extend({
        CONSTS: {
            keyCode: {
                ALT: 18,
                BACKSPACE: 8,
                CAPS_LOCK: 20,
                COMMA: 188,
                COMMAND: 91,
                COMMAND_LEFT: 91, // COMMAND
                COMMAND_RIGHT: 93,
                CONTROL: 17,
                DELETE: 46,
                DOWN: 40,
                END: 35,
                ENTER: 13,
                ESCAPE: 27,
                HOME: 36,
                INSERT: 45,
                LEFT: 37,
                MENU: 93, // COMMAND_RIGHT
                NUMPAD_ADD: 107,
                NUMPAD_DECIMAL: 110,
                NUMPAD_DIVIDE: 111,
                NUMPAD_ENTER: 108,
                NUMPAD_MULTIPLY: 106,
                NUMPAD_SUBTRACT: 109,
                PAGE_DOWN: 34,
                PAGE_UP: 33,
                PERIOD: 190,
                RIGHT: 39,
                SHIFT: 16,
                SPACE: 32,
                TAB: 9,
                UP: 38,
                WINDOWS: 91 // COMMAND
            },
            html: {
                message: $('<div class="th-ui-message"><div class="th-ui-message-icon"></div><div class="th-ui-message-text"></div><div style="clear:both;"></div></div>'),
                messageQ: $("<div class='lgh-ui-messageQ'><div class='img'></div><div class='text'></div>"),
                modal: $('<div class="th-ui-modal"></div>'),
                modal1: $('<div class="th-ui-modal1"></div>'),
                messageLoading: $('<div class="th-ui-message-loading"></div>'),
                messageReport: $('<div class="th-ui-message-report"><div class="th-ui-message-report-main"><div class="th-ui-message-report-title" title="提示:鼠标双击标题可以最小/大化切换！"><a class="tip" name="tip"></a><a class="title"></a><a class="max"></a><a class="min"></a><a class="close"></a></div><div class="th-ui-message-report-content"></div></div></div>'),
                picIframe: $('<div class="th-ui-pic-iframe"><div class="iframe-main"><div class="pic-title"></div><div class="pic-main"></div><div class="pic-desc"></div><div class="iframe-close"></div><div class="iframe-loading"></div></div></div>')
            },
            // 分页相关
            pager: {
                //            TOTAL_MESSAGE: "总共{0}条记录,{1}/{2}页",
                TOTAL_MESSAGE: "共{0}条记录,当前{1}/{2}页",
                TOTAL_MESSAGE_ZREO: "共0条记录",
                PARAMETER_ERROR: "分页参数有错误",
                FIRST_PAGE: "首页",
                PRE_PAGE: "上一页",
                LAST_PAGE: "末页",
                NEXT_PAGE: "下一页",
                PAGEINDEX_OVERFLOW: "页索引超出范围"
            },
            message: {
                Not_Found: "请求地址错误",
                Internal_Server_Error: "服务器内部错误"
            }
        }
    })
})(jQuery);

/*常用方法*/
(function () {
    var ua = navigator.userAgent.toLowerCase();
    var isStrict = document.compatMode == "CSS1Compat",
        isOpera = ua.indexOf("opera") > -1,
        isSafari = (/webkit|khtml/).test(ua),
        isSafari3 = isSafari && ua.indexOf('webkit/5') != -1,
        isIE = !isOpera && ua.indexOf("msie") > -1,
        isIE7 = !isOpera && ua.indexOf("msie 7") > -1,
        isGecko = !isSafari && ua.indexOf("gecko") > -1,
        isBorderBox = isIE && !isStrict,
        isWindows = (ua.indexOf("windows") != -1 || ua.indexOf("win32") != -1),
        isMac = (ua.indexOf("macintosh") != -1 || ua.indexOf("mac os x") != -1),
        isAir = (ua.indexOf("adobeair") != -1),
        isLinux = (ua.indexOf("linux") != -1),
        isSecure = window.location.href.toLowerCase().indexOf("https") === 0;

    var stripTagsRE = /<\/?[^>]+>/gi;
    var stripScriptsRe = /(?:<script.*?>)((\n|\r|.)*?)(?:<\/script>)/ig;

    $.extend({
        /** @type Boolean */
        isOpera: isOpera,
        /** @type Boolean */
        isSafari: isSafari,
        /** @type Boolean */
        isSafari3: isSafari3,
        /** @type Boolean */
        isSafari2: isSafari && !isSafari3,
        /** @type Boolean */
        isIE: isIE,
        /** @type Boolean */
        isIE6: isIE && !isIE7,
        /** @type Boolean */
        isIE7: isIE7,
        /** @type Boolean */
        isGecko: isGecko,
        /** @type Boolean */
        isBorderBox: isBorderBox,
        /** @type Boolean */
        isLinux: isLinux,
        /** @type Boolean */
        isWindows: isWindows,
        /** @type Boolean */
        isMac: isMac,
        /** @type Boolean */
        isAdobeAir: isAir,
        /** @将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式 */
        stringFormat: function (str, array) {
            if (array.length == 0) return null;
            for (var i = 0; i < array.length; i++) {
                var re = new RegExp('\\{' + (i) + '\\}', 'gm');
                str = str.replace(re, array[i]);
            }
            return str;
        },
        // 判断是否为空
        isEmpty: function (obj, allowBlank) {
            return obj === null || obj === undefined || (!allowBlank ? obj === '' : false);
        },
        // 判断是否是日期对象
        isDate: function (f) {
            return f && typeof f.getFullYear == 'function';
        },
        // 判断是否是数组
        isArray: function (obj) {
            return obj && typeof obj.pop == 'function';
        },
        floatFormat: function (src, pos) {
            return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
        },
        type: function (o) {
            if (o === undefined || o === null) {
                return false;
            }
            if (o.htmlElement) {
                return 'element';
            }
            var t = typeof o;
            if (t == 'object' && o.nodeName) {
                switch (o.nodeType) {
                    case 1: return 'element';
                    case 3: return (/\S/).test(o.nodeValue) ? 'textnode' : 'whitespace';
                }
            }
            if (t == 'object' || t == 'function') {
                switch (o.constructor) {
                    case Array: return 'array';
                    case RegExp: return 'regexp';
                }
                if (typeof o.length == 'number' && typeof o.item == 'function') {
                    return 'nodelist';
                }
            }
            return t;
        }, makeUrl: function (url, request) {
            return url + $.urlAndChar(url) + $.httpBuildQuery(request);
        },
        //添加&字符。
        urlAndChar: function (url) {
            /// <summary>判断URL字符中是否包含?字符，如果已经包含，则向URL结尾添加&字符；否则添加？字符。</summary>
            /// <param name="url" type="String">需要处理的URL地址。</param>
            return (/\?/.test(url) ? "&" : "?");
        },
        //URL编码。
        urlEncode: function (str) {
            /// <summary>将给定的字符串进行URL编码。</summary>
            /// <param name="str" type="String">需要进行URL编码的字符串。</param>
            /// <returns type="String" >编码后的字符。</returns>
            return encodeURIComponent(str);
        },
        //URL解码。
        urlDecode: function (str) {
            /// <summary>将给定的字符串进行URL解码。</summary>
            /// <param name="str" type="String">需要解码的字符串。</param>
            /// <returns type="String" >解码后的字符串。如果给定的字符串是null，则返回空白字符。</returns>
            if (str == undefined) return "";
            var decoded = decodeURIComponent(str);
            return decoded == "null" ? "" : decoded;
        },
        //创建URL查询字符串
        httpBuildQuery: function (obj) {
            /// <summary>根据给定对象的属性和属性值，创建URL查询字符串</summary>
            /// <param name="obj" type="Object">将根据该对象的属性创建URL查询字符串。</param>
            /// <returns type="String" />
            if (typeof obj != "object") return obj;
            var arr = [];
            for (var key in obj) {
                if ($.checkFunction(obj[key], false)) continue;
                arr.push($.urlEncode(key) + "=" + $.urlEncode(obj[key]));
            }
            return arr.join("&");
        },
        ///检查给定的对象是否是Funciton
        checkFunction: function (func, defaultFunc) {
            /// <summary>检查给定的对象是否是Function类型</summary>
            /// <param name="func" type="Function">需要判断的对象</param>
            /// <param name="defaultFunc" type="Function">默认的方法对象。如果func不是Function对象，则将使用该值。</param>
            var result = (typeof func == 'function');

            if (!result) {
                if (typeof defaultFunc == 'function') {
                    func = defaultFunc;
                }
            }
            return result;
        },
        urlSerialize: function (o) {
            if (!o) {
                return "";
            }
            var buf = [];
            for (var key in o) {
                var ov = o[key], k = encodeURIComponent(key);
                var type = typeof ov;
                if (type == 'undefined') {
                    //buf.push(k, "=&");
                } else if (type != "function" && type != "object") {
                    buf.push(k, "=", encodeURIComponent(ov), "&");
                } else if ($.isArray(ov)) {
                    if (ov.length) {
                        for (var i = 0, len = ov.length; i < len; i++) {
                            buf.push(k, "=", encodeURIComponent(ov[i] === undefined ? '' : ov[i]), "&");
                        }
                    } else {
                        buf.push(k, "=&");
                    }
                }
            }
            buf.pop();
            return buf.join("");
        },
        /**
        * Takes an encoded URL and and converts it to an object. e.g. $.urlDeserialize("foo=1&bar=2"); would return {foo: 1, bar: 2} or $.urlDeserialize("foo=1&bar=2&bar=3&bar=4", true); would return {foo: 1, bar: [2, 3, 4]}.
        * @param {String} string
        * @param {Boolean} overwrite (optional) Items of the same name will overwrite previous values instead of creating an an array (Defaults to false).
        * @return {Object} A literal with members
        */
        urlDeserialize: function (string, overwrite) {
            if (!string || !string.length) {
                return {};
            }
            var obj = {};
            var pairs = string.split('&');
            var pair, name, value;
            for (var i = 0, len = pairs.length; i < len; i++) {
                pair = pairs[i].split('=');
                name = decodeURIComponent(pair[0]);
                value = decodeURIComponent(pair[1]);
                if (overwrite !== true) {
                    if (typeof obj[name] == "undefined") {
                        obj[name] = value;
                    } else if (typeof obj[name] == "string") {
                        obj[name] = [obj[name]];
                        obj[name].push(value);
                    } else {
                        obj[name].push(value);
                    }
                } else {
                    obj[name] = value;
                }
            }
            return obj;
        },
        undef: function (value) {
            return value !== undefined ? value : "";
        },
        defaultValue: function (value, defaultValue) {
            return value !== undefined && value !== '' ? value : defaultValue;
        },
        htmlEncode: function (value) {
            return !value ? value : String(value).replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        },
        /**
        * Convert certain characters (&, <, >, and ') from their HTML character equivalents.
        * @param {String} value The string to decode
        * @return {String} The decoded text
        */
        htmlDecode: function (value) {
            return !value ? value : String(value).replace(/&amp;/g, "&").replace(/&gt;/g, ">").replace(/&lt;/g, "<").replace(/&quot;/g, '"');
        },
        // 截断字符串
        substr: function (value, start, length) {
            return String(value).substr(start, length);
        },
        // 小写形式
        lowercase: function (value) {
            return String(value).toLowerCase();
        },
        // 大写形式
        uppercase: function (value) {
            return String(value).toUpperCase();
        },
        // 第一个字母大写，后面都小写
        capitalize: function (value) {
            return !value ? value : value.charAt(0).toUpperCase() + value.substr(1).toLowerCase();
        },
        usMoney: function (v) {
            v = (Math.round((v - 0) * 100)) / 100;
            v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
            v = String(v);
            var ps = v.split('.');
            var whole = ps[0];
            var sub = ps[1] ? '.' + ps[1] : '.00';
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            v = whole + sub;
            if (v.charAt(0) == '-') {
                return '-$' + v.substr(1);
            }
            return "$" + v;
        },
        //超过长度省略显示
        ellipsis: function (value, len) {
            /// <summary>超过长度省略显示</summary>
            if (value && value.length > len) {
                return value.substr(0, len) + "...";
            }
            return value;
        },
        // 文件大小格式化
        fileSize: function (_size) {
            var size = parseFloat(_size);
            if (size < 1024) {
                return size + " bytes";
            } else if (size < 1048576) {
                return (Math.round(((size * 10) / 1024)) / 10) + " KB";
            } else {
                return (Math.round(((size * 10) / 1048576)) / 10) + " MB";
            }
        },
        date: function (v, format) {
            if (!v) {
                return "";
            }
            if (!$.isDate(v)) {
                v = new Date(Date.parse(v));
            }
            return v.dateFormat(format || "m/d/Y");
        },

        /**
        * Returns a date rendering function that can be reused to apply a date format multiple times efficiently
        * @param {String} format Any valid date format string
        * @return {Function} The date formatting function
        */
        dateRenderer: function (format) {
            return function (v) {
                return $.date(v, format);
            };
        },
        renderTime: function (data) {
            if (data == null) return data;
            var da = eval('new ' + data.replace('/', '', 'g').replace('/', '', 'g'));
            return da;
        },

        dateTimeFormat: function (time, format) {
            var o = {
                "M+": time.getMonth() + 1,  //month
                "d+": time.getDate(),     //day
                "h+": time.getHours(),    //hour
                "m+": time.getMinutes(),  //minute
                "s+": time.getSeconds(), //second
                "q+": Math.floor((time.getMonth() + 3) / 3),  //quarter
                "S": time.getMilliseconds() //millisecond
            }

            if (/(y+)/.test(format)) {
                format = format.replace(RegExp.$1, (time.getFullYear() + "").substr(4 - RegExp.$1.length));
            }

            for (var k in o) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
            return format;
        },
        /**
        * Strips all HTML tags
        * @param {Mixed} value The text from which to strip tags
        * @return {String} The stripped text
        */
        stripTags: function (v) {
            return !v ? v : String(v).replace(stripTagsRE, "");
        },

        /**
        * Strips all script tags
        * @param {Mixed} value The text from which to strip script tags
        * @return {String} The stripped text
        */
        stripScripts: function (v) {
            return !v ? v : String(v).replace(stripScriptsRe, "");
        },
        applyIf: function (o, c) {
            if (o && c) {
                for (var p in c) {
                    if (typeof o[p] == "undefined") { o[p] = c[p]; }
                }
            }
            return o;
        },
        isRepeat: function (arr) {
            if (!arr || arr.length == 0 || arr.length == 1) return false;
            var t = "";
            for (var i = 0; i < arr.length; i++) {
                if (i == 0) {
                    t = arr[i];
                } else {
                    if (arr[i] == t)
                        return true;
                    t = arr[i];
                }
            }
            return false;
        },
        padLeft: function (value, length, sChar) {
            if (value.length >= length) return value;
            var _valueLength = value.length;
            for (var i = 0; i < length - _valueLength; i++) {
                value = sChar + value;
            }
            return value;
        },
        padRight: function (value, length, sChar) {
            if (value.length >= length) return value;
            var _valueLength = value.length;
            for (var i = 0; i < length - _valueLength; i++) {
                value = value + sChar;
            }
            return value;
        },
        addFavorite: function (sURL, sTitle) {
            try {
                window.external.addFavorite(sURL, sTitle);
            }
            catch (e) {
                try {
                    window.sidebar.addPanel(sTitle, sURL, "");
                }
                catch (e) {
                    alert("加入收藏失败，请使用Ctrl+D进行添加");
                }
            }
        },
        setHome: function (c, a) {
            if (!$.isIE) {
                alert("您的浏览器不支持自动设置主页，请使用浏览器菜单手动设置。");
                return
            }
            var b = a;
            if (!b) {
                b = window.location.href
            }
            c.style.behavior = "url(#default#homepage)";
            c.setHomePage(b)
        },
        jsonString: function (obj) {
            var ransferCharForJavascript = function (s) {
                var newStr = s.replace(
                /[\x26\x27\x3C\x3E\x0D\x0A\x22\x5C\x00]/g,
                    function (c) {
                        ascii = c.charCodeAt(0)
                        return '\\u00' + (ascii < 16 ? '0' + ascii.toString(16) : ascii.toString(16))
                    }
                );
                return newStr;
            }
            if (obj == null) {
                return null
            }
            else if (obj.constructor == Array) {
                var builder = [];
                builder.push("[");
                for (var index in obj) {
                    if (typeof obj[index] == "function") continue;
                    if (index > 0) builder.push(",");
                    builder.push($.jsonString(obj[index]));
                }
                builder.push("]");
                return builder.join("");
            }
            else if (obj.constructor == Object) {
                var builder = [];
                builder.push("{");
                var index = 0;
                for (var key in obj) {
                    if (typeof obj[key] == "function") continue;
                    if (index > 0) builder.push(",");
                    builder.push($.stringFormat("\"{0}\":{1}", [key, $.jsonString(obj[key])]));
                    index++;
                }
                builder.push("}");
                return builder.join("");
            }
            else if (obj.constructor == Boolean) {
                return obj.toString();
            }
            else if (obj.constructor == Number) {
                return obj.toString();
            }
            else if (obj.constructor == String) {
                return $.stringFormat('"{0}"', [ransferCharForJavascript(obj)]);
            }
            else if (obj.constructor == Date) {
                return $.stringFormat('{"__DataType":"Date","_$thisue":{0}}', [obj.getTime() - (new Date(1970, 0, 1, 0, 0, 0)).getTime()]);
            }
            else if (this.toString != undefined) {
                return $.jsonString(obj);
            }
        }
    })

    $.extend({
        registerCheckBox: function (chkAllName, chkName) {
            if ($("input[type='checkbox'][name='" + chkAllName + "']").attr("checked")) {
                $("input[type='checkbox'][name='" + chkAllName + "']").attr("hasChecked", "1");
            } else {
                $("input[type='checkbox'][name='" + chkAllName + "']").attr("hasChecked", "0");
            }


            $("input[type='checkbox'][name='" + chkAllName + "']").on("click", function () {
                if ($(this).attr("hasChecked") == "1") {
                    $(this).removeAttr("checked");
                    $(this).attr("hasChecked", "0");
                    $("input[type='checkbox'][name='" + chkName + "']").removeAttr("checked");
                } else {
                    $(this).attr("checked", "checked");
                    $(this).attr("hasChecked", "1");
                    $("input[type='checkbox'][name='" + chkName + "']").attr("checked", "checked");
                }
            })
            $("input[type='checkbox'][name='" + chkName + "']").on("click", function () {
                var _chkObj = $("input[type='checkbox'][name='" + chkName + "']");
                if (_chkObj.size() == _chkObj.filter(":checked").size()) {
                    $("input[type='checkbox'][name='" + chkAllName + "']").attr("checked", "checked");
                    $("input[type='checkbox'][name='" + chkAllName + "']").attr("hasChecked", "1");
                } else {
                    $("input[type='checkbox'][name='" + chkAllName + "']").removeAttr("checked");
                    $("input[type='checkbox'][name='" + chkAllName + "']").attr("hasChecked", "0");
                }
            })
            $("input[type='checkbox'][name='" + chkAllName + "']").removeAttr("checked");
            $("input[type='checkbox'][name='" + chkName + "']").removeAttr("checked");
        },
        getCheckBoxData: function (chkName, attrName, type) {
            var _chkObj = $("input[type='checkbox'][name='" + chkName + "']");
            if (type == false) {
                _chkObj = _chkObj.not(":checked");
            } else {
                _chkObj = _chkObj.filter(":checked");
            }
            return _chkObj.map(function (element, index) {
                return $(this).attr(attrName ? attrName : "value");
            }).get();
        },
        TableEvenTrCss: function (table) {
            $(table).find("tbody").find("tr:odd").addClass("even");
        }
    })

    $.fn.extend({
        toggleClass: function (cla1, cla2) {
            if ($(this).hasClass(cla1)) {
                $(this).removeClass(cla1);
                if (cla2)
                    $(this).addClass(cla2);
            } else {
                $(this).addClass(cla1);
                if (cla2)
                    $(this).removeClass(cla2);
            }
        },
        textTip: function (type, value) {
            var obj = this;
            if (!type && !value) {
                $(this).data("textTip", $(this).val());
                setColorStyle(textTipValue());
                $(this).change(function () {
                    setColorStyle($(this).val());
                    if ($(this).val() == '') {
                        $(this).val(textTipValue());
                        //  $(obj).css("color", "#999");
                    }
                }).focus(function () {
                    if ($(this).val() == textTipValue()) {
                        $(this).val('');
                        //       $(obj).css("color", "#fff");
                    }
                }).blur(function () {
                    if ($(this).val() == '') {
                        $(this).val(textTipValue());
                        //      $(obj).css("color", "#999");
                    }
                })
            } else if (type == "value") {
                if (value) {
                    $(this).val(value);
                    setColorStyle(value);
                } else {
                    if ($(obj).val() == textTipValue()) return "";
                    return $(obj).val();
                }
            }

            function textTipValue() {
                return $(obj).data("textTip");
            }

            function setColorStyle(content) {
                if (content == textTipValue()) {
                    //      $(obj).css("color", "#999");
                } else {
                    //     $(obj).css("color", "#fff");
                }
            }

        },
        numberTextbox: function () {
            $(this).keydown(function (e) {
                if (e.keyCode == 46 || e.keyCode == 8) {
                    return true;
                } else if (e.keyCode < 48) {
                    return false;
                } else if (e.keyCode > 57 && e.keyCode < 96) {
                    return false;
                } else if (e.keyCode > 105) {
                    return false;
                } else {
                    if ($(this).attr("length")) {
                        if ($(this).val().length >= parseInt($(this).attr("length"))) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            })
        },
        textLimit: function (property) {
            if (!property || !property.length || !property.change) return;
            $(this).bind("keyup", function () {
                _event(this);
            })

            if ($(this).val() != "") {

                property.change(_value_length($(this).val()), property.length - _value_length($(this).val()));
            } else {
                property.change(0, property.length);
            }

            function _event(obj) {
                var _value = $(obj).val();
                if (_value_length(_value) > property.length) {
                    property.change(property.length, 0);
                    if (property.overflow) {
                        property.overflow(_value_length(_value) - property.length);
                    }
                    if (property.cutOverFlow) {
                        var _valueItem = $(obj).val();
                        while (_value_length(_valueItem) > property.length) {
                            _valueItem = _valueItem.substring(0, _valueItem.length - 1);
                        }
                        $(obj).val(_valueItem);
                    } else {
                        $(obj).attr("isoverflow", "1");
                    }
                } else {
                    $(obj).removeAttr("isoverflow");
                    property.change(_value_length(_value), property.length - _value_length(_value));
                }
            }

            function _value_length(value) {
                //                if(property.chinese){
                //                    var ascRegexp = /[^\x00-\xFF]/g;
                //                    return value.replace(ascRegexp, '..').length;
                //                }else{
                if (!value) {
                    return 0;
                } else {
                    if (value.indexOf("\n") == -1) {
                        return value.length;
                    } else {
                        var _itemValue = value;
                        while (_itemValue.indexOf("\n") != -1) {
                            _itemValue = _itemValue.replace("\n", "**")
                        }
                        return _itemValue.length;
                    }
                }
                //                }
            }
        },
        enterKey: function (event) {
            $(this).keydown(function (e) {
                if (e.keyCode == $.CONSTS.keyCode.ENTER) {
                    event();
                }
            })
        },
        getAttr: function (attr, fun) {
            if (!attr) return;
            if (!fun) {
                return $(this).map(function (a) { return $(this).attr(attr); }).get();
            } else {
                return $.grep($(this).map(function () { return $(this).attr(attr); }).get(), fun);
            }
        },
        screenCenter: function (container) {
            if (!container) {
                $(this).css({
                    left: (document.documentElement.clientWidth - $(this).width()) / 2 + 'px',
                    top: (document.documentElement.clientHeight - $(this).height()) / 2 + 'px'
                });
            } else {
                $(this).css({
                    left: (container.width() - $(this).width()) / 2 + 'px',
                    top: (container.height() - $(this).height()) / 2 + 'px'
                });
            }
            return $(this);
        }
    })
})(jQuery);

/*消息显示*/
(function () {
    var $msgTimeId;
    var $msgReportTimeId;
    var _iframeDialogGlobal = {};
    $.extend({
        messageInit: function () {
            $.CONSTS.html.message.appendTo("body");
            $.CONSTS.html.messageQ.appendTo("body");
            $.CONSTS.html.modal.appendTo("body");
            $.CONSTS.html.modal1.appendTo("body");
            $.CONSTS.html.messageReport.appendTo("body");
            $.CONSTS.html.picIframe.appendTo("body");

            $.CONSTS.html.messageReport.find("a.close").click(function () {
                $.CONSTS.html.messageReport.animate({ bottom: "-" + $.CONSTS.html.messageReport.height() + "px" }, 1000, function () {
                    $.CONSTS.html.messageReport.hide();
                    if ($msgReportTimeId) {
                        clearInterval($msgReportTimeId);
                        $.CONSTS.html.messageReport.find("a[name='tip']").show();
                    }
                });
            })

            $.CONSTS.html.messageReport.find("a.max").hide();

            $.CONSTS.html.messageReport.find("a.min").click(function () {
                $.CONSTS.html.messageReport.animate({ bottom: "-" + ($.CONSTS.html.messageReport.height() - 30) + "px" }, 500);
                $.CONSTS.html.messageReport.find("a.min").hide();
                $.CONSTS.html.messageReport.find("a.max").show();
            })
            $.CONSTS.html.messageReport.find("a.max").click(function () {
                $.CONSTS.html.messageReport.animate({ bottom: "0px" }, 500);
                $.CONSTS.html.messageReport.find("a.max").hide();
                $.CONSTS.html.messageReport.find("a.min").show();
                if ($msgReportTimeId) {
                    clearInterval($msgReportTimeId);
                    $.CONSTS.html.messageReport.find("a[name='tip']").show();
                }
            })
            $.CONSTS.html.messageReport.find(".th-ui-message-report-title").dblclick(function () {
                if ($.CONSTS.html.messageReport.find("a.min").is(":visible")) {
                    $.CONSTS.html.messageReport.find("a.min").click();
                } else {
                    $.CONSTS.html.messageReport.find("a.max").click();
                }
            })
        },
        showModal: function (index) {
            if (!index) index = "";
            $.CONSTS.html["modal" + index].show();
        },
        closeModal: function (index) {
            if (!index) index = "";
            $.CONSTS.html["modal" + index].hide();
        },
        showMessage: function (property) {
            if (!property) return;
            if (property == "close") {
                _MessageDialogClose(true);
                return;
            }
            var _setting = {
                message: '',
                icon: "success",
                fromQQ: false,
                containerId: null,
                modal: true,
                delay: 1000,
                callback: {
                    beforeOpen: function () { },
                    afterClose: function () { },
                    cancle: function () { } // confirm的取消按钮触发时间
                }
            };
            $.extend(_setting, property);

            if (_setting.fromQQ) {
                if (_setting.icon == "success") {
                    _setting.delay = 1000;
                } else if (_setting.icon == "promt" || _setting.icon == "warning" || _setting.icon == "error") {
                    _setting.delay = 2000;
                } else {
                    _setting.delay = 3000000;
                }
            }

            if (typeof (property.beforeOpen) == "function") {
                _setting.callback.beforeOpen = property.beforeOpen;
            }

            if (typeof (property.afterClose) == "function") {
                _setting.callback.afterClose = property.afterClose;
            }

            if (typeof (property.cancle) == "function") {
                _setting.callback.cancle = property.cancle;
            }

            if ($msgTimeId) clearTimeout($msgTimeId);

            _SetMessageText();

            _SetMessageIcon();

            _DefineMessageDialog();

            _SetDialogButton();

            _MessageDialogOpen();

            function _SetMessageText() {
                if (!_setting.fromQQ) {
                    $.CONSTS.html.message.find('.th-ui-message-text').html(_setting.message);
                } else {
                    $.CONSTS.html.messageQ.find('div.text').html(_setting.message);
                }
            }

            function _SetMessageIcon() {
                if (!_setting.fromQQ) {
                    $.CONSTS.html.message.find('.th-ui-message-icon').removeClass("th-ui-message-icon-success")
                        .removeClass("th-ui-message-icon-warning")
                        .removeClass("th-ui-message-icon-error")
                        .removeClass("th-ui-message-icon-loading")
                        .removeClass("th-ui-message-icon-confirm")
                        .addClass("th-ui-message-icon-" + _setting.icon);
                } else {
                    $.CONSTS.html.messageQ.find('.img')
                    .removeClass('success')
                    .removeClass('warning')
                    .removeClass('prompt')
                    .removeClass('error')
                    .removeClass('loading')
                    .addClass(_setting.icon);
                }
            }

            function _DefineMessageDialog() {
                if (!_setting.fromQQ) {
                    $.CONSTS.html.message.dialog({
                        autoOpen: false,
                        title: "提示",
                        draggable: false,
                        resizable: false,
                        modal: _setting.modal,
                        minWidth: 420,
                        maxWidth: 1000,
                        close: function () {
                            if ($msgTimeId) clearTimeout($msgTimeId);

                            if (_setting.icon == "success") {
                                _setting.callback.afterClose();
                            }

                        }
                    });
                } else {
                    if (_setting.containerId) {
                        $.CONSTS.html.messageQ.css({
                            left: (($("#" + _setting.containerId).width() - $.CONSTS.html.messageQ.width()) / 2) + $("#" + _setting.containerId).offset().left + 'px',
                            top: (($("#" + _setting.containerId).height() - $.CONSTS.html.messageQ.height()) / 2) + $("#" + _setting.containerId).offset().top + 'px'
                        })
                    } else {
                        $.CONSTS.html.messageQ.css({
                            left: (document.documentElement.clientWidth - $.CONSTS.html.messageQ.width()) / 2 + 'px',
                            top: (document.documentElement.clientHeight - $.CONSTS.html.messageQ.height()) / 2 + 'px'
                        })
                    }
                }
            }

            function _SetDialogButton() {
                if (_setting.fromQQ) return;
                var type = _setting.icon;
                if (type == "warning" || type == "error") {
                    $.CONSTS.html.message.dialog("option", "buttons", {
                        "关 闭": function () {
                            _MessageDialogClose();
                            _setting.callback.afterClose();
                        }
                    });
                } else if (type == "confirm") {
                    $.CONSTS.html.message.dialog("option", "buttons", {
                        "确 定": function () {
                            _MessageDialogClose();
                            _setting.callback.afterClose();
                        }, "取 消": function () {
                            _MessageDialogClose();
                            _setting.callback.cancle();
                        }
                    });
                } else if (type == "success") {
                    $.CONSTS.html.message.dialog("option", "buttons", {
                        "确 定": function () {
                            _MessageDialogClose();
                            _setting.callback.afterClose();
                        }
                    });
                } else {
                    $.CONSTS.html.message.dialog("option", "buttons", {});
                }
            }

            function _MessageDialogOpen() {
                if (!_setting.fromQQ) {
                    $.CONSTS.html.message.dialog('open');
                    $.CONSTS.html.message.find('.th-ui-message-text').focus();
                    //                if(_setting.icon=="success"){
                    //                    msgTimeId = setTimeout(_MessageDialogClose,_setting.delay);
                    //                }

                    $("a.ui-dialog-titlebar-close").hide();
                } else {
                    $.CONSTS.html.modal.show();
                    $.CONSTS.html.messageQ.show();
                    msgTimeId = setTimeout(_MessageDialogClose, _setting.delay);
                }
            }

            function _MessageDialogClose(all) {
                if (!_setting.fromQQ || all) {
                    $.CONSTS.html.message.dialog('close');
                    $("a.ui-dialog-titlebar-close").show();
                }
                if (_setting.fromQQ || all) {
                    $.CONSTS.html.modal.hide();
                    $.CONSTS.html.messageQ.hide();
                    _setting.callback.afterClose();
                }
            }
        },
        showMessageQ: function (property) {
            property.fromQQ = true;
            $.showMessage(property);
        },
        messageSuccess: function (message, callback) {
            $.showMessage({ message: message, afterClose: callback });
        },
        messageSuccessQ: function (message, callback, containerId) {
            $.showMessageQ({ message: message, afterClose: callback, containerId: containerId });
        },
        messageSuccessX: function (message, callback) {
            $.iframeDialog({ message: { message: message, icon: 'success' }, callback: callback });
        },
        messageWarning: function (message, callback) {
            $.showMessage({ message: message, icon: "warning", afterClose: callback });
        },
        messageWarningQ: function (message, callback, containerId) {
            $.showMessageQ({ message: message, icon: "warning", afterClose: callback, containerId: containerId });
        },
        messageWarningX: function (message, callback) {
            $.iframeDialog({ message: { message: message, icon: 'warning' }, callback: callback });
        },
        messageError: function (message, callback) {
            $.showMessage({ message: message, icon: "error", afterClose: callback });
        },
        messageErrorQ: function (message, callback, containerId) {
            $.showMessageQ({ message: message, icon: "error", afterClose: callback, containerId: containerId });
        },
        messageErrorX: function (message, callback) {
            $.iframeDialog({ message: { message: message, icon: 'error' }, callback: callback });
        },
        messageConfirm: function (message, callback, cancleCallback) {
            $.showMessage({ message: message, icon: "confirm", afterClose: callback, cancle: cancleCallback });
        },
        messageConfirmX: function (message, callback, cancleCallback) {
            $.iframeDialog({ message: { message: message, icon: 'confirm' }, callback: callback });
        },
        messageLoading: function (message) {
            if (message) {
                $.showMessage({ message: message, icon: "loading" });
            } else {
                $.showMessage('close');
            }
        },
        messageLoadingQ: function (message) {
            if (message) {
                $.showMessageQ({ message: message, icon: "loading" });
            } else {
                $.showMessage('close');
            }
        },
        messageLoadingX: function (message) {
            $.iframeDialog({ message: { message: message, icon: 'loading' } });
        },
        loadingIcon: function (containerId) {
            if (containerId == "close") {
                $.CONSTS.html.modal.hide();
                $.CONSTS.html.messageLoading.hide();
                return;
            }
            if (!containerId) {
                containerId = "body";

                $.CONSTS.html.messageLoading.appendTo(containerId);
                $.CONSTS.html.modal.appendTo(containerId);
                $.CONSTS.html.messageLoading.css("position", "fixed");

                $.CONSTS.html.messageLoading.css({
                    left: (document.documentElement.clientWidth - $.CONSTS.html.messageLoading.width()) / 2 + 'px',
                    top: (document.documentElement.clientHeight - $.CONSTS.html.messageLoading.height()) / 2 + 'px'
                });
                $.CONSTS.html.modal.show();
                $.CONSTS.html.messageLoading.show();
            } else {
                containerId = "#" + containerId;

                $.CONSTS.html.messageLoading.appendTo("body");
                //                $(containerId).css("position","relative");
                $.CONSTS.html.messageLoading.css("position", "absolute");
                $.CONSTS.html.messageLoading.css({
                    left: ($(containerId).offset().left + ($(containerId).width() - $.CONSTS.html.messageLoading.width()) / 2) + 'px',
                    top: ($(containerId).offset().top + ($(containerId).height() - $.CONSTS.html.messageLoading.height()) / 2) + 'px'
                });
                $.CONSTS.html.messageLoading.show();
            }
        },
        iframeDialogInit: function (property) {
            _iframeDialogGlobal.iframeContainer = property.iframeContainer;
        },
        iframeDialog: function (property) {
            var obj = {};
            var _setting = {
                id: Math.round(Math.random() * 1000),
                iframeContainer: _iframeDialogGlobal.iframeContainer,
                title: '',
                width: 500,
                height: 300,
                modal: true,
                left: 0,
                top: 0,
                content: '',
                url: '',
                hideClose: true,
                message: null,
                buttons: null,
                callback: function () { }
            };
            $.extend(_setting, property);
            if (_setting.message) {
                _setting.id = "Message";
            }
            $.extend(_setting, {
                iframeId: "iframeDialog_" + _setting.id,
                iframeMainId: "iframeMain_" + _setting.id
            });

            if ($("#" + _setting.iframeId).size() == 0) {
                $("body").append('<iframe id="' + _setting.iframeId + '" src="JavaScript:false;" class="iframeDialog" frameborder="0" scrolling="auto">')
                $("body").append('<div id="' + _setting.iframeMainId + '" style="display:none;"></div>')
            }

            if (_dealMessage()) return;
            _defineDialog();

            $.extend(obj, {
                isOpen: function () { return $("#" + _setting.iframeMainId).dialog("isOpen") },
                setContent: _setContent,
                close: function () {
                    $("#" + _setting.iframeMainId).dialog("close")
                },
                open: function () {
                    $("#" + _setting.iframeMainId).dialog("open")
                }
            });
            return obj;

            function _dealMessage() {
                if (_setting.message) {
                    _setting.width = 428;
                    _setting.height = 158;
                    if (_setting.message.icon == "loading") {
                        _setting.height = 150;
                    }
                    $("#" + _setting.iframeId).css({
                        width: _setting.width + 'px',
                        height: _setting.height + 'px',
                        left: (document.documentElement.clientWidth - _setting.width) / 2 + 'px',
                        top: (document.documentElement.clientHeight - _setting.height) / 2 + 'px'
                    })
                    $("#" + _setting.iframeId).show();
                    $.showMessage({
                        message: _setting.message.message,
                        icon: _setting.message.icon,
                        afterClose: function () {
                            $("#" + _setting.iframeId).hide();
                            _setting.callback();
                        },
                        cancle: function () {
                            $("#" + _setting.iframeId).hide();
                        }
                    });
                    return true;
                }
                return false;
            }

            function _setHeightWidth() {
                var _dialogObj = $("div.ui-dialog").filter("[aria-labelledby='ui-dialog-title-" + _setting.iframeMainId + "']");
                if (_setting.hideClose) {
                    _dialogObj.find("a.ui-dialog-titlebar-close").hide();
                }
                if (_setting.left) {
                    _dialogObj.css({ left: _setting.left + "px" });
                }
                if (_setting.top) {
                    _dialogObj.css({ top: _setting.top + "px" });
                }

                $("#" + _setting.iframeId).css({
                    width: _dialogObj.width() + 8,
                    height: _dialogObj.height() + 8,
                    left: _dialogObj.offset().left,
                    top: _dialogObj.offset().top
                })
            }

            function _defineDialog() {
                $("#" + _setting.iframeMainId).dialog({
                    title: _setting.title,
                    autoOpen: true,
                    draggable: false,
                    resizable: false,
                    modal: _setting.modal,
                    height: _setting.height,
                    width: _setting.width,
                    open: function () {
                        if (_setting.modal) {
                            $.showModal();
                        }
                        _setHeightWidth();
                        $("#" + _setting.iframeId).show();
                        _setContent();
                    },
                    close: function () {
                        $.closeModal();
                        $("#" + _setting.iframeId).hide();
                    },
                    buttons: _setting.buttons
                })
            }

            function _setContent(value) {
                if (_setting.content) {
                    $("#" + _setting.iframeMainId).html(value ? value : _setting.content);
                } else {
                    $.loadingIcon(_setting.iframeMainId);
                    $("#" + _setting.iframeMainId).html('');
                    $.ajax({
                        url: value ? value : _setting.url,
                        //                        dataType:'html',
                        error: function () {
                            $.loadingIcon('close');
                            $.messageError("加载IframeDialog【" + _setting.url + "】发生错误");
                        },
                        success: function (response) {
                            $.loadingIcon('close');
                            $.ajaxResultHandle(response, function (_html) {
                                $("#" + _setting.iframeMainId).html(_html);
                            })
                        }
                    })
                }
            }
        },
        iframeDialogDeal: function (id) {
            var _config = window.parent.window[id].setting;
            ParentDialog = window.parent.window[id];
            ParentWindow = window.parent.window;

            ParentDialog.setContent = _dealContent;

            if (_dealMessage()) return;
            _dealIframeDialogInit();

            _dealContent();

            function _dealIframeDialogInit() {
                $(".iframeDialog-content").dialog({
                    autoOpen: true,
                    draggable: false,
                    resizable: false,
                    top: 0,
                    title: _config.title,
                    width: _config.width,
                    height: _config.height,
                    close: function () {
                        $("a.ui-dialog-titlebar-close").show();
                    },
                    buttons: _config.buttons
                })
                $("a.ui-dialog-titlebar-close").hide();
            }

            function _dealMessage() {
                if (_config.message) {
                    $.showMessage({
                        message: _config.message.message,
                        icon: _config.message.icon,
                        modal: false,
                        beforeOpen: function () { },
                        afterClose: function () {
                            ParentDialog.close();
                        },
                        cancle: function () {
                            ParentDialog.close(true);
                        }
                    });
                    return true;
                }
                return false;
            }

            function _dealContent(value) {
                if (_config.content) {
                    $(".iframeDialog-content").html(value ? value : _config.content);
                } else {
                    $(".iframeDialog-loading").css({
                        left: (_config.width - $(".iframeDialog-loading").width()) / 2 + "px",
                        top: (_config.height - $(".iframeDialog-loading").height()) / 2 + "px"
                    })
                    $(".iframeDialog-loading").show();
                    _dealContentAjax(value);
                }
            }

            function _dealContentAjax(value) {
                $.ajax({
                    type: 'GET',
                    dataType: 'html',
                    url: value ? value : _config.url,
                    cache: false,
                    error: function () {
                        $(".iframeDialog-loading").hide();
                        alert("iframeDialog加载出错");
                    },
                    success: function (response) {
                        $(".iframeDialog-loading").hide();
                        $(".iframeDialog-content").html(response);
                    }
                })
            }

        },
        messageReport: function (property) {
            if (property == "close") {
                $.CONSTS.html.messageReport.find("a.close").click();
                return;
            }
            $.CONSTS.html.messageReport.find("a.title").text(property.title);
            $.CONSTS.html.messageReport.find(".th-ui-message-report-content").html(property.content);
            if (property.open) {
                $.CONSTS.html.messageReport.css({
                    bottom: "-" + property.height + "px",
                    width: property.width + "px",
                    height: property.height + "px"
                })
                $.CONSTS.html.messageReport.find("a.max").hide();
                $.CONSTS.html.messageReport.find("a.min").show();
                $.CONSTS.html.messageReport.find("a.title").text(property.title);
                $.CONSTS.html.messageReport.show();
                $.CONSTS.html.messageReport.animate({ bottom: "0px" }, 1000);
            } else {
                if ($.CONSTS.html.messageReport.is(":hidden")) {
                    $.CONSTS.html.messageReport.css({
                        bottom: "-" + property.height + "px",
                        width: property.width + "px",
                        height: property.height + "px"
                    })
                    $.CONSTS.html.messageReport.find("a.min").hide();
                    $.CONSTS.html.messageReport.find("a.max").show();
                    $.CONSTS.html.messageReport.show();
                    $.CONSTS.html.messageReport.animate({ bottom: "-" + (property.height - 30) + "px" }, 100);
                }
                if (property.tip == true) {
                    $msgReportTimeId = setInterval(messageReportTip, 200);
                }
            }

            function messageReportTip() {
                $.CONSTS.html.messageReport.find("a[name='tip']").toggle();
            }

        }
    });
    $.extend({
        Validate_Promt_Init: function () {
            var $validate_propmt = $("<span class='field-prompt'></span>");
            $validate_propmt.appendTo("body");
            $validate_propmt.hide();
            $("input[prompt],textarea[prompt]").on("click", function () {
                $validate_propmt.css({
                    left: ($(this).offset().left + 15 + $(this).width()) + "px",
                    top: $(this).offset().top + "px"
                });
                $validate_propmt.html($(this).attr("prompt"));
                $(this).addClass("input-prompt");
                $validate_propmt.show();
            }).on("blur", function () {
                $(this).removeClass("input-prompt");
                $validate_propmt.hide();
            })
        },
        Input_Trim_Init: function () {
            $("input[trim='1']").on("blur", function () {
                $(this).val($.trim($(this).val()));
            })
            $("textarea[trim='1']").on("blur", function () {
                $(this).val($.trim($(this).val()));
            })
        },
        Input_Number_Init: function () {
            $("input[number='number']").on("keydown", function (e) {
                if (e.keyCode == 46 || e.keyCode == 8) {
                    return true;
                } else if (e.keyCode < 48) {
                    return false;
                } else if (e.keyCode > 57 && e.keyCode < 96) {
                    return false;
                } else if (e.keyCode > 105) {
                    return false;
                } else {
                    if ($(this).attr("length")) {
                        if ($(this).val().length >= parseInt($(this).attr("length"))) {
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            })
        }
    })
})(jQuery);

/*分页显示*/
(function () {
    $.extend({
        createPageData: function (array, pageIndex, pageSize) {
            if (!array) return null;
            if (!pageIndex) pageIndex = 1;
            var result = {
                PageIndex: pageIndex,
                PageSize: pageSize,
                TotalItemCount: 0,
                PageData: null
            };
            if (!array) return null;
            result.TotalItemCount = array.length;
            result.PageData = $.grep(array, function (a, i) {
                return (i + 1) > ((pageIndex - 1) * pageSize) && (i + 1) <= (pageIndex * pageSize);
            })
            return result;
        }
    })
    $.fn.extend({
        formSubmit: function (pageIndex) {
            if ($(this).find("input[type='hidden'][name='pageIndex']").size() == 0) {
                $(this).append("<input type='hidden' id='pageIndex' name='pageIndex'/>");
            }
            if ($(this).find("input[type='hidden'][name='pageIndex']")) {
                if (pageIndex) {
                    $(this).find("input[type='hidden'][name='pageIndex']").val(pageIndex);
                } else {
                    $(this).find("input[type='hidden'][name='pageIndex']").val('');
                }
                $(this).submit();
            }
        },
        createPager: function (property) {
            var pageHtml = "";
            var pageUrl = "";

            // 是否需要前面显示...
            var preEtc = false;
            // 是否需要后面显示...
            var nextEtc = false;
            // 是否需要前面显示上一页
            var prePage = true;
            // 是否需要后面显示后一页
            var nextPage = true;

            var TOTAL_MESSAGE = "<span class='total'>" + $.CONSTS.pager.TOTAL_MESSAGE + "</span>";
            var TOTAL_MESSAGE_ZREO = "<span class='total'>" + $.CONSTS.pager.TOTAL_MESSAGE_ZREO + "</span>";
            var A_LINK_FORMAT = "<a href='{0}'>{1}</a>";
            var A_LINK_CURRENT_FORMAT = "<a href='JavaScript:;' class='sel'>{1}</a>";
            var A_LINK_DISABLED_FORMAT = "<a disabled='disabled'>{0}</a>";

            var PARAMETER_ERROR = $.CONSTS.pager.PARAMETER_ERROR;
            var FIRST_PAGE = $.CONSTS.pager.FIRST_PAGE;
            var PRE_PAGE = $.CONSTS.pager.PRE_PAGE;
            var LAST_PAGE = $.CONSTS.pager.LAST_PAGE;
            var NEXT_PAGE = $.CONSTS.pager.NEXT_PAGE;
            var PAGEINDEX_OVERFLOW = $.CONSTS.pager.PAGEINDEX_OVERFLOW;

            // 默认分页属性
            var PagerProperty = {
                pageIndex: 1,
                pageSize: 10,
                total: null,
                pageNum: 0,
                pageCount: 8,
                formId: null,
                needButton: false,
                linkClick: null,
                linkFun: null,
                numberVisible: true
            };

            $.extend(PagerProperty, property);

            $(this).html('');
            if ($(this).attr("numberVisible") == "false") {
                PagerProperty.numberVisible = false;
            }
            if ($(this).attr("pageCount")) {
                PagerProperty.pageCount = parseInt($(this).attr("pageCount"));
            }
            if (!property || (!property.total && property.total != 0)) {
                $.messageWarning(PARAMETER_ERROR);
                return;
            }

            // 总共的页数
            PagerProperty.pageNum = PagerProperty.total == 0 ? 0 : (Math.ceil(PagerProperty.total / PagerProperty.pageSize));

            //            PagerProperty.pageCount = PagerProperty.pageCount > PagerProperty.pageNum ? PagerProperty.pageNum : PagerProperty.pageCount;

            if (PagerProperty.pageNum == 0 || PagerProperty.pageCount == 0 || PagerProperty.pageNum == 1) {
                if ($(this).attr('container')) {
                    $("#" + $(this).attr('container')).hide();
                } else {
                    $(this).hide();
                }
                //$(this).html(TOTAL_MESSAGE_ZREO);
                return;
            } else {
                if ($(this).attr('container')) {
                    $("#" + $(this).attr('container')).show();
                } else {
                    $(this).show();
                }
            }

            $(this).addClass("pager");
            $(this).attr("PageIndex", PagerProperty.pageIndex);
            //            if ($(this).attr('showTotal')) {
            pageHtml = $.stringFormat($(this).attr("totalFormat") ? ("<span class='total'>" + $(this).attr("totalFormat") + "</span>") : TOTAL_MESSAGE, new Array(PagerProperty.total, PagerProperty.pageIndex, PagerProperty.pageNum));
            //            }

            if (PagerProperty.pageIndex != 1) {
                prePage = true;
            }
            if (PagerProperty.pageIndex != PagerProperty.pageNum) {
                nextPage = true;
            }

            var arr = createPageNumbers(PagerProperty.pageIndex, PagerProperty.pageNum, PagerProperty.pageCount);

            pageHtml += "<ul>";

            if (!PagerProperty.numberVisible) {
                if (PagerProperty.pageIndex != 1) {
                    pageHtml += "<li>" + $.stringFormat(A_LINK_FORMAT, createLinkHtml(1, FIRST_PAGE)) + "</li>";
                }
            }

            if (prePage == true && PagerProperty.pageIndex > 1) {
                pageHtml += "<li>" + $.stringFormat(A_LINK_FORMAT, createLinkHtml(PagerProperty.pageIndex - 1, PRE_PAGE)) + "</li>";
            }
            if (PagerProperty.numberVisible) {
                pageHtml += makeALinkHtml(PagerProperty.pageIndex, 1);

                if (preEtc) {
                    pageHtml += "<li><span class='etc'>...</span></li>"
                }

                for (var i = 0; i < arr.length; i++) {
                    pageHtml += makeALinkHtml(PagerProperty.pageIndex, arr[i]);
                }
                if (nextEtc) {
                    pageHtml += "<li><span class='etc'>...</span></li>"
                }
                pageHtml += makeALinkHtml(PagerProperty.pageIndex, PagerProperty.pageNum);
            }
            if (nextPage == true && PagerProperty.pageIndex < PagerProperty.pageNum) {
                pageHtml += "<li>" + $.stringFormat(A_LINK_FORMAT, createLinkHtml(PagerProperty.pageIndex + 1, NEXT_PAGE)) + "</li>";
            }
            if (!PagerProperty.numberVisible) {
                if (PagerProperty.pageIndex != PagerProperty.pageNum) {
                    pageHtml += "<li>" + $.stringFormat(A_LINK_FORMAT, createLinkHtml(PagerProperty.pageNum, LAST_PAGE)) + "</li>";
                }
            }
            if (this.needButton) {
                pageHtml += "<li><input type='text' style='width:25px;height:15px;line-height:15px;margin-bottom:3px;margin-left:5px;font-size:13px;' value='" + this.pageIndex + "'/><input type='button' value='跳转'/></li>"
            }
            pageHtml += "</ul>"

            $(this).html(pageHtml);

            var _linkFun = PagerProperty.linkFun;
            var _pageNum = PagerProperty.pageNum;
            if ($(this).find("input[type='text']").length != 0) {
                $(this).find("input[type='text']").blur(function () {
                    if ($(this).val() == "") return;
                    var strP = /^[1-9]\d*$/;
                    if (!strP.test($(this).val())) {
                        alert(PAGEINDEX_OVERFLOW);
                        $(this).val('');
                        $(this).focus();
                    } else {
                        if (parseInt($(this).val()) > _pageNum) {
                            alert(PAGEINDEX_OVERFLOW);
                            $(this).val('');
                            $(this).focus();
                        }
                    }
                })
                $(this).find("input[type='button']").click(function () {
                    _linkFun(parseInt($(this).siblings("input[type='text']").val()));
                })
            }

            function makeALinkHtml(pageIndex, num) {
                if (num == pageIndex) {
                    return "<li>" + $.stringFormat(A_LINK_CURRENT_FORMAT, createLinkHtml(num, num)) + "</li>";
                } else {
                    return "<li>" + $.stringFormat(A_LINK_FORMAT, createLinkHtml(num, num)) + "</li>";
                }
            }

            function createPageNumbers(pageIndex, pageNum, pageCount) {
                var arr = new Array();
                var half = pageCount / 2;
                if (pageNum <= pageCount) {
                    for (var i = 2; i < pageNum; i++) {
                        arr.push(i);
                    }
                } else if (pageIndex <= half) {
                    nextEtc = true;
                    for (var i = 2; i < (3 + half) ; i++) {
                        arr.push(i);
                    }
                } else if (pageIndex >= (pageNum - half + 1)) {
                    preEtc = true;
                    for (var i = (pageNum - pageCount + 3) ; i < pageNum; i++) {
                        arr.push(i);
                    }
                } else {
                    preEtc = true;
                    nextEtc = true;
                    for (var i = (pageIndex - half + 2) ; i <= (pageIndex + half - 2) ; i++) {
                        arr.push(i);
                    }
                }

                return arr;
            }

            function createPageUrl(pageIndex) {
                if (PagerProperty.formId && typeof (PagerProperty.formId) == "string") {
                    return $.stringFormat("JavaScript:$(\"#{0}\").formSubmit({1})", new Array(PagerProperty.formId, pageIndex));
                } else if (PagerProperty.linkClick && typeof (PagerProperty.linkClick) == "string") {
                    return $.stringFormat("JavaScript:{0}({1})", new Array(PagerProperty.linkClick, pageIndex));
                } else if (PagerProperty.linkClick == null) {
                    return getLocationHref(pageIndex);
                }
            }

            function getLocationHref(pageIndex) {
                if (window.location.search == "")
                    return window.location.href + "?pageIndex=" + pageIndex;
                var _href = window.location.href.substring(0, window.location.href.indexOf("?"));

                if (window.location.search.indexOf("?pageIndex") == -1)
                    return _href + "?pageIndex=" + pageIndex + "&" + window.location.search.substring(1);

                if (window.location.search.indexOf("&") != -1) {
                    return _href + "?pageIndex=" + pageIndex + window.location.search.substring(window.location.search.indexOf("&"));
                } else {
                    return _href + "?pageIndex=" + pageIndex;
                }
            }

            function createLinkHtml(pageIndex, text) {
                var _arr = new Array();
                var _a_string = createPageUrl(pageIndex);
                _arr.push(_a_string);
                _arr.push(text);
                return _arr;
            }
        }
    })
})(jQuery);

/*大图片显示插件*/
(function () {
    $.extend({
        picDialog: function (property) {

            var _defaultProperty = {
                _interval_id: null,
                height: document.documentElement.clientHeight - 150,
                width: document.documentElement.clientWidth - 200,
                multi: false,
                htmlModal: false,
                html: {
                    init: function (e) {

                    },
                    load: function (e) {

                    },
                    content: ''
                },
                pic: {
                    title: '',
                    desc: '',
                    url: ""
                }
            };
            $.extend(_defaultProperty, property);
            $.CONSTS.html.picIframe.find('.iframe-loading').css({
                top: (_defaultProperty.height - 32) / 2 + "px",
                left: (_defaultProperty.width - 32) / 2 + "px"
            });

            $.CONSTS.html.picIframe.find('.iframe-main').css({
                height: _defaultProperty.height + "px",
                width: _defaultProperty.width + "px"
            });

            $.CONSTS.html.picIframe.css({
                top: _iframe_top() + "px",
                left: _iframe_left() + "px"
            });


            _define_event();

            if (_defaultProperty.htmlModal == true) {
                _set_pic_title();
                _set_pic_desc();
                if (_defaultProperty.html.init) {
                    $.CONSTS.html.picIframe.find('.pic-main').html('');
                    _defaultProperty.html.init($.CONSTS.html.picIframe.find('.pic-main'));
                    return;
                } else if (_defaultProperty.html.content) {
                    $.CONSTS.html.picIframe.find('.iframe-loading').hide();
                    $.CONSTS.html.picIframe.find('.pic-main').html(_defaultProperty.html.content);
                } else {
                    $.CONSTS.html.picIframe.find('.iframe-loading').show();
                    $.CONSTS.html.picIframe.find('.pic-main').hide();
                    _defaultProperty.html.load($.CONSTS.html.picIframe.find('.pic-main'), $.CONSTS.html.picIframe.find('.iframe-loading'));
                }
                _show_pic_iframe();

            } else {
                _set_pic_title();
                _set_pic_desc();
                _set_pic_path();
                _show_pic_iframe();
            }

            function _init() {
                _init();
                $.CONSTS.html.picIframe.find('.pic-main').html('');
            }

            function _show_pic_iframe() {
                $.showModal(1);
                $.CONSTS.html.picIframe.show();
            }


            function _hide_pic_iframe() {
                $.closeModal(1);
                $.CONSTS.html.picIframe.hide();
                if (_defaultProperty._interval_id) clearInterval(_defaultProperty._interval_id);
            }

            function _define_event() {
                $.CONSTS.html.picIframe.find('.iframe-close').click(function () {
                    _hide_pic_iframe();
                })
            }

            function _set_pic_title(_title) {
                if (_defaultProperty.pic.title) {
                    $.CONSTS.html.picIframe.find('.pic-title').text(_defaultProperty.pic.title).show();
                } else {
                    $.CONSTS.html.picIframe.find('.pic-title').hide();
                }
            }

            function _set_pic_desc() {
                if (_defaultProperty.pic.desc) {
                    $.CONSTS.html.picIframe.find('.pic-desc').text(_defaultProperty.pic.desc);
                } else {
                    $.CONSTS.html.picIframe.find('.pic-desc').hide();
                }
            }

            function _set_pic_path() {
                if (_defaultProperty.multi) {
                } else {
                    $.CONSTS.html.picIframe.find('.iframe-loading').show();
                    $.CONSTS.html.picIframe.find('.pic-main').html('<img class="pic" src="' + _defaultProperty.pic.url + '" onload="JavaScript:$(this).attr(\'loadState\',\'loadState\')" />');
                    _defaultProperty._interval_id = setInterval(function () {
                        if ($.CONSTS.html.picIframe.find('.pic').attr('loadState') == 'loadState') {
                            _set_pic_height_width();
                            clearInterval(_defaultProperty._interval_id);
                        }
                    }, 200)
                }
            }

            function _get_pic_default_height_width() {
                var _h = null;
                return {
                    height: _defaultProperty.height - 100,
                    width: _defaultProperty.width
                };
            }

            function _set_pic_height_width() {
                var _picDefaultProperty = _get_pic_default_height_width();
                var _o = $.CONSTS.html.picIframe.find('.pic');
                if (_o.height() <= _picDefaultProperty.height && _o.width() <= _picDefaultProperty.width) {
                } else if (_o.height() > _picDefaultProperty.height && _o.width() <= _picDefaultProperty.width) {
                    _o.css({
                        height: _picDefaultProperty.height + 'px',
                        width: (_picDefaultProperty.height * _o.width()) / _o.height() + 'px'
                    });
                } else if (_o.height() <= _picDefaultProperty.height && _o.width() > _picDefaultProperty.width) {
                    _o.css({
                        width: _picDefaultProperty.width + 'px',
                        height: (_picDefaultProperty.width * _o.height()) / _o.width() + 'px'
                    });
                } else {
                    if ((_picDefaultProperty.width / _picDefaultProperty.height) >= (_o.width() / _o.height())) {
                        _o.css({
                            height: _picDefaultProperty.height + 'px',
                            width: (_picDefaultProperty.height * _o.width()) / _o.height() + 'px'
                        });
                    } else {
                        _o.css({
                            width: _picDefaultProperty.width + 'px',
                            height: (_picDefaultProperty.width * _o.height()) / _o.width() + 'px'
                        });
                    }
                }
                if (_o.height() < _picDefaultProperty.height) {
                    _o.css({ 'margin-top': (_picDefaultProperty.height - _o.height()) / 2 + 'px' });
                }
                $.CONSTS.html.picIframe.find('.iframe-loading').hide();
                $.CONSTS.html.picIframe.find('.pic-title,.pic-desc').show();
                _o.show();
            }

            function _iframe_top() {
                var _h = document.documentElement.clientHeight - _defaultProperty.height;
                if (_h > 0) {
                    return _h / 2;
                } else {
                    return 0;
                }
            }

            function _iframe_left() {
                var _w = document.documentElement.clientWidth - _defaultProperty.width;
                if (_w > 0) {
                    return _w / 2;
                } else {
                    return 0;
                }
            }
        }
    })
})(jQuery);

/*Ajax相关*/
(function () {
    $.extend({
        ajaxHandle: function (property) {
            var _ajaxGlobalSettings = {
                url: "",
                type: "POST",
                data: {},
                loading: true,
                loadingContainer: null,
                error: function (response) { },
                success: function (response) { }
            };
            $.extend(_ajaxGlobalSettings, property);
            $.ajax({
                type: _ajaxGlobalSettings.type,
                url: _ajaxGlobalSettings.url,
                data: _ajaxGlobalSettings.data,
                beforeSend: function () {
                    if (_ajaxGlobalSettings.loading) {
                        $.loadingIcon(_ajaxGlobalSettings.loadingContainer);
                    }
                },
                error: function (XMLHttpRequest, errorInfo, exception) {
                    $.loadingIcon("close");
                    $.messageError($.CONSTS.message.Internal_Server_Error + "【" + XMLHttpRequest.status + "】");
                },
                success: function (response, textStatus, jqXHR) {
                    $.loadingIcon("close");
                    $.ajaxResultHandle(response, _ajaxGlobalSettings.success);
                }
            });
        },
        ajaxResultHandle: function (response, callback) {
            $.loadingIcon('close');
            if (!response) {
                return;
            }
            if (response.e_type == 'modelstate') {
                for (var ele in response.e_content) {
                    $.showElementModelStateMsg(ele, response.e_content[ele]);
                }
            } else if (response.e_type == 'userexception') {
                $.messageErrorX(response.e_content);
            } else if (response.e_type == 'exception') {
                $.messageErrorX($.CONSTS.message.Internal_Server_Error);
            } else if (response.e_type == 'authorize') {
                location.reload();
            } else {
                if (callback) {
                    callback(response);
                }
            }
        },
        showElementModelStateMsg: function (ele, msg) {
            if (typeof (ele) == "string") {
                var obj = $("form [name='" + ele + "']");
                var _span = $("span[data-valmsg-for='" + ele + "']");
                obj.addClass("input-validation-error");
                _span.removeClass("field-validation-valid").addClass("field-validation-error");
                _span.html('<span class="" for="' + ele + '" generated="true">' + msg + '</span>');

                obj.one("focus", function () {
                    obj.removeClass("input-validation-error");
                    _span.removeClass("field-validation-error").addClass("field-validation-valid");
                    _span.html('');
                })
            } else {
                var obj = ele;
                var _span = $("span[data-valmsg-for='" + ele.attr("name") + "']");
                obj.addClass("input-validation-error");
                _span.removeClass("field-validation-valid").addClass("field-validation-error");
                _span.html('<span class="" for="' + ele.attr("name") + '" generated="true">' + msg + '</span>');

                obj.one("focus", function () {
                    obj.removeClass("input-validation-error");
                    _span.removeClass("field-validation-error").addClass("field-validation-valid");
                    _span.html('');
                })
            }
        },
        removeElementModelStateMsg: function (ele) {
            if (typeof (ele) == "string") {
                var obj = $("form [name='" + ele + "']");
                var _span = $("span[data-valmsg-for='" + ele + "']");
                obj.removeClass("input-validation-error");
                _span.removeClass("field-validation-error").addClass("field-validation-valid");
                _span.html('');
            } else {
                var obj = ele;
                var _span = $("span[data-valmsg-for='" + ele.attr("name") + "']");
                obj.removeClass("input-validation-error");
                _span.removeClass("field-validation-error").addClass("field-validation-valid");
                _span.html('');
            }
        }
    })
})(jQuery);

/*数据绑定*/
(function () {
    $.fn.extend({
        bindingDataProperty: function (data, baseData) {
            var property = GetbindingDataProperty($(this));

            if (!property) return;

            for (var _pro in property) {
                if (_pro.indexOf("Format") != -1
                || _pro.indexOf("Handle") != -1
                || _pro.indexOf("Length") != -1
                || _pro.indexOf("Data") != -1
                || _pro.indexOf("lazyLoad") != -1)
                    continue;

                if (_pro == 'text') {
                    var _re = GetDataPropertyInfo(property.text,
                        property.textHandle,
                        property.textFormat,
                        $(this),
                        data);
                    if (_re != null && _re != undefined) {
                        if (property.textData && property.textData[_re]) {
                            $(this).text(property.textData[_re]);
                        } else if (property.textLength) {
                            $(this).text($.ellipsis(_re, property.textLength));
                            $(this).attr("title", _re);
                        } else {
                            $(this).text(_re);
                        }
                    }
                } else if (_pro == 'html') {
                    var _html = GetDataPropertyInfo(property.html,
                            property.htmlHandle,
                            property.htmlFormat,
                            $(this),
                            data);
                    if (_html != null && _html != undefined) {
                        if (property.htmlData && property.htmlData[_html]) {
                            $(this).html(property.htmlData[_html]);
                        } else {
                            $(this).html(_html);
                        }
                    }
                } else if (_pro == 'value') {
                    var _value = GetDataPropertyInfo(property.value,
                        property.valueHandle,
                        property.valueFormat,
                        $(this),
                        data);
                    if (_value != null && _value != undefined) {
                        if (property.valueData && property.valueData[_value]) {
                            $(this).val(property.valueData[_value]);
                        } else {
                            $(this).val(_value);
                        }
                    }
                } else if (_pro == 'list') {
                    var BindingTempleteID = "BindingTemplete_" + $(this).attr("list_id");
                    if (!window[BindingTempleteID]) {
                        window[BindingTempleteID] = $(this).html();
                    }

                    $(this).bindingDataList(data[property.list], { bindingformat: window[BindingTempleteID], baseData: data });
                } else {
                    var _attr = GetDataPropertyInfo(property[_pro],
                            property[_pro + "Handle"],
                            property[_pro + "Format"],
                            $(this),
                            data);
                    if (_attr != null && _attr != undefined) {
                        if (property[_pro + "Data"] && property[_pro + "Data"][_attr]) {
                            $(this).attr(_pro, property[_pro + "Data"][_attr]);
                        } else {
                            $(this).attr(_pro, _attr)
                        }
                    }
                }
            }

            function GetDataPropertyInfo(text, handle, format, obj, data, baseData) {
                if (handle) {
                    if ($.inArray(handle, $.dataBinder.options.typename) != -1) {
                        return $.dataBinder.options.rule[handle](GetJsonDataValue(data, text));
                    } else {
                        if (handle.indexOf && handle.indexOf("datetime=") != -1) {
                            var arr = handle.split('=');
                            return $.dataBinder.options.rule.datetime(GetJsonDataValue(data, text), arr[1]);
                        }
                        if (handle.indexOf && handle.indexOf("datetimeGMT=") != -1) {
                            var arr = handle.split('=');
                            return $.dataBinder.options.rule.datetimeGMT(GetJsonDataValue(data, text), arr[1]);
                        }
                        return handle(GetJsonDataValue(data, text, true), data, obj);
                    }
                } else {
                    if (format) {
                        return GetTextFormatResult(format, text, data);
                    } else {
                        return GetJsonDataValue(data, text);
                    }
                }
            }

            function GetbindingDataProperty(obj) {
                if (!obj || !obj.attr("binding")) {
                    return null;
                } else {
                    var _bindingStr = obj.attr("binding");
                    var returnObj;
                    try {
                        returnObj = eval("(" + _bindingStr + ")");
                    } catch (e) {
                        alert("GetbindingDataProperty" + e);
                    }
                    if (returnObj.source) {
                        try {
                            returnObj = eval("(" + returnObj.source + ")");
                        } catch (e) {
                            alert("GetbindingDataProperty" + e);
                        }
                    }
                    return returnObj;
                }
            }

            function GetTextFormatResult(format, dataProperty, data) {
                var _property = dataProperty.split(',');
                return $.stringFormat(format, $.map(_property, function (n) { return GetJsonDataValue(data, n); }));
            }

            function GetJsonDataValue(a, b, needZero) {
                if (b == undefined || b == null || b == '') return a;
                if (b.indexOf(".") == -1) {
                    if (a[b] === 0 && !needZero) return '0';
                    if (a[b] == undefined || a[b] == null) return '';
                    return a[b];
                } else {
                    var _arr = b.split('.');
                    var _value = a;

                    var _startIndex = 0;

                    if (_arr[0] == "base" && baseData) {
                        _value = baseData;
                        _startIndex = 1;
                    }

                    for (var i = _startIndex; i < _arr.length; i++) {
                        if (_value != null && _value != undefined) {
                            _value = _value[_arr[i]];
                        }
                    }
                    return _value;
                }
            }
        },
        bindingData: function (data, baseData) {
            $(this).find("[binding]").add(this).each(function () {
                $(this).bindingDataProperty(data, baseData);
            })
            return $(this);
        },
        bindingDataList: function (list, para) {
            if (!para) para = {};

            var bindingformat = para.bindingformat;
            var baseData = para.baseData;

            if ($(this).size() == 0) return;
            if ($(this).length == undefined) return;
            if (!bindingformat && !$(this).data("BindingFormat")) {
                $(this).data("BindingFormat", $(this).html());
                $(this).html('');
                $(this).hide();
            }

            if (!bindingformat && !$(this).data("BindingFormat")) {
                return $(this);
            }

            if (!bindingformat) {
                bindingformat = $(this).data("BindingFormat");
            }

            var property = eval("(" + $(this).attr("binding") + ")");

            var data = list;
            if (property && property.pageId) {
                data = list.PageData;
            }
            if (property && property.emptyHtml && (!data || data.length == 0)) {
                if (property.emptyHtml.indexOf('#') == 0) {
                    property.emptyHtml = $(property.emptyHtml).html();
                }
                $(this).html(property.emptyHtml);
                $(this).show();

                if (property.pageId) {
                    $("#" + property.pageId).html('');
                }

                return $(this);
            }
            if (!data) return $(this);
            $(this).html('');
            for (var i = 0; i < data.length; i++) {
                var formatObj = $(bindingformat);
                data[i].Binding_Index = i + 1;
                formatObj.bindingData(data[i], baseData);
                $(this).append(formatObj);
                if (para.itemHandle) {
                    para.itemHandle(i, formatObj, data[i]);
                }
            }
            $(this).show();

            if (property && property.pageId) {
                $("#" + property.pageId).createPager({
                    pageIndex: list.PageIndex,
                    pageSize: list.PageSize,
                    total: list.TotalItemCount,
                    formId: property.formId ? property.formId : null,
                    linkClick: property.pageCallback
                });
            }
            return $(this);
        }
    })
    $.dataBinder = {
        options: {
            typename: new Array("datetime", "filesize", "datetimeGMT", "datetime_Date", "datetimeGMT_Date"),
            rule: {
                datetime: dataBinder_Datetime,
                filesize: dataBinder_FileSize,
                datetimeGMT: dataBinder_DateTimeGMT,
                datetime_Date: dataBinder_DateTime_Date,
                datetimeGMT_Date: dataBinder_DateTimeGMT_Date
            }
        },
        addMethod: function (name, handle) {
            $.dataBinder.options.typename.push(name);
            $.dataBinder.options.rule[name] = handle;
        }
    }
    function dataBinder_Datetime(value, format) {
        if (!value) return "";
        if (!format) { format = "yyyy-MM-dd hh:mm:ss"; }
        return $.dateTimeFormat($.renderTime(value), format)
    }
    function dataBinder_DateTime_Date(value) {
        if (!value) return "";
        return $.dateTimeFormat($.renderTime(value), "yyyy-MM-dd")
    }
    function dataBinder_FileSize(value) {
        if (!value) return "";
        return $.fileSize(value);
    }
    function dataBinder_DateTimeGMT(value, format) {
        if (!value) return "";
        if (!format) { format = "yyyy-MM-dd hh:mm:ss"; }
        value = value.replace("GMT+08:00", "GMT+08");
        value = value.replace("GMT+0800", "GMT+08");
        value = value.replace("+0800", "GMT+08");
        var _date = new Date(value);
        return $.dateTimeFormat(_date, format)
    }
    function dataBinder_DateTimeGMT_Date(value) {
        if (!value) return "";
        value = value.replace("GMT+08:00", "GMT+08");
        var _date = new Date(value);
        return $.dateTimeFormat(_date, "yyyy-MM-dd")
    }
})(jQuery);


(function () {
    $.extend({
        w_socket: function (param) {
            var defaultParam = {
                url: "",
                onstart: function () { },
                onopen: function () { },
                onclose: function () { },
                onerror: function () { },
                onmessage: function () { },
                onexception: function (ex) { },
                onnotsupport: function () { }
            }

            $.extend(defaultParam, param);
            var obj = {};
            var _socket;

            try {
                var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

                if (support == null) {
                    defaultParam.onnotsupport();
                    return null;
                }

                _socket = new window[support](defaultParam.url);

                _socket.onopen = function () {
                    defaultParam.onopen();
                }

                _socket.onclose = function (evt) {
                    defaultParam.onclose();
                }

                _socket.onerror = function (evt) {
                    defaultParam.onerror();
                }

                _socket.onmessage = function (evt) {
                    defaultParam.onmessage(eval("(" + evt.data + ")"), evt.data);
                }

            } catch (e) {
                defaultParam.onexception(e);
                return null;
            }

            obj.send = function (jsonData) {
                if (typeof (jsonData) == 'string') {
                    _socket.send(jsonData);
                } else {
                    _socket.send($.jsonString(jsonData))
                }
            }

            obj.socket = _socket;
            return obj;
        }
    })
})(jQuery);

// jquery.sy.cookie.js
(function ($) {
    $.extend({
        cookie: function (name, value, options) {
            /// <summary>读取或设置Cookie</summary>
            /// <param></param>
            if (typeof value != 'undefined') { // name and value given, set cookie
                options = options || {}; { }
                if (value === null) {
                    value = '';
                    options.expires = -1;
                }
                var expires = '';
                if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                    var date;
                    if (typeof options.expires == 'number') {
                        date = new Date();
                        date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                    } else {
                        date = options.expires;
                    }
                    expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
                }
                // CAUTION: Needed to parenthesize options.path and options.domain
                // in the following expressions, otherwise they evaluate to undefined
                // in the packed version for some reason...
                var path = options.path ? '; path=' + (options.path) : '';
                var domain = options.domain ? '; domain=' + (options.domain) : '';
                var secure = options.secure ? '; secure' : '';
                document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
            } else { // only name given, get cookie
                var cookieValue = null;
                if (document.cookie && document.cookie != '') {
                    var cookies = document.cookie.split(';');
                    for (var i = 0; i < cookies.length; i++) {
                        var cookie = jQuery.trim(cookies[i]);
                        // Does this cookie string begin with the name we want?
                        if (cookie.substring(0, name.length + 1) == (name + '=')) {
                            cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                            break;
                        }
                    }
                }
                return cookieValue;
            }
        },
        setCookie: function (name, value, options) {
            /// <summary>设置Cookie</summary>
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            // CAUTION: Needed to parenthesize options.path and options.domain
            // in the following expressions, otherwise they evaluate to undefined
            // in the packed version for some reason...
            var path = options.path ? '; path=' + (options.path) : '';
            var domain = options.domain ? '; domain=' + (options.domain) : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        },
        getCookie: function (name) {
            /// <summary>读取Cookie</summary> 
            var cookieValue = null;
            if (document.cookie && document.cookie != '') {
                var cookies = document.cookie.split(';');
                for (var i = 0; i < cookies.length; i++) {
                    var cookie = jQuery.trim(cookies[i]);
                    // Does this cookie string begin with the name we want?
                    if (cookie.substring(0, name.length + 1) == (name + '=')) {
                        cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                        break;
                    }
                }
            }
            return cookieValue;
        },
        deleteCookie: function (name) {
            /// <summary>删除Cookie</summary>
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = $.getCookie(name);
            if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
        }
    })
})(jQuery);

/*Init*/
$(function () {
    $.messageInit();

    $.Validate_Promt_Init();

    $.Input_Trim_Init();

    $.Input_Number_Init();
})





