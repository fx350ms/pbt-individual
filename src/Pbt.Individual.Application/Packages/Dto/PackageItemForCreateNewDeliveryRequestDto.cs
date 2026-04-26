using System;
using Abp.Application.Services.Dto;

namespace Pbt.Individual.Packages.Dto
{
    public class PackageItemForCreateNewDeliveryRequestDto
    {

        public string PackageNumber { get; set; }
        public string BagNumber { get; set; }
        public string WaybillNumber { get; set; }
        public string WarehouseName { get; set; }
    }


    public class PackageSummaryByStatusDto
    {
        public int Total { get; set; }
        public int TotalShipping { get; set; }
        public int TotalInVietNameWarehouse { get; set; }
        public int TotalDeliveryInProgress { get; set; }
    }

    public class PackageSummaryItemByStatusDto
    {
        public int Status { get; set; }

        public int TotalCount { get; set; }
    }
}