//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LopushokApp.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductBid
    {
        public int ID { get; set; }
        public Nullable<int> ID_Agent { get; set; }
        public Nullable<int> ID_Product { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public int Count_Product { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> Count_Day { get; set; }
        public Nullable<int> Count_Mater { get; set; }
        public Nullable<int> Cost_Price { get; set; }
        public Nullable<bool> Prepayment { get; set; }
        public Nullable<bool> Delivery { get; set; }
        public Nullable<bool> Examination { get; set; }
        public Nullable<bool> Full_Payment { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual Product Product { get; set; }
    }
}
