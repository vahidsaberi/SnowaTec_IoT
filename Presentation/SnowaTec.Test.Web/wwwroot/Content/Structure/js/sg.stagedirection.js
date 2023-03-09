const RouteMethodType = {
    Pathname: "pathname",
    Href: "href",
    Origin: "origin"
}

function getRoute(routemethodType = RouteMethodType.Pathname) {

    switch (routemethodType) {
        case RouteMethodType.Pathname:
            return window.location.pathname; // Returns path only (/path/example.html)
        case RouteMethodType.Href:
            return window.location.href;     // Returns full URL (https://example.com/path/example.html)
        case RouteMethodType.Origin:
            return window.location.origin;   // Returns base URL (https://example.com)
    }

    return '';
}

function blockUI(elementId, message = '') {

    var html = '<i class="oicon2-lock-4 text-white" role="status"></i>';

    if (message.length > 0)
        html += '<br/><p class="text-white">' + message + '</p>'

    var element = $('#' + elementId);
    element.block({
        message: html,
        //timeout: 1000,
        css: {
            backgroundColor: 'transparent',
            border: '0'
        },
        overlayCSS: {
            opacity: 0.5
        }
    });
}

function unblockUI(elementId) {
    $('#' + elementId).unblock();
}

function addCommas(value) {
    var comma = /,/g;
    value = value.toString().replace(comma, '');

    if ($.isNumeric(value) === false) {
        return '';
    }

    x = value.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    var result = x1 + x2;

    return result;
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function createSelect2() {
    $("select.select-input").select2();

    $("select.tag-input").select2({
        tags: true,
        tokenSeparators: [',']
    });

    //$(".select2").select2("enable", false);
}

function createPersianDatePicker() {
    $(".persian-date-time").pDatepicker({
        timePicker: {
            enabled: true
        },
        toolbox: {
            calendarSwitch: {
                enabled: false
            }
        },
        format: 'YYYY-MM-DD HH:mm',
        initialValueType: 'persian',
    });

    $(".persian-date").pDatepicker({
        initialValueType: 'persian',
        format: 'YYYY-MM-DD',
        toolbox: {
            calendarSwitch: {
                enabled: false
            }
        },
    });

    $(".persian-time").pDatepicker({
        onlyTimePicker: true,
        format: "HH:mm"
    });
}

function createGregorianDatePicker() {
    var basicPickr = $('.flatpickr-basic'),
        timePickr = $('.flatpickr-time'),
        dateTimePickr = $('.flatpickr-date-time');

    // Default
    if (basicPickr.length) {
        basicPickr.flatpickr();
    }

    // Time
    if (timePickr.length) {
        timePickr.flatpickr({
            enableTime: true,
            noCalendar: true
        });
    }

    // Date & TIme
    if (dateTimePickr.length) {
        dateTimePickr.flatpickr({
            enableTime: true
        });
    }
}

function createSpectrum() {
    $(".color-select-picker").spectrum({
        type: "component",
        showPaletteOnly: true,
        togglePaletteOnly: true,
        showAlpha: false
    });
}

function createFeatherIcon() {
    feather.replace();
}

function createInputNumber() {

    var counterMin = 0,
        counterMax = 9999999999;

    $('.touchspin').TouchSpin({
        min: counterMin,
        max: counterMax,
        mousewheel: true,
        buttondown_class: 'btn btn-primary',
        buttonup_class: 'btn btn-primary',
        buttondown_txt: feather.icons['minus'].toSvg(),
        buttonup_txt: feather.icons['plus'].toSvg()
    });

    $('.touchspin-color').each(function (index) {
        var down = 'btn btn-primary',
            up = 'btn btn-primary',
            $this = $(this);

        if ($this.data('bts-button-down-class')) {
            down = $this.data('bts-button-down-class');
        }

        if ($this.data('bts-button-up-class')) {
            up = $this.data('bts-button-up-class');
        }

        $this.TouchSpin({
            min: counterMin,
            max: counterMax,
            mousewheel: true,
            buttondown_class: down,
            buttonup_class: up,
            buttondown_txt: feather.icons['minus'].toSvg(),
            buttonup_txt: feather.icons['plus'].toSvg()
        });
    });
}

function createInputMask() {
    var creditCard = $('.credit-card-mask'),
        phoneMask = $('.phone-number-mask'),
        dateMask = $('.date-mask'),
        timeMask = $('.time-mask'),
        numeralMask = $('.numeral-mask'),
        blockMask = $('.block-mask'),
        delimiterMask = $('.delimiter-mask'),
        customDelimiter = $('.custom-delimiter-mask'),
        prefixMask = $('.prefix-mask');

    // Credit Card
    if (creditCard.length) {
        creditCard.each(function () {
            new Cleave($(this), {
                creditCard: true
            });
        });
    }

    // Phone Number
    if (phoneMask.length) {
        new Cleave(phoneMask, {
            phone: true,
            phoneRegionCode: 'US'
        });
    }

    // Date
    if (dateMask.length) {
        new Cleave(dateMask, {
            date: true,
            delimiter: '-',
            datePattern: ['Y', 'm', 'd']
        });
    }

    // Time
    if (timeMask.length) {
        new Cleave(timeMask, {
            time: true,
            timePattern: ['h', 'm', 's']
        });
    }

    //Numeral
    if (numeralMask.length) {
        new Cleave(numeralMask, {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand'
        });
    }

    //Block
    if (blockMask.length) {
        new Cleave(blockMask, {
            blocks: [4, 3, 3],
            uppercase: true
        });
    }

    // Delimiter
    if (delimiterMask.length) {
        new Cleave(delimiterMask, {
            delimiter: '·',
            blocks: [3, 3, 3],
            uppercase: true
        });
    }

    // Custom Delimiter
    if (customDelimiter.length) {
        new Cleave(customDelimiter, {
            delimiters: ['.', '.', '-'],
            blocks: [3, 3, 3, 2],
            uppercase: true
        });
    }

    // Prefix
    if (prefixMask.length) {
        new Cleave(prefixMask, {
            prefix: '+63',
            blocks: [3, 3, 3, 4],
            uppercase: true
        });
    }
}

function createTooltip() {
    $('svg[data-bs-toggle="tooltip"]').each(function () { new bootstrap.Tooltip(this) });
}

function createDragAndDrop() {
    var els = document.getElementsByClassName('dragable-list');

    Array.prototype.forEach.call(els, function (el) {
        dragula([el])
        .on('drag', function (el) {
            if (typeof callBackAfterDrag === "function") {
                callBackAfterDrag(el);
            }
        }).on('drop', function (el) {
            if (typeof callBackAfterDrop === "function") {
                callBackAfterDrop(el);
            }
        }).on('over', function (el, container) {
            if (typeof callBackAfterOver === "function") {
                callBackAfterOver(el);
            }
        }).on('out', function (el, container) {
            if (typeof callBackAfterOut === "function") {
                callBackAfterOut(el);
            }
        });
    });
}

var dpz;

function createUploader(path) {

    if (dpz !== undefined) {
        dpz.destroy();
    }

    Dropzone.autoDiscover = false;

    var url = getRoute() + '?handler=DropzoneUploader';
    var type = 'replace'; //'dont', 'add'

    dpz = new Dropzone("div#file-uploader-content", {
        url: url + '&path=' + path + "&type=" + type + '&isCompress=0',
        maxFilesize: 0,
        timeout: 0,
        addRemoveLinks: true,
        success: function (file, response) {
            if (response.succeeded) {
                showNotification('بارگذاری فایل', response.message, NotificationType.Success);

                if (typeof callBackAfterUpload === "function") {
                    callBackAfterUpload(response);
                }
            }
            else {
                showNotification('بارگذاری فایل', response.message, NotificationType.Error);
            }
        }
    });

    dpz.on("sending", function (file, xhr, formData) {

        var lastOrder = function () {
            if (typeof getCountItems === "function") {
                return getCountItems();
            }
            else return 0;
        }

        formData.append("lastOrder", lastOrder());

    });
}

function showAlert(title, message) {
    Swal.fire({
        title: '<strong>' + title + '</strong>',
        icon: 'info',
        html: message,
        showCloseButton: true,
        //showCancelButton: true,
        focusConfirm: false,
        confirmButtonText: feather.icons['thumbs-up'].toSvg({ class: 'font-medium-1 me-50' }) + 'باشه',
        //confirmButtonAriaLabel: 'Thumbs up, great!',
        //cancelButtonText: feather.icons['thumbs-down'].toSvg({ class: 'font-medium-1' }) + '',
        //cancelButtonAriaLabel: 'Thumbs down',
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-outline-danger ms-1'
        },
        buttonsStyling: false
    });
}

const NotificationType = {
    Success: "success",
    Error: "error",
    Info: "info",
    Warning:"warning"
}

function showNotification(title, message, NotificationType) {
    toastr[NotificationType](message, title, {
        closeButton: true,
        tapToDismiss: false,
        rtl: isRtl
    });
}

const AjaxType = {
    Post: "POST",
    Get: "GET"
}

function callAjax(handler, AjaxType, data, callback, dataType) {
    //dataType = dataType || 'json';
    if (dataType === undefined)
        dataType = 'json';

    const done = jQuery.Deferred();

    var url;
    if (handler.indexOf('?handler=') !== -1) {
        url = handler;
    }
    else
    { 
        url = getRoute() + '?handler=' + handler;
    }

    $.ajax({
        url: url,
        type: AjaxType,
        dataType: dataType,
        headers: { "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
        data: data,
        success: function (response) {
            if (callback !== undefined && callback !== null) {
                callback(response);
            }

            done.resolve(response);
        },
        error: function (xhr, textStatus, errorThrown) {
            showNotification('Error', xhr.message, NotificationType.Error);
            done.reject(null);
        }
    });

    return done.promise();
}

var action = '';
var viewType = '';
var fieldsContent = '';

const viewShowType = {
    InPage: "in-page",
    Modal: "modal"
}

function manageForm() {
    if (viewType === viewShowType.InPage) {
        $('form').removeData("validator");
        $.validator.unobtrusive.parse('form');
        $(".edit-container-page").removeClass("hidden");
        $(".edit-container-page").removeClass("modal modal-slide-in fade");
        $(".show-container").addClass("hidden");
        $("input:text").first().focus();
        $(window).scrollTop(0);
    }
    else {
        $(".edit-container-modal").addClass("modal modal-slide-in fade");
        $('.edit-container-modal').modal('show');
        $.validator.unobtrusive.parse('form');
        $("input:text").first().focus();
        $('.edit-container-modal').on('shown.bs.modal', function () {
            $('.first-element').first().focus();//set focus on the first element
            $('form').removeData("validator");
            $.validator.unobtrusive.parse('form');
        });
    }

    createSelect2();

    createSpectrum();

    createPersianDatePicker();

    createGregorianDatePicker();

    createFeatherIcon();

    createInputNumber();

    createInputMask();

    createDragAndDrop();
}

function showData(response) {
    if (viewType === viewShowType.InPage) {
        $('.edit-container-page').html(response);
    }
    else {
        $('.edit-container-modal').html(response);
    }

    manageForm();
}

function callbackNew(response) {
    $('#form-header-text').html('( رکورد جدید )');

    showData(response);
}

function callbackEdit(response) {
    $('#password_input').hide();
    $('#confirmpassword_input').hide();
    $('#form-header-text').html('( ویرایش اطلاعات )');

    showData(response);
}

function callbackSave(response) {
    var title = '';

    if (action === '') {
        title = 'Save';
    }

    if (action === 'edit') {
        title = 'Edit';
    }

    if (action === 'new') {
        title = 'Create';
    }

    if (response.succeeded) {
        if (action !== '') {

            if (dt !== undefined)
                dt.ajax.reload(null, false);

            if (viewType === viewShowType.InPage) {
                $(".edit-container-page").addClass("hidden");
                $(".show-container").removeClass("hidden");
            } else {
                $('.edit-container-modal').modal('hide');
                $('.edit-container-modal').html('');
            }
        }
        else if (response.url !== undefined && response.url !== '' && response.url !== 'undefined') {
            window.location.href = response.url;
        }

        showNotification(title, response.message, NotificationType.Success);
    }
    else {
        showNotification(title, response.message + '\n' + response.errors.join(" | "), NotificationType.Error);
    }
}

function callbackDelete(response) {
    if (response.succeeded) {

        $('.delete-container').modal('hide');

        if (dt !== undefined)
            dt.ajax.reload(null, false);

        showNotification('Delete', response.message, NotificationType.Success);
    }
    else {
        showNotification('Delete', response.errors, NotificationType.Error);
    }
}

function callbackUndelete(response) {
    if (response.succeeded) {

        $('.undelete-container').modal('hide');

        if (dt !== undefined)
            dt.ajax.reload(null, false);

        showNotification('Data recovery', response.message, NotificationType.Success);
    }
    else {
        showNotification('Data recovery', response.errors, NotificationType.Error);
    }
}

function callbackDetail(response) {
    if (response.succeeded === undefined || response.succeeded === false) {
        showNotification("Error", response.errors, NotificationType.Error);
        return false;
    }

    if (viewType === viewShowType.InPage) {
        $('.edit-container-page').html(response);
    }
    else {
        $('.edit-container-modal').html(response);
    }

    $('#form-header-text').html('<i class="icon-eye"></i>  Show Info ');

    if (viewType === viewShowType.InPage) {
        $(".edit-container-page").removeClass("hidden");
        $(".show-container").addClass("hidden");
    }
    else {
        $('.edit-container-modal').modal('show');
    }

    createSelect2();

    createPersianDatePicker();

    createGregorianDatePicker();

    createInputMask();

    $('.edit-container-modal').on('shown.bs.modal', function () {
        var disabledelement = $('.disabled-element'); //elements to disable
        $.each(disabledelement, function () {
            $(this).attr("readonly", "true");
        });
    });
}

function callbackFieldRow(response) {
    $("#" + fieldsContent).append(response);

    $("#" + fieldsContent + "> div").last().slideDown("", function () {
        $(this).find("select.select-input").select2();

        $(this).find("select.tag-input").select2({
            tags: true,
            tokenSeparators: [',']
        });

        createSpectrum();

        createPersianDatePicker();

        createGregorianDatePicker();

        createFeatherIcon();

        createInputNumber();

        createInputMask();

        createDragAndDrop();
    });
}

function callbackRecovery(response) {
    if (viewType === viewShowType.InPage) {
        $('.edit-container-page').html(response);
    }
    else {
        $('.edit-container-modal').html(response);
    }

    manageForm();
}

function callbackView(response) {
    if (viewType === viewShowType.InPage) {
        $('.edit-container-page').html(response);
    }
    else {
        $('.edit-container-modal').html(response);
    }

    $('#form-header-text').html('( نمایش اطلاعات )');

    manageForm();
}

var dt;

function createDatatable(id, columnsArray, formatfunc, parameters) {
    var detailRows = [];

    var values = '';
    if (parameters !== undefined && parameters.length > 0) {
        var i;
        var param = [];

        for (i = 0; i < parameters.length; ++i) {
            param.push(parameters[i].key + '=' + parameters[i].value);
        }

        values = '?Handler=&' + param.join('&');
    }

    dt = $('#' + id).DataTable({
        destroy: true, // add this line to distory 
        retrieve: true,
        dom: '<"d-flex justify-content-between align-items-center mx-0 row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>t<"d-flex justify-content-between mx-0 row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
        language: { "url": "/Content/FileData/PersianDT.json" },
        ajax: {
            type: 'POST',
            url: getRoute() + values,
            data: function (d) {
                return $.extend({}, d, {
                    "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val(),
                });
            },
            dataSrc: "data"
        },
        buttons: [],
        columns: columnsArray,
        autoWidth: false,
        order: [[1, "desc"]],
        processing: true,
        serverSide: true,
        orderMulti: false,
        createdRow: function (row, data, dataIndex) {
            if (data.deleted === true) {
                $(row).addClass('remove');
            }
        },
        searchDelay: 1500,
        pagingType: 'full_numbers'
    });

    dt.columns(2).search('FILTER-0');

    dt.on('draw', function () {
        $.each(detailRows, function (i, id) {
            $('#' + id + ' td.details-control').trigger('click');
        });
    });

    $('.searchString').on('input', function () {
        var colid = $(this).attr('data-col-no');
        dt.column(colid).search($(this).val().trim());
    });

    $(".searchString").keypress(function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            var colid = $(this).attr('data-col-no');
            dt.column(colid).search($(this).val().trim()).draw();
        }
    });

    $(document.body).on('change', ".filterString", function (event) {
        var colid = $(this).attr('data-col-no');

        var value = $(this).val().trim();

        if (value !== '') {
            dt.column(colid).search("FILTER-" + value).draw();
        }
        else {
            dt.column(colid).search(value).draw();
        }
    });

    $('#tabledata tbody').on('click', 'tr td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = dt.row(tr);
        var idx = $.inArray(tr.attr('id'), detailRows);

        if (row.child.isShown()) {
            tr.removeClass('details');
            row.child.hide();

            // Remove from the 'open' array
            detailRows.splice(idx, 1);
        }
        else {
            tr.addClass('details');
            row.child(formatfunc(row.data())).show();

            // Add to the 'open' array
            if (idx === -1) {
                detailRows.push(tr.attr('id'));
            }
        }
    });
}

function subset(id, obj) {
    var name = obj.getAttribute('data-name');
    $(".row-parent-category").append('<li class="breadcrumb-item" id="p-' + id + '" onclick="backward(' + id + ',this);"><a href="javascript:void(0)">' + name + '</a></li>');
    $(".new-btn").attr('data-pid', id);
    dt.column(0).search(id);
    dt.draw();
}

function backward(idRow, obj) {
    var id = obj.getAttribute('id');
    var str = "#" + id;
    var item = $(str);
    var parent = item.parent();
    var index = parent.children().index(item);
    var str2 = "";

    for (i = index + 1; i < parent.children().length; i++) {
        if (str2 !== "") {
            str2 = str2 + "," + "#" + parent.children()[i].getAttribute('id');
        }
        else {
            str2 = "#" + parent.children()[i].getAttribute('id');
        }
    }

    if (str2 !== "") {
        $(".breadcrumb-item").remove(str2);
    }

    $(".new-btn").attr('data-pid', idRow);
    dt.column(0).search(idRow);
    dt.draw();
}

$(function () {
    $(document).on('click', '.new-btn', function (e) {
        e.preventDefault();

        var currentParentId = getUrlVars()['pid'];

        if (currentParentId === undefined) {
            currentParentId = 0;
            var pid = $(this).attr('data-pid'); // the id that's given to each button in the list
            if (pid !== undefined) {
                currentParentId = pid;
            }
        }

        //var currentSelectValue = getUrlVars()['selectName'];
        //if (currentSelectValue === undefined) {
        //    currentSelectValue = "";
        //    var ptype = $(this).attr('data-selectName');
        //    if ($("#" + ptype) != undefined) {
        //        currentSelectValue = $("#" + ptype).val();
        //        $("." + ptype + "-container").modal('hide');
        //    }
        //}

        var data = { id: 0, parentId: null };

        if (currentParentId.length > 0)
            data.parentId = currentParentId;

        action = 'new';
        viewType = $(this).attr('data-type');

        var route = 'CreateUpdate';

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        callAjax(route, AjaxType.Get, data, callbackNew, 'html').then(function (result) {
            if (typeof callBackAfterNew === "function") {
                callBackAfterNew(result);
            }
        });
    });

    $(document).on('click', '.edit-btn', function (e) {
        e.preventDefault();

        action = 'edit';
        viewType = $(this).attr('data-type');

        var id = $(this).attr('data-id');

        var route = 'CreateUpdate';

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        callAjax(route, AjaxType.Get, { id }, callbackEdit, 'html').then(function (result) {
            if (typeof callBackAfterEdit === "function") {
                callBackAfterEdit(result);
            }
        });
    });

    $(document).on('click', '.save-btn', function (e) {
        e.preventDefault();
        
        $('.save-btn').addClass('disabled');

        var currentParentId = getUrlVars()['pid'];

        if (currentParentId === undefined) {
            currentParentId = '';
        }

        if ($(".content-base-editor")[0] !== undefined) {
            $(".content-base-editor").val(editor.root.innerHTML);
            $(".content-base-editor").html(editor.root.innerHTML);
        }

        var formdata;

        if (viewType === viewShowType.InPage) {
            formdata = $('#edit-form-page').serialize();
        }
        else {
            formdata = $('#edit-form-modal').serialize();
        }

        if (typeof addDataForSave === "function") {
            formdata = addDataForSave(formdata);
        }

        var route = 'Save';

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        //if ($('#edit-form').valid()) {
            callAjax(route, AjaxType.Post, formdata, callbackSave).then(function (result) {
                if (typeof callBackAfterSave === "function") {
                    callBackAfterSave(result);
                }
            });
        //}

        $('.save-btn').removeClass('disabled');
    });

    $(document).on('click', '.delete-btn-show', function (e) {
        e.preventDefault();
        $("#hidden-id-delete").val($(this).attr('data-id')); // the id that's given to each button in the list
        $("#delete-modal-object").text($(this).attr('data-name'));
        $("#hidden-route-delete").val($(this).attr('data-route'));
        $("#delete-modal-name").text(' حذف رکورد ');
        $('.delete-container').modal('show');
    });

    $(document).on('click', '.delete-btn', function (e) {
        e.preventDefault();

        $('.delete-btn').addClass('disabled');

        var id = $("#hidden-id-delete").val();

        var type = $(this).data("type");

        var route = 'Delete';

        var newRoute = $("#hidden-route-delete").val();

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        var data = {
            id: id.split(','),
            type: type
        };

        callAjax(route, AjaxType.Post, data, callbackDelete).then(function (result) {
            if (typeof callBackAfterDelete === "function") {
                callBackAfterDelete(result);
            }
        });

        $('.delete-btn').removeClass('disabled');
    });

    $(document).on('click', '.undelete-btn-show', function (e) {
        e.preventDefault();
        $("#hidden-id-undelete").val($(this).attr('data-id')); // the id that's given to each button in the list
        $("#undelete-modal-name").text(' Delete Record ');
        $("#undelete-modal-object").text($(this).attr('data-name'));
        $('.undelete-container').modal('show');
    });

    $(document).on('click', '.undelete-btn', function (e) {
        e.preventDefault();

        $('.undelete-btn').addClass('disabled');

        var id = $("#hidden-id-undelete").val();

        var type = $(this).data("type");

        var route = 'Undelete';

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        callAjax(route, AjaxType.Post, { id, type }, callbackUndelete).then(function (result) {
            if (typeof callBackAfterUndelete === "function") {
                callBackAfterUndelete(result);
            }
        });

        $('.undelete-btn').removeClass('disabled');
    });

    $(document).on('click', '.btn-close-inpage', function (e) {
        e.preventDefault();
        $(".edit-container-page").addClass("hidden");
        $(".detail-container").addClass("hidden");
        $(".show-container").removeClass("hidden");
        refresh = $(this).attr('data-refresh');
        if (refresh !== undefined && type !== "undefined") {
            dt.ajax.reload();
        }
    });

    $(document).on('click', '.detail-btn', function (e) {
        e.preventDefault;

        var id = $(this).attr('data-id'); // the id that's given to each button in the list

        viewType = $(this).attr('data-type');

        callAjax('Detail', AjaxType.Get, {}, callbackDetail);
    });

    $(document).on('click', '.refresh-btn', function (e) {
        e.preventDefault();
        dt.ajax.reload(null, false);
    });

    $(document).on("click", ".internal-new-row-btn", function (e) {
        e.preventDefault();

        fieldsContent = $(this).attr("data-tableName");
        action = $(this).attr("data-action");

        fieldItemCounter = $("#" + fieldsContent + " [data-repeater-item]").length;

        var additionalValue = $(this).attr("data-value");

        callAjax(action, AjaxType.Get, { rowNo: fieldItemCounter, additionalValue }, callbackFieldRow, 'html').then(function (result) {
            if (typeof callBackAfterAddRow === "function") {
                callBackAfterAddRow(result);
            }
        });
    });

    $(document).on("click", ".internal-delete-row-btn", function (e) {
        e.preventDefault();
        $(this.parentElement.parentElement.parentElement.parentElement.parentElement).slideUp("", function () {
            $(this).remove();
        })
    });

    $(document).on("keyup", ".price-camma", function (e) {
        e.preventDefault();

        $(this).val(addCommas($(this).val()));
    });

    $(document).on('click', ".copy-value", function (e) {
        e.preventDefault();

        var value = $(this).attr('data-value');

        var tempInput = document.createElement('input');
        tempInput.value = value;
        document.body.appendChild(tempInput);
        tempInput.select();
        document.execCommand('copy');
        document.body.removeChild(tempInput);
        showNotification('Clipboars', 'در کلیپ بورد کپی شد!', NotificationType.Success);
    });

    $(document).on("click", ".download-btn", function (e) {
        e.preventDefault();

        var fileName = $(this).attr('file-name');

        var route = 'Download'

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        var api = getRoute() + '?handler=' + route;

        callAjax('DownloadView', AjaxType.Get, {}, null, 'html').then(function (response) {
            Swal.fire({
                title: 'تعیین تنظیمات',
                html: response,
                inputAttributes: {
                    accept: "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                },
                customClass: {
                    confirmButton: 'btn btn-primary',
                    cancelButton: 'btn btn-outline-danger ms-1'
                },
                buttonsStyling: false,
                showCancelButton: true,
                confirmButtonText: 'دریافت',
                cancelButtonText: 'انصراف',
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    $(".swal2-file").change(function () {
                        var reader = new FileReader();
                        reader.readAsDataURL(this.files[0]);
                    });
                }
            }).then((data) => {

                var formData;
                if (typeof getDataForDownload === "function") {
                    formData = getDataForDownload();
                }               

                $.ajax({
                    method: 'POST',
                    url: api,
                    data: formData,
                    dataSrc: "data",
                    xhrFields: {
                        responseType: 'blob'
                    },
                    success: function (data) {
                        var a = document.createElement('a');
                        var url = window.URL.createObjectURL(data);
                        a.href = url;
                        a.download = fileName;
                        document.body.append(a);
                        a.click();
                        a.remove();
                        window.URL.revokeObjectURL(url);
                    },
                    error: function (response) {
                        showNotification('Error', xhr.message, NotificationType.Error);
                    }
                });
            });

            createSelect2();
        })
    });

    $(document).on("click", ".import-btn", function (e) {
        e.preventDefault();

        var route = 'Import'

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        var api = getRoute() + '?handler=' + route;

        callAjax('ImportView', AjaxType.Get, {}, null, 'html').then(function (response) {
            Swal.fire({
                title: 'انتخاب فایل اکسل',
                //input: 'file',
                html: response,
                inputAttributes: {
                    //name: "upload[]",
                    //id: "fileToUpload",
                    accept: "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    //multiple: "multiple"
                },
                customClass: {
                    confirmButton: 'btn btn-primary',
                    cancelButton: 'btn btn-outline-danger ms-1'
                },
                buttonsStyling: false,
                showCancelButton: true,
                confirmButtonText: 'ارسال',
                cancelButtonText: 'انصراف',
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    $(".swal2-file").change(function () {
                        var reader = new FileReader();
                        reader.readAsDataURL(this.files[0]);
                    });
                }
            }).then((data) => {


                var formData;
                if (typeof getDataOfImport === "function") {
                    formData = getDataOfImport();
                }

                //callAjax(route, AjaxType.Post, formData, null, '').then(function (response) {
                //    if (typeof callBackAfterImport === "function") {
                //        callBackAfterImport(response);
                //    }
                //});
                $.ajax({
                    type: "POST",
                    url: api,
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        dt.ajax.reload(null, false);

                        if (typeof callBackAfterImport === "function") {
                            callBackAfterImport(response);
                        }
                    },
                    error: function (xhr, status, error) {
                        showNotification('Error', xhr.message, NotificationType.Error);
                    }
                });
            });

            createSelect2();
        })
    });

    $(document).on("click", ".export-btn", function (e) {
        e.preventDefault();

        var route = 'Export'

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        //var data = function (d) {
        //    return $.extend({}, d, {
        //        "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val(),
        //    });
        //};

        var api = getRoute() + '?handler=' + route;

        var fileName = $(this).attr('file-name');

        $.ajax({
            method: 'POST',
            url: api,
            data: $.param(dt.ajax.params()),
            dataSrc: "data",
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response) {
                if (response.succeeded !== undefined && !response.succeeded) {
                    showNotification('Error', response.message, NotificationType.Error);
                }
                else
                {
                    var a = document.createElement('a');
                    var url = window.URL.createObjectURL(response);
                    a.href = url;
                    a.download = fileName;
                    document.body.append(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);
                }
            },
            error: function (request, status, error) {
                showNotification('Error', request.responseText, NotificationType.Error);
            }
        });
    });

    $(document).on("click", ".print-btn", function (e) {
        e.preventDefault();

        var id = $(this).attr('data-id');

        if (id === 0) {
            showNotification('چاپ', 'اطلاعات ابتدا باید ثبت شود', NotificationType.Error);

            return;
        }

        var handler = 'Print';

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            handler = newRoute;

        var url = getRoute() + '?handler=' + handler + '&id=' + id;

        Object.assign(document.createElement("a"), {
            target: "_blank",
            href: url
        }).click();
    });

    $(document).on("click", ".recovery-btn", function (e) {
        e.preventDefault();

        var schema = $(this).attr('data-schema');
        var tableName = $(this).attr('data-tablename');

        window.location.href = `/Admin/Recovery?schema=${schema}&tableName=${tableName}`;
    });

    $(document).on('click', '.restore-btn', function (e) {
        e.preventDefault();

        var id = $(this).attr('data-id');
        var route = 'RecoveryItem';

        var data = { id };

        callAjax(route, AjaxType.Post, data).then(function (response) {
            if (response.succeeded) {

                if (dt !== undefined)
                    dt.ajax.reload(null, false);

                showNotification('Recovery', response.message, NotificationType.Success);
            }
            else {
                showNotification('Recovery', response.message + '\n' + response.errors.join(" | "), NotificationType.Error);
            }
        });
    });

    $(document).on('click', '.view-btn', function (e) {
        e.preventDefault();

        action = 'view';
        viewType = $(this).attr('data-type');

        var id = $(this).attr('data-id');

        var route = 'View';

        var newRoute = $(this).attr('data-route');

        if (newRoute !== undefined && newRoute !== '')
            route = newRoute;

        callAjax(route, AjaxType.Get, { id }, callbackView, 'html').then(function (result) {
            if (typeof callBackAfterView === "function") {
                callBackAfterView(result);
            }
        });
    });

    //active menu selected
    $('[href="' + getRoute() + '"]').parent().addClass("active");
});