(function ($) {
    var _deliveryRequestService = abp.services.app.deliveryRequest,
        l = abp.localization.getSource('Individual'),
        _$modal = $('#DeliveryRequestCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#DeliveryRequestsTable'),
        _$pendingTable = $('#PendingTable');

    const deliveryStatusDescriptions = {
        1: { text: 'Yêu cầu mới', class: 'badge badge-warning' },
        2: { text: 'Đang xử lý', class: 'badge badge-info' },
        3: { text: 'Đã xử lý', class: 'badge badge-success' },
        4: { text: 'Đã giao hàng', class: 'badge badge-primary' },
        5: { text: 'Hoàn thành', class: 'badge badge-success' },
        6: { text: 'Hủy', class: 'badge badge-dark' }

    };
    var _$deliveryRequestTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        sortable: false,
        listAction: {

            ajaxFunction: _deliveryRequestService.getPaged,
            inputFilter: function () {
                return $('#DeliveryRequestSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$deliveryRequestTable.draw(false)
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
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    var html = `<a href="/DeliveryRequests/Detail/${row.id}">${row.requestCode}</a>`;
                    return html;
                }
            },
            {
                targets: 3,
                sortable: false,
                className: 'text-center',
                render: (data, type, row, meta) => {
                    const status = deliveryStatusDescriptions[row.status];
                    // Trả về mô tả với màu sắc được áp dụng
                    return [`<span class="m-1 ${status ? status.class : 'badge badge-secondary'}">${status ? status.text : ''}</span> `,

                    ].join('');
                }
            },
            {
                targets: 4,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return row.warehouseName;
                }
            },
            {
                targets: 5,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return row.packageCount;
                }
            },
            {
                targets: 6,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return row.totalWeight;
                }
            },
            {
                targets: 7,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return formatDateToDDMMYYYYHHmmss(row.creationTime) || '';
                }
            }
            ,
            {
                targets: 8,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    // trả về link xem chi tiết
                    var html = `<a href="/DeliveryRequests/Detail/${row.id}"><i class="fas fa-eye"></i></a>`;
                    return html;
                }
            }
        ]
    });

    var _$pendingRequestTable = _$pendingTable.DataTable({
        paging: false,
        serverSide: true,
        sortable: false,
        listAction: {
            ajaxFunction: _deliveryRequestService.getAllPackagesForCreateNewDeliveryRequest
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$pendingRequestTable.draw(false)
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
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return row.packageNumber;
                }
            },

            {
                targets: 3,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return row.waybillNumber;
                }
            }

        ]
    });

})(jQuery);