using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using Examples.Core.DataStructures;
using System.IO;

namespace Examples.Core.IO
{
    /// <summary>
    /// Takes care of outputting files into Magento importer format
    /// </summary>
    public class Magento
    {
        public void SaveMagentoFile(string MagentoFilePath, string HeaderRow, List<iMagentoRecord> mList)
        {
            try
            {
                List<MagentoRecord> newlist = mList.Cast<MagentoRecord>().ToList();
                FileHelperEngine<MagentoRecord> magentoEngine = new FileHelperEngine<MagentoRecord>();
                File.WriteAllText(MagentoFilePath, HeaderRow);
                magentoEngine.AppendToFile(MagentoFilePath, newlist);
            }
            catch
            {
                List<MagentoRecordSB> newlist = mList.Cast<MagentoRecordSB>().ToList();
                FileHelperEngine<MagentoRecordSB> magentoEngine = new FileHelperEngine<MagentoRecordSB>();
                File.WriteAllText(MagentoFilePath, HeaderRow);
                magentoEngine.AppendToFile(MagentoFilePath, newlist);
            }
        }
    }
}
