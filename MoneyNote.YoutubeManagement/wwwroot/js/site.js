var App = {
    getToken: function () {
        if (localStorage) { return localStorage.getItem("usertoken"); }
        return '';
    },
    guidEmpty: function () {
        return '00000000-0000-0000-0000-000000000000';

    },
    guidCreate: function () {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
    }
}

$.ajaxSetup({
    headers: { 'Authorization': 'Bearer ' + App.getToken() }
});

var Auth = {
    login: function (uidDomId, pwdDomId) {
        var uid = jQuery('#' + uidDomId).val();
        var pwd = jQuery('#' + pwdDomId).val();

        jQuery.ajax({
            method: "POST",
            url: "/Login",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ Username: uid, Password: pwd })
        }).done(function (msg) {
            if (msg.code == 0) {

                if (localStorage) { localStorage.setItem("usertoken", msg.data); }

                window.location = "/";
            } else {
                alert(msg.message);
            }
        });
    }
}

var Category = {
    _$grid: null,
    _data: [],
    init: function ($grid) {
        Category._$grid = $grid;

        Category.loadData(function () {
            Category.loadGrid();
        });
    },
    loadData: function (funcCallback) {
        jQuery.ajax({
            method: "POST",
            url: "/Category/SelectAll",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({})
        }).done(function (msg) {

            Category._data = msg.data;

            if (funcCallback) {
                funcCallback();
            }
        });
    },
    findById: function (id) {
        for (var i of Category._data) {
            if (i.id == id) return i;
        }

        return null;
    },
    findAllParent: function (id) {
        var res = [];
        var current = Category.findById(id);
        while (true) {
            if (current.parentId == null || current.parentId == 'undefined' || current.parentId == App.guidEmpty()) {
                break;
            }
            current = Category.findById(current.parentId);

            if (current != null) res.push(current);
            else break;
        }
        res.reverse();
        return res;
    },   
    loadGrid: function () {
        Category._$grid = Category._$grid.jsGrid({

            height: "auto",
            width: "100%",

            heading: true,
            filtering: true,
            inserting: true,
            editing: true,
            selecting: true,
            sorting: true,

            paging: true,
            pageLoading: true,
            autoload: true,

            pageSize: 50,
            pageButtonCount: 5,
            deleteConfirm: function (itm) {
                return "The item: '" + itm.title + "' will be removed. Are you sure?";
            },
            rowClick: function (args) {
                //showDetailsDialog("Edit", args.item);
            },
            controller: {
                loadData: function (filter) {
                    var defer = jQuery.Deferred();
                    jQuery.ajax({
                        method: "POST",
                        url: "/Category/SelectAll",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(filter)
                    }).done(function (msg) {

                        var dataResult = {
                            data: msg.data,
                            itemsCount: msg.data.length
                        };

                        defer.resolve(dataResult);
                    });

                    return defer.promise();
                },
                insertItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Category/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ title: itm.title, parentId: itm.parentId })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Category.loadData(function () {
                                Category._$grid.jsGrid("search");
                            });
                        } else {
                            alert(msg.message);
                        }
                    });
                },
                updateItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Category/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            title: itm.title,
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' ? App.guidEmpty() : itm.parentId,
                            id: itm.id == null || itm.id == 'undefinded' ? App.guidEmpty() : itm.id
                        })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Category.loadData(function () {
                                Category._$grid.jsGrid("search");
                            });
                        } else {
                            alert(msg.message);
                        }
                    });
                },
                deleteItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Category/Delete",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ id: itm.id })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Category.loadData(function () {
                                Category._$grid.jsGrid("search");
                            });
                        } else {
                            alert(msg.message);
                        }
                    });
                }
            },
            fields: [
                {
                    headerTemplate: "Parent", name: "parentId",
                    type: "select", items: Category._data, valueField: "id", textField: "title",
                    itemTemplate: function (val, item) {
                        var parent = Category.findById(item.parentId);
                        if (parent == null) return '';
                        return parent.title;
                    }
                },
                {
                    headerTemplate: "Title", name: "title", type: "text", width: 150,
                    itemTemplate: function (val, item) {
                        var parents = Category.findAllParent(item.id);
                        var text = '';
                        for (var i of parents) {
                            text+='<small>'+i.title+'</small> /&nbsp;'
                        }
                        return "<i>"+ text+"</i><br>" + item.title;
                    }
                },
                { type: "control" }
            ]
        });

        /*,

            onSelectRow: function (id) {
                if (id && id !== Category._selectedRowId) {
                    Category._$grid.restoreRow(Category._selectedRowId);
                    Category._selectedRowId = id;
                }
                Category._$grid.editRow(id, true);
            }*/
    }
}

