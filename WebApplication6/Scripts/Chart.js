function renderChart(like, dislike) {
    if (!Highcharts.theme) {
        Highcharts.setOptions({
            reflow: false,
            chart: {
                backgroundColor: "none",
               

            },
            colors: ['#F62366', '#9DFF02'],
            title: {
                style: {
                    color: 'silver'
                }
            },
            tooltip: {
                style: {
                    color: 'silver'
                },

            },
            credits: {
                enabled: false
            }
        });
    }

    /**
     * In the chart render event, add icons on top of the circular shapes
     */
    Highcharts.chart('chart-container', {
        chart: {
            type: 'solidgauge',
            height: '160%',
            margin: [10, 0, 0, 0],
            renderTo: 'chart-container',
            width: 180,
            height: 200,
        },

        title: {
            text: 'Рейтинг',
            style: {
                fontSize: '24px'
            }
        },

        tooltip: {
            borderWidth: 0,
            backgroundColor: 'none',
            shadow: false,
            style: {
                fontSize: '14px',
            },
            pointFormat: '{series.name}<br><span style="font-size:1,2em; color: {point.color}; font-weight: bold">{point.y}</span>',
            positioner: function (labelWidth) {
                return {
                    x: (this.chart.chartWidth - labelWidth) / 2,
                    y: (this.chart.plotHeight / 2) - 15
                };
            }
        },

        pane: {
            startAngle: 0,
            endAngle: 360,
            background: [{
                // Track for pf
                outerRadius: '87%',
                innerRadius: '63%',
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[1])
                    .setOpacity(0.3)
                    .get(),
                borderWidth: 0
            }, { // Track for Stand
                outerRadius: '62%',
                innerRadius: '38%',
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[0])
                    .setOpacity(0.3)
                    .get(),
                borderWidth: 0
            }]
        },

        yAxis: {
            min: 0,
            max: 100,
            lineWidth: 0,
            tickPositions: []
        },

        plotOptions: {
            solidgauge: {
                dataLabels: {
                    enabled: false
                },
                stickyTracking: true,
                rounded: true
            }
        },

        series: [{
            name: 'За',
            data: [{
                color: Highcharts.getOptions().colors[1],
                radius: '87%',
                innerRadius: '63%',
                y: like
            }],
            events: {
                mouseOver: function () {
                    document.getElementById("lable").classList.add("lable-hide");
                    document.getElementById("lable").classList.remove("lable-show");
                },
                mouseOut: function () {
                    document.getElementById("lable").classList.remove("lable-hide");
                    document.getElementById("lable").classList.add("lable-show");
                }
            }
        }
            ,
        {
            name: 'Против',
            data: [{
                color: Highcharts.getOptions().colors[0],
                radius: '62%',
                innerRadius: '38%',
                y: dislike
            }],
            events: {
                afterAnimate: function () {
                    document.getElementById("chart-container").insertAdjacentHTML('beforeend', "<div id='lable'>" + (like - dislike) + "</div>");
                },
                mouseOver: function () {
                    document.getElementById("lable").classList.add("lable-hide");
                    document.getElementById("lable").classList.remove("lable-show");
                },
                mouseOut: function () {
                    document.getElementById("lable").classList.remove("lable-hide");
                    document.getElementById("lable").classList.add("lable-show");
                }
            }
        }]
    });
}