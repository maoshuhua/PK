(function () {
    var _this = $.demo;
    $.extend($.demo, {
        taxi_index: 1,
        taxi_loadStatus: false,
        taxi_recover: function () {
            this.taxi_index = 1;
            this.taxi_loadStatus = false;
            this.map.clear();
            this.taxi_markerList = [];
            this.taxi_markerListObj = {};
        },
        taxi_load: function (notFirst) {
            if (notFirst) this.taxi_loadStatus = true;
            $.ajax({
                type: 'post',
                url: ACTION_URL("GetCarGps", "Apply"),
                data: {
                    carcount: $("#taxi_count").val(),
                    carindex: _this.taxi_index
                },
                success: function (response) {
                    if (_this.taxi_loadStatus == false) return;
                    if (_this.taxi_markerList.length == 0) {
                        _this.taxi_map_addMarker(response);
                    } else {
                        _this.taxi_map_updateMarker(response);
                    }

                    _this.taxi_index++;

                    setTimeout(function () { _this.taxi_load(); }, parseInt($("#taxi_tick").val()) * 1000);

                }
            })
        },
        taxi_markerList: [],
        taxi_markerListObj: {},
        taxi_map_addMarker: function (list) {

            $.each(list, function (index, ele) {

                var _marker = _this.map.addImgMarker(ele.Lon, ele.Lat, {
                    icon: _this.taxi_map_createIcon()
                });

                //_marker.l_rotate(_this.getRandomNum(0, 360));

                _this.taxi_markerList = _marker;

                _this.taxi_markerListObj["taxi_" + ele.CarID] = _marker;

            })

        },
        taxi_map_updateMarker: function (list) {

            $.each(list, function (index, ele) {
                var _marker = _this.taxi_markerListObj["taxi_" + ele.CarID];
                if (_marker) {
                    _marker.setLatLng([ele.Lat, ele.Lon]);
                }
            })

        },
        taxi_map_createIcon: function () {
            var _r = Math.random();
            return {
                iconUrl: CONTENT_URL("Images/MapIcon/car_" + (_r > 0.5 ? "1" : "2") + ".png"),
                iconSize: [6, 12],
                iconAnchor: [6, 24]
            };
        }
    })
})(jQuery);

(function () {
    var _this = $.demo;
    $.extend($.demo, {
        ilayer_TILE:L.tileLayer('http://tm.mapabc.com/trafficengine/mapabc/traffictile?v=1.0&;t=1&zoom={zoom}&x={x}&y={y}', {
            minZoom: 7,
            maxZoom: 17,
            zoom: function (data) {
                return 17-data.z;
            }
        }),
        ilayer_load: function () {
            if ($("#sel_ilayer").val() == "") {
                this.map.removeLayer(this.ilayer_TILE);
            } else {
                this.map.addLayer(this.ilayer_TILE);
            }
        },
        changeTLayer: function () {
            this.map.changeTLayer($("#sel_changeTLayer").val().toString());
        }
    })
})(jQuery);