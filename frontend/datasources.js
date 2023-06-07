
var data = { 
	"64803f54258fc800116fef05": [
		{
			"id": 1,
			"timestamp": "2023-06-06 19:40",
			"isSuccessed": true
		},
		{
			"id": 2,
			"timestamp": "2023-06-06 20:40",
			"isSuccessed": true
		},
		{
			"id": 3,
			"timestamp": "2023-06-06 21:40",
			"isSuccessed": true
		},
		{
			"id": 4,
			"timestamp": "2023-06-06 22:40",
			"isSuccessed": true
		},
		{
			"id": 5,
			"timestamp": "2023-06-06 23:40",
			"isSuccessed": false
		},
		{
			"id": 6,
			"timestamp": "2023-06-07 06:40",
			"isSuccessed": false
		}
	]
};
webix.proxy.demo = {
	$proxy:true,
	load:function(view){
		if (view.count && view.count())
			view.clearAll();
		view.parse( webix.copy(data[this.source]||[]) );
	}
};
