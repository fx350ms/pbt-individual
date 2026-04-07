using Pbt.Individual.Commons;
using System.Collections.Generic;

namespace Pbt.Individual.Orders.Dto
{
    public class CreateWaybillListResultDto : ExecSqlResultDto
    {
 
        
        public int TotalWaybills { get; set; }  
        public int TotalCreatedSuccess { get; set; }
        public int TotalFailed  { get; set; }
    }
}
