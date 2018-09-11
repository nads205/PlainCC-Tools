using FileHelpers.MasterDetail;
using Examples.Core.DataStructures;
using FileHelpers;

namespace Examples.Core.IO
{
    /// <summary>
    /// Takes care of loading Paypal Files
    /// </summary>
    public class Paypal
    {
        public MasterDetails[] LoadPaypalFile(string PaypalFilePath)
        {
            MasterDetails[] recordsMS;
            try //see if you can open the file in the new Paypal Format
            {
                MasterDetailEngine engine = new MasterDetailEngine(typeof(PaypalRecord), typeof(PaypalRecord), new MasterDetailSelector(Process.PaypalSelectorClass.PaypalSelector));
                recordsMS = (MasterDetails[])engine.ReadFile(PaypalFilePath);
            }
            catch (ConvertException) //else try to open in the old Paypal Format
            {
                try
                {
                    MasterDetailEngine engine = new MasterDetailEngine(typeof(PaypalRecordOldType), typeof(PaypalRecordOldType), new MasterDetailSelector(Process.PaypalSelectorClass.PaypalSelector));
                    recordsMS = (MasterDetails[])engine.ReadFile(PaypalFilePath);
                    foreach (MasterDetails record in recordsMS)
                    {
                        //convert each record in the file to a PaypalRecord from PaypalRecordOldType!
                        record.Master = Core.Converter.ConvertToPaypalRecord((PaypalRecordOldType)record.Master);
                        for (int x = 0; x < record.Details.Length; x++)
                        {
                            record.Details[x] = Converter.ConvertToPaypalRecord((PaypalRecordOldType)record.Details[x]);
                        }
                    }
                }
                catch (ConvertException)
                {
                    MasterDetailEngine engine = new MasterDetailEngine(typeof(PaypalRecordBusinessAccount), typeof(PaypalRecordBusinessAccount), new MasterDetailSelector(Process.PaypalSelectorClass.PaypalSelector));
                    recordsMS = (MasterDetails[])engine.ReadFile(PaypalFilePath);
                    foreach (MasterDetails record in recordsMS)
                    {
                        //convert each record in the file to a PaypalRecord from PaypalRecordOldType!
                        record.Master = Core.Converter.ConvertToPaypalRecord((PaypalRecordBusinessAccount)record.Master);
                        for (int x = 0; x < record.Details.Length; x++)
                        {
                            record.Details[x] = Converter.ConvertToPaypalRecord((PaypalRecordBusinessAccount)record.Details[x]);
                        }
                    }
                }
            }

            return recordsMS;
        }
    }
}
