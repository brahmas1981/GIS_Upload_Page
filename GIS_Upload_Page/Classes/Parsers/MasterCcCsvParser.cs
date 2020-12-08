using GIS_Upload_Page.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIS_Upload_Page.Classes.Exceptions;
using GIS_Upload_Page.Extensions;

namespace GIS_Upload_Page.Classes.Parsers
{
    public class MasterCcCsvParser : IParser<MasterCcViewModel>
    {
       // private const int EXPECTED_ROW_NU = 10;

        public MasterCcViewModel Parse(string data, out bool isHeader)
        {
            isHeader = false;
            MasterCcViewModel viewModel = null;

            if (!string.IsNullOrEmpty(data))
            {
                var values = data.Split(",", StringSplitOptions.RemoveEmptyEntries);
                //if (values.Length != EXPECTED_ROW_NU)
                //    throw new ShowMessageException("There was an error parsing the file- invalid number of entries");
                //else if (string.Equals(values[0], "Source ID", StringComparison.OrdinalIgnoreCase))
                //        isHeader = true;
                //else
                //{
                    //viewModel = new MasterCcViewModel();
                    //viewModel.SourceID = values[0].TrimSafe();
                    //viewModel.SourceDocumentName = values[1].TrimSafe();
                    //viewModel.SourceDocumentNumber = values[2].TrimSafe();
                    //viewModel.VerbatimStatement = values[3].TransformTextForDb();
               // }
            }

            return viewModel;
        }

        MasterCcViewModel IParser<MasterCcViewModel>.Parse(string data)
        {
            bool isHeader = false;
            return Parse(data, out isHeader);
        }
    }
}
