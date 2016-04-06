(function (window) {
    L.Pd2Marker = L.Marker.extend({
        options: {
            p_icon: null,  // 图标
            popupContent: "",  // 弹出框内容
            routes: null,  // 轨迹路劲{list:[],interval:500,speed:10}
            riseOnHover: true
        },
        initialize: function (latlng, options) {
            L.setOptions(this, options);
            this._latlng = L.latLng(latlng);
            if (this.options.p_icon) {
                this.options.icon = this._createDivIcon(this.options.p_icon);
                this._p_icon = this.options.icon;
            }
            if (this.options.popupContent) {
                this.bindPopup(this.options.popupContent, {
                    moveNotUpdateContent: true,
                    maxWidth: 2000
                });
            }
        },
        onAdd: function (map) {
            this._map = map;
            var _thisMark = this;

            map.on('viewreset', this.update, this);

            this._initIcon();
            this.update();

            if (map.options.zoomAnimation && map.options.markerZoomAnimation) {
                map.on('zoomanim', this._animateZoom, this);
            }

            this.on("remove", this._mark_remove)
            //this.on("mouseover", function () {
            //    _thisMark._bringToFront();
            //})
            //this.on("mouseleave", function () {
            //    _thisMark._resetZIndex();
            //})

            if (this.options.routes) {
                var _speed = this.options.routes.speed ? this.options.routes.speed : 10;
                var _routes = this._routesSplit(this.options.routes.list, _speed);
                var _interval = this.options.routes.interval ? this.options.routes.interval : 500;
                var _index = 0
                var _t = this;
                this.routesIntervalTimeId = setInterval(function () {
                    _thisMark.setLatLng(_routes[_index]);
                    if (_t.options.routesHandle) {
                        _t.options.routesHandle(_routes[_index]);
                    }
                    _index++;
                    if (_index == _routes.length) {
                        _index = 0;
                    }
                }, _interval)
            }
        },
        setIconOpacity: function (_opacity) {
            this._icon.style["opacity"] = _opacity;
        },
        showColorIcon: function (type) {
            this.setIcon(this._createDivIcon(this.options.p_icon, type))
        },
        hideColorIcon: function () {
            this.setIcon(this._createDivIcon(this.options.p_icon, ""))
        },
        showHoverIcon: function (type) {
            if (!type) type = 'mark_hover';
            this.setIcon(this._createDivIcon(this.options.p_icon, type, true))
        },
        hideHoverIcon: function () {
            this.setIcon(this._createDivIcon(this.options.p_icon, ""))
        },
        popupRemove: function () {
            this.unbindPopup();
        },
        popupUpdate: function (_content) {
            this.bindPopup(_content, {
                moveNotUpdateContent: true,
                maxWidth: 2000
            });
        },
        popupRecover: function () {
            this.bindPopup(this.options.popupContent, {
                moveNotUpdateContent: true,
                maxWidth: 2000
            });
        },
        _createDivIcon: function (_icon, iconType, showTitle) {
            if (!iconType) iconType = "mark_blue";
            var _w = _icon.iconSize[0];
            var _h = _icon.iconSize[1];
            var _html = '<img style="width:'+_w+'px;height:'+_h+'px;" src="' + _icon.iconUrl + '"/>';

            if (this.options.title) {
                var _s = ""
                if (showTitle == true) {
                    _s = "display:block;";
                }
                _html += '<div class="hover_title" style="'+_s+'">'
                    + this.options.title + '</div>'
            }

            return L.divIcon({
                className: 'pd_mark_icon2 ' + iconType,
                html: _html,
                iconSize: [_w+6,_h+6],
                iconAnchor: [_w - 10,_h+10]
            })
        },
        _createDivIcon2: function (_icon, iconType, showTitle) {
            if (!iconType) iconType = "";
            var _w = _icon.iconSize[0];
            var _h = _icon.iconSize[1];
            var _html = '<div class="img bg_c' + iconType + ' ' + (this.options.shadow == true ? "shadow" : "") + '" style="height:' + (_h + 4) + 'px;width:' + (_w + 4) + 'px">'
            + '<img src="' + _icon.iconUrl + '" style="height:' + (_h) + 'px;width:' + (_w) + 'px" />'
                + '</div><div class="dir"><span class="bg_c' + iconType + '"></span></div>';
            if (this.options.title) {
                var _s = ""
                if (showTitle == true) {
                    _s = "display:block;";
                }
                _html += '<div class="hover_title" style="' + _s + 'top:-' + (_h + 10) + 'px;left:' + (_w + 15) + 'px;"><span class="t_dir bg_chover"></span><div class="t_content bg_chover">'
                    + this.options.title + '</div></div>'
            }
            return L.divIcon({
                className: 'pd_mark_icon',
                html: _html,
                iconSize: [_w + 4, _h + 5],
                iconAnchor: [_w / 2 + 3, _h + 8]
            })
        },
        _routesSplit: function (pointlist, num) {
            //路径数组
            var _distancet, psnum;
            var newpointlist = [];
            //测距
            var point_a = new L.LatLng(pointlist[0].lat, pointlist[0].lng);
            var point_b = new L.LatLng(pointlist[1].lat, pointlist[1].lng);
            _distancet = parseFloat(point_a.distanceTo(point_b));
            //假设传来的基数是100
            psnum = parseInt(_distancet / num);
            for (var i = 0; i < pointlist.length - 1; i++) {
                if (_distancet < 0) newpointlist.push({ lat: parseFloat(pointlist[i + 1].lat), lng: parseFloat(pointlist[i + 1].lng) });
                //分割的段数
                var x0 = parseFloat((pointlist[i + 1].lat - pointlist[i].lat)) / psnum;
                var y0 = parseFloat((pointlist[i + 1].lng - pointlist[i].lng)) / psnum;
                var x1 = parseFloat(pointlist[i].lat), y1 = parseFloat(pointlist[i].lng);
                for (var k = 0; k < psnum - 1; k++) {
                    x1 = parseFloat(x1 + x0); y1 = parseFloat(y1 + y0);
                    newpointlist.push({ lat: x1, lng: y1 });
                }
                newpointlist.push({ lat: parseFloat(pointlist[i + 1].lat), lng: parseFloat(pointlist[i + 1].lng) });
            }

            return newpointlist;
        },
        _mark_remove: function () {
            if (this.routesIntervalTimeId) {
                clearInterval(this.routesIntervalTimeId);
                this.routesIntervalTimeId = null;
            }
        }
    })
    L.pd2Marker = function (latlng, options) {
        return new L.Pd2Marker(latlng, options);
    };

    L.Pd2DraggleMarker = L.Marker.extend({
        options: {
            html: null,  // 内容
            height: 100,
            width: 100,
            c_point:null,
            draggable: true,
            targetMarker: null,
            polyOnAdd: function () { },
            polyOnRemove: function () { },
            polyLineStyle: {
                color: 'blue',
                weight: 2,
                opacity: 0.5
            },
            polyLineStyleHover: {
                color: '#F87112',
                weight: 2,
                opacity: 0.9
            }
        },
        initialize: function (options) {
            L.setOptions(this, options);
            this._latlng = L.latLng(this.options.targetMarker.getLatLng());
            //this._latlng = L.latLng(this.options.targetMarker.res_lng, this.options.targetMarker.res_lat);
            this.options.icon = this._createDivIcon();


        },
        onAdd: function (map) {
            if (this.options.c_point) {
                this._c_point = this.options.c_point;
                var _l = map.containerPointToLatLng(this._c_point);
                this.setLatLng(_l);
            } else {
                this._c_point = map.latLngToContainerPoint(this._latlng);
            }

            this.options.targetMarker.dragMarker = this;
            this.options.targetMarker.dragMarkerFlag = true;

            this._map = map;

            map.on('viewreset', this.update, this);

            this._initIcon();
            this.update();

            if (map.options.zoomAnimation && map.options.markerZoomAnimation) {
                map.on('zoomanim', this._animateZoom, this);
            }

            this.connectionLine = L.polyline([this.getLatLng(),
                this.options.targetMarker.getLatLng()], this.options.polyLineStyle);
            //this._latlng = L.latLng(this.options.targetMarker.res_lng, this.options.targetMarker.res_lat);
            //this.connectionLine = L.polyline([this.getLatLng(),
            //     this._latlng], this.options.polyLineStyle);

            this.options.polyOnAdd(this.connectionLine);

            this.on("drag", function () {
                this._c_point = this._map.latLngToContainerPoint(this.getLatLng());

                this._setPolyLineLnglat();
            });

            var _t = this;

            this.options.targetMarker.on("move", function () {
                _t._setPolyLineLnglat();
            })

            this._map.on("move", this._mapMove, this);

            this.on("mouseover", function () {
                this._polyLine_hover();
                this.options.targetMarker.showHoverIcon();
            })
            this.on("mouseout", function () {
                this._polyLine_blur();
                this.options.targetMarker.hideHoverIcon();
            })

            this.on("remove", function () {
                if (this.options.onremove) {
                    this.options.onremove();
                }
            })

        },
        onPolyRemove: function () {
            this.options.polyOnRemove(this.connectionLine);
            this.options.targetMarker.dragMarkerFlag = false;
        },
        flyToPosition: function (point) {
            this._icon.style.transition = "all linear 1s";
            this._c_point = point;
            this._mapMove();
            var _t = this;
            setTimeout(function () {
                _t._icon.style.transition = "none";
            }, 1000)
        },
        _polyLine_hover: function () {
            this.connectionLine.setStyle(this.options.polyLineStyleHover)
        },
        _polyLine_blur: function () {
            this.connectionLine.setStyle(this.options.polyLineStyle)
        },
        _setPolyLineLnglat: function () {
            this.connectionLine.setLatLngs([this.getLatLng(), this.options.targetMarker.getLatLng()]);
            //this._latlng = L.latLng(this.options.targetMarker.res_lng, this.options.targetMarker.res_lat);
            //this.connectionLine.setLatLngs([this.getLatLng(), this._latlng]);
            
        },
        _mapMove: function () {
            if (!this._map) return;
            var _latlng = this._map.containerPointToLatLng(this._c_point);
            this.setLatLng(_latlng);
             this.connectionLine.setLatLngs([_latlng, this.options.targetMarker.getLatLng()]);
            //this._latlng = L.latLng(this.options.targetMarker.res_lng, this.options.targetMarker.res_lat);
            //this.connectionLine.setLatLngs([_latlng, this._latlng]);
        },
        _createDivIcon: function () {

            return L.divIcon({
                className: 'pd_mark_dragmarker',
                html: this.options.html,
                iconSize: [this.options.width, this.options.height],
                iconAnchor: [this.options.width, this.options.height]
            })
        }
    })
    L.pd2DraggleMarker = function (latlng, options) {
        return new L.Pd2DraggleMarker(latlng, options);
    };
})(window);

function PD2_tMAP_extension(_map) {
    $.extend(_map, {
        addpd2Mark: function (lng, lat, param) {
            if (param && typeof param == 'string') {
                param = eval("(" + param + ")");
            }
            var defaultParam = {
                icon: {
                    iconUrl: $.tMap.SitePath + '/tmap2.0/images/marker-icon.png', // 图标图片
                    iconSize: [25, 41]
                },
                hover_icon: null,
                hover_icon_func: function () { return true },
                popupContent: null,  // 弹窗内容
                autoOpen: false,
                title: null,
                addOverlay: true,
                openpopup_hovericon: false,
                click: function () { },
                dbclick: function () { },
                mouseover: function () { },
                mouseout: function () { },
                remove: function () { },
                popupopen: function () { },
                popupclose: function () { },
                groupname: ''
            };
            $.extend(defaultParam, param);

            var _marker = L.pd2Marker([lat, lng], defaultParam.options);

            _marker.attribute = defaultParam.attribute;

            if (defaultParam.popupContent) {
                _marker.bindPopup(defaultParam.popupContent, {
                    moveNotUpdateContent: true,
                    maxWidth: 2000
                });
            }

            _marker.popupState = false;
            // 注册事件
            var _event_types = ["click", "dbclick", "mouseover", "mouseout", "remove", "popupopen", "popupclose"];
            $.each(_event_types, function (index, ele) {
                var _handle = defaultParam[ele];
                if ($.isFunction(_handle)) {
                    _marker.on(ele, function (e) {
                        _handle(e, _marker);
                    })
                }
            })

            if (defaultParam.addOverlay) {
                this.addToLayerGroup(_marker, defaultParam.groupname);
            }

            _marker.toLayerGroup = function () {
                map.addToLayerGroup(_marker);
            };

            if (defaultParam.autoOpen) {
                _marker.openWindow();
            }


            var thisMap = this;
            if (defaultParam.contextmenuItems && defaultParam.contextmenuItems.length > 0) {
                var _menuMark = L.marker([0, 0], {
                    icon: thisMap.createContextMenuDivIcon(defaultParam.contextmenuItems, { lng: 0, lat: 0 }, { iconAnchor: [-200, -10] })
                });
                this.addToLayerGroup(_menuMark);
                _marker.on("contextmenu", function (evt) {
                    _menuMark.setIcon(thisMap.createContextMenuDivIcon(defaultParam.contextmenuItems, evt.latlng, { iconAnchor: [-10, 10] }));
                    _menuMark.setLatLng(evt.latlng);
                })
                this.map.on("click", function () {
                    _menuMark.setLatLng([0, 0]);
                })
            }

            return _marker;
        },
        addContextMenu: function (param) {
            var thisMap = this;
            var _menuMark = L.marker([0, 0], {
                icon: thisMap.createContextMenuDivIcon(param, { lng: 0, lat: 0 }),
                zIndexOffset: 300
            }).addTo(this.map);


            this.map.on("contextmenu", function (evt) {
                _menuMark.setIcon(thisMap.createContextMenuDivIcon(param, evt.latlng));
                _menuMark.setLatLng(evt.latlng);
            })

            _menuMark.on("click", function () {
                _menuMark.setLatLng([0, 0]);
            })

            this.map.on("click", function () {
                _menuMark.setLatLng([0, 0]);
            })
        },
        createContextMenuDivIcon: function (menuList, lnglat, iconParam) {
            var _html = "<ul class='tmap_contextmenu pd_bg pd_bg_shadow'>";
            $.each(menuList, function (index, ele) {
                _html += '<li class="pd_bg_hover2"  ';
                if (ele.attr && ele.attr.length != 0) {
                    for (var a in ele.attr) {
                        _html += a + "=" + ele.attr[a] + " ";
                    }
                }
                if (ele.click) {
                    _html += "onclick='" + ele.click + "(this," + lnglat.lng + "," + lnglat.lat + ")'"
                }
                _html += '>' + ele.text + '</li>';
            })
            return L.divIcon($.extend({ html: _html, className: 'tmap_contextmenu_d' }, iconParam));
        }
    })
}
