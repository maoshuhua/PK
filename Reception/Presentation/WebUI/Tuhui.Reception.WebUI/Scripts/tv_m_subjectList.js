// 管理界面-专题管理
$.tv_m = {};
(function () {
    var _this = $.tv_m;
    $.extend($.tv_m, {
        Data_AllVideoList: [],
        Data_MapVideoList: [],
        Data_MarkerList: [],
        Data_MarkerDic: {},
        Data_VideoIds: [],
        Data_VideoSelectIds: []
    })
})(jQuery);
(function () {
    var _this = $.tv_m;
    $.extend($.tv_m, {
        tmap_Init: function () {
            this.map = $.tMap.init("map", {
                defaultLayer: "autonavi_wx",
                lng: 118.7464141845703,
                lat: 32.026415223498915,
                zoom: 11
            })

            this.map.on("move", function () {
                var _center = _this.map.getCenter();
                $("#centerlng").val(_center.lng);
                $("#centerlat").val(_center.lat);

            })

            this.map.on("zoomend", function () {
                var _zoom = _this.map.getZoom();
                $("#centerzoom").val(_zoom);
            })

        },
        togglePanel: function () {
            if ($("#page_panel").attr("state") == "0") {
                $("#page_panel").attr("state", "1")
                    .text("隐藏信息")
                    .css({ "left": "260px" })
                $("#subject_page_frame").css({ "left": "5px" })
            } else {
                $("#page_panel").attr("state", "0")
                    .text("显示信息")
                    .css({ "left": "5px" })
                $("#subject_page_frame").css({ "left": "-300px" })
            }
        },
        tmap_drawCircle: function () {
            this.map.drawCircle({
                complete: function (result) {
                    $.each(_this.Data_MapVideoList, function (index, ele) {
                        if (_this.map.getDistance(result.lng, result.lat, ele.videolng, ele.videolat) < result.radius) {
                            if ($.inArray(ele.videoid, _this.Data_VideoSelectIds) == -1) {
                                _this.Data_VideoSelectIds.push(ele.videoid);
                            }
                        }
                    })
                    _this.changeVideoStateToMap();
                }
            });
        },
        tmap_drawRect: function () {
            this.map.drawRectangle({
                complete: function (result) {
                    $.each(_this.Data_MapVideoList, function (index, ele) {
                        if (_this.map.isInRect(ele.videolng, ele.videolat, result.start, result.end)) {
                            if ($.inArray(ele.videoid, _this.Data_VideoSelectIds) == -1) {
                                _this.Data_VideoSelectIds.push(ele.videoid);
                            }
                        }
                    })
                    _this.changeVideoStateToMap();
                }
            });
        },
        tmap_drawRectClear: function () {
            this.map.drawRectangle({
                complete: function (result) {
                    $.each(_this.Data_MapVideoList, function (index, ele) {
                        if (_this.map.isInRect(ele.videolng, ele.videolat, result.start, result.end)) {
                            if ($.inArray(ele.videoid, _this.Data_VideoSelectIds) != -1) {
                                _this.Data_VideoSelectIds = $.grep(_this.Data_VideoSelectIds, function (a) { return a != ele.videoid });
                            }
                        }
                    })
                    _this.changeVideoStateToMap();
                }
            });
        }
    })
})(jQuery);
(function () {
    var _this = $.tv_m;
    $.extend($.tv_m, {
        zTree_Init: function (dataList) {
            var setting = {
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                callback: {
                    onClick: _this.zTree_OnClick,
                    onRightClick: function (event, treeId, treeNode) {
                        if (!treeNode && event.target.tagName.toLowerCase() != "button" && $(event.target).parents("a").length == 0) {
                            _this.zTree.cancelSelectedNode();
                            //showRMenu("root", event.clientX, event.clientY);
                        } else if (treeNode && !treeNode.noR) {
                            _this.zTree.selectNode(treeNode);
                            //showRMenu("node", event.clientX, event.clientY);
                        }
                    }
                }
            };
            $.fn.zTree.init($("#treeDemo"), setting, dataList);
            this.zTree = $.fn.zTree.getZTreeObj("treeDemo");
        },
        zTree_OnClick: function (event, treeId, treeNode) {
            if (treeNode.isleaf == 0) {
                $("#subject_page_frame").find("[name='forsubsubject']").hide();
                $("#div_addSubject").show();
                $("#tj_subjectname").val('');
                $("#zj_subjectname").val('');
                _this.map.resetView();
            } else {
                $("#subject_page_frame").find("[name='forsubsubject']").show();
                $("#div_addSubject").hide();
                if (treeNode.subject.centerlng != 0) {
                    _this.map.setView(treeNode.subject.centerlng, treeNode.subject.centerlat, treeNode.subject.centerzoom);
                }
            }
            _this.showSubjectInfo(treeNode.subject);
        }
    })
})(jQuery);

(function () {
    var _this = $.tv_m;
    $.extend($.tv_m, {
        ajax_GetSubjectList: function (noVideoList) {
            $.L.ajaxHandle({
                url: ACTION_URL("GetAllSubjects", "Manager"),
                loading: 2,
                loadingContainer: $("#map"),
                success: function (response) {
                    _this.zTree_Init($.map(response, function (a) {
                        return {
                            id: a.subjectid,
                            pId: a.parentid,
                            open: a.isleaf == 0,
                            name: a.subjectname,
                            isParent: a.isleaf == 0,
                            isleaf: a.isleaf,
                            subject: a
                        }
                    }))
                    if (!noVideoList) {
                        setTimeout(function () {
                            _this.ajax_GetAllVideoList();
                        }, 300)
                    }
                }
            })
        },
        ajax_GetAllVideoList: function () {
            $.L.ajaxHandle({
                url: ACTION_URL("GetAllVideo", "Manager"),
                loading: 4,
                loadingContainer: $("#map"),
                data: { pageSize: 100000 },
                success: function (response) {
                    _this.Data_AllVideoList = response.PageData;
                    _this.Data_MapVideoList = _this.Data_AllVideoList;
                    setTimeout(function () {
                        _this.showVideoToMap();
                    }, 300)
                }
            })
        },
        ajax_GetVideoListBySubjectId: function (subjectId) {
            $.L.ajaxHandle({
                url: ACTION_URL("GetVideoListBySubjectId", "Manager"),
                loading: 4,
                loadingContainer: $("#map"),
                data: { subjectId: subjectId },
                success: function (response) {
                    _this.Data_VideoSelectIds = response;
                    _this.changeVideoStateToMap();
                }
            })
        },
        ajax_AddSubject: function (type) {
            var _addData;
            if (type == 1) {
                if ($("#tj_subjectname").val() == '') {
                    $.L.messageWarningQ("专题名不能为空！");
                    return;
                }
                _addData = {
                    isleaf: 0,
                    subjectname: $("#tj_subjectname").val()
                }
            } else if (type == 2) {
                if ($("#zj_subjectname").val() == '') {
                    $.L.messageWarningQ("专题名不能为空！");
                    return;
                }
                _addData = {
                    isleaf: $("#zj_isleaf").prop("checked") == true ? 1 : 0,
                    parentid: $("#subjectid").val(),
                    subjectname: $("#zj_subjectname").val()
                }
            }
            $.L.ajaxHandle({
                url: ACTION_URL("AddSubject", "Manager"),
                loading: 2,
                loadingContainer: $("#map"),
                data: _addData,
                success: function (response) {
                    if (response == true) {
                        setTimeout(function () {
                            _this.ajax_GetSubjectList(true);
                        }, 300)
                    }
                }
            })
        },
        ajax_UpdateSubject: function () {
            var _updateData = {};
            $("#subject_page_frame input").each(function () {
                _updateData[$(this).attr("id")] = $(this).val();
            })

            $.L.ajaxHandle({
                url: ACTION_URL("UpdateSubject", "Manager"),
                loading: 2,
                loadingContainer: $("#map"),
                data: _updateData,
                success: function (response) {
                    if (response == true) {
                        setTimeout(function () {
                            _this.ajax_GetSubjectList(true);
                        }, 300)
                    }
                }
            })
        },
        ajax_UpdateSubjectVideoList: function () {
            var _subjectId = $("#subjectid").val();
            var _videoIds = _this.Data_VideoSelectIds;
            $.L.ajaxHandle({
                url: ACTION_URL("UpdateSubjectVideoList", "Manager"),
                loading: 2,
                loadingContainer: $("#map"),
                data: $.param({ subjectId: _subjectId, videoIds: _videoIds }, true),
                success: function (response) {
                    if (response == true)
                        $.L.messageSuccessT("更新成功");
                }
            })
        },
        ajax_DeleteSubject: function () {
            $.L.messageConfirmT("删除专题会同时删除与视频列表的关联关系,确认删除?", function () {
                $.L.ajaxHandle({
                    url: ACTION_URL("DeleteSubject", "Manager"),
                    loading: 2,
                    loadingContainer: $("#map"),
                    data: { subjectid: $("#subjectid").val() },
                    success: function (response) {
                        if (response == 100) {
                            $.L.messageErrorT("无法删除有子专题的节点！");
                        } else {
                            setTimeout(function () {
                                _this.ajax_GetSubjectList(true);
                            }, 300)
                        }
                    }
                })
            })
        }
    })
})(jQuery);

(function () {
    var _this = $.tv_m;
    $.extend($.tv_m, {
        changeVideoStateToMap: function () {
            $("#subjectVideoCount").text(_this.Data_VideoSelectIds.length);

            var _list = [];
            $.each(_this.Data_MarkerList, function (index, ele) {
                if ($.inArray(ele.attribute.videoid, _this.Data_VideoSelectIds) == -1) {
                    ele.methodB1.changeColor({ bgColor: "rgba(41,125,252,0.9)" });
                } else {
                    _list.push(ele.attribute);
                    ele.methodB1.changeColor({ bgColor: "rgba(255,0,0,0.9)" });
                }
            })
            $("#ul_subjectVideoList").L_bindingDataList(_list, {
                itemHandle: function (index, obj, data) {
                    obj.data("VideoData", data);
                }
            });
            _this.ul_subjectVideoList_EventDefine();
        },
        ul_subjectVideoList_EventDefine: function () {
            $("#ul_subjectVideoList li").mouseover(function () {
                var _data = $(this).data("VideoData");
                if (!_data) return;

                var _pos = $(this).offset();
                var _fixedLine = _this.map.addFixed2Line({
                    point: { x: _pos.left - 390, y: _pos.top - 140 },
                    latlng: { lng: _data.videolng, lat: _data.videolat },
                    options: {
                        stroke: true,
                        color: "rgb(0, 0, 254)",
                        weight: 2,
                        opacity: 1,
                        fillOpacity: 1
                    }
                })

                $(this).data("fixedLine", _fixedLine);
            }).mouseleave(function () {
                var _data = $(this).data("VideoData");
                if (!_data) return;

                var _fixLine = $(this).data("fixedLine");
                if (!_fixLine) return;

                _fixLine.l_remove();
            })
        },
        showVideoToMap: function () {
            _this.Data_MarkerList = [];
            _this.Data_MarkerDic = {};
            _this.Data_VideoIds = [];
            $.each(_this.Data_MapVideoList, function (index, ele) {
                var _mark = _this.map.addCssMarker(ele.videolng, ele.videolat, {
                    option: {
                        title: ele.videlname
                    },
                    type: "B1",
                    param_B1: {
                        height: 18,
                        width: 18,
                        bgColor: "rgba(41,125,252,0.9)",
                        bgHoverColor: "rgba(255,106,0,0.9)"
                    },
                    //popupContent: _this.createVideoPopupContent(ele),
                    //popupOption: {
                    //    maxWidth: 1000
                    //},
                    attribute: ele,
                    event: {
                        mouseover: function (evt) {
                            var _m = evt.target;
                            var _lnglat = _m.getLatLng();
                            _m._t_label = _this.map.addLabel(_lnglat.lng, _lnglat.lat, _m.attribute.videoname, {
                                anchor: [15, -15],
                                bgColor: "rgba(255, 120, 24, 0.9)"
                            });
                        },
                        mouseout: function (evt) {
                            var _m = evt.target;
                            _m._t_label.m_removeFromMap();
                        }
                    }
                });
                _this.Data_MarkerList.push(_mark);
                _this.Data_MarkerDic["videoid_" + ele.videoid] = _mark;
                _this.Data_VideoIds.push(ele.videoid);
            })
        },
        showSubjectInfo: function (subjectObj) {

            $("#subject_page_frame").L_bindingData(subjectObj);

            _this.ajax_GetVideoListBySubjectId(subjectObj.subjectid);
        }
    })
})(jQuery);

//(function () {
//    var _this = $.tv_m;
//    $.extend($.tv_m, {
//    })
//})(jQuery);

//(function () {
//    var _this = $.tv_m;
//    $.extend($.tv_m, {
//    })
//})(jQuery);