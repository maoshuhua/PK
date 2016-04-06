(function (window) {
    L.RadarMarker = L.Marker.extend({
        options: {
            width: 50,
            height: 50,
            level: 3,
            duringTime:20,
            riseOnHover: true
        },
        initialize: function (latlng, options) {
            L.setOptions(this, options);
            this._latlng = L.latLng(latlng);

            this.options.icon = this._createDivIcon();
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
        },
        _createDivIcon: function () {
            return L.divIcon({
                className: 'radar-marker',
                html: _createHtml(this.options.level, this.options.duringTime),
                iconSize: [this.options.width, this.options.height],
                iconAnchor: [25, 0]
            })

            function _createHtml(_level, duringTime) {
                var _result = '';
                for (var i = 1; i <= _level; i++) {
                    _result += "<p class='cir' style='-webkit-animation-duration:" + (i / _level * duringTime) + "s;'></p>";
                }
                return _result;
            }

        }
    })
    L.radarMarker = function (latlng, options) {
        return new L.RadarMarker(latlng, options);
    };
})(window);

(function () {
    var _this = $.case;
    $.extend($.case, {
        radarDataList: [
            { lng: 118.77319335937499, lat: 32.04256820049613, level: 3 },
            { lng: 118.74795913696289, lat: 32.07268370653683, level: 5 },
            { lng: 118.83584976196288, lat: 32.04358676118635, level: 2 },
            { lng: 118.79877090454102, lat: 32.00487348917992, level: 7 },
            { lng: 118.69766235351561, lat: 32.00312664014669, level: 8 }
        ],
        radarInit: function () {
            $.each(this.radarDataList, function (index, ele) {
                L.radarMarker([ele.lat, ele.lng], {
                    level:ele.level
                }).addTo(_this.map.map);
            })
        }
    })
})(jQuery);