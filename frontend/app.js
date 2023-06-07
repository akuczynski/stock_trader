
webix.ready(function(){
	webix.CustomScroll.init();
	webix.ui({
	"rows": [
		{
			"css": "webix_dark",
			"view": "toolbar",
			"height": 44,
			"cols": [
				{
					"label": "Date range",
					"value": {
						"start": "2023-06-07 10:26:57",
						"end": "2023-07-07 10:26:57"
					},
					"view": "daterangepicker",
					"height": 38
				},
				{
					"view": "button",
					"label": "Apply",
					"autowidth": true
				}
			]
		},
		{
			"url": "demo->64803f54258fc800116fef05",
			"columns": [
				{
					"id": "id",
					"header": "id",
					"fillspace": false,
					"hidden": true
				},
				{
					"id": "timestamp",
					"header": "Date",
					"fillspace": true,
					"sort": "date",
					"hidden": false
				},
				{
					"id": "isSuccessed",
					"header": "Success",
					"sort": "int",
					"fillspace": false,
					"hidden": false
				}
			],
			"view": "datatable",
			"width": 0,
			"id": 1686126419014
		}
	]
});
});