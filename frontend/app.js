
webix.ready(function(){
	webix.CustomScroll.init();
	webix.ui({
	rows: [
		{
			css: "webix_dark",
			view: "toolbar",
			height: 44,
			cols: [
				{
					view:"daterangepicker", 
					name:"datarange",
					label:"Date range",
					stringResult:true, 
					format:"%Y-%F-%d %H:%i", 
					suggest:{
					  view:"daterangesuggest",
					  body:{
						calendarCount:1,
						icons:false,
						timepicker:true
					  }
					},
					value: {
						start: "2023-06-06 08:00:00",
						end: "2023-06-08 10:00:00"
					},
					height: 38
				},
				{
					view: "button",
					label: "Apply",
					autowidth: true,
					click:function(){
						var values = this.getFormView().getValues();
						console.log(values.datarange);
					}
				}
			]
		},
		{
			url: "https://traderazfunctions.azurewebsites.net/api/GetAllLogs?from=2023-06-07%2008:00:00&to=2023-06-07%2016:00:00",
			columns: [
				{
					id: "id",
					header: "id",
					fillspace: false,
					hidden: true
				},
				{
					id: "timestamp",
					header: "Date of import",
					fillspace: true,
					sort: "date",
					hidden: false
				},
				{
					id: "isSucceded",
					header: "Success",
					sort: "int",
					fillspace: false,
					hidden: false
				}
			],
			view: "datatable",
			width: 0,
			id: 1686126419014
		}
	]
});
});