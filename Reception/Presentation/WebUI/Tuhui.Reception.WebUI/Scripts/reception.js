var map;
$(function () {

    window.onresize = _onResize;

    _onResize();
   
    map = $.tMap.init("map", {
        defaultLayer: "autonavi",
        lng: 118.63792419433595,
        lat: 32.067155999071446
    });
  
    //资源分类及其列表
    GetResourceTypeList();

    //获取所有资源
    GetResourceList();

    //地图切换
    $(".resource_status_left").on("click", function () {
        if ($(this).hasClass("wx")) //卫星
        {
            map.changeTLayer("autonavi_wx");
            $(this).removeClass("wx").addClass("wp");
        } else { //瓦片
            map.changeTLayer("autonavi");
            $(this).removeClass("wp").addClass("wx");
        }
    });

    //最新导航切换
    $(".resource_navigation_newest").on("click", function () {
        if ($(this).hasClass("navi_open")) //打开
        {
            GetNewest();
            $(this).removeClass("navi_open").addClass("navi_close");
        } else { //关闭
            map.clear();
            //获取所有资源
            GetResourceList();
            $(this).removeClass("navi_close").addClass("navi_open");
        }
    });

    //右侧资源点点击定位
    $(document).delegate(".resource_list_item", "click", function () {
        var lng = $(this).attr("data-lng");
        var lat = $(this).attr("data-lat");
        map.panTo(lng, lat);
        var popupContent = "";
        popupContent += "<div style='color:#14265c;text-align:center;font-size:22px;font-weight:bold;border-bottom:3px solid #758db8;min-width:300px;padding-bottom:5px;'>" + $(this).html() + "</div>";
        popupContent += "<div style='padding:0 20px;font-size:18px;line-height:30px;'>";
        popupContent += "<div style='border-bottom:1px solid #cdced0;height:30px;padding-left:46px;margin-top:10px;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;'><strong>所属街道</strong>&nbsp;&nbsp;" + $(this).attr("data-ssjd") + "</div>";
        popupContent += "<div style='border-bottom:1px solid #cdced0;height:30px;padding-left:46px;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;'><strong>责任单位</strong>&nbsp;&nbsp;" + $(this).attr("data-rrdw") + "</div>";
        popupContent += "<div style='border-bottom:1px solid #cdced0;height:30px;padding-left:46px;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;'><strong>施方单位</strong>&nbsp;&nbsp;" + $(this).attr("data-sfdw") + "</div>";
        popupContent += "<div style='border-bottom:1px solid #cdced0;height:30px;padding-left:46px;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;'><strong>开工时间</strong>&nbsp;&nbsp;" + $(this).attr("data-starttime")._data() + "</div>";
        popupContent += "<div style='height:30px;padding-left:10px;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;'><strong>计划完成时间</strong>&nbsp;&nbsp;<span style='font-weight:200;'>" + $(this).attr("data-endtime")._data() + "</span></div>";
        popupContent += "<div style='background-color:#3A89D3;text-align:center;color:#fff;border-radius:5px;margin-top:5px;cursor:pointer;' onclick='RedirectTo(\"" + $(this).attr("data-rid") + "\");'>查看详情</div>";
        popupContent += "</div>";
        map.openPopup(lng, lat, popupContent);
    });

    //选择搜索项
    $(".dropdown-menu li a").on("click", function () {
        var val = $(this).html();
        $("#selectType").html(val);
    });

    //搜索
    $("#search").on("click", function () {
        var searchName = $.trim($("#searchName").val());
        if (searchName.length == 0) {
            $("#searchName").focus();
        } else {
            var val = $("#selectType").html();
            if (val == "项目") {
                window.location.href = ACTION_URL("SearchResource?keyword=" + escape(searchName), "Reception");
            } else if (val == "大事件") {
                window.location.href = ACTION_URL("SearchResourceEvent?keyword=" + escape(searchName), "Reception");
            }
        }
    });
})

function _onResize() {
    $("body,#map").css({
        height: document.documentElement.clientHeight + "px"
    })
}

//获取所有资源
function GetResourceList() {

    $.ajax({
        type: "get",
        cache: "false",
        url: ACTION_URL("GetResourceList", "Reception"),
        data: {
        },
        success: function (response) {
            if (response.length > 0) {
                var _markers = [];
                for (var i = 0; i < response.length; i++) {
                    //图片类型
                    var iconUrl = CONTENT_URL("Content/images/reception/");
                    switch (response[i].RStatus)
                    {
                        case "1": //未开工
                            iconUrl += "wkg_big.png";
                            break;
                        case "2": //已开工
                            iconUrl += "ykg_big.png";
                            break;
                        case "3": //已投产
                            iconUrl += "ywg_big.png";
                            break;
                    }
                    
                    var _m = map.addImgMarker(response[i].Long, response[i].Lat, {
                        icon: {
                            addToMapFlag: false,
                            iconUrl: iconUrl,
                            iconSize: [47, 57],
                            iconAnchor: [23.5, 0]
                        }
                    });
                    _markers.push(_m);
                }

                map.addClusterMarkers(_markers, {
                    maxClusterRadius: 50,
                    spiderfyOnMaxZoom: true,
                    showCoverageOnHover: true,
                    zoomToBoundsOnClick: true,
                    singleMarkerMode: false,
                    disableClusteringAtZoom: null,
                    removeOutsideVisibleBounds: true,
                    animateAddingMarkers: false,
                    spiderfyDistanceMultiplier: 1,
                    chunkedLoading: false,
                    chunkInterval: 200,
                    chunkDelay: 50,
                    chunkProgress: null,
                    polygonOptions: {}
                });
            }
        }
    });
}

//获取最新一条导航
function GetNewest(){
    $.L.ajaxHandle({
        type: "get",
        cache: "false",
        url: ACTION_URL("GetNewest", "Reception"),
        data: {
        },
        success: function (response) {
            if (response != null) {
                var list = JSON.parse(response.GHLJ);
                //显示线
                map.addLine(list);
                //定位到第一个点
                if (list.length > 0) {
                    map.panTo(list[0].lng, list[0].lat);
                }
            }
        }
    });
}

//资源分类及其列表
function GetResourceTypeList(){
    $.L.ajaxHandle({
        type: "get",
        cache: "false",
        url: ACTION_URL("GetResource_Type", "Reception"),
        data: {
        },
        success: function (response) {
            var html = template('resourceList', {
                list: response
            });
            document.getElementsByClassName('resource_list')[0].innerHTML = html;

            //处理
            var rtidArr = [];
            $(".resource_list_head").each(function (index,value) {
                var data_rtid = $(this).attr("data-rtid");
                if (rtidArr.indexOf(data_rtid) == -1) {
                    rtidArr.push(data_rtid);
                    $(".resource_list_item[data-rtid='" + data_rtid + "']:last").addClass("no_border");
                } else {
                    $(this).remove();
                }
            });
        }
    });
}

//进入到详情页面
function RedirectTo(rid){
    window.location.href = ACTION_URL("ResourceDetail?id=" + rid, "Reception");
}