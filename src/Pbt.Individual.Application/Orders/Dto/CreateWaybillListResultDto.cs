using System.Collections.Generic;

namespace Pbt.Individual.Orders.Dto
{
  public class CreateWaybillListResultDto
  {
    public List<long> CreatedOrderIds { get; set; }
    public List<string> FailedWaybillCodes { get; set; }

    public int TotalWaybills => CreatedOrderIds.Count + FailedWaybillCodes.Count;
    public int TotalCreatedSuccess { get; set; }
    public int TotalFailed => FailedWaybillCodes.Count;
  }
}
