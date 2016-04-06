L.TmapCircle = L.Circle.extend({
    options: {
        latlng: null,
        radius: 0,
        styleOptions: {},
        editEnable: false,
        editIcon: new L.DivIcon({
            iconSize: new L.Point(8, 8),
            className: 'tmap-div-icon tmap-editing-icon'
        }),
        editLabelIcon: new L.DivIcon({
            iconSize: new L.Point(50, 20),
            iconAnchor: new L.Point(58, 58),
            className: 'tmap-div-icon tmap-editing-icon',
            html: "李根华"
        }),
        editDragStart: function () {

        },
        editDrag: function () {

        },
        editDragEnd: function () {

        }
    },
    initialize: function (options) {
        L.setOptions(this, options);
        L.Path.prototype.initialize.call(this, options.styleOptions);
        this._latlng = L.latLng(options.latlng);
        this._mRadius = options.radius;
    },
    onAdd: function (map) {
        L.Path.prototype.onAdd.call(this, map);
        if (this.options.editEnable) {
            this.addEditHooks();
        }
    },
    onRemove: function (map) {
        if (this.options.editEnable) {
            this.removeEditHooks();
        }

        L.Path.prototype.onRemove.call(this, map);
    },
    addEditHooks: function () {
        if (this._map) {
            if (!this._editMarkerGroup) {
                this._initEditMarkers();
            }
            this._map.addLayer(this._editMarkerGroup);
        }
    },
    removeEditHooks: function () {
        if (this._map) {
            this._map.removeLayer(this._editMarkerGroup);
            delete this._editMarkerGroup;
            delete this._byMarker;
        }
    },
    _initEditMarkers: function () {
        this._editMarkerGroup = new L.layerGroup();

        var circleBounds = this.getBounds(),
			swCoord = circleBounds.getSouthWest(),
			neCoord = circleBounds.getNorthEast(),
			northCenterCoord = new L.LatLng((neCoord.lat + swCoord.lat) / 2, neCoord.lng, true)

        this._byMarker = this._createEditMarker(northCenterCoord);
        this._editMarkerGroup.addLayer(this._byMarker);
        this._editMarkerGroup.addLayer(this._byMarker.radiusLabelMarker);
    },
    _createEditMarker: function (latlng) {
        var marker = L.marker(latlng, {
            draggable: true,
            icon: this.options.editIcon
        })

        marker.radiusLabelMarker = L.marker(latlng, {
            icon: this.options.editLabelIcon
        })
        marker.radiusLabelMarker.setOpacity(0);

        marker.on('dragstart', this._onbyMarkerMoveStart, this);
        marker.on('drag', this._onbyMarkerMove, this);
        marker.on('dragend', this._onbyMarkerMoveEnd, this);

        return marker;
    },
    _onbyMarkerMoveEnd: function (e) {
        var marker = e.target;
        marker.radiusLabelMarker.setOpacity(0);
        this.options.editDragEnd(this.getLatLng(), this.getRadius());
    },
    _onbyMarkerMoveStart: function () {
        this.options.editDragStart(this.getLatLng(), this.getRadius());
    },
    _onbyMarkerMove: function (e) {
        var marker = e.target;

        var center = this.getLatLng();
        var axis = marker._latlng;

        var distance = center.distanceTo(axis);

        this.setRadius(distance);

        this.redraw();

        if (marker.radiusLabelMarker) {
            marker.radiusLabelMarker.setOpacity(1);
            marker.radiusLabelMarker.setLatLng(marker._latlng);
            marker.radiusLabelMarker._icon.innerHTML = "<p style='color:red;'>" + distance + "</p>";
        }

        this.fire('radiuschange');
    }
})

L.Marker.include({
    l_rotate: function (deg) {
        $(this._icon).find(".div-icon-div").css({
            "transform": "rotateZ(" + deg + "deg)",
        })
    },
    l_rotate_ms: function (deg) {
        $(this._icon).find(".div-icon-div").css({
            "-ms-transform": "rotateZ(" + deg + "deg)",
        })
    },
    l_animate: function (state, type) {
        if (!type) type = "jump";

        if (!state) {
            $(this._icon).removeClass("tmap-th-marker-jump");
            $(this._icon).removeClass("tmap-th-marker-swing");
            $(this._icon).find(".div-icon-div").removeClass("tmap-th-marker-rotateX");
            $(this._icon).find(".div-icon-div").removeClass("tmap-th-marker-rotateY");
            $(this._icon).find(".div-icon-div").removeClass("tmap-th-marker-rotateZ");
            $(this._icon).find(".div-icon-div").removeClass("tmap-th-marker-scale");
            return;
        }

        if (type == "jump") {
            $(this._icon).addClass("tmap-th-marker-" + type);

        } else if (type == "swing") {
            $(this._icon).addClass("tmap-th-marker-" + type);

        } else if (type == "rotateX") {
            $(this._icon).find(".div-icon-div").addClass("tmap-th-marker-" + type);
        } else if (type == "rotateY") {
            $(this._icon).find(".div-icon-div").addClass("tmap-th-marker-" + type);
        } else if (type == "rotateZ") {
            $(this._icon).find(".div-icon-div").addClass("tmap-th-marker-" + type);
        } else if (type == "scale") {
            $(this._icon).find(".div-icon-div").addClass("tmap-th-marker-" + type);
        }

        return this;
    }
    //l_rotate: function (rotate) {
    //    $(this._icon).find(".div-icon-div").css({
    //        "-webkit-transform": "rotateZ(" + rotate + "deg)"
    //    })
    //}
})

L.TileLayer.WMTS = L.TileLayer.extend({

    defaultWmtsParams: {
        service: 'WMTS',
        request: 'GetTile',
        version: '1.0.0',
        layer: '',
        style: '',
        tilematrixSet: '',
        format: 'image/jpeg'
    },

    initialize: function (url, options) { // (String, Object)
        this._url = url;
        var wmtsParams = L.extend({}, this.defaultWmtsParams),
            tileSize = options.tileSize || this.options.tileSize;
        if (options.detectRetina && L.Browser.retina) {
            wmtsParams.width = wmtsParams.height = tileSize * 2;
        } else {
            wmtsParams.width = wmtsParams.height = tileSize;
        }
        for (var i in options) {
            // all keys that are not TileLayer options go to WMTS params
            if (!this.options.hasOwnProperty(i) && i != "matrixIds") {
                wmtsParams[i] = options[i];
            }
        }
        this.wmtsParams = wmtsParams;
        this.matrixIds = options.matrixIds;
        L.setOptions(this, options);
    },

    onAdd: function (map) {
        L.TileLayer.prototype.onAdd.call(this, map);
    },

    getTileUrl: function (tilePoint, zoom) { // (Point, Number) -> String
        var map = this._map;
        crs = map.options.crs;
        tileSize = this.options.tileSize;
        nwPoint = tilePoint.multiplyBy(tileSize);
        //+/-1 pour être dans la tuile
        nwPoint.x += 1;
        nwPoint.y -= 1;
        sePoint = nwPoint.add(new L.Point(tileSize, tileSize));
        nw = crs.project(map.unproject(nwPoint, zoom));
        se = crs.project(map.unproject(sePoint, zoom));
        tilewidth = se.x - nw.x;
        zoom = map.getZoom();
        ident = this.matrixIds[zoom].identifier;
        X0 = this.matrixIds[zoom].topLeftCorner.lng;
        Y0 = this.matrixIds[zoom].topLeftCorner.lat;
        tilecol = Math.floor((nw.x - X0) / tilewidth);
        tilerow = -Math.floor((nw.y - Y0) / tilewidth);
        url = L.Util.template(this._url, { s: this._getSubdomain(tilePoint) });

        return url + L.Util.getParamString(this.wmtsParams, url) + "&tilematrix=" + ident + "&tilerow=" + tilerow + "&tilecol=" + tilecol;
    },

    setParams: function (params, noRedraw) {
        L.extend(this.wmtsParams, params);
        if (!noRedraw) {
            this.redraw();
        }
        return this;
    }
});

L.tileLayer.wtms = function (url, options) {
    return new L.TileLayer.WMTS(url, options);
};