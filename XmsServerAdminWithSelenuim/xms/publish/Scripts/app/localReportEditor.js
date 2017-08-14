(function (app) {

    app.localReportEditor = (function () {

        // private functions

        function localReport_read(options) {

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

        function localReport_create(options) {
            var that = this;
            $.ajax({
                type: "post",
                url: that.endPoints.create,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                var message = data.responseJSON === undefined ? "התרחשה שגיאה בשרת" : data.responseJSON.Message;
                show_errors(message);
            });
        }

        function localReport_update(options) {
            var that = this;
            $.ajax({
                type: "post",
                url: this.endPoints.update,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
                updatePristineData(that.kendoGrid, { "Id": options.data.OldId }, data.Data[0]);
            }).fail(function (data) {
                options.error(data);
                var message = data.responseJSON === undefined ? "התרחשה שגיאה בשרת" : data.responseJSON.Message;
                show_errors(message);
            });
        }

        function localReport_delete(options) {

            $.ajax({
                type: "post",
                url: this.endPoints.destroy,
                data: { model: options.data }
            }).done(function (data) {
                options.success(data);
            }).fail(function (data) {
                options.error(data);
                var message = data.responseJSON === undefined ? "התרחשה שגיאה בשרת" : data.responseJSON.Message;
                show_errors(message);
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
                    optionLabel: {
                        Description: "",
                        Id: null
                    },
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

        function sourceIdDropDownEditor(container, options) {
            $('<input  name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: true,
                    dataTextField: "Description",
                    dataValueField: "Id",
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: this.endPoints.getSourceIds,
                                contentType: 'application/json; charset=utf-8',
                                datatype: "json",
                                data: { taxonomyId: this.taxonomyDropDown.data("kendoDropDownList").value().toString() }
                            }
                        }
                    }

                });
        }

        function entityIdentifireDropDownEditor(container, options) {
            $('<input  name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    optionLabel: {
                        Description: "",
                        Id: 0
                    },
                    autoBind: true,
                    dataTextField: "Description",
                    dataValueField: "Id",
                    dataSource: {
                        type: "json",
                        transport: {
                            read: this.endPoints.getLocalEntities,
                        }
                    },

                });
        }


        // constructor
        function localReportEditor(gridElement, taxonomyDropDown, model, endPoints) {

            // properites
            this.taxonomyDropDown = taxonomyDropDown;
            this.gridElement = gridElement;
            this.model = model;
            this.endPoints = endPoints;
            this.lestActiveTaxonomy = model.SelectedTaxonomy.TaxonomyId;
        }

        localReportEditor.prototype.load = function () {

            var that = this;
            for (var i = 0; i < that.model.Taxonomies.length; i++) {
                $(that.taxonomyDropDown).append("<option value=" + that.model.Taxonomies[i].TaxonomyId + ">" + that.model.Taxonomies[i].Description + "</option>")
            }

            that.taxonomyDropDown.kendoDropDownList();

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

                                kendoConfirm("אזהרה", " ? האם אתה בטוח שברצונך למחוק את הישות", function () {
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
                            title: "קוד דוח",


                        },
                        {
                            field: "SourceId",
                            title: "קוד מקור",
                            editor: sourceIdDropDownEditor.bind(this),
                            template: "#=SourceId.Description #",
                        },
                         {
                             field: "Description",
                             title: "תאור דוח"
                         },
                      {
                          field: "PeriodType",
                          title: "סוג תקופה",
                          editor: periodTypeDropDownEditor.bind(this),
                          template: "#=PeriodType.Description==null ? '' : PeriodType.Description #",

                      },
                      {
                          field: "EntityIdentifire",
                          title: "מזהה יישות",
                          editor: entityIdentifireDropDownEditor.bind(this),
                          template: "#=EntityIdentifire.Description==null ? '' : EntityIdentifire.Description#",

                      }, {
                          field: "EntryUri",
                          title: "Entry Uri"
                      },
                      {
                          field: "FileName",
                          title: "שם קובץ",
                      },
                      {
                          field: "EntitySchema",
                          title: "Entity Schema"
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
                          field: "TnProcessorId",
                          title: "Tn Processor Id"
                      }, {
                          field: "TnRevisionId",
                          title: "Tn Revison Id"
                      }
                ],
                "scrollable": true,
                toolbar: ["create"],
                edit: function edit(e) {
                    if (e.model.isNew() == false) {
                        if (e.model.OldId != e.model.Id) {
                            e.model.OldId = e.model.Id
                        }
                        if (e.model.OldSourceId != e.model.SourceId.Id) {
                            e.model.OldSourceId = e.model.SourceId.Id
                        }
                    }
                    e.model.TaxonomyId = that.taxonomyDropDown.data("kendoDropDownList").value().toString();

                },
                editable: "inline",
                "height": 600,
                "dataSource": {
                    transport: {
                        read: localReport_read.bind(this),
                        create: localReport_create.bind(this),
                        update: localReport_update.bind(this),
                        destroy: localReport_delete.bind(this),
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
                                Description: { type: "string" },
                                SourceId: { validation: { required: true }, defaultValue: { Id: "", Description: "" } },
                                EntryUri: { type: "string" },
                                FileName: { type: "string" },
                                EntityIdentifire: { defaultValue: { Id: "", Description: "" } },
                                Currency: { type: "string" },
                                Decimals: { type: "string" },
                                EntitySchema: { type: "string" },
                                DecimalDecimals: { type: "string" },
                                IntegerDecimals: { type: "string" },
                                MonetaryDecimals: { type: "string" },
                                PureDecimals: { type: "string" },
                                SharesDecimals: { type: "string" },
                                TnProcessorId: { type: "string" },
                                TnRevisionId: { type: "string" },
                                PeriodType: { defaultValue: { PeriodType: "", Description: "" } },
                                TaxonomyId: { type: "string" },
                                OldId: { type: "string" },
                                OldSourceId: { type: "string" }
                            }
                        }
                    }
                }
            }).data().kendoGrid;

            autoFitGrid(this.kendoGrid, 100);
        }

        return localReportEditor;
    }());


})(window.app = window.app || {});