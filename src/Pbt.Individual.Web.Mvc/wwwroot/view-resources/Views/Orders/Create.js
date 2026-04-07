(function ($) {
    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('PbtIndividual'),
        _$modal = $('#CustomerAddressCreateModal'),
        _$waybillModal = $('#WaybillCreateModal'),
        _$waybillCreateForm = _$waybillModal.find('form');

    _$waybillCreateForm.on('submit', (e) => {
        e.preventDefault();
        var form = $(e.currentTarget);
        if (!form.valid()) {
            return;
        }
        var dto = form.serializeFormToObject();
        abp.ui.setBusy(_$waybillModal);
        _orderService.CreateWaybills(dto).done(() => {
            _$waybillModal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            _$waybillCreateForm[0].reset();
        }).always(() => {
            abp.ui.clearBusy(_$waybillModal);
        });
    });

})(jQuery);