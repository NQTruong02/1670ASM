
function DefaultData() {
    const data = [
        ['Revenue source', 'Quantity sold'],
        ['Sport bicycle', 11],
        ['Bicycle racing', 2],
        ['Tourist bike', 2],
        ['Bicycle for children', 20],
        ['Electric bicycle',23]
    ];
    return data;
}


google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart2);

function drawChart2() {

    var data = google.visualization.arrayToDataTable(DefaultData());

    var options = {
        width: 350,
        height: 300,
        chartArea: { left: 10, top: 20, width: "100%", height: "100%" },
        title: ''
    };

        var chart = new google.visualization.PieChart(document.getElementById('piechart'));

    chart.draw(data, options);
}

google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Year', 'Sales', 'Expenses'],
        ['2019', 1000, 400],
        ['2020', 1170, 460],
        ['2021', 660, 1120],
        ['2022', 1030, 540],
        ['2023', 1030, 540]
    ]);

    var options = {
        curveType: 'function',
        legend: { position: 'bottom' }
    };

    var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));

    chart.draw(data, options);
}
