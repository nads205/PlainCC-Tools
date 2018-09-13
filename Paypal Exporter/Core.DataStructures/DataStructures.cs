using System;
using FileHelpers;

namespace Examples.Core.DataStructures
{
    public class Name
    {
        public string FirstName { get; set ; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string TownorCity { get; set; }
        public string StateorCounty { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
    }

    public class EbayItem
    {
        public string Decription { get; set; }
        public PCCsize Size { get; set; }
        public string Colour { get; set; }
        public PCCitemcode Item { get; set;}
        public string SKU { get; set; }
    }
    public enum PCCitemcode { MHPA, MHZA, LJPA, KJPA, KHPA, KHZA, NONE };
    public enum PCCsize { XXS,XS,S,M,L,XL,XXL };
    public enum ColourCode { black = 006, red = 007, charcoal = 020, navyblue = 014, navy = 014, lightgrey = 011, royalblue = 003, green = 022, turquoiseblue = 002, turquiseblue = 002, orange = 009, babypink = 015, skyblue = 005, purple = 023, beige = 016 };

    public abstract class iFileHelpersClass
    {
        //can't remember why this abstract class is needed - I think It might be related to the FileHelpers library
    }

    //------------------------
    //   RECORD CLASS (Example, change at your will)
    //   TIP: Remember to use the wizard to generate this class
    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    public class PaypalRecord : iFileHelpersClass
    {
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        public DateTime OrderDate;
        [FieldConverter(ConverterKind.Date, "HH:mm:ss")]
        public DateTime OrderTime;
        public string TimeZone;
        public string FullName;
        public string PaypalType;
        public string Status;
        public string Currency;
        public double Gross;
        [FieldNullValue(typeof(double), "0")]
        public double Fee;
        [FieldNullValue(typeof(double), "0")]
        public double Net;
        public string FromEmailAddress;
        public string ToEmailAddress;
        public string TransactionID;
        public string CounterpartyStatus;
        public string AddressStatus;
        [FieldQuoted()]
        public string ItemTitle;
        [FieldQuoted()]
        public string ItemID; //item ID comes with a comma so can't be a straight double
        [FieldNullValue(typeof(double), "0")]
        public double PostageandPackagingAmount;
        [FieldNullValue(typeof(double), "0")]
        public double InsuranceAmount;
        [FieldNullValue(typeof(double), "0")]
        public double VAT;
        public string Option1Name;
        public string Option1Value;
        public string Option2Name;
        public string Option2Value;
        public string AuctionSite;
        public string BuyerID;
        [FieldQuoted()]
        public string ItemURL;
        [FieldNullValue(typeof(DateTime), "01/01/1970")]
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        public DateTime ClosingDate;
        [FieldNullValue(typeof(string), "")]
        public string EscrowID;
        [FieldNullValue(typeof(string), "")]
        public string InvoiceID;
        [FieldNullValue(typeof(string), "")]
        public string ReferenceTxnID;
        [FieldNullValue(typeof(string), "")]
        public string InvoiceNumber;
        [FieldNullValue(typeof(string), "")]
        public string CustomNumber;
        [FieldNullValue(typeof(string), "")]
        public string Quantity;
        [FieldNullValue(typeof(string), "")]
        public string ReceiptID;
        [FieldNullValue(typeof(string), "")]
        public string Balance;
        [FieldQuoted]
        public string AddressLine1;
        [FieldQuoted]
        public string AddressLine2;
        [FieldQuoted]
        public string TownorCity;
        [FieldQuoted]
        public string StateorCounty;
        [FieldQuoted]
        public string Postcode;
        [FieldQuoted]
        public string Country;
        [FieldNullValue(typeof(string),"")]
        public string ContactPhoneNumber;
    }
    /// <summary>
    /// Used for stray "..." in the fee field in the Paypal file
    /// </summary>
    public class FeeDoubleConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            try
            {
                return Convert.ToDouble(from); 
            }
            catch
            {
                return 0.00;
            } 
        }
    }

    public class StripMultipleLines : ConverterBase
    {
        public override object StringToField(string from)
        {
            if (from.Contains("\r\n"))
            {
                var s = from.Replace("\r\n", "");
                return s;
            }
            return from;
        }
    }

    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    public class PaypalRecordOldType : iFileHelpersClass
    {
        [FieldQuoted]
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        public DateTime OrderDate;
        [FieldQuoted]
        [FieldConverter(ConverterKind.Date, "HH:mm:ss")]
        public DateTime OrderTime;
        [FieldQuoted]
        public string TimeZone;
        [FieldQuoted]
        public string FullName;
        [FieldQuoted]
        public string PaypalType;
        [FieldQuoted]
        public string Status;
        [FieldQuoted]
        public string Currency;
        [FieldQuoted]
        public double Gross;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        [FieldConverter (typeof(FeeDoubleConverter))]
        public double Fee;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double Net;
        [FieldQuoted]
        public string FromEmailAddress;
        [FieldQuoted]
        public string ToEmailAddress;
        [FieldQuoted]
        public string TransactionID;
        [FieldQuoted]
        public string CounterpartyStatus;
        [FieldQuoted]
        public string AddressStatus;
        [FieldQuoted]
        public string ItemTitle;
        [FieldQuoted]
        public string ItemID; //item ID comes with a comma so can't be a straight double
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double PostageandPackagingAmount;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double InsuranceAmount;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double VAT;
        [FieldQuoted]
        public string Option1Name;
        [FieldQuoted]
        public string Option1Value;
        [FieldQuoted]
        public string Option2Name;
        [FieldQuoted]
        public string Option2Value;
        [FieldQuoted]
        public string AuctionSite;
        [FieldQuoted]
        public string BuyerID;
        [FieldQuoted()]
        public string ItemURL;
        [FieldQuoted]
        [FieldNullValue(typeof(DateTime), "01/01/1970")]
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        public DateTime ClosingDate;
        [FieldQuoted]
        public string ReferenceTxnID;
        [FieldQuoted]
        public string InvoiceNumber;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double CustomNumber;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Quantity;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string ReceiptID;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Balance;
        [FieldQuoted]
        public string AddressLine1;
        [FieldQuoted]
        public string AddressLine2;
        [FieldQuoted]
        public string TownorCity;
        [FieldQuoted]
        public string StateorCounty;
        [FieldQuoted]
        public string Postcode;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Country;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string ContactPhoneNumber;
    }

    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    public class PaypalRecordBusinessAccount : iFileHelpersClass
    {
        [FieldQuoted]
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        public DateTime OrderDate;
        [FieldQuoted]
        [FieldConverter(ConverterKind.Date, "HH:mm:ss")]
        public DateTime OrderTime;
        [FieldQuoted]
        public string TimeZone;
        [FieldQuoted]
        public string FullName;
        [FieldQuoted]
        public string PaypalType;
        [FieldQuoted]
        public string Status;
        [FieldQuoted]
        public string Currency;
        [FieldQuoted]
        public double Gross;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        [FieldConverter(typeof(FeeDoubleConverter))]
        public double Fee;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double Net;
        [FieldQuoted]
        public string FromEmailAddress;
        [FieldQuoted]
        public string ToEmailAddress;
        [FieldQuoted]
        public string TransactionID;
        [FieldQuoted]
        public string CounterpartyStatus;
        [FieldQuoted]
        public string AddressStatus;
        [FieldQuoted]
        public string ItemTitle;
        [FieldQuoted]
        public string ItemID; //item ID comes with a comma so can't be a straight double
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double PostageandPackagingAmount;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double InsuranceAmount;
        [FieldQuoted]
        [FieldNullValue(typeof(double), "0")]
        public double VAT;
        [FieldQuoted]
        public string Option1Name;
        [FieldQuoted]
        public string Option1Value;
        [FieldQuoted]
        public string Option2Name;
        [FieldQuoted]
        public string Option2Value;
        [FieldQuoted]
        public string ReferenceTxnID;
        [FieldQuoted]
        public string InvoiceNumber;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string CustomNumber;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Quantity;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string ReceiptID;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Balance;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Country;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Subject;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string CountryCode;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string Note;
        [FieldQuoted]
        [FieldNullValue(typeof(string), "")]
        public string BalanceImpact;
    }

    public interface iMagentoRecord
    {
    }

    [DelimitedRecord(",")]
    public class MagentoRecord : iMagentoRecord
    {
        public string state = "complete";
        public string status = "complete";
        public string coupon_code = "";
        public string protect_code = "34b33d";
        public string shipping_description;
        public double is_virtual = 0;
        public double store_id = 1;
        public string customer_id = "";
        public double base_discount_amount = 0;
        public string base_discount_canceled = "";
        public double base_discount_invoiced = 0;
        public string base_discount_refunded = "0";
        public double base_grand_total;
        public double base_shipping_amount;
        public string base_shipping_canceled = "";
        public double base_shipping_invoiced;
        public string base_shipping_refunded = "";
        public double base_shipping_tax_amount = 0;
        public string base_shipping_tax_refunded = "0";
        public double base_subtotal;
        public double base_subtotal_canceled;
        public double base_subtotal_invoiced;
        public string base_subtotal_refunded = "";
        public double base_tax_amount = 0;
        public string base_tax_canceled = "";
        public double base_tax_invoiced = 0;
        public string base_tax_refunded = "";
        public double base_to_global_rate = 1;
        public double base_to_order_rate = 1;
        public string base_total_canceled = "";
        public double base_total_invoiced;
        public double base_total_invoiced_cost = 0;
        public double base_total_offline_refunded = 0; //sticking to double now...
        public double base_total_online_refunded = 0;
        public double base_total_paid;
        public double base_total_qty_ordered = 0;
        public double base_total_refunded = 0;
        public double discount_amount = 0;
        public double discount_canceled = 0;
        public double discount_invoiced = 0;
        public double discount_refunded = 0;
        public double grand_total;
        public double shipping_amount;
        public double shipping_canceled = 0;
        public double shipping_invoiced;
        public double shipping_refunded = 0;
        public double shipping_tax_amount = 0;
        public double shipping_tax_refunded = 0;
        public double store_to_base_rate = 1;
        public double store_to_order_rate = 1;
        public double subtotal;
        public double subtotal_canceled = 0;
        public double subtotal_invoiced;
        public double subtotal_refunded = 0;
        public double tax_amount = 0;
        public double tax_canceled = 0;
        public double tax_invoiced = 0;
        public double tax_refunded = 0;
        public double total_canceled = 0;
        public double total_invoiced;
        public double total_offline_refunded = 0;
        public double total_online_refunded = 0;
        public double total_paid;
        public double total_qty_ordered;
        public double total_refunded = 0;
        public string can_ship_partially = "";
        public string can_ship_partially_item = "";
        public double customer_is_guest = 1;
        public double customer_note_notify = 0;
        public int billing_address_id;
        public int customer_group_id = 0;
        public double edit_increment = 0;
        public double email_sent = 0;
        public double forced_shipment_with_invoice = 0;
        public double payment_auth_expiration = 0;
        public string quote_address_id = "";
        public int shipping_address_id;
        public double adjustment_negative = 0;
        public double adjustment_positive = 0;
        public double base_adjustment_negative = 0;
        public double base_adjustment_positive = 0;
        public double base_shipping_discount_amount = 0;
        public double base_subtotal_incl_tax;
        public double base_total_due = 0;
        public double payment_authorization_amount = 0;
        public double shipping_discount_amount = 0;
        public double subtotal_incl_tax;
        public double total_due = 0;
        public int weight = 0;
        public string customer_dob = "";
        public int increment_id;
        public double applied_rule_ids = 0;
        public string base_currency_code = "GBP";
        public string customer_email;
        public string customer_firstname = "";
        public string customer_lastname = "";
        public string customer_middlename = "";
        public string customer_prefix = "";
        public string customer_suffix = "";
        public double customer_taxvat = 0;
        public string discount_description = "";
        public string ext_customer_id = "";
        public string ext_order_id = "";
        public string global_currency_code = "GBP";
        public string hold_before_state = "";
        public string hold_before_status = "";
        public string order_currency_code = "GBP";
        public string original_increment_id = "";
        public string relation_child_id = "";
        public string relation_child_real_id = "";
        public string relation_parent_id = "";
        public string relation_parent_real_id = "";
        public string remote_ip = "";
        public string shipping_method = "m2eproshipping_m2eproshipping";
        public string store_currency_code = "GBP";
        public string store_name = "\"Main Website\nMain Website Store\nDefault Store View\"";
        public string x_forwarded_for = "";
        public string customer_note = "";
        public string created_at;
        public string updated_at = System.DateTime.Now.ToString();
        public int total_item_count;
        public string customer_gender = "";
        public double hidden_tax_amount = 0;
        public double base_hidden_tax_amount = 0;
        public double shipping_hidden_tax_amount = 0;
        public double base_shipping_hidden_tax_amnt = 0;
        public double hidden_tax_invoiced = 0;
        public double base_hidden_tax_invoiced = 0;
        [FieldNullValue(typeof(double), "0")]
        public double hidden_tax_refunded;
        [FieldNullValue(typeof(double), "0")]
        public double base_hidden_tax_refunded;
        public double shipping_incl_tax;
        public double base_shipping_incl_tax;
        public string coupon_rule_name = "";
        public int paypal_ipn_customer_notified = 0;
        public string gift_message_id = "";
        public string payment_authorization_expiration = "";
        public string forced_do_shipment_with_invoice = "";
        public double base_shipping_hidden_tax_amount = 0;
        public string products;
        public string customer;
        public string shipping_address;
        public string billing_address;
        public string payment;
        public string invoices;
        public string creditmemo;
        public string shipments;
        public string comments;
    }

    [DelimitedRecord(",")]
    public class MagentoRecordSB : iMagentoRecord
    {
        public int orderid; //eg 100000001
        public string website = "admin";
        public string group_id = "Default";
        public string prefix;
        public string firstname;
        public string middlename;
        public string lastname;
        public string suffix;
        public string password = "changeme";
        public string billing_prefix;
        public string billing_firstname;
        public string billing_middlename;
        public string billing_lastname;
        [FieldQuoted]
        public string billing_suffix;
        [FieldQuoted]
        public string billing_street_full;
        [FieldQuoted]
        public string billing_street_2;
        [FieldQuoted]
        public string billing_city;
        [FieldQuoted]
        public string billing_region;
        [FieldQuoted]
        public string billing_country;
        [FieldQuoted]
        public string billing_postcode;
        [FieldQuoted]
        public string billing_telephone;
        [FieldQuoted]
        public string billing_company;
        public string billing_fax;
        public string shipping_prefix;
        public string shipping_firstname;
        public string shipping_middlename;
        public string shipping_lastname;
        public string shipping_suffix;
        [FieldQuoted]
        public string shipping_street_full;
        [FieldQuoted]
        public string shipping_street_2;
        [FieldQuoted]
        public string shipping_city;
        [FieldQuoted]
        public string shipping_region;
        [FieldQuoted]
        public string shipping_country;
        [FieldQuoted]
        public string shipping_postcode;
        [FieldQuoted]
        public string shipping_telephone;
        public string shipping_company;
        public string shipping_fax;
        public string email = "";
        public string customers_id = "";
        public string created_in = "admin";   
        public string is_subscribed = "0";
        public string created_at;
        public string updated_at = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        public double tax_amount;
        public string shipping_method = "m2eproshipping_m2eproshipping";
        public double shipping_amount;
        public double discount_amount;
        public double subtotal;
        public double grand_total;
        public double total_paid;
        public double total_refunded;
        public string total_qty_ordered;
        public double total_canceled;
        public double total_invoiced;
        public double total_online_refunded;
        public double total_offline_refunded;
        public double base_tax_amount;
        public double base_shipping_amount;
        public double base_discount_amount;
        public double base_subtotal;
        public double base_grand_total;
        public double base_total_paid;
        public double base_total_refunded;
        public double base_total_qty_ordered;
        public double base_total_canceled;
        public double base_total_invoiced;
        public double base_total_online_refunded;
        public double base_total_offline_refunded;
        public double subtotal_refunded;
        public double subtotal_canceled;
        public double discount_refunded;
        public double discount_invoiced;
        public double tax_refunded, tax_canceled, shipping_refunded, shipping_canceled, base_subtotal_refunded, base_subtotal_canceled, base_discount_refunded, base_discount_canceled;
        public double base_discount_invoiced;
        public double base_tax_refunded, base_tax_canceled, base_shipping_refunded, base_shipping_canceled;
        public double subtotal_invoiced;
        public double tax_invoiced;
        public double shipping_invoiced;
        public double base_subtotal_invoiced;
        public double base_tax_invoiced;
        public double base_shipping_invoiced;
        public double shipping_tax_amount;
        public double base_shipping_tax_amount;
        public double shipping_tax_refunded, base_shipping_tax_refunded;
        public double store_id = 1;
        public string payment_method = "m2epropayment";
        [FieldQuoted]
        public string products_ordered = "";
        public string tracking_date = "";
        public string tracking_ship_method = "";
        public string tracking_codes = "";
        public string order_status = "complete";
    }
}
