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
    
    public partial class Supply
    {
        public int ID { get; set; }
        public Nullable<int> ID_Mater { get; set; }
        public Nullable<int> ID_Supp { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
