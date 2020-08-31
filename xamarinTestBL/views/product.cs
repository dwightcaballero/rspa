﻿using System;
using System.Collections.Generic;
using SQLite;

namespace xamarinTestBL
{
    public partial class views
    {
        public class product
        {
            [PrimaryKey, NotNull] public Guid id { get; set; }
            [NotNull] public string productCode { get; set; }
            [NotNull] public string productName { get; set; }
            public string productBrand { get; set; }
            public string productVariation { get; set; }
            public string productStore { get; set; }
            [NotNull] public decimal productPrice { get; set; }

            public decimal productPack_Initial { get; set; }
            public decimal productPiece_Initial { get; set; }
            [NotNull] public decimal productPrice_Initial { get; set; }

            public string productImage { get; set; }
            public Guid categoryUID { get; set; }
            [NotNull] public int updateType { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime editedDate { get; set; }

            [Ignore] public decimal productPrice_10 => Math.Ceiling(productPrice + (productPrice * Convert.ToDecimal(.10)));
            [Ignore] public decimal productPrice_15 => Math.Ceiling(productPrice + (productPrice * Convert.ToDecimal(.15)));

            public static product getProductByID(Guid productUID)
            {
                return dataservices.product.getProductByID(productUID);
            }

            public static List<product> getListProductsForListView()
            {
                return dataservices.product.getListProductsForListView();
            }
        }
    }
}
