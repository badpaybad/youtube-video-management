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

            Category._data.unshift({ id: App.guidEmpty(), title: 'Find root only' });
            Category._data.unshift({ id: -1, title: 'All' });

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
                    if (filter.parentId == null || filter.parentId == 'undefined') filter.parentId = App.guidEmpty();
                    if (filter.categoryIds == null || filter.categoryIds == 'undefined') filter.categoryIds = [];
                    if (filter.categoryIds.includes(-1)) {
                        filter.categoryIds = [];
                    }
                    if (filter.categoryIds.includes(App.guidEmpty())) {
                        filter.findRootItem = true;
                    }

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
                        data: JSON.stringify({
                            title: itm.title,
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' ? App.guidEmpty() : itm.parentId
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
                    },
                    filtering: false
                },
                {
                    headerTemplate: "Title", name: "title", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        var parents = Category.findAllParent(item.id);
                        var text = '';
                        for (var i of parents) {
                            text += '<small>' + i.title + '</small>/ '
                        }
                        return "<i>/" + text + "</i><br>" + item.title;
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

var Content = {
    _$grid: null,
    _data: [],
    _listCategory: [],
    _listRelation: [],
    init: function ($grid) {
        Content._$grid = $grid;
        Category.loadData(function () {
            Content._listCategory = Category._data;
            Content.loadGrid();
        });

    },
    findCategoryIdRelationToConentId: function (id) {
        var res = [];
        for (var i of Content._listRelation) {
            if (i.contentId == id) {
                res.push(i.categoryId);
            }
        }
        return res;
    },
    findCategoryByRelationIds: function (relationIds) {
        var res = [];
        for (var i of Content._listCategory) {
            if (relationIds.includes(i.id)) {
                res.push(i);
            }
        }
        return res;
    },
    loadGrid: function () {
        Content._$grid = Content._$grid.jsGrid({

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

            pageSize: 10,
            pageButtonCount: 5,
            deleteConfirm: function (itm) {
                return "The item: '" + itm.title + "' will be removed. Are you sure?";
            },
            rowClick: function (args) {
                //showDetailsDialog("Edit", args.item);
            },
            controller: {
                loadData: function (filter) {

                    if (filter.parentId == null || filter.parentId == 'undefined') filter.parentId = App.guidEmpty();
                    if (filter.categoryIds == null || filter.categoryIds == 'undefined') filter.categoryIds = [];
                    if (filter.parentId == null || filter.parentId == 'undefined') filter.parentId = App.guidEmpty();
                    if (filter.categoryIds == null || filter.categoryIds == 'undefined') filter.categoryIds = [];
                    if (filter.categoryIds.includes(-1)) {
                        filter.categoryIds = [];
                    }
                    if (filter.categoryIds.includes(App.guidEmpty())) {
                        filter.findRootItem = true;
                    }

                    var defer = jQuery.Deferred();
                    jQuery.ajax({
                        method: "POST",
                        url: "/Content/SelectAll",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(filter)
                    }).done(function (msg) {

                        var dataResult = {
                            data: msg.data.data,
                            itemsCount: msg.data.itemsCount
                        };

                        //Content._listCategory = msg.data.listCategory;
                        Content._listRelation = msg.data.listRelation;

                        defer.resolve(dataResult);
                    });

                    return defer.promise();
                },
                insertItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Content/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            title: itm.title,
                            urlRef: itm.urlRef,
                            thumbnail: itm.thumbnail,
                            description: itm.description,
                            categoryIds: itm.categoryIds,
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' ? App.guidEmpty() : itm.parentId
                        })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Content._$grid.jsGrid("search");
                        } else {
                            alert(msg.message);
                        }
                    });
                },
                updateItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Content/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            title: itm.title,
                            urlRef: itm.urlRef,
                            thumbnail: itm.thumbnail,
                            description: itm.description,
                            categoryIds: itm.categoryIds,
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' ? App.guidEmpty() : itm.parentId,
                            id: itm.id == null || itm.id == 'undefinded' ? App.guidEmpty() : itm.id
                        })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Content._$grid.jsGrid("search");
                        } else {
                            alert(msg.message);
                        }
                    });
                },
                deleteItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Content/Delete",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ id: itm.id })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Content._$grid.jsGrid("search");
                        } else {
                            alert(msg.message);
                        }
                    });
                }
            },
            fields: [
                {
                    headerTemplate: "Category", name: "categoryIds", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        var catIds = Content.findCategoryIdRelationToConentId(item.id);
                        var cats = Content.findCategoryByRelationIds(catIds);
                        var text = '/';
                        for (var i of cats) {
                            text += i.title + "/ ";
                        }
                        return text;
                    },
                    editTemplate: function (val, item) {
                        var catIds = Content.findCategoryIdRelationToConentId(item.id);

                        var text = '';

                        for (var i of Content._listCategory) {
                            var checked = catIds.includes(i.id) ? "checked" : "";
                            var template = `<div class='form-check' id='editContent'>
                                        <input class="form-check-input" value='${i.id}' ${checked} type="checkbox">
                                        <label class="form-check-label">${i.title}</label>
                                        </div>`;
                            text += template;
                        }

                        return text;
                    },
                    editValue: function (val, item) {
                        var res = [];
                        $.each($("#editContent input:checked"), function () {
                            res.push(jQuery(this).val());
                        });
                        return res;
                    },
                    insertTemplate: function (val, item) {
                        var text = '';
                        for (var i of Content._listCategory) {
                            var template = `<div class='form-check' id='addContent'>
                                        <input class="form-check-input" value='${i.id}' type="checkbox">
                                        <label class="form-check-label">${i.title}</label>
                                        </div>`;
                            text += template;
                        }

                        return text;
                    },
                    insertValue: function (val, item) {
                        var res = [];
                        $.each($("#addContent input:checked"), function () {
                            res.push(jQuery(this).val());
                        });
                        return res;
                    },
                    filterTemplate: function (val, item) {
                        var text = "<div  id='filterContent'>";
                        for (var i of Content._listCategory) {
                            var template = `
                                        <input value='${i.id}' type="checkbox">
                                        ${i.title} &nbsp;
                                       `;
                            text += template;
                        }

                        return text + " </div>";
                    },
                    filterValue: function (val, item) {
                        var res = [];
                        $.each($("#filterContent input:checked"), function () {
                            res.push(jQuery(this).val());
                        });
                        return res;
                    }
                },
                {
                    headerTemplate: "Title", name: "title", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return item.title;
                    }
                }, {
                    headerTemplate: "UrlRef", name: "urlRef", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return item.urlRef;
                    }
                }, {
                    headerTemplate: "Description", name: "description", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return item.description;
                    }
                },
                {
                    headerTemplate: "Thumbnail", name: "thumbnail", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return item.thumbnail;
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

var Admin = {
    _$grid: null,
    _data: [],
    _listModule: [],
    _listPermission: [],
    _listUserAcl: [],
    init: function ($grid) {
        Admin._$grid = $grid;
        Admin.loadData(function () {

            Admin.loadGrid();

        });
    },
    loadData: function (funcCallback) {

        jQuery.ajax({
            method: "POST",
            url: "/Admin/SelectAllPermission",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({})
        }).done(function (msg) {

            Admin._listPermission = msg.data;

            for (var i of Admin._listPermission) {
                if (Admin._listModule.includes(i.moduleCode) == false) {
                    Admin._listModule.push(i.moduleCode);
                }
            }

            if (funcCallback) {
                funcCallback();
            }
        });
    }
    ,
    findAclByUserId: function (userId) {
        var res = [];
        for (var i of Admin._listUserAcl) {
            if (i.userId == userId) {
                res.push(i);
            }
        }
        return res;
    }
    ,

    loadGrid: function () {
        Admin._$grid = Admin._$grid.jsGrid({

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

            pageSize: 10,
            pageButtonCount: 5,
            deleteConfirm: function (itm) {
                return "The item: '" + itm.title + "' will be removed. Are you sure?";
            },
            controller: {
                loadData: function (filter) {

                    if (filter.parentId == null || filter.parentId == 'undefined') filter.parentId = App.guidEmpty();
                    if (filter.categoryIds == null || filter.categoryIds == 'undefined') filter.categoryIds = [];
                    if (filter.parentId == null || filter.parentId == 'undefined') filter.parentId = App.guidEmpty();
                    if (filter.categoryIds == null || filter.categoryIds == 'undefined') filter.categoryIds = [];
                    if (filter.categoryIds.includes(-1)) {
                        filter.categoryIds = [];
                    }
                    if (filter.categoryIds.includes(App.guidEmpty())) {
                        filter.findRootItem = true;
                    }

                    var defer = jQuery.Deferred();
                    jQuery.ajax({
                        method: "POST",
                        url: "/Admin/SelectAll",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(filter)
                    }).done(function (msg) {

                        var dataResult = {
                            data: msg.data.data,
                            itemsCount: msg.data.itemsCount
                        };

                        Admin._listUserAcl = msg.data.listUserAcl;
                        if (Admin._listUserAcl == null || Admin._listUserAcl == 'undefined') {
                            Admin._listUserAcl = [];
                        }

                        defer.resolve(dataResult);
                    });

                    return defer.promise();
                },
                insertItem: function (itm) {
                    var acls = [];
                    if (itm.acls != null && itm.acls != 'undefined') {
                        for (var i of itm.acls) {
                            var arr = i.split('/');
                            acls.push({
                                userId: itm.id,
                                moduleCode: arr[0],
                                permissionCode:arr[1]
                            });
                        }
                    }

                    jQuery.ajax({
                        method: "POST",
                        url: "/Admin/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' ? App.guidEmpty() : itm.parentId,
                            username: itm.username,
                            password: itm.password,
                            acls: acls
                        })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Admin._$grid.jsGrid("search");
                        } else {
                            alert(msg.message);
                        }
                    });
                },
                updateItem: function (itm) {
                    var acls = [];
                    if (itm.acls != null && itm.acls != 'undefined') {
                        for (var i of itm.acls) {
                            var arr = i.split('/');
                            acls.push({
                                userId: itm.id,
                                moduleCode: arr[0],
                                permissionCode: arr[1]
                            });
                        }
                    }
                    jQuery.ajax({
                        method: "POST",
                        url: "/Admin/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            username: itm.username,
                            password: itm.password,
                            acls: acls,
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' ? App.guidEmpty() : itm.parentId,
                            id: itm.id == null || itm.id == 'undefinded' ? App.guidEmpty() : itm.id
                        })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Admin._$grid.jsGrid("search");
                        } else {
                            alert(msg.message);
                        }
                    });
                },
                deleteItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Admin/Delete",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ id: itm.id })
                    }).done(function (msg) {
                        if (msg.code == 0) {
                            Admin._$grid.jsGrid("search");
                        } else {
                            alert(msg.message);
                        }
                    });
                }
            },
            fields: [
                {
                    headerTemplate: "Permision", name: "acls", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        var acls = Admin.findAclByUserId(item.id);

                        var text = '';
                        for (var i of acls) {
                            text += "{" + i.moduleCode + "/" + i.permissionCode + "}";
                        }
                        return text;
                    },
                    editTemplate: function (val, item) {
                        var acls = Admin.findAclByUserId(item.id);

                        var tempAcls = [];
                        for (var i of acls) {
                            tempAcls.push(i.moduleCode + "/" + i.permissionCode);
                        }

                        var text = '';

                        for (var i of Admin._listPermission) {
                            var checked = tempAcls.includes(i.moduleCode + "/" + i.code) ? "checked" : "";

                            var template = `<div class='form-check' id='editUser'>
                                        <input class="form-check-input" value='${i.moduleCode}/${i.code}' ${checked} type="checkbox">
                                        <label class="form-check-label">${i.moduleCode}/${i.code}</label>
                                        </div>`;
                            text += template;
                        }

                        return text;
                    },
                    editValue: function (val, item) {
                        var res = [];
                        $.each($("#editUser input:checked"), function () {
                            res.push(jQuery(this).val());
                        });
                        return res;
                    },
                    insertTemplate: function (val, item) {
                        var text = '';

                        for (var i of Admin._listPermission) {
                            var template = `<div class='form-check' id='addUser'>
                                        <input class="form-check-input" value='${i.moduleCode}/${i.code}' type="checkbox">
                                        <label class="form-check-label">${i.moduleCode}/${i.code}</label>
                                        </div>`;
                            text += template;
                        }

                        return text;
                    },
                    insertValue: function (val, item) {
                        var res = [];
                        $.each($("#addUser input:checked"), function () {
                            res.push(jQuery(this).val());
                        });
                        return res;
                    },
                    filterTemplate: function (val, item) {
                        var text = "<div  id='filterUser'>";
                        for (var i of Admin._listPermission) {
                            var template = `
                                        {<input value='${i.moduleCode}/${i.code}' type="checkbox">
                                        ${i.moduleCode}/${i.code}} &nbsp;
                                       `;
                            text += template;
                        }

                        return text + " </div>";
                    },
                    filterValue: function (val, item) {
                        var res = [];
                        $.each($("#filterUser input:checked"), function () {
                            res.push(jQuery(this).val());
                        });
                        return res;
                    }
                }
                ,
                {
                    headerTemplate: "Username", name: "username", type: "text", width: 150,
                    itemTemplate: function (val, item) {
                        return item.username;
                    },
                     editTemplate: function (val, item) {
                         return item.username + `<input type='hidden' id='username' value='${item.username}'>`;
                    },
                     editValue: function (val, item) {
                         return jQuery('#username').val();
                    }
                }
                ,
                {
                    headerTemplate: "Password", name: "password", type: "text", width: 150,
                    itemTemplate: function (val, item) {
                        return "***";
                    },
                    editTemplate: function (val, item) {
                        return `<input type='text' id='password' placeholder='leave empty to no change password'>`;
                    }
                    ,
                    editValue: function (val, item) {
                        return jQuery('#password').val();
                    }
                    ,
                    filtering: false
                }
                ,
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

