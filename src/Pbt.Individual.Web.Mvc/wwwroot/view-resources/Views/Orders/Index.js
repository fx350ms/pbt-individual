(function ($) {
    var _orderService = abp.services.app.order,
        _packageService = abp.services.app.package,
        l = abp.localization.getSource('Individual'),
        _$modal = $('#OrderCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#OrdersTable');

    const orderStatusDescriptions = {
        0: { text: 'Thiếu thông tin', color: 'danger' },
        1: { text: 'Đã ký gửi', color: 'info' },
        2: { text: 'Hàng về kho TQ', color: 'success' },
        3: { text: 'Đang VC QT', color: 'info' },
        4: { text: 'Đã đến kho VN', color: 'success' },
        5: { text: 'Đang giao đến khách', color: 'info' },
        6: { text: 'Đã giao', color: 'success' },
        7: { text: 'Khiếu nại', color: 'danger' },
        8: { text: 'Hoàn tiền', color: 'warning' },
        9: { text: 'Huỷ', color: 'danger' },
        10: { text: 'Hoàn thành đơn', color: 'success' }
    };

    const shippingLines = {
        0: { text: '', color: 'blue' },
        1: { text: l('Lô'), color: 'blue' },
        2: { text: l('TMĐT'), color: 'green' },
        3: { text: l('CN'), color: 'yellow' },
        4: { text: l('XT'), color: 'pink' },

    };

    var _$ordersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        sortable: false,
        listAction: {
            ajaxFunction: _orderService.getCustomerOrders,
            inputFilter: function () {
                return $('#OrderSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$ordersTable.draw(false)
            }
        ],
        language: {
            "info": l('Display') + " _START_ - _END_ | " + l('Total') + " _TOTAL_ " + l('Record'),
            "lengthMenu": l('Display') + " _MENU_ " + l('Record'),
            "emptyTable": l('EmptyTable'),
            "zeroRecords": l('ZeroRecords')
        },
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                width: 35,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },

            {
                targets: 2,
                data: 'waybillNumber',
                 width: 180,
                sortable: false,
                /*className: 'dt-control',*/
                render: function (data, type, row, meta) {

                    const status = shippingLines[row.shippingLine];
                    return `<a target="_blank"  href="/Orders/Detail/${row.id}" title="${l('Detail')}">${data}</a> <br/> <strong style="color: ${status ? status.color : 'black'};">${status ? status.text : ''}</strong>`;
                }
            },

            {
                targets: 3,
                width: 80,
                sortable: false,
                data: 'status',
                render: function (data, type, row, meta) {
                    var status = orderStatusDescriptions[row.orderStatus];
                    if (status) {
                        return '<span class="badge badge-' + status.color + '">' + status.text + '</span>';
                    }
                    return '';
                }
            },

            {
                //BagCoverWeight
                targets: 4,
                width: 90,
                sortable: false,
                className: 'text-right',
                data: 'bagCoverWeight',
                render: (data, type, row, meta) => {
                    return formatNumberWithThousandsSeparator(data) | '-';
                }
            },
            {
                //Weight
                targets: 5,
                data: 'weight',
                sortable: false,
                className: 'text-right',
                width: 120,
                render: (data, type, row, meta) => {
                    return formatNumberWithThousandsSeparator(data);
                }
            },
            {
                //Volume
                targets: 6,
                data: 'volume',
                sortable: false,
                className: 'text-right',
                width: 100,
                render: (data, type, row, meta) => {
                    return formatNumberWithThousandsSeparator(data);
                }
            },


            {
                //TotalFee -> phí vận chuyển QT
                targets: 7,
                sortable: false,
                data: 'totalFee',
              
                render: (data, type, row, meta) => {

                    var woodenPackagingFee = row.woodenPackagingFee ? FormatNumberToDisplay(row.woodenPackagingFee, 0) : 0;
                    var shockproofFee = row.shockproofFee ? FormatNumberToDisplay(row.shockproofFee, 0) : 0;
                    var domesticShippingFee = row.domesticShippingFee ? FormatNumberToDisplay(row.domesticShippingFee, 0) : 0;
                    var insuranceFee = row.insuranceFee ? FormatNumberToDisplay(row.insuranceFee, 0) : 0;
                    
                    return [
                       `${l('WoodenPackaging')}: <strong>${woodenPackagingFee}</strong><hr/>`,
                       `${l('Shockproof')}: <strong>${shockproofFee}</strong><hr/>`,
                       `${l('DomesticShipping')}: <strong>${domesticShippingFee}</strong><hr/>`,
                       `${l('Insurance')}: <strong>${insuranceFee}</strong>`
                    ].join('');         
                }
            },


            {
                //TotalCost
                targets: 8,
                sortable: false,
                className: 'text-right',
                data: 'totalPrice',
                width: 100,
                render: (data, type, row, meta) => {
                    // return data ? `<strong class="text-primary">${FormatNumberToDisplay(data, 0)}</strong>` : '-';
                    return formatNumberWithThousandsSeparator(data);

                }
            },


            {
                //NOTE
                targets: 9,
                sortable: false,
                className: 'text-right',
                data: 'Note',
                width: 240,
                render: (data, type, row, meta) => {
                    return data ? `<span class="text-primary">${data}</span>` : '-';
                }
            },
            {
                targets: 10,
                sortable: false,
                width: 200,
                render: (data, type, row) => {

                    // Ngày tạo
                    const creation = formatDateToDDMMYYYYHHmm(row.creationTime) || '';
                    // Ngày xuất kho TQ
                    const exportChina = row.inTransitTime ? formatDateToDDMMYYYYHHmm(row.inTransitTime) : '';
                    // Ngày nhập kho VN
                    const importVN = row.inTransitToVietnamWarehouseTime ? formatDateToDDMMYYYYHHmm(row.inTransitToVietnamWarehouseTime) : '';

                    return [
                        `<div class="timeline-item">
                            <span class="label">${l('CreationTime')}:</span>
                            <span class="value">${creation}</span>
                          </div>`,
                        `<div class="timeline-item">
                            <span class="label">${l('ExportDateCN')}:</span>
                            <span class="value">${exportChina}</span>
                          </div>`,
                        `<div class="timeline-item">
                            <span class="label">${l('ImportDateVN')}:</span>
                            <span class="value">${importVN}</span>
                          </div>`
                    ].join('');
                }
            },

            {
                targets: 11,
                sortable: false,
                width: 20,
                render: (data, type, row, meta) => {
                    const isEditable = row.orderStatus === 1; // Only allow edit/delete if status is 1
                    const canCreatePackage = row.orderStatus === 1 || row.orderStatus === 2; // Allow creating package if status is 1 or 2
                    const canCreateDeliveryRequest = row.orderStatus === 4; // Allow creating delivery request if status is 4
                    const canMakePayment = row.paymentStatus === 1; // Allow making payment if payment status is 'Chờ thanh toán'
                    const canDelete = row.orderStatus === 0; // Only allow delete if status is 0 (Thiếu thông tin)
                    return [
                        ` <div class="btn-group"> `,
                        `   <button type="button" class="btn btn-default dropdown-toggle dropdown-icon" data-toggle="dropdown" aria-expanded="false">`,
                        ` </button>`,
                        ` <div class="dropdown-menu" style="">`,

                        `   <a target="_blank" type="button" class="dropdown-item  bg-primary" data-order-id="${row.id}" href="/Orders/Detail/${row.id}" title="${l('Detail')}" data-toggle="tooltip"> `,
                        `       <i class="fas fa-info-circle"></i> ${l('Detail')}`,
                        '   </a>',
                        `   <a type="button" class="dropdown-item bg-warning view-order-mote" data-order-id="${row.id}" title="${l('Note')}" data-toggle="modal" data-target="#OrderNote" > ` +
                        `       <i class="fas fa-pencil-alt"></i> ${l('Note')}` +
                        `   </a>`,
                        (canDelete ? `   <button type="button" class="dropdown-item bg-danger delete-order" data-order-id="${row.id}" data-order-name="${row.waybillNumber}" title="${l('Delete')}" data-toggle="tooltip">` +
                            `       <i class="fas fa-trash"></i> ${l('Delete')}` +
                            `   </button>` : ''),
                        `    </div>`,
                        `   </div>`
                    ].join('');
                }
            }
        ]
    });


    _$ordersTable.on('click', 'tbody td.dt-control', function (e) {
        return;
        let tr = e.target.closest('tr');
        let row = _$ordersTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(getRow(row.data()), 'child-row').show();

        }
    });

    function getRow(d) {

        // `d` is the original data object for the row

        var orderId = d.id;
        var rowContent = '';
        $.ajax({
            url: '/Orders/GetPackagesByOrder?orderId=' + orderId,
            type: "GET",
            async: false,
            dataType: "html",
            success: function (html) {
                rowContent = html;
            },
            error: function () {

            }
        });

        return rowContent;
    }

    $('.btn-clear-value').on('click', function () {
        var target = $(this).attr('target');
        var targetInput = $(this).attr('target-date');
        if (target) {
            var targetValue = $(this).attr('target-value');
            if (targetValue) {
                $('[name="' + target + '"]').val(targetValue);
            }
            else {
                $('[name="' + target + '"]').val('');
                $('.' + targetInput).val('');
            }
        }
    });


    $('.date-range').daterangepicker(
        {
            "locale": {
                "format": "DD/MM/YYYY",
                "separator": " - ",
                "applyLabel": l('Apply'),
                "cancelLabel": l('Cancel'),
                "fromLabel": l('From'),
                "toLabel": l('To'),
                "customRangeLabel": l('Select'),
                "weekLabel": "W",
            },
            autoUpdateInput: false,
            "cancelClass": "btn-danger"
        }

    ).val('');

    $('.btn-export-order').on('click', function () {
        const filterData = $('#OrderSearchForm').serializeFormToObject();
        filterData.isExcel = true;
        const url = toQueryString(filterData);
        window.location.href = '/Orders/ExportMyOrder?' + url;
        abp.ui.clearBusy();
    });

    $('.btn-export-package').on('click', function () {
        const filterData = $('#OrderSearchForm').serializeFormToObject();
        filterData.isExcel = true;
        const url = toQueryString(filterData);
        window.location.href = '/Orders/DownloadPackage?' + url;
        abp.ui.clearBusy();
    });

    $('.date-range').on('apply.daterangepicker', function (ev, picker) {

        var target = $(this).attr('target');
        $('.start-date.' + target).val(picker.startDate.format('DD/MM/YYYY'));
        $('.end-date.' + target).val(picker.endDate.format('DD/MM/YYYY'));
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
    });

    $('.date-range').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        var target = $(this).attr('target');
        $('.start-date.' + target).val('');
        $('.end-date.' + target).val('');
    });

    $('#input-date-range').on('apply.daterangepicker', function (ev, picker) {
        $('#start-date').val(picker.startDate.format('DD-MM-YYYY'));
        $('#end-date').val(picker.endDate.format('DD-MM-YYYY'));
    });

    _$form.find('.save-button').on('click', (e) => {

        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var order = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _orderService.create(order).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            PlayAudio('success', function () {

            });
            _$ordersTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.btn-sync-order', function () {
        var orderId = $(this).attr("data-id");
        _orderService.syncWeightAndFee(orderId).done(() => {
            abp.notify.info(l('SuccessfullyUpdated'));
            _$ordersTable.ajax.reload();
        });

    });
    $(document).on('click', '.mark-as-completed', function () {
        var orderId = $(this).attr("data-id");
        var orderName = $(this).attr('data-name');
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToChangeOrderToCompleted'),
                orderName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.markAsCompleted(orderId).done(() => {
                        abp.notify.info(l('SuccessfullyUpdated'));
                        _$ordersTable.ajax.reload(null, false);
                    });
                }
            }
        );
    });

    $(document).on('click', '.mark-as-delivered', function () {
        var orderId = $(this).attr("data-id");
        var orderName = $(this).attr('data-name');

        markAsDelivered(orderId, orderName);
    });

    function markAsDelivered(orderId, orderName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToChangeOrderToDelivered'),
                orderName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.markAsDelivered(orderId).done(() => {
                        abp.notify.info(l('SuccessfullyUpdated'));
                        _$ordersTable.ajax.reload(null, false);
                    });
                }
            }
        );
    }

    $(document).on('click', '.delete-order', function () {
        var orderId = $(this).attr("data-order-id");
        var orderName = $(this).attr('data-order-name');

        deleteOrders(orderId, orderName);
    });

    function deleteOrders(orderId, orderName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                orderName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _orderService.delete({
                        id: orderId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$ordersTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.view-package-list', function () {
        var orderId = $(this).data('order-id');

        abp.ajax({
            url: abp.appPath + 'Orders/GetPackageList/' + orderId,
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#PackageListModel div.modal-body').html(content);
                $('#PackageListModel').modal('show');
            },
            error: function (e) {
                abp.notify.error(l('ErrorLoadingPackageList'));
            }
        });
    });


    abp.event.on('order.edited', (data) => {
        _$ordersTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$ordersTable.ajax.reload();
        return false;
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$ordersTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
