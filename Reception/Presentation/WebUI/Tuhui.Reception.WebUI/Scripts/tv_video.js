// 已经过时
$.tv = {};
(function () {
    var _this = $.tv;
    $.extend($.tv, {
        mapInit: function () {
            this.map = $.tMap.init("map", {
                defaultLayer: function () {
                    return L.tileLayer('{url}', {
                        maxZoom: 14,
                        minZoom: 0,
                        zoomOffset: 3,
                        continuousWorld: true,
                        url: function (data) {
                            return _this.get_map_url(data);
                        }
                    })
                },
                lng: 118.79688262939452,
                lat: 32.04620586520412,
                zoom:8,
                m_param: {
                    zoomControl: true,
                    crs: L.extend({}, L.CRS, {
                        projection: L.Projection.LonLat,
                        transformation: new L.Transformation(1, 180, -1, 90),
                        scale: function (zoom) {
                            var scales = [1 / 40960000, 1 / 20480000, 1 / 10240000, 1 / 5120000, 1 / 2560000, 1 / 1280000, 1 / 640000, 1 / 320000, 1 / 160000, 1 / 80000, 1 / 40000, 1 / 20000, 1 / 10000, 1 / 5000, 1 / 2500];
                            var resolutions = [];
                            var _DPI = 96;
                            for (var i = 0; i < scales.length; i++) {
                                resolutions[i] = 1 / scales[i] * 0.0254 / _DPI * (360 / 2 / Math.PI / 6378137);//不同比例尺下每像素代表多少度
                            }
                            return 1 / resolutions[zoom];
                        }
                    })
                }
            })

            $("#video_menu").L_bindingDataList(SUBJECT_LIST);
            this.menu_bindingEvent();
        },
        get_map_url:function(data) {
            var _mapscales = [40960000, 20480000, 10240000, 5120000, 2560000, 1280000, 640000, 320000, 160000, 80000, 40000, 20000, 10000, 5000, 2500];
            var url = "http://218.2.208.140:8092/Super_Map/JC_Map?level={level}&x={tileX}&y={tileY}";
            url = L.Util.template(url, {
                level: _mapscales[data.z - data.zoomOffset],
                tileX: data.y,
                tileY: data.x
            });
            return url;
        },
        dataInit: function () {
            this.VIDEOLIST = DATALIST;
            this.showResultToPage(this.VIDEOLIST);
        },
        menuInit: function () {

        },
        showResultToPage: function (result) {
            this.showResultToList(result);
            this.showResultToMap(result);
        },
        showResultToList: function (result) {
            $("#ul_video_list").L_bindingDataList(result, {
                itemHandle: function (index, obj, data) {
                    obj.data("VideoData", data);
                }
            });
            this.list_hoverEvent();
        },
        showResultToMap: function (result) {
            this.map.clear();
            var _markList = [];
            $.each(result, function (index, ele) {
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
                    type: "A1",
                    param_A1: {
                        height: 30,
                        width: 30,
                        bgColor: ele.videosource == 20 ? "rgba(41,125,252,0.9)" : "rgba(255,126,83,0.9)",
                        bgHoverColor: "rgba(255,106,0,0.9)",
                        iconUrl: CONTENT_URL("images/mapIcon/icon_101.png")
                    },
                    addToMapFlag: false,
                    popupContent:_this.createVideoPopupContent(ele),
                    popupOption: {
                        maxWidth: 1000
                    },
                    attribute:ele,
                    event: {
                        mouseover: function (evt) {
                            var _m = evt.target;
                            var _lnglat = _m.getLatLng();
                            _m._t_label = _this.map.addLabel(_lnglat.lng, _lnglat.lat, _m.attribute.videoname, {
                                anchor: [30, -40],
                                bgColor: "rgba(255, 120, 24, 0.9)"
                            });
                        },
                        mouseout: function (evt) {
                            var _m = evt.target;
                            _m._t_label.m_removeFromMap();
                        }
                    }
                });

                _markList.push(_mark);

                //PT_MARKER_DIC["resId_" + ele.videoid] = _mark;
            })

            _this.map.addClusterMarkers(_markList, {
                tIcon: {
                    type: 1,
                    colorType: 1,
                    iconUrl: CONTENT_URL("images/mapIcon/icon_101.png")
                }
            });

        },
        takeVideoListFromSubject: function (_values) {
            if (_values.length == 0) {
                _this.map.resetView();
                _this.map.clear();
                //setTimeout(function () {
                //    _this.showResultToPage(_this.VIDEOLIST);
                //},300)
                return;
            }

            var _s_ids = [];
            var f_subject = null;
            var a_subjects = [];
            for (var i = 0; i < _values.length; i++) {
                var _value = _values[i];
                var _subject = this.SubjectListDic["subject_" + _value];
                if (i == 0) f_subject = _subject;
                a_subjects.push(_subject);
                if (_subject.list) {
                    for (var j = 0; j < _subject.list.length; j++) {
                        if ($.inArray(_subject.list[j],_s_ids)==-1){
                            _s_ids.push(_subject.list[j]);
                        }
                    }
                }
            }

            if (_values.length == 1) {
                _this.map.setView(f_subject.centerLng, f_subject.centerLat, f_subject.centerZoom);
            } else {
                _this.map.fitBounds($.map(a_subjects, function (a) { return {lng:a.centerLng,lat:a.centerLat}}))
            }


            this.showResultToPage($.grep(this.VIDEOLIST, function (a) {
                return $.inArray(a.videoid, _s_ids) != -1;
            }));

        },
        menu_bindingEvent: function () {
            this.SubjectListDic = {};

            $.each(SUBJECT_LIST, function (index, ele) {
                $.each(ele.list, function (index2, ele2) {
                    _this.SubjectListDic["subject_" + ele2.id] = ele2;
                })
            })


            $("#video_menu").find("tbody").hide();
            $("#video_menu").find("tbody").eq(0).show();

            $("#video_menu").find("thead .thead_title").click(function () {
                $("#tbody_subject1_" + $(this).attr("t_id")).toggle();
            })

            $("#video_menu").find("input[name='subjectChkItem']").click(function () {
                var _values = [];
                $("#video_menu").find("input[name='subjectChkItem']").filter(":checked").each(function(){
                    _values.push($(this).val());
                })
                _this.takeVideoListFromSubject(_values);
            })

        },
        list_hoverEvent: function () {
            $("#ul_video_list li").on("mouseover", function () {
                var _data = $(this).data("VideoData");
                if (!_data) return;

                var _pos = $(this).offset();

                var _fixedLine = _this.map.addFixed2Line({
                    point: { x: _pos.left+280, y: _pos.top-30 },
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
        },
        createVideoPopupContent: function (ele) {
            if (ele.videosource == 20) {
                return "<iframe style='width:500px;height:375px;' src='" + ACTION_URL("YCVLC", "Video") + "?c_id=" + ele.videoaccess + "'></iframe>";
            } else {
                return "<iframe style='width:500px;height:375px;' src='" + ele.videoaccess + "'></iframe>";
            }
        }
    })
})(jQuery);