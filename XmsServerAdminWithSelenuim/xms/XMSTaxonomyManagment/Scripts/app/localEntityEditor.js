(function (app) {

    app.localEntityEditor = (function () {

        // private functions
         
        function LocalEntity_read(options) {

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
         
        function LocalEntity_create(options) {
              
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

        function LocalEntity_update(options) {
             
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
         
        function LocalEntity_delete(options) {
              
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
        function localEntityEditor(gridElement, model, endPoints) {

            // properites
            this.gridElement = gridElement;
            this.model = model;
            this.endPoints = endPoints; 

        }
       
        localEntityEditor.prototype.load = function () {

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
                "height": 600,
                "dataSource": {
                    transport: {
                        read: LocalEntity_read.bind(this),
                        create: LocalEntity_create.bind(this),
                        update: LocalEntity_update.bind(this),
                        destroy: LocalEntity_delete.bind(this),
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
                                OldId: { type: "string" },
                            }
                        }
                    }
                }
            }).data().kendoGrid;


            autoFitGrid(this.kendoGrid, 100);
        }
        localEntityEditor.prototype.refresh = function (gridElement) {
            var that = this;
            that.gridElement.data('kendoGrid').dataSource.read();
        }

        return localEntityEditor;
    }());


})(window.app = window.app || {});