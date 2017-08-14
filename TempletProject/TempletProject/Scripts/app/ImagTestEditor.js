(function (app) {


    app.ImagTestEditor = (function () {

        // private functions

        function ImagTest_read(options) {

            var that = this;

            $.ajax({
                type: "get",
                url: this.endPoints.read,
                data: options.data
            }).done(function (data) {
                options.success(data);
                autoFitGrid(that.gridElement.data().kendoGrid, 100);
            }).fail(function (data) {
                options.error(data);
            });


        }

        function ImagTest_create(options) {

            $.ajax({
                type: "post",
                url: this.endPoints.create,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });
        }

        function ImagTest_update(options) {

            $.ajax({
                type: "post",
                url: this.endPoints.update,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
                //updatePristineData(this.kendoGrid, { "Id": options.data.OldId }, data.Data[0]);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });
        }

        function ImagTest_delete(options) {

            $.ajax({
                type: "post",
                url: this.endPoints.destroy,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });
        }

        function show_errors(errormessages) {
            var errors = "";
            if (Array.isArray(errormessages)) {
                for (var i = 0; i < errormessages.length; i++) {
                    errors += errormessages[i] + " </br>";
                }
                alertkendo(errors);
            } else {
                alertkendo(errormessages);
            }

        }


        // constructor
        function ImagTestEditor(gridElement, model, endPoints) {

            // properites
            this.gridElement = gridElement;
            this.model = model;
            this.endPoints = endPoints;

        }

        ImagTestEditor.prototype.load = function () {

            var that = this;

            this.kendoGrid = that.gridElement.kendoGrid(
            {
                "columns": [
                       {
                           command: [
                      "edit",
                      {
                          name: "מחק",
                          click: function (e) {

                              e.preventDefault(); //prevent page scroll reset
                              var tr = $(e.target).closest("tr"); //get the row for deletion
                              var data = this.dataItem(tr); //get the row data so it can be referred later

                              kendoConfirm("אזהרה", "? האם אתה בטוח שברצונך למחוק את הישות", function () {
                                  that.kendoGrid.dataSource.remove(data);
                                  that.kendoGrid.dataSource.sync();
                              }, function () {

                              });

                          },
                      }
                           ], width: 170, widthFixed: true
                       },
                    { field: "Branch", title: " ", width: "65px" },
                    { field: "Subject1", title: "נושא 1", width: "65px", template: "<div class='imageContainer'><div class='#=Image1#'></div><span>#=Subject1#</span><div>" },
                    { field: "Subject2", title: "נושא 2", width: "65px", template: "<div class='imageContainer'><div class='#=Image2#'></div><span>#=Subject2#</span><div>" },
                    { field: "Subject3", title: "נושא 3", width: "65px", template: "<div class='imageContainer'><div class='#=Image3#'></div><span>#=Subject3#</span><div>" },
                    { field: "Subject4", title: "נושא 4", width: "65px", template: "<div class='imageContainer'><div class='#=Image4#'></div><span>#=Subject4#</span><div>" },
                    { field: "Subject5", title: "נושא 5", width: "65px", template: "<div class='imageContainer'><div class='#=Image5#'></div><span>#=Subject5#</span><div>" },
                    { field: "Subject6", title: "נושא 6", width: "65px", template: "<div class='imageContainer'><div class='#=Image6#'></div><span>#=Subject6#</span><div>" },
                    { field: "Subject7", title: "נושא 7", width: "65px", template: "<div class='imageContainer'><div class='#=Image7#'></div><span>#=Subject7#</span><div>" },
                    { field: "Subject8", title: "נושא 8", width: "65px", template: "<div class='imageContainer'><div class='#=Image8#'></div><span>#=Subject8#</span><div>" },
                    { field: "Subject9", title: "נושא 9", width: "65px", template: "<div class='imageContainer'><div class='#=Image9#'></div><span>#=Subject9#</span><div>" },
                    { field: "Subject10", title: "נושא 10", width: "65px", template: "<div class='imageContainer'><div class='#=Image10#'></div><span>#=Subject10#</span><div>" },
                    { field: "Subject11", title: "נושא 11", width: "65px", template: "<div class='imageContainer'><div class='#=Image11#'></div><span>#=Subject11 #</span><div>" }

                ],
                "scrollable": true,
                toolbar: ["create"], 
                editable: "inline",
                height: 600,
                resizable: true,
                autoSync: true,
                "dataSource": {
                    transport: {
                        read: ImagTest_read.bind(this),
                        create: ImagTest_create.bind(this),
                        update: ImagTest_update.bind(this),
                        destroy: ImagTest_delete.bind(this),
                        error: function (a) {
                            show_errors(a);
                        }
                    },
                    "schema": {
                        "data": "Data",
                        "total": "Total",
                        "errors": "Errors",
                        model: {
                            id: "Id",
                            fields: {
                                Branch: { type: "string" },
                                Subject1: { type: "string" },
                                Image1: { type: "string" },
                                Subject2: { type: "string" },
                                Image2: { type: "string" },
                                Subject3: { type: "string" },
                                Image3: { type: "string" },
                                Subject4: { type: "string" },
                                Image4: { type: "string" },
                                Subject5: { type: "string" },
                                Image5: { type: "string" },
                                Subject6: { type: "string" },
                                Image6: { type: "string" },
                                Subject7: { type: "string" },
                                Image7: { type: "string" },
                                Subject8: { type: "string" },
                                Image8: { type: "string" },
                                Subject9: { type: "string" },
                                Image9: { type: "string" },
                                Subject10: { type: "string" },
                                Image10: { type: "string" },
                                Subject11: { type: "string" },
                                Image11: { type: "string" },
                            }
                        }
                    }
                }
            }).data().kendoGrid;

            autoFitGrid(this.kendoGrid, 100);

            $(window).on("resize", function () {
                kendo.resize($(that.gridElement));
            });
        }

        return ImagTestEditor;
    }());


})(window.app = window.app || {});