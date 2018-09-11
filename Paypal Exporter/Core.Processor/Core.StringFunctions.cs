using System;
using Examples.Core.DataStructures;
using System.Text.RegularExpressions;

namespace Examples.Core.IO
{
    public static class StringFunctions
    {
        /// <summary>
        /// Makes the first letter of the word uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
        public static Address ParseAddress(string fulladdress)
        {
            var addresslines = fulladdress.Split(',');
            Address address = new Address();
            address.AddressLine1 = "";
            address.AddressLine2 = "";
            address.TownorCity = "";
            address.StateorCounty = "";
            address.Postcode = "";
            address.Country = "";
            switch (addresslines.Length)
            {
                case 3:
                    address.AddressLine1 = addresslines[1].Trim();
                    address.Country = addresslines[2].Trim();
                    break;
                case 4:
                    address.AddressLine1 = addresslines[1].Trim();
                    address.TownorCity = addresslines[2].Trim();
                    address.Country = addresslines[3].Trim();
                    break;
                case 5:
                    address.AddressLine1 = addresslines[1].Trim();
                    address.TownorCity = addresslines[2].Trim();
                    address.StateorCounty = addresslines[3].Trim();
                    address.Country = addresslines[4].Trim();
                    break;
                case 6:
                    address.AddressLine1 = addresslines[1].Trim();
                    address.TownorCity = addresslines[2].Trim();
                    address.StateorCounty = addresslines[3].Trim();
                    address.Postcode = addresslines[4].Trim();
                    address.Country = addresslines[5].Trim();
                    break;
                case 7:
                    address.AddressLine1 = addresslines[1].Trim();
                    address.AddressLine2 = addresslines[2].Trim();
                    address.TownorCity = addresslines[3].Trim();
                    address.StateorCounty = addresslines[4].Trim();
                    address.Postcode = addresslines[5].Trim();
                    address.Country = addresslines[6].Trim();
                    break;
            }
            return address;
        }

        /// <summary>
        /// Takes a full name, with or without tiles and or middle names and parses it into a Name object
        /// </summary>
        /// <param name="fullname">The full name you wish to parse</param>
        /// <returns>A name object with all fields set</returns>
        public static Name ParseName(string fullname)
        {
            var names = fullname.Split(' ');
            Name name = new Name();
            name.FirstName = "";
            name.LastName = "";
            name.MiddleName = "";
            name.Title = "";
            switch (names.Length)
            {
                case 1:
                    name.FirstName = names[0].UppercaseFirst();
                    //only first name provided (or blank)
                    break;
                case 2:
                    switch (names[0].Trim('.').ToLower())
                    {
                        case "mr":
                        case "miss":
                        case "mrs":
                        case "ms":
                        case "dr":
                        case "rev":
                            name.Title = names[0].UppercaseFirst();
                            name.LastName = names[1].UppercaseFirst();
                            break;
                        default:
                            name.FirstName = names[0].UppercaseFirst();
                            name.LastName = names[1].UppercaseFirst();
                            break;
                    }
                    break;
                case 3:
                    switch (names[0].Trim('.').ToLower())
                    {
                        case "mr":
                        case "miss":
                        case "mrs":
                        case "ms":
                        case "dr":
                        case "rev":
                            name.Title = names[0].UppercaseFirst();
                            name.FirstName = names[1].UppercaseFirst();
                            name.LastName = names[2].UppercaseFirst();
                            break;
                        default:
                            name.FirstName = names[0].UppercaseFirst();
                            name.LastName = names[2].UppercaseFirst();
                            name.MiddleName = names[1].UppercaseFirst();
                            break;
                    }
                    break;
                default:
                    switch (names[0].Trim('.').ToLower())
                    {
                        case "mr":
                        case "miss":
                        case "mrs":
                        case "ms":
                        case "dr":
                        case "rev":
                            name.Title = names[0].UppercaseFirst();
                            name.FirstName = names[1].UppercaseFirst();
                            name.MiddleName = names[2].UppercaseFirst();
                            name.LastName = names[3].UppercaseFirst();
                            break;
                        default:
                            name.FirstName = names[0].UppercaseFirst();
                            name.MiddleName = names[1].UppercaseFirst() + " " + names[2].UppercaseFirst();
                            name.LastName = names[3].UppercaseFirst();
                            break;
                    }
                    break;
            }
            return name;
        }
        /// <summary>
        /// Takes an eBay item description,
        /// and converts this into an EbayItem object with it's own Description, Colour, Size and Item type enum properties
        /// </summary>
        /// <param name="itemDescription">An eBay itemdescription such as: "Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours"</param>
        /// <returns>EbayItem object</returns>
        public static EbayItem ParseEbayItem(string itemDescription)
        {
            //try
            //{
            EbayItem ebayItem = new EbayItem();
            if (itemDescription.Contains("\r\n"))
            {
                itemDescription = itemDescription.Replace("\r\n", "");
            }
            Match match = Regex.Match(itemDescription, @"(.*)\[(Black|Red|Charcoal|Navy Blue|Navy|Light Grey|Royal Blue|Green|Turquoise Blue|Turquise Blue|Orange|Baby Pink|Sky Blue|Purple|Beige)\s*,(.*)\s*\]*", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                ebayItem.Decription = match.Groups[1].Value.Trim();
                ebayItem.Colour = match.Groups[2].Value;
                ebayItem.Item = DetermineItemTypeFromDescription(ebayItem.Decription);
                ebayItem.Size = DetermineItemSizeFromSizestring(match.Groups[3].Value, ebayItem.Item);
                ebayItem.SKU = GetSKUFromItem(ebayItem);
                return ebayItem;
            }
            else
            {
                Match matchOtherWay = Regex.Match(itemDescription, @"(.*)\[(.*),(Black|Red|Charcoal|Navy Blue|Navy|Light Grey|Royal Blue|Green|Turquoise Blue|Turquise Blue|Orange|Baby Pink|Sky Blue|Purple|Beige)\s*\]*", RegexOptions.IgnoreCase);
                if (matchOtherWay.Success)
                {
                    ebayItem.Decription = matchOtherWay.Groups[1].Value.Trim();
                    ebayItem.Colour = matchOtherWay.Groups[3].Value;
                    ebayItem.Item = DetermineItemTypeFromDescription(ebayItem.Decription);
                    ebayItem.Size = DetermineItemSizeFromSizestring(matchOtherWay.Groups[2].Value, ebayItem.Item);
                    ebayItem.SKU = GetSKUFromItem(ebayItem);
                    return ebayItem;
                }
            }
            return null;
            //}
            //catch
            //{
            //    Process.Summary.AppendLine ("not an ebay item");
            //    return null;
            //}
        }
        /// <summary>
        /// Takes an eBayItem and tries to generate a SKU from it
        /// </summary>
        /// <param name="ebayItem"></param>
        /// <returns></returns>
        public static string GetSKUFromItem(EbayItem ebayItem)
        {
            try
            {
                var v = ebayItem.Colour.Replace(" ", "").ToLower();
                var e = Enum.Parse(typeof(ColourCode), v);
                var x = Enum.Format(typeof(ColourCode), e, "d").PadLeft(3, '0');
                var SKU = ebayItem.Item + x + ebayItem.Size.ToString().PadLeft(3, 'O');
                return SKU;
            }
            catch
            {
                Process.Summary.AppendLine("Couldn't parse eBayItem");
                return "";
            }
        }
        /// <summary>
        /// Takes a PlainCC item description, and tries to work out what item it is
        /// </summary>
        /// <param name="description">An eBay item such as "Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours"</param>
        /// <returns></returns>
        public static PCCitemcode DetermineItemTypeFromDescription(string description)
        {
            var d = description.ToLower();
            if (d.Contains("kids") && (d.Contains("hoodie") || d.Contains("hooded")) && (d.Contains("pull") || d.Contains("blank"))) { return PCCitemcode.KHPA; }
            if (d.Contains("kids") && (d.Contains("hoodie") || d.Contains("hooded")) && d.Contains("zip")) { return PCCitemcode.KHZA; }
            if (d.Contains("kids") && d.Contains("jog")) { return PCCitemcode.KJPA; }
            if ((d.Contains("ladies") || d.Contains("womens")) && (d.Contains("jog") || d.Contains("yoga")) && d.Contains("pants")) { return PCCitemcode.LJPA; }
            if (d.Contains("mens") && (d.Contains("pull") || d.Contains("blank")) && (d.Contains("hoodie") || d.Contains("hooded")) && !d.Contains("women")) { return PCCitemcode.MHPA; }
            if (d.Contains("mens") && d.Contains("zip") && (d.Contains("hoodie") || d.Contains("hooded")) && !d.Contains("women")) { return PCCitemcode.MHZA; }
            Process.Summary.AppendFormat("Can't determine clothing type for {0}\r\n", description);
            return PCCitemcode.NONE;
        }
        /// <summary>
        /// Given an the size portion of an eBay item description, tries to correctly determine the size of the item.
        /// </summary>
        /// <param name="sizeString"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static PCCsize DetermineItemSizeFromSizestring(string sizeString, PCCitemcode Item)
        {
            var s = sizeString.ToUpper();
            if (Item == PCCitemcode.KHPA || Item == PCCitemcode.KHZA || Item == PCCitemcode.KJPA)
            {
                if (s.Contains("2-3")) { return PCCsize.XXS; }
                if (s.Contains("4-5")) { return PCCsize.XS; }
                if (s.Contains("6-7")) { return PCCsize.S; }
                if (s.Contains("8-9")) { return PCCsize.M; }
                if (s.Contains("10-11")) { return PCCsize.L; }
                if (s.Contains("12-13")) { return PCCsize.XL; }
                if (s.Contains("14") || s.Contains("14-15")) { return PCCsize.XXL; }
            }
            if (Item == PCCitemcode.LJPA)
            {
                if (s.Contains("6-8")) { return PCCsize.XS; }
                if (s.Contains("8-10")) { return PCCsize.S; }
                if (s.Contains("10-12")) { return PCCsize.M; }
                if (s.Contains("12-14")) { return PCCsize.L; }
                if (s.Contains("14-16")) { return PCCsize.XL; }
            }
            else
            {
                if (s.Contains("XXL")) { return PCCsize.XXL; }
                if (s.Contains("XS")) { return PCCsize.XS; }
                if (s.Contains("XL")) { return PCCsize.XL; }
                if (s.Contains("S")) { return PCCsize.S; }
                if (s.Contains("M")) { return PCCsize.M; }
                if (s.Contains("L")) { return PCCsize.L; }
            }
            throw new MissingFieldException("Can't determine item size");
        }
    }
}
