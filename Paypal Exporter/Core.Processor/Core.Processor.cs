using System;
using System.Text;
using Examples.Core.DataStructures;
using System.Globalization;
using FileHelpers.MasterDetail;

namespace Examples.Core.IO
{
    public static class Process
    {
        public static StringBuilder Summary = new StringBuilder();
        public static class PaypalSelectorClass
        {
            public static RecordAction PaypalSelector(string record)
            {
                if (string.IsNullOrEmpty(record.Split(',')[8].Replace("\"","")))
                    return RecordAction.Detail;
                else
                    return RecordAction.Master;
            }
        }
        public static int totalShipmentsCount = 0;
        public static string CreateShipmentString(MasterDetails masterDetail, int startNumber)
        {
            StringBuilder shipments = new StringBuilder();
            var record = (PaypalRecord)masterDetail.Master;
            var recordDetails = (object[])masterDetail.Details;          
            shipments.Append(@"""a:1:{i:0;a:15:{s:8:""""store_id"""";s:1:""""1"""";s:12:""""total_weight"""";N;s:9:""""total_qty"""";s:{totalnumberofitemscount}:""""{totalnumberofitems}"""";s:10:""""email_sent"""";N;s:8:""""order_id"""";s:{orderidlength}:""""{orderid}"""";s:11:""""customer_id"""";N;s:19:""""shipping_address_id"""";s:{shippingaddressidlength}:""""{shippingaddressid}"""";s:18:""""billing_address_id"""";s:{billingaddressidlength}:""""{billingaddressid}"""";s:15:""""shipment_status"""";N;s:12:""""increment_id"""";s:{incrementidlength}:""""{incrementid}"""";s:10:""""created_at"""";s:{orderdatetimelength}:""""{orderdatetime}"""";s:10:""""updated_at"""";s:19:""""2014-02-27 18:01:35"""";s:8:""""packages"""";N;s:14:""""shipping_label"""";N;s:5:""""items"""";a:{totalitems}:{");
            shipments.Replace("{totalnumberofitems}", recordDetails.Length.ToString("N4"));
            shipments.Replace("{totalnumberofitemscount}", recordDetails.Length.ToString("N4").Length.ToString());
            shipments.Replace("{totalitems}", recordDetails.Length.ToString());
            shipments.Replace("{orderid}", startNumber.ToString());
            shipments.Replace("{orderidlength}", startNumber.ToString().Length.ToString());
            //goes up in increments of 2, starting at 1
            shipments.Replace("{billingaddressid}", startNumber.ToString());
            shipments.Replace("{billingaddressidlength}", startNumber.ToString().Length.ToString());
            //goes up in increments of 2, starting at 2
            shipments.Replace("{shippingaddressid}", (startNumber + 1).ToString());
            shipments.Replace("{shippingaddressidlength}", (startNumber + 1).ToString().Length.ToString());
            var incrementid = startNumber + 100000000;
            shipments.Replace("{incrementid}", incrementid.ToString());
            shipments.Replace("{incrementidlength}", incrementid.ToString().Length.ToString());
            var datetimestring = record.OrderDate.ToString("yyyy-MM-dd") + " " + record.OrderTime.ToString("HH:mm:ss");
            shipments.Replace("{orderdatetime}", datetimestring);
            shipments.Replace("{orderdatetimelength}", datetimestring.Length.ToString());
            shipments.Replace("{grandtotal}", record.Gross.ToString("N4"));
            shipments.Replace("{grandtotallength}", record.Gross.ToString("N4").Length.ToString());
            shipments.Replace("{vat}", "0.0000");
            shipments.Replace("{vatlength}", "6");
            shipments.Replace("{shippingcost}", record.PostageandPackagingAmount.ToString("N4"));
            shipments.Replace("{shippingcostlength}", record.PostageandPackagingAmount.ToString("N4").Length.ToString());
            shipments.Replace("{subtotalincvat}", (record.Gross - record.PostageandPackagingAmount).ToString("N4"));
            shipments.Replace("{subtotalincvatlength}", (record.Gross - record.PostageandPackagingAmount).ToString("N4").Length.ToString());
            shipments.Replace("{subtotalexvat}", (record.Gross - record.PostageandPackagingAmount).ToString("N4"));
            shipments.Replace("{subtotalexvatlength}", (record.Gross - record.PostageandPackagingAmount).ToString("N4").Length.ToString());
            shipments.Replace("{postagecostincvat}", record.PostageandPackagingAmount.ToString("N4"));
            shipments.Replace("{postagecostincvatlength}", record.PostageandPackagingAmount.ToString("N4").Length.ToString());
            const string itemstring = @"i:{itemnumber};a:12:{s:9:""""entity_id"""";s:{totalitemcountlength}:""""{totalitemcount}"""";s:9:""""parent_id"""";s:{parentidlength}:""""{parentid}"""";s:9:""""row_total"""";N;s:5:""""price"""";s:{itemcostlength}:""""{itemcost}"""";s:6:""""weight"""";N;s:3:""""qty"""";s:6:""""1.0000"""";s:10:""""product_id"""";N;s:13:""""order_item_id"""";s:{totalitemcountlength}:""""{totalitemcount}"""";s:15:""""additional_data"""";N;s:11:""""description"""";N;s:4:""""name"""";s:{itemdescriptionlength}:""""{itemdescription}"""";s:3:""""sku"""";s:{SKUlength}:""""{SKU}"""";}";
            for (int x = 0; x < recordDetails.Length; ++x)
            {
                totalShipmentsCount++;
                var recorddetail = (PaypalRecord)recordDetails[x];
                shipments.Append(itemstring);
                shipments.Replace("{itemnumber}", x.ToString());
                shipments.Replace("{totalitemcount}", (totalShipmentsCount).ToString());
                shipments.Replace("{totalitemcountlength}", (totalShipmentsCount).ToString().Length.ToString());
                shipments.Replace("{parentid}", startNumber.ToString());
                shipments.Replace("{parentidlength}", startNumber.ToString().Length.ToString());
                shipments.Replace("{itemcost}", recorddetail.Gross.ToString("N4"));
                shipments.Replace("{itemcostlength}", recorddetail.Gross.ToString("N4").Length.ToString());
                var ebayItem = StringFunctions.ParseEbayItem(recorddetail.ItemTitle);
                shipments.Replace("{SKU}", StringFunctions.GetSKUFromItem(ebayItem));
                shipments.Replace("{SKUlength}", StringFunctions.GetSKUFromItem(ebayItem).ToString().Length.ToString());
                shipments.Replace("{itemdescription}", recorddetail.ItemTitle);
                shipments.Replace("{itemdescriptionlength}", recorddetail.ItemTitle.Length.ToString());
            }
            shipments.Append(@"}}}""");
            totalShipmentsCount = 0; //reset for the unit tests
            return shipments.ToString();
        }
        public static string CreateCommentString()
        {
            StringBuilder commentstring = new StringBuilder();
            commentstring.Append(@"""a:3:{i:0;a:7:{s:20:""""is_customer_notified"""";s:1:""""2"""";s:19:""""is_visible_on_front"""";s:1:""""0"""";s:7:""""comment"""";N;s:6:""""status"""";s:8:""""complete"""";s:10:""""created_at"""";s:19:""""2014-02-27 18:01:03"""";s:11:""""entity_name"""";s:8:""""shipment"""";s:8:""""store_id"""";s:1:""""1"""";}i:1;a:7:{s:20:""""is_customer_notified"""";s:1:""""2"""";s:19:""""is_visible_on_front"""";s:1:""""0"""";s:7:""""comment"""";N;s:6:""""status"""";s:10:""""processing"""";s:10:""""created_at"""";s:19:""""2014-02-27 18:01:02"""";s:11:""""entity_name"""";s:7:""""invoice"""";s:8:""""store_id"""";s:1:""""1"""";}i:2;a:7:{s:20:""""is_customer_notified"""";s:1:""""2"""";s:19:""""is_visible_on_front"""";s:1:""""0"""";s:7:""""comment"""";N;s:6:""""status"""";s:7:""""pending"""";s:10:""""created_at"""";s:19:""""2014-02-27 18:01:02"""";s:11:""""entity_name"""";s:5:""""order"""";s:8:""""store_id"""";s:1:""""1"""";}}""");
            return commentstring.ToString();
        }
        public static int totalCustomerCount = 0;
        public static int totalItemCount = 1;
        public static string CreateInvoicesString(MasterDetails masterDetail, int startNumber, int billingAddressID, int shippingAddressID)
        {
            
            StringBuilder invoicestring = new StringBuilder();
            var record = (PaypalRecord)masterDetail.Master;
            var recordDetails = (object[]) masterDetail.Details;
            invoicestring.Append(@"""a:1:{i:0;a:44:{s:8:""""store_id"""";s:1:""""1"""";s:16:""""base_grand_total"""";s:{grandtotallength}:""""{grandtotal}"""";s:19:""""shipping_tax_amount"""";s:6:""""0.0000"""";s:10:""""tax_amount"""";s:{vatlength}:""""{vat}"""";s:15:""""base_tax_amount"""";s:{vatlength}:""""{vat}"""";s:19:""""store_to_order_rate"""";s:6:""""1.0000"""";s:24:""""base_shipping_tax_amount"""";s:6:""""0.0000"""";s:20:""""base_discount_amount"""";s:6:""""0.0000"""";s:18:""""base_to_order_rate"""";s:6:""""1.0000"""";s:11:""""grand_total"""";s:{grandtotallength}:""""{grandtotal}"""";s:15:""""shipping_amount"""";s:{shippingcostlength}:""""{shippingcost}"""";s:17:""""subtotal_incl_tax"""";s:{subtotalincvatlength}:""""{subtotalincvat}"""";s:22:""""base_subtotal_incl_tax"""";s:{subtotalincvatlength}:""""{subtotalincvat}"""";s:18:""""store_to_base_rate"""";s:6:""""1.0000"""";s:20:""""base_shipping_amount"""";s:{shippingcostlength}:""""{shippingcost}"""";s:9:""""total_qty"""";s:6:""""1.0000"""";s:19:""""base_to_global_rate"""";s:6:""""1.0000"""";s:8:""""subtotal"""";s:{subtotalexvatlength}:""""{subtotalexvat}"""";s:13:""""base_subtotal"""";s:{subtotalexvatlength}:""""{subtotalexvat}"""";s:15:""""discount_amount"""";s:6:""""0.0000"""";s:18:""""billing_address_id"""";s:{billingaddressidlength}:""""{billingaddressid}"""";s:18:""""is_used_for_refund"""";N;s:8:""""order_id"""";s:{orderidlength}:""""{orderid}"""";s:10:""""email_sent"""";N;s:13:""""can_void_flag"""";s:1:""""0"""";s:5:""""state"""";s:1:""""2"""";s:19:""""shipping_address_id"""";s:{shippingaddressidlength}:""""{shippingaddressid}"""";s:19:""""store_currency_code"""";s:3:""""GBP"""";s:14:""""transaction_id"""";N;s:19:""""order_currency_code"""";s:3:""""GBP"""";s:18:""""base_currency_code"""";s:3:""""GBP"""";s:20:""""global_currency_code"""";s:3:""""GBP"""";s:12:""""increment_id"""";s:{incrementidlength}:""""{incrementid}"""";s:10:""""created_at"""";s:{orderdatetimelength}:""""{orderdatetime}"""";s:10:""""updated_at"""";s:19:""""2014-02-27 18:00:45"""";s:17:""""hidden_tax_amount"""";s:6:""""0.0000"""";s:22:""""base_hidden_tax_amount"""";s:6:""""0.0000"""";s:26:""""shipping_hidden_tax_amount"""";s:6:""""0.0000"""";s:29:""""base_shipping_hidden_tax_amnt"""";N;s:17:""""shipping_incl_tax"""";s:{postagecostincvatlength}:""""{postagecostincvat}"""";s:22:""""base_shipping_incl_tax"""";s:{postagecostincvatlength}:""""{postagecostincvat}"""";s:19:""""base_total_refunded"""";N;s:20:""""discount_description"""";N;s:5:""""items"""";a:{totalnumberofitems}:");
            invoicestring.Replace("{grandtotal}", record.Gross.ToString("N4"));
            invoicestring.Replace("{grandtotallength}", record.Gross.ToString("N4").Length.ToString());
            invoicestring.Replace("{vat}", "0.0000");
            invoicestring.Replace("{vatlength}", "6");
            invoicestring.Replace("{shippingcost}",record.PostageandPackagingAmount.ToString("N4"));
            invoicestring.Replace("{shippingcostlength}", record.PostageandPackagingAmount.ToString("N4").Length.ToString());
            invoicestring.Replace("{subtotalincvat}", (record.Gross - record.PostageandPackagingAmount).ToString("N4"));
            invoicestring.Replace("{subtotalincvatlength}", (record.Gross - record.PostageandPackagingAmount).ToString("N4").Length.ToString());
            invoicestring.Replace("{subtotalexvat}", (record.Gross - record.PostageandPackagingAmount).ToString("N4"));
            invoicestring.Replace("{subtotalexvatlength}", (record.Gross - record.PostageandPackagingAmount).ToString("N4").Length.ToString());
            //goes up in increments of 2, starting at 1
            invoicestring.Replace("{billingaddressid}", billingAddressID.ToString());
            invoicestring.Replace("{billingaddressidlength}", billingAddressID.ToString().Length.ToString());
            invoicestring.Replace("{orderid}", startNumber.ToString());
            invoicestring.Replace("{orderidlength}", startNumber.ToString().Length.ToString());
            //goes up in increments of 2, starting at 2
            invoicestring.Replace("{shippingaddressid}", shippingAddressID.ToString());
            invoicestring.Replace("{shippingaddressidlength}", shippingAddressID.ToString().Length.ToString());
            invoicestring.Replace("{incrementid}", (startNumber + 100000000).ToString());
            invoicestring.Replace("{incrementidlength}", (startNumber + 100000000).ToString().Length.ToString());
            var datetimestring = record.OrderDate.ToString("yyyy-MM-dd") + " " + record.OrderTime.ToLongTimeString();
            invoicestring.Replace("{orderdatetime}", datetimestring);
            invoicestring.Replace("{orderdatetimelength}", datetimestring.Length.ToString());
            invoicestring.Replace("{postagecostincvat}", record.PostageandPackagingAmount.ToString("N4"));
            invoicestring.Replace("{postagecostincvatlength}", record.PostageandPackagingAmount.ToString("N4").Length.ToString());
            invoicestring.Replace("{totalnumberofitems}", recordDetails.Length.ToString());
            invoicestring.Append("{");
            const string itemstring = @"i:{itemnumber};a:34:{s:9:""""entity_id"""";s:{entityIDlength}:""""{entityID}"""";s:9:""""parent_id"""";s:{parentidlength}:""""{parentid}"""";s:10:""""base_price"""";s:{itemcostlength}:""""{itemcost}"""";s:10:""""tax_amount"""";s:6:""""0.0000"""";s:14:""""base_row_total"""";s:{itemcostlength}:""""{itemcost}"""";s:15:""""discount_amount"""";N;s:9:""""row_total"""";s:{itemcostlength}:""""{itemcost}"""";s:20:""""base_discount_amount"""";N;s:14:""""price_incl_tax"""";s:{itemcostlength}:""""{itemcost}"""";s:15:""""base_tax_amount"""";s:6:""""0.0000"""";s:19:""""base_price_incl_tax"""";s:{itemcostlength}:""""{itemcost}"""";s:3:""""qty"""";s:6:""""1.0000"""";s:9:""""base_cost"""";N;s:5:""""price"""";s:{itemcostlength}:""""{itemcost}"""";s:23:""""base_row_total_incl_tax"""";s:{itemcostlength}:""""{itemcost}"""";s:18:""""row_total_incl_tax"""";s:{itemcostlength}:""""{itemcost}"""";s:10:""""product_id"""";s:{productIDlength}:""""{productID}"""";s:13:""""order_item_id"""";s:{orderitemIDlength}:""""{orderitemID}"""";s:15:""""additional_data"""";N;s:11:""""description"""";N;s:3:""""sku"""";s:10:""""{SKU}"""";s:4:""""name"""";s:{itemdescriptionlength}:""""{itemdescription}"""";s:17:""""hidden_tax_amount"""";s:6:""""0.0000"""";s:22:""""base_hidden_tax_amount"""";s:6:""""0.0000"""";s:28:""""base_weee_tax_applied_amount"""";s:6:""""0.0000"""";s:30:""""base_weee_tax_applied_row_amnt"""";s:6:""""0.0000"""";s:32:""""base_weee_tax_applied_row_amount"""";s:6:""""0.0000"""";s:23:""""weee_tax_applied_amount"""";s:6:""""0.0000"""";s:27:""""weee_tax_applied_row_amount"""";s:6:""""0.0000"""";s:16:""""weee_tax_applied"""";s:6:""""a:0:{}"""";s:20:""""weee_tax_disposition"""";s:6:""""0.0000"""";s:24:""""weee_tax_row_disposition"""";s:6:""""0.0000"""";s:25:""""base_weee_tax_disposition"""";s:6:""""0.0000"""";s:29:""""base_weee_tax_row_disposition"""";s:6:""""0.0000"""";}";
            for (int x = 0; x < recordDetails.Length; x++)
            {
                var recorddetail = (PaypalRecord)recordDetails[x];
                invoicestring.Append(itemstring);
                invoicestring.Replace("{entityID}", totalItemCount.ToString()); //this goes up once per product
                invoicestring.Replace("{entityIDlength}", totalItemCount.ToString().Length.ToString());
                invoicestring.Replace("{orderitemID}", totalItemCount.ToString());
                invoicestring.Replace("{orderitemIDlength}", totalItemCount.ToString().Length.ToString());
                invoicestring.Replace("{itemnumber}", x.ToString());
                invoicestring.Replace("{parentid}", startNumber.ToString());
                invoicestring.Replace("{parentidlength}", startNumber.ToString().Length.ToString());
                invoicestring.Replace("{itemcost}", recorddetail.Gross.ToString("N4"));
                invoicestring.Replace("{itemcostlength}", recorddetail.Gross.ToString("N4").Length.ToString());
                var ebayItem = StringFunctions.ParseEbayItem(recorddetail.ItemTitle);
                //invoicestring.Replace("{SKU}", StringFunctions.GetSKUFromItem(ebayItem));
                invoicestring.Replace("{SKU}", "MHZA006OOL");
                invoicestring.Replace("{itemdescription}", recorddetail.ItemTitle);
                invoicestring.Replace("{itemdescriptionlength}",recorddetail.ItemTitle.Length.ToString());
                //this is the Magento ITEM ID from products
                invoicestring.Replace("{productID}", "27");
                invoicestring.Replace("{productIDlength}", "2");
                totalItemCount++;
            }
            invoicestring.Append (@"}}}""");
            return invoicestring.ToString();
        }
        /// <summary>
        /// Creates a product string for Scott Belasovitch's extension
        /// </summary>
        /// <param name="recordMS"></param>
        /// <returns>A string in the format: 
        /// sku:qty:price:product_name
        /// </returns>
        public static string CreateProductsStringSB(MasterDetails recordMS)
        {
            var record = (PaypalRecord)recordMS.Master;
            StringBuilder products = new StringBuilder();
            if (record.PaypalType == "Mobile Payment Received")
            {
                var ebayItem = StringFunctions.ParseEbayItem(record.ItemTitle);
                products.Append(ebayItem.SKU);
                products.Append(":");
                products.Append(record.Quantity);
                products.Append(":");
                products.Append(record.Gross);
                products.Append(":");
                products.Append(record.ItemTitle);
            }
            else
            {
                int counter = 0;
                foreach (PaypalRecord item in recordMS.Details)
                {
                    var ebayItem = StringFunctions.ParseEbayItem(item.ItemTitle);
                    if (ebayItem != null)
                    {
                        products.Append(ebayItem.SKU);
                        products.Append(":");
                        products.Append(item.Quantity);
                        products.Append(":");
                        products.Append(item.Gross);
                        products.Append(":");
                        products.Append(item.ItemTitle);
                    }
                    else
                    {
                        products.Append(item.Gross < 0 ? "Discount" : "SKU");
                        products.Append(":");
                        products.Append(item.Quantity);
                        products.Append(":");
                        products.Append(item.Gross);
                        products.Append(":");
                        products.Append(item.PaypalType);
                    }
                    if ((recordMS.Details.Length > 1) && (counter < recordMS.Details.Length - 1))
                    {
                        products.Append("|");
                    }
                    counter++;
                }
            }
            return products.ToString();
        }

        /// <summary>
        /// Generates a product string for one of the columns in the output file which can be imported into Magento
        /// </summary>
        /// <param name="record">a PaypalRecord</param>
        /// <param name="startNumber"></param>
        /// <returns></returns>
        public static string CreateProductsString(MasterDetails recordMS, int startNumber)
        {
            //generate m.products
            var record = (PaypalRecord)recordMS.Master;
            StringBuilder products = new StringBuilder();
            products.Append("\"a:{num}:{");
            var replaceditemTitles = record.ItemTitle.Replace("],","];");
            var itemTitles = replaceditemTitles.Split(';');
            var itemIDs = record.ItemID.Split(',');
            var itemCount = itemIDs.Length;
            for (int x = 0; x < itemCount; ++x)
            {
                var itemcountlength = x.ToString().Length.ToString();
                var itemdescription = itemTitles[x];
                var itemdescriptionlength = itemdescription.Length.ToString();
                var itemID = System.Convert.ToDouble(itemIDs[x]);
                StringBuilder product = new StringBuilder();
                product.Append(@"i:{x};a:43:{s:9:""""entity_id"""";s:{itemcountlength}:""""{itemcount}"""";s:14:""""parent_item_id"""";N;s:3:""""sku"""";s:10:""""{SKU}"""";s:15:""""product_options"""";a:1:{s:15:""""info_buyRequest"""";a:1:{s:3:""""qty"""";i:1;}}s:6:""""weight"""";N;s:10:""""is_virtual"""";s:1:""""0"""";s:4:""""name"""";s:{itemdescriptionlength}:""""{itemdescription}"""";s:11:""""description"""";N;s:15:""""additional_data"""";s:141:""""a:1:{s:16:""""m2epro_extension"""";a:1:{s:5:""""items"""";a:1:{i:0;a:2:{s:7:""""item_id"""";s:12:""""{itemID}"""";s:14:""""transaction_id"""";s:13:""""1166311965018"""";}}}}"""";s:13:""""free_shipping"""";s:1:""""0"""";s:14:""""is_qty_decimal"""";N;s:11:""""no_discount"""";s:1:""""0"""";s:15:""""qty_backordered"""";N;s:12:""""qty_canceled"""";s:6:""""0.0000"""";s:12:""""qty_invoiced"""";s:6:""""1.0000"""";s:11:""""qty_ordered"""";s:6:""""1.0000"""";s:12:""""qty_refunded"""";s:6:""""0.0000"""";s:11:""""qty_shipped"""";s:6:""""1.0000"""";s:4:""""cost"""";N;s:6:""""status"""";s:7:""""Shipped"""";s:14:""""original_price"""";s:7:""""{price}"""";s:5:""""price"""";s:7:""""{price}"""";s:10:""""base_price"""";s:7:""""{price}"""";s:3:""""qty"""";N;s:19:""""base_original_price"""";s:7:""""{price}"""";s:11:""""tax_percent"""";s:6:""""0.0000"""";s:10:""""tax_amount"""";s:6:""""0.0000"""";s:15:""""base_tax_amount"""";s:6:""""0.0000"""";s:12:""""tax_invoiced"""";s:6:""""0.0000"""";s:17:""""base_tax_invoiced"""";s:6:""""0.0000"""";s:16:""""discount_percent"""";s:6:""""0.0000"""";s:15:""""discount_amount"""";s:6:""""0.0000"""";s:20:""""base_discount_amount"""";s:6:""""0.0000"""";s:22:""""base_discount_invoiced"""";s:6:""""0.0000"""";s:15:""""amount_refunded"""";s:6:""""0.0000"""";s:20:""""base_amount_refunded"""";s:6:""""0.0000"""";s:9:""""row_total"""";s:7:""""{price}"""";s:14:""""base_row_total"""";s:7:""""{price}"""";s:12:""""row_invoiced"""";s:7:""""{price}"""";s:17:""""base_row_invoiced"""";s:7:""""{price}"""";s:10:""""row_weight"""";s:6:""""0.0000"""";s:24:""""base_tax_before_discount"""";N;s:19:""""tax_before_discount"""";N;}");
                product.Replace("{x}", x.ToString());
                product.Replace("{itemcountlength}", itemcountlength);
                product.Replace("{itemcount}", (x + startNumber).ToString());
                var ebayItem = StringFunctions.ParseEbayItem(itemdescription);
                product.Replace("{SKU}", StringFunctions.GetSKUFromItem(ebayItem));
                product.Replace("{itemdescriptionlength}", itemdescriptionlength);
                product.Replace("{itemdescription}", itemdescription);
                product.Replace("{itemID}", itemID.ToString());
                product.Replace("{price}", record.Gross.ToString("N4"));
                products.Append(product);
            }
            products.Replace("{num}", itemCount.ToString());
            products.Append("}\"");
            return products.ToString();
        }

        public static string CreateCreditmemoString()
        {
            const string creditmemo = @"""a:0:{}""";
            return creditmemo;
        }

        public static string CreatePaymentString(PaypalRecord record, int ID)
        {
            StringBuilder payment = new StringBuilder();
            payment.Append(@"""a:1:{i:0;a:54:{s:9:""""parent_id"""";s:{idlength}:""""{id}"""";s:22:""""base_shipping_captured"""";s:{shippingamountlength}:""""{shippingamount}"""";s:17:""""shipping_captured"""";s:{shippingamountlength}:""""{shippingamount}"""";s:15:""""amount_refunded"""";N;s:16:""""base_amount_paid"""";s:{amountpaidlength}:""""{amountpaid}"""";s:15:""""amount_canceled"""";N;s:22:""""base_amount_authorized"""";N;s:23:""""base_amount_paid_online"""";N;s:27:""""base_amount_refunded_online"""";N;s:20:""""base_shipping_amount"""";s:{shippingamountlength}:""""{shippingamount}"""";s:15:""""shipping_amount"""";s:{shippingamountlength}:""""{shippingamount}"""";s:11:""""amount_paid"""";s:{amountpaidlength}:""""{amountpaid}"""";s:17:""""amount_authorized"""";N;s:19:""""base_amount_ordered"""";s:{amountpaidlength}:""""{amountpaid}"""";s:22:""""base_shipping_refunded"""";N;s:17:""""shipping_refunded"""";N;s:20:""""base_amount_refunded"""";N;s:14:""""amount_ordered"""";s:{amountpaidlength}:""""{amountpaid}"""";s:20:""""base_amount_canceled"""";N;s:16:""""quote_payment_id"""";N;s:15:""""additional_data"""";s:360:""""a:5:{s:14:""""component_mode"""";s:4:""""ebay"""";s:14:""""payment_method"""";s:6:""""PayPal"""";s:16:""""channel_order_id"""";s:26:""""261190349760-1369082865016"""";s:17:""""channel_final_fee"""";d:{ebayfinalfee};s:12:""""transactions"""";a:1:{i:0;a:4:{s:14:""""transaction_id"""";s:17:""""{paypaltransactionID}"""";s:3:""""fee"""";d:{paypalfee};s:3:""""sum"""";d:{amountpaidlong};s:16:""""transaction_date"""";s:{transactiondatelength}:""""{transactiondate}"""";}}}"""";s:12:""""cc_exp_month"""";N;s:16:""""cc_ss_start_year"""";N;s:16:""""echeck_bank_name"""";N;s:6:""""method"""";s:13:""""m2epropayment"""";s:21:""""cc_debug_request_body"""";N;s:16:""""cc_secure_verify"""";N;s:22:""""protection_eligibility"""";N;s:11:""""cc_approval"""";N;s:8:""""cc_last4"""";N;s:21:""""cc_status_description"""";N;s:11:""""echeck_type"""";N;s:28:""""cc_debug_response_serialized"""";N;s:17:""""cc_ss_start_month"""";N;s:19:""""echeck_account_type"""";N;s:13:""""last_trans_id"""";s:17:""""{paypaltransactionID}"""";s:13:""""cc_cid_status"""";N;s:8:""""cc_owner"""";N;s:7:""""cc_type"""";N;s:9:""""po_number"""";N;s:11:""""cc_exp_year"""";N;s:9:""""cc_status"""";N;s:21:""""echeck_routing_number"""";N;s:14:""""account_status"""";N;s:17:""""anet_trans_method"""";N;s:22:""""cc_debug_response_body"""";N;s:11:""""cc_ss_issue"""";N;s:19:""""echeck_account_name"""";N;s:13:""""cc_avs_status"""";N;s:13:""""cc_number_enc"""";N;s:11:""""cc_trans_id"""";N;s:21:""""paybox_request_number"""";N;s:14:""""address_status"""";N;s:22:""""additional_information"""";a:0:{}}}""");
            payment.Replace("{id}", ID.ToString());
            payment.Replace("{idlength}", ID.ToString().Length.ToString());
            payment.Replace("{shippingamountlength}",record.PostageandPackagingAmount.ToString("N4").Length.ToString());
            payment.Replace("{shippingamount}",record.PostageandPackagingAmount.ToString("N4"));
            payment.Replace("{amountpaid}",record.Gross.ToString("N4"));
            payment.Replace("{amountpaidlength}", record.Gross.ToString("N4").Length.ToString());
            payment.Replace("{amountpaidlong}", record.Gross.ToString("N15"));
            payment.Replace("{ebayfinalfee}",(record.Gross/10).ToString("N16"));
            payment.Replace("{paypaltransactionID}",record.TransactionID);
            payment.Replace("{paypalfee}", Math.Abs(record.Fee).ToString());
            var datestring = record.OrderDate.ToString("yyyy-MM-dd");
            payment.Replace("{transactiondate}",datestring + " " + record.OrderTime.ToString("HH:mm:ss"));
            payment.Replace("{transactiondatelength}", (datestring + " " + record.OrderTime.ToString("HH:mm:ss")).Length.ToString());
            return payment.ToString();
        }

        public static string CreateShippingString(PaypalRecord record, int ID)
        {
            StringBuilder shipping = new StringBuilder();
            shipping.Append(@"""a:25:{s:9:""""parent_id"""";s:{idlength}:""""{id}"""";s:19:""""customer_address_id"""";N;s:16:""""quote_address_id"""";N;s:9:""""region_id"""";s:1:""""1"""";s:11:""""customer_id"""";N;s:3:""""fax"""";N;s:6:""""region"""";s:{regionlength}:""""{region}"""";s:8:""""postcode"""";s:{postcodelength}:""""{postcode}"""";s:8:""""lastname"""";s:{lastnamelength}:""""{lastname}"""";s:6:""""street"""";s:{streetlength}:""""{street}"""";s:4:""""city"""";s:{citylength}:""""{city}"""";s:5:""""email"""";s:{emaillength}:""""{email}"""";s:9:""""telephone"""";N;s:10:""""country_id"""";s:2:""""{countryID}"""";s:9:""""firstname"""";s:{firstnamelength}:""""{firstname}"""";s:12:""""address_type"""";s:8:""""shipping"""";s:6:""""prefix"""";N;s:10:""""middlename"""";N;s:6:""""suffix"""";N;s:7:""""company"""";N;s:6:""""vat_id"""";N;s:12:""""vat_is_valid"""";N;s:14:""""vat_request_id"""";N;s:16:""""vat_request_date"""";N;s:19:""""vat_request_success"""";N;}""");
            shipping.Replace("{id}", ID.ToString());
            shipping.Replace("{idlength}", ID.ToString().Length.ToString());
            shipping.Replace("{postcode}",record.Postcode);
            shipping.Replace("{postcodelength}", record.Postcode.Length.ToString());
            var name = StringFunctions.ParseName(record.FullName);
            shipping.Replace("{firstname}", name.FirstName);
            shipping.Replace("{firstnamelength}", name.FirstName.Length.ToString());  
            shipping.Replace("{lastname}", name.LastName);
            shipping.Replace("{lastnamelength}", name.LastName.Length.ToString());
            shipping.Replace("{street}", record.AddressLine1);
            shipping.Replace("{streetlength}", record.AddressLine1.Length.ToString());
            shipping.Replace("{region}",record.StateorCounty);
            shipping.Replace("{regionlength}",record.StateorCounty.Length.ToString());
            shipping.Replace("{city}", record.TownorCity);
            shipping.Replace("{citylength}", record.TownorCity.Length.ToString());
            shipping.Replace("{email}", record.FromEmailAddress);
            shipping.Replace("{emaillength}", record.FromEmailAddress.Length.ToString());
            shipping.Replace("{countryID}", GetCountryCode(record.Country));
            return shipping.ToString();
        }
        /// <summary>
        /// Returns two letter ISO RegionName
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static string GetCountryCode(string countryName)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures);//.Select(x => new RegionInfo(x.LCID));
            foreach (CultureInfo cultureinfo in regions)
            {
                if (cultureinfo.EnglishName.Contains(countryName))
                {
                    var regionInfo = new RegionInfo(cultureinfo.LCID);
                    return regionInfo.TwoLetterISORegionName;
                }
                else if (countryName.ToLower() == "cyprus")
                { //cyprus is not in the .Net list of cultures :(!!
                    return "CY";
                }
            }
            throw new CultureNotFoundException("Can't find country code");
        }

        public static string CreateBillingString(PaypalRecord record, int ID)
        {
            StringBuilder shipping = new StringBuilder();
            shipping.Append(@"""a:25:{s:9:""""parent_id"""";s:{idlength}:""""{id}"""";s:19:""""customer_address_id"""";N;s:16:""""quote_address_id"""";N;s:9:""""region_id"""";s:1:""""1"""";s:11:""""customer_id"""";N;s:3:""""fax"""";N;s:6:""""region"""";s:{regionlength}:""""{region}"""";s:8:""""postcode"""";s:{postcodelength}:""""{postcode}"""";s:8:""""lastname"""";s:{lastnamelength}:""""{lastname}"""";s:6:""""street"""";s:{streetlength}:""""{street}"""";s:4:""""city"""";s:{citylength}:""""{city}"""";s:5:""""email"""";s:{emaillength}:""""{email}"""";s:9:""""telephone"""";N;s:10:""""country_id"""";s:2:""""{countryID}"""";s:9:""""firstname"""";s:{firstnamelength}:""""{firstname}"""";s:12:""""address_type"""";s:7:""""billing"""";s:6:""""prefix"""";N;s:10:""""middlename"""";N;s:6:""""suffix"""";N;s:7:""""company"""";N;s:6:""""vat_id"""";N;s:12:""""vat_is_valid"""";N;s:14:""""vat_request_id"""";N;s:16:""""vat_request_date"""";N;s:19:""""vat_request_success"""";N;}""");
            shipping.Replace("{id}", ID.ToString());
            shipping.Replace("{idlength}", ID.ToString().Length.ToString());
            shipping.Replace("{postcode}", record.Postcode);
            shipping.Replace("{postcodelength}", record.Postcode.Length.ToString());
            var name = StringFunctions.ParseName(record.FullName);
            shipping.Replace("{firstname}", name.FirstName);
            shipping.Replace("{firstnamelength}", name.FirstName.Length.ToString());
            shipping.Replace("{lastname}", name.LastName);
            shipping.Replace("{lastnamelength}", name.LastName.Length.ToString());
            shipping.Replace("{street}", record.AddressLine1);
            shipping.Replace("{streetlength}", record.AddressLine1.Length.ToString());
            shipping.Replace("{region}", record.StateorCounty);
            shipping.Replace("{regionlength}", record.StateorCounty.Length.ToString());
            shipping.Replace("{city}", record.TownorCity);
            shipping.Replace("{citylength}", record.TownorCity.Length.ToString());
            shipping.Replace("{email}", record.FromEmailAddress);
            shipping.Replace("{emaillength}", record.FromEmailAddress.Length.ToString());
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures);//.Select(x => new RegionInfo(x.LCID));
            foreach (CultureInfo cultureinfo in regions)
            {
                if (cultureinfo.EnglishName.Contains(record.Country))
                {
                    var regionInfo = new RegionInfo(cultureinfo.LCID);
                    shipping.Replace("{countryID}", regionInfo.TwoLetterISORegionName);
                    break;
                }
            }
            return shipping.ToString();
        }

        public static string CreateCustomerString()
        {
            StringBuilder customer = new StringBuilder();
            customer.Append(@"""a:1:{s:10:""""created_at"""";s:25:""""2014-04-09T16:19:57+01:00"""";}""");
            return customer.ToString();
        }
    }
}
