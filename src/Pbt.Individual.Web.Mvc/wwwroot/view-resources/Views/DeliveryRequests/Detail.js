(function ($) {
    const _deliveryRequestServices = abp.services.app.deliveryRequest,
        l = abp.localization.getSource('pbt'),
        _$form = $("#form-dr");
        _$drItemTable = $('#deliveryRequestItemTable');
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
                    <td colspan="7" style="text-align: left; font-weight: bold;">
                        Kiện: ${totalItems} <span style="color: #007bff;"></span> | 
                        Bao: ${totalBags} <span style="color: #007bff;"></span> | 
                        Cân nặng: <span style="color: #dc3545;">${totalWeight.toFixed(2)} kg</span>
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
                className: 'text-center',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
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
    deliveryRequestItemTable.ajax.reload(function() {
        // Tự động mở tất cả các bao để hiển thị danh sách kiện
        deliveryRequestItemTable.rows().every(function() {
            var row = this;
            var data = row.data();
            if (data && !data.packageCode) { // Nếu là bao (không có packageCode)
                row.child(getRow(data.itemId)).show();
            }
        });
    });
})(jQuery);
