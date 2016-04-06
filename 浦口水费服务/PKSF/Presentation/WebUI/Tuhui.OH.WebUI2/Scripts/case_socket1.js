(function () {
    var _this = $.case;
    $.extend($.case, {
        ws: null,
        socketIp: "ws://www.tuhuitech.com:2013/",
        showMessage: function (msg) {
            $("#message").prepend("<p>" + msg + "</p>")
        },
        connectSocketServer: function () {
            var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

            if (support == null) {
                _this.showMessage("Your browser cannot support WebSocket!")
                return;
            }

            _this.showMessage("connection " + this.socketIp)
            // create a new websocket and connect
            this.ws = new window[support](this.socketIp);

            // when data is comming from the server, this metod is called
            this.ws.onmessage = function (evt) {
                _this.showMessage("message recieve success!")
                var result = eval("(" + evt.data + ")");

                if (result.Action == "Action_GetCarList") {
                    _this.loadCarList(result.data);
                } else if (result.Action == "Action_CarGps") {
                    showRuncar(result.data);
                } else if (result.Action == "Action_New_CarGps") {
                    _this.showRuncar(result.data);
                }
                // $("#result").html(evt.data);
            };

            // when the connection is established, this method is called
            this.ws.onopen = function () {
                _this.showMessage("connection success!")
                setTimeout(function () { _this.startLoadCarList() }, 1000);
            };

            this.ws.onerror = function (evt) {
                _this.showMessage("connection error!")
            };

            // when the connection is closed, this method is called
            this.ws.onclose = function () {
                _this.showMessage("connection close!")
            }
        },
        sendWsMessage: function (text) {
            if (this.ws) {
                this.ws.send(text);
            }
        },
        disconnectWebSocket: function () {
            if (this.ws) {
                this.ws.close();
            }
        },
        startLoadCarList: function () {
            this.showMessage("开始通过通道加载车辆数据...")
            this.sendWsMessage("Action_GetCarList|2000");
        },
        runCarCount: 0,
        startRunCarGps: function (count) {
            if (!count) {
                this.runCarCount++;
                count = this.runCarCount;
            }
            this.showMessage("【" + count + "】开始通过通道加载位置信息...");
            this.sendWsMessage("Action_New_CarGps|2000|" + count);
        },
        markList: [],
        loadCarList: function (carList) {
            for (var i = 0; i < carList.length; i++) {
                var a = carList[i];
                var title = a.Lat;
                var _iconUrl = this.getIcon();
                var marker = L.marker(new L.LatLng(a.Lon, a.Lat), { title: title, icon: new L.Icon({ iconUrl: _iconUrl, iconSize: this.getIconSize() }) });
                marker.iconUrl = _iconUrl;
                marker.bindPopup(title);
                this.markList.push(marker);
                this.map.map.addLayer(marker);
            }
            $("div[name='div_operator']").eq(0).show();
            setTimeout(function () { _this.startRunCarGps(); }, 1000);
        },
        showRuncar: function (data) {
            for (var i = 0; i < data.length; i++) {
                var a = data[i];
                this.markList[i].setLatLng(new L.LatLng(a.Lon, a.Lat));
            }
            if (this.autoUpdateFlag == false) return;
            setTimeout(function () { _this.startRunCarGps(); }, 1000);
        },
        autoUpdateFlag: true,
        autoUpdate: function (type) {
            if (type == 0) {
                this.autoUpdateFlag = false;
            } else if (type == 1) {
                this.autoUpdateFlag = true;
                setTimeout(function () { _this.startRunCarGps(); }, 500);
            }
            $("div[name='div_operator']").slideToggle();

        },
        zoomend: function () {
            if (markList.length != 0) {
                for (var i = 0; i < markList.length; i++) {
                    markList[i].setIcon(new L.Icon({ iconUrl: markList[i].iconUrl, iconSize: getIconSize() }));
                }
            }
        },
        getIconSize: function () {
            var _level = this.map.map.getZoom();
            var _p = _level / 17;
            if (_level != 17) _p = _p * 0.5;
            return [12 * _p, 24 * _p];
        },
        getIcon: function () {
            if (Math.random() > 0.5) {
                return carIcon[1];
            } else {
                return carIcon[0];
            }
        }
    })
})(jQuery);