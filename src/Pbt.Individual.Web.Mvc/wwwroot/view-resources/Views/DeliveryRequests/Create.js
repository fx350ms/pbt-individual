(function ($) {
    const _deliveryRequestServices = abp.services.app.deliveryRequest,
        l = abp.localization.getSource('pbt'),
        _$form = $("#form-dr");

    var _$bagPackageTable = $('#bag-package-listTable'),
        _$drItemTable = $('#deliveryRequestItemTable');

    $('#select-warehouse').on('change', function () {
        var warehouseId = $('#select-warehouse').val();
        _deliveryRequestServices.create({ warehouseId: warehouseId }).done(function (res) {
            debugger;
            $('#hiddenDeliveryRequestId').val(res.id);
            $('#deliveryRequestNumber').text(('#' +res.requestCode));
        });
        bagPackageTable.ajax.reload();
        deliveryRequestItemTable.ajax.reload();
    });


    var bagPackageTable = _$bagPackageTable.DataTable({
        paging: false,
        serverSide: true,
        processing: false,
        deferLoading: 0,
        listAction: {
            ajaxFunction: _deliveryRequestServices.getDeliveryRequestItemsForCreateRequest,
            inputFilter: function () {
                var input = {
                    warehouseId: $('#select-warehouse').val(),
                };
                return input;
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: function () { bagPackageTable.draw(false); }
            }
        ],

        language: {
            "info": l('Display') + " _START_ - _END_ | " + l('Total') + " _TOTAL_ " + l('Record'),
            "lengthMenu": l('Display') + " _MENU_ " + l('Record'),
            "emptyTable": l('EmptyTable'),
            "zeroRecords": l('zeroRecords')
        },

        columnDefs: [
            {
                targets: 0,
                data: 'bagNumber',
                className: 'dt-control',
                render: function (data, type, row, meta) {
                    if (row && row.bagNumber) {
                        return `<a href="javascript:void(0)"  > ${row.bagNumber} </a>`;
                    }
                    else
                        return '';
                }
            },
            {
                targets: 1,
                data: 'packageCode',

            },
            {
                targets: 2,
                data: 'waybillNumber',
            },

            {
                targets: 3,
                data: 'weight',
                render: function (data) {
                    return data ? `${data.toFixed(2)} kg` : '-';
                }
            },

            {
                targets: 4,
                data: 'importDate',
                render: function (data) {
                    return data ? moment(data).format('DD/MM/YYYY HH:mm') : '-';
                }
            },
            {
                targets: 5,
                data: 'id',
                width: 20,
                render: function (data, type, row, meta) {
                    return `<a href="javascript:void(0)" data-id="${row.id}" data-type="${row.itemType}" class="btn btn-sm btn-info btn-add-item-2-delivery-request" title="Thêm vào phiếu yêu cầu giao"> <i class="fas fa-caret-right"></i> </a>`;
                }
            }
        ]
    });

    var deliveryRequestItemTable = _$drItemTable.DataTable({
        paging: false,
        serverSide: true,
        processing: false,
        deferLoading: 0,
        listAction: {
            ajaxFunction: _deliveryRequestServices.getDeliveryRequestItemsByRequestId,
            inputFilter: function () {
                var drId = $('#hiddenDeliveryRequestId').val();
                return drId;
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: function () { deliveryRequestItemTable.draw(false); }
            }
        ],

        language: {
            "info": l('Display') + " _START_ - _END_ | " + l('Total') + " _TOTAL_ " + l('Record'),
            "lengthMenu": l('Display') + " _MENU_ " + l('Record'),
            "emptyTable": l('EmptyTable'),
            "zeroRecords": l('zeroRecords')
        },

        columnDefs: [

            {
                targets: 0,
                data: 'itemId',
                width: 20,
                render: function (data, type, row, meta) {
                    return `<a href="javascript:void(0)" data-id="${row.id}" data-type="${row.itemType}" class="btn btn-sm btn-info btn-remove-item" title="Thêm vào phiếu yêu cầu giao"> <i class="fas fa-caret-left"></i> </a>`;
                }
            },
            {
                targets: 1,
                data: 'bagNumber',
                className: 'dt-control',
                render: function (data, type, row, meta) {
                    if (row && row.bagNumber) {
                        return `<a href="javascript:void(0)"  > ${row.bagNumber} </a>`;
                    }
                    else
                        return '';
                }
            },
            {
                targets: 2,
                data: 'packageCode'

            },
            {
                targets: 3,
                data: 'weight',
                render: function (data) {
                    return data ? `${data.toFixed(2)} kg` : '-';
                }
            },
            {
                targets: 4,
                data: 'totalPackages',
                render: function (data, type, row, meta) {
                    if (row.itemType == 1) return 1;
                    return row.totalPackages;
                }
            }
        ]
    });

    function getRow(d) {
        var bagId = d.id;
        var rowContent = '';
        $.ajax({
            url: '/DeliveryRequest/GetPackagesByBagId?bagId=' + bagId,
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

    bagPackageTable.on('click', 'tbody td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = bagPackageTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(getRow(row.data())).show();
        }
    });

    deliveryRequestItemTable.on('click', 'tbody td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = bagPackageTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(getRow(row.data())).show();
        }
    });


    $(document).on('click', '.btn-add-item-2-delivery-request', function () {
        var itemId = $(this).data('id');
        var itemType = $(this).data('type');
        var deliveryRequestId = $('#hiddenDeliveryRequestId').val();

        abp.ui.setBusy(_$drItemTable);
        var item = {
            itemId: itemId,
            itemType: itemType,
            deliveryRequestId: deliveryRequestId
        };
        _deliveryRequestServices.addItemToDeliveryRequest(item).done(function () {
            deliveryRequestItemTable.ajax.reload();
            bagPackageTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$drItemTable);
        });
    });

    $(document).on('click', '.btn-remove-item', function () {
        var deliveryRequestItemId = $(this).data('id');

        abp.ui.setBusy(_$drItemTable);
        _deliveryRequestServices.removeItemFromDeliveryRequest(deliveryRequestItemId).done(function () {
            deliveryRequestItemTable.ajax.reload();
            bagPackageTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$drItemTable);
        });
    });



    $('.btn-submit').on('click', function () {
        const data = _$form.serializeFormToObject();
        data.id = $('#hiddenDeliveryRequestId').val();
        // Gọi API submitDeliveryRequest
        abp.ui.setBusy(); // Hiển thị trạng thái đang xử lý
        _deliveryRequestServices.submitDeliveryRequest(data)
            .done(function (result) {
                if (result.success) {
                    abp.message.success('Yêu cầu giao hàng đã được gửi thành công.');
                    window.location.href = '/DeliveryRequest'; // Chuyển hướng sau khi thành công
                } else {
                    abp.message.error(result.message || 'Có lỗi xảy ra khi gửi yêu cầu giao hàng.');
                }
            })
            .fail(function (error) {
                abp.message.error('Có lỗi xảy ra khi gửi yêu cầu giao hàng. Vui lòng thử lại.');
            })
            .always(function () {
                abp.ui.clearBusy(); // Ẩn trạng thái đang xử lý
            });
    });
})(jQuery);
