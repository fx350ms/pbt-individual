using System.ComponentModel;

public enum WarehouseType
{
    /// <summary>
    /// Kho nguồn tạo (Trung Quốc)
    /// </summary>
    Source = 1,


    /// <summary>
    /// Kho đích nhận (Việt Nam) 
    /// </summary>
    Destination = 2
}

  public enum ShippingLine
  {
      [Description("Hàng lô")]
      Consignment  = 1,
      
      [Description("TMĐT")]
      Ecommerce = 2,
      [Description("Chính ngạch")]
      Official = 3,
      [Description("Xách tay")]
      Portable = 4
  }


  