//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PolKaramnov
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartnerProducts
    {
        public int IdPartnerProducts { get; set; }
        public int Products { get; set; }
        public int NamePartner { get; set; }
        public int Quantity { get; set; }
        public System.DateTime DateSale { get; set; }
    
        public virtual Partners Partners { get; set; }
        public virtual Products Products1 { get; set; }
    }
}
