(function ($) {
    const _deliveryRequestServices = abp.services.app.deliveryRequest,
        l = abp.localization.getSource('pbt'),
        _$form = $("#form-dr");
    var _$bagPackageTable = $('#bag-package-listTable'),
        _$drItemTable = $('#deliveryRequestItemTable');

    $('#select-warehouse').on('change', function () {
        var warehouseId = $('#select-warehouse').val();
        _deliveryRequestServices.create({ warehouseId: warehouseId }).done(function (res) {
            $('#hiddenDeliveryRequestId').val(res.id);
            $('#deliveryRequestNumber').text(('#' + res.requestCode));
            setTimeout(() => {
                deliveryRequestItemTable.ajax.reload();
                $('#btn-submit-dr').removeClass('d-none');
            }, 100);
        });
        bagPackageTable.ajax.reload();

    });

    var bagPackageTable = _$bagPackageTable.DataTable({
        paging: false,
        serverSide: true,
        processing: false,
        ordering: false,
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
 
        ],
        dom: 'Brt',
        language: {
            "emptyTable": "Chưa có hàng trong kho chờ giao",
            "zeroRecords": "Chưa có hàng trong kho chờ giao",
        },
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();
            var pageData = api.rows({ search: 'applied' }).data();
            
            var totalItems = 0; // Kiện (itemType = 1)
            var totalBags = 0;  // Bao (itemType = 2)
            var totalWeight = 0;
            
            pageData.each(function (row) {
                if (row.itemType === 1) {
                    totalItems++;
                } else if (row.itemType === 2) {
                    totalBags++;
                }
                totalWeight += parseFloat(row.weight) || 0;
            });
            
            var tfoot = $(_$bagPackageTable).find('tfoot');
            tfoot.html(`
                <tr>
                    <td colspan="7" style="text-align: right; font-weight: bold;">
                        Tổng kiện: <span style="color: #007bff;">${totalItems}</span> | 
                        Tổng bao: <span style="color: #007bff;">${totalBags}</span> | 
                        Tổng cân nặng: <span style="color: #dc3545;">${totalWeight.toFixed(2)} kg</span>
                    </td>
                </tr>
            `);
        },

        columnDefs: [
            {
                targets: 0,
                data: 'bagNumber',
                className: 'dt-control',
                orderable: false,
                width: '12%',
                render: function (data, type, row, meta) {
                    if (row && row.bagNumber) {
                        return `<a class="view-packages-link badge badge-info" href="javascript:void(0)" style="font-size:0.95em;">${row.bagNumber}</a>`;
                    }
                    else
                        return '';
                }
            },
            {
                targets: 1,
                data: 'packageNumber'
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
                data: 'packageNumber',
                render: function (data, type, row, meta) {
                    if (data === null || data === '') {
                        return `<a href="javascript:void(0)" class="view-packages-link badge badge-primary" data-bag-id="${row.id}" title="Nhấn để xem danh sách kiện">Xem kiện</a>`;
                    }
                    return '';
                }
            },
            {
                targets: 6,
                data: 'id',
                width: 20,
                render: function (data, type, row, meta) {
                    return `<a href="javascript:void(0)" data-id="${row.id}" data-type="${row.itemType}" class="btn btn-sm btn-outline-success btn-add-item-2-delivery-request" title="Nhấn chọn để chuyển vào yêu cầu giao">Chọn</a>`;
                }
            },
        ]
    });

    var deliveryRequestItemTable = _$drItemTable.DataTable({
        paging: false,
        serverSide: true,
        processing: false,
        deferLoading: 0,
        ordering: false,
        listAction: {
            ajaxFunction: _deliveryRequestServices.getDeliveryRequestItemsByRequestId,
            inputFilter: function () {
                var drId = $('#hiddenDeliveryRequestId').val();
                return { deliveryRequestId: drId };
            }
        },
        buttons: [

        ],
        dom: 'Brt',
        language: {
            "emptyTable": "Danh sách giao hiện đang trống",
            "zeroRecords": "Danh sách giao hiện đang trống",
        },
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();
            var pageData = api.rows({ search: 'applied' }).data();
            
            var totalItems = 0; // Kiện (itemType = 1)
            var totalBags = 0;  // Bao (itemType = 2)
            var totalWeight = 0;
            
            pageData.each(function (row) {
                if (row.itemType === 1) {
                    totalItems++;
                } else if (row.itemType === 2) {
                    totalBags++;
                }
                totalWeight += parseFloat(row.weight) || 0;
            });
            
            var tfoot = $(_$drItemTable).find('tfoot');
            tfoot.html(`
                <tr>
                    <td colspan="7" style="text-align: right; font-weight: bold;">
                        Tổng kiện: <span style="color: #007bff;">${totalItems}</span> | 
                        Tổng bao: <span style="color: #007bff;">${totalBags}</span> | 
                        Tổng cân nặng: <span style="color: #dc3545;">${totalWeight.toFixed(2)} kg</span>
                    </td>
                </tr>
            `);
        },

        columnDefs: [

            {
                targets: 0,
                data: 'itemId',
                width: 70,
                sortable: false,
                render: function (data, type, row, meta) {
                    return `<a href="javascript:void(0)" data-id="${row.id}" data-type="${row.itemType}" class="btn btn-sm btn-outline-danger btn-remove-item" title="Nhấn để loại bỏ khỏi yêu cầu giao">Bỏ chọn</a>`;
                }
            },
            {
                targets: 1,
                data: 'bagNumber',
                className: 'dt-control',
                render: function (data, type, row, meta) {
                    if (row && row.bagNumber) {
                        return `<a href="javascript:void(0)" class="view-packages-link badge badge-info" style="font-size:0.95em;">${row.bagNumber}</a>`;
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
                data: 'waybillNumber'
            },
            {
                targets: 4,
                data: 'weight',
                render: function (data) {
                    return data ? `${data.toFixed(2)} kg` : '-';
                }
            },
            {
                targets: 5,
                data: 'totalPackages',
                render: function (data, type, row, meta) {
                    if (row.itemType == 1) return 1;
                    return row.totalPackages;
                }
            },
            {
                targets: 6,
                data: 'packageCode',
                render: function (data, type, row, meta) {
                    if (data === null || data === '') {
                        return `<a href="javascript:void(0)" class="view-packages-link badge badge-primary" data-bag-id="${row.id}" title="Nhấn để xem danh sách kiện">Xem kiện</a>`;
                    }
                    return '';
                }
            }
        ]
    });

    function getRow(bagId) {
        var rowContent = '';
        $.ajax({
            url: '/DeliveryRequests/GetPackagesByBagId?bagId=' + bagId,
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

    bagPackageTable.on('click', '.view-packages-link', function (e) {
        let tr = e.target.closest('tr');
        let row = bagPackageTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(getRow(row.data().id)).show();
        }
    });

    deliveryRequestItemTable.on('click', '.view-packages-link', function (e) {
        let tr = e.target.closest('tr');
        let row = deliveryRequestItemTable.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(getRow(row.data().itemId)).show();
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
            // Hide table error after adding item
            $('#deliveryRequestItemTable').removeClass('border-danger');
            $('#deliveryRequestItemTable').prev('.validation-error').remove();
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
        // Validation
        var hasItems = deliveryRequestItemTable.rows().count() > 0;
        var address = $('#txtAddress').val().trim();
        var hasAddress = address !== '';

        // Clear previous errors
        $('#deliveryRequestItemTable').removeClass('border-danger');
        $('#txtAddress').removeClass('is-invalid');
        $('.validation-error').remove();

        var errors = [];

        if (!hasItems) {
            errors.push('Danh sách đã chọn phải có ít nhất một kiện/bao.');
            $('#deliveryRequestItemTable').addClass('border-danger');
            $('#deliveryRequestItemTable').before('<div class="alert alert-danger validation-error">Danh sách đã chọn phải có ít nhất một kiện/bao.</div>');
        }

        if (!hasAddress) {
            errors.push('Địa chỉ giao hàng là bắt buộc.');
            $('#txtAddress').addClass('is-invalid');
            $('#txtAddress').before('<div class="alert alert-danger validation-error">Địa chỉ giao hàng là bắt buộc.</div>');
        }

        if (errors.length > 0) {
            return;
        }

        const data = _$form.serializeFormToObject();
        data.id = $('#hiddenDeliveryRequestId').val();
        // Gọi API submitDeliveryRequest
        abp.ui.setBusy(); // Hiển thị trạng thái đang xử lý
        _deliveryRequestServices.submitDeliveryRequest(data)
            .done(function (result) {
                if (result.success) {
                    abp.message.success('Yêu cầu giao hàng đã được gửi thành công.');
                    window.location.href = '/DeliveryRequests'; // Chuyển hướng sau khi thành công
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

    // Hide address error when typing
    $('#txtAddress').on('input', function () {
        if ($(this).hasClass('is-invalid')) {
            $(this).removeClass('is-invalid');
            $(this).prev('.validation-error').remove();
        }
    });
    $("#select-warehouse").removeAttr("disabled");
})(jQuery);
