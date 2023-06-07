webix.ready(function () {
  webix.CustomScroll.init();
  webix.ui({
    rows: [
      {
        css: "webix_dark",
        view: "toolbar",
        height: 44,
        cols: [
          {
            view: "daterangepicker",
            name: "datarange",
            label: "Date range",
            stringResult: true,
            format: "%Y-%F-%d %H:%i",
            suggest: {
              view: "daterangesuggest",
              body: {
                calendarCount: 1,
                icons: false,
                timepicker: true,
              },
            },
            value: {
              start: "2023-06-06 08:00:00",
              end: "2023-06-08 10:00:00",
            },
            height: 38,
          },
          {
            view: "button",
            label: "Apply",
            autowidth: true,
            click: function () {
              var datarange = this.getFormView().getValues().datarange;

              var url =
                "https://traderazfunctions.azurewebsites.net/api/GetAllLogs?";
              url +=
                "from=" +
                encodeURIComponent(datarange.start) +
                "&to=" +
                encodeURIComponent(datarange.end);

              $$("maintable").clearAll();
              $$("maintable").load(url);
            },
          },
        ],
      },
      {
        view: "datatable",
        id: "maintable",
        url: "https://traderazfunctions.azurewebsites.net/api/GetAllLogs?from=2023-06-07%2008:00:00&to=2023-06-07%2016:00:00",
        columns: [
          {
            id: "id",
            header: "id",
            fillspace: false,
            hidden: true,
          },
          {
            id: "timestamp",
            header: "Date of import",
            fillspace: true,
            sort: "date",
            hidden: false,
          },
          {
            id: "isSucceded",
            header: "Success",
            sort: "int",
            fillspace: false,
            hidden: false,
          },
        ],
        on: {
          onSelectChange: function () {
            var table = $$("maintable");
            var logId = table.getSelectedId(true).join();

            var selectedRow = table.find(function (obj) {
              return obj.id == logId;
            });

            if (selectedRow[0].isSucceded) {
              var url = "https://traderazfunctions.azurewebsites.net/api/GetLog?id=" + logId;

			  console.log(url);

              $$("subtable").clearAll();
              $$("subtable").load(url);
            }
			else {
			  $$("subtable").clearAll();
			}
          },
        },
        select: "row",
        width: 0,
      },
      { view: "resizer" },
      {
        view: "datatable",
        id: "subtable",
        columns: [
          {
            id: "ticker",
            header: "Ticker",
            fillspace: false,
            hidden: false,
          },
          {
            id: "sentiment",
            header: "Sentiment",
            fillspace: false,
            sort: "string",
            hidden: false,
          },
          {
            id: "sentiment_score",
            header: "Sentiment Score",
            sort: "int",
            fillspace: false,
            hidden: false,
          },
          {
            id: "no_of_comments",
            header: "Nr of comments",
            sort: "int",
            fillspace: true,
            hidden: false,
          },
        ],
        url: "https://traderazfunctions.azurewebsites.net/api/GetLog?id=ca14ea32-9b65-498f-8792-5158a370c9fc",
      },
    ],
  });
});
