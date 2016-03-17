// 地图视频展示页面
$.tv_f = {};
(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {
        Data_AllVideoList: [],
        Data_AllVideoDic: {},
        Data_MapVideoList: [],
        Data_SubjectList: [],
        Data_SubjectTreeData: [],
        Data_SubjectDic: {},
        Data_MarkerList: [],
        Data_MarkerDic: {},
        Data_LinePopupList: []
    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {
        tmap_Init: function () {
            this.map = $.tMap.init("map", {
                defaultLayer: "autonavi",
                lng: 118.7464141845703,
                lat: 32.026415223498915,
                zoom: 11,
                m_param: {
                    zoomControl: true
                }
            })
            this.linePopup_Init();

            this.map.on("contextmenu", function (evt) {
                if (_this.map.contextmenuMarker) {
                    _this.map.contextmenuMarker.m_removeFromMap();
                    _this.map.contextmenuMarker = null;
                }
                _this.map.contextmenuMarker = _this.map.addCssMarker(evt.latlng.lng, evt.latlng.lat, {
                    option: {
                        zIndexOffset: 10000
                    },
                    type: "C1",
                    param_C1: {
                        color: "#000",
                        width: 36,
                        text: $("#div_mapcontextMenu").html()
                    }
                });
            }).on("preclick", function (evt) {
                if (_this.map.contextmenuMarker) {
                    _this.map.contextmenuMarker.m_removeFromMap();
                    _this.map.contextmenuMarker = null;
                }
            })
        },
        linePopup_Init: function () {
            var _p_width = $.config.fixPopup.width;
            var _p_height = $.config.fixPopup.height;

            var _screenWidth = document.documentElement.clientWidth - 280;
            var _screenHeight = document.documentElement.clientHeight;

            _this.Data_LinePopupList = [];

            var _last_X = 0;

            for (var i = 0; i < 20; i++) {
                if (100 + (i + 1) * _p_width > _screenWidth) continue;
                var _x = 100 + (i + 1) * (_p_width + 5);
                var _y = 5 + _p_height;
                _this.Data_LinePopupList.push({
                    x: _x,
                    y: _y,
                    anchor: [_p_width, _p_height]
                });
                _last_X = _x
            }

            for (var i = 1; i < 10; i++) {
                if (5 + $(".video_head").height() + (i + 1) * _p_height > _screenHeight) continue;
                var _x = _last_X - _p_width;
                var _y = 5 + (i + 1) * (_p_height + 5);
                _this.Data_LinePopupList.push({
                    x: _x,
                    y: _y,
                    anchor: [0, _p_height]
                });
            }


            $.each(_this.Data_LinePopupList, function (index, ele) {

                $.extend(ele, {
                    markId: null,
                    fixedMarker: null
                })

                //$("body").append("<div style='z-index:1000;position:absolute;height:200px;width:200px;background-color:rgba(255,0,0,0.5);top:" + ele.y + "px;left:" + ele.x + "px;'></div>")
            })
        },
        linePopup_getPosition: function () {
            for (var i = 0; i < _this.Data_LinePopupList.length; i++) {
                if (_this.Data_LinePopupList[i].fixedMarker == null) return _this.Data_LinePopupList[i];
            }
            return null;
        },
        checkLinePopupExsixt: function (videoid) {
            for (var i = 0; i < _this.Data_LinePopupList.length; i++) {
                if (_this.Data_LinePopupList[i].markId == videoid) return true;
            }
            return false;
        },
        showFixdPopup: function (mark) {
            if (_this.checkLinePopupExsixt(mark.attribute.videoid)) {
                _this.closeFixdPopup(mark.attribute.videoid);
                return;
            }
            $(".video_search_panel").hide();
            var _container = this.linePopup_getPosition();
            if (_container == null) return "full";

            _container.markId = mark.attribute.videoid;

            _container.fixedMarker = _this.map.addFixedMarker({
                point: { x: _container.x, y: _container.y },
                html: "<div style='height:" + $.config.fixPopup.height + "px;width:" + $.config.fixPopup.width + "px;'>" + createHtml(mark.attribute) + "</div>",
                size: [$.config.fixPopup.width, $.config.fixPopup.height],
                anchor: _container.anchor,
                option: {
                    //clickable:true,
                    draggable: true,
                    //keyboard:true,
                    //title:"",
                    //alt:"",
                    zIndexOffset: 100000
                    //opacity:1.0,
                    //riseOnHover:false,
                    //riseOffset:250
                },
                lineOption: {
                    stroke: true,
                    color: "rgb(254,119,22)",
                    weight: 2,
                    opacity: 1,
                    fillOpacity: 1
                },
                fixedLngLat: mark.getLatLng()
            })

            var line = _this.map.addFixedMarker({
                point: { x: _container.x , y: _container.y  },
                //html: "<div style='height:" + $.config.fixPopup.height + "px;width:" + $.config.fixPopup.width + "px;'>" + createHtml(mark.attribute) + "</div>",
                size: [$.config.fixPopup.width, $.config.fixPopup.height],
                anchor: _container.anchor,
                option: {
                    //clickable:true,
                    draggable: true,
                    //keyboard:true,
                    //title:"",
                    //alt:"",
                    zIndexOffset: 100000
                    //opacity:1.0,
                    //riseOnHover:false,
                    //riseOffset:250
                },
                lineOption: {
                    stroke: true,
                    color: "rgb(254,19,212)",
                    weight: 2,
                    opacity: 1,
                    fillOpacity: 1
                },
                fixedLngLat: mark.getLatLng()
            })

            line.m_removeFromMap();

            _container.close = function () {
                if (_container.fixedMarker) {
                    _container.fixedMarker.m_removeFromMap();
                    _container.markId = null;
                    _container.fixedMarker = null;
                }
            }



            function createHtml(ele) {
                var _html = "<div class='linePopup_title'>" + ele.videoname + "<span onclick='$.tv_f.closeFixdPopup(" + ele.videoid + ")'>x</span></div>";

                var _src = ACTION_URL("PlayVideo", "Video") + "?videoid=" + ele.videoid;

                //if (ele.videosource == 20) { 
                //    _src = ACTION_URL("YCVLC", "Video") + "?c_id=" + encodeURIComponent(ele.videoaccess);
                //} else if (ele.videosource == 30) {
                //    _src = ACTION_URL("STVIDEO2", "Video") + "?c_id=" + ele.thirdid;
                //} else {
                //    if (ele.videoaccess.indexOf("rtmp") != -1) {
                //        _src = ACTION_URL("YCVLC", "Video") + "?c_id=" + encodeURIComponent(ele.videoaccess);
                //    }else{
                //        _src = ele.videoaccess;
                //    }
                //}

                _html += "<iframe style='width:100%;height:" + ($.config.fixPopup.height - 30) + "px;' src='" + _src + "'></iframe>";
                return _html;
            }

        },
        closeFixdPopup: function (id) {
            debugger;
            var _state = 0;
            for (var i = 0; i < _this.Data_LinePopupList.length; i++) {
                var item = _this.Data_LinePopupList[i];
                if (id == null || item.markId == id) {
                    item.close();
                }
                if (item.markId != null) _state++;
            }
            if (_state == 0) {
                $(".video_search_panel").show();
            }
        },
        searchByDrawLine: function () {
            setTimeout(_drawLine, 300)

            function _drawLine() {
                _this.map.drawLine({
                    begin: function () {
                        //showNotify("drawLine has begin!");
                    },
                    complete: function (result) {
                        $(".subject_menu").find("ul.sub_menu").hide();
                        $(".subject_menu").find(".t_allvideo").hide();
                        $(".subject_menu").find("p.chk_item_icon").removeClass("chk_item_icon_open");
                        if ($("#page_panel1").attr("state") == "0") $("#page_panel1").click();

                        _this.showResultToPage($.grep(_this.Data_MapVideoList, function (a) { return _isARoundLine(a) }));

                        function _isARoundLine(ele) {
                            for (var i = 0; i < result.length; i++) {
                                if (_this.map.getDistance(ele.videolng, ele.videolat, result[i].lng, result[i].lat) < 1000) {
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                })
            }

        },
        searchByDrawCircle: function () {
            this.map.drawCircle({
                begin: function () {
                    //showNotify("drawCircle has begin!");
                },
                complete: function (result) {
                    $(".subject_menu").find("ul.sub_menu").hide();
                    $(".subject_menu").find(".t_allvideo").hide();
                    $(".subject_menu").find("p.chk_item_icon").removeClass("chk_item_icon_open");
                    if ($("#page_panel1").attr("state") == "0") $("#page_panel1").click();
                    _this.showResultToPage($.grep(_this.Data_MapVideoList, function (a) { return _this.map.getDistance(a.videolng, a.videolat, result.lng, result.lat) < result.radius }));
                }
            })

        },
        searchByDrawRect: function () {
            this.map.drawRectangle({
                begin: function () {
                    //showNotify("drawRect has begin!");
                },
                complete: function (result) {
                    $(".subject_menu").find("ul.sub_menu").hide();
                    $(".subject_menu").find(".t_allvideo").hide();
                    $(".subject_menu").find("p.chk_item_icon").removeClass("chk_item_icon_open");
                    if ($("#page_panel1").attr("state") == "0") $("#page_panel1").click();

                    _this.showResultToPage($.grep(_this.Data_MapVideoList, function (a) { return _this.map.isInRect(a.videolng, a.videolat, result.start, result.end) }));
                }
            })

        }
    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {

    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {

    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {
        subject_Menu_EventDefine: function () {
            //$(".subject_menu a.level1").click(function () {
            //    $(this).siblings("ul.sub_menu").slideToggle();
            //});

            setTimeout(function () {
                $(".subject_menu2 a").eq(0).click();
            }, 300)

            $(".subject_menu2 a[isleaf='1']").click(function () {
                $(this).siblings(".chk_item").click();
            })

            $(".subject_menu2 .chk_item").click(function () {
                if ($(this).prop("checked")) {
                    $(this).siblings("p.chk_item_icon").addClass("chk_item_icon_open");
                    $(this).siblings(".t_allvideo").show();
                } else {
                    $(this).siblings("p.chk_item_icon").removeClass("chk_item_icon_open");
                    $(this).siblings(".t_allvideo").hide();
                }
                var _v = $.map($(".subject_menu2").find(".chk_item").filter(":checked"), function (a) { return $(a).val() });
                _this.dealSubjectChkChange(_v)
            })

        },
        videoList_HoverEventDefine: function () {
            $("#ul_video_list li").on("mouseover", function () {
                var _data = $(this).data("VideoData");
                if (!_data) return;

                var _pos = $(this).offset();

                var _fixLine = $(this).data("fixedLine");
                if (_fixLine) _fixLine.l_remove();

                _fixedLine = _this.map.addFixed2Line({
                    point: { x: _pos.left + 280, y: _pos.top - 30 },
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

            }).on("mouseleave", function () {
                var _data = $(this).data("VideoData");
                if (!_data) return;

                var _fixLine = $(this).data("fixedLine");
                if (!_fixLine) return;
                _fixLine.l_remove();


            }).on("click", function () {
                var _data = $(this).data("VideoData");
                if (!_data) return;

                _this.map.setView(_data.videolng, _data.videolat);
                setTimeout(function () {
                    var _zoom = _this.map.getZoom();
                    if (_zoom < 16) {
                        _this.map.zoomIn();
                    }
                }, 500)

            })
        }
    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {
        dealSubjectChkChange: function (_values) {
            if (_values.length == 0) {
                _this.map.resetView();
                _this.map.clear();
                $("#ul_video_list").html('');
                setTimeout(function () {
                    _this.showResultToPage(_this.Data_MapVideoList);
                }, 300)
                return;
            }

            var _s_ids = [];
            var f_subject = null;
            var a_subjects = [];
            for (var i = 0; i < _values.length; i++) {
                var _value = _values[i];
                var _subject = this.Data_SubjectDic["subject_" + _value];
                if (i == 0) f_subject = _subject;
                a_subjects.push(_subject);
                if (_subject.videoIds) {
                    for (var j = 0; j < _subject.videoIds.length; j++) {
                        if ($.inArray(_subject.videoIds[j], _s_ids) == -1) {
                            _s_ids.push(_subject.videoIds[j]);
                        }
                    }
                }
            }

            if (_values.length == 1) {
                _this.map.setView(f_subject.centerlng, f_subject.centerlat, f_subject.centerzoom);
            } else {
                _this.map.fitBounds($.map(a_subjects, function (a) { return { lng: a.centerlng, lat: a.centerlat } }))
            }

            var lineList = [];
            $.each(a_subjects, function (index, ele) {
                if (ele.lineData) lineList.push(ele.lineData);
            })

            this.showResultToPage($.grep(_this.Data_MapVideoList, function (a) {
                return $.inArray(a.videoid, _s_ids) != -1;
            }), lineList);
        },
        openSubjectVideos: function (obj) {
            var subjectid = $(obj).attr("subjectid");
            var _subject = this.Data_SubjectDic["subject_" + subjectid];

            if (!$(obj).hasClass("t_allvideo_open")) {
                $(obj).addClass("t_allvideo_open");

                _this.map.setView(_subject.centerlng, _subject.centerlat, _subject.centerzoom);

                if (_subject.videoIds.length != 0)
                    _show(0);

            } else {
                $(obj).removeClass("t_allvideo_open");
                for (var j = 0; j < _subject.videoIds.length; j++) {

                    _this.closeFixdPopup(_subject.videoIds[j]);

                }
            }

            function _show(index) {

                if (index == _subject.videoIds.length) return;

                var _mark = _this.Data_MarkerDic["videoid_" + _subject.videoIds[index]];

                if (_this.showFixdPopup(_mark) == "full") return;

                setTimeout(function () {
                    _show(index + 1);
                }, 100);

            }

        }
    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {
        showAllResultToPage: function () {
            _this.showResultToPage(_this.Data_MapVideoList);
        },
        searchVideoList: function () {
            var _keyword = $.trim($("#txtVideoSearchKey").val());
            if (_keyword == "") {
                _this.showAllResultToPage()
            } else {
                _this.showResultToPage($.grep(_this.Data_MapVideoList, function (a) { return a.videoname.indexOf(_keyword) != -1 }));
            }
        },
        showResultToPage: function (dataList, lineList) {

            $("#ul_video_list").L_bindingDataList(dataList, {
                itemHandle: function (index, obj, data) {
                    obj.data("VideoData", data);
                }
            });

            this.videoList_HoverEventDefine();

            this.showVideoToMap(dataList, lineList);
        },
        showVideoToMap: function (dataList, lineList) {
            _this.Data_MarkerList = [];
            _this.Data_MarkerDic = {};
            _this.Data_VideoIds = [];
            this.map.clear();
            $.each(dataList, function (index, ele) {
                var _mark = _this.map.addCssMarker(ele.videolng, ele.videolat, {
                    option: {
                        title: ele.videlname
                    },
                    tIcon: {
                        type: 1,
                        colorType: 1,
                        iconUrl: CONTENT_URL("images/mapIcon/icon_101.png"),
                        iconFont: "视",
                        iconTitle: ele.videoname
                    },
                    // TODO ligenhua
                    //type: "A1",
                    //param_A1: {
                    //    height: 30,
                    //    width: 30,
                    //    bgColor: ele.videosource == 20 ? "rgba(41,125,252,0.9)" : "rgba(255,126,83,0.9)",
                    //    bgHoverColor: "rgba(255,106,0,0.9)",
                    //    iconUrl: CONTENT_URL("images/mapIcon/icon_101.png")
                    //},
                    type: "C1",
                    param_C1: {
                        color: "#000",
                        width: 36,
                        text: _this.createVideoMarkerC1Html(ele)
                    },
                    addToMapFlag: false,
                    //popupContent: _this.createVideoPopupContent(ele),
                    //popupOption: {
                    //    maxWidth: 1000
                    //},
                    attribute: ele,
                    event: {
                        click: function (evt) {
                            _this.showFixdPopup(evt.target);
                        },
                        mouseover: function (evt) {
                            var _m = evt.target;
                            var _lnglat = _m.getLatLng();
                            _m._t_label = _this.map.addLabel(_lnglat.lng, _lnglat.lat, _m.attribute.videoname, {
                                // TODO ligenhua
                                //anchor: [30, -40],
                                anchor: [30, -15],
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
            // TODO ligenhua
            //_this.map.addClusterMarkers(_this.Data_MarkerList, {
            //    tIcon: {
            //        type: 1,
            //        colorType: 1,
            //        iconUrl: CONTENT_URL("images/mapIcon/icon_101.png")
            //    }
            //});

            _this.map.addClusterMarkers(_this.Data_MarkerList, {
                iconCreateFunction: function (cluster) {
                    return L.divIcon({ html: _this.createVideoMarkerClusterHtml(cluster.getAllChildMarkers().length), className: "video_marker", iconSize: L.point(36, 36), iconAnchor: [18, 18] });
                }
            });

            if (lineList && lineList.length != 0) {
                $.each(lineList, function (index, ele) {
                    _this.map.addLine(ele, {
                        options: {
                            stroke: true,
                            color: "rgb(0,0,255)",
                            weight: 12,
                            opacity: 0.8,
                            fillOpacity: 1
                        },
                        dash: "10,10"
                    })
                })
            }

        },
        createVideoMarkerC1Html: function (ele) {
            var _html = "";
            _html += '<div class="video_marker">';
            if (ele.videostatus == 0) {
                _html += '<div class="v_disabled"></div>'
            }
            _html += '<img src="' + CONTENT_URL("images/mapIcon/icon_" + ele.videosource + ".png") + '" />'
            _html += '</div>';
            return _html;
        },
        createVideoMarkerClusterHtml: function (_count) {
            return '<img src="' + CONTENT_URL("images/mapIcon/icon_video.png") + '" />'
            + '<span class="clusterCount">' + _count + '</span>';
        },
        createVideoPopupContent: function (ele) {
            return "<iframe style='width:500px;height:375px;' src='" + ACTION_URL("PlayVideo", "Video") + "?videoid=" + ele.videoid; + "'></iframe>";

            //if (ele.videosource == 20) {
            //    return "<iframe style='width:500px;height:375px;' src='" + ACTION_URL("YCVLC", "Video") + "?c_id=" + encodeURIComponent(ele.videoaccess); + "'></iframe>";
            //} else if (ele.videosource == 30) {
            //    return "<iframe style='width:500px;height:375px;' src='" + ACTION_URL("YCVLC1", "Video") + "?c_id=" + ele.videoaccess + "'></iframe>";
            //} else {

            //    if (ele.videoaccess.indexOf("rtmp") != -1) {
            //        return "<iframe style='width:500px;height:375px;' src='" + ACTION_URL("YCVLC", "Video") + "?c_id=" + encodeURIComponent(ele.videoaccess); + "'></iframe>";
            //    } else {
            //        return "<iframe style='width:500px;height:375px;' src='" + ele.videoaccess + "'></iframe>";
            //    }
            //}
        }
    })
})(jQuery);

(function () {
    var _this = $.tv_f;
    $.extend($.tv_f, {
        ajax_GetSubjectList: function () {
            $.L.ajaxHandle({
                url: ACTION_URL("GetAllSubjects", "Manager"),
                loading: 1,
                loadingContainer: $("#map"),
                data: { containVideoIds: true },
                success: function (response) {
                    //if ($.config.subjectid) {
                    //    _this.Data_SubjectList = $.grep(response, function (a) { return a.subjectid == $.config.subjectid || a.parentid == $.config.subjectid });
                    //    if (_this.Data_SubjectList.length == 0)
                    //        _this.Data_SubjectList = response;
                    //} else {
                    //    _this.Data_SubjectList = response;
                    //}
                    _this.Data_SubjectList = response;

                    _this.Data_SubjectTreeData = [];

                    $.each(_this.Data_SubjectList, function (index, ele) {
                        _this.Data_SubjectDic["subject_" + ele.subjectid] = ele;
                        if (ele.parentid == null) {
                            _this.Data_SubjectTreeData.push(ele);
                        }
                    })

                    $.each(_this.Data_SubjectTreeData, function (index, ele) {
                        ele.sub_subject = $.grep(_this.Data_SubjectList, function (a) {
                            return a.parentid == ele.subjectid;
                        })
                    })

                    //$("#subject_menu").L_bindingDataList(_this.Data_SubjectTreeData);

                    _this.subject_Menu_EventDefine();

                    //$("#subject_menu .sub_menu").hide();

                    setTimeout(function () {
                        _this.ajax_GetAllVideoList();
                    }, 300)
                }
            })
        },
        ajax_GetAllVideoList: function () {
            $.L.ajaxHandle({
                url: ACTION_URL("GetAllVideo", "Manager"),
                loading: 1,
                loadingContainer: $("#map"),
                data: { pageSize: 100000, justEnable: 1 },
                success: function (response) {
                    _this.Data_AllVideoList = response.PageData;
                    $.each(_this.Data_AllVideoList, function (index, ele) {
                        _this.Data_AllVideoDic['video_' + ele.videoid] = ele;
                    })

                    var _subjectids = $.map($(".subject_menu2 a[isleaf='1']"), function (a) {
                        return $(a).attr('subjectid');
                    })

                    var recordVideoIds = [];

                    for (var i = 0; i < _subjectids.length; i++) {
                        var _s_o = _this.Data_SubjectDic["subject_" + _subjectids[i]];
                        if (_s_o.videoIds) {
                            $.each(_s_o.videoIds, function (index, ele) {
                                if ($.inArray(ele, recordVideoIds) == -1) {
                                    if (_this.Data_AllVideoDic['video_' + ele]) {
                                        _this.Data_MapVideoList.push(_this.Data_AllVideoDic['video_' + ele])
                                    }
                                }
                            })
                        }
                    }

                    //alert(_subjectids);

                    //_this.Data_MapVideoList = response.PageData;
                    setTimeout(function () {
                        _this.showResultToPage(_this.Data_MapVideoList);
                    }, 300)
                }
            })
        }
    })
})(jQuery);
