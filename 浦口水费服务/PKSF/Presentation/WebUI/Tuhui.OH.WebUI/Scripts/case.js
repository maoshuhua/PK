$.case = {};
(function () {
    var _this = $.case;
    $.extend($.case, {
        pageInit: function () {
            $("#map body").css({
                height: document.documentElement.clientHeight + "px",
                width: document.documentElement.clientWidth + "px"
            })
        },
        mapInit: function (param) {
            this.map = $.tMap.init("map", $.extend({
                defaultLayer: 'google'
            }, param))
        },
        //数据访问
        TMAP_RES_SOURCES: [],
        tmap_SearchAllResource: function () {
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
                    _this.TMAP_RES_SOURCES = response.resources;
                    // alert(_this.TMAP_RES_SOURCES.length);
                }
            })
        }
    })
})(jQuery);