﻿namespace Smartstore.ShippingByWeight.Models
{
    [LocalizedDisplay("Plugins.Shipping.ByWeight.Fields.")]
    public class ByWeightListModel : ModelBase
    {
        [LocalizedDisplay("*LimitMethodsToCreated")]
        public bool LimitMethodsToCreated { get; set; }

        [LocalizedDisplay("*CalculatePerWeightUnit")]
        public bool CalculatePerWeightUnit { get; set; }

        [LocalizedDisplay("*IncludeWeightOfFreeShippingProducts")]
        public bool IncludeWeightOfFreeShippingProducts { get; set; }


        [LocalizedDisplay("*Zip")]
        public string SearchZip { get; set; }

        [LocalizedDisplay("*Country")]
        public int SearchCountryId { get; set; }

        [LocalizedDisplay("*ShippingMethod")]
        public int SearchShippingMethodId { get; set; }

        [UIHint("Stores")]
        [LocalizedDisplay("Admin.Common.Store.SearchFor")]
        public int SearchStoreId { get; set; }
    }
}
