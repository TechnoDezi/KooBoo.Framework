using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooBoo.Framework.Helpers
{
    public class PDFHelper
    {
        /// <summary>
        /// Creates a PDF from html string
        /// </summary>
        /// <param name="htmlContent">The HTML content to be converted</param>
        /// <param name="pageSize">The size of the output page</param>
        /// <returns></returns>
        public MemoryStream CreatePDFFromHtml(string htmlContent, iTextSharp.text.Rectangle pageSize)
        {
            MemoryStream ms = new MemoryStream();

            //Step 1: Create a Docuement-Object
            iTextSharp.text.Document document = new iTextSharp.text.Document(pageSize);

            //Step 2: we create a writer that listens to the document
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

            //Step 3: Open the document
            document.Open();

            // Add a new page to the pdf file
            document.NewPage();

            //make an arraylist ....with STRINGREADER since its no IO reading file...
            List<iTextSharp.text.IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlContent), null);

            //add the collection to the document
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                document.Add((iTextSharp.text.IElement)htmlarraylist[k]);
            }

            document.Close();

            return ms;
        }
    }
}
