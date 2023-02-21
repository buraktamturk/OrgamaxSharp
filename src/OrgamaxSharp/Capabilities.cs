namespace OrgamaxSharp;

[Flags]
public enum Capabilities
{
    HasOpenOrder = 1,
    GetOrders = 2,
    StatusChange = 4,
    GetProducts = 8,
    GetProductListe = 16,
    ImportProducts = 32,
    ImportStockCounts = 64,
    ImportPriceLists = 128,
    GetProductCount = 256,
    All = HasOpenOrder | GetOrders | StatusChange | GetProducts | GetProductListe | ImportProducts | ImportStockCounts | ImportPriceLists | GetProductCount
}