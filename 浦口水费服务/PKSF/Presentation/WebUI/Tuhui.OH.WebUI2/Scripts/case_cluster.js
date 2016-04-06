
(function () {
    var _this = $.case;
    $.extend($.case, {
        case_clusterInit: function () {
            $.case.case_addmark();
        },
        TMAP_RES_SOURCES: [],
        case_addmark: function () {
            //debugger;
            var icontype;
            //_this.res_martobj = {};
            $.ajax({
                url: "http://www.tuhuitech.com:1011/pd2indexApi/home/GetAllResources?jsoncallback=?",
                cache: false,
                dataType: "jsonp",
                data: {

                },
                error: function (info) {
                    alert("GetAllResources:error!");
                },
                success: function (response) {
                    $.each(response.resources, function (index, ele) {
                        if (ele.res_type_id >= 200 && ele.res_type_id < 300) {
                            icontype = CONTENT_URL("images/mapicon/icon_200.png");
                        }
                        else if (ele.res_type_id==301) {
                            icontype = CONTENT_URL("images/mapicon/icon_300_car.png");
                        }
                        else if (ele.res_type_id > 300) {
                            icontype = CONTENT_URL("images/mapicon/icon_300.png");
                        }
                        else {
                            icontype=CONTENT_URL("images/mapicon/icon_" + ele.res_type_id + ".png");
                        }
                        _this.map.addMark(ele.res_lng, ele.res_lat, {
                            icon: {
                                iconUrl: icontype,
                                iconAnchor: [24, 24]
                            },
                            hover_icon:CONTENT_URL("images/mapicon/icon_100_sel.png"),
                            addoverlay: false
                        });

                    });
                }
            })

        }

    })

})(jQuery);