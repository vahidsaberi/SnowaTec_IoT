"use strict";

Highcharts.theme = {
    colors: ['#2b908f', '#90ee7e', '#f45b5b', '#7798BF', '#aaeeee', '#ff0066',
        '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
    chart: {
        backgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 },
            stops: [
                [0, '#283046'],
                [1, '#283046']
            ]
        },
        style: {
            fontFamily: '\'Unica One\', sans-serif'
        },
        plotBorderColor: '#606063'
    },
    title: {
        style: {
            color: '#E0E0E3',
            textTransform: 'uppercase',
            fontSize: '20px'
        }
    },
    subtitle: {
        style: {
            color: '#E0E0E3',
            textTransform: 'uppercase'
        }
    },
    xAxis: {
        gridLineColor: '#707073',
        labels: {
            style: {
                color: '#E0E0E3'
            }
        },
        lineColor: '#707073',
        minorGridLineColor: '#505053',
        tickColor: '#707073',
        title: {
            style: {
                color: '#A0A0A3'
            }
        }
    },
    yAxis: {
        gridLineColor: '#707073',
        labels: {
            style: {
                color: '#E0E0E3'
            }
        },
        lineColor: '#707073',
        minorGridLineColor: '#505053',
        tickColor: '#707073',
        tickWidth: 1,
        title: {
            style: {
                color: '#A0A0A3'
            }
        }
    },
    tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.85)',
        style: {
            color: '#F0F0F0'
        }
    },
    plotOptions: {
        series: {
            dataLabels: {
                color: '#F0F0F3',
                style: {
                    fontSize: '13px'
                }
            },
            marker: {
                lineColor: '#333'
            }
        },
        boxplot: {
            fillColor: '#505053'
        },
        candlestick: {
            lineColor: 'white'
        },
        errorbar: {
            color: 'white'
        }
    },
    legend: {
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
        itemStyle: {
            color: '#E0E0E3'
        },
        itemHoverStyle: {
            color: '#FFF'
        },
        itemHiddenStyle: {
            color: '#606063'
        },
        title: {
            style: {
                color: '#C0C0C0'
            }
        }
    },
    credits: {
        style: {
            color: '#666'
        }
    },
    labels: {
        style: {
            color: '#707073'
        }
    },
    drilldown: {
        activeAxisLabelStyle: {
            color: '#F0F0F3'
        },
        activeDataLabelStyle: {
            color: '#F0F0F3'
        }
    },
    navigation: {
        buttonOptions: {
            symbolStroke: '#DDDDDD',
            theme: {
                fill: '#505053'
            }
        }
    },
    // scroll charts
    rangeSelector: {
        buttonTheme: {
            fill: '#505053',
            stroke: '#000000',
            style: {
                color: '#CCC'
            },
            states: {
                hover: {
                    fill: '#707073',
                    stroke: '#000000',
                    style: {
                        color: 'white'
                    }
                },
                select: {
                    fill: '#000003',
                    stroke: '#000000',
                    style: {
                        color: 'white'
                    }
                }
            }
        },
        inputBoxBorderColor: '#505053',
        inputStyle: {
            backgroundColor: '#333',
            color: 'silver'
        },
        labelStyle: {
            color: 'silver'
        }
    },
    navigator: {
        handles: {
            backgroundColor: '#666',
            borderColor: '#AAA'
        },
        outlineColor: '#CCC',
        maskFill: 'rgba(255,255,255,0.1)',
        series: {
            color: '#7798BF',
            lineColor: '#A6C7ED'
        },
        xAxis: {
            gridLineColor: '#505053'
        }
    },
    scrollbar: {
        barBackgroundColor: '#808083',
        barBorderColor: '#808083',
        buttonArrowColor: '#CCC',
        buttonBackgroundColor: '#606063',
        buttonBorderColor: '#606063',
        rifleColor: '#FFF',
        trackBackgroundColor: '#404043',
        trackBorderColor: '#404043'
    }
};

Highcharts.setOptions(Highcharts.theme);

function getActiveUser() {
    const done = jQuery.Deferred();

    callAjax('ActiveUser', AjaxType.Get, {}, null).then(function (data, status) {
        if (status === 'success') {
            if (data.length) {
                var result = $(data).map(function () {
                    return this.nickName;
                }).get()

                result.push('Total');

                done.resolve(result);
            }
        }

        done.reject(null);
    });

    //$.get("/api/Report/GetActiveUser", function (data, status) {
    //    if (status === 'success') {
    //        if (data.length) {
    //            var result = $(data).map(function () {
    //                return this.nickName;
    //            }).get()

    //            result.push('Total');

    //            done.resolve(result);
    //        }
    //    }

    //    done.reject(null);
    //});

    return done.promise();
}

function getTimlyProfit(nickName, daysAgo) {
    const done = jQuery.Deferred();

    $.get("/Position/GetTimlyProfit", { nickName, daysAgo }, function (data, status) {
        if (status === 'success') {
            done.resolve(data);
        }

        done.reject(null);
    });

    return done.promise();
}

function success(name, data) {

    const done = jQuery.Deferred();

    if (data.length) {
        var json = $.parseJSON(data);
        var result = json.map(function (item) { return [item.OpenDateTime, item.Profit] })

        var i = names.indexOf(name);
        seriesOptions[i] = {
            name: name,
            data: result
        };
    }

    seriesCounter += 1;

    if (seriesCounter === names.length) {
        done.resolve(true);
    }
    else {
        done.resolve(false);
    }

    return done.promise();
}

function createChart(id) {
    Highcharts.stockChart(id, {

        xAxis: {
            title: {
                text: 'Date'
            },
            type: 'datetime'
        },

        yAxis: {
            labels: {
                formatter: function () {
                    return (this.value > 0 ? ' + ' : '') + this.value + '%';
                }
            },
            plotLines: [{
                value: 0,
                width: 2,
                color: 'silver'
            }]
        },

        scrollbar: {
            enabled: false
        },

        rangeSelector: {
            //selected: 4
            enabled: false
        },

        navigator: {
            enabled: false
        },

        plotOptions: {
            series: {
                compare: 'percent',
                showInNavigator: true
            }
        },

        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
            valueDecimals: 2,
            split: true
        },

        series: seriesOptions
    });
}

var names = [],
    seriesOptions = [],
    seriesCounter = 0;

function run(elementId, daysCount) {

    const done = jQuery.Deferred();

    getActiveUser().then(function (employees) {
        if (employees !== null) {
            names = employees;
            if (employees.length) {
                employees.map(function (nickName) {
                    getTimlyProfit(nickName, daysCount).then(function (data) {
                        if (data !== null) {
                            success(nickName, data).then(function (result) {
                                if (result === true) {
                                    createChart(elementId);

                                    done.resolve(true);
                                }
                            });
                        }
                    });
                });
            }
        }
    });

    return done.promise();
}

$(function () {
    (async () => {
        try {

            run('monthlyChart', -30).then(function (result) {
                if (result === true) {
                    names = [];
                    seriesOptions = [];
                    seriesCounter = 0;

                    run('weeklyChart', -7).then(function (result) {
                        if (result === true) {
                            names = [];
                            seriesOptions = [];
                            seriesCounter = 0;

                            run('dailyChart', -1);
                        }
                    });
                }
            });

        } catch (err) {
            console.log(err);
        }
    })()
})