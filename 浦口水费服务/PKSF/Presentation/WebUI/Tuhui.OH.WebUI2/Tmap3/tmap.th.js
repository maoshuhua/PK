$.tMap = {};
(function () {
    $.extend($.tMap, {
        // 初始化
        init: function (container, param) {
            var defaultParam = {
                defaultLayer: 'th',    // google,baidu,bing,th
                defaultLayerType: '',   // 区别街道图、卫星图等
                lng: 118.79585,
                lat: 32.03762,
                zoom: 13,
                m_param: {}            // 地图初始化参数
            };

            $.extend(defaultParam, param);
            var map = {};

            map.containerId = container;

            var mapParam = {
                attributionControl: false,
                zoomControl: false
            };

            $.extend(mapParam, defaultParam.m_param);


            // 瓦片地图图片来源选择：defaultParam.defaultLayer
            //    th:图慧内部地图(仅仅江苏省区域)
            //    google:谷歌地图
            //    bing:微软bing地图
            //    autonavi:高德地图
            if (typeof (defaultParam.defaultLayer) == "string") {
                var _l_f = $.tMap["tileLayer_" + defaultParam.defaultLayer];

                var _d_l = _l_f(defaultParam.defaultLayerType);

                map.defaultLayer = _d_l;

                if ($.tMap.tileLayer_crs[defaultParam.defaultLayer]) {
                    mapParam.crs = $.tMap.tileLayer_crs[defaultParam.defaultLayer];
                }

            } else if (typeof (defaultParam.defaultLayer) == "function") {

                map.defaultLayer = defaultParam.defaultLayer();
            }

            // 创建地图对象
            map.map = L.map(container, mapParam).setView([defaultParam.lat, defaultParam.lng], defaultParam.zoom);

            if (map.defaultLayer instanceof Array) {
                for (var i = 0; i < map.defaultLayer.length; i++) {
                    map.map.addLayer(map.defaultLayer[i], true);
                }
            } else {
                map.map.addLayer(map.defaultLayer, true);
            }

            map.defaultParam = defaultParam;

            // 地图对象默认Group
            map.defaultLayerGroup = L.layerGroup().addTo(map.map);

            // 地图对象扩展
            _extend();

            return map;

            function _extend() {
                $.extend(map, {
                    // 更换地图基本图层
                    changeTLayer: function (type) {
                        if (this.defaultLayer instanceof Array) {
                            for (var i = 0; i < this.defaultLayer.length; i++) {
                                this.removeLayer(this.defaultLayer[i], true);
                            }
                        } else {
                            this.removeLayer(this.defaultLayer, true);
                        }

                        if (typeof (type) == "string") {

                            var _l_f = $.tMap["tileLayer_" + type];

                            this.defaultLayer = _l_f();

                            if ($.tMap.tileLayer_crs[type]) {
                                map.map.options.crs = $.tMap.tileLayer_crs[type];
                            } else {
                                map.map.options.crs = L.CRS.EPSG3857;
                            }


                        } else if (typeof (type) == "function") {

                            this.defaultLayer = type();
                        }

                        if (this.defaultLayer instanceof Array) {
                            for (var i = 0; i < this.defaultLayer.length; i++) {
                                this.addLayer(this.defaultLayer[i], true);
                            }
                        } else {
                            this.addLayer(this.defaultLayer, true);
                        }
                    },
                    print: function () {
                        $("#" + this.containerId).print();
                    },
                    changeCursor: function (_cursor) {
                        if (!_cursor) _cursor = "pointer";
                        var _container = this.map.getContainer();
                        _container.style.cursor = _cursor;
                    }
                })
                $.extend(map, {
                    // 恢复地图默认中心点和等级
                    resetView: function () {
                        this.map.setView([defaultParam.lat, defaultParam.lng], defaultParam.zoom);
                    },
                    // 设置地图中心点或等级
                    setView: function (lng, lat, zoom, options) {
                        if (!zoom) zoom = this.map.getZoom();
                        this.map.setView([lat, lng], zoom, options);
                    },
                    // 设置地图等级
                    setZoom: function (zoom, options) {
                        this.map.setZoom(zoom, options);
                    },
                    // 地图等级放大
                    zoomIn: function (delta, options) {
                        this.map.zoomIn(delta, options);
                    },
                    // 地图等级缩小
                    zoomOut: function (delta, options) {
                        this.map.zoomOut(delta, options);
                    },
                    setZoomAround: function (lng, lat, zoom, options) {
                        this.map.setZoomAround([lat, lng], zoom, options);
                    },
                    // 将地图根据坐标集合,设置到适合等级和位置
                    fitBounds: function (xys, options) {
                        var latlngs = [];
                        if (typeof xys == 'string') {
                            var _xys = xys.split(';');
                            for (var i = 0; i < _xys.length; i++) {
                                var t = _xys[i].split(',');
                                latlngs.push([parseFloat(t[1]), parseFloat(t[0])]);
                            }
                        } else {
                            latlngs = xys;
                        }
                        this.map.fitBounds(latlngs, options);
                    },
                    fitWorld: function (options) {
                        this.map.fitWorld(options);
                    },
                    panTo: function (lng, lat, options) {
                        this.map.panTo([lat, lng], options);
                    },
                    //panInsideBounds
                    panBy: function (x, y, options) {
                        this.map.panBy([x, y], options);
                    },
                    /*获取地图状态*/
                    getCenter: function () {
                        return this.map.getCenter();
                    },
                    getZoom: function () {
                        return this.map.getZoom();
                    },
                    getMinZoom: function () {
                        return this.map.getMinZoom();
                    },
                    getMaxZoom: function () {
                        return this.map.getMaxZoom();
                    },
                    getBounds: function () {
                        return this.map.getBounds();
                    },
                    getBoundsZoom: function () {
                        return this.map.getBoundsZoom();
                    },
                    getSize: function () {
                        return this.map.getSize();
                    },
                    getPixelBounds: function () {
                        return this.map.getPixelBounds();
                    },
                    getPixelOrigin: function () {
                        return this.map.getPixelOrigin();
                    },
                    getRandomLngLat: function () {
                        var bounds = this.getBounds();
                        var southWest = bounds.getSouthWest();
                        var northEast = bounds.getNorthEast();
                        var lngSpan = northEast.lng - southWest.lng;
                        var latSpan = northEast.lat - southWest.lat;
                        return L.latLng(
					        southWest.lat + latSpan * Math.random(),
					        southWest.lng + lngSpan * Math.random());
                    },
                    GPSToGoogle: function (wgLon, wgLat) {
                        var pi = 3.14159265358979324;
                        var a = 6378245.0;
                        var ee = 0.00669342162296594323;

                        var mgLat, mgLon;

                        if (outOfChina(wgLat, wgLon)) {
                            mgLat = wgLat;
                            mgLon = wgLon;
                        } else {
                            var dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
                            var dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
                            var radLat = wgLat / 180.0 * pi;
                            var magic = Math.sin(radLat);
                            magic = 1 - ee * magic * magic;
                            var sqrtMagic = Math.sqrt(magic);
                            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
                            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.cos(radLat) * pi);
                            mgLat = wgLat + dLat;
                            mgLon = wgLon + dLon;
                        }

                        return {
                            lng: mgLon,
                            lat: mgLat
                        };

                        function outOfChina(lat, lon) {
                            if (lon < 72.004 || lon > 137.8347)
                                return true;
                            if (lat < 0.8293 || lat > 55.8271)
                                return true;
                            return false;
                        }

                        function transformLat(x, y) {
                            var ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.sqrt(Math.abs(x));
                            ret += (20.0 * Math.sin(6.0 * x * pi) + 20.0 * Math.sin(2.0 * x * pi)) * 2.0 / 3.0;
                            ret += (20.0 * Math.sin(y * pi) + 40.0 * Math.sin(y / 3.0 * pi)) * 2.0 / 3.0;
                            ret += (160.0 * Math.sin(y / 12.0 * pi) + 320 * Math.sin(y * pi / 30.0)) * 2.0 / 3.0;
                            return ret;
                        }


                        function transformLon(x, y) {
                            var ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.sqrt(Math.abs(x));
                            ret += (20.0 * Math.sin(6.0 * x * pi) + 20.0 * Math.sin(2.0 * x * pi)) * 2.0 / 3.0;
                            ret += (20.0 * Math.sin(x * pi) + 40.0 * Math.sin(x / 3.0 * pi)) * 2.0 / 3.0;
                            ret += (150.0 * Math.sin(x / 12.0 * pi) + 300.0 * Math.sin(x / 30.0 * pi)) * 2.0 / 3.0;
                            return ret;
                        }
                    },
                    isInRect: function (lng, lat, start, end) {
                        var _minLng = start.lng < end.lng ? start.lng : end.lng;
                        var _maxLng = start.lng < end.lng ? end.lng : start.lng;
                        var _minLat = start.lat < end.lat ? start.lat : end.lat;
                        var _maxLat = start.lat < end.lat ? end.lat : start.lat;


                        return lng > _minLng && lng < _maxLng && lat > _minLat && lat < _maxLat;
                    },
                    getDistance: function (lng1, lat1, lng2, lat2) {
                        return L.latLng(lat1, lng1).distanceTo(L.latLng(lat2, lng2));
                    },
                    getDistances: function (lnglats) {
                        var _dis = 0;
                        for (var i = 1; i < lnglats.length; i++) {
                            var a1 = lnglats[i - 1];
                            var a2 = lnglats[i];
                            _dis += this.getDistance(a1.lng, a1.lat, a2.lng, a2.lat);
                        }
                        return _dis;
                    },
                    getDeg: function (lng1, lat1, lng2, lat2) {
                        var x = lng2 - lng1;
                        var y = lat2 - lat1;
                        //if (x < 0.0000001 && y < 0.0000001) return null;
                        var hyp = Math.sqrt(x * x + y * y);

                        var Z = x / hyp;

                        var rad = Math.acos(Z);

                        var deg = 180 / (Math.PI / rad);
                        if (lat1 < lat2)
                            deg = 360 - deg;
                        return deg;
                    },
                    getCircleArea: function (radius) {
                        return Math.PI * radius * radius;
                    },
                    getRectArea: function (startPoint, endPoint) {
                        var _w = this.getDistance(startPoint.lng, startPoint.lat, endPoint.lng, startPoint.lat);
                        var _h = this.getDistance(startPoint.lng, startPoint.lat, startPoint.lng, endPoint.lat);
                        return _w * _h;
                    },
                    getArea: function (lnglats) {

                        return geodesicArea(lnglats);

                        function geodesicArea(latLngs) {
                            var pointsCount = latLngs.length,
                                area = 0.0,
                                d2r = L.LatLng.DEG_TO_RAD,
                                p1, p2;

                            if (pointsCount > 2) {
                                for (var i = 0; i < pointsCount; i++) {
                                    p1 = latLngs[i];
                                    p2 = latLngs[(i + 1) % pointsCount];
                                    area += ((p2.lng - p1.lng) * d2r) *
                                            (2 + Math.sin(p1.lat * d2r) + Math.sin(p2.lat * d2r));
                                }
                                area = area * 6378137.0 * 6378137.0 / 2.0;
                            }

                            return Math.abs(area).toFixed(2);
                        }

                        function readableArea(area, isMetric) {
                            var areaStr;

                            if (isMetric) {
                                if (area >= 10000) {
                                    areaStr = (area * 0.0001).toFixed(2) + ' ha';
                                } else {
                                    areaStr = area.toFixed(2) + ' m&sup2;';
                                }
                            } else {
                                area *= 0.836127; // Square yards in 1 meter

                                if (area >= 3097600) { //3097600 square yards in 1 square mile
                                    areaStr = (area / 3097600).toFixed(2) + ' mi&sup2;';
                                } else if (area >= 4840) {//48040 square yards in 1 acre
                                    areaStr = (area / 4840).toFixed(2) + ' acres';
                                } else {
                                    areaStr = Math.ceil(area) + ' yd&sup2;';
                                }
                            }

                            return areaStr;
                        }

                    }
                });
                $.extend(map, {
                    addMiniMap: function (layerName, options) {
                        var defaultOptions = {
                            toggleDisplay: true
                        }

                        $.extend(defaultOptions, options);

                        if (this.MiniMap) {
                            this.MiniMap.removeFrom(this.map);
                            this.MiniMap = null;
                        }

                        var _miniLayer;
                        if (typeof (layerName) == "string") {

                            var _l_f = $.tMap["tileLayer_" + layerName];

                            _miniLayer = _l_f();

                        } else if (typeof (type) == "function") {

                            _miniLayer = layerName();
                        }
                        this.MiniMap = new L.Control.MiniMap(_miniLayer, defaultOptions).addTo(this.map);
                    }
                })
                $.extend(map, {
                    addLayer: function (layer, insertAtTheBottom) {
                        this.map.addLayer(layer, insertAtTheBottom);
                    },
                    removeLayer: function (layer) {
                        this.map.removeLayer(layer);
                    },
                    hasLayer: function (layer) {
                        return this.map.hasLayer(layer);
                    },
                    openPopup: function (lng, lat, html, options) {
                        this.map.openPopup(html, [lat, lng], options);
                    },
                    closePopup: function (popup) {
                        // if popup param is null, will close the popup which belong method 'openPopup'; 
                        this.map.closePopup(popup);
                    },
                    addControl: function (control) {
                        this.map.addControl(layer);
                    },
                    removeControl: function (control) {
                        this.map.removeControl(layer);
                    }
                });

                $.extend(map, {
                    clear: function () {
                        this.defaultLayerGroup.clearLayers();
                    },
                    addToLayerGroup: function (layer) {
                        this.defaultLayerGroup.addLayer(layer);
                    },
                    removeFromLayerGroup: function (layer) {
                        this.defaultLayerGroup.removeLayer(layer);
                    }
                })

                $.extend(map, {
                    latLngToLayerPoint: function (lng, lat) {
                        // return a point
                        return this.map.latLngToLayerPoint([lat, lng]);
                    },
                    layerPointToLatLng: function (x, y) {
                        // return a latlng
                        return this.map.layerPointToLatLng([x, y]);
                    },
                    containerPointToLayerPoint: function (x, y) {
                        // return a point
                        return this.map.containerPointToLayerPoint([x, y]);
                    },
                    layerPointToContainerPoint: function (x, y) {
                        // return a point
                        return this.map.layerPointToContainerPoint([x, y]);
                    },
                    latLngToContainerPoint: function (lng, lat) {
                        // return a point
                        return this.map.latLngToContainerPoint([lat, lng]);
                    },
                    containerPointToLatLng: function (x, y) {
                        // return a latlng
                        return this.map.containerPointToLatLng([x, y]);
                    },
                    project: function (lng, lat, zoom) {
                        // return a point
                        return this.map.project([lat, lng], zoom);
                    },
                    unproject: function (x, y, zoom) {
                        return this.map.unproject([x, y], zoom);
                    },
                    mouseEventToContainerPoint: function (evt) {
                        // return a point
                        return this.map.mouseEventToContainerPoint(evt);
                    },
                    mouseEventToLayerPoint: function (evt) {
                        // return a point
                        return this.map.mouseEventToLayerPoint(evt);
                    },
                    mouseEventToLatLng: function (evt) {
                        // return a latlng
                        return this.map.mouseEventToLatLng(evt);
                    }
                });


                $.extend(map, {
                    getContainer: function () {
                        // return a HTMLElement
                        return this.map.getContainer();
                    },
                    getPanes: function () {
                        // return MapPanes
                        return this.map.getPanes();
                    },
                    whenReady: function (fn, context) {
                        this.map.whenReady(fn, context);
                    },
                    on: function (eventtype, fn) {
                        this.map.on(eventtype, fn);
                        return this;
                    },
                    off: function (eventtype, fn) {
                        this.map.off(eventtype, fn);
                        return this;
                    }
                })

                $.extend(map, {
                    addImageOverLayer: function (url, param) {

                        var defaultParam = {
                            l_t: [],
                            r_b: [],
                            options: {},
                            addToMapFlag: true
                        };

                        $.extend(defaultParam, param);

                        var imageUrl = url,
                imageBounds = [[defaultParam.l_t[1], defaultParam.l_t[0]], [defaultParam.r_b[1], defaultParam.r_b[0]]];

                        var _layer = L.imageOverlay(imageUrl, imageBounds, defaultParam.options);

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(_layer);
                        }
                    }
                })

                $.extend(map, {
                    addLabel: function (lng, lat, text, param) {
                        var thisMap = this;
                        var defaultParam = {
                            anchor: [10, -15],
                            bgColor: "rgba(0, 0, 0, 0.6)",
                            bgHoverColor: "rgba(0, 0, 0, 0.9)",
                            addToMapFlag: true
                        };
                        $.extend(defaultParam, param);

                        var _mark = L.marker([lat, lng], {
                            zIndexOffset: 100000
                        });

                        _mark.setIcon(_createDivIcon());

                        _mark.on("add", function () {
                            $(_mark._icon).css({
                                "margin-left": defaultParam.anchor[0] + "px",
                                "margin-top": defaultParam.anchor[1] + "px"
                            }).css({
                                height: "auto",
                                width: "auto"
                            });

                            if (defaultParam.bgColor != null) {
                                $(_mark._icon).css({ "background-color": defaultParam.bgColor });
                                $(_mark._icon).find("p.before").css({ "border-right-color": defaultParam.bgColor });
                            }

                            if (defaultParam.bgHoverColor != null) {
                                $(_mark._icon).mouseover(function () {
                                    $(_mark._icon).css({ "background-color": defaultParam.bgHoverColor });
                                    $(_mark._icon).find("p.before").css({ "border-right-color": defaultParam.bgHoverColor });

                                }).mouseleave(function () {
                                    $(_mark._icon).css({ "background-color": defaultParam.bgColor });
                                    $(_mark._icon).find("p.before").css({ "border-right-color": defaultParam.bgColor });
                                })
                            }
                        })

                        _mark.m_setText = function (text) {
                            $(_mark._icon).find(".c_text").html(text);
                            return this;
                        }
                        _mark.m_addToMap = function () {
                            thisMap.addToLayerGroup(_mark);
                        }

                        _mark.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(_mark);
                        }

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(_mark);
                        }

                        function _createDivIcon() {
                            var _html = "<p class='before'></p><div class='c_text'>";
                            _html += text + "</div>";


                            return L.divIcon({
                                iconSize: [0, 0],
                                iconAnchor: [0, 0],
                                className: 'tmap-th-label',
                                html: _html
                            })
                        }

                        return _mark;
                    },
                    addImgMarker: function (lng, lat, param) {
                        var _m = this;
                        var defaultParam = {
                            icon: {
                                iconUrl: "",
                                iconSize: [25, 41],
                                iconAnchor: [12, 41]
                            },
                            //sizeOnZoom: function (zoom) { },
                            option: {
                                //clickable:true,
                                //draggable:false,
                                //keyboard:true,
                                //title:"",
                                //alt:"",
                                //zIndexOffset:0,
                                //opacity:1.0,
                                //riseOnHover:false,
                                //riseOffset:250
                            },
                            attribute: null,
                            popupContent: "",
                            popupOption: {},
                            needTransition: false,
                            addToMapFlag: true,
                            //event: {
                            //    //"click": function () { },
                            //    //"dblclick": function () { },
                            //    //"mousedown": function () { },
                            //    //"mouseover": function () { },
                            //    //"mouseout": function () { },
                            //    //"contextmenu": function () { },
                            //    //"dragstart": function () { },
                            //    //"drag": function () { },
                            //    //"dragend": function () { },
                            //    //"move": function () { },
                            //    //"add": function () { },
                            //    //"remove": function () { },
                            //    //"popupopen": function () { },
                            //    //"popupclose": function () { }
                            //},
                            routeEnable: false,
                            routeState: false,
                            routeIndex: 1,
                            routeArr: [],
                            routeRotateOffset: 0
                        }

                        $.extend(defaultParam, param);

                        var _mark = L.marker([lat, lng], defaultParam.option);

                        defaultParam.routeArr.push([lat, lng]);

                        _mark.setIcon(_createDivIcon(defaultParam.icon.iconSize, defaultParam.icon.iconAnchor));

                        if (defaultParam.popupContent) {
                            _mark.bindPopup(defaultParam.popupContent, defaultParam.popupOption);
                        }
                        if (defaultParam.attribute) {
                            _mark.attribute = defaultParam.attribute;
                        }

                        _mark.on("add", function () {
                            if (defaultParam.sizeOnZoom) {
                                _mark._map.on("zoomend", function () {
                                    if (!_mark._map) return;
                                    var _iconOption = defaultParam.sizeOnZoom(_mark._map.getZoom());
                                    _mark.setIcon(_createDivIcon(_iconOption.iconSize, _iconOption.iconAnchor));
                                })
                            }

                            if (defaultParam.routeEnable) {

                                defaultParam.routeState = true;


                                var _dealRoute = function () {
                                    if (defaultParam.routeState == false) return;
                                    if (defaultParam.routeArr.length > defaultParam.routeIndex) {
                                        defaultParam.routeIndex++;
                                        var _thisLngLat = defaultParam.routeArr[defaultParam.routeIndex - 1];
                                        var _preLngLat = defaultParam.routeArr[defaultParam.routeIndex - 2];
                                        _mark.setLatLng(_thisLngLat);
                                        var _deg = _m.getDeg(_preLngLat[1], _preLngLat[0], _thisLngLat[1], _thisLngLat[0]);
                                        if (_deg != null) {
                                            _mark.l_rotate(_deg + defaultParam.routeRotateOffset);
                                        }
                                    }

                                    setTimeout(_dealRoute, 1000);
                                }

                                _dealRoute();

                            }
                        })

                        _mark.on("remove", function () {
                            defaultParam.routeState = false;
                        })

                        if (defaultParam.event) {
                            for (var a in defaultParam.event) {
                                _mark.on(a, defaultParam.event[a]);
                            }
                        }

                        _mark.insertRouteItem = function (lng, lat) {
                            defaultParam.routeArr.push([lat, lng]);
                        }

                        _mark.needTransition = function (state) {
                            if (state == true) {
                                $(_mark._icon).addClass("tmap-th-transition");
                            } else if (state == false) {
                                $(_mark._icon).removeClass("tmap-th-transition");
                            }
                        }

                        _mark.changeIconUrl = function (iconUrl) {
                            $(_mark._icon).find("img").attr("src", iconUrl);
                        }

                        _mark.changeIcon = function (iconOption) {
                            defaultParam.icon = iconOption;
                            _mark.setIcon(_createDivIcon(defaultParam.icon.iconSize, defaultParam.icon.iconAnchor));
                        }

                        function _createDivIcon(_iconSize, _iconAnchor) {
                            var _html = "";

                            var _className = "tmap-th-div-icon";

                            if (defaultParam.needTransition) {
                                _className += " tmap-th-transition";
                            }

                            _html = "<div class='div-icon-div'><img style='height:100%;width:100%;' src='" + defaultParam.icon.iconUrl + "' /></div>";

                            return L.divIcon({
                                iconSize: _iconSize,
                                iconAnchor: _iconAnchor,
                                className: _className,
                                html: _html
                            })
                        }

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(_mark);
                        }

                        return _mark;

                    },
                    addFixedMarker: function (param) {
                        var thisMap = this;
                        var defaultParam = {
                            point: { x: 0, y: 0 },
                            html: "",
                            size: [0, 0],
                            anchor: [0, 0],
                            addToMapFlag: true,
                            option: {
                                //clickable:true,
                                //draggable:false,
                                //keyboard:true,
                                //title:"",
                                //alt:"",
                                //zIndexOffset:0,
                                //opacity:1.0,
                                //riseOnHover:false,
                                //riseOffset:250
                            },
                            fixedLngLat: null,  //{lng:lat:}
                            lineOption: null
                        };

                        $.extend(defaultParam, param);

                        var _lnglat = thisMap.containerPointToLatLng(defaultParam.point.x, defaultParam.point.y);

                        var _mark = L.marker([_lnglat.lat, _lnglat.lng], defaultParam.option);

                        _mark.setIcon(_createDivIcon());

                        _mark.on("add", function () {
                            if (defaultParam.fixedLngLat) {
                                _addFixedLine();
                            }

                            thisMap.on("move", _mapMove, this);
                        })

                        _mark.on("remove", function () {
                            if (defaultParam.fixedLngLat && _mark.fixedLine) {
                                _mark.fixedLine.l_remove();
                            }
                            thisMap.off("move", _mapMove, this);
                        })

                        _mark.on("drag", function () {
                            var _lnglat = _mark.getLatLng();
                            defaultParam.point = thisMap.latLngToContainerPoint(_lnglat.lng, _lnglat.lat);
                            _changeFixLine();
                        });

                        function _mapMove() {
                            var _lnglat = thisMap.containerPointToLatLng(defaultParam.point.x, defaultParam.point.y);
                            _mark.setLatLng(_lnglat);
                        }

                        function _createDivIcon() {
                            var _html = "";

                            var _className = "tmap-th-div-icon";

                            var _iconSize = defaultParam.size;
                            var _iconAnchor = defaultParam.anchor;

                            _html += "<div class='div-icon-div'>";
                            _html += "<div style='height:100%;width:100%;'>" + defaultParam.html + "</div>"
                            _html += "</div>";


                            return L.divIcon({
                                iconSize: _iconSize,
                                iconAnchor: _iconAnchor,
                                className: _className,
                                html: _html
                            })
                        }

                        function _addFixedLine() {
                            _mark.fixedLine = thisMap.addFixed2Line({
                                point: defaultParam.point,
                                latlng: defaultParam.fixedLngLat,
                                options: defaultParam.lineOption,
                                addToMapFlag: true
                            })
                        }

                        function _changeFixLine() {
                            if (_mark.fixedLine) {
                                _mark.fixedLine.l_setPoint(defaultParam.point);
                            }
                        }


                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(_mark);
                        }

                        _mark.m_addToMap = function () {
                            thisMap.addToLayerGroup(_mark);
                        }

                        _mark.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(_mark);
                        }

                        return _mark;

                    },
                    addCssMarker: function (lng, lat, param) {
                        var thisMap = this;
                        var defaultParam = {
                            option: {
                                //clickable:true,
                                //draggable:false,
                                //keyboard:true,
                                //title:"",
                                //alt:"",
                                //zIndexOffset:0,
                                //opacity:1.0,
                                //riseOnHover:false,
                                //riseOffset:250
                            },
                            popupContent: "",
                            popupOption: {},
                            attribute: null,
                            needTransition: false,
                            addToMapFlag: true,
                            event: {
                                //"click": function () { },
                                //"dblclick": function () { },
                                //"mousedown": function () { },
                                //"mouseover": function () { },
                                //"mouseout": function () { },
                                //"contextmenu": function () { },
                                //"dragstart": function () { },
                                //"drag": function () { },
                                //"dragend": function () { },
                                //"move": function () { },
                                //"add": function () { },
                                //"remove": function () { },
                                //"popupopen": function () { },
                                //"popupclose": function () { }
                            },
                            type: "A1",
                            param_A1: {
                                height: 30,
                                width: 30,
                                bgColor: "rgba(41,125,252,0.9)",
                                bgHoverColor: "rgba(255,106,0,0.9)"
                                //iconUrl: "",
                                //iconHtml:""
                            },
                            param_B1: {
                                height: 20,
                                width: 20,
                                bgColor: "rgba(41,125,252,0.9)",
                                bgHoverColor: "rgba(255,106,0,0.9)"
                            },
                            param_C1: {
                                text: '',
                                color: "#000",
                                iconAnchor: [0, 0]
                            }
                        };

                        $.extend(defaultParam, param);

                        var _mark = L.marker([lat, lng], defaultParam.option);
                        if (defaultParam.type == "A1") {
                            _mark.setIcon(_createDivIconA1());

                            $.extend(defaultParam.popupOption, {
                                offset: [0, 0 - (defaultParam.param_A1.height + 5)]
                            })

                        } else if (defaultParam.type == "B1") {
                            _mark.setIcon(_createDivIconB1());
                        } else if (defaultParam.type == "C1") {
                            _mark.setIcon(_createDivIconC1());
                        }

                        if (defaultParam.popupContent) {
                            _mark.bindPopup(defaultParam.popupContent, defaultParam.popupOption);
                        }
                        if (defaultParam.attribute) {
                            _mark.attribute = defaultParam.attribute;
                        }
                        if (defaultParam.type == "A1") {
                            _mark.methodA1 = {};
                            _methodA1Extend();
                        } else if (defaultParam.type == "B1") {
                            _mark.methodB1 = {};
                            _methodB1Extend();
                        } else if (defaultParam.type == "C1") {
                            _mark.methodC1 = {};
                            _methodC1Extend();
                        }


                        _mark.on("add", function () {
                            if (defaultParam.type == "A1") {
                                _mark.methodA1.refreshColor();
                            } else if (defaultParam.type == "B1") {
                                $(_mark._icon).find(".tmap-th-marker-B1").css({ "background-color": defaultParam.param_B1.bgColor });
                            } else if (defaultParam.type == "C1") {
                                $(_mark._icon).css({ "width": "auto" });
                            }
                        })



                        _mark.on("mouseover", function () {

                            if (defaultParam.type == "A1") {
                                $(_mark._icon).find(".tmap-th-marker-A1").css({ "background-color": defaultParam.param_A1.bgHoverColor });
                                $(_mark._icon).find(".A1_bottom").css({ "border-top-color": defaultParam.param_A1.bgHoverColor });
                            } else if (defaultParam.type == "B1") {
                                $(_mark._icon).find(".tmap-th-marker-B1").css({ "background-color": defaultParam.param_B1.bgHoverColor });
                            }


                        }).on("mouseout", function () {
                            if (defaultParam.type == "A1") {
                                $(_mark._icon).find(".tmap-th-marker-A1").css({ "background-color": defaultParam.param_A1.bgColor });
                                $(_mark._icon).find(".A1_bottom").css({ "border-top-color": defaultParam.param_A1.bgColor });
                            } else if (defaultParam.type == "B1") {
                                $(_mark._icon).find(".tmap-th-marker-B1").css({ "background-color": defaultParam.param_B1.bgColor });
                            }
                        })


                        function _createDivIconA1() {
                            var _html = "";

                            var _className = "tmap-th-div-icon";

                            if (defaultParam.needTransition) {
                                _className += " tmap-th-transition";
                            }

                            var _iconSize = [0, 0];
                            var _iconAnchor = [defaultParam.param_A1.width / 2, defaultParam.param_A1.height + 12];

                            _html += "<div class='div-icon-div'>";
                            _html += "<div class='tmap-th-marker-A1' style='line-height:" + defaultParam.param_A1.height + "px;width:" + defaultParam.param_A1.width + "px;'>";
                            _html += "<div class='A1_main'>";
                            if (defaultParam.param_A1.iconUrl) {
                                _html += "<img src='" + defaultParam.param_A1.iconUrl + "' style='height:100%;width:100%;' />";
                            } else if (defaultParam.param_A1.iconHtml) {
                                _html += defaultParam.param_A1.iconHtml;
                            }
                            _html += "</div>";
                            _html += "<div class='A1_bottom'>";
                            _html += "</div>";
                            _html += "</div>";
                            _html += "</div>";


                            return L.divIcon({
                                iconSize: _iconSize,
                                iconAnchor: _iconAnchor,
                                className: _className,
                                html: _html
                            })
                        }

                        function _createDivIconB1() {
                            var _html = "";

                            var _className = "tmap-th-div-icon";

                            if (defaultParam.needTransition) {
                                _className += " tmap-th-transition";
                            }

                            var _iconSize = [0, 0];
                            var _iconAnchor = [defaultParam.param_B1.width / 2, defaultParam.param_B1.height / 2];

                            _html += "<div class='div-icon-div'>";
                            _html += "<div class='tmap-th-marker-B1' style='border-radius:500px;line-height:" + defaultParam.param_B1.height + "px;height:" + defaultParam.param_B1.height + "px;width:" + defaultParam.param_B1.width + "px;'>";
                            _html += "</div>";
                            _html += "</div>";


                            return L.divIcon({
                                iconSize: _iconSize,
                                iconAnchor: _iconAnchor,
                                className: _className,
                                html: _html
                            })
                        }

                        function _methodA1Extend() {
                            $.extend(_mark.methodA1, {
                                changeIconUrl: function (iconUrl) {
                                    defaultParam.param_A1.iconUrl = iconUrl;
                                    $(_mark._icon).find(".tmap-th-marker-A1").find(".A1_main").find("img").attr("src", iconUrl);
                                },
                                changeIconHtml: function (iconHtml) {
                                    defaultParam.param_A1.iconHtml = iconHtml;
                                    $(_mark._icon).find(".tmap-th-marker-A1").find(".A1_main").html(iconHtml);
                                },
                                changeColor: function (param) {
                                    $.extend(defaultParam.param_A1, param);
                                    this.refreshColor();
                                },
                                refreshColor: function () {
                                    $(_mark._icon).find(".tmap-th-marker-A1").css({ "background-color": defaultParam.param_A1.bgColor });
                                    $(_mark._icon).find(".A1_bottom").css({ "left": (defaultParam.param_A1.width - 10) / 2 + "px", "top": defaultParam.param_A1.height + "px" })
                                        .css({
                                            "border-top-color": defaultParam.param_A1.bgColor
                                        });
                                },
                                changeSize: function (param) {
                                    $.extend(defaultParam.param_B1, param);
                                    $(_mark._icon).css({
                                        "margin-left": "-" + defaultParam.param_B1.width / 2 + "px",
                                        "margin-top": "-" + defaultParam.param_B1.height / 2 + "px"
                                    }).find(".tmap-th-marker-B1").css({
                                        height: defaultParam.param_B1.height + "px",
                                        width: defaultParam.param_B1.width + "px"
                                    });
                                }
                            })
                        }

                        function _methodB1Extend() {
                            $.extend(_mark.methodB1, {
                                changeColor: function (param) {
                                    $.extend(defaultParam.param_B1, param);
                                    $(_mark._icon).find(".tmap-th-marker-B1").css({ "background-color": defaultParam.param_B1.bgColor });
                                },
                                changeSize: function (param) {
                                    $.extend(defaultParam.param_B1, param);
                                    $(_mark._icon).css({
                                        "margin-left": "-" + defaultParam.param_B1.width / 2 + "px",
                                        "margin-top": "-" + defaultParam.param_B1.height / 2 + "px"
                                    }).find(".tmap-th-marker-B1").css({
                                        height: defaultParam.param_B1.height + "px",
                                        width: defaultParam.param_B1.width + "px"
                                    });
                                }
                            })
                        }

                        function _createDivIconC1() {
                            var _html = "";

                            var _className = "tmap-th-div-icon";

                            if (defaultParam.needTransition) {
                                _className += " tmap-th-transition";
                            }

                            var _iconSize = [0, 0];
                            var _iconAnchor = defaultParam.param_C1.iconAnchor;

                            _html += "<div class='div-icon-div'>";
                            _html += "<div class='tmap-th-marker-C1' style='color:" + defaultParam.param_C1.color + ";width:" + defaultParam.param_C1.width + "px;'>";
                            _html += defaultParam.param_C1.text;
                            _html += "</div>";
                            _html += "</div>";


                            return L.divIcon({
                                iconSize: _iconSize,
                                iconAnchor: _iconAnchor,
                                className: _className,
                                html: _html
                            })
                        }

                        function _methodC1Extend() {
                            $.extend(_mark.methodC1, {
                                changeFontSize: function (param) {
                                    $.extend(defaultParam.param_C1, param);
                                    $(_mark._icon).find(".tmap-th-marker-C1").css({
                                        "font-size": defaultParam.param_C1.fontSize
                                    });
                                }
                            })
                        }

                        if (defaultParam.event) {
                            for (var a in defaultParam.event) {
                                _mark.on(a, defaultParam.event[a]);
                            }
                        }

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(_mark);
                        }

                        _mark.m_addToMap = function () {
                            thisMap.addToLayerGroup(_mark);
                        }

                        _mark.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(_mark);
                        }

                        return _mark;
                    },
                    addLine: function (latlngs, param) {
                        var thisMap = this;

                        // 默认参数
                        var defaultParam = {
                            // 线条样式
                            options: $.tMap.utility._cloneJsonObject($.tMap.constant.polyLineOption),
                            // 线条Hover样式
                            hoverOptions: null,
                            // 线条Hover的Label提示
                            label: null,
                            // 线条对象附加对象,一般带入业务对象
                            attribute: null,
                            // 线条点击后的Popup框
                            popupContent: null,
                            // Popup框参数
                            popupOption: {},
                            // 线条对象是否立刻添加到地图
                            addToMapFlag: true,
                            dash: null,
                            // 线条事件
                            event: {
                                //"click": function () { },
                                //"dblclick": function () { },
                                //"mousedown": function () { },
                                //"mouseover": function () { },
                                //"mouseout": function () { },
                                //"contextmenu": function () { },
                                //"add": function () { },
                                //"remove": function () { },
                                //"popupopen": function () { },
                                //"popupclose": function () { }
                            }
                        };

                        $.extend(defaultParam, param);

                        // 坐标参数转换
                        var _arr = [];
                        if (typeof (latlngs) === "string") {
                            var _t1 = latlngs.split(";");
                            for (var i = 0; i < _t1.length; i++) {
                                var _t2 = _t1[i].split(",");
                                _arr.push([parseFloat(_t2[1]), parseFloat(_t2[0])]);
                            }
                        } else if ($.tMap.utility._isArray(latlngs)) {
                            for (var i = 0; i < latlngs.length; i++) {
                                var _t3 = latlngs[i];
                                if (typeof (_t3) === "string") {
                                    _arr.push([parseFloat(_t3.split(",")[1]), parseFloat(_t3.split(",")[0])])
                                } else {
                                    _arr.push([_t3.lat, _t3.lng]);
                                }
                            }
                        }

                        // 创建线条对象
                        var polyline = L.polyline(_arr, defaultParam.options);

                        // 添加业务附加对象
                        if (defaultParam.attribute) polyline.attribute = defaultParam.attribute;

                        // 绑定Popup对象
                        if (defaultParam.popupContent) {
                            polyline.bindPopup(defaultParam.popupContent, defaultParam.popupOption);
                        }

                        // 创建提示Label对象
                        if (defaultParam.label) {
                            polyline.hoverLabel = thisMap.addLabel(0, 0, defaultParam.label.text, $.extend(defaultParam.label, {
                                addToMapFlag: false
                            }));
                        }

                        polyline.on("add", function (evt) {
                            if (defaultParam.dash) {
                                $(polyline._path).attr('stroke-dasharray', defaultParam.dash);
                            }
                        }).on("mouseover", function (evt) {
                            if (defaultParam.hoverOptions) {
                                polyline.setStyle(defaultParam.hoverOptions);
                            }

                            if (polyline.hoverLabel) {
                                thisMap.addToLayerGroup(polyline.hoverLabel);
                            }

                        }).on("mouseout", function (evt) {
                            if (defaultParam.hoverOptions) {
                                polyline.setStyle(defaultParam.options);
                            }

                            if (polyline.hoverLabel) {
                                thisMap.removeFromLayerGroup(polyline.hoverLabel);
                            }
                        }).on("mousemove", function (evt) {
                            if (polyline.hoverLabel) {
                                polyline.hoverLabel.setLatLng(evt.latlng);
                            }
                        })

                        // 处理事件
                        if (defaultParam.event) {
                            for (var a in defaultParam.event) {
                                polyline.on(a, defaultParam.event[a]);
                            }
                        }

                        polyline.m_addToMap = function () {
                            thisMap.addToLayerGroup(polyline);
                        }

                        polyline.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(polyline);
                        }

                        if (defaultParam.addToMapFlag == true) {
                            thisMap.addToLayerGroup(polyline);
                        }

                        return polyline;
                    },
                    addFixed2Line: function (param) {
                        var _m = this;

                        var defaultParam = {
                            point: { x: 0, y: 0 },
                            latlng: { lng: 0, lat: 0 },
                            options: {},
                            attribute: null,
                            addToMapFlag: true
                        };

                        $.extend(defaultParam, param);

                        var defaultOptions = $.tMap.utility._cloneJsonObject($.tMap.constant.polyLineOption);

                        $.extend(defaultOptions, defaultParam.options);

                        var polyline = L.polyline([[0, 0], [0, 0]], defaultOptions);

                        updatePosition();

                        polyline.on("add", function () {
                            _m.on("move", function () {
                                updatePosition();
                            })
                        })

                        polyline.l_setPoint = function (point) {
                            defaultParam.point = point;
                            updatePosition();
                        }

                        polyline.l_setLatLng = function (latlng) {
                            defaultParam.latlng = latlng;
                            updatePosition();
                        }

                        polyline.l_add = function () {
                            _m.addToLayerGroup(polyline);
                        }

                        polyline.l_remove = function () {
                            _m.removeFromLayerGroup(polyline);
                        }


                        if (defaultParam.addToMapFlag == true) {
                            _m.addToLayerGroup(polyline);
                        }

                        function updatePosition() {
                            var _latLng = _m.containerPointToLatLng(defaultParam.point.x, defaultParam.point.y);
                            polyline.setLatLngs([_latLng, defaultParam.latlng]);
                        }


                        return polyline;

                    },
                    addPolygon: function (latlngs, param) {
                        var thisMap = this;
                        // 默认参数
                        var defaultParam = {
                            // 多边形样式
                            options: $.tMap.utility._cloneJsonObject($.tMap.constant.polygonOption),
                            // 多边形Hover样式
                            hoverOptions: null,
                            // 多边形Hover的Label提示
                            label: null,
                            // 多边形对象附加对象,一般带入业务对象
                            attribute: null,
                            // 多边形点击后的Popup框
                            popupContent: null,
                            // Popup框参数
                            popupOption: {},
                            // 多边形对象是否立刻添加到地图
                            addToMapFlag: true,
                            dash: null,
                            // 多边形事件
                            event: {
                                //"click": function () { },
                                //"dblclick": function () { },
                                //"mousedown": function () { },
                                //"mouseover": function () { },
                                //"mouseout": function () { },
                                //"contextmenu": function () { },
                                //"add": function () { },
                                //"remove": function () { },
                                //"popupopen": function () { },
                                //"popupclose": function () { }
                            }
                        };

                        $.extend(defaultParam, param);

                        // 坐标参数转换
                        var _arr = [];
                        if (typeof (latlngs) === "string") {
                            var _t1 = latlngs.split(";");
                            for (var i = 0; i < _t1.length; i++) {
                                var _t2 = _t1[i].split(",");
                                _arr.push([parseFloat(_t2[1]), parseFloat(_t2[0])]);
                            }
                        } else if ($.tMap.utility._isArray(latlngs)) {
                            for (var i = 0; i < latlngs.length; i++) {
                                var _t3 = latlngs[i];
                                if (typeof (_t3) === "string") {
                                    _arr.push([parseFloat(_t3.split(",")[1]), parseFloat(_t3.split(",")[0])])
                                } else {
                                    _arr.push([_t3.lat, _t3.lng]);
                                }
                            }
                        }

                        // 创建多边形对象
                        var polygon = L.polygon(_arr, defaultParam.options);

                        // 添加业务附加对象
                        if (defaultParam.attribute) polygon.attribute = defaultParam.attribute;

                        // 绑定Popup对象
                        if (defaultParam.popupContent) {
                            polygon.bindPopup(defaultParam.popupContent, defaultParam.popupOption);
                        }

                        // 创建提示Label对象
                        if (defaultParam.label) {
                            polygon.hoverLabel = thisMap.addLabel(0, 0, defaultParam.label.text, $.extend(defaultParam.label, {
                                addToMapFlag: false
                            }));
                        }

                        polygon.on("add", function (evt) {
                            if (defaultParam.dash) {
                                $(polygon._path).attr('stroke-dasharray', defaultParam.dash);
                            }
                        }).on("mouseover", function (evt) {
                            if (defaultParam.hoverOptions) {
                                polygon.setStyle(defaultParam.hoverOptions);
                            }

                            if (polygon.hoverLabel) {
                                thisMap.addToLayerGroup(polygon.hoverLabel);
                            }

                        }).on("mouseout", function (evt) {
                            if (defaultParam.hoverOptions) {
                                polygon.setStyle(defaultParam.options);
                            }

                            if (polygon.hoverLabel) {
                                thisMap.removeFromLayerGroup(polygon.hoverLabel);
                            }
                        }).on("mousemove", function (evt) {
                            if (polygon.hoverLabel) {
                                polygon.hoverLabel.setLatLng(evt.latlng);
                            }
                        })

                        // 处理事件
                        if (defaultParam.event) {
                            for (var a in defaultParam.event) {
                                polygon.on(a, defaultParam.event[a]);
                            }
                        }


                        polygon.m_addToMap = function () {
                            thisMap.addToLayerGroup(polygon);
                        }

                        polygon.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(polygon);
                        }

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(polygon);
                        }

                        return polygon;
                    },
                    addRectangle: function (latlngs, param) {
                        var thisMap = this;

                        // 默认参数
                        var defaultParam = {
                            // 矩形样式
                            options: $.tMap.utility._cloneJsonObject($.tMap.constant.rectangleOption),
                            // 矩形Hover样式
                            hoverOptions: null,
                            // 矩形Hover的Label提示
                            label: null,
                            // 矩形对象附加对象,一般带入业务对象
                            attribute: null,
                            // 矩形点击后的Popup框
                            popupContent: null,
                            // Popup框参数
                            popupOption: {},
                            // 矩形对象是否立刻添加到地图
                            addToMapFlag: true,
                            // 矩形事件
                            event: {
                                //"click": function () { },
                                //"dblclick": function () { },
                                //"mousedown": function () { },
                                //"mouseover": function () { },
                                //"mouseout": function () { },
                                //"contextmenu": function () { },
                                //"add": function () { },
                                //"remove": function () { },
                                //"popupopen": function () { },
                                //"popupclose": function () { }
                            }
                        };

                        $.extend(defaultParam, param);

                        // 坐标参数转换
                        var _arr = [];
                        if (typeof (latlngs) === "string") {
                            var _t1 = latlngs.split(";");
                            for (var i = 0; i < _t1.length; i++) {
                                var _t2 = _t1[i].split(",");
                                _arr.push([parseFloat(_t2[1]), parseFloat(_t2[0])]);
                            }
                        } else if ($.tMap.utility._isArray(latlngs)) {
                            for (var i = 0; i < latlngs.length; i++) {
                                var _t3 = latlngs[i];
                                if (typeof (_t3) === "string") {
                                    _arr.push([parseFloat(_t3.split(",")[1]), parseFloat(_t3.split(",")[0])])
                                } else {
                                    _arr.push([_t3.lat, _t3.lng]);
                                }
                            }
                        }

                        // 创建矩形对象
                        var rectangle = L.rectangle(_arr, defaultParam.options);

                        // 添加业务附加对象
                        if (defaultParam.attribute) rectangle.attribute = defaultParam.attribute;

                        // 绑定Popup对象
                        if (defaultParam.popupContent) {
                            rectangle.bindPopup(defaultParam.popupContent, defaultParam.popupOption);
                        }

                        // 创建提示Label对象
                        if (defaultParam.label) {
                            rectangle.hoverLabel = thisMap.addLabel(0, 0, defaultParam.label.text, $.extend(defaultParam.label, {
                                addToMapFlag: false
                            }));
                        }

                        rectangle.on("add", function (evt) {
                            if (defaultParam.dash) {
                                $(rectangle._path).attr('stroke-dasharray', defaultParam.dash);
                            }
                        }).on("mouseover", function (evt) {
                            if (defaultParam.hoverOptions) {
                                rectangle.setStyle(defaultParam.hoverOptions);
                            }

                            if (rectangle.hoverLabel) {
                                thisMap.addToLayerGroup(rectangle.hoverLabel);
                            }

                        }).on("mouseout", function (evt) {
                            if (defaultParam.hoverOptions) {
                                rectangle.setStyle(defaultParam.options);
                            }

                            if (rectangle.hoverLabel) {
                                thisMap.removeFromLayerGroup(rectangle.hoverLabel);
                            }
                        }).on("mousemove", function (evt) {
                            if (rectangle.hoverLabel) {
                                rectangle.hoverLabel.setLatLng(evt.latlng);
                            }
                        })

                        // 处理事件
                        if (defaultParam.event) {
                            for (var a in defaultParam.event) {
                                rectangle.on(a, defaultParam.event[a]);
                            }
                        }

                        rectangle.m_addToMap = function () {
                            thisMap.addToLayerGroup(rectangle);
                        }

                        rectangle.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(rectangle);
                        }

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(rectangle);
                        }

                        return rectangle;
                    },
                    addCircle: function (lng, lat, radius, param) {

                        var thisMap = this;

                        // 223默认参数
                        var defaultParam = {
                            // 圆形样式
                            options: $.tMap.utility._cloneJsonObject($.tMap.constant.circleOption),
                            // 多边形Hover样式
                            hoverOptions: null,
                            // 多边形Hover的Label提示
                            label: null,
                            // 多边形对象附加对象,一般带入业务对象
                            attribute: null,
                            // 多边形点击后的Popup框
                            popupContent: null,
                            // Popup框参数
                            popupOption: {},
                            // 多边形对象是否立刻添加到地图
                            addToMapFlag: true,
                            // 多边形事件
                            event: {
                                //"click": function () { },
                                //"dblclick": function () { },
                                //"mousedown": function () { },
                                //"mouseover": function () { },
                                //"mouseout": function () { },
                                //"contextmenu": function () { },
                                //"add": function () { },
                                //"remove": function () { },
                                //"popupopen": function () { },
                                //"popupclose": function () { }
                            }
                        };

                        $.extend(defaultParam, param);

                        // 创建圆形对象
                        var circle = L.circle([lat, lng], radius, defaultParam.options);

                        // 添加业务附加对象
                        if (defaultParam.attribute) circle.attribute = defaultParam.attribute;

                        // 绑定Popup对象
                        if (defaultParam.popupContent) {
                            circle.bindPopup(defaultParam.popupContent, defaultParam.popupOption);
                        }

                        // 创建提示Label对象
                        if (defaultParam.label) {
                            circle.hoverLabel = thisMap.addLabel(0, 0, defaultParam.label.text, $.extend(defaultParam.label, {
                                addToMapFlag: false
                            }));
                        }

                        circle.on("add", function (evt) {
                            if (defaultParam.dash) {
                                $(circle._path).attr('stroke-dasharray', defaultParam.dash);
                            }
                        }).on("mouseover", function (evt) {
                            if (defaultParam.hoverOptions) {
                                circle.setStyle(defaultParam.hoverOptions);
                            }

                            if (circle.hoverLabel) {
                                thisMap.addToLayerGroup(circle.hoverLabel);
                            }

                        }).on("mouseout", function (evt) {
                            if (defaultParam.hoverOptions) {
                                circle.setStyle(defaultParam.options);
                            }

                            if (circle.hoverLabel) {
                                thisMap.removeFromLayerGroup(circle.hoverLabel);
                            }
                        }).on("mousemove", function (evt) {
                            if (circle.hoverLabel) {
                                circle.hoverLabel.setLatLng(evt.latlng);
                            }
                        })

                        // 处理事件
                        if (defaultParam.event) {
                            for (var a in defaultParam.event) {
                                circle.on(a, defaultParam.event[a]);
                            }
                        }

                        circle.m_addToMap = function () {
                            thisMap.addToLayerGroup(circle);
                        }

                        circle.m_removeFromMap = function () {
                            thisMap.removeFromLayerGroup(circle);
                        }

                        if (defaultParam.addToMapFlag == true) {
                            this.addToLayerGroup(circle);
                        }

                        return circle;
                    }
                })

                $.extend(map, {
                    drawLine: function (param) {
                        var defaultParam = {
                            styleOptions: {
                                stroke: true,
                                color: 'blue',
                                weight: 4,
                                opacity: 0.7,
                                fill: false,
                                fillColor: null, //same as color by default
                                fillOpacity: 0.2,
                                clickable: true
                            },
                            begin: function () { },
                            complete: function () { }
                        };
                        var resultObj = {};

                        $.extend(defaultParam, param);

                        $.extend(resultObj, {
                            group: L.layerGroup(),
                            lnglats: [],
                            label: null,
                            line: null,
                            dynamicLine: null
                        })

                        map.addToLayerGroup(resultObj.group);

                        _initAction();

                        function _initAction() {
                            map.map.doubleClickZoom.disable();
                            map.on("click", _click);
                            map.on("mousemove", _mousemove);
                            map.on("dblclick", _dblclick);


                            resultObj.label = map.addLabel(0, 0, "点击开始绘制线条", { addToMapFlag: false });
                            resultObj.group.addLayer(resultObj.label);

                        }

                        function _click(e) {
                            resultObj.lnglats.push(e.latlng);

                            if (resultObj.lnglats.length == 1) {

                                resultObj.line = L.polyline([e.latlng, e.latlng], defaultParam.styleOptions);
                                resultObj.dynamicLine = L.polyline([e.latlng, e.latlng], defaultParam.styleOptions);

                                resultObj.group.addLayer(resultObj.line);
                                resultObj.group.addLayer(resultObj.dynamicLine);

                            }

                            resultObj.line.addLatLng(e.latlng);
                            resultObj.dynamicLine.setLatLngs([e.latlng, e.latlng]);
                        }

                        function _mousemove(e) {
                            if (resultObj.lnglats.length != 0) {
                                resultObj.dynamicLine.setLatLngs([resultObj.lnglats[resultObj.lnglats.length - 1], e.latlng]);
                                resultObj.label.m_setText("双击结束绘制");
                            }
                            resultObj.label.setLatLng(e.latlng);
                        }

                        function _dblclick(e) {
                            map.removeFromLayerGroup(resultObj.group);

                            defaultParam.complete(resultObj.lnglats);

                            _endAction();
                        }

                        function _getDistance(_latlng) {
                            var _d1 = map.getDistances(resultObj.lnglats);

                            var _d2 = map.getDistances([resultObj.lnglats[resultObj.lnglats.length - 1], _latlng]);

                            var _d3 = _d1 + _d2;

                            if (_d3 >= 1000) {
                                return (_d3 * 0.001).toFixed(3) + "千米";
                            } else {
                                return _d3.toFixed(3) + "米";
                            }

                        }

                        function _endAction() {
                            map.off("click", _click);
                            map.off("mousemove", _mousemove);
                            map.off("dblclick", _dblclick);
                            setTimeout(function () {
                                map.map.doubleClickZoom.enable();
                            }, 100);
                        }

                        $.extend(resultObj, {
                            remove: function () {
                                map.removeFromLayerGroup(resultObj.group);
                            }
                        })

                        return resultObj;

                    },
                    drawPoly: function (param) {
                        var defaultParam = {
                            styleOptions: {
                                stroke: true,
                                color: 'blue',
                                weight: 4,
                                opacity: 0.7,
                                fill: true,
                                fillColor: null, //same as color by default
                                fillOpacity: 0.2,
                                clickable: true
                            },
                            begin: function () { },
                            complete: function () { }
                        };
                        var resultObj = {};
                        $.extend(defaultParam, param);

                        $.extend(resultObj, {
                            group: L.layerGroup(),
                            lnglats: [],
                            label: null,
                            poly: null,
                            dynamicPoly: null
                        })

                        map.addToLayerGroup(resultObj.group);


                        _initAction();

                        function _initAction() {
                            map.map.doubleClickZoom.disable();
                            map.on("click", _click);
                            map.on("mousemove", _mousemove);
                            map.on("dblclick", _dblclick);

                            resultObj.label = map.addLabel(0, 0, "点击开始绘制多边形", { addToMapFlag: false });
                            resultObj.group.addLayer(resultObj.label);

                        }

                        function _click(e) {

                            resultObj.lnglats.push(e.latlng);

                            if (resultObj.lnglats.length == 1) {

                                resultObj.poly = L.polygon([e.latlng, e.latlng], defaultParam.styleOptions);

                                resultObj.dynamicPoly = L.polygon([e.latlng, e.latlng], defaultParam.styleOptions);

                                resultObj.group.addLayer(resultObj.poly);

                                resultObj.group.addLayer(resultObj.dynamicPoly);

                            }

                            resultObj.poly.addLatLng(e.latlng);
                            resultObj.dynamicPoly.setLatLngs([e.latlng, e.latlng]);
                        }

                        function _mousemove(e) {
                            if (resultObj.lnglats.length > 1) {
                                resultObj.dynamicPoly.setLatLngs([resultObj.lnglats[resultObj.lnglats.length - 1], e.latlng]);
                                resultObj.label.m_setText("双击结束绘制");
                            }
                            resultObj.label.setLatLng(e.latlng);
                        }

                        function _dblclick(e) {
                            map.removeFromLayerGroup(resultObj.group);

                            defaultParam.complete(resultObj.lnglats);

                            _endAction();
                        }

                        function _getArea(_latlng) {
                            var _area = map.getArea(resultObj.lnglats);

                            if (_area >= 1000000) {
                                _area = (_area * 0.000001).toFixed(3) + "平方千米";
                            } else {
                                _area = _area.fixed(3) + "平方米";
                            }
                            return "面积:" + _area;
                        }

                        function _endAction() {
                            map.off("click", _click);
                            map.off("mousemove", _mousemove);
                            map.off("dblclick", _dblclick);

                            setTimeout(function () {
                                map.map.doubleClickZoom.enable();
                            }, 100);

                        }

                        $.extend(resultObj, {
                            remove: function () {
                                map.removeFromLayerGroup(resultObj.group);
                            }
                        })

                        return resultObj;

                    },
                    drawCircle: function (param) {
                        var defaultParam = {
                            styleOptions: {
                                stroke: true,
                                color: 'blue',
                                weight: 1,
                                opacity: 0.5,
                                fill: true,
                                fillColor: null, //same as color by default
                                fillOpacity: 0.2,
                                clickable: true
                            },
                            begin: function () { },
                            complete: function () { }
                        };
                        var resultObj = {};
                        $.extend(defaultParam, param);

                        $.extend(resultObj, {
                            group: L.layerGroup(),
                            center: null,
                            radius: 0,
                            label: null,
                            circle: null
                        })

                        map.addToLayerGroup(resultObj.group);

                        var _container = map.map.getContainer();

                        _initAction();

                        function _initAction() {
                            _container.style.cursor = "crosshair";
                            map.map.dragging.disable();

                            map.on("mousedown", _mousedown);
                            map.on("mousemove", _mousemove);
                            map.on("mouseup", _mouseup);

                            resultObj.label = map.addLabel(0, 0, "点击开始绘制圆", { addToMapFlag: false });

                            resultObj.group.addLayer(resultObj.label);
                        }

                        function _mousedown(e) {

                            defaultParam.begin();

                            resultObj.center = e.latlng;

                            resultObj.circle = L.circle(e.latlng, 0.1, defaultParam.styleOptions);

                            resultObj.group.addLayer(resultObj.circle);

                        }

                        function _mousemove(e) {
                            if (resultObj.circle != null) {
                                resultObj.radius = e.latlng.distanceTo(resultObj.center);
                                resultObj.circle.setRadius(resultObj.radius);
                                resultObj.label.m_setText(_getArea(resultObj.radius));
                            }
                            resultObj.label.setLatLng(e.latlng);
                        }

                        function _mouseup(e) {
                            map.removeFromLayerGroup(resultObj.group);
                            defaultParam.complete({
                                lng: resultObj.center.lng,
                                lat: resultObj.center.lat,
                                radius: resultObj.radius
                            });
                            _endAction();
                        }

                        function _getArea(_radius) {
                            var lengthInfo = "";
                            var areaInfo = "";

                            if (_radius >= 1000) {
                                lengthInfo = (_radius * 0.001).toFixed(3) + "千米";
                            } else {
                                lengthInfo = _radius.toFixed(3) + "米";
                            }

                            //var _area = map.getCircleArea(_radius);
                            //if (_area >= 1000000) {
                            //    areaInfo = (_area * 0.000001).toFixed(3) + "平方千米"
                            //} else {
                            //    areaInfo = _area.toFixed(3) + "平方米"
                            //}

                            return "<p>半径：" + lengthInfo + "</p>";
                        }

                        function _endAction() {
                            map.off("mousedown", _mousedown);
                            map.off("mousemove", _mousemove);
                            map.off("mouseup", _mouseup);

                            map.map.dragging.enable();
                            _container.style.cursor = "pointer";
                        }

                        $.extend(resultObj, {
                            remove: function () {
                                map.removeFromLayerGroup(resultObj.group);
                            }
                        })

                        return resultObj;
                    },
                    drawRectangle: function (param) {
                        var defaultParam = {
                            onlyOnce: false,
                            single: true,
                            styleOptions: {
                                stroke: true,
                                color: 'blue',
                                weight: 1,
                                opacity: 0.5,
                                fill: true,
                                fillColor: null, //same as color by default
                                fillOpacity: 0.2,
                                clickable: true
                            },
                            begin: function () { },
                            complete: function () { }
                        };

                        var resultObj = {};

                        $.extend(defaultParam, param);

                        $.extend(resultObj, {
                            group: L.layerGroup(),
                            startPoint: null,
                            endPoint: null,
                            label: null,
                            rect: null
                        })

                        map.addToLayerGroup(resultObj.group);


                        var _rectangle = null;
                        var startPoint = null;
                        var _container = map.map.getContainer();
                        var _labelObject = this.addLabel(0, 0, "绘制矩形区域", {
                            anchor: [10, -15]
                        });
                        defaultParam.begin();

                        _initAction();

                        function _initAction() {
                            _container.style.cursor = "crosshair";

                            map.map.dragging.disable();

                            map.on("mousedown", _mousedown);
                            map.on("mousemove", _mousemove);
                            map.on("mouseup", _mouseup);

                            resultObj.label = map.addLabel(0, 0, "点击开始绘制矩形", { addToMapFlag: false });

                            resultObj.group.addLayer(resultObj.label);
                        }

                        function _mousedown(e) {
                            defaultParam.begin();

                            resultObj.startPoint = e.latlng;

                            var bounds = [resultObj.startPoint, resultObj.startPoint];

                            resultObj.rect = L.rectangle(bounds, defaultParam.styleOptions);

                            resultObj.group.addLayer(resultObj.rect);
                        }

                        function _mousemove(e) {

                            if (resultObj.rect != null) {
                                var bounds = [resultObj.startPoint, e.latlng];
                                resultObj.rect.setBounds(bounds);

                                resultObj.label.m_setText(_getArea(e.latlng));
                            }

                            resultObj.label.setLatLng(e.latlng);

                        }

                        function _mouseup(e) {
                            resultObj.endPoint = e.latlng;
                            map.removeFromLayerGroup(resultObj.group);

                            defaultParam.complete({
                                start: resultObj.startPoint,
                                end: resultObj.endPoint
                            });

                            _endAction();

                        }

                        function _getArea(_latlng) {
                            var _area = map.getRectArea(resultObj.startPoint, _latlng);
                            if (_area >= 1000000) {
                                _area = (_area * 0.000001).toFixed(3) + "平方千米";
                            } else {
                                _area = _area.toFixed(3) + "平方米";
                            }
                            return "面积:" + _area;
                        }


                        function _endAction() {
                            map.off("mousedown", _mousedown);
                            map.off("mousemove", _mousemove);
                            map.off("mouseup", _mouseup);

                            map.map.dragging.enable();
                            _container.style.cursor = "pointer";
                        }

                        $.extend(resultObj, {
                            remove: function () {
                                map.removeFromLayerGroup(resultObj.group);
                            }
                        })

                        return resultObj;
                    }
                })

                $.extend(map, {
                    addClusterMarkers: function (markers, param) {
                        var defaultParam = {
                            iconCreateFunction: function (cluster) {
                                return L.divIcon({ html: cluster.getAllChildMarkers().length, className: "tmap-th-cluster", iconSize: L.point(30, 30) });
                            },
                            maxClusterRadius: 80,
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
                            //tIcon: {
                            //    type: 1,
                            //    colorType:1
                            //    iconUrl: "",
                            //    iconFont:""
                            //}
                        }

                        $.extend(defaultParam, param);

                        if (defaultParam.tIcon) {
                            defaultParam.iconCreateFunction = function (cluster) {

                                if (defaultParam.tIcon.type == 1) {
                                    var _iconSize = [30, 40];
                                    var _iconAnchor = [15, 40];
                                    _html = "<div class='div-icon-div'><div class='tmap-th-marker-type" + defaultParam.tIcon.type + " ";
                                    _html += "tmap-th-mark-type1-colorType-" + defaultParam.tIcon.colorType + "'>";
                                    //if (defaultParam.tIcon.iconUrl) {
                                    //    _html += "<img src='" + defaultParam.tIcon.iconUrl + "' />"
                                    //} else if (defaultParam.tIcon.iconFont) {
                                    //    _html += "<p class='font'>" + defaultParam.tIcon.iconFont + "</p>"
                                    //}
                                    _html += "<img src='" + defaultParam.tIcon.iconUrl + "' />";
                                    _html += "<p class='cluster'>" + cluster.getAllChildMarkers().length + "</p>";
                                    _html += "</div></div>";

                                    return L.divIcon({
                                        iconSize: _iconSize,
                                        iconAnchor: _iconAnchor,
                                        className: 'tmap-th-div-icon',
                                        html: _html
                                    })

                                }

                                return L.divIcon({ html: cluster.getAllChildMarkers().length, className: "tmap-th-cluster", iconSize: L.point(30, 30) });
                            }
                        }
                        var markerCluster = new L.MarkerClusterGroup(defaultParam);

                        $.each(markers, function (i, a) { markerCluster.addLayer(a); })

                        map.addToLayerGroup(markerCluster);

                        return markerCluster;
                    },
                    addEllipse: function (param) {
                        var defaultParam = {
                            lng: null,
                            lat: null,
                            radii_w: null,
                            radii_h: null,
                            radius: 0,
                            options: null
                        };

                        $.extend(defaultParam, param);

                        var elli = L.ellipse([defaultParam.lat, defaultParam.lng], [defaultParam.radii_w, defaultParam.radii_h], defaultParam.radius, defaultParam.options);

                        map.addToLayerGroup(elli);

                        return elli;
                    }
                })

                $.extend(map, {
                    addHeatMap: function (param) {
                        var defaultOptions = {
                            "radius": 20,
                            "maxOpacity": .8,
                            "scaleRadius": true,
                            "useLocalExtrema": true,
                            'latField': 'lat',
                            'lngField': 'lng',
                            'valueField': 'value'
                        };

                        var defaultParam = {
                            options: {}
                        };

                        $.extend(defaultParam, param);

                        $.extend(defaultOptions, param.options);

                        defaultParam.options = defaultOptions;

                        var _heatLayer = new HeatmapOverlay(defaultParam.options);

                        map.addToLayerGroup(_heatLayer);

                        return _heatLayer;

                    }
                })

                $.extend(map, {
                    toolDistance: function (param) {
                        var defaultParam = {
                            clearAfterComplete: false,
                            click: function () { },
                            complete: function () { }
                        };

                        $.extend(defaultParam, param)

                        var _points = [];
                        var total_distancet = 0;//总长度
                        var _distanceGroup = L.layerGroup();
                        this.addToLayerGroup(_distanceGroup);
                        var _polyline = L.polyline(_points, { color: '#ff0000', opacity: '0.8', weight: '3' });
                        var _movePolyline = L.polyline(_points, { color: '#f68e15', weight: '3' });;
                        var _moveMarkIcon = _createDivIcon([0, 0], "", null, [0, 10]);
                        var _lastMarkIcon = null;
                        _distanceGroup.addLayer(_polyline);
                        _distanceGroup.addLayer(_movePolyline);
                        _distanceGroup.addLayer(_moveMarkIcon);

                        var _container = map.map.getContainer();
                        _initAction();
                        function _initAction() {
                            _container.style.cursor = "hander";
                            map.map.dragging.disable();
                            map.map.doubleClickZoom.disable();
                            _onmousedown();
                        }

                        function _createPointIcon(pos) {
                            return L.marker(pos, { icon: L.divIcon({ className: "tmap_distance", iconAnchor: [10, 10], iconSize: [8, 8], html: "<div style='border:1px solid red; width:8px;height:8px;border-radius:10px;'></div>" }) });
                        }

                        function _createDivIcon(pos, text, size, iconAnchor) {
                            return L.marker(pos, { icon: _createIcon(text, size, iconAnchor) });
                        }

                        function _createIcon(text, size, iconAnchor) {
                            return L.divIcon({ className: "tmap_distance", iconAnchor: iconAnchor, iconSize: size ? size : [180, 20], html: '<div><span style="background-color:rgba(255,255,255,1);padding:2px 3px;font-size:12px; border-radius:5px; border:1px solid #888888; color:#888888;">' + text + '</span></div>' });
                        }

                        function _onmousedown() {
                            //map.drawPoint();
                            _onMouseMove();
                            map.on("click", function (e) {
                                if (_points.length > 1 && e.latlng.lat == _points[_points.length - 1].lat) return;
                                _points.push(e.latlng);
                                defaultParam.click(_points.length, e.latlng);
                                if (_points.length == 1) {
                                    _ondbclick();

                                    _distanceGroup.addLayer(_createDivIcon(e.latlng, "起点", null, [30, 30]));

                                    //                                _distanceGroup.addLayer(_createPointIcon(e.latlng));

                                    //_distanceGroup.addLayer(new L.Marker(e.latlng));

                                }
                                else {
                                    _polyline.setLatLngs(_points);
                                    _movePolyline.setLatLngs([]);
                                    var _endPoint = _points[_points.length - 2];
                                    var point_a = new L.LatLng(e.latlng.lat, e.latlng.lng);
                                    var point_b = new L.LatLng(_endPoint.lat, _endPoint.lng);
                                    var _distancet = parseFloat(point_b.distanceTo(point_a));
                                    total_distancet = parseFloat(total_distancet + _distancet);
                                    //"<font style='color:#ff0000;font-weight:bold;'>"+_points.length+"</font>"+
                                    _lastMarkIcon = _createDivIcon(e.latlng, _getDistanceText(total_distancet), null, [30, 30]);
                                    _distanceGroup.addLayer(_lastMarkIcon);
                                    //                                _distanceGroup.addLayer(_createPointIcon(e.latlng));
                                    //_distanceGroup.addLayer(new L.Marker(e.latlng));
                                }
                            })
                        }

                        function _onMouseMove() {
                            map.on("mousemove", function (e) {
                                if (_points.length == 0) {
                                    _moveMarkIcon.setLatLng(e.latlng);
                                    _moveMarkIcon.setIcon(_createIcon("点击地图标记起点", null, [30, 30]));
                                    return;
                                }
                                var _endPoint = _points[_points.length - 1];
                                if (_points.length >= 1) {
                                    _movePolyline.setLatLngs([_endPoint, e.latlng]);
                                    _moveMarkIcon.setLatLng(e.latlng);
                                    var point_a = new L.LatLng(e.latlng.lat, e.latlng.lng);
                                    var point_b = new L.LatLng(_endPoint.lat, _endPoint.lng);
                                    var _distancet = parseFloat(point_b.distanceTo(point_a));
                                    _moveMarkIcon.setIcon(_createIcon("当前" + _getDistanceText(_distancet), null, [30, 30]));

                                }
                            })
                        }

                        function _ondbclick() {
                            map.on("dblclick", function (e) {
                                map.off("click");
                                map.off("mousemove");
                                map.off("dblclick");

                                if (defaultParam.clearAfterComplete) {
                                    map.removeFromLayerGroup(_distanceGroup);
                                }

                                if (_points.length > 1) {
                                    _movePolyline.setLatLngs([]);
                                    _moveMarkIcon.setLatLng([0, 0]);
                                    _polyline.setLatLngs(_points);
                                    _lastMarkIcon.setIcon(_createIcon("总长:" + _getDistanceText(total_distancet) + "<a class='close' style='background-color:red;color:#fff;margin-left:10px;padding:1px 5px;'>X</a>", null, [30, 30]));

                                    $(_lastMarkIcon._icon).find("a.close").click(function () {
                                        map.removeFromLayerGroup(_distanceGroup);
                                    })

                                    defaultParam.complete(total_distancet);
                                    //_distanceGroup.addLayer(new L.Marker(e.latlng));
                                }

                                map.map.dragging.enable();
                                setTimeout(function () {
                                    map.map.doubleClickZoom.enable();
                                }, 100);
                            })
                        }

                        function _getDistanceText(_d) {
                            if (_d < 1000) {
                                return _d.toFixed(2) + "米";
                            } else {
                                return (_d / 1000).toFixed(2) + "千米";
                            }
                        }
                    }
                })

            }

        },
        constant: {
            polyLineOption: {
                stroke: true,
                color: "#03f",
                weight: 5,
                opacity: 1,
                fillOpacity: 1
            },
            circleOption: {
                stroke: true,
                color: "#03f",
                weight: 5,
                opacity: 0.5,
                fillOpacity: 0.2
            },
            polygonOption: {
                stroke: true,
                color: "#03f",
                weight: 5,
                opacity: 0.5,
                fillOpacity: 0.2
            },
            rectangleOption: {
                stroke: true,
                color: "#03f",
                weight: 5,
                opacity: 0.5,
                fillOpacity: 0.2
            }
        },
        utility: {
            _isArray: function (obj) {
                return Object.prototype.toString.call(obj) === '[object Array]';
            },
            _cloneJsonObject: function (obj) {
                var re = {};
                for (var a in obj) {
                    re[a] = obj[a];
                }
                return re;
            },
            _getSiteBasePath: function () {
                var curWwwPath = window.document.location.href;
                var pathName = window.document.location.pathname;
                var pos = curWwwPath.indexOf(pathName);
                var localhostPaht = curWwwPath.substring(0, pos);
                var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
                return (localhostPaht + projectName);
            },
            _getFunction: function (code, argNames) {
                var fn = window, parts = (code || "").split(".");
                for (var i = 0; i < parts.length; i++) {
                    if (!fn) return null;
                    if (parts[i] == 'window') continue;
                    fn = fn[parts[i]];
                    alert(fn.toString())
                }
                if (typeof (fn) === "function") {
                    return fn;
                }
                argNames.push(code);
                return Function.constructor.apply(null, argNames);
            },
            _hefei_crs: function () {
                return new L.Proj.CRS('EPSG:21460',
                '+proj=tmerc +lat_0=0 +lon_0=117 +k=1 +x_0=500000 +y_0=0 +ellps=krass +towgs84=15.8,-154.4,-82.3,0,0,0,0 +units=m +no_defs',
                {
                    origin: [259950, 3842516.87],
                    resolutions: [
                        2328.30625,
                        1164.153125,
                        582.0765625,
                        291.03828125,
                        145.519140625,
                        72.7595703125,
                        36.37978515625,
                        18.189892578125,
                        9.0949462890625,
                        4.54747314453125,
                        2.273736572265625,
                        1.1368682861328125,
                        0.56843414306640625,
                        0.284217071533203125,
                        0.1421085357666015625
                    ]
                });
            }

        }
    });
})(jQuery);


(function () {
    $.extend($.tMap, {
        SitePath: $.tMap.utility._getSiteBasePath(),
        tileLayer_crs: {
            //tianditu_jwd: L.CRS.EPSG4326,
            //tianditu_wx_jwd: L.CRS.EPSG4326,
            //tianditu_dx_jwd: L.CRS.EPSG4326,
            tianditu_WTMS_jwd: L.CRS.EPSG4326,
            tianditu_wx_WTMS_jwd: L.CRS.EPSG4326,
            tianditu_dx_WTMS_jwd: L.CRS.EPSG4326
        }
    })
})(jQuery);


(function () {
    $.extend($.tMap, {
        // 图慧地图
        tileLayer_th: function () {
            return L.tileLayer('http://www.tuhuitech.com:18080{url}.png', {
                minZoom: 7,
                maxZoom: 17,
                url: function (data) {
                    return $.tMap.map_url_th(data);
                }
            });
        },
        // 谷歌街道图
        tileLayer_google: function () {  //s@111层是影像层，m@177000000是线划图层，h@177000000是注记层，r@177000000是地形层
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=m@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 22
            });
        },
        // 谷歌街道图(大文字图)
        tileLayer_google_max: function () {
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=m@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo&style=2', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 22
            });
        },
        // 谷歌卫星图
        tileLayer_google_s: function () {
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=s@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 20
            });
        },
        // 谷歌地形图
        tileLayer_google_r: function () {
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=r@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 22
            });
        },
        // 谷歌标记图
        tileLayer_google_h: function () {
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=h@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 22
            });
        },
        // 谷歌卫星标记图
        tileLayer_google_sh: function () {
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=s@177000000,h@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 20
            });
        },
        // 谷歌卫星标记图(大文字图)
        tileLayer_google_sh_max: function () {
            return L.tileLayer('http://mt{s}.google.cn/vt/lyrs=s@177000000,h@177000000&hl=zh-CN&gl=cn&src=app&x={x}&y={y}&z={z}&s=Galileo&style=2', {
                subdomains: '123',
                minZoom: 0,
                maxZoom: 22
            });
        },
        // 新浪地图
        tileLayer_sina: function () {
            return L.tileLayer(' http://dituapi.iask.com:8080/mapabc/maptile?v=w2.61&z={z}&x={x}&y={y}', {
                minZoom: 0,
                maxZoom: 18
            });
        },
        tileLayer_qq: function () {

            return L.tileLayer(' http://dituapi.iask.com:8080/mapabc/maptile?v=w2.61&z={z}&x={x}&y={y}', {
                maxZoom: 22
            });
        },
        tileLayer_baidu: function () {
            return L.tileLayer('http://online{s}.map.bdimg.com/tile/?qt=tile&{url}&styles=pl&udt=20130712', {  //地形图
                subdomains: '01234',
                maxZoom: 17,
                minZoom: 2,
                url: function (data) {
                    return $.tMap.map_url_baidu(data);
                }
            });
        },
        // 微软bing地图
        tileLayer_bing: function () {
            return L.tileLayer('http://t{s}.tiles.ditu.live.com/tiles/r{url}.png?g=102&mkt=zh-cn&n=z', {
                minZoom: 0,
                maxZoom: 17,
                subdomains: '0123',
                url: function (data) {
                    return $.tMap.map_url_bing(data);
                }
            })
        },
        // 高德地图
        tileLayer_autonavi: function () {
            return L.tileLayer('http://webrd0{s}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={x}&y={y}&z={z}', {  //街道图
                subdomains: '1234',
                minZoom: 0,
                maxZoom: 18
            })

        },
        // 高德地图-大图
        tileLayer_autonavi_max: function () {
            return L.tileLayer('http://webst0{s}.is.autonavi.com/appmaptile?style=7&x={x}&y={y}&z={z}', {
                subdomains: '1234',
                minZoom: 0,
                maxZoom: 20
            });

        },
        // 高德地图-卫星
        tileLayer_autonavi_wx: function (type) {
            return [L.tileLayer('http://webst0{s}.is.autonavi.com/appmaptile?style=6&x={x}&y={y}&z={z}', {
                subdomains: '1234',
                minZoom: 0,
                maxZoom: 18
            }), L.tileLayer('http://webst0{s}.is.autonavi.com/appmaptile?style=8&x={x}&y={y}&z={z}', {
                subdomains: '1234',
                minZoom: 0,
                maxZoom: 18
            })];

        },
        // 高德地图-标记图
        tileLayer_autonavi_bj: function (type) {
            return L.tileLayer('http://webst0{s}.is.autonavi.com/appmaptile?style=8&x={x}&y={y}&z={z}', {
                subdomains: '1234',
                minZoom: 0,
                maxZoom: 18
            });

        },
        // 天地-街道图
        tileLayer_tianditu: function () {
            return [L.tileLayer('http://t{s}.tianditu.com/DataServer?T=vec_w&x={x}&y={y}&l={z}', {
                subdomains: '01234567',
                maxZoom: 18
            }), L.tileLayer('http://t{s}.tianditu.com/DataServer?T=cva_w&x={x}&y={y}&l={z}', {
                subdomains: '01234567',
                maxZoom: 18
            })];
        },
        // 天地-街道图-经纬度
        tileLayer_tianditu_jwd: function () {
            return [L.tileLayer('http://t{s}.tianditu.com/DataServer?T=vec_c&x={x}&y={y}&l={z}', {
                subdomains: '01234567',
                maxZoom: 18
            }), L.tileLayer('http://t{s}.tianditu.com/DataServer?T=cva_c&x={x}&y={y}&l={z}', {
                subdomains: '01234567',
                maxZoom: 18
            })];
        },
        // 天地-卫星图
        tileLayer_tianditu_wx: function () {
            return [L.tileLayer('http://t{s}.tianditu.com/DataServer?T=img_w&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            }), L.tileLayer('http://t{s}.tianditu.com/DataServer?T=cia_w&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            })];
        },
        // 天地-卫星图-经纬度
        tileLayer_tianditu_wx_jwd: function () {
            return [L.tileLayer('http://t{s}.tianditu.com/DataServer?T=img_c&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            }), L.tileLayer('http://t{s}.tianditu.com/DataServer?T=cia_c&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            })];
        },
        // 天地-地形图
        tileLayer_tianditu_dx: function () {
            return [L.tileLayer('http://t{s}.tianditu.com/DataServer?T=ter_w&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            }), L.tileLayer('http://t{s}.tianditu.com/DataServer?T=cta_w&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            })];
        },
        // 天地-地形图-经纬度
        tileLayer_tianditu_dx_jwd: function () {
            return [L.tileLayer('http://t{s}.tianditu.com/DataServer?T=ter_c&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            }), L.tileLayer('http://t{s}.tianditu.com/DataServer?T=cta_c&X={x}&Y={y}&L={z}', {
                subdomains: '01234567',
                maxZoom: 18
            })];
        },
        // 天地-街道图-WTMS
        tileLayer_tianditu_WTMS: function () {
            var matrixIds3857 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds3857[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(20037508, -20037508)
                };
            }
            return [L.tileLayer.wtms('http://t{s}.tianditu.cn/vec_w/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'vec',
                tilematrixSet: "w",
                matrixIds: matrixIds3857,
                format: 'tiles'
            }), L.tileLayer.wtms('http://t{s}.tianditu.cn/cva_w/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'cva',
                tilematrixSet: "w",
                matrixIds: matrixIds3857,
                format: 'tiles'
            })];
        },
        // 天地-街道图-WTMS-经纬度
        tileLayer_tianditu_WTMS_jwd: function () {
            var matrixIds4326 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds4326[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(90, -180)
                };
            }
            return [L.tileLayer.wtms('http://t{s}.tianditu.cn/vec_c/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'vec',
                tilematrixSet: "c",
                matrixIds: matrixIds4326,
                format: 'tiles'
            }), L.tileLayer.wtms('http://t{s}.tianditu.cn/cva_c/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'cva',
                tilematrixSet: "c",
                matrixIds: matrixIds4326,
                format: 'tiles'
            })];
        },
        // 天地-卫星图-WTMS
        tileLayer_tianditu_wx_WTMS: function () {
            var matrixIds3857 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds3857[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(20037508, -20037508)
                };
            }
            return [L.tileLayer.wtms('http://t{s}.tianditu.cn/img_w/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'img',
                tilematrixSet: "w",
                matrixIds: matrixIds3857,
                format: 'tiles'
            }), L.tileLayer.wtms('http://t{s}.tianditu.cn/cia_w/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'cia',
                tilematrixSet: "w",
                matrixIds: matrixIds3857,
                format: 'tiles'
            })];
        },
        // 天地-卫星-WTMS-经纬度
        tileLayer_tianditu_wx_WTMS_jwd: function () {
            var matrixIds3857 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds3857[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(20037508, -20037508)
                };
            }
            var matrixIds4326 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds4326[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(90, -180)
                };
            }
            return [L.tileLayer.wtms('http://t{s}.tianditu.cn/img_c/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'img',
                tilematrixSet: "c",
                matrixIds: matrixIds4326,
                format: 'tiles'
            }), L.tileLayer.wtms('http://t{s}.tianditu.cn/cia_c/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'cia',
                tilematrixSet: "c",
                matrixIds: matrixIds4326,
                format: 'tiles'
            })];
        },
        // 天地-地形-WTMS
        tileLayer_tianditu_dx_WTMS: function () {
            var matrixIds3857 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds3857[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(20037508, -20037508)
                };
            }
            return [L.tileLayer.wtms('http://t{s}.tianditu.cn/ter_w/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'ter',
                tilematrixSet: "w",
                matrixIds: matrixIds3857,
                format: 'tiles'
            }), L.tileLayer.wtms('http://t{s}.tianditu.cn/cta_w/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'cta',
                tilematrixSet: "w",
                matrixIds: matrixIds3857,
                format: 'tiles'
            })];
        },
        // 天地-地形-WTMS-经纬度
        tileLayer_tianditu_dx_WTMS_jwd: function () {
            var matrixIds3857 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds3857[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(20037508, -20037508)
                };
            }
            var matrixIds4326 = new Array(22);
            for (var i = 0; i < 22; i++) {
                matrixIds4326[i] = {
                    identifier: "" + i,
                    topLeftCorner: new L.LatLng(90, -180)
                };
            }
            return [L.tileLayer.wtms('http://t{s}.tianditu.cn/ter_c/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'ter',
                tilematrixSet: "c",
                matrixIds: matrixIds4326,
                format: 'tiles'
            }), L.tileLayer.wtms('http://t{s}.tianditu.cn/cta_c/wmts', {
                maxZoom: 18,
                minZoom: 7,
                subdomains: '01234567',
                style: 'default',
                layer: 'cta',
                tilematrixSet: "c",
                matrixIds: matrixIds4326,
                format: 'tiles'
            })];
        },
        map_url_th: function (data) {
            var z = data.z,
                   x = data.x,
                   y = Math.pow(2, z) - data.y - 1;
            var shift = z / 2;
            var half = 2 << shift;
            var digits = 1;
            if (half > 10) {
                digits = Math.ceil(Math.floor(Math.log(half) / Math.log(10)) + 1);
            }
            var halfx = Math.floor(x / half);
            var halfy = Math.floor(y / half);

            return "/EPSG_900913_" + zeroPadder(z, 2, "") + "/" + zeroPadder(halfx, digits, "") + "_" + zeroPadder(halfy, digits, "") + "/" + zeroPadder(x, 2 * digits, "") + "_" + zeroPadder(y, 2 * digits, "");

            function zeroPadder(number, order, padding) {
                var numberOrder = 1;
                if (number > 9) {
                    if (number > 11) {
                        numberOrder = number.toString().length;
                    } else {
                        numberOrder = 2;
                    }
                }

                var diffOrder = order - numberOrder;
                if (diffOrder > 0) {
                    while (diffOrder > 0) {
                        padding = padding + "" + ('0');
                        diffOrder--;
                    }
                    padding = padding + "" + (number);
                } else {
                    padding = padding + "" + (number);
                }
                return padding;
            }
        },
        map_url_baidu_ags_row: [[0, 0], [1, 0], [3, 1], [6, 3], [13, 6], [26, 12], [52, 24], [105, 49], [210, 98], [421, 197], [843, 395], [1685, 790], [3372, 1581], [6744, 3163], [13489, 6327], [26979, 12654], [53958, 25308], [107917, 50617]],
        map_url_baidu_ags_col: [[0, 0], [0, 0], [1, 0], [2, 1], [5, 2], [12, 4], [24, 9], [48, 18], [97, 36], [194, 73], [387, 147], [776, 294], [1551, 589], [3103, 1178], [6207, 2356], [12415, 4712], [24830, 9425], [49661, 18851]],
        map_url_baidu: function (data) {
            var nz = data.z + 1;
            var ck_row = this.map_url_baidu_ags_row[data.z];
            var ck_col = this.map_url_baidu_ags_col[data.z];
            var nx = data.x + parseInt(ck_row[1]) - parseInt(ck_row[0]);
            var ny = parseInt(ck_col[0]) + parseInt(ck_col[1]) - data.y;
            return "x=" + nx + "&y=" + ny + "&z=" + nz;

            var zoom = data.z - 1;
            var offsetX = Math.pow(2, zoom);
            var offsetY = offsetX - 1;
            var numX = data.x - offsetX;
            var numY = (-data.y) + offsetY;
            zoom = data.z + 1;
            return "x=" + numX + "&y=" + numY + "&z=" + zoom;
        },
        map_url_bing: function (data) {
            var qcode = tileXYToQuadKey(data.x, data.y, data.z);
            return qcode;

            function tileXYToQuadKey(tileX, tileY, levelOfDetail) {
                var quadKey = "";
                for (var i = levelOfDetail; i > 0; i--) {
                    var digit = 0;
                    var mask = 1 << (i - 1);
                    if ((tileX & mask) != 0) {
                        digit++;
                    }
                    if ((tileY & mask) != 0) {
                        digit++;
                        digit++;
                    }
                    quadKey += digit.toString();
                }
                return quadKey;
            }
        }
    });
})(jQuery);