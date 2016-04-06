$.demo = {};
(function () {
    var _this = $.demo;
    $.extend($.demo, {
        poi_type_hour: null,

        poi_type_day: null,

        poi_type_year: null,

        time_heatmap: 0,

        pageInit: function () {
            $("body").css({
                height: "1080px",
                width: "3840px"
            })

            $("#full").css({
                height: "1080px",
                width: "3840px"
                //height: document.documentElement.clientHeight + "px",
                //width: document.documentElement.clientWidth + "px"
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

            L.tileLayer.YLC = function (options) {
                return new L.TileLayer.YLC(options);
            };

            L.TileLayer.YLC = L.TileLayer.extend({
                options: {
                    mapFrom: "autonavi"
                },
                initialize: function (options) { // (String, Object)
                    L.setOptions(this, options);
                },
                getTileUrl: function (tilePoint) {
                    return L.Util.template(ACTION_URL("LoadMapGetPng", "Home") + '?type={type}&x={x}&y={y}&z={z}', L.extend({
                        type: this.options.mapFrom,
                        z: tilePoint.z,
                        x: tilePoint.x,
                        y: tilePoint.y
                    }, this.options));
                }
            })

            this.map = $.tMap.init("map", $.extend({
                defaultLayer: function () {
                    return [L.tileLayer.YLC({ mapFrom: "autonavi" }), L.tileLayer.YLC({ mapFrom: "autonavi" })];
                }
            }))


            //this.map = $.tMap.init("map", {
            //    defaultLayer: "autonavi",
            //})

            setTimeout(function () {
                //_this.showHeatmap(0);
                //_this.showJiance();
                _this.showClusterData();
                //_this.showArea();
            }, 2000)


            this.map.map.on("click", function (evt) {
                $("#map_status").html("点击坐标:" + evt.latlng.lng + "," + evt.latlng.lat);
            })


        },
        showStatus: function (message) {
            $("#map_status").html(message);
        },

        poi_name: null,
        setobjMonthPicTotal: function (date, type, time, n, p) {
            if (p == 0) {
                switch (type) {
                    case 0:
                        _this.poi_type_hour = _this.data.Hour_2000;
                        _this.poi_type_day = _this.data.DAY_2000;
                        break;
                    case 2001:
                        _this.poi_type_hour = _this.data.Hour_2001;
                        _this.poi_type_day = _this.data.DAY_2001;
                        break;
                    case 2002:
                        _this.poi_type_hour = _this.data.Hour_2002;
                        _this.poi_type_day = _this.data.DAY_2002;
                        break;
                    case 2003:
                        _this.poi_type_hour = _this.data.Hour_2003;
                        _this.poi_type_day = _this.data.DAY_2003;
                        break;
                    case 2004:
                        _this.poi_type_hour = _this.data.Hour_2004;
                        _this.poi_type_day = _this.data.DAY_2004;
                        break;
                    case 2006:
                        _this.poi_type_hour = _this.data.Hour_2006;
                        _this.poi_type_day = _this.data.DAY_2006;
                        break;
                    case 2007:
                        _this.poi_type_hour = _this.data.Hour_2007;
                        _this.poi_type_day = _this.data.DAY_2007;
                        break;
                    case 2008:
                        _this.poi_type_hour = _this.data.Hour_2008;
                        _this.poi_type_day = _this.data.DAY_2008;
                        break;
                    case 2009:
                        _this.poi_type_hour = _this.data.Hour_2009;
                        _this.poi_type_day = _this.data.DAY_2009;
                        break;
                    case 2010:
                        _this.poi_type_hour = _this.data.Hour_2010;
                        _this.poi_type_day = _this.data.DAY_2010;
                        break;
                    case 2013:
                        _this.poi_type_hour = _this.data.Hour_2013;
                        _this.poi_type_day = _this.data.DAY_2013;
                        break;
                    case 2016:
                        _this.poi_type_hour = _this.data.Hour_2016;
                        _this.poi_type_day = _this.data.DAY_2016;
                        break;
                    case 2022:
                        _this.poi_type_hour = _this.data.Hour_2022;
                        _this.poi_type_day = _this.data.DAY_2022;
                        break;
                    case 2024:
                        _this.poi_type_hour = _this.data.Hour_2024;
                        _this.poi_type_day = _this.data.DAY_2024;
                        break;
                    default:
                }
            }

            var data = [];
            var x = [];
            var today = new Date();
            var oneHour = 1000 * 60 * 60;
            var oneday = 1000 * 60 * 60 * 24;
            var oneMonth = 1000 * 60 * 60 * 24 * 30;

            var id;
            var name;
            var charts_type = "";
            //var charts_type = "spline";
            //var charts_type = "column";

            var pingjun;

            if (time == 0) {
                $("#day").css("background-color", "#117ADC");
                $("#month").css("background-color", "");
                $("#year").css("background-color", "");
            }
            if (n != undefined) {

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
                        $("input[name='flag']")[7].checked = true;
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }
            }

            var checkid = 0;
            for (var i = 0; i < $("input[name='flag']").length; i++) {
                if ($("input[name='flag']")[i].checked == true) {
                    id = $("input[name='flag']")[i].id;
                    if (checkid == id) {
                        break;
                    }
                    checkid = id;
                    switch (id) {
                        case "CheckBox1":
                            name = "OHI";
                            if (date == "month") {
                                charts_type = "column";
                                data_aqi = [];
                                var q = 0;
                                //for (var i = 0; i < 10; i++) {
                                //var s = new Date(today - oneday * 3 * i).getDate();
                                var color = "";
                                var y = 0;
                                for (var j = 0; j < _this.poi_type_day.length; j++) {
                                    var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                    //if (time == s) {
                                    y = _this.poi_type_day[j].OHI;
                                    q = q + y;
                                    if (y <= 50) {
                                        color = "#02E300";
                                    } else if (y > 50 && y <= 100) {
                                        color = "#FFFF00";
                                    } else if (y > 100 && y <= 150) {
                                        color = "#FF7E00";
                                    } else if (y > 150 && y <= 200) {
                                        color = "#FE0000";
                                    } else if (y > 200 && y <= 300) {
                                        color = "#98004B";
                                    } else if (y > 300) {
                                        color = "#7E0123";
                                    }

                                    //}
                                    data_aqi.push({ 'color': color, y: y })
                                    x.push(s + "日");
                                }
                                //data_aqi.push({ 'color': color, y: y })
                                x.push(time + "日");
                                //}
                                var u = new Date(parseInt(_this.poi_type_hour[0].HOUR.replace("/Date(", "").replace(")/", "")));
                                var u_last = new Date(parseInt(_this.poi_type_hour[_this.poi_type_hour.length - 1].HOUR.replace("/Date(", "").replace(")/", "")));
                                pingjun = u_last.getFullYear() + "/" + u_last.getMonth() + "/" + u_last.getDate() + "  " + u_last.getHours() + "时至" + u.getFullYear() + "/" + u.getMonth() + "/" + u.getDate() + "  " + u.getHours() + "时";
                                //pingjun = (q / 10).toFixed(2);

                                x.reverse();
                                data = data_aqi.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_aqi = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].OHI;
                                            q = q + y;
                                            if (y <= 50) {
                                                color = "#02E300";
                                            } else if (y > 50 && y <= 100) {
                                                color = "#FFFF00";
                                            } else if (y > 100 && y <= 150) {
                                                color = "#FF7E00";
                                            } else if (y > 150 && y <= 200) {
                                                color = "#FE0000";
                                            } else if (y > 200 && y <= 300) {
                                                color = "#98004B";
                                            } else if (y > 300) {
                                                color = "#7E0123";
                                            }
                                        }
                                    }
                                    data_aqi.push({ 'color': color, y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_aqi.reverse();

                            } else if (date == "day") {
                                charts_type = "line";
                                data_aqi = [];
                                var q = 0;
                                //for (var i = 0; i < 12; i++) {
                                var s = new Date(today - oneHour * 2 * i).getHours();
                                var color = "";
                                var y = 0;
                                for (var j = 0; j < _this.poi_type_hour.length ; j++) {
                                    //if ((j * 2) > _this.poi_type_hour.length - 1) { break; }
                                    var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                    //if (time == s) {
                                    y = _this.poi_type_hour[j].OHI;
                                    q = q + y;
                                    if (y <= 50) {
                                        color = "#02E300";
                                    } else if (y > 50 && y <= 100) {
                                        color = "#FFFF00";
                                    } else if (y > 100 && y <= 150) {
                                        color = "#FF7E00";
                                    } else if (y > 150 && y <= 200) {
                                        color = "#FE0000";
                                    } else if (y > 200 && y <= 300) {
                                        color = "#98004B";
                                    } else if (y > 300) {
                                        color = "#7E0123";
                                    }
                                    //}
                                    data_aqi.push({ 'color': color, y: y })
                                    x.push(time + ":00");
                                }
                                //data_aqi.push({ 'color': color, y: y })
                                //x.push(p);
                                //}
                                var u = new Date(parseInt(_this.poi_type_hour[0].HOUR.replace("/Date(", "").replace(")/", "")));
                                var u_last = new Date(parseInt(_this.poi_type_hour[_this.poi_type_hour.length - 1].HOUR.replace("/Date(", "").replace(")/", "")));
                                pingjun = u_last.getFullYear() + "/" + (u_last.getMonth() + 1) + "/" + u_last.getDate() + "  " + u_last.getHours() + "时至" + u.getFullYear() + "/" + (u.getMonth() + 1) + "/" + u.getDate() + "  " + u.getHours() + "时";

                                //pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_aqi.reverse();
                            }
                            break;
                        case "CheckBox2":
                            name = "PM2.5";
                            if (date == "month") {
                                charts_type = "column";
                                data_pm25 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_day.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.poi_type_day[j].PM25;
                                            q = q + y;
                                        }
                                    }
                                    data_pm25.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 10).toFixed(2);
                                x.reverse();
                                data = data_pm25.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_pm25 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].PM25;
                                            q = q + y;
                                        }
                                    }
                                    data_pm25.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_pm25.reverse();

                            } else if (date == "day") {
                                charts_type = "line";
                                data_pm25 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_hour.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.poi_type_hour[j].PM25;
                                            q = q + y;
                                        }
                                    }
                                    data_pm25.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_pm25.reverse();
                            }
                            break;
                        case "CheckBox3":
                            name = "PM10";
                            if (date == "month") {
                                charts_type = "column";
                                data_pm10 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_day.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.poi_type_day[j].PM10;
                                            q = q + y;
                                        }
                                    }
                                    data_pm10.push({ 'color': "#0198DC", y: y })

                                    x.push(s + "日");
                                }
                                pingjun = (q / 10).toFixed(2);
                                x.reverse();
                                data = data_pm10.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_pm10 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].PM10;
                                            q = q + y;
                                        }
                                    }
                                    data_pm10.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_pm10.reverse();

                            } else if (date == "day") {
                                charts_type = "line";
                                data_pm10 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_hour.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.poi_type_hour[j].PM10;
                                            q = q + y;
                                        }
                                    }
                                    data_pm10.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_pm10.reverse();
                            }
                            break;
                        case "CheckBox4":
                            name = "SO2";
                            if (date == "month") {
                                charts_type = "column";
                                data_so2 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_day.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.poi_type_day[j].SO2;
                                            q = q + y;
                                        }
                                    }
                                    data_so2.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 10).toFixed(2);
                                x.reverse();
                                data = data_so2.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_so2 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].SO2;
                                            q = q + y;
                                        }
                                    }
                                    data_so2.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_so2.reverse();

                            } else if (date == "day") {
                                charts_type = "line";
                                data_so2 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_hour.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.poi_type_hour[j].SO2;
                                            q = q + y;
                                        }
                                    }
                                    data_so2.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_so2.reverse();
                            }
                            break;
                        case "CheckBox5":
                            name = "NO2";
                            if (date == "month") {
                                charts_type = "column";
                                data_no2 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_day.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.poi_type_day[j].NO2;
                                            q = q + y;
                                        }
                                    }
                                    data_no2.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_no2.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_no2 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].NO2;
                                            q = q + y;
                                        }
                                    }
                                    data_no2.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_no2.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_no2 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_hour.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.poi_type_hour[j].NO2;
                                            q = q + y;
                                        }
                                    }
                                    data_no2.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_no2.reverse();
                            }
                            break;
                        case "CheckBox6":
                            name = "CO";
                            if (date == "month") {
                                charts_type = "column";
                                data_co = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_day.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.poi_type_day[j].CO;
                                            q = q + y;
                                        }
                                    }
                                    data_co.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_co = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].CO;
                                            q = q + y;
                                        }
                                    }
                                    data_co.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_co = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_hour.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.poi_type_hour[j].CO;
                                            q = q + y;
                                        }
                                    }
                                    data_co.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co.reverse();
                            }
                            break;
                        case "CheckBox7":
                            name = "CO";
                            if (date == "month") {
                                charts_type = "column";
                                data_co = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.qc_List_day[0][j].wq_co;
                                            q = q + y;
                                        }
                                    }
                                    data_co.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_co = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_mouth[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth();
                                        if (s == time) {
                                            y = _this.data.qc_List_mouth[0][j].wq_co;
                                            q = q + y;
                                        }
                                    }
                                    data_co.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_co = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.qc_List_hour[0][j].wq_co;
                                            q = q + y;
                                        }
                                    }
                                    data_co.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co.reverse();
                            }
                            break;
                        case "CheckBox8":
                            name = "NO";
                            if (date == "month") {
                                charts_type = "column";
                                data_no = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.qc_List_day[0][j].wq_no;
                                            q = q + y;
                                        }
                                    }
                                    data_no.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_no.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_no = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_mouth[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth();
                                        if (s == time) {
                                            y = _this.data.qc_List_mouth[0][j].wq_no;
                                            q = q + y;
                                        }
                                    }
                                    data_no.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_no.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_no = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.qc_List_hour[0][j].wq_no;
                                            q = q + y;
                                        }
                                    }
                                    data_no.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_no.reverse();
                            }
                            break;
                        case "CheckBox9":
                            name = "CO2";
                            if (date == "month") {
                                charts_type = "column";
                                data_co2 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.qc_List_day[0][j].wq_co2;
                                            q = q + y;
                                        }
                                    }
                                    data_co2.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co2.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_co2 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_mouth[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth();
                                        if (s == time) {
                                            y = _this.data.qc_List_mouth[0][j].wq_co2;
                                            q = q + y;
                                        }
                                    }
                                    data_co2.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co2.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_co2 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.qc_List_hour[0][j].wq_co2;
                                            q = q + y;
                                        }
                                    }
                                    data_co2.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_co2.reverse();
                            }
                            break;
                        case "CheckBox10":
                            name = "CH";
                            if (date == "month") {
                                charts_type = "column";
                                data_ch = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.qc_List_day[0][j].wq_ch;
                                            q = q + y;
                                        }
                                    }
                                    data_ch.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_ch.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_ch = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_mouth[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth();
                                        if (s == time) {
                                            y = _this.data.qc_List_mouth[0][j].wq_ch;
                                            q = q + y;
                                        }
                                    }
                                    data_ch.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_ch.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_ch = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.qc_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.qc_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.qc_List_hour[0][j].wq_ch;
                                            q = q + y;
                                        }
                                    }
                                    data_ch.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_ch.reverse();
                            }
                            break;
                        case "CheckBox11":
                            name = "O3";
                            if (date == "month") {
                                charts_type = "column";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_day.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_day[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.poi_type_day[j].O3_8;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                        if (s == time) {
                                            y = _this.data.Year_2000[0][j].O3;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.poi_type_hour.length; j++) {
                                        var time = new Date(parseInt(_this.poi_type_hour[j].HOUR.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.poi_type_hour[j].O3_8;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            }
                            break;
                        case "CheckBox12":
                            name = "SO2";
                            if (date == "month") {
                                charts_type = "column";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.gywr_List_day[0][j].gy_so2;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                        var time = (new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth()) + 1;
                                        if (s == time) {
                                            y = _this.data.gywr_List_mouth[0][j].gy_so2;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.gywr_List_hour[0][j].gy_so2;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            }
                            break;
                        case "CheckBox13":
                            name = "CO";
                            if (date == "month") {
                                charts_type = "column";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.gywr_List_day[0][j].gy_co;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                        var time = (new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth()) + 1;
                                        if (s == time) {
                                            y = _this.data.gywr_List_mouth[0][j].gy_co;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.gywr_List_hour[0][j].gy_co;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            }
                            break;
                        case "CheckBox14":
                            name = "NO";
                            if (date == "month") {
                                charts_type = "column";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.gywr_List_day[0][j].gy_no;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                        var time = (new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth()) + 1;
                                        if (s == time) {
                                            y = _this.data.gywr_List_mouth[0][j].gy_no;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.gywr_List_hour[0][j].gy_no;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            }
                            break;
                        case "CheckBox15":
                            name = "烟尘";
                            if (date == "month") {
                                charts_type = "column";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.gywr_List_day[0][j].gy_dust;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                        var time = (new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth()) + 1;
                                        if (s == time) {
                                            y = _this.data.gywr_List_mouth[0][j].gy_dust;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.gywr_List_hour[0][j].gy_dust;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            }
                            break;
                        case "CheckBox16":
                            name = "废气";
                            if (date == "month") {
                                charts_type = "column";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 10; i++) {
                                    var s = new Date(today - oneday * 3 * i).getDate();
                                    //if (s % 3 == 0) {
                                    //    s = s + 3;
                                    //}
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                        if (time == s) {
                                            y = _this.data.gywr_List_day[0][j].gy_gas;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': "#0198DC", y: y })
                                    x.push(s + "日");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "year") {
                                charts_type = "column";
                                var data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneMonth * i).getMonth() + 1;
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                        var time = (new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth()) + 1;
                                        if (s == time) {
                                            y = _this.data.gywr_List_mouth[0][j].gy_gas;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + "月");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            } else if (date == "day") {
                                charts_type = "line";
                                data_O3_8 = [];
                                var q = 0;
                                for (var i = 0; i < 12; i++) {
                                    var s = new Date(today - oneHour * 2 * i).getHours();
                                    var color = "";
                                    var y = 0;
                                    for (var j = 0; j < _this.data.gywr_List_hour[0].length; j++) {
                                        var time = new Date(parseInt(_this.data.gywr_List_hour[0][j].time.replace("/Date(", "").replace(")/", ""))).getHours();
                                        if (time == s) {
                                            y = _this.data.gywr_List_hour[0][j].gy_gas;
                                            q = q + y;
                                        }
                                    }
                                    data_O3_8.push({ 'color': '#0198DC', y: y })
                                    x.push(s + ":00");
                                }
                                pingjun = (q / 12).toFixed(2);
                                x.reverse();
                                data = data_O3_8.reverse();
                            }
                            break;
                    }
                }
            }


            if (_this.poi_name != null) {
                $("#heatmaphead").html(_this.poi_name + name + "柱状图");
            } else {
                $("#heatmaphead").html(name + "柱状图");
            }

            $('#objMonthPicTotal').highcharts({
                chart: {
                    plotBorderWidth: 2,
                    plotBorderColor: '#fff',
                    backgroundColor: '#4EA6F8',
                    type: charts_type,
                    //borderColor: '#21814C',
                    //borderWidth: 2
                },
                title: {
                    text: ''
                },
                //colors: [
                //    '#EB9301',
                //    '#0198DC',
                //],
                xAxis: {
                    categories: x,
                    lineColor: '#fff',
                    lineWidth: 2,
                    labels: {
                        style: {
                            color: '#fff',
                            fontSize: '20px'
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    //plotLines: [{
                    //    value: 0,
                    //    width: 1,
                    //    color: '#808080'
                    //}],
                    //gridLineColor: '#fff',
                    lineColor: '#fff',
                    lineWidth: 2,
                    labels: {
                        style: {
                            color: '#fff',
                            y: 15,
                            fontSize: '15px'
                        }
                    }
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
                    name: '<div style="font-size:20px; color:white;">' + name + '：' + pingjun + '</div>',
                    //colorByPoint: false,
                    type: charts_type,
                    color: '#1E1E1E',
                    //dashStyle: 'dash',
                    data: data
                }],

            });

        },

        setobjMonthPicTotal_area: function () {
            var chart;
            var dataValue = [['<div style="font-size:15px; color:white;">黄标车</div>', 6538], ['<div style="font-size:15px; color:white;">无标车</div>', 3213], ['<div style="font-size:15px; color:white;">绿标车</div>', 90362]];
            $('#MonthPicTotal_area').highcharts({
                chart: {
                    //plotBorderWidth: 2,
                    //plotBorderColor: '#fff',
                    backgroundColor: 'RGBA(132, 188, 245,.7)',
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
                    enabled: true
                },
                //tooltip: {
                //    pointFormat: function () {
                //        return this.series.name + '：' + this.point.y;
                //    },
                //},
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        innerSize: 40,
                        depth: 15,
                        dataLabels: {
                            enabled: true,
                            format: '<p style="color:#FFF;">{point.percentage:.1f} %</p>'
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
                    name: '数量',
                    data: dataValue
                }]
            });
        },

        fenxi_type: "OHI",
        setobjMonthPicTotal_fengxi22222: function (v, type, time, enble) {
            var data_gongye = [];
            var data_weiqi = [];
            var data_fengsu = [];
            var other_data = [];

            var data_gongye_full = [];
            var data_weiqi_full = [];
            var data_fengsu_full = [];
            var other_data_full = [];
            var name;
            var x = [];
            var x_full = [];
            var id = null;

            if (enble == 0) {
                $("#month_1").css("background-color", "#242424");
                $("#year_1").css("background-color", "");
            }

            function getCountDays() {
                var curDate = new Date();
                /* 获取当前月份 */
                var curMonth = curDate.getMonth();
                /*  生成实际的月份: 由于curMonth会比实际月份小1, 故需加1 */
                curDate.setMonth(curMonth);
                /* 将日期设置为0, 这里为什么要这样设置, 我不知道原因, 这是从网上学来的 */
                curDate.setDate(0);
                /* 返回当月的天数 */
                return curDate.getDate();
            }

            switch (type) {
                case "OHI":
                    name = "OHI";
                    _this.fenxi_type = name;
                    if (time == "month") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 10; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].OHI;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co + _this.data.gywr_List_day[0][j].gy_so2 + _this.data.gywr_List_day[0][j].gy_no + _this.data.gywr_List_day[0][j].gy_dust + _this.data.gywr_List_day[0][j].gy_cod);
                                }
                            }
                            data_aqi.push(y);
                            data_wq.push(c);
                            data_fs.push(f);
                            data_gy.push(g);
                            x.push(s + "日");
                        }

                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_gongye = data_gy.reverse();
                        data_fengsu = data_fs.reverse();


                        data_aqi_full = [];
                        data_wq_full = [];
                        data_fs_full = [];
                        data_gy_full = [];
                        for (var i = 0; i < 31 ; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].OHI;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co + _this.data.gywr_List_day[0][j].gy_so2 + _this.data.gywr_List_day[0][j].gy_no + _this.data.gywr_List_day[0][j].gy_dust + _this.data.gywr_List_day[0][j].gy_cod);
                                }
                            }
                            data_aqi_full.push(y);
                            data_wq_full.push(c);
                            data_fs_full.push(f);
                            data_gy_full.push(g);
                            x_full.push(s + "日");
                        }

                        x_full.reverse();
                        other_data_full = data_aqi_full.reverse();
                        data_weiqi_full = data_wq_full.reverse();
                        data_gongye_full = data_gy_full.reverse();
                        data_fengsu_full = data_fs_full.reverse();

                    } else if (time == "year") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 12; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                if (s == time) {
                                    y = _this.data.Year_2000[0][j].OHI;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    c = _this.data.weather_data_year[j].js;
                                    f = _this.data.weather_data_year[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_co + _this.data.gywr_List_mouth[0][j].gy_so2 + _this.data.gywr_List_mouth[0][j].gy_no + _this.data.gywr_List_mouth[0][j].gy_dust + _this.data.gywr_List_mouth[0][j].gy_cod);
                                }
                            }

                            data_aqi.push(y);
                            data_wq.push(c);
                            data_gy.push(g);
                            data_fs.push(f);
                            x.push(s + "月");
                            x_full.push(s + "月");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_fengsu = data_fs.reverse();
                        data_gongye = data_gy.reverse();
                        x_full.reverse();
                        other_data_full = data_aqi.reverse();
                        data_weiqi_full = data_wq.reverse();
                        data_fengsu_full = data_fs.reverse();
                        data_gongye_full = data_gy.reverse();

                    }
                    break;

                case "PM25":
                    name = "PM25";
                    _this.fenxi_type = name;
                    if (time == "month") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 10; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].PM25;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                }
                            }
                            data_aqi.push(y);
                            data_wq.push(c);
                            data_fs.push(f);
                            data_gy.push(g);
                            x.push(s + "日");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_gongye = data_gy.reverse();
                        data_fengsu = data_fs.reverse();

                        data_aqi_full = [];
                        data_wq_full = [];
                        data_fs_full = [];
                        data_gy_full = [];
                        for (var i = 0; i < 31 ; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].PM25;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                }
                            }
                            data_aqi_full.push(y);
                            data_wq_full.push(c);
                            data_gy_full.push(g);
                            data_fs_full.push(f);
                            x_full.push(s + "日");
                        }
                        x_full.reverse();
                        other_data_full = data_aqi_full.reverse();
                        data_weiqi_full = data_wq_full.reverse();
                        data_gongye_full = data_gy_full.reverse();
                        data_fengsu_full = data_fs_full.reverse();


                    } else if (time == "year") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 12; i++) {
                            var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                if (s == time) {
                                    y = _this.data.Year_2000[0][j].PM25;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    c = _this.data.weather_data_year[j].js;
                                    f = _this.data.weather_data_year[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_dust);
                                }
                            }

                            data_aqi.push(y);
                            data_wq.push(c);
                            data_gy.push(g);
                            data_fs.push(f);
                            x.push(s + "月");
                            x_full.push(s + "月");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_fengsu = data_fs.reverse();
                        data_gongye = data_gy.reverse();
                        x_full.reverse();
                        other_data_full = data_aqi.reverse();
                        data_weiqi_full = data_wq.reverse();
                        data_fengsu_full = data_fs.reverse();
                        data_gongye_full = data_gy.reverse();

                    }
                    break;

                case "PM10":
                    name = "PM10";
                    _this.fenxi_type = name;
                    if (time == "month") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 10; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            //if (s % 3 == 0) {
                            //    s = s + 3;
                            //}
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].PM10;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                }
                            }
                            data_aqi.push(y);
                            data_wq.push(c);
                            data_fs.push(f);
                            data_gy.push(g);
                            x.push(s + "日");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_gongye = data_gy.reverse();
                        data_fengsu = data_fs.reverse();


                        data_aqi_full = [];
                        data_fs_full = [];
                        data_wq_full = [];
                        data_gy_full = [];
                        for (var i = 0; i < 31; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].PM10;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                }
                            }
                            data_aqi_full.push(y);
                            data_wq_full.push(c);
                            data_fs_full.push(f);
                            data_gy_full.push(g);
                            x_full.push(s + "日");
                        }
                        x_full.reverse();
                        other_data_full = data_aqi_full.reverse();
                        data_weiqi_full = data_wq_full.reverse();
                        data_gongye_full = data_gy_full.reverse();
                        data_fengsu_full = data_fs_full.reverse();


                    } else if (time == "year") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 12; i++) {
                            var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                if (s == time) {
                                    y = _this.data.Year_2000[0][j].PM10;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    c = _this.data.weather_data_year[j].js;
                                    f = _this.data.weather_data_year[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_dust);
                                }
                            }

                            data_aqi.push(y);
                            data_wq.push(c);
                            data_gy.push(g);
                            data_fs.push(f);
                            x.push(s + "月");
                            x_full.push(s + "月");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_fengsu = data_fs.reverse();
                        data_gongye = data_gy.reverse();
                        x_full.reverse();
                        other_data_full = data_aqi.reverse();
                        data_weiqi_full = data_wq.reverse();
                        data_fengsu_full = data_fs.reverse();
                        data_gongye_full = data_gy.reverse();
                    }
                    break;


                case "SO2":
                    name = "SO2";
                    _this.fenxi_type = name;
                    if (time == "month") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 10; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].SO2;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_so2);
                                }
                            }
                            data_aqi.push(y);
                            data_wq.push(c);
                            data_fs.push(f);
                            data_gy.push(g);
                            x.push(s + "日");
                        }

                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_gongye = data_gy.reverse();
                        data_fengsu = data_fs.reverse();


                        data_aqi_full = [];
                        data_wq_full = [];
                        data_fs_full = [];
                        data_gy_full = [];
                        for (var i = 0; i < 31 ; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].SO2;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_so2);
                                }
                            }
                            data_aqi_full.push(y);
                            data_wq_full.push(c);
                            data_fs_full.push(f);
                            data_gy_full.push(g);
                            x_full.push(s + "日");
                        }

                        x_full.reverse();
                        other_data_full = data_aqi_full.reverse();
                        data_weiqi_full = data_wq_full.reverse();
                        data_gongye_full = data_gy_full.reverse();
                        data_fengsu_full = data_fs_full.reverse();

                    } else if (time == "year") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 12; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                if (s == time) {
                                    y = _this.data.Year_2000[0][j].SO2;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    c = _this.data.weather_data_year[j].js;
                                    f = _this.data.weather_data_year[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_so2);
                                }
                            }

                            data_aqi.push(y);
                            data_wq.push(c);
                            data_gy.push(g);
                            data_fs.push(f);
                            x.push(s + "月");
                            x_full.push(s + "月");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_fengsu = data_fs.reverse();
                        data_gongye = data_gy.reverse();
                        x_full.reverse();
                        other_data_full = data_aqi.reverse();
                        data_weiqi_full = data_wq.reverse();
                        data_fengsu_full = data_fs.reverse();
                        data_gongye_full = data_gy.reverse();

                    }
                    break;

                case "NO2":
                    name = "NO2";
                    _this.fenxi_type = name;
                    if (time == "month") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 10; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].NO2;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                                }
                            }
                            data_aqi.push(y);
                            data_wq.push(c);
                            data_fs.push(f);
                            data_gy.push(g);
                            x.push(s + "日");
                        }

                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_gongye = data_gy.reverse();
                        data_fengsu = data_fs.reverse();


                        data_aqi_full = [];
                        data_wq_full = [];
                        data_fs_full = [];
                        data_gy_full = [];
                        for (var i = 0; i < 31 ; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].NO2;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                                }
                            }
                            data_aqi_full.push(y);
                            data_wq_full.push(c);
                            data_fs_full.push(f);
                            data_gy_full.push(g);
                            x_full.push(s + "日");
                        }

                        x_full.reverse();
                        other_data_full = data_aqi_full.reverse();
                        data_weiqi_full = data_wq_full.reverse();
                        data_gongye_full = data_gy_full.reverse();
                        data_fengsu_full = data_fs_full.reverse();

                    } else if (time == "year") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 12; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                if (s == time) {
                                    y = _this.data.Year_2000[0][j].NO2;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    c = _this.data.weather_data_year[j].js;
                                    f = _this.data.weather_data_year[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_no);
                                }
                            }

                            data_aqi.push(y);
                            data_wq.push(c);
                            data_gy.push(g);
                            data_fs.push(f);
                            x.push(s + "月");
                            x_full.push(s + "月");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_fengsu = data_fs.reverse();
                        data_gongye = data_gy.reverse();
                        x_full.reverse();
                        other_data_full = data_aqi.reverse();
                        data_weiqi_full = data_wq.reverse();
                        data_fengsu_full = data_fs.reverse();
                        data_gongye_full = data_gy.reverse();

                    }
                    break;

                case "CO":
                    name = "CO";
                    _this.fenxi_type = name;
                    if (time == "month") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 10; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].CO;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                                }
                            }
                            data_aqi.push(y);
                            data_wq.push(c);
                            data_fs.push(f);
                            data_gy.push(g);
                            x.push(s + "日");
                        }

                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_gongye = data_gy.reverse();
                        data_fengsu = data_fs.reverse();


                        data_aqi_full = [];
                        data_wq_full = [];
                        data_fs_full = [];
                        data_gy_full = [];
                        for (var i = 0; i < 31 ; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                                var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    y = _this.data.DAY_2000[j].CO;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    c = _this.data.weather_data[j].js;
                                    f = _this.data.weather_data[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                                }
                            }
                            data_aqi_full.push(y);
                            data_wq_full.push(c);
                            data_fs_full.push(f);
                            data_gy_full.push(g);
                            x_full.push(s + "日");
                        }

                        x_full.reverse();
                        other_data_full = data_aqi_full.reverse();
                        data_weiqi_full = data_wq_full.reverse();
                        data_gongye_full = data_gy_full.reverse();
                        data_fengsu_full = data_fs_full.reverse();

                    } else if (time == "year") {
                        data_aqi = [];
                        data_wq = [];
                        data_fs = [];
                        data_gy = [];
                        for (var i = 0; i < 12; i++) {
                            //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                            var y = 0;
                            var c = 0;
                            var f = 0;
                            var g = 0;
                            for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                                var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                                if (s == time) {
                                    y = _this.data.Year_2000[0][j].CO;
                                }
                            }
                            for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                                var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    c = _this.data.weather_data_year[j].js;
                                    f = _this.data.weather_data_year[j].fs;
                                }
                            }
                            for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                                var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                                if (time == s) {
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_co);
                                }
                            }

                            data_aqi.push(y);
                            data_wq.push(c);
                            data_gy.push(g);
                            data_fs.push(f);
                            x.push(s + "月");
                            x_full.push(s + "月");
                        }
                        x.reverse();
                        other_data = data_aqi.reverse();
                        data_weiqi = data_wq.reverse();
                        data_fengsu = data_fs.reverse();
                        data_gongye = data_gy.reverse();
                        x_full.reverse();
                        other_data_full = data_aqi.reverse();
                        data_weiqi_full = data_wq.reverse();
                        data_fengsu_full = data_fs.reverse();
                        data_gongye_full = data_gy.reverse();

                    }
                    break;
                default:

            }
            $("#chart_head").html(type + "分析图");
            $("#chart_head_full").html(type + "分析图");
            //for (var i = 0; i < 12; i++) {
            //    var s = new Date(new Date() - 1000 * 60 * 60 * 2 * i).getHours();
            //    x.push(s + ":00");
            //}

            //for (var i = 0; i < 10; i++) {
            //    var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
            //    if (s % 3 == 0) {
            //        s = s + 3;
            //    }
            //    x.push(s + "日");
            //}

            //x.reverse();
            $('#objMonthPicTotal_2').highcharts({
                title: {
                    text: '',
                },
                xAxis: {
                    categories: x,
                    labels: {
                        style: {
                            fontSize: '20px'
                        }
                    }
                },
                yAxis: [{
                    labels: {
                        y: 10,
                        style: {
                            fontSize: '20px'
                        }
                    },
                    title: {
                        text: '',
                    },
                    opposite: false

                }, { //工业
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { //降水
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { // 风速
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                }],
                plotOptions: {
                    area: {
                        fillColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#C78111'],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 2
                            }
                        },
                        threshold: null
                    }
                },
                series: [{
                    name: '<div style="font-size:20px;">工业</div>',
                    color: '#FE0000',
                    dashStyle: 'dash',
                    yAxis: 1,
                    data: data_gongye
                }, {
                    name: '<div style="font-size:20px;">降水量</div>',
                    color: '#84DA46',
                    yAxis: 2,
                    dashStyle: 'shortdot',
                    tooltip: {
                        valueSuffix: ' mm'
                    },
                    data: data_weiqi
                }, {
                    name: '<div style="font-size:20px;">风速</div>',
                    yAxis: 3,
                    color: '#32A9E3',
                    tooltip: {
                        valueSuffix: ' 级'
                    },
                    data: data_fs
                }, {
                    type: 'area',
                    name: '<div style="font-size:20px;">' + name + '</div>',
                    color: '#C78111',
                    data: other_data
                }]

            });


            $('#objMonthPicTotal_2_full').highcharts({
                title: {
                    text: '',
                },
                xAxis: {
                    categories: x_full,
                    labels: {
                        style: {
                            fontSize: '30px'
                        }
                    }
                },
                yAxis: [{
                    labels: {
                        y: 10,
                        style: {
                            fontSize: '25px'
                        }
                    },
                    title: {
                        text: '',
                    },
                    opposite: false

                }, { //工业
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { //降水
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { // 风速
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                }],
                plotOptions: {
                    area: {
                        fillColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#C78111'],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 2
                            }
                        },
                        threshold: null
                    }
                },
                series: [{
                    name: '<div style="font-size:25px;">工业</div>',
                    color: '#FE0000',
                    dashStyle: 'dash',
                    yAxis: 1,
                    data: data_gongye_full
                }, {
                    name: '<div style="font-size:25px;">降水量</div>',
                    color: '#84DA46',
                    yAxis: 2,
                    dashStyle: 'shortdot',
                    tooltip: {
                        valueSuffix: ' mm'
                    },
                    data: data_weiqi_full
                }, {
                    name: '<div style="font-size:25px;">风速</div>',
                    yAxis: 3,
                    color: '#32A9E3',
                    tooltip: {
                        valueSuffix: ' 级'
                    },
                    data: data_fengsu_full
                }, {
                    type: 'area',
                    name: '<div style="font-size:25px;">' + name + '</div>',
                    color: '#C78111',
                    data: other_data_full
                }]

            });

        },

        setobjMonthPicTotal_fengxi: function (v, type, time, enble) {
            var data_gongye = [];
            var data_weiqi = [];
            var data_fengsu = [];
            var other_data = [];

            var data_gongye_full = [];
            var data_weiqi_full = [];
            var data_fengsu_full = [];
            var other_data_full = [];
            var name;
            var x = [];
            var x_full = [];
            var id = null;

            if (enble == 0) {
                $("#month_1").css("background-color", "#242424");
                $("#year_1").css("background-color", "");
            }


            name = type;
            _this.fenxi_type = name;
            if (time == "month") {
                data_aqi = [];
                data_wq = [];
                data_fs = [];
                data_gy = [];
                for (var i = 0; i < 10; i++) {
                    //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                    var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                    var y = 0;
                    var c = 0;
                    var f = 0;
                    var g = 0;
                    for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                        var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                        if (time == s) {
                            switch (type) {
                                case "OHI":
                                    y = _this.data.DAY_2000[j].OHI;
                                    break;
                                case "PM2.5":
                                    y = _this.data.DAY_2000[j].PM25;
                                    break;
                                case "PM10":
                                    y = _this.data.DAY_2000[j].PM10;
                                    break;
                                case "SO2":
                                    y = _this.data.DAY_2000[j].SO2;
                                    break;
                                case "NO2":
                                    y = _this.data.DAY_2000[j].NO2;
                                    break;
                                case "CO":
                                    y = _this.data.DAY_2000[j].CO;
                                    break;
                                case "O3":
                                    y = _this.data.DAY_2000[j].O3_8;
                                    break;
                                default:
                            }

                        }
                    }
                    for (var j = 0; j < _this.data.weather_data.length; j++) {
                        var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                        if (time == s) {
                            c = _this.data.weather_data[j].js;
                            f = _this.data.weather_data[j].fs;
                        }

                    }
                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                        if (time == s) {
                            switch (type) {
                                case "OHI":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co + _this.data.gywr_List_day[0][j].gy_so2 + _this.data.gywr_List_day[0][j].gy_no + _this.data.gywr_List_day[0][j].gy_dust + _this.data.gywr_List_day[0][j].gy_cod);
                                    break;
                                case "PM2.5":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                    break;
                                case "PM10":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                    break;
                                case "SO2":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_so2);
                                    break;
                                case "NO2":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                                    break;
                                case "CO":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                                    break;
                                default:
                            }
                        }

                    }
                    data_aqi.push(y);
                    data_gy.push(g);
                    data_wq.push(c);
                    data_fs.push(f);
                    x.push(s + "日");
                }
                x.reverse();
                other_data = data_aqi.reverse();
                data_weiqi = data_wq.reverse();
                data_gongye = data_gy.reverse();
                data_fengsu = data_fs.reverse();


                data_aqi_full = [];
                data_wq_full = [];
                data_fs_full = [];
                data_gy_full = [];
                //for (var i = 0; i < 30; i++) {
                //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                //var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                var y = 0;
                var c = 0;
                var f = 0;
                var g = 0;
                for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                    var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                    //if (time == s) {
                    switch (type) {
                        case "OHI":
                            y = _this.data.DAY_2000[j].OHI;
                            break;
                        case "PM2.5":
                            y = _this.data.DAY_2000[j].PM25;
                            break;
                        case "PM10":
                            y = _this.data.DAY_2000[j].PM10;
                            break;
                        case "SO2":
                            y = _this.data.DAY_2000[j].SO2;
                            break;
                        case "NO2":
                            y = _this.data.DAY_2000[j].NO2;
                            break;
                        case "CO":
                            y = _this.data.DAY_2000[j].CO;
                            break;
                        case "O3":
                            y = _this.data.DAY_2000[j].O3_8;
                            break;
                        default:
                    }
                    //}
                    data_aqi_full.push(y);
                }
                for (var j = 0; j < _this.data.weather_data.length; j++) {
                    //var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                    //if (time == s) {
                    c = _this.data.weather_data[j].js;
                    f = _this.data.weather_data[j].fs;
                    //}
                    data_wq_full.push(c);
                    data_fs_full.push(f);
                }
                for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                    //var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                    //if (time == s) {
                    switch (type) {
                        case "OHI":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_co + _this.data.gywr_List_day[0][j].gy_so2 + _this.data.gywr_List_day[0][j].gy_no + _this.data.gywr_List_day[0][j].gy_dust + _this.data.gywr_List_day[0][j].gy_cod);
                            break;
                        case "PM2.5":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                            break;
                        case "PM10":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                            break;
                        case "SO2":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_so2);
                            break;
                        case "NO2":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                            break;
                        case "CO":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                            break;
                        default:
                    }
                    //}
                    data_gy_full.push(g);
                }
                //data_aqi_full.push(y);
                //data_wq_full.push(c);
                //data_fs_full.push(f);
                //data_gy_full.push(g);
                //x_full.push(s + "日");
                //}

                x_full.reverse();
                other_data_full = data_aqi_full.reverse();
                data_weiqi_full = data_wq_full.reverse();
                data_gongye_full = data_gy_full.reverse();
                data_fengsu_full = data_fs_full.reverse();

            } else if (time == "year") {
                data_aqi = [];
                data_wq = [];
                data_fs = [];
                data_gy = [];
                for (var i = 0; i < 12; i++) {
                    //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                    var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                    var y = 0;
                    var c = 0;
                    var f = 0;
                    var g = 0;
                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                        if (s == time) {
                            switch (type) {
                                case "OHI":
                                    y = _this.data.Year_2000[0][j].OHI;
                                    break;
                                case "PM2.5":
                                    y = _this.data.Year_2000[0][j].PM25;
                                    break;
                                case "PM10":
                                    y = _this.data.Year_2000[0][j].PM10;
                                    break;
                                case "SO2":
                                    y = _this.data.Year_2000[0][j].SO2;
                                    break;
                                case "NO2":
                                    y = _this.data.Year_2000[0][j].NO2;
                                    break;
                                case "CO":
                                    y = _this.data.Year_2000[0][j].CO;
                                    break;
                                case "O3":
                                    y = _this.data.DAY_2000[j].O3_8;
                                    break;
                                default:
                            }
                        }
                    }
                    for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                        var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                        if (time == s) {
                            c = _this.data.weather_data_year[j].js;
                            f = _this.data.weather_data_year[j].fs;
                        }
                    }
                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                        var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                        if (time == s) {
                            switch (type) {
                                case "OHI":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_co + _this.data.gywr_List_mouth[0][j].gy_so2 + _this.data.gywr_List_mouth[0][j].gy_no + _this.data.gywr_List_mouth[0][j].gy_dust + _this.data.gywr_List_mouth[0][j].gy_cod);
                                    break;
                                case "PM2.5":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_dust);
                                    break;
                                case "PM10":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_dust);
                                    break;
                                case "SO2":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_so2);
                                    break;
                                case "NO2":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                                    break;
                                case "CO":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                                    break;
                                default:
                            }
                        }
                    }

                    data_aqi.push(y);
                    data_wq.push(c);
                    data_gy.push(g);
                    data_fs.push(f);
                    x.push(s + "月");
                    x_full.push(s + "月");
                }
                x.reverse();
                other_data = data_aqi.reverse();
                data_weiqi = data_wq.reverse();
                data_fengsu = data_fs.reverse();
                data_gongye = data_gy.reverse();
                x_full.reverse();
                other_data_full = data_aqi.reverse();
                data_weiqi_full = data_wq.reverse();
                data_fengsu_full = data_fs.reverse();
                data_gongye_full = data_gy.reverse();

            }


            $("#chart_head").html(type + "分析图");
            $("#chart_head_full").html(type + "分析图");
            $('#objMonthPicTotal_2').highcharts({
                chart: {
                    plotBorderWidth: 2,
                    plotBorderColor: '#fff',
                    backgroundColor: '#4EA6F8',
                },
                title: {
                    text: '',
                },
                xAxis: {
                    categories: x,
                    //lineColor: '#fff',
                    //lineWidth: 2,
                    //type: 'datetime',
                    //maxZoom: 48 * 3600 * 1000,
                    labels: {
                        style: {
                            fontSize: '20px',
                            color: '#fff'
                        }
                    }
                },
                yAxis: [{
                    //lineColor: '#fff',
                    //lineWidth: 2,
                    labels: {
                        y: 10,
                        style: {
                            fontSize: '20px',
                            color: '#fff'
                        }
                    },
                    title: {
                        text: '',
                    },
                    opposite: false

                }, { //工业
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    min: 25,
                    labels: {
                        enabled: false
                    }

                }, { //降水
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { // 风速
                    gridLineWidth: 0,
                    max: 20,
                    min: -20,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                }, { // 用电量
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                }],
                plotOptions: {
                    areaspline: {
                        fillColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#ffc524'],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        //marker: {
                        //    enabled: false
                        //},
                        lineWidth: 3,
                        states: {
                            hover: {
                                lineWidth: 5
                            }
                        },
                        threshold: null
                    },
                    //areaspline: {
                    //    fillOpacity: 0.5
                    //}
                    spline: {
                        lineWidth: 3,
                        states: {
                            hover: {
                                lineWidth: 5
                            }
                        },
                        //marker: {
                        //    enabled: false
                        //},

                    }
                },
                series: [
                    {
                        name: '<div style="font-size:20px; color:white;">工业</div>',
                        color: '#f15539',
                        dashStyle: 'dash',
                        type: 'spline',
                        //lineWidth: 4,
                        yAxis: 1,
                        data: data_gongye,
                        //pointInterval: 18 * 3600 * 1000
                    }, {
                        name: '<div style="font-size:20px; color:white;">降水量</div>',
                        color: '#2C8A13',
                        yAxis: 2,
                        type: 'spline',
                        //lineWidth: 4,
                        dashStyle: 'shortdot',
                        tooltip: {
                            valueSuffix: ' mm'
                        },
                        data: data_weiqi,
                        //pointInterval: 24 * 3600 * 1000
                    }, {
                        name: '<div style="font-size:20px; color:white;">风速</div>',
                        yAxis: 3,
                        color: '#c447a0',
                        type: 'spline',
                        //lineWidth: 4,
                        tooltip: {
                            valueSuffix: ' 级'
                        },
                        data: data_fs,
                        //pointInterval: 24 * 3600 * 1000
                    },
                    {
                        name: '<div style="font-size:20px; color:white;">用电量</div>',
                        yAxis: 4,
                        color: '#00deff',
                        type: 'column',
                        //lineWidth: 4,
                        tooltip: {
                            valueSuffix: '千瓦'
                        },
                        data: [2938, 3526, 5463, 2354, 1263, 3542, 6432, 2431, 3647, 3521],
                        //pointInterval: 24 * 3600 * 1000
                    },
                {
                    type: 'areaspline',
                    name: '<div style="font-size:20px; color:white;">' + name + '</div>',
                    color: '#ffc524',
                    data: other_data,
                    //pointInterval: 24 * 3600 * 1000
                }]

            });

            $('#objMonthPicTotal_2_full').highcharts({
                chart: {
                    plotBorderWidth: 2,
                    plotBorderColor: '#fff',
                    backgroundColor: '#4EA6F8',
                },
                title: {
                    text: '',
                },
                xAxis: {
                    categories: x_full,
                    //minRange: 100,
                    labels: {
                        style: {
                            fontSize: '0px'
                        }
                    }
                },
                yAxis: [{
                    labels: {
                        y: 10,
                        style: {
                            fontSize: '25px',
                            color: '#fff'
                        }
                    },
                    title: {
                        text: '',
                    },
                    opposite: false

                }, { //工业
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    min: 25,
                    labels: {
                        enabled: false
                    }

                }, { //降水
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { // 风速
                    gridLineWidth: 0,
                    max: 20,
                    min: -20,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                },
                //{ // 用电量
                //    gridLineWidth: 0,
                //    title: {
                //        text: '',
                //    },
                //    labels: {
                //        enabled: false

                //    },
                //    opposite: true
                //}
                ],
                plotOptions: {
                    areaspline: {
                        fillColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#C78111'],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 5,
                        states: {
                            hover: {
                                lineWidth: 7
                            }
                        },
                        threshold: null
                    },
                    spline: {
                        lineWidth: 5,
                        states: {
                            hover: {
                                lineWidth: 7
                            }
                        },
                        marker: {
                            enabled: false
                        },

                    }
                },
                series: [{
                    name: '<div style="font-size:25px; color:white;">工业</div>',
                    color: '#FE0000',
                    dashStyle: 'dash',
                    type: 'spline',
                    yAxis: 1,
                    pointInterval: 5,
                    data: data_gongye_full
                }, {
                    name: '<div style="font-size:25px; color:white;">降水量</div>',
                    color: '#2C8A13',
                    yAxis: 2,
                    dashStyle: 'spline',
                    type: 'spline',
                    pointInterval: 1,
                    tooltip: {
                        valueSuffix: ' mm'
                    },
                    data: data_weiqi_full
                }, {
                    name: '<div style="font-size:25px; color:white;">风速</div>',
                    yAxis: 3,
                    color: '#c447a0',
                    type: 'spline',
                    pointInterval: 1,
                    tooltip: {
                        valueSuffix: ' 级'
                    },
                    data: data_fengsu_full
                },
                //{
                //    name: '<div style="font-size:20px;">用电量</div>',
                //    yAxis: 4,
                //    color: '#742894',
                //    type: 'spline',
                //    pointInterval: 3.5,
                //    //lineWidth: 4,
                //    tooltip: {
                //        valueSuffix: ' 千瓦'
                //    },
                //    data: other_data_full,
                //},
                {
                    type: 'areaspline',
                    name: '<div style="font-size:25px; color:white;">' + name + '</div>',
                    color: '#C78111',
                    pointInterval: 5,
                    data: other_data_full
                }]

            });
            //alert("工业" + data_gongye_full.length + "降水量" + data_weiqi_full.length + "风速" + data_fengsu_full.length + name + other_data_full.length)
        },





        setobjMonthPicTotal_fengxi3333333: function (v, type, time, enble) {
            var data_gongye = [];
            var data_weiqi = [];
            var data_fengsu = [];
            var other_data = [];

            var data_gongye_full = [];
            var data_weiqi_full = [];
            var data_fengsu_full = [];
            var other_data_full = [];
            var name;
            var x = [];
            var x_full = [];
            var id = null;

            if (enble == 0) {
                $("#month_1").css("background-color", "#242424");
                $("#year_1").css("background-color", "");
            }


            name = type;
            _this.fenxi_type = name;
            if (time == "month") {
                data_aqi = [];
                data_wq = [];
                data_fs = [];
                data_gy = [];
                //for (var i = 0; i < 10; i++) {
                //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                //var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 3 * i).getDate();
                var y = 0;
                var c = 0;
                var f = 0;
                var g = 0;
                for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                    var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                    //if (time == s) {
                    switch (type) {
                        case "OHI":
                            y = _this.data.DAY_2000[j].OHI;
                            break;
                        case "PM2.5":
                            y = _this.data.DAY_2000[j].PM25;
                            break;
                        case "PM10":
                            y = _this.data.DAY_2000[j].PM10;
                            break;
                        case "SO2":
                            y = _this.data.DAY_2000[j].SO2;
                            break;
                        case "NO2":
                            y = _this.data.DAY_2000[j].NO2;
                            break;
                        case "CO":
                            y = _this.data.DAY_2000[j].CO;
                            break;
                        default:
                    }
                    data_aqi.push(y);
                    // }
                }
                for (var j = 0; j < _this.data.weather_data.length; j++) {
                    var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                    //if (time == s) {
                    c = _this.data.weather_data[j].js;
                    f = _this.data.weather_data[j].fs;
                    //}
                    data_wq.push(c);
                    data_fs.push(f);
                }
                for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                    var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                    //if (time == s) {
                    switch (type) {
                        case "OHI":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_co + _this.data.gywr_List_day[0][j].gy_so2 + _this.data.gywr_List_day[0][j].gy_no + _this.data.gywr_List_day[0][j].gy_dust + _this.data.gywr_List_day[0][j].gy_cod);
                            break;
                        case "PM2.5":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                            break;
                        case "PM10":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                            break;
                        case "SO2":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_so2);
                            break;
                        case "NO2":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                            break;
                        case "CO":
                            g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                            break;
                        default:
                    }
                    //}
                    data_gy.push(g);

                }
                //x.push("日");
                //}
                //x.reverse();
                other_data = data_aqi.reverse();
                data_weiqi = data_wq.reverse();
                data_gongye = data_gy.reverse();
                data_fengsu = data_fs.reverse();


                data_aqi_full = [];
                data_wq_full = [];
                data_fs_full = [];
                data_gy_full = [];
                for (var i = 0; i < 30; i++) {
                    //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * i).getDate();
                    var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * i).getDate();
                    var y = 0;
                    var c = 0;
                    var f = 0;
                    var g = 0;
                    for (var j = 0; j < _this.data.DAY_2000.length; j++) {
                        var time = new Date(parseInt(_this.data.DAY_2000[j].DAY.replace("/Date(", "").replace(")/", ""))).getDate();
                        if (time == s) {
                            switch (type) {
                                case "OHI":
                                    y = _this.data.DAY_2000[j].OHI;
                                    break;
                                case "PM2.5":
                                    y = _this.data.DAY_2000[j].PM25;
                                    break;
                                case "PM10":
                                    y = _this.data.DAY_2000[j].PM10;
                                    break;
                                case "SO2":
                                    y = _this.data.DAY_2000[j].SO2;
                                    break;
                                case "NO2":
                                    y = _this.data.DAY_2000[j].NO2;
                                    break;
                                case "CO":
                                    y = _this.data.DAY_2000[j].CO;
                                    break;
                                default:
                            }
                        }
                    }
                    for (var j = 0; j < _this.data.weather_data.length; j++) {
                        var time = new Date(parseInt(_this.data.weather_data[j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                        if (time == s) {
                            c = _this.data.weather_data[j].js;
                            f = _this.data.weather_data[j].fs;
                        }
                    }
                    for (var j = 0; j < _this.data.gywr_List_day[0].length; j++) {
                        var time = new Date(parseInt(_this.data.gywr_List_day[0][j].time.replace("/Date(", "").replace(")/", ""))).getDate();
                        if (time == s) {
                            switch (type) {
                                case "OHI":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co + _this.data.gywr_List_day[0][j].gy_so2 + _this.data.gywr_List_day[0][j].gy_no + _this.data.gywr_List_day[0][j].gy_dust + _this.data.gywr_List_day[0][j].gy_cod);
                                    break;
                                case "PM2.5":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                    break;
                                case "PM10":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_dust);
                                    break;
                                case "SO2":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_so2);
                                    break;
                                case "NO2":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                                    break;
                                case "CO":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                                    break;
                                default:
                            }
                        }
                    }
                    data_aqi_full.push(y);
                    data_wq_full.push(c);
                    data_fs_full.push(f);
                    data_gy_full.push(g);
                    x_full.push(s + "日");
                }

                x_full.reverse();
                other_data_full = data_aqi_full.reverse();
                data_weiqi_full = data_wq_full.reverse();
                data_gongye_full = data_gy_full.reverse();
                data_fengsu_full = data_fs_full.reverse();

            } else if (time == "year") {
                data_aqi = [];
                data_wq = [];
                data_fs = [];
                data_gy = [];
                for (var i = 0; i < 12; i++) {
                    //var s = new Date(new Date() - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                    var s = new Date(parseInt(_this.data.result_time.replace("/Date(", "").replace(")/", "")) - 1000 * 60 * 60 * 24 * 30 * i).getMonth() + 1;
                    var y = 0;
                    var c = 0;
                    var f = 0;
                    var g = 0;
                    for (var j = 0; j < _this.data.Year_2000[0].length; j++) {
                        var time = _this.data.Year_2000[0][j].TIME.substring(4).replace(/^0/, '');
                        if (s == time) {
                            switch (type) {
                                case "OHI":
                                    y = _this.data.Year_2000[0][j].OHI;
                                    break;
                                case "PM2.5":
                                    y = _this.data.Year_2000[0][j].PM25;
                                    break;
                                case "PM10":
                                    y = _this.data.Year_2000[0][j].PM10;
                                    break;
                                case "SO2":
                                    y = _this.data.Year_2000[0][j].SO2;
                                    break;
                                case "NO2":
                                    y = _this.data.Year_2000[0][j].NO2;
                                    break;
                                case "CO":
                                    y = _this.data.Year_2000[0][j].CO;
                                    break;
                                default:
                            }
                        }
                    }
                    for (var j = 0; j < _this.data.weather_data_year.length; j++) {
                        var time = new Date(parseInt(_this.data.weather_data_year[j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                        if (time == s) {
                            c = _this.data.weather_data_year[j].js;
                            f = _this.data.weather_data_year[j].fs;
                        }
                    }
                    for (var j = 0; j < _this.data.gywr_List_mouth[0].length; j++) {
                        var time = new Date(parseInt(_this.data.gywr_List_mouth[0][j].time.replace("/Date(", "").replace(")/", ""))).getMonth() + 1;
                        if (time == s) {
                            switch (type) {
                                case "OHI":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_co + _this.data.gywr_List_mouth[0][j].gy_so2 + _this.data.gywr_List_mouth[0][j].gy_no + _this.data.gywr_List_mouth[0][j].gy_dust + _this.data.gywr_List_mouth[0][j].gy_cod);
                                    break;
                                case "PM2.5":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_dust);
                                    break;
                                case "PM10":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_dust);
                                    break;
                                case "SO2":
                                    g = parseInt(_this.data.gywr_List_mouth[0][j].gy_so2);
                                    break;
                                case "NO2":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_no);
                                    break;
                                case "CO":
                                    g = parseInt(_this.data.gywr_List_day[0][j].gy_co);
                                    break;
                                default:
                            }
                        }
                    }

                    data_aqi.push(y);
                    data_wq.push(c);
                    data_gy.push(g);
                    data_fs.push(f);
                    x.push(s + "月");
                    x_full.push(s + "月");
                }
                x.reverse();
                other_data = data_aqi.reverse();
                data_weiqi = data_wq.reverse();
                data_fengsu = data_fs.reverse();
                data_gongye = data_gy.reverse();
                x_full.reverse();
                other_data_full = data_aqi.reverse();
                data_weiqi_full = data_wq.reverse();
                data_fengsu_full = data_fs.reverse();
                data_gongye_full = data_gy.reverse();

            }


            $("#chart_head").html(type + "分析图");
            $("#chart_head_full").html(type + "分析图");
            $('#objMonthPicTotal_2').highcharts({
                title: {
                    text: '',
                },
                xAxis: {
                    //categories: x,
                    type: 'datetime',
                    //maxZoom: 48 * 3600 * 1000,
                    //minRange: 1 * 24 * 3600000,
                    //labels: {
                    //    style: {
                    //        fontSize: '20px'
                    //    }
                    //}
                },
                yAxis: [{
                    labels: {
                        y: 10,
                        style: {
                            fontSize: '20px'
                        }
                    },
                    title: {
                        text: '',
                    },
                    opposite: false

                }, { //工业
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { //降水
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { // 风速
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                }],
                plotOptions: {
                    areaspline: {
                        fillColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#C78111'],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 2
                            }
                        },
                        threshold: null
                    },
                    //areaspline: {
                    //    fillOpacity: 0.5
                    //}
                },
                series: [
                    //{
                    //    name: '<div style="font-size:20px;">工业</div>',
                    //    color: '#FE0000',
                    //    dashStyle: 'dash',
                    //    type: 'spline',
                    //    yAxis: 1,
                    //    data: data_gongye
                    //},
                    {
                        name: '<div style="font-size:20px;">降水量</div>',
                        color: '#84DA46',
                        yAxis: 2,
                        type: 'spline',
                        dashStyle: 'shortdot',
                        pointInterval: 5 * 24 * 3600 * 1000,
                        tooltip: {
                            valueSuffix: ' mm'
                        },
                        data: data_weiqi
                    }, {
                        name: '<div style="font-size:20px;">风速</div>',
                        yAxis: 3,
                        color: '#32A9E3',
                        type: 'spline',
                        pointInterval: 5 * 24 * 3600 * 1000,
                        tooltip: {
                            valueSuffix: ' 级'
                        },
                        data: data_fs
                    },
                {
                    type: 'areaspline',
                    name: '<div style="font-size:20px;">' + name + '</div>',
                    color: '#C78111',
                    pointInterval: 24 * 3600 * 1000,
                    data: other_data
                }]

            });


            $('#objMonthPicTotal_2_full').highcharts({
                title: {
                    text: '',
                },
                xAxis: {
                    categories: x_full,
                    //minRange: 14 * 24 * 3600000,
                    //labels: {
                    //    style: {
                    //        fontSize: '30px'
                    //    }
                    //}
                },
                yAxis: [{
                    labels: {
                        y: 10,
                        style: {
                            fontSize: '25px'
                        }
                    },
                    title: {
                        text: '',
                    },
                    opposite: false

                }, { //工业
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { //降水
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false
                    }

                }, { // 风速
                    gridLineWidth: 0,
                    title: {
                        text: '',
                    },
                    labels: {
                        enabled: false

                    },
                    opposite: true
                }],
                plotOptions: {
                    area: {
                        fillColor: {
                            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                            stops: [
                                [0, '#C78111'],
                                [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 2
                            }
                        },
                        threshold: null
                    }
                },
                series: [{
                    name: '<div style="font-size:25px;">工业</div>',
                    color: '#FE0000',
                    dashStyle: 'dash',
                    yAxis: 1,
                    data: data_gongye_full
                }, {
                    name: '<div style="font-size:25px;">降水量</div>',
                    color: '#84DA46',
                    yAxis: 2,
                    dashStyle: 'shortdot',
                    tooltip: {
                        valueSuffix: ' mm'
                    },
                    data: data_weiqi_full
                }, {
                    name: '<div style="font-size:25px;">风速</div>',
                    yAxis: 3,
                    color: '#32A9E3',
                    tooltip: {
                        valueSuffix: ' 级'
                    },
                    data: data_fengsu_full
                }, {
                    type: 'areaspline',
                    name: '<div style="font-size:25px;">' + name + '</div>',
                    color: '#C78111',
                    data: other_data_full
                }]

            });

        },











        setobjMonthPicTotal_column: function () {
            var year = new Date().getFullYear();
            var x, y;
            var year = [];
            for (var i = 0; i < _this.data.pm25_avg[0].length; i++) {
                var time = new Date(parseInt(_this.data.pm25_avg[0][i].time.replace("/Date(", "").replace(")/", ""))).getFullYear() + '年';
                year.push(time);
            }
            x = parseFloat(_this.changeTwoDecimal(_this.data.pm25_avg[0][0].num));
            y = parseFloat(_this.changeTwoDecimal(_this.data.pm25_avg[0][1].num));
            $('#MonthPicTotal_column').highcharts({
                chart: {
                    //plotBorderWidth: 2,
                    //plotBorderColor: '#fff',
                    backgroundColor: 'RGBA(132, 188, 245,.7)',
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
                    categories: year,
                    lineColor: '#fff',
                    lineWidth: 2,
                    labels: {
                        style: {
                            fontSize: '20px',
                            color: '#fff',
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    //plotLines: [{
                    //    value: 0,
                    //    width: 1,
                    //    color: '#808080',
                    //}],
                    lineColor: '#fff',
                    lineWidth: 2,
                    labels: {
                        style: {
                            y: 15,
                            fontSize: '15px',
                            color: '#fff',
                        }
                    }
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
                    pointFormat: '*' + '{point.y}',
                    footerFormat: '<table><tbody><tr><td style="padding:0">{series.name}: </td><td style="padding:0"><b>{point.y}</b></td></tr></tbody></table>',
                    shared: true,
                    useHTML: true
                },
                credits: {
                    enabled: false
                },

                series: [{
                    name: '2013年',
                    data: [78, 78]
                }, {
                    name: '年',
                    data: [x, y]
                }]
            });
        },


        showJiance: function () {
            //$("#map_change_1").css("background-color", "rgba(73,84,70,.8)");
            //$("#map_change_2").css("background-color", "rgba(206,24,21,.8)");
            //$("#jianceIcon").show();
            //$("#heatIcon").hide();
            //$(".timeRelver").hide();
            _this.clear();
            _this.showMarker();
            //_this.aaa();
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
            $("#map_change_2").css("background-color", "rgba(73,84,70,.8)");
            $("#map_change_1").css("background-color", "rgba(206,24,21,.8)");
            $("#jianceIcon").hide();
            $("#heatIcon").show();
            _this.clear();
            _this.showHeatmap(0);
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
                number: 10,
                radius: 1000
            }

            this.map.clear();

            var _markers = [];
            var _markers_1 = [];
            var _markers_2 = [];

            $.each(_this.data.OHI_List[0], function (index, n) {
                var d_id;
                switch (n.DimManageLevelID) {
                    case 0:
                        d_id = "";
                        break;
                    case 1:
                        d_id = "国控";
                        break;
                    case 2:
                        d_id = "省控";
                        break;
                    default:

                }
                var time = new Date(parseInt(n.T_Stamp.replace("/Date(", "").replace(")/", "")));
                if (n.Longitude != 0 && n.Latitude != 0) {
                    var _m = $.demo.map.addCssMarker(n.Longitude, n.Latitude, {
                        addToMapFlag: false,
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
                                _this.poi_name = n.Name + "监测点";
                                _this.setobjMonthPicTotal("day", n.ID, 0, 1, 0);
                                $("#kongqi").css("background-color", "#4DA6F8");
                                $("#jiance").css("background-color", "#107BDD");
                                $("#tubiao2").hide();
                                $("#tubiao").show();
                                $("#zhonglei2").hide();
                                $("#zhonglei").show();
                                $("#p_click_change").hide();
                                $("#p_click").show();
                                $("#type_1_1").html("空气监测");
                            },
                        },
                        popupContent:
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + n.Name + "（" + d_id + "）·</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + n.Address + "</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + "   " + time.getHours() + ":00:00</div>",
                    });
                    _markers.push(_m);
                }
            });
            $.each(_this.data.wq_List[0], function (index, e) {
                var time = new Date(parseInt(e.T_Stamp.replace("/Date(", "").replace(")/", "")));
                var _m = $.demo.map.addCssMarker(e.Latitude, e.Longitude, {
                    addToMapFlag: false,
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
                            _this.poi_name = e.name;
                            $("#kongqi").css("background-color", "#4DA6F8");
                            $("#jiance").css("background-color", "#107BDD");
                            $("#tubiao2").hide();
                            $("#tubiao").show();
                            $("#zhonglei2").hide();
                            $("#zhonglei").show();
                            $("#p_click_change").hide();
                            $("#p_click").show();
                            $("#type_1_1").html("汽车尾气");
                            _this.setobjMonthPicTotal("day", 1, 0, 2, e.name);

                        },
                    },
                    popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>编号：" + e.code + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + e.name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + e.road + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + "   " + time.getHours() + ":00:00</div>",
                });
                _markers_1.push(_m);
            });

            $.each(_this.data.GY_pollution, function (index, f) {
                var time = new Date(parseInt(f.T_Stamp.replace("/Date(", "").replace(")/", "")));
                var _m = $.demo.map.addCssMarker(f.Latitude, f.Longitude, {
                    addToMapFlag: false,
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
                    popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>排口编码：" + f.code + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>站点名称：" + f.name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>企业名称：" + f.site_name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + "   " + time.getHours() + ":00:00</div>",
                });
                _markers_2.push(_m);
            });

            //this.map.addClusterMarkers(_markers, {
            //    maxClusterRadius: param.radius,
            //    spiderfyOnMaxZoom: true,
            //    showCoverageOnHover: true,
            //    zoomToBoundsOnClick: true,
            //    singleMarkerMode: false,
            //    disableClusteringAtZoom: null,
            //    removeOutsideVisibleBounds: true,
            //    animateAddingMarkers: false,
            //    spiderfyDistanceMultiplier: 1,
            //    chunkedLoading: false,
            //    chunkInterval: 200,
            //    chunkDelay: 50,
            //    chunkProgress: null,
            //    polygonOptions: {}
            //});

            this.map.addClusterMarkers(_markers, {
                iconCreateFunction: function (cluster) {
                    return L.divIcon({ html: _this.createVideoMarkerClusterHtml(cluster.getAllChildMarkers().length), className: "video_marker", iconSize: L.point(36, 36), iconAnchor: [18, 18] });
                }
            });
            this.map.addClusterMarkers(_markers_1, {
                iconCreateFunction: function (cluster) {
                    return L.divIcon({ html: _this.createVideoMarkerClusterHtml_1(cluster.getAllChildMarkers().length), className: "video_marker", iconSize: L.point(36, 36), iconAnchor: [18, 18] });
                }
            });
            this.map.addClusterMarkers(_markers_2, {
                iconCreateFunction: function (cluster) {
                    return L.divIcon({ html: _this.createVideoMarkerClusterHtml_2(cluster.getAllChildMarkers().length), className: "video_marker", iconSize: L.point(36, 36), iconAnchor: [18, 18] });
                }
            });

        },

        createVideoMarkerClusterHtml: function (_count) {
            return '<img src="' + CONTENT_URL("Content/images/a_07_k.png") + '" />'
            + '<span class="clusterCount">' + _count + '</span>';
        },
        createVideoMarkerClusterHtml_1: function (_count) {
            return '<img src="' + CONTENT_URL("Content/images/a_03_k.png") + '" />'
            + '<span class="clusterCount">' + _count + '</span>';
        },
        createVideoMarkerClusterHtml_2: function (_count) {
            return '<img src="' + CONTENT_URL("Content/images/a_13.png") + '" />'
            + '<span class="clusterCount">' + _count + '</span>';
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

        heatmap_year: new Date().getFullYear(),
        showHeatmap: function (e, s) {
            if (s == 1) {
                var d = new Date();
                if (_this.time_heatmap == 0) {
                    var t = new Date(d - 1000 * 60 * 60 * 2 * (e - 1)).getHours();
                    var str = t + ":00";
                    $("#heatTime").show();
                    $("#heatmap_num").html(str);
                } else if (_this.time_heatmap == 1) {
                    var t = new Date(d - 1000 * 60 * 60 * 24 * 3 * (e - 1)).getDate();
                    var str = t + "日";
                    $("#heatTime").show();
                    $("#heatmap_num").html(str);
                } else if (_this.time_heatmap == 2) {
                    var t = new Date(d - 1000 * 60 * 60 * 24 * 30 * (e - 1)).getMonth() + 1;
                    var str;
                    if (t == 1) {
                        str = _this.heatmap_year + "-" + t;
                        _this.heatmap_year = _this.heatmap_year - 1;
                    } else {
                        str = _this.heatmap_year + "-" + t;
                    }
                    $("#heatTime").show();
                    $("#heatmap_num").html(str);
                }

            }

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

                var data;

                switch (i + 1) {
                    case 1:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2001;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2001;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2001;
                        }
                    case 2:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2002;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2002;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2002;
                        }
                    case 3:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2003;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2003;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2003;
                        }
                    case 4:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2004;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2004;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2004;
                        }
                    case 5:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2006;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2006;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2006;
                        }
                    case 6:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2007;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2007;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2007;
                        }
                    case 7:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2008;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2008;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2008;
                        }
                    case 8:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2009;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2009;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2009;
                        }
                    case 9:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2010;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2010;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2010;
                        }
                    case 10:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2013;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2013;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2013;
                        }
                    case 11:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2016;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2016;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2016;
                        }
                    case 12:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2022;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2022;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2022;
                        }
                    case 13:
                        if (_this.time_heatmap == 0) {
                            data = _this.data.Hour_2024;
                        } else if (_this.time_heatmap == 1) {
                            data = _this.data.DAY_2024;
                        } else if (_this.time_heatmap == 2) {
                            data = _this.data.Hour_2024;
                        }
                    default:
                }
                //var t1 = new Date(d - 1000 * 60 * 60 * 2 * (e - 1)).getHours();

                if (e != 0) {
                    _testData.data.push({
                        lng: p[i].Longitude + 0.00001 * e,
                        lat: p[i].Latitude + 0.00002 * e,
                        value: _this.getRandomNum(2, 3)
                    });
                } else {
                    _testData.data.push({
                        lng: p[i].Longitude,
                        lat: p[i].Latitude,
                        value: (data[i].OHI)
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
        },

        showArea: function () {
            $.each(_this.data.DATALIST, function (index, ele) {
                _this.map.addPolygon(ele.AreaOutline, {
                    options: _this.GetColorStyle(ele, 1),
                    //hoverOptions: _this.GetColorStyle(ele, 2),
                    attribute: ele,
                    label: {
                        text: ele.AreaName,
                        anchor: [15, -15],
                        bgColor: "rgba(0,0,0,.9)",
                        //bgHoverColor: "rgba(0,0,0,.9)"
                    },
                    popupOption: {
                        maxWidth: 1000
                    },
                    event: {
                        click: function (evt) {
                        }
                    }
                });
            })
        },


        GetColorStyle: function (ele, type) {
            if (type == 1) {
                return {
                    stroke: true,
                    color: ele.AreaColor,
                    weight: 4,
                    opacity: 0.4,
                    fillOpacity: 0.5
                };
            } else if (type == 2) {
                return {
                    stroke: true,
                    color: ele.AreaColor,
                    weight: 0,
                    opacity: 0.2,
                    fillOpacity: 0.9
                };
            } else if (type == 3) {
                return {
                    stroke: true,
                    color: ele.AreaColor,
                    weight: 3,
                    opacity: 1,
                    fillOpacity: 1
                };
            }
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

        changeTwoDecimal: function (v) {
            if (isNaN(v)) {//参数为非数字
                return 0;
            }
            var fv = parseFloat(v);
            fv = Math.round(fv * 100) / 100; //四舍五入，保留两位小数
            var fs = fv.toString();
            var fp = fs.indexOf('.');
            if (fp < 0) {
                fp = fs.length;
                fs += '.';
            }
            while (fs.length <= fp + 2) { //小数位小于两位，则补0
                fs += '0';
            }
            return fs;
        },

        getRandomNum: function (Min, Max) {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        },

        day: function () {
            $("#day").css("background-color", "#117ADC");
            $("#month").css("background-color", "");
            $("#year").css("background-color", "");
            _this.setobjMonthPicTotal("day", 0, 1);
            _this.time_heatmap = 0;
        },

        month: function () {
            $("#month").css("background-color", "#117ADC");
            $("#day").css("background-color", "");
            $("#year").css("background-color", "");
            _this.setobjMonthPicTotal("month", 0, 1);
            //_this.time_heatmap = 1;
            //_this.showHeatmap(0, 0);
        },

        year: function () {
            $("#year").css("background-color", "#117ADC");
            $("#month").css("background-color", "");
            $("#day").css("background-color", "");
            _this.setobjMonthPicTotal("year", 0, 1);
            //_this.time_heatmap = 2;
        },

        fenxi_month: function () {
            $("#month_1").css("background-color", "#117ADC");
            $("#year_1").css("background-color", "");
            _this.setobjMonthPicTotal_fengxi(0, _this.fenxi_type, "month");
        },

        fenxi_year: function () {
            $("#year_1").css("background-color", "#117ADC");
            $("#month_1").css("background-color", "");
            _this.setobjMonthPicTotal_fengxi(0, _this.fenxi_type, "year");
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
            var OHI_list = _this.data.OHI_List[0];
            //var car_Exhaust = _this.data.car_Exhaust;

            var air_list = _this.data.wq_List[0];
            var fac = _this.data.GY_pollution;
            //var now = new Date();
            //var nowStr = now.toLocaleString();
            //var car = _this.data.car;
            $.each(OHI_list, function (index, n) {
                var d_id;
                switch (n.DimManageLevelID) {
                    case 0:
                        d_id = "";
                        break;
                    case 1:
                        d_id = "国控";
                        break;
                    case 2:
                        d_id = "省控";
                        break;
                    default:

                }
                var time = new Date(parseInt(n.T_Stamp.replace("/Date(", "").replace(")/", "")));
                if (n.Longitude != 0 && n.Latitude != 0) {
                    $.demo.map.addCssMarker(n.Longitude, n.Latitude, {
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
                                _this.poi_name = n.Name + "监测点";
                                _this.setobjMonthPicTotal("day", n.ID, 0, 1, 0);
                                $("#kongqi").css("background-color", "#B17718");
                                $("#jiance").css("background-color", "#434343");
                                $("#tubiao2").hide();
                                $("#tubiao").show();
                                $("#zhonglei2").hide();
                                $("#zhonglei").show();
                                $("#p_click_change").hide();
                                $("#p_click").show();
                                $("#type_1_1").html("空气监测");
                            },
                        },
                        popupContent:
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + n.Name + "（" + d_id + "）·</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + n.Address + "</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + "   " + time.getHours() + ":00:00</div>",
                    });
                }
            });

            $.each(fac, function (index, f) {
                var time = new Date(parseInt(f.T_Stamp.replace("/Date(", "").replace(")/", "")));
                $.demo.map.addCssMarker(f.Latitude, f.Longitude, {
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
                    popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>排口编码：" + f.code + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>站点名称：" + f.name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>企业名称：" + f.site_name + "</div>" +
                        "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + "   " + time.getHours() + ":00:00</div>",
                });
            });
            $.each(air_list, function (index, e) {
                if (e.Longitude != 0 && e.Latitude != 0) {
                    var time = new Date(parseInt(e.T_Stamp.replace("/Date(", "").replace(")/", "")));
                    $.demo.map.addCssMarker(e.Latitude, e.Longitude, {
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
                                _this.poi_name = e.name;
                                $("#kongqi").css("background-color", "#B17718");
                                $("#jiance").css("background-color", "#434343");
                                $("#tubiao2").hide();
                                $("#tubiao").show();
                                $("#zhonglei2").hide();
                                $("#zhonglei").show();
                                $("#p_click_change").hide();
                                $("#p_click").show();
                                $("#type_1_1").html("汽车尾气");
                                _this.setobjMonthPicTotal("day", 1, 0, 2, e.name);

                            },
                        },
                        popupContent: "<div style='color:#000; font-size:15px; font-weight:500;'>编号：" + e.code + "</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>名称：" + e.name + "</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>地址：" + e.road + "</div>" +
                            "<div style='color:#000; font-size:15px; font-weight:500; margin-top:10px;'>最新监测时间：" + time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + "   " + time.getHours() + ":00:00</div>",
                    });
                }
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
                _this.showHeatmap(s, 1);
                if (s == 12) {
                    $(".timeRelver").css("background-color", "#8A8A8A");
                    $("#heatTime").hide();
                    clearInterval(time);
                }
            }, 2000);

        },

        KQ: function () {
            $("#type_1").css("background-color", "");
            $("#type_2").css("background-color", "#117ADC");
            $("#type_3").css("background-color", "#117ADC");
            $("#kq_1").show();
            $("#qc_1").hide();
            $("#gywr_1").hide();
        },

        GY: function () {
            $("#type_1").css("background-color", "#117ADC");
            $("#type_2").css("background-color", "");
            $("#type_3").css("background-color", "#117ADC");
            $("#kq_1").hide();
            $("#qc_1").hide();
            $("#gywr_1").show();
        },

        QC: function () {
            $("#type_1").css("background-color", "#117ADC");
            $("#type_2").css("background-color", "#117ADC");
            $("#type_3").css("background-color", "");
            $("#kq_1").hide();
            $("#gywr_1").hide();
            $("#qc_1").show();
        },

        OHI_paihang: function () {
            $("#aqi_paihang").show();
            $("#gy_paihang").hide();
            $("#OHI").css("background-color", "RGBA(83,159,243,.7)");
            $("#gongyewuran").css("background-color", "RGBA(17,122,220,.7)");
        },

        GY_paihang: function () {
            $("#aqi_paihang").hide();
            $("#gy_paihang").show();
            $("#OHI").css("background-color", "RGBA(17,122,220,.7)");
            $("#gongyewuran").css("background-color", "RGBA(83,159,243,.7)");
        },

        showFengxi: function () {
            $("#kongqi").css("background-color", "#107BDD");
            $("#jiance").css("background-color", "#4DA6F8");
            $("#tubiao").hide();
            $("#tubiao2").show();
            $("#zhonglei").hide();
            $("#zhonglei2").show();
            $.demo.setobjMonthPicTotal_fengxi(0, "OHI", "month");

        },

        showKongqi_totle: function () {
            $("#kongqi").css("background-color", "#4DA6F8");
            $("#jiance").css("background-color", "#107BDD");
            $("#tubiao2").hide();
            $("#tubiao").show();
            $("#zhonglei2").hide();
            $("#zhonglei").show();
            $("#p_click_change").show();
            $("#p_click").hide();
            _this.poi_name = null;
            $.demo.setobjMonthPicTotal("day", 0, 0, undefined, 0);
            //location.reload();
        },

        show_full: function () {
            $("#full").slideDown(1000);
        },

        back: function () {
            $("#full").slideUp(1000);
        },
    })
})(jQuery);