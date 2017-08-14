(function (app) {

    app.taxonomyEditor = (function () {

        // private functions
         
        function taxonomies_read(options) {

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

        function transform_model_for_server(model) {

            if (model.TaxonomyDate !== undefined &&
                model.TaxonomyCreationDate !== undefined) {
                model.TaxonomyDate = model.TaxonomyDate.toISOString();
                model.TaxonomyCreationDate = model.TaxonomyCreationDate.toISOString();
            }
            return model; 
        }

        function taxonomies_create(options) {

            var model = transform_model_for_server(options.data);
           
            var that = this;

            $.ajax({
                type: "post",
                url: this.endPoints.create,
                data: { model: model }
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

        function taxonomies_update(options) {

            var model = transform_model_for_server(options.data);
            var that = this;

            $.ajax({
                type: "post",
                url: this.endPoints.update,
                data: { model: model }
            }).done(function (data) {
                options.success(data);
                updatePristineData(that.kendoGrid, { "TaxonomyId": options.data.OldTaxonomyId }, data.Data[0]);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });
        }
         
        function taxonomies_delete(options) {

            var model = transform_model_for_server(options.data);

            $.ajax({
                type: "post",
                url: this.endPoints.destroy,
                data: { model: model }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });
        }
        
        function taxonomyReport_delete(options) {

            var that = this;
            var model = transform_model_for_server(data);

            $.ajax({
                type: "post",
                url: that.endPoints.destroy,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });

        }



        // constructor
        function taxonomyEditor(gridElement, model, endPoints) {

            // properites
            this.gridElement = gridElement;
            this.model = model;
            this.endPoints = endPoints; 

        }
       
        taxonomyEditor.prototype.load = function () {

            var that = this;

            this.kendoGrid = this.gridElement.kendoGrid(
            {
                "columns": [
                     {
                         command: [
                    "edit",
                    {
                        name: "מחק", 
                        click: function (e) {
                            e.preventDefault();
                            var tr = $(e.target).closest("tr");
                            var data = this.dataItem(tr);

                            kendoConfirm("אזהרה", "? האם אתה בטוח שברצונך למחוק את הישות ", function () {
                                that.kendoGrid.dataSource.remove(data);
                                that.kendoGrid.dataSource.sync();
                            }, function () {

                            });

                        }, 
                    }
                         ], width: 200, widthFixed: true
                     },
                     {
                         field: "TaxonomyId",
                         title: "קוד טקסונומיה" 
                     },
                     {
                         field: "Description",
                         title: "תאור"
                     },
                  {
                      field: "TaxonomyDate",
                      title: "תאריך טקסונומיה",
                      format: "{0:MM/dd/yyyy}",
                      widthFixed: true,
                      width: 150

                  }, {
                      field: "EntityIdentifier",
                      title: "Entity Identifier"
                  },
                  {
                      field: "Currency",
                      title: "Currency"
                  },
                   {
                       field: "Decimals",
                       title: "Decimals"
                   },
                   {
                       field: "EntitySchema",
                       title: "Entity Schema"

                   }, {
                       field: "TaxonomyCreationDate",
                       title: "Taxonomy Creation Date",
                       format: "{0:MM/dd/yyyy}",
                       widthFixed: true,
                       width: 150
                   },
                    {
                        field: "TnProcessorId",
                        title: "Tn Processor Id" 
                    },

                   {
                       field: "DecimalDecimals",
                       title: "Decimal Decimals"
                   }
                  , {
                      field: "IntegerDecimals",
                      title: "Integer Decimals"
                  }
                  , {
                      field: "MonetaryDecimals",
                      title: "Monetary Decimals"

                  }, {
                      field: "PureDecimals",
                      title: "Pure Decimals"
                  }, {
                      field: "SharesDecimals",
                      title: "Shares Decimals"
                  }, {
                      field: "TnRevisionId",
                      title: "Tn Revision Id"
                  }
                ],
                "scrollable": true,
                toolbar: ["create"],
                edit: function edit(e) {
                    if (e.model.isNew() == false) {
                        if (e.model.OldTaxonomyId != e.model.TaxonomyId) {
                            e.model.OldTaxonomyId = e.model.TaxonomyId;
                        }
                    } 
                },
                editable: "inline",
                "height": 600,
                "dataSource": {
                    transport: {
                        read: taxonomies_read.bind(this),
                        create: taxonomies_create.bind(this),
                        update: taxonomies_update.bind(this),
                        destroy: taxonomies_delete.bind(this),
                        error: function (a) {
                            show_errors(a);
                        }
                    },

                    "schema": {
                        "data": "Data",
                        "total": "Total",
                        "errors": "Errors",
                        "model": {
                            "id": "TaxonomyId",
                            "fields": {
                                TaxonomyId: { editable: true, validation: { required: true } },
                                Description: { type: "string", validation: { required: true } },
                                TaxonomyDate: { type: "date", validation: { required: true } },
                                EntityIdentifier: { type: "string", validation: { required: true } },
                                Currency: { type: "string", validation: { required: true } },
                                Decimals: { type: "string", validation: { required: true } },
                                EntitySchema: { type: "string", validation: { required: true } },
                                TaxonomyCreationDate: { type: "date", validation: { required: true } },
                                TnProcessorId: { type: "string", validation: { required: true } },
                                DecimalDecimals: { type: "string" },
                                IntegerDecimals: { type: "string" },
                                MonetaryDecimals: { type: "string" },
                                PureDecimals: { type: "string" },
                                SharesDecimals: { type: "string" },
                                TnRevisionId: { type: "string" },
                                OldTaxonomyId: { type: "string" },
                            }
                        }
                    }
                }
            }).data().kendoGrid;
             
            
        }
        taxonomyEditor.prototype.refresh = function (gridElement) {
            var that = this;
            that.gridElement.data('kendoGrid').dataSource.read();
        }

        return taxonomyEditor;
    }());


})(window.app = window.app || {});