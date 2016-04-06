(function () {
    var _this = $.case;
    $.extend($.case, {
        videoplayerInit: function () {
            _this.map.map.setZoom(14);
            _this.tmap_circle_click();
        },
        //画圆样式
        _tmapclrcle: null,
        tmap_circle_style: function () {
            _this._tmapclrcle = L.circle([0, 0], 0, {
                stroke: true,
                color: 'blue',
                weight: 1,
                opacity: 0.5,
                fill: true,
                fillColor: null,
                fillOpacity: 0.2
            });
            _this.map.addToLayerGroup(_this._tmapclrcle);
        },

        tmap_circle_click: function (resid) {
            _this.tmap_circle_style();
            _this._tmapclrcle.setLatLng([0, 0]);
            var result = _this.map.drawCircle({
                begin: function () {
                    //_this.map.clear();

                },
                complete: function (e) {
                    result.clear();
                    _this._tmapclrcle.setLatLng([e.lat, e.lng]);
                    _this._tmapclrcle.setRadius(e.radius);
                    _this.tmap_addMark_circle(e.lat, e.lng, e.radius);
                    //打开视频
                    //_this.videoPlayer_Vlc2();

                }
            });

        },
        //res_markList: {},
        tmap_addMark_circle: function (_lat, _lng, _radius) {
            //debugger;
            var Circle_point_video = [];
            $.each(_this.TMAP_RES_SOURCES, function (index, a) {
                if (a.res_type_id == 100) {
                    var point_a = new L.LatLng(a.res_lat, a.res_lng);
                    var point_b = new L.LatLng(_lat, _lng);
                    var _distancet = parseFloat(point_a.distanceTo(point_b));
                    if (_distancet < _radius) {
                        Circle_point_video.push(a);
                        a._latlng = a.res_lat + "," + a.res_lng;
                        //_this.res_markList["mark_" + a.res_id] = a;
                        //console.log(a.res_type_id + "," + a.res_lat + "," + a.res_lng);
                        //_this.map.addMark(a.res_lng, a.res_lat, {
                        //    icon: {
                        //        iconUrl: CONTENT_URL("images/mapicon/icon_100.png"),
                        //        iconAnchor:[24,24]
                        //    }
                        //});
                        //_this.res_markList["mark_" + a.res_id] = a;


      
                        //$.each(resList, function (index, ele) {

                        //})

                    }
                }
            });
            _this.tmap_addMark(Circle_point_video, false);
            $.each(Circle_point_video, function (index, ele) {
                //debugger;
                setTimeout(function () {
                    var _p = _this.map_getDragPoint();
                    if (_p) {
                        _this.videoPlayer_Vlc2(ele.res_id, ele.res_name, ele.res_accessid, {
                            point: _p
                        });
                    }
                }, 1000)
            })


        },
        videoPlayer_Vlc2: function (id, title, accessid, param) {
            var _mk =_this.res_markList["mark_" + id];
            //if (_mk) _mk.closePopup();
            var strurl = "http://www.tuhui.com/pd2/TestYC/IndexYlc3?c_id=" + accessid;
            if (_mk.dragMarkerFlag == true) return;

            var _c_point = null;
            if (param && param.point) _c_point = param.point.point;

            var _d_mk = L.pd2DraggleMarker({
                targetMarker: _mk,
                height: 180,
                width: 200,
                c_point: _c_point,
                polyOnAdd: function (poly) {
                    _this.map.addToLayerGroup(poly);
                },
                polyOnRemove: function (poly) {
                    _this.map.removeFromLayerGroup(poly);
                },
                onremove: function () {
                    if (param && param.point) param.point.mark = null;
                },
                html: "<div class='pd_bg pd_bg_hover2' style='height:30px;line-height:30px;color:#fff;overflow:hidden;'>"
                    + "<span class='pd_win_title'> " + title + "</span>"
                    + "<a style='float:right;color:#fff;margin-right:10px;' onclick='$.case.videoPlayer_Vlc2_dragMarkClose(" + id + ")'>X</a></div><iframe style='height:150px;width:100%;border:none;' src='" + strurl + "'></iframe>"


                //http://www.tuhui.com/pd2" + ACTION_URL("IndexYlc3", "TestYC") + "?c_id=" + accessid + "
            });
            _this.map.addToLayerGroup(_d_mk);
            if (param && param.point) param.point.mark = _d_mk;

            return _d_mk;
        },
        videoPlayer_Vlc2_dragMarkClose: function (id) {
            var _mk = _this.res_markList["mark_" + id];
            if (_mk && _mk.dragMarker) {
                _mk.dragMarker.onPolyRemove();
                setTimeout(function () {
                    _this.map.removeFromLayerGroup(_mk.dragMarker);
                    _mk.hideHoverIcon();
                }, 100)
            }
        },
        map_getDragPointArr: [
            { point: [210, 230], mark: null },
            { point: [420, 230], mark: null },
            { point: [630, 230], mark: null },
            { point: [840, 230], mark: null },
            { point: [1050, 230], mark: null },
            { point: [1260, 230], mark: null },
            { point: [1470, 230], mark: null },
            { point: [210, 430], mark: null },
            { point: [210, 630], mark: null },
            { point: [210, 830], mark: null }
        ],
        map_getDragPointArrInit: function () {
            var _width = document.documentElement.clientWidth - 300;
            var _height = document.documentElement.clientHeight - 100;
            this.map_getDragPointArr = [];
            this.map_getDragPointArr.push({ point: [210, 230], mark: null });
            for (var i = 2; i < 20; i++) {
                if (210 * i < _width) {
                    this.map_getDragPointArr.push({ point: [210*i, 230], mark: null });
                }
            }

            for (var i = 1; i < 20; i++) {
                if (230 + 200 * i < _height) {
                    this.map_getDragPointArr.push({ point: [210, 230 + 200 * i], mark: null });
                }
            }

        },
        map_getDragPoint: function () {
            for (var i = 0; i < this.map_getDragPointArr.length; i++) {
                if (this.map_getDragPointArr[i].mark == null) return this.map_getDragPointArr[i];
            }
            return null;
        },
        //地图显示点
        res_markList: {},
        tmap_addMark: function (res_list, isCluster, param) {
            var _clusterFlag = false;
            //_clusterFlag = res_list.length > 100 ? true : false;
            if (isCluster == true || isCluster == false) _clusterFlag = isCluster

            _this.map.clear();
            //var markList_1 = [];
            //var markList_2 = [];
            //var markList_3 = [];
            _this.res_markList = {};
            $.each(res_list, function (index, ele) {

                //var cluster = CONTENT_URL('/images/Cluster/' + _this.getRandomNum(1, 10) + '.jpg')
                var mark_icon = _this.tmap_getMarkIcon(ele);
                var _contextmenuItems = [];

                //if (ele.res_type_id >= 200 && ele.res_type_id < 300) {
                //    _contextmenuItems = [{
                //        text: '加入语音组',
                //        click: '$.pd2.res_contextmenu_click',
                //        attr: {
                //            action_type: '1',
                //            resid: ele.res_id,
                //            resname: ele.res_name,
                //            restypeid: ele.res_type_id,
                //            accessid: ele.res_accessid
                //        }
                //    }];
                //}
                //else if (ele.res_type_id == 312) {
                //    _contextmenuItems = [{
                //        text: '划定禁行区域',
                //        click: '$.pd2.res_contextmenu_click',
                //        attr: {
                //            action_type: '2',
                //            resid: ele.res_id,
                //            resname: ele.res_name,
                //            restypeid: ele.res_type_id
                //        }
                //    }];
                //}
                // console.log(_contextmenuItems);
                var _mark = _this.map.addpd2Mark(ele.res_lng, ele.res_lat, {
                    options: {
                        title: ele.res_name,
                        p_icon: mark_icon.icon,
                        opacity:1,
                        shadow: false,
                        // routes: _createMarkRoutes(ele)
                    },
                    addOverlay: !_clusterFlag,
                    popupContent:"",
                    openpopup_hovericon: true,
                    hover_icon: mark_icon.hover_icon,
                    hover_icon_func: function () { return !_this.map_ctrl_state; },
                    attribute: {
                        x: ele.res_lng,
                        y: ele.res_lat,
                        id: ele.res_id,
                        name: ele.res_name,
                        typeid: ele.res_type_id,
                        accessid: ele.res_accessid,
                        cluster: {
                            img: "",
                            typeImg: mark_icon.icon.iconUrl,
                            text: ele.res_name,
                            click: ""
                        }
                    },
                    contextmenuItems: _contextmenuItems,
                    click: function (e, m) {
                        if (m.attribute.typeid == 100 && !_this.map_ctrl_state) {
                            _this.videoPlayer_Vlc2(m.attribute.id, m.attribute.name, m.attribute.accessid);
                        }
                        _this.res_group_mark_click(e, m);
                    },
                    mouseover: function (e, m) {
                        //_this.res_group_mark_mouseover(e, m);
                    },
                    mouseout: function (e, m) {
                        //_this.res_group_mark_mouseout(e, m);
                    }
                });

                // _this.bindingDragPopup(_mark, ele);

                //if (ele.res_type_id >= 300) {
                //    markList_3.push(_mark);
                //} else if (ele.res_type_id >= 200) {
                //    markList_2.push(_mark);
                //} else if (ele.res_type_id >= 100) {
                //    markList_1.push(_mark);
                //}

                _this.res_markList["mark_" + ele.res_id] = _mark;
            })

            //if (_clusterFlag) {
            //    _this.map.addMarkerClusterer(markList_1);
            //    _this.map.addMarkerClusterer(markList_2);
            //    _this.map.addMarkerClusterer(markList_3);
            //}

        },
        tmap_getMarkIcon: function (ele) {
            var result = {};
            if (ele.res_type_id == 201 || ele.res_type_id == 202) {
                result.icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_' + ele.res_type_id + '.png'), iconSize: [24, 24] };
                result.hover_icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_200_sel.png'), iconSize: [50, 50] };
            }
                //危化车
            else if (ele.res_type_id == 312) {
                result.icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_truck1.png'), iconSize: [24, 24] };
                result.hover_icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_truck1.png'), iconSize: [50, 50] };
            }
            else if (ele.res_type_id == 301) {
                result.icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_300_car1.png'), iconSize: [24, 24] };
                result.hover_icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_300_car_sel1.png'), iconSize: [50, 50] };
            } else if (ele.res_type_id >= 300) {
                result.icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_300.png'), iconSize: [24, 24] };
                result.hover_icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_300_sel.png'), iconSize: [50, 50] };
            } else {
                result.icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_' + ele.res_type_id + '.png'), iconSize: [24, 24] };
                result.hover_icon = { iconUrl: CONTENT_URL('/images/MapIcon/icon_' + ele.res_type_id + '_sel.png'), iconSize: [50, 50] };
            }
            return result;

        }

    })
})(jQuery);



























//$.each(resList, function (index, ele) {
//    setTimeout(function () {
//        var _p = _this.map_getDragPoint();
//        if (_p) {
//            _this.videoPlayer_Vlc2(ele.res_id, ele.res_name, ele.res_accessid, {
//                point: _p
//            });
//        }
//    }, 1000)
//})



