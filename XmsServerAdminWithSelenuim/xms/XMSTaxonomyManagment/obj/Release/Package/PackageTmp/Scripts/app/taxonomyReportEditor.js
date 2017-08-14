(function (app) {

    app.taxonomyReportEditor = (function () {

        // private functions

        function taxonomyReport_read(options) {

            var that = this;
            var data = { taxonomyId: that.taxonomyDropDown.data("kendoDropDownList").value().toString() }
            $.ajax({
                type: "get",
                url: this.endPoints.read,
                data: data
            }).done(function (data) {
                options.success(data);
                autoFitGrid(that.gridElement.data().kendoGrid, 100);
            }).fail(function (data) {
                options.error(data);
            }); 
        }

        function taxonomyReport_create(options) {

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

        function taxonomyReport_update(options) {

            $.ajax({
                type: "post",
                url: this.endPoints.update,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                show_errors(data.responseJSON.Message);
            });
        }

        function taxonomyReport_delete(options) {

            var that = this;

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

        function periodTypeDropDownEditor(container, options) {

            $('<input  name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: true,
                    dataTextField: "Description",
                    dataValueField: "PeriodType",
                    dataSource: {
                        type: "json",
                        transport: {
                            read: this.endPoints.getPeriodTypes,
                        }
                    }
                });
        }



        // constructor
        function taxonomyReportEditor(gridElement, taxonomyDropDown, model, endPoints) {

            // properites
            this.taxonomyDropDown = taxonomyDropDown;
            this.gridElement = gridElement;
            this.model = model;
            this.endPoints = endPoints;
            this.lestActiveTaxonomy = model.SelectedTaxonomy.TaxonomyId;

        }

        taxonomyReportEditor.prototype.load = function () {
             
            var that = this;

            for (var i = 0; i < that.model.Taxonomies.length; i++) {
                $(that.taxonomyDropDown).append("<option value=" + that.model.Taxonomies[i].TaxonomyId + ">" + that.model.Taxonomies[i].Description + "</option>")
            }
             
            that.taxonomyDropDown.on('change', function () {
                if ($(that.gridElement).find('.k-grid-edit-row').length) {

                    kendoConfirm("שגיאה", " ? ישנם שינויים שלא נשמרו האם אתה רוצה להמשיך", function () {
                        that.kendoGrid.dataSource.read();
                        that.lestActiveTaxonomy = that.taxonomyDropDown.data("kendoDropDownList").value();

                    }, function () {
                        that.taxonomyDropDown.data("kendoDropDownList").value(that.lestActiveTaxonomy);
                    });

                } else {

                    that.kendoGrid.dataSource.read();
                    that.lestActiveTaxonomy = that.taxonomyDropDown.data("kendoDropDownList").value();

                }
            });

            that.taxonomyDropDown.kendoDropDownList();

            this.kendoGrid = this.gridElement.kendoGrid(
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
                            title: "מזהה הדוח "
                        },
                        {
                            field: "Description",
                            title: "תאור הדוח" 
                        },
                        {
                            field: "PeriodType",
                            title: "סוג תקופה",
                            editor: periodTypeDropDownEditor.bind(that),
                            template: "#=PeriodType.Description#",

                        },
                        {
                            field: "EntryUri",
                            title: "Entry Uri"
                        },
                        {
                            field: "FileName",
                            title: "שם קובץ"
                        },
                        {
                            field: "EntitySchema",
                            title: "Entity Schema"
                        },
                        {
                            field: "EntityIdentifire",
                            title: "מזהה ישות"
                        },
                        {
                            field: "Currency",
                            title: "מטבע"
                        },
                        {
                            field: "Decimals",
                            title: "Decimals"
                        },
                        {
                            field: "DecimalDecimals",
                            title: "Decimal Decimals"
                        }, {
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
                      field: "TnProcessorId",
                      title: "Tn Processor Id"
                  }, {
                      field: "TnRevisionId",
                      title: "Tn Revison Id"
                  }],
                "scrollable": true,
                toolbar: ["create"],
                edit: function edit(e) {

                    if (e.model.isNew() == false) {
                        $('input[name="Id"]').parent().html(e.model.Id);

                    }
                    e.model.TaxonomyId = that.taxonomyDropDown.data("kendoDropDownList").value().toString();

                }, 
                editable: "inline",
                "height": 600,
                "dataSource": {
                    transport: {
                        read: taxonomyReport_read.bind(this),
                        create: taxonomyReport_create.bind(this),
                        update: taxonomyReport_update.bind(this),
                        destroy: taxonomyReport_delete.bind(this),
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
                                EntryUri: { type: "string", validation: { required: true } },
                                FileName: { type: "string", validation: { required: true } },
                                EntityIdentifire: { type: "string", validation: { required: true } },
                                Currency: { type: "string" },
                                Decimals: { type: "string" },
                                EntitySchema: { type: "string", validation: { required: true } },
                                DecimalDecimals: { type: "string" },
                                IntegerDecimals: { type: "string" },
                                MonetaryDecimals: { type: "string" },
                                PureDecimals: { type: "string" },
                                SharesDecimals: { type: "string" },
                                TnProcessorId: { type: "string" },
                                TnRevisionId: { type: "string" },
                                PeriodType: { defaultValue: { PeriodType: this.model.PeriodTypeDefaultValue.PeriodType, Description: this.model.PeriodTypeDefaultValue.Description }, validation: { required: true } },
                                TaxonomyId: { type: "string" }
                            }
                        }
                    }
                }
            }).data().kendoGrid;
           
                autoFitGrid(that.kendoGrid, 100); 
             
        }
        taxonomyReportEditor.prototype.refresh = function (gridElement) {
            var that = this;
            that.gridElement.data('kendoGrid').dataSource.read();
        }
        return taxonomyReportEditor;
    }());


})(window.app = window.app || {});