using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers.MasterDetail;
using Examples.Core.DataStructures;
using Examples.Core.IO;
using System.Collections.Specialized;

namespace Examples.Core
{
    public class Generator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaxRecords"></param>
        public List<iMagentoRecord> Generate(NameValueCollection appSettings, MasterDetails[] recordsMS)
        {
            var MaxRecords = Convert.ToInt32(appSettings["NumberOfRecordsToExport"]);
            var startRecordNumber = Convert.ToInt32(appSettings["StartRecordNumber"]);
            var Outputfiletype = appSettings["MagentoType"];
            List<iMagentoRecord> mList = new List<iMagentoRecord>();

            //not all products are PlainCC (eBay/website) products. If they don't have an itemID or a parseable description then discard            
            var plainCCProducts = recordsMS.Where(c => c.Details.OfType<PaypalRecord>().Any(d => d.ItemTitle != "" && d.ItemID != "" && StringFunctions.DetermineItemTypeFromDescription(d.ItemTitle) != PCCitemcode.NONE)).Take(MaxRecords);

                
            if (Outputfiletype == "Magazento")
                {
                    #region Magezento
                    int billingShippingStartNumber = startRecordNumber;
                    var loopcounter = 1;
                    foreach (MasterDetails record in plainCCProducts)
                    {
                        var recordMaster = (PaypalRecord)record.Master;
                        if (recordMaster.ItemID == "") continue;
                        MagentoRecord m = new MagentoRecord();
                        //do processing here
                        m.base_grand_total = recordMaster.Gross;
                        m.base_shipping_amount = recordMaster.PostageandPackagingAmount;
                        m.base_shipping_invoiced = recordMaster.PostageandPackagingAmount;
                        m.shipping_description = recordMaster.Country == appSettings["BaseCountry"] ? appSettings["ShippingDomestic"] : appSettings["ShippingInternational"];
                        m.base_subtotal = recordMaster.Gross - recordMaster.PostageandPackagingAmount;
                        m.base_subtotal_canceled = recordMaster.Gross;
                        m.base_subtotal_invoiced = recordMaster.Gross - recordMaster.PostageandPackagingAmount;
                        m.base_total_invoiced = recordMaster.Gross;
                        m.base_total_paid = recordMaster.Gross;
                        m.grand_total = recordMaster.Gross;
                        m.shipping_amount = recordMaster.PostageandPackagingAmount;
                        m.shipping_invoiced = recordMaster.PostageandPackagingAmount;
                        m.subtotal = recordMaster.Gross - recordMaster.PostageandPackagingAmount;
                        m.subtotal_invoiced = recordMaster.Gross - recordMaster.PostageandPackagingAmount;
                        m.total_invoiced = recordMaster.Gross;
                        m.total_paid = recordMaster.Gross;
                        var itemCount = recordMaster.ItemID.Split(',').Length;
                        m.total_qty_ordered = itemCount;
                        m.billing_address_id = billingShippingStartNumber++;
                        m.shipping_address_id = billingShippingStartNumber++;
                        m.base_subtotal_incl_tax = recordMaster.Gross - recordMaster.PostageandPackagingAmount;
                        m.customer_email = recordMaster.FromEmailAddress;
                        var name = StringFunctions.ParseName(recordMaster.FullName);
                        m.customer_firstname = name.FirstName;
                        m.customer_lastname = name.LastName;
                        m.customer_middlename = name.MiddleName;
                        m.customer_prefix = name.Title;
                        m.created_at = recordMaster.OrderDate.ToShortDateString() + " " + recordMaster.OrderTime.ToShortTimeString();
                        m.total_item_count = itemCount;
                        m.shipping_incl_tax = recordMaster.PostageandPackagingAmount;
                        m.base_shipping_incl_tax = recordMaster.PostageandPackagingAmount;
                        m.products = Process.CreateProductsString(record, 1);
                        m.customer = Process.CreateCustomerString();
                        m.shipping_address = Process.CreateShippingString(recordMaster, startRecordNumber);
                        m.billing_address = Process.CreateBillingString(recordMaster, startRecordNumber);
                        m.payment = Process.CreatePaymentString(recordMaster, startRecordNumber);
                        m.invoices = Process.CreateInvoicesString(record, startRecordNumber, m.billing_address_id, m.shipping_address_id);
                        m.creditmemo = Process.CreateCreditmemoString();
                        m.shipments = Process.CreateShipmentString(record, startRecordNumber);
                        m.comments = Process.CreateCommentString();
                        m.increment_id = startRecordNumber++ + Convert.ToInt32(appSettings["IncrementCounter"]);
                        mList.Add(m);
                        loopcounter++;
                    #endregion
                    }
                }
                else if (Outputfiletype == "ScottBelasovitch")
                {
                    int billingShippingStartNumber = startRecordNumber;
                    var loopcounter = 1;
                    foreach (MasterDetails record in plainCCProducts)
                    {
                        var recordMaster = (PaypalRecord)record.Master;
                        //if (recordMaster.ItemID == "") continue;
                        MagentoRecordSB m = new MagentoRecordSB();
                        //do processing here
                        m.orderid = startRecordNumber++ + Convert.ToInt32(appSettings["IncrementCounter"]);
                        var name = StringFunctions.ParseName(recordMaster.FullName);
                        
                        m.prefix = name.Title; 
                        m.shipping_prefix = name.Title;
                        m.billing_prefix = name.Title;

                        m.firstname = name.FirstName;
                        m.shipping_firstname = name.FirstName;
                        m.billing_firstname = name.FirstName;
                        
                        m.middlename = name.MiddleName;
                        m.shipping_middlename = name.MiddleName;
                        m.billing_middlename = name.MiddleName;

                        m.lastname = name.LastName;
                        m.shipping_lastname = name.LastName;
                        m.billing_lastname = name.LastName;

                        m.billing_street_full = recordMaster.AddressLine1;
                        m.shipping_street_full = recordMaster.AddressLine1;

                        m.billing_street_2 = recordMaster.AddressLine2;
                        m.shipping_street_2 = recordMaster.AddressLine2;

                        m.billing_city = recordMaster.TownorCity;
                        m.shipping_city = recordMaster.TownorCity;

                        m.billing_region = recordMaster.StateorCounty;
                        m.shipping_region = recordMaster.StateorCounty;
                        
                        m.billing_postcode = recordMaster.Postcode;
                        m.shipping_postcode = recordMaster.Postcode;
                        
                        m.billing_country =  Process.GetCountryCode(recordMaster.Country);
                        m.shipping_country = Process.GetCountryCode(recordMaster.Country);

                        m.billing_telephone = recordMaster.ContactPhoneNumber;
                        m.shipping_telephone = recordMaster.ContactPhoneNumber;

                        m.email = recordMaster.FromEmailAddress;

                        m.created_at = recordMaster.OrderDate.ToString("yyyy-MM-dd") + " " + recordMaster.OrderTime.ToShortTimeString();

                        m.tax_amount = Math.Round(recordMaster.Gross / 6, 2, MidpointRounding.AwayFromZero); 
                        m.base_tax_amount = 0;
                        m.tax_invoiced = 0;
                        m.base_tax_invoiced = 0;

                        m.shipping_amount = recordMaster.PostageandPackagingAmount;
                        m.base_shipping_amount = recordMaster.PostageandPackagingAmount;

                        m.discount_amount = 0;
                        m.base_discount_amount = 0;

                        m.subtotal = recordMaster.Gross - recordMaster.PostageandPackagingAmount;
                        m.base_subtotal = recordMaster.Gross - recordMaster.PostageandPackagingAmount;

                        m.grand_total = recordMaster.Gross;
                        m.base_grand_total = recordMaster.Gross;

                        m.total_paid = recordMaster.Gross;
                        m.base_total_paid = recordMaster.Gross;

                        m.total_invoiced = recordMaster.Gross;
                        m.base_total_invoiced = recordMaster.Gross;

                        m.subtotal_invoiced = recordMaster.Net;
                        m.base_subtotal_invoiced = recordMaster.Net;

                        m.shipping_invoiced = recordMaster.PostageandPackagingAmount;
                        m.base_shipping_invoiced = recordMaster.PostageandPackagingAmount;
                        
                        m.shipping_tax_amount = Math.Round(recordMaster.PostageandPackagingAmount / 6, 2, MidpointRounding.AwayFromZero);
                        m.base_shipping_tax_amount = 0;
                        
                        var total_qty_ordered = (
                            from PaypalRecord p in record.Details
                            group p by p.Quantity into g
                            select new { Total = g.Sum(p => System.Convert.ToInt32(p.Quantity)) });
                        try
                        {
                            m.total_qty_ordered = total_qty_ordered.First().Total.ToString();
                        }
                        catch
                        {
                            m.total_qty_ordered = "1";
                        }

                        m.products_ordered = Process.CreateProductsStringSB(record);

                        mList.Add(m);
                        loopcounter++;
                    }
                }
                else
                {
                    Process.Summary.AppendLine("Can't determine Outputfiletype");
                }
                return mList;
        }
    }
}
