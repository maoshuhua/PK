
(function () {
    var _this = $.case;
    $.extend($.case, {
        connectionSocket1: function () {
            this.ws = $.w_socket({
                url: SOCKETIP + "TMAP/MUTISCREEN1",
                onstart: function () { },
                onopen: function () {
                },
                onclose: function () { },
                onerror: function () { },
                onmessage: function (data, content) {
                    if (data.to == 'MUTISCREEN1') {
                        if (data.action == 'Code_ChangeMapCenter') {
                            _this.map.moveTo(118 + _this.getRandomNum(1, 1000) * 0.001,
                                32 + _this.getRandomNum(1, 1000) * 0.001,
                                _this.getRandomNum(8, 16));
                        } else if (data.action == 'Code_ShowResourcePopup') {
                            var _length = _this.PD2_RESOURCES.length;
                            var _res = _this.PD2_RESOURCES[_this.getRandomNum(1, _length)];
                            var _mark = _this.res_markList["mark_" + _res.res_id];
                            if (_mark) {
                                _mark.openPopup();
                                _this.map.moveTo(_mark.attribute.x, _mark.attribute.y, 16);
                            }
                        }
                    }
                },
                onexception: function (ex) { },
                onnotsupport: function () { }
            })
        },
        startSocketAction1: function () {
            this.map.doEvent("zoomend", function (evt) {
                if (_this.ws) {
                    _this.ws.send({
                        from: "MUTISCREEN1",
                        to: 'MUTISCREEN2',
                        action: 'zoomend',
                        content: { zoom: _this.map.map.getZoom() }
                    })
                }
            })

            this.map.doEvent("moveend", function (evt) {
                if (_this.ws) {
                    var _c = _this.map.map.getCenter();
                    _this.ws.send({
                        from: "MUTISCREEN1",
                        to: 'MUTISCREEN2',
                        action: 'moveend',
                        content: { lng: _c.lng, lat: _c.lat, zoom: _this.map.map.getZoom() }
                    })
                }
            })
        },
        action_dragMarkerObject:null,
        action_dragMarker1: function () {
            this.action_dragMarkerObject = L.marker([32.04256820049613, 118.77319335937499], {
                icon: L.icon({
                    iconUrl: CONTENT_URL("images/case_mutiscreen/dragmarker.png"),
                    iconSize:[48,48]
                }),
                draggable: true
            });

            this.action_dragMarkerObject.on("drag", function () {
                var _latlng = _this.action_dragMarkerObject.getLatLng();
                _this.ws.send({
                    from: "MUTISCREEN1",
                    to: 'MUTISCREEN2',
                    action: 'dragmarker',
                    content: { lng: _latlng.lng, lat: _latlng.lat}
                })
            });

            this.action_dragMarkerObject.addTo(this.map.map);
        },
        action_dragMarker2: function () {
            this.action_dragMarkerObject = L.marker([32.04256820049613, 118.77319335937499], {
                icon: L.icon({
                    iconUrl: CONTENT_URL("images/case_mutiscreen/dragmarker.png"),
                    iconSize: [48, 48]
                })
            });
            this.action_dragMarkerObject.addTo(this.map.map);
        },
        action_dragMarker2_setLngLat: function (lat, lng) {
            this.action_dragMarkerObject.setLatLng([lat, lng]);
        },
        connectionSocket2: function () {
            this.ws = $.w_socket({
                url: SOCKETIP + "PD2/MUTISCREEN2",
                onstart: function () {
                    $("#message").html("开始连接");
                },
                onopen: function () {
                    $("#message").html("连接成功").animate({ 'opacity': '0' }, 2000);
                },
                onclose: function () {
                    $("#message").html("连接关闭");
                },
                onerror: function () {
                    $("#message").html("连接失败");
                },
                onmessage: function (data, content) {
                    if (data.to == 'MUTISCREEN2') {
                        if (data.action == 'zoomend') {
                            _this.map.map.setZoom(data.content.zoom);
                        } else if (data.action == 'moveend') {
                            _this.map.map.setView([data.content.lat, data.content.lng], data.content.zoom);
                        } else if (data.action == 'dragmarker') {
                            _this.action_dragMarker2_setLngLat(data.content.lat, data.content.lng);
                        }
                    }
                },
                onexception: function (ex) {
                    $("#message").html("Exception");
                },
                onnotsupport: function () { }
            })
        }
    })
})(jQuery);