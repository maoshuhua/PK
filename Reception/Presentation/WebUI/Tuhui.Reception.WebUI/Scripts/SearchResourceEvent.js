var pageIndex = 1; //分页当前索引
var isLoad = true; //是否滚动加载
$(function () {
    //获取大事件
    GetResourceEvent();

    //返回
    $(".glyphicon-circle-arrow-left").on("click", function () {

        window.location.href = ACTION_URL("Index", "Reception");
    });

    //大事件详情
    $(document).delegate(".eventdetail", "click", function () {
        var event_id = $(this).attr("data-id");

        //获取大事件相关信息
        $.L.ajaxHandle({
            type: "get",
            cache: "false",
            url: ACTION_URL("GetEventById", "Reception"),
            data: {
                id: event_id
            },
            success: function (response) {
                if (response != null) {
                    layer.open({
                        title: false,
                        type: 1,
                        area: ['720px', '540px'], //宽高
                        closeBtn: 1,
                        shadeClose: true,
                        content: '<h3>' + response.Name + '&nbsp;&nbsp;<small>' + response.AddTime._data() + '</small></h3><h6>' + response.JDRY + '</h6><div style="text-align:left;padding:20px;">' + response.Content + '</div>'
                    });
                }
            }
        });
    });

    //滚动加载
    window.onscroll = function () {
        if (isLoad) {
            if (getScrollTop() + getClientHeight() == getScrollHeight()) {
                pageIndex++;
                //获取大事件
                GetResourceEvent();
            }
        }
    }
});

//获取大事件
function GetResourceEvent() {

    $.L.ajaxHandle({
        type: "get",
        cache: "false",
        url: ACTION_URL("SearchEventList", "Reception"),
        data: {
            keyword: keyword,
            pageIndex: pageIndex
        },
        success: function (response) {
            if (response.TotalItemCount > 0) {
                if (response.TotalPageCount == pageIndex) {
                    isLoad = false;
                }
                var data = response.PageData;
                if (data.length > 0) {
                    var str = "";
                    for (var i = 0; i < data.length; i++) {
                        str += "<tr>";
                        str += "<td>" + data[i].AddTime._data() + "</td>";
                        str += "<td>" + data[i].JDRY + "</td>";
                        str += "<td>" + data[i].Name + "</td>";
                        str += "<td><a href='javascript:void(0);' class='eventdetail' data-id='" + data[i].RE_ID + "'>查看详情</a></td>";
                        str += "</tr>";
                    }

                    $("#eventlist").append(str);
                }
            } else {
                $("#no_data").show();
                isLoad = false;
            }
        }
    });
}

//获取滚动条当前的位置 
function getScrollTop() {
    var scrollTop = 0;
    if (document.documentElement && document.documentElement.scrollTop) {
        scrollTop = document.documentElement.scrollTop;
    }
    else if (document.body) {
        scrollTop = document.body.scrollTop;
    }
    return scrollTop;
}

//获取当前可视范围的高度 
function getClientHeight() {
    var clientHeight = 0;
    if (document.body.clientHeight && document.documentElement.clientHeight) {
        clientHeight = Math.min(document.body.clientHeight, document.documentElement.clientHeight);
    }
    else {
        clientHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight);
    }
    return clientHeight;
}

//获取文档完整的高度 
function getScrollHeight() {
    return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
}

