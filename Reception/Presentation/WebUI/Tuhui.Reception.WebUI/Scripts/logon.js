(function ($) {
    $.extend({
        initInput: function (id, text) {
            var ipt = $(id);

            if (ipt) {
                ipt.css("color", "#fff");
                ipt.attr("alt", "请输入" + text);
                ipt.attr("title", "请输入" + text);
                if (ipt.val() == "") {
                    ipt.val(text);
                }
                ipt.focus(function () {
                    if (ipt.attr("isPwd")) {
                        ipt.attr("type", "password");
                    }
                    ipt.css("color", "#333");
                    if (ipt.val() == text) {
                        ipt.val("");
                    }
                }).blur(function () {
                    ipt.css("color", "#fff");
                    if (ipt.val() == "") {
                        ipt.val(text);
                        if (ipt.attr("isPwd")) {
                            ipt.attr("type", "text");
                        }
                    }
                });
            }
        },
        setPassword: function (pwdid, pwdtxtid) {

            var pwd = $(pwdid);
            var pwdTxt = $(pwdtxtid);

            pwd.css("color", "#fff");
            pwdTxt.css("color", "#fff");
            pwdTxt.attr("alt", "请输入密码");
            pwdTxt.attr("title", "请输入密码");
            pwd.attr("alt", "请输入密码");
            pwd.attr("title", "请输入密码");

            if (pwd.val() == "") {
                pwd.hide();
                pwdTxt.val("密码").show();
            }
            pwdTxt.focus(function () {
                pwdTxt.hide();
                pwd.show().focus();
                return false;
            });
            pwd.focus(function () {
                pwd.css("color", "#fff");
            })
            pwd.blur(function () {
                pwd.css("color", "#fff");
                if (pwd.val() == "") {
                    pwd.hide();
                    pwdTxt.val("密码").show();
                }
                return false;
            });
        }
    })
})(jQuery);

$(function () {

    $("#UserName").val('');
    $("#Password").val('');

    $.initInput("#UserName", "帐号");
    $.setPassword("#Password", "#Password_Text");


    $("#UserName,#Password").enterKey(function () {
        form_submit();
    })

    $(".logon-button").click(function () {
        form_submit();
    })

    function form_submit() {
        if ($("#UserName").val() == "帐号") {
            $("#UserName").val('');
        }

        if ($("#Password").val() == "密码") {
            $("#Password").val('');
        }

        $('.logon-button').attr('disabled', 'disabled');
       /* $('.logon-button').text('登录中...');*/
        $("#logon_form").submit();
    }
})