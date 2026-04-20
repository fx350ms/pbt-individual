(function ($) {
    var _deliveryRequestService = abp.services.app.deliveryRequest,
        l = abp.localization.getSource('Individual'),
        _$modal = $('#OrderCreateModal'),
        _$form = _$modal.find('form'),
        _$deliveryRequestTableID = $('#DeliveryRequestTable');
       

    var _$deliveryRequestTable = _$deliveryRequestTableID.DataTable({
        paging: true,
        serverSide: true,
        sortable: false,
         listAction: {
             ajaxFunction: _deliveryRequestService.getPaged,
             inputFilter: function () {
                 return $('#OrderSearchForm').serializeFormToObject(true);
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
                 return '';  
                }
            },
             {
                targets: 3,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                 return '';  
                }
            },
             {
                targets: 4,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                 return '';  
                }
            },
             {
                targets: 5,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                 return '';  
                }
            },
             {
                targets: 6,
                sortable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {
                 return '';  
                }
            }
        ]}
    );
});