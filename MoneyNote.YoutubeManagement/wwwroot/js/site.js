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

            Category._data.unshift({ id: App.guidEmpty(), title: 'Root' })

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

                        Category._data = msg.data;

                        Category._data.unshift({ id: App.guidEmpty(), title: 'Root' })

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
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' || itm.parentId == 0 ? App.guidEmpty() : itm.parentId,
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
                updateItem: function (itm) {
                    jQuery.ajax({
                        method: "POST",
                        url: "/Category/CreateOrUpdate",
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            title: itm.title,
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' || itm.parentId == 0 ? App.guidEmpty() : itm.parentId,
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
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' || itm.parentId == 0 ? App.guidEmpty() : itm.parentId,
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
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' || itm.parentId == 0 ? App.guidEmpty() : itm.parentId,
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
                    },
                    insertTemplate: function () {
                        return `<textarea id='contentTitle'></textarea>`;
                    },
                    insertValue: function () {
                        return jQuery('#contentTitle').val();
                    }
                },
                {
                    headerTemplate: "UrlRef", name: "urlRef", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return item.urlRef;
                    },
                    insertTemplate: function () {
                        return `<textarea id='contentUrlRef'></textarea>`;
                    },
                    insertValue: function () {
                        return jQuery('#contentUrlRef').val();
                    }
                },
                {
                    headerTemplate: "Description", name: "description", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return `<textarea id='contentDescription' readonly >${item.description}</textarea>`;
                    },
                    insertTemplate: function () {
                        return `<textarea id='contentDescription'></textarea>`;
                    },
                    insertValue: function () {
                        return jQuery('#contentDescription').val();
                    }
                },
                {
                    headerTemplate: "Thumbnail", name: "thumbnail", type: "textarea", width: 150,
                    itemTemplate: function (val, item) {
                        return item.thumbnail;
                    },
                    insertTemplate: function () {
                        return `<textarea id='contentThumbnail'></textarea>`;
                    },
                    insertValue: function () {
                        return jQuery('#contentThumbnail').val();
                    }
                },
                {
                    type: "control"
                    ,
                    insertTemplate: function () {
                        var $result = jsGrid.fields.control.prototype.insertTemplate.apply(this, arguments);
                        var $myButton = jQuery(`<a href='javascript:void(0)' title='youtube crawl' 
                                    class='info-box-icon bg-warning'><span class='far fa-copy'> &nbsp;</span></a>`);
                        $myButton.on('click', function () {

                            var buttonClicked = jQuery(this).find('span');

                            YoutuberCrawler.askUrl(function (data) {
                                jQuery('#contentTitle').val(data.title);
                                jQuery('#contentUrlRef').val(data.urlRef);
                                jQuery('#contentDescription').val(data.description);
                                jQuery('#contentThumbnail').val(data.thumbnail);

                                jQuery(buttonClicked).attr('class', "far fa-copy");

                            }, function () {
                                jQuery(buttonClicked).attr('class', "fas fa-2x fa-sync-alt fa-spin");
                            });
                        });
                        $result = $result.add(jQuery('<hr>'));
                        return $result.add($myButton);
                    },
                    headerTemplate: function () {
                        var $result = jsGrid.fields.control.prototype.headerTemplate.apply(this, arguments);
                        var $myButton = jQuery(`<a href='javascript:void(0)' title='youtube crawl' 
                                    class='info-box-icon bg-warning'><span class='far fa-copy'>Crawl</span></a>`);
                        $myButton.on('click', function () {

                            var buttonClicked = jQuery(this).find('span');

                            YoutuberCrawler.askUrl(function (data) {

                                Content._$grid.jsGrid("search");

                                jQuery(buttonClicked).attr('class', "far fa-copy");


                            }, function () {
                                jQuery(buttonClicked).attr('class', "fas fa-2x fa-sync-alt fa-spin");
                            }, true);
                        });
                        $result = $result.add(jQuery('<hr>'));
                        return $result.add($myButton);
                    }
                }
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
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' || itm.parentId == 0 ? App.guidEmpty() : itm.parentId,
                            username: itm.username,
                            password: itm.password,
                            acls: acls,
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
                            parentId: itm.parentId == null || itm.parentId == 'undefinded' || itm.parentId == 0 ? App.guidEmpty() : itm.parentId,
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

var YoutuberCrawler = {
    askUrl: function (onSuccess, onBegin, autoSave) {
        var url = prompt("Enter your youtube url");
        if (url != null && url != '') {
            if (!autoSave) autoSave = false;
            onBegin();

            jQuery.ajax({
                method: "POST",
                url: "/Content/YoutubeCrawl",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ url: url, autoSave: autoSave })
            }).done(function (msg) {
                if (msg.code == 0) {
                    onSuccess(msg.data);
                } else {
                    alert(msg.message);
                }
            });
        }
    }
}


var Home = {
    _$content: null,
    _$categoryTree: null,
    _selectedCategory: null,
    _allCategory: null,
    init: function ($categoryTree, $content) {
        Home._$categoryTree = $categoryTree;
        Home._$content = $content;
        Home.loadCategoryTree();
        Home.loadContent();
    },
    findCategoryById: function (id) {
        for (var i of Home._allCategory) {
            if (i.id == id) return i;
        }

        return null;
    },
    _categoryId: null,
    addNewLeft: function () {
        Home._categoryId = App.guidEmpty();
        jQuery('#categoryInfo').attr("style", "");

        jQuery('#categoryTitle').val('');
        var template = ``;        
        template += `<option value='${App.guidEmpty()}' >Root</option>`;

        for (var r of Home._allCategory.filter(i => i.parentId == App.guidEmpty())) {
            r.children = Home._allCategory.filter(i => i.parentId == r.id);
            
            template += `<option value='${r.id}' >${r.title}</option>`;

            for (var r1 of r.children) {
                r1.children = Home._allCategory.filter(i => i.parentId == r1.id);
                
                template += `<option value='${r1.id}' >- ${r1.title}</option>`;
                for (var r2 of r1.children) {
                    
                    template += `<option value='${r2.id}' >-- ${r2.title}</option>`;
                }
            }
        }

        jQuery('#categoryDrlParent').html(template);
    },
    closeLeft: function () {
        jQuery('#categoryInfo').attr("style", "display:none");
    },
    saveLeft: function () {
        if (Home._categoryId == null || Home._categoryId == 'undefined') {
            Home._categoryId = App.guidEmpty()
        }

        jQuery.ajax({
            method: "POST",
            url: "/Category/CreateOrUpdate",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                title: jQuery('#categoryTitle').val(),
                parentId: jQuery('#categoryDrlParent').val(),
                id: Home._categoryId
            })
        }).done(function (msg) {
            if (msg.code == 0) {
                Home.loadCategoryTree();                
            } else {
                alert(msg.message);
            }
        });
    },
    deleteLeft: function () {
        if (Home._categoryId == null || Home._categoryId == 'undefined') return;
        var okDelete = confirm("Do you want to Delete?");
        if (!okDelete) return;

        jQuery.ajax({
            method: "POST",
            url: "/Category/Delete",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: Home._categoryId })
        }).done(function (msg) {
            if (msg.code == 0) {
                Home.loadCategoryTree();
                Home.closeLeft();
            } else {
                alert(msg.message);
            }
        });
    },
    editCategory: function (id) {
        Home._categoryId = id;
        jQuery('#categoryInfo').attr("style", "");

        var cat = Home.findCategoryById(id);
        jQuery('#categoryTitle').val(cat.title);
        var template = ``;
        var selectedRoot = cat.parentId == App.guidEmpty() ? "selected" : "";
        template += `<option value='${App.guidEmpty()}' ${selectedRoot} >Root</option>`;

        for (var r of Home._allCategory.filter(i => i.parentId == App.guidEmpty())) {
            r.children = Home._allCategory.filter(i => i.parentId == r.id);
            var selected = r.id == cat.parentId ? "selected" : "";
            template += `<option value='${r.id}' ${selected} >${r.title}</option>`;

            for (var r1 of r.children) {
                r1.children = Home._allCategory.filter(i => i.parentId == r1.id);
                var selected1 = r1.id == cat.parentId ? "selected":"";
                template += `<option value='${r1.id}' ${selected1}>- ${r1.title}</option>`;
                for (var r2 of r1.children) {
                    var selected2 = r2.id == cat.parentId ? "selected":"";
                    template += `<option value='${r2.id}' ${selected2} >-- ${r2.title}</option>`;
                }
            }
        }

        jQuery('#categoryDrlParent').html(template);
    },
    loadCategoryTree: function () {
        jQuery.ajax({
            method: "POST",
            url: "/Category/GetTree",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({})
        }).done(function (msg) {

            Home._allCategory = msg.data.data;

            var totalItem = Home._allCategory.reduce(function (t, n) {
                return { itemsCount: t.itemsCount + n.itemsCount };
            });
            var idsValid = [];
            var template = `
<div><a href='javascript:void(0)' onclick='Home.selectCategory(0)'>All (${totalItem.itemsCount + msg.data.uncategoryItemsCount})</a></div>
<div><a href='javascript:void(0)' onclick='Home.selectCategory(-1)'>Uncategory (${msg.data.uncategoryItemsCount})</a></div>
`;
            for (var r of Home._allCategory.filter(i => i.parentId == App.guidEmpty())) {
                r.children = Home._allCategory.filter(i => i.parentId == r.id);
                idsValid.push(r.id);
                template += `<div>
<button onclick='Home.editCategory("${r.id}")'>...</button>
<a href='javascript:void(0)'  onclick='Home.selectCategory("${r.id}")' style='padding-left:5px'> ${r.title} (${r.itemsCount})</a></div>`;
                for (var r1 of r.children) {
                    r1.children = Home._allCategory.filter(i => i.parentId == r1.id);
                    idsValid.push(r1.id);
                    template += `<div>
<button onclick='Home.editCategory("${r1.id}")'>...</button>
-<a href='javascript:void(0)'  onclick='Home.selectCategory("${r1.id}")' style='padding-left:5px'> ${r1.title} (${r1.itemsCount})</a></div>`;
                    for (var r2 of r1.children) {
                        idsValid.push(r2.id);
                        template += `<div>
<button onclick='Home.editCategory("${r2.id}")'>...</button>
--<a href='javascript:void(0)'  onclick='Home.selectCategory("${r2.id}")' style='padding-left:10px'>${r2.title} (${r2.itemsCount})</a></div>`;
                    }
                }
            }
            if (idsValid.length > 0) {
                var orphanItems = Home._allCategory.filter(i => !idsValid.includes(i.id));
                template += `<hr>`;
                for (var r of orphanItems) {
                    template += `<div>
<button onclick='Home.editCategory("${r.id}")'>...</button> 
<a href='javascript:void(0)'  onclick='Home.selectCategory("${r.id}")' style='padding-left:5px'> ${r.title} (${r.itemsCount})</a></div>`;
                }
            }

            Home._$categoryTree.html(template);
        });
    },
    selectCategory: function (catId) {
        Home._selectedCategory = catId;
        Home.loadContent();
    },
    loadContent: function () {
        var filter = {
            pageSize: 0,
            pageIndex: 0,
            contentId: App.guidEmpty(),
            parentId: App.guidEmpty(),
            categoryIds: [],
            findRootItem: false,
            title: "",
            urlRef: "",
            description: "",
            thumbnail: ""
        };

        if (Home._selectedCategory == 0 || Home._selectedCategory == null) {
            filter.categoryIds = [];
        } else {
            if (Home._selectedCategory == -1) {
                filter.findRootItem = true;
            } else {
                filter.categoryIds = [Home._selectedCategory];
            }            
        }
        

        jQuery.ajax({
            method: "POST",
            url: "/Content/SelectAll",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(filter)
        }).done(function (msg) {

            var template = ``;

            var i, j, chunk = 3;
            for (i = 0, j = msg.data.data.length; i < j; i += chunk) {
                var temparray = msg.data.data.slice(i, i + chunk);
                template += `<div style="clear:both; ">`
                for (var itm of temparray) {
                    var publised = itm.isDeleted == 1 ? "Publised" : "Unpublish";
                    template += `<div style='width:32%;max-width:32%;float:left; padding-left:1%;padding-bottom:10px;'>
                                    <img src='${itm.thumbnail}' alt='${itm.title}' style='max-width:99%; height:150px'/>
                                    <div style='width:95%; clear:both'>                                                                             
                                            <button onclick='Home.editContent("${itm.id}")'>...</button>
                                            ${itm.title}
                                            <div> ${publised} | views: ${itm.countView} | <a target='_blank' href='${itm.urlRef}'>Origin</a></div>
                                    </div>
                              </div>`;
                }
                template += `</div>`;
            }


            Home._$content.html(template);

        });
    }
    ,
    _contentId: null,
    editContent: function (id) {
        Home._contentId = id;
        var filter = {
            pageSize: 0,
            pageIndex: 0,
            contentId: id,
            parentId: App.guidEmpty(),
            findRootItem: false,
            categoryIds: [],
            title: "",
            urlRef: "",
            description: "",
            thumbnail: ""
        };

        jQuery.ajax({
            method: "POST",
            url: "/Content/SelectAll",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(filter)
        }).done(function (msg) {

            var content = msg.data.data[0];

            jQuery('#mid').attr("class", "col-md-5");
            jQuery('#right').attr("class", "col-md-4");
            jQuery('#right').attr("style", "");


            jQuery('#contentTitle').val(content.title);
            jQuery('#contentUrlRef').val(content.urlRef);
            jQuery('#contentDescription').val(content.description);
            jQuery('#contentThumbnail').val(content.thumbnail);
            jQuery('#contentImgThumbnail').attr("src", content.thumbnail);
            jQuery('#contentOpenUrlRef').attr("href", content.urlRef);

            if (content.isDeleted == 1) {
                jQuery('#contentPublished').prop('checked', true);
            } else {

                jQuery('#contentPublished').prop('checked', false);
            }

            var template = ``;
            for (var r of Home._allCategory.filter(i => i.parentId == App.guidEmpty())) {
                r.children = Home._allCategory.filter(i => i.parentId == r.id);
                var checked = msg.data.listRelation.filter(i => i.categoryId == r.id).length > 0 ? "checked" : "";

                template += `<div class='form-check' >
                                        <input class="form-check-input" value='${r.id}' ${checked} type="checkbox">
                                        <label class="form-check-label">${r.title}</label>
                                        </div>`;

                for (var r1 of r.children) {
                    r1.children = Home._allCategory.filter(i => i.parentId == r1.id);
                    var checked1 = msg.data.listRelation.filter(i => i.categoryId == r1.id).length > 0 ? "checked" : "";

                    template += `<div class='form-check'  > 
                                        <input class="form-check-input" value='${r1.id}' ${checked1} type="checkbox">
                                        <label class="form-check-label">- ${r1.title}</label>
                                        </div>`;
                    for (var r2 of r1.children) {
                        var checked2 = msg.data.listRelation.filter(i => i.categoryId == r2.id).length > 0 ? "checked" : "";

                        template += `<div class='form-check' > 
                                        <input class="form-check-input" value='${r2.id}' ${checked2} type="checkbox">
                                        <label class="form-check-label">-- ${r2.title}</label>
                                        </div>`;

                    }
                }
            }


            jQuery('#contentCategories').html(template);

        });
    },
    closeRight: function () {
        jQuery('#mid').attr("class", "col-md-9");
        jQuery('#right').attr("class", "");
        jQuery('#right').attr("style", "display:none");
    },
    addNewRight: function () {
        Home._contentId = App.guidEmpty();
        jQuery('#mid').attr("class", "col-md-5");
        jQuery('#right').attr("class", "col-md-4");
        jQuery('#right').attr("style", "");

        jQuery('#contentTitle').val('');
        jQuery('#contentUrlRef').val('');
        jQuery('#contentDescription').val('');
        jQuery('#contentThumbnail').val('');
        jQuery('#contentImgThumbnail').attr("src", '');


        jQuery('#contentPublised').prop("checked", false);


        var template = ``;
        for (var r of Home._allCategory.filter(i => i.parentId == App.guidEmpty())) {
            r.children = Home._allCategory.filter(i => i.parentId == r.id);
           
            template += `<div class='form-check' >
                                        <input class="form-check-input" value='${r.id}' type="checkbox">
                                        <label class="form-check-label">${r.title}</label>
                                        </div>`;

            for (var r1 of r.children) {
                r1.children = Home._allCategory.filter(i => i.parentId == r1.id);
             
                template += `<div class='form-check'  > 
                                        <input class="form-check-input" value='${r1.id}'  type="checkbox">
                                        <label class="form-check-label">- ${r1.title}</label>
                                        </div>`;
                for (var r2 of r1.children) {
                   
                    template += `<div class='form-check' > 
                                        <input class="form-check-input" value='${r2.id}'  type="checkbox">
                                        <label class="form-check-label">-- ${r2.title}</label>
                                        </div>`;

                }
            }
        }


        jQuery('#contentCategories').html(template);

    },
    saveRight: function () {
        if (Home._contentId == null || Home._contentId == 'undefined')
            Home._contentId = App.guidEmpty();

        var categories = [];
        $.each($("#contentCategories input:checked"), function () {
            categories.push(jQuery(this).val());
        });
        var isDeleted = $("#contentPublished input:checked").length > 0;
        if (isDeleted == false) {
            isDeleted = $("#contentPublished").prop("checked");
        }

        jQuery.ajax({
            method: "POST",
            url: "/Content/CreateOrUpdate",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                title: jQuery('#contentTitle').val(),
                urlRef: jQuery('#contentUrlRef').val(),
                thumbnail: jQuery('#contentThumbnail').val(),
                description: jQuery('#contentDescription').val(),
                isDeleted: isDeleted==true?1:0,
                categoryIds: categories,
                parentId: App.guidEmpty(),
                id: Home._contentId
            })
        }).done(function (msg) {
            if (msg.code == 0) {
                Home.loadContent();
            } else {
                alert(msg.message);
            }
        });
    },
    deleteRight: function () {
        var okDelete = confirm("Do you want to Delete?");
        if (!okDelete) return;

        jQuery.ajax({
            method: "POST",
            url: "/Content/Delete",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: Home._contentId })
        }).done(function (msg) {
            if (msg.code == 0) {

                Home.loadContent();

                Home._contentId = App.guidEmpty();

                Home.closeRight();

            } else {
                alert(msg.message);
            }
        });

    },
    crawlRight: function (sender) {
        YoutuberCrawler.askUrl(function (data) {

            Home.addNewRight();

            jQuery('#contentTitle').val(data.title);
            jQuery('#contentUrlRef').val(data.urlRef);
            jQuery('#contentDescription').val(data.description);
            jQuery('#contentThumbnail').val(data.thumbnail);
            jQuery('#contentImgThumbnail').attr("src", data.thumbnail);
            jQuery('#contentOpenUrlRef').attr("href", data.urlRef);
            
            jQuery(sender).text('Crawl new');

        }, function () {
            jQuery(sender).text('Crawling ...');
        }, false);
    }
}