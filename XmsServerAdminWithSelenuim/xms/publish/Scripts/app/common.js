kendo.culture("he-IL");
kendo.ui.Grid.prototype.options.messages.editable.confirmation = "האם אתה בטוח שברצונך למחוק ?";

function alertkendo(content) {
    
    $("<div></div>").kendoAlert({
        title: "שגיאה",
        content: content,
        messages: {
            okText: "אישור"
        }
    }).data("kendoAlert").open();
}

function updatePristineData(grid, oldKey, model) {
    var pristineData = grid.dataSource._pristineData;

    $.each(pristineData, function (idx, pristineModel) {
        // compare old keys
        var foundModel = true;

        $.each(oldKey, function (key) {
            if (pristineModel[key] != oldKey[key]) {
                foundModel = false;
            }
        });

        if (foundModel) {
            $.each(model, function (key, value) {
                grid.dataSource._pristineData[idx][key] = value;
            });
        }
    });
}
 
function autoFitGrid(grid, minWidth) {
    $.each(grid.columns, function (idx) {
        if (grid.columns[idx].widthFixed) return;
        grid.autoFitColumn(idx);
        var width = grid.columns[idx].width;
        if (width < minWidth) setColumnWidth(grid, idx, minWidth); 
    });

    grid.content.scrollLeft(Number.MAX_SAFE_INTEGER)
}

function setColumnWidth(grid, index, width) {
    var that = grid, options = that.options, columns = that.columns, index, th, headerTable, isLocked, visibleLocked = that.lockedHeader ? leafDataCells(that.lockedHeader.find('>table>thead')).filter(isCellVisible).length : 0, col, notGroupOrHierarchyCol = 'col:not(.k-group-col):not(.k-hierarchy-col)', notGroupOrHierarchyVisibleCell = 'td:visible:not(.k-group-cell):not(.k-hierarchy-cell)';

    var column = that.columns[index];
    var isLocked = column.locked;
    if (isLocked) {
        headerTable = that.lockedHeader.children('table');
    } else {
        headerTable = that.thead.parent();
    }

    var th = headerTable.find('[data-index=\'' + index + '\']');
    var contentTable = isLocked ? that.lockedTable : that.table, footer = that.footer || $();

    var footerTable = footer.find('table').first();
    if (that.lockedHeader && !isLocked) {
        index -= visibleLocked;
    }
    for (var j = 0; j < columns.length; j++) {
        if (columns[j] === column) {
            break;
        } else {
            if (columns[j].hidden) {
                index--;
            }
        }
    }
    if (options.scrollable) {
        col = headerTable.find(notGroupOrHierarchyCol).eq(index).add(contentTable.children('colgroup').find(notGroupOrHierarchyCol).eq(index)).add(footerTable.find('colgroup').find(notGroupOrHierarchyCol).eq(index));
    } else {
        col = contentTable.children('colgroup').find(notGroupOrHierarchyCol).eq(index);
    }

    col.width(width);
}

function kendoConfirm(title, message,done,fail) {
    
    if ($("#confirm").length > 0) {
        $("#confirm").remove();
    }
    $("body").append("<div id='confirm'></div>")

    $("#confirm").kendoConfirm({
        title: title,
        messages: {
            cancel: "ביטול",
            okText: "אישור"
        },
        content: message,

    }).data("kendoConfirm").open().result.done(done).fail(fail);
}

 