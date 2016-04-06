$.demo = {};
(function () {
    var _this = $.demo;
    $.extend($.demo, {

        pageInit: function () {
            $("body").css({
                height: "1080px",
                width: "3840px"
            })

            $("#map").css({
                height: "1080px",
                width: "1920px"
            })

            window.onresize = function () {
                $("#map,body").css({
                    height: document.documentElement.clientHeight + "px",
                    width: document.documentElement.clientWidth + "px"
                })
            }

            $(".index-content a.right").click(function () {
                $(".index-content").removeClass("index-content-show");
            })

            $(".index-nav a").click(function () {
                $.demo.loadPage($(this).attr("a_href"));
            })


        },
        mapInit: function () {
            var mercator = { x: 6798, y: 3367 };
            var lonLat = {};
            var x = mercator.x / 20037508.34 * 180;
            var y = mercator.y / 20037508.34 * 180;
            y = 180 / Math.PI * (2 * Math.atan(Math.exp(y * Math.PI / 180)) - Math.PI / 2);
            lonLat.x = x;
            lonLat.y = y;

            this.map = $.tMap.init("map", {
                defaultLayer: "autonavi",
            })

            _this.showHeatmap(0);

            this.map.map.on("click", function (evt) {
                $("#map_status").html("点击坐标:" + evt.latlng.lng + "," + evt.latlng.lat);
            })

        },
        showStatus: function (message) {
            $("#map_status").html(message);
        },

        //dateAdd: function (interval, number) {
        //    var d = this;
        //    var k = { 'y': 'FullYear', 'q': 'Month', 'm': 'Month', 'w': 'Date', 'd': 'Date', 'h': 'Hours', 'n': 'Minutes', 's': 'Seconds', 'ms': 'MilliSeconds' };
        //    var n = { 'q': 3, 'w': 7 };
        //    eval('d.set' + k[interval] + '(d.get' + k[interval] + '()+' + ((n[interval] || 1) * number) + ')');
        //    return d;
        //},



        setobjMonthPicTotal: function (date, type, time, n) {

            var data = [];
            var x = [];
            var today = new Date();
            var oneHour = 1000 * 60 * 60;
            var oneday = 1000 * 60 * 60 * 24;
            var oneMonth = 1000 * 60 * 60 * 24 * 30;

            var id;
            var name;

            if (time == 0) {
                $("#day").css("background-color", "#242424");
                $("#month").css("background-color", "");
                $("#year").css("background-color", "");
            }

            if (type == 1) {
                switch (n) {
                    case 1:
                        _this.KQ();
                        for (var i = 0; i < $("input[name='flag']").length; i++) {
                            $("input[name='flag']")[i].checked = false;
                        }
                        $("input[name='flag']")[0].checked = true;
                        break;
                    case 2:
                        _this.QC();
                        for (var i = 0; i < $("input[name='flag']").length; i++) {
                            $("input[name='flag']")[i].checked = false;
                        }
                        $("input[name='flag']")[6].checked = true;
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }
            }

            for (var i = 0; i < $("input[name='flag']").length; i++) {
                if ($("input[name='flag']")[i].checked == true) {
                    id = $("input[name='flag']")[i].id;
                    switch (id) {
                        case "CheckBox1":
                            name = "OHI";
                            if (date == "month") {
                                data = [{ 'color': '#E9C006', y: 138 }, { 'color': '#1ACF06', y: 76 }, { 'color': '#E9C006', y: 172 }, { 'color': '#EA1B13', y: 212 }, { 'color': '#E9C006', y: 154 },
                                { 'color': '#EA1B13', y: 217 }, { 'color': '#1ACF06', y: 83 }, { 'color': '#E9C006', y: 132 }, { 'color': '#E9C006', y: 169 }, { 'color': '#E9C006', y: 167 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#E9C006', y: 165 }, { 'color': '#E9C006', y: 186 }, { 'color': '#1ACF06', y: 93 }, { 'color': '#1ACF06', y: 85 }, { 'color': '#E9C006', y: 121 },
                                        { 'color': '#E9C006', y: 147 }, { 'color': '#E9C006', y: 123 }, { 'color': '#EA1B13', y: 228 }, { 'color': '#EA1B13', y: 236 }, { 'color': '#1ACF06', y: 98 },
                                        { 'color': '#E9C006', y: 136 }, { 'color': '#E9C006', y: 159 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#E9C006', y: 118 }, { 'color': '#E9C006', y: 136 }, { 'color': '#1ACF06', y: 98 }, { 'color': '#E9C006', y: 102 }, { 'color': '#1ACF06', y: 91 },
                                    { 'color': '#E9C006', y: 157 }, { 'color': '#E9C006', y: 163 }, { 'color': '#E9C006', y: 178 }, { 'color': '#E9C006', y: 189 }, { 'color': '#EA1B13', y: 204 },
                                    { 'color': '#EA1B13', y: 210 }, { 'color': '#E9C006', y: 192 }];
                            }
                            break;
                        case "CheckBox2":
                            name = "PM2.5";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 35 }, { 'color': '#0198DC', y: 11 }, { 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 8 },
                                { 'color': '#0198DC', y: 17 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 9 }, { 'color': '#0198DC', y: 14 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 16 }, { 'color': '#0198DC', y: 33 }, { 'color': '#0198DC', y: 15 }, { 'color': '#0198DC', y: 41 },
                                        { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 18 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 24 },
                                        { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 12 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 26 }, { 'color': '#0198DC', y: 31 }, { 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 21 },
                                    { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 7 }, { 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 29 }, { 'color': '#0198DC', y: 14 },
                                    { 'color': '#0198DC', y: 10 }, { 'color': '#0198DC', y: 26 }];
                            }
                            break;
                        case "CheckBox3":
                            name = "PM10";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 35 }, { 'color': '#0198DC', y: 11 }, { 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 8 },
                                { 'color': '#0198DC', y: 17 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 9 }, { 'color': '#0198DC', y: 14 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 16 }, { 'color': '#0198DC', y: 33 }, { 'color': '#0198DC', y: 15 }, { 'color': '#0198DC', y: 41 },
                                        { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 18 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 24 },
                                        { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 12 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 26 }, { 'color': '#0198DC', y: 31 }, { 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 21 },
                                    { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 7 }, { 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 29 }, { 'color': '#0198DC', y: 14 },
                                    { 'color': '#0198DC', y: 10 }, { 'color': '#0198DC', y: 26 }];
                            }
                            break;
                        case "CheckBox4":
                            name = "SO2";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 72 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 69 }, { 'color': '#0198DC', y: 34 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 86 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 45 }, { 'color': '#0198DC', y: 81 },
                                        { 'color': '#0198DC', y: 57 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                        { 'color': '#0198DC', y: 70 }, { 'color': '#0198DC', y: 22 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 41 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                    { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 83 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                    { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 42 }];
                            }
                            break;
                        case "CheckBox5":
                            name = "NO2";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 35 }, { 'color': '#0198DC', y: 11 }, { 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 8 },
                                { 'color': '#0198DC', y: 17 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 9 }, { 'color': '#0198DC', y: 14 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 16 }, { 'color': '#0198DC', y: 33 }, { 'color': '#0198DC', y: 15 }, { 'color': '#0198DC', y: 41 },
                                        { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 18 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 24 },
                                        { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 12 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 26 }, { 'color': '#0198DC', y: 31 }, { 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 21 },
                                    { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 7 }, { 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 29 }, { 'color': '#0198DC', y: 14 },
                                    { 'color': '#0198DC', y: 10 }, { 'color': '#0198DC', y: 26 }];
                            }
                            break;
                        case "CheckBox6":
                            name = "CO";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 72 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 69 }, { 'color': '#0198DC', y: 34 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 86 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 45 }, { 'color': '#0198DC', y: 81 },
                                        { 'color': '#0198DC', y: 57 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                        { 'color': '#0198DC', y: 70 }, { 'color': '#0198DC', y: 22 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 41 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                    { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 83 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                    { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 42 }];
                            }
                            break;
                        case "CheckBox7":
                            name = "CO";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 72 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 69 }, { 'color': '#0198DC', y: 34 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 86 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 45 }, { 'color': '#0198DC', y: 81 },
                                        { 'color': '#0198DC', y: 57 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                        { 'color': '#0198DC', y: 70 }, { 'color': '#0198DC', y: 22 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 41 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                    { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 83 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                    { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 42 }];
                            }
                            break;
                        case "CheckBox8":
                            name = "NO";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 35 }, { 'color': '#0198DC', y: 11 }, { 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 8 },
                                { 'color': '#0198DC', y: 17 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 9 }, { 'color': '#0198DC', y: 14 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 16 }, { 'color': '#0198DC', y: 33 }, { 'color': '#0198DC', y: 15 }, { 'color': '#0198DC', y: 41 },
                                        { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 18 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 24 },
                                        { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 12 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 26 }, { 'color': '#0198DC', y: 31 }, { 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 21 },
                                    { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 7 }, { 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 29 }, { 'color': '#0198DC', y: 14 },
                                    { 'color': '#0198DC', y: 10 }, { 'color': '#0198DC', y: 26 }];
                            }
                            break;
                        case "CheckBox9":
                            name = "CO2";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 72 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 69 }, { 'color': '#0198DC', y: 34 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 86 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 45 }, { 'color': '#0198DC', y: 81 },
                                        { 'color': '#0198DC', y: 57 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                        { 'color': '#0198DC', y: 70 }, { 'color': '#0198DC', y: 22 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 41 }, { 'color': '#0198DC', y: 62 }, { 'color': '#0198DC', y: 71 },
                                    { 'color': '#0198DC', y: 97 }, { 'color': '#0198DC', y: 83 }, { 'color': '#0198DC', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                                    { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 42 }];
                            }
                            break;
                        case "CheckBox10":
                            name = "CH";
                            if (date == "month") {
                                data = [{ 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 35 }, { 'color': '#0198DC', y: 11 }, { 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 8 },
                                { 'color': '#0198DC', y: 17 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#0198DC', y: 9 }, { 'color': '#0198DC', y: 14 }];
                            } else if (date == "year") {
                                data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 16 }, { 'color': '#EB9301', y: 33 }, { 'color': '#0198DC', y: 15 }, { 'color': '#EB9301', y: 41 },
                                        { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 18 }, { 'color': '#EB9301', y: 39 }, { 'color': '#0198DC', y: 24 },
                                        { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 12 }];
                            } else if (date == "day") {
                                data = [{ 'color': '#0198DC', y: 28 }, { 'color': '#0198DC', y: 26 }, { 'color': '#0198DC', y: 31 }, { 'color': '#0198DC', y: 12 }, { 'color': '#0198DC', y: 21 },
                                    { 'color': '#0198DC', y: 27 }, { 'color': '#0198DC', y: 7 }, { 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 29 }, { 'color': '#0198DC', y: 14 },
                                    { 'color': '#0198DC', y: 10 }, { 'color': '#0198DC', y: 26 }];
                            }
                            break;
                    }
                }
            }
            $("#heatmaphead").html(name + "柱状图");

            switch (date) {
                case "month":
                    for (var i = 0; i < 10; i++) {
                        var s = new Date(today - oneday * 3 * i).getDate();
                        x.push(s + "日");
                    }
                    x.reverse();
                    //data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#EB9301', y: 72 }, { 'color': '#EB9301', y: 62 }, { 'color': '#EB9301', y: 71 },
                    //    { 'color': '#EB9301', y: 97 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 44 }, { 'color': '#EB9301', y: 69 }, { 'color': '#0198DC', y: 34 }];
                    break;
                case "year":
                    for (var i = 0; i < 12; i++) {
                        var s = new Date(today - oneMonth * i).getMonth() + 1;
                        x.push(s + "月");
                    }
                    x.reverse();
                    //data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#EB9301', y: 86 }, { 'color': '#0198DC', y: 23 }, { 'color': '#0198DC', y: 45 }, { 'color': '#EB9301', y: 81 },
                    //    { 'color': '#EB9301', y: 57 }, { 'color': '#0198DC', y: 23 }, { 'color': '#EB9301', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                    //    { 'color': '#EB9301', y: 70 }, { 'color': '#0198DC', y: 22 }];
                    break;
                case "day":
                    for (var i = 0; i < 12; i++) {
                        var s = new Date(today - oneHour * 2 * i).getHours();
                        x.push(s + ":00");
                    }
                    x.reverse();
                    //data = [{ 'color': '#0198DC', y: 38 }, { 'color': '#0198DC', y: 36 }, { 'color': '#0198DC', y: 41 }, { 'color': '#EB9301', y: 62 }, { 'color': '#EB9301', y: 71 },
                    //    { 'color': '#EB9301', y: 97 }, { 'color': '#EB9301', y: 83 }, { 'color': '#EB9301', y: 58 }, { 'color': '#0198DC', y: 39 }, { 'color': '#0198DC', y: 34 },
                    //    { 'color': '#0198DC', y: 40 }, { 'color': '#0198DC', y: 42 }];
                    break;
            }

            $('#objMonthPicTotal').highcharts({
                chart: {
                    type: 'column',
                    /**borderColor: '#21814C',
                    borderWidth: 2**/
                },
                title: {
                    text: ''
                },
                //colors: [
                //    '#EB9301',
                //    '#0198DC',
                //],
                xAxis: {
                    categories: x
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    formatter: function () {
                        return '时间：' + this.x + '<br/>' + name + '：' + this.y;
                    }
                },
                credits: {
                    enabled: false
                },

                series: [{
                    name: name + '平均值：' + parseInt(Math.random() * 100 + 1),
                    colorByPoint: true,
                    data: data
                }],

            });

        },

        setobjMonthPicTotal_area: function () {
            var chart;
            var dataValue = [['黄标车', 65], ['无标车', 32], ['绿标车', 903]];
            $('#MonthPicTotal_area').highcharts({
                chart: {
                    type: 'pie',
                    marginTop: 30,
                    options3d: {
                        enabled: true,
                        alpha: 55,
                        beta: 0
                    }
                },
                title: {
                    text: '<p style="font-size:20px; color:white; font-weight: 600;">机动车标志比例图</p>'
                },
                credits: {
                    enabled: false
                },
                tooltip: {
                    pointFormat: ''
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        innerSize: 40,
                        depth: 15,
                        dataLabels: {
                            enabled: true,
                            format: '<p style="color:white;">{point.percentage:.1f} %</p>'
                        },
                        showInLegend: true
                    }
                },
                colors: [
                    '#EC9301',
                    '#000000',
                    '#07A336',
                ],
                series: [{
                    type: 'pie',
                    name: '',
                    data: dataValue
                }]
            });
        },

        setobjMonthPicTotal_column: function () {
            var year = new Date().getFullYear();

            $('#MonthPicTotal_column').highcharts({
                chart: {
                    type: 'column',
                    /**borderColor: '#21814C',
                    borderWidth: 2**/
                },
                title: {
                    text: '<p style="font-size:20px; color:white; font-weight: 600;">PM2.5平均浓度</p>'
                },
                colors: [
                    '#A8CF04',
                    '#6ECF04'
                ],
                xAxis: {
                    categories: ['2014年', '2015年']
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080',
                    }]
                },
                plotOptions: {
                    column: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: 'white',
                            formatter: function () {
                                return this.y;
                            }
                        },
                        showInLegend: false
                    }
                },
                tooltip: {
                    //formatter: function () {
                    //    return this.series.name + '：' + this.point.y;
                    //},
                    headerFormat: '<span>{point.key}:</span>',
                    pointFormat: '' + '{point.y}%',
                    footerFormat: '<table><tbody><tr><td style="padding:0">{series.name}: </td><td style="padding:0"><b>{point.y}%</b></td></tr></tbody></table>',
                    shared: true,
                    useHTML: true
                },
                credits: {
                    enabled: false
                },

                series: [{
                    name: '2013年',
                    data: [77, 77]
                }, {
                    name: '年',
                    data: [73.7, 71.7]
                }]
            });
        },


        showJiance: function () {
            $("#jiance").css("background-color", "#B17718");
            $("#kongqi").css("background-color", "#434343");
            $("#jianceIcon").show();
            $("#heatIcon").hide();
            $(".timeRelver").hide();
            _this.clear();
            _this.showMarker();

            _this.aaa();
        },

        aaa: function () {

            if (_this.car_markerList.length == 0) {
                _this.showCar();
            } else {
                _this.updateCarMarker();
            }

            setTimeout(function () { _this.aaa(); }, 4000);
        },

        showKongqi: function () {
            $("#kongqi").css("background-color", "#B17718");
            $("#jiance").css("background-color", "#434343");
            $("#heatIcon").show();
            $("#jianceIcon").hide();
            location.reload();
        },

    })

})(jQuery);

(function () {
    var _this = $.demo;
    $.extend($.demo, {
        itemTest: function () {
            //var s = [];
            //s.push([this.map.getRandomLngLat(), this.map.getRandomLngLat(), this.map.getRandomLngLat()])
            //s.push([this.map.getRandomLngLat(), this.map.getRandomLngLat(), this.map.getRandomLngLat(), this.map.getRandomLngLat(), this.map.getRandomLngLat()])
            //L.multiPolygon(s, {
            //    stroke: true,
            //    color: "#03f",
            //    weight: 5,
            //    opacity: 1,
            //    fillOpacity: 1
            //}).addTo(this.map.map);
            this.map.setZoomAround(118.77422332763672, 31.992644847013356, 10);
        },
        // 复位中心点
        resetview: function () {
            this.map.resetView();
        },
        // 设置中心点
        setview: function () {
            var _lng = 118.79 + Math.random() * 0.1;
            var _lat = 32.03 + Math.random() * 0.1;
            var _zoom = this.getRandomNum(8, 19);
            this.map.setView(_lng, _lat, _zoom);
        },
        // 设置地图等级
        setzoom: function () {
            var _zoom = this.getRandomNum(1, 19);
            this.map.setZoom(_zoom);
        },
        // 地图放大
        zoomin: function () {
            this.map.zoomIn();
        },
        // 地图缩小
        zoomout: function () {
            this.map.zoomOut();
        },
        // 移动中心点
        panto: function () {
            var _lng = 118.79 + Math.random() * 0.1;
            var _lat = 32.03 + Math.random() * 0.1;
            this.map.panTo(_lng, _lat);
        },
        // 上下左右移动
        panby: function (x, y) {
            this.map.panBy(x, y);
        },
        // 获取中心点
        getcenter: function () {
            this.showStatus("当前中心点:" + this.map.getCenter());
            var s = this.map.getCenter();
            alert(this.map.getCenter());
        },
        // 获取地图等级
        getzoom: function () {
            this.showStatus("当前等级:" + this.map.getZoom());
            alert(this.map.getZoom());
        },
        // 获取最小等级
        getminzoom: function () {
            this.showStatus("地图最小等级:" + this.map.getMinZoom());
            alert(this.map.getMinZoom());
        },
        // 获取最大等级
        getmaxzoom: function () {
            this.showStatus("地图最大等级:" + this.map.getMaxZoom());
            alert(this.map.getMaxZoom());
        },
        // 获取地图可视范围坐标
        getbounds: function () {
            var _bounds = this.map.getBounds();

            this.showStatus("地图当前坐标范围:" + _bounds.toBBoxString());

            alert(_bounds.toBBoxString());
        },
        // 获取地图大小
        getsize: function () {
            var _size = this.map.getSize();

            this.showStatus("地图当前大小:" + _size);

            alert(_size);
        },
        // 获取可视范围像素值
        getPixelBounds: function () {
            var _bounds = this.map.getPixelBounds();

            this.showStatus("地图当前像素值:\n" + _bounds.min + "\n" + _bounds.max);

            alert("地图当前大小:\n" + _bounds.min + "\n" + _bounds.max);
        },
        // 获取地图左上角像素值
        getPixelOrigin: function () {
            var _bounds = this.map.getPixelOrigin();

            this.showStatus("地图当前像素值:\n" + _bounds);

            alert("地图当前大小:\n" + _bounds);
        }
    })
})(jQuery);


(function () {
    var _this = $.demo;
    $.extend($.demo, {
        // 地图清理
        clear: function () {
            this.map.clear();
        },
        // 地图添加Label
        addLabel: function () {
            this.map.addLabel(118.591618537903, 32.1321581345115, "李根华A", {
                //bgColor: "red",
                //bgHoverColor:"blue"
            });

            this.map.addLabel(118.74263763427734, 32.037256950424805, "李根华B", {
                anchor: [20, 10],
                bgColor: "red",
                bgHoverColor: "blue"
            });

            this.map.addLabel(118.7779998779297, 32.04766089060864, "李根华C", {
                anchor: [20, 10],
                bgColor: "rgba(243,123,54,.5)",
                bgHoverColor: "rgba(143,223,54,.9)"
            });

            this.map.addLabel(118.79868507385254, 32.027652263919215, "李根华D", {
                anchor: [20, 10],
                bgColor: "rgba(0,11,112,.4)",
                bgHoverColor: "rgba(66,33,99,.9)"
            });

            this.map.addLabel(118.76667022705077, 32.01586331746566, "李根华E", {
                anchor: [20, 10],
                bgColor: "rgba(99,66,33,.7)",
                bgHoverColor: "rgba(231,222,0,.7)"
            });

            this.map.addLabel(118.74478340148926, 32.04184065020709, "李根华F", {
                anchor: [20, 10],
                bgColor: "rgba(0,222,0,.7)",
                bgHoverColor: "green"
            });
        },
        // 地图添加Marker点
        addMarker: function () {
            // 添加Marker
            this.map.addCssMarker(118.72718811035155, 32.016663849488474, {
                type: "A1",
                param_A1: {
                    height: 30,
                    width: 30,
                    bgColor: "rgba(41,125,252,0.9)",
                    bgHoverColor: "rgba(255,106,0,0.9)",
                    //iconUrl: "",
                    iconHtml: "我"
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"
            }).l_animate(true, "jump");


            // 添加Marker,并跳跃动画
            this.map.addCssMarker(118.76272201538085, 32.02685182790788, {
                type: "B1",
                param_B1: {
                    height: 30,
                    width: 30,
                    bgColor: "rgba(41,125,252,0.9)",
                    bgHoverColor: "rgba(255,106,0,0.9)"
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"
            }).l_animate(true, "jump");

            // 添加Marker,并跳跃动画
            this.map.addCssMarker(118.74692916870116, 31.99395513667603, {
                type: "C1",
                param_C1: {
                    text: "<p style='width:80px;'>李根华</p>",
                    color: "#ff0000"
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"
            }).l_animate(true, "jump");


            // 添加Marker,并摇摆动画
            this.map.addImgMarker(118.79387855529785, 32.02248569017118, {
                icon: {
                    iconUrl: CONTENT_URL('Images/MapIcon/icon1.gif'),
                    iconSize: [50, 50],
                    iconAnchor: [25, 0]
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"

            }).l_animate(true, "swing");


            // 添加Marker,并放大缩小动画
            this.map.addImgMarker(118.72735977172853, 32.06046411667274, {
                icon: {
                    iconUrl: CONTENT_URL('Images/MapIcon/icon2.gif'),
                    iconSize: [80, 80],
                    iconAnchor: [40, 0]
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"

            }).l_animate(true, "scale");


            // 添加Marker,并旋转动画
            this.map.addImgMarker(118.83928298950195, 32.05871832770345, {
                icon: {
                    iconUrl: CONTENT_URL('Images/MapIcon/icon3.gif'),
                    iconSize: [100, 100],
                    iconAnchor: [50, 0]
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"

            }).l_animate(true, "rotateX");

            // 添加Marker,并旋转动画
            this.map.addImgMarker(118.77817153930664, 32.04489632256306, {
                icon: {
                    iconUrl: CONTENT_URL('Images/MapIcon/icon3.gif'),
                    iconSize: [100, 100],
                    iconAnchor: [50, 0]
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"

            }).l_animate(true, "rotateY");

            // 添加Marker,并旋转动画
            this.map.addImgMarker(118.80529403686523, 32.0661377007248, {
                icon: {
                    iconUrl: CONTENT_URL('Images/MapIcon/icon3.gif'),
                    iconSize: [100, 100],
                    iconAnchor: [50, 0]
                },
                popupContent: "<div style='padding:100px;color:#000;'>118.76272201538085, 32.02685182790788</div>"

            }).l_animate(true, "rotateZ");
        },
        // 添加线对象
        addLine: function () {
            this.map.addLine("118.69457244873047,32.03776626173338;118.72306823730469,32.02219460692298;118.72306823730469,32.02219460692298;118.72753143310547,31.983181088126432",
                {
                    label: {
                        text: 'you can try to click me!'
                    },
                    popupContent: "<div style='padding:50px;color:#000;'>I am a line!</div>"
                })
            //this.map.addLine(["118.75310897827148,32.05464469054932", "118.81507873535156,32.04475081666865", "118.80924224853516,32.01084163889594", "118.80924224853516,32.01084163889594"])
            //this.map.addLine([
            //    { lng: 118.75310897827148, lat: 32.05464469054932 },
            //    { lng: 118.81507873535156, lat: 32.04475081666865 },
            //    { lng: 118.80924224853516, lat: 32.01084163889594 },
            //    { lng: 118.80924224853516, lat: 32.01084163889594 }
            //])
        },
        // 添加多边形对象
        addPolygon: function () {
            //this.map.addPolygon("118.75310897827148,32.05464469054932;118.81507873535156,32.04475081666865;118.80924224853516,32.01084163889594;118.80924224853516,32.01084163889594")
            this.map.addPolygon(["118.78280639648437,32.02670629333614", "118.81319046020508,32.03776626173338", "118.82881164550781,32.00268992268768", "118.82881164550781,32.00268992268768", "118.75585556030273,32.00021515112686"], {
                label: {
                    text: 'you can try to click me!'
                },
                popupContent: "<div style='padding:50px;color:#000;'>I am a polygon!</div>"
            })
            //this.map.addPolygon([
            //    { lng: 118.75310897827148, lat: 32.05464469054932 },
            //    { lng: 118.81507873535156, lat: 32.04475081666865 },
            //    { lng: 118.80924224853516, lat: 32.01084163889594 },
            //    { lng: 118.80924224853516, lat: 32.01084163889594 }
            //])
        },
        // 添加矩形对象
        addRectangle: function () {
            this.map.addRectangle("118.77490997314453,32.08519254738235;118.82795333862303,32.05609958178297", {
                label: {
                    text: 'you can try to click me!'
                },
                popupContent: "<div style='padding:50px;color:#000;'>I am a Rectangle!</div>"
            });
        },
        // 添加圆对象
        //addCircle: function () {
        //    this.map.addCircle(118.71997833251951, 32.07312009026311, 3000, {
        //        popupContent: "<div style='padding:50px;color:#000;'>I am a circle!</div>",
        //        label: {
        //            text: 'you can try to click me!'
        //        }
        //    });
        //}
    })
})(jQuery);

(function () {
    var _this = $.demo;
    $.extend($.demo, {
        // 绘制线
        drawLine: function () {
            var line = this.map.drawLine({
                complete: function (result) {
                    alert(result);
                    line.remove();
                }
            });
        },
        // 绘制多边形
        drawPoly: function () {
            var poly = this.map.drawPoly({
                complete: function (result) {
                    alert(result);
                    poly.remove();
                }
            });
        },
        // 绘制圆
        drawCircle: function () {
            var circle = this.map.drawCircle({
                complete: function (result) {
                    alert("圆点：" + result.lng + "," + result.lat + "\n"
                        + "半径：" + result.radius);
                    circle.remove();
                }
            });
        },
        // 绘制矩形
        drawRectangle: function () {
            var rect = this.map.drawRectangle({
                complete: function (result) {
                    alert("最小经度：" + result.min_lng + "\n"
                        + "最小纬度：" + result.min_lat + "\n"
                        + "最大经度：" + result.max_lng + "\n"
                        + "最大纬度：" + result.max_lat);
                    rect.remove();
                }
            });
        }
    })
})(jQuery);

(function () {
    var _this = $.demo;
    $.extend($.demo, {
        tool_distance: function () {
            this.map.toolDistance();
        },
        tool_area: function () {
            alert("尚未完成.");
        }
    })
})(jQuery);

(function () {
    var _this = $.demo;
    $.extend($.demo, {
        showClusterData: function () {
            var param = {
                number: parseInt($("#cluster_number").val()),
                radius: parseInt($("#cluster_radius").val())
            }

            this.map.clear();

            var _markers = [];

            for (var i = 0; i < param.number; i++) {
                var p = this.map.getRandomLngLat();

                var _m = this.map.addCssMarker(p.lng, p.lat, {
                    type: "B1",
                    addToMapFlag: false,
                    popupContent: "<h1>I am " + i + "!  </h1>"
                });

                _markers.push(_m);

            }

            this.map.addClusterMarkers(_markers, {
                maxClusterRadius: param.radius,
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
            });

        },
        showEllipse: function () {

            var param = {
                width: parseInt($("#txt_ty_width").val()),
                height: parseInt($("#txt_ty_height").val()),
                angel: parseInt($("#txt_ty_angel").val())
            }

            this.map.clear();

            var s = this.map.getCenter();


            this.map.addEllipse({
                lng: s.lng,
                lat: s.lat,
                radii_w: param.width,
                radii_h: param.height,
                radius: param.angel
            })

        },
        showPlotter: function () {
            this.map.clear();

            var _latlngs = [];
            for (var i = 0; i < 10; i++) {
                var p = this.map.getRandomLngLat();

                _latlngs.push([p.lat, p.lng]);

            }


            var plottedPolyline = L.Polyline.Plotter(_latlngs, {
                weight: 5
            }).addTo(this.map.map);


        },
        showHeatmap1: function () {
            this.map.clear();
            var heatmapLayer = L.TileLayer.heatMap({
                radius: 20,
                opacity: 0.8,
                gradient: {
                    0.45: "rgb(0,0,255)",
                    0.55: "rgb(0,255,255)",
                    0.65: "rgb(0,255,0)",
                    0.95: "yellow",
                    1.0: "rgb(255,0,0)"
                }
            });

            var _latlngs = [];
            for (var i = 0; i < 10; i++) {
                var p = this.map.getRandomLngLat();

                _latlngs.push({
                    lat: p.lat,
                    lon: p.lng,
                    value: this.getRandomNum(1, 20)
                });

            }


            heatmapLayer.addData(_latlngs);

            heatmapLayer.addTo(this.map.map);

            //this.map.addToLayerGroup(heatmapLayer);
        },

        showHeatmap: function (e) {
            var param = {
                size: 0.03,
                count: 13
            }
            this.map.clear();


            var _heatLayer = this.map.addHeatMap({
                options: {
                    "radius": param.size,
                    "maxOpacity": .8,
                    "scaleRadius": true,
                    "useLocalExtrema": true
                }
            });


            var _testData = { max: 8, data: [] };

            for (var i = 0; i < param.count; i++) {
                //var p = this.map.getRandomLngLat();
                var p = $.demo.data.OHI;

                if (e == 1) {
                    _testData.data.push({
                        lng: p[i].Lng + 0.00001 * e,
                        lat: p[i].Lat + 0.00002 * e,
                        value: _this.getRandomNum(2, 3)
                    });
                } else {
                    _testData.data.push({
                        lng: p[i].Lng,
                        lat: p[i].Lat,
                        value: _this.getRandomNum(2, 3)
                    });
                }

            }

            _heatLayer.setData(_testData);

        },

        showWebGLHeatmap: function () {

            this.map.clear();

            var heatmap = new L.TileLayer.WebGLHeatMap({ size: 1000, autoresize: true });

            for (var i = 0; i < 500; i++) {
                var p = this.map.getRandomLngLat();

                heatmap.addDataPoint(p.lat, p.lng, this.getRandomNum(1, 100));

            }

            this.map.addToLayerGroup(heatmap);

        },
        showImageLayer: function () {
            this.map.clear();
            var imageUrl = $("#txt_img_url").val();
            var lng1 = parseFloat($("#txt_lt_lng").val());
            var lat1 = parseFloat($("#txt_lt_lat").val());
            var lng2 = parseFloat($("#txt_rb_lng").val());
            var lat2 = parseFloat($("#txt_rb_lat").val());

            this.map.addImageOverLayer(imageUrl, {
                l_t: [lng1, lat1],
                r_b: [lng2, lat2]
            });
        },
        editImageLayer: function () {
            this.map.clear();

            var _width = parseInt($("#txt_img_width").val());

            var _height = parseInt($("#txt_img_height").val());

            var _mark = this.map.addImgMarker(118.67053985595703, 32.08781046022499, {
                option: {
                    draggable: true,
                    opacity: 0.8
                },
                icon: {
                    iconUrl: ACTION_URL("ImageLayer", "Test") + "?width=" + _width + "&height=" + _height,
                    iconSize: [_width, _height],
                    iconAnchor: [0, 0]
                },
                event: {
                    add: function () {
                        _this.map.on("zoomend", _getNewBounds);
                    },
                    remove: function () {
                        _this.map.off("zoomend", _getNewBounds);
                    },
                    dragend: function () {
                        _getNewBounds();
                    }
                }
            })
            _getNewBounds();
            function _getNewBounds() {
                var _latLng1 = _mark.getLatLng();
                var _point1 = _this.map.latLngToContainerPoint(_latLng1.lng, _latLng1.lat);
                var _point2 = { x: _point1.x + _width, y: _point1.y + _height };
                var _latLng2 = _this.map.containerPointToLatLng(_point2.x, _point2.y);
                $("#txt_lt_lng").val(_latLng1.lng);
                $("#txt_lt_lat").val(_latLng1.lat);
                $("#txt_rb_lng").val(_latLng2.lng);
                $("#txt_rb_lat").val(_latLng2.lat);
            }

        },
        tool_print: function () {
            _this.map.print();
        }
    })
})(jQuery);


(function () {
    var _this = $.demo;
    $.extend($.demo, {

        //GPS转经纬度
        GpsTransform: function (wgLon, wgLat) {
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
                lon: mgLon,
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

        getRandomNum: function (Min, Max) {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        },

        day: function () {
            $("#day").css("background-color", "#242424");
            $("#month").css("background-color", "");
            $("#year").css("background-color", "");
            _this.setobjMonthPicTotal("day", 0, 1);
        },

        month: function () {
            $("#month").css("background-color", "#242424");
            $("#day").css("background-color", "");
            $("#year").css("background-color", "");
            _this.setobjMonthPicTotal("month", 0, 1);
        },

        year: function () {
            $("#year").css("background-color", "#242424");
            $("#month").css("background-color", "");
            $("#day").css("background-color", "");
            _this.setobjMonthPicTotal("year", 0, 1);
        },

        car_markerList: [],
        car_markerListObj: {},

        showCar: function () {
            var car = _this.data.car;

            $.each(car, function (index, c) {
                var _marker = $.demo.map.addCssMarker(c.Lng, c.Lat, {
                    type: "A1",
                    param_A1: {
                        height: 48,
                        width: 48,
                        bgColor: "rgba(190,148,0,0.9)",
                        //bgHoverColor: "rgba(255,106,0,0.9)",
                        iconUrl: CONTENT_URL("Content/images/a_11_k.png"),
                        //iconHtml: "我"
                    },
                    event: {
                        "click": function () {
                            //_this.showRightMain(c);
                            _this.setobjMonthPicTotal("day", 1, 0, 4);
                        },
                    },
                });
                _this.car_markerList = _marker;

                _this.car_markerListObj["car_" + c.id] = _marker;
            });
        },

        i: -1,

        updateCarMarker: function () {
            _this.i = _this.i + 1;

            var list = [
                {
                    id: 1001, op: [{ Lng: "118.80143165588379", Lat: "32.02601500096252" }, { Lng: "118.79969830450136", Lat: "32.01884708309526" }, { Lng: "118.79658222198486", Lat: "32.01000466571624" },
                                    { Lng: "118.79602432250976", Lat: "32.00509184296872" }, { Lng: "118.81087303161622", Lat: "32.00702061218214" }, { Lng: "118.81357669830322", Lat: "32.02022977080685" },
                                    { Lng: "118.81975650787353", Lat: "32.05311702985569" }, { Lng: "118.816237449646", Lat: "32.06526486454641" }]
                },
                {
                    id: 1002, op: [{ Lng: "118.76465320587157", Lat: "32.01175138368326" }, { Lng: "118.7542676925659", Lat: "32.01549943696158" }, { Lng: "118.74982595443724", Lat: "32.01748256818705" },
                                    { Lng: "118.73853921890259", Lat: "32.021757980316536" }, { Lng: "118.72763872146605", Lat: "32.023959284922455" }, { Lng: "118.72100830078125", Lat: "32.02546924014024" },
                                    { Lng: "118.71611595153807", Lat: "32.027797796987954" }, { Lng: "118.7197422981262", Lat: "32.035510718715535" }, ]
                }
            ];
            $.each(list, function (index, ele) {
                var _marker = _this.car_markerListObj["car_" + ele.id];
                if (_marker) {
                    _marker.setLatLng([ele.op[_this.i].Lat, ele.op[_this.i].Lng]);
                }
            });

        },

        showMarker: function () {
            var OHI_list = _this.data.OHI;

            //var car_Exhaust = _this.data.car_Exhaust;

            var air_list = _this.data.air;
            var fac = _this.data.ndustrial_pollution;
            var now = new Date();
            var nowStr = now.toLocaleString();
            //var car = _this.data.car;

            //$.each(car, function (index, c) {
            //    var _marker = $.demo.map.addCssMarker(c.Lng, c.Lat, {
            //        type: "A1",
            //        param_A1: {
            //            height: 48,
            //            width: 48,
            //            bgColor: "rgba(163,158,15,0.9)",
            //            //bgHoverColor: "rgba(255,106,0,0.9)",
            //            iconUrl: CONTENT_URL("Content/images/a_11.png"),
            //            //iconHtml: "我"
            //        },
            //        event: {
            //            "click": function () {
            //                _this.showRightMain(c);
            //            },
            //        },
            //    });
            //});

            $.each(OHI_list, function (index, n) {

                $.demo.map.addCssMarker(n.Lng, n.Lat, {
                    type: "A1",
                    param_A1: {
                        height: 48,
                        width: 48,
                        bgColor: "rgba(2,131,206,0.9)",
                        //bgHoverColor: "rgba(255,106,0,0.9)",
                        iconUrl: CONTENT_URL("Content/images/a_07_k.png"),
                    },
                    popule: {},
                    event: {
                        "click": function () {
                            //_this.showRightMain(n);
                            _this.setobjMonthPicTotal("day", 1, 0, 1);
                        },
                    },
                    popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>编号：" + n.num + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + n.name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + n.address + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + nowStr + "</div>",

                });
            });

            $.each(fac, function (index, f) {
                $.demo.map.addCssMarker(f.Lng, f.Lat, {
                    type: "A1",
                    param_A1: {
                        height: 48,
                        width: 48,
                        bgColor: "rgba(203,39,0,0.9)",
                        //bgHoverColor: "rgba(255,106,0,0.9)",
                        iconUrl: CONTENT_URL("Content/images/a_13.png"),
                    },
                    event: {
                        "click": function () {
                            //_this.showRightMain(f);
                            //_this.setobjMonthPicTotal("day", 1, 0, 2);
                        },
                    },
                    popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>编号：" + f.num + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + f.name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + f.address + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + nowStr + "</div>",
                });
            });

            $.each(air_list, function (index, e) {
                $.demo.map.addCssMarker(e.Lng, e.Lat, {
                    type: "A1",
                    param_A1: {
                        height: 48,
                        width: 48,
                        bgColor: "rgba(90,171,24,0.9)",
                        //bgHoverColor: "rgba(255,106,0,0.9)",
                        iconUrl: CONTENT_URL("Content/images/a_03_k.png"),
                        //iconHtml: "我"
                    },
                    event: {
                        "click": function () {
                            //_this.showRightMain(e);
                            _this.setobjMonthPicTotal("day", 1, 0, 2);
                        },
                    },
                    popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>编号：" + e.num + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + e.name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + e.address + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + nowStr + "</div>",
                });
            });

        },

        showRightMain: function (info) {

            var now = new Date();
            var nowStr = now.toLocaleString();

            $("#indexInfo").hide();
            $("#detailedInfo").show();

            $("#num").text("编号：" + info.num);
            $("#name").text("名称：" + info.name);
            $("#address").text("地址：" + info.address);
            $("#time").text("最新监测时间：" + nowStr);

        },

        HeatmapSet: function () {
            $(".timeRelver").css("background-color", "#B17718");

            var s = 0;

            var time = setInterval(function () {
                s = s + 1;
                _this.showHeatmap(s);
                if (s == 10) {
                    $(".timeRelver").css("background-color", "#8A8A8A");
                    clearInterval(time);
                }
            }, 2000);

        },

        KQ: function () {
            $("#type_1").css("background-color", "");
            $("#type_2").css("background-color", "#383838");
            $("#type_3").css("background-color", "#383838");
            $("#kq_1").show();
            $("#qc_1").hide();
        },

        GY: function () {
            $("#type_1").css("background-color", "#383838");
            $("#type_2").css("background-color", "");
            $("#type_3").css("background-color", "#383838");
        },

        QC: function () {
            $("#type_1").css("background-color", "#383838");
            $("#type_2").css("background-color", "#383838");
            $("#type_3").css("background-color", "");
            $("#kq_1").hide();
            $("#qc_1").show();
        },

        OHI_paihang: function () {
            $("#aqi_paihang").show();
            $("#gy_paihang").hide();
            $("#OHI").css("background-color", "#B17718");
            $("#gongyewuran").css("background-color", "#434343");
        },

        GY_paihang: function () {
            $("#aqi_paihang").hide();
            $("#gy_paihang").show();
            $("#OHI").css("background-color", "#434343");
            $("#gongyewuran").css("background-color", "#B17718");
        },

        aaaaabbbb: function () {
            $.ajaxHandle({
                url: ACTION_URL("aaa", "Manager"),
                data: {
                    id: 1
                },
                loading: false,
                success: function (response) {
                    debugger;
                    if (response) {
                        var s = response;
                    }

                }
            });
        },

    })
})(jQuery);