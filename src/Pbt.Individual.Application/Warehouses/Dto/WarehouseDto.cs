using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pbt.Individual.Warehouses.Dto
{
    public class WarehouseDto : EntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class WarehouseNameAndCodeDto : EntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
