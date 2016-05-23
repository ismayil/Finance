'use strict';
app.factory('rootService', ['$http', '$q','$window', function ($http, $q,$window) {
    var serviceBase = 'http://192.168.77.151/finance/';
    var rootServiceFactory = {};
    var _getTotalValues = function (data) {
        return $http.post(serviceBase + 'api/value/total',data).then(function (results) {
            return results;
        });
    };
    var uri = 'data:application/vnd.ms-excel;base64,',
            template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta charset="utf-8"/></head><body><table>{table}</table></body></html>',
            base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
            format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
    var _excelFactory= {
        tableToExcel: function (tableId, worksheetName) {
            var table = document.getElementById(tableId),
                ctx = { worksheet: worksheetName, table: table.innerHTML },
                href = uri + base64(format(template, ctx));
           // console.log(table);
            return href;
        }
    };
    rootServiceFactory.getTotalValues = _getTotalValues;
    rootServiceFactory.excelFactory = _excelFactory;
    return rootServiceFactory;
}]);