var pageIndex = 1; //分页当前索引
var isLoad = true; //是否滚动加载
$(function () {
    //获取项目
    GetResource();

    //返回
    $(".glyphicon-circle-arrow-left").on("click", function () {

        window.location.href = ACTION_URL("Index", "Reception");
    });

    //大事件详情
    $(document).delegate(".resourcedetail", "click", function () {
        var r_id = $(this).attr("data-id");

        window.location.href = ACTION_URL("ResourceDetail?id=" + r_id, "Reception");
    });

    //滚动加载
    window.onscroll = function () {
        if (isLoad) {
            if (getScrollTop() + getClientHeight() == getScrollHeight()) {
                pageIndex++;
                //获取项目
                GetResource();
            }
        }
    }
});

//获取大事件
function GetResource() {

    $.L.ajaxHandle({
        type: "get",
        cache: "false",
        url: ACTION_URL("SearchResourceList", "Reception"),
        data: {
            keyword: keyword,
            pageIndex: pageIndex
        },
        success: function (response) {
            //alert(JSON.stringify(response));
            if (response.TotalItemCount > 0) {
                if (response.TotalPageCount == pageIndex) {
                    isLoad = false;
                }
                var data = response.PageData;
                if (data.length > 0) {
                    var str = "";
                    for (var i = 0; i < data.length; i++) {
                        str += "<tr>";
                        str += "<td>" + data[i].Name + "</td>";
                        str += "<td>" + data[i].SSJD + "</td>";
                        str += "<td>" + data[i].StartTime._data() + "</td>";
                        str += "<td>" + data[i].EndTime._data() + "</td>";
                        switch (data[i].RStatus) {
                            case "1":
                                str += "<td>未开工</td>";
                                break;
                            case "2":
                                str += "<td>已开工</td>";
                                break;
                            case "3":
                                str += "<td>已投产</td>";
                                break;
                        }
                        str += "<td><a href='javascript:void(0);' class='resourcedetail' data-id='" + data[i].R_ID + "'>查看详情</a></td>";
                        str += "</tr>";
                    }

                    $("#resourcelist").append(str);
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

