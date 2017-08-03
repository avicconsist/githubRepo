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
                updatePristineData(that.kendoGrid, { "Id": options.data.OldId }, data.Data[0]);
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
                           ], width: 200, widthFixed: true
                       },
                {
                    field: "Id",
                    title: "קוד ישות"

                },
                 {
                     field: "Description",
                     title: "תיאור ישות"

                 } 
                ],
                "scrollable": true,
                toolbar: ["create"],
                edit: function edit(e) {

                    if (e.model.isNew() == false) {
                        if (e.model.OldId != e.model.Id)
                            e.model.OldId = e.model.Id;
                    }

                },
                editable: "inline",
                "height": 250,
                resizable: true, 
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
                        "model": {
                            "id": "Id",
                            "fields": {
                                Id: { editable: true, nullable: false, validation: { required: true } },
                                Description: { type: "string", validation: { required: true } },
                                CreatedDate: { type: "date" },
                                OldId: { type: "string" },
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