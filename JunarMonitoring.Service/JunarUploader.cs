using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DataStore.Junar
{
    public class JunarUploader
    {

        public JunarUploader()
        {
        }

        public void UploadAll()
        {
            //var paymentSummary = _publicUow.RealEstateAssessmentPaymentSummary.Get().ToList();
            //UploadDataSet(paymentSummary, "Payment Summary TEST");

            //var vwAssessment = _publicUow.Assessment.Get().ToList();
            //UploadDataSet(vwAssessment, "Assessment TEST");

            //var realEstateProperties = _publicUow.Property.Get().ToList();
            //UploadDataSet(realEstateProperties, "Properties TEST");


        }

        public void Upload<T>()
        {
            var t = typeof(T);
            
            //find method
            //download data
            //UploadDataSet
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataset"></param>
        /// <param name="title"></param>
        public void UploadDataSet<T>(IEnumerable<T> dataset, string title, string category, string description, string filetype, string id) where T : class
        {
            //The API domain is the one defined in the Junar Workspace. In this document we’ll use the following: 
            //http://api.data.arlingtonva.us/datastreams/publish/{guid} 

            // guid: An alphanumeric string that's unique to each data view. Optional. Use only when
            //      updating a resource with new file data (source, titles, description, etc.). 
            //      If this parameter is not present, a new resource with a new GUID will be created.
            // title: Up to 100 characters. Mandatory.
            // description: Up to 140 characters. Optional.
            // category : Name of the category to use. It must be a pre-existing category for the account. Mandatory. Categories are created from the Admin section of the account Workspace.  tags: Tags for the data view. Multiple tags can be added separating them by commas. Optional.
            // notes: Additional notes to provide context to the data view. This field supports basic HTML text format and up to 1000 characters. Optional.
            // table_id: The location of the desired table within the document, starting from zero. If the document is an XLS file or any other multi-sheet document, the id will increase by one for each sheet in the document (i.e., the first sheet will be table0, the second sheet will be table1, and so on). Mandatory.
            // auth_key : Your private API auth key, provided by Junar. Mandatory.
            // meta_data: If the resource has an additional metadata field it should be entered here as an encoded JSON element. Optional.
            // file_data: The name of the file to be used as the dataset. The file should be in the same directory as the CURL executable in order to collect it, and contain an @ before the full file name with its extension type (i.e., @sample-dataset.csv). Mandatory if not using ‘source’ parameter.

            // source: The URL to the file to be used as the dataset. Mandatory if not using ‘file_data’ parameter.
            // clone: If set to True, this allows the new revision to inherit the settings of the existing data
            //          view (headers, filters, parameters, column formatting). 
            //          Must be accompanied by the GUID parameter to indicate the data view that is being cloned. Optional.
            // header_row: Defines which row to be selected as the header row of the data view. Rows must be entered as row0 (for the first row), row1 (for the second row) and so on. Only one header row accepted per data view. 

            var csvData = dataset.ToCsv();
            var bytes = Encoding.UTF8.GetBytes(csvData);

            UploadFile(bytes, title, category, description, filetype, id);
        }

        public void UploadFile(byte[] bytes, string title, string category, string description, string extension, string id)
        {
            //The API domain is the one defined in the Junar Workspace. In this document we’ll use the following: 
            //http://api.data.arlingtonva.us/datastreams/publish/{guid} 

            // guid: An alphanumeric string that's unique to each data view. Optional. Use only when
            //      updating a resource with new file data (source, titles, description, etc.). 
            //      If this parameter is not present, a new resource with a new GUID will be created.
            // title: Up to 100 characters. Mandatory.
            // description: Up to 140 characters. Optional.
            // category : Name of the category to use. It must be a pre-existing category for the account. Mandatory. Categories are created from the Admin section of the account Workspace.  tags: Tags for the data view. Multiple tags can be added separating them by commas. Optional.
            // notes: Additional notes to provide context to the data view. This field supports basic HTML text format and up to 1000 characters. Optional.
            // table_id: The location of the desired table within the document, starting from zero. If the document is an XLS file or any other multi-sheet document, the id will increase by one for each sheet in the document (i.e., the first sheet will be table0, the second sheet will be table1, and so on). Mandatory.
            // auth_key : Your private API auth key, provided by Junar. Mandatory.
            // meta_data: If the resource has an additional metadata field it should be entered here as an encoded JSON element. Optional.
            // file_data: The name of the file to be used as the dataset. The file should be in the same directory as the CURL executable in order to collect it, and contain an @ before the full file name with its extension type (i.e., @sample-dataset.csv). Mandatory if not using ‘source’ parameter.

            // source: The URL to the file to be used as the dataset. Mandatory if not using ‘file_data’ parameter.
            // clone: If set to True, this allows the new revision to inherit the settings of the existing data
            //          view (headers, filters, parameters, column formatting). 
            //          Must be accompanied by the GUID parameter to indicate the data view that is being cloned. Optional.
            // header_row: Defines which row to be selected as the header row of the data view. Rows must be entered as row0 (for the first row), row1 (for the second row) and so on. Only one header row accepted per data view. 

            var client = new HttpClient();

            var form = new MultipartFormDataContent();

            form.Add(new StringContent("992553d28eb2f87dab62e79d14e39f5127e59b4e"), "auth_key");
            form.Add(new StringContent(title), "title");
            form.Add(new StringContent(category), "category");
            form.Add(new StringContent(description), "description");
            form.Add(new StringContent("Arlington"), "tags");
            form.Add(new StringContent("published"), "status");
            form.Add(new ByteArrayContent(bytes), "file", $"{title}.{extension}");

            var response = client.PutAsync($"https://api.data.arlingtonva.us/api/v2/datasets/" + id + ".json", form).Result;
            client.Dispose();
        }
    }

    public static class LinqToCSV
    {
        static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof (string)
                   || type == typeof (decimal)
                   || type == typeof (DateTime);

        }

        public static string ToCsv<T>(this IEnumerable<T> items)
            where T : class
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();

            var headerValues = properties.Where(w => IsSimple(w.PropertyType)).Select(p => p.Name.ToCsvValue()).ToArray();
            string headerLine = string.Join(",", headerValues);
            csvBuilder.AppendLine(headerLine);

            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            foreach (T item in items)
            {
                var propertyValues =
                    properties.Where(w => IsSimple(w.PropertyType)).Select(p => p.GetValue(item, null).ToCsvValue()).ToArray();

                string line = string.Join(",", propertyValues)
                    .Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace(lineSeparator, string.Empty)
                    .Replace(paragraphSeparator, string.Empty);

                csvBuilder.AppendLine(line);
            }
            return csvBuilder.ToString();
        }

        private static string ToCsvValue<T>(this T item)
        {
            if (item == null) return "\"\"";

            if (item is string)
            {
                return string.Format("\"{0}\"", item.ToString().Replace("\"", "'"));
            }
            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
            {
                return string.Format("{0}", item);
            }
            return string.Format("\"{0}\"", item);
        }
    }
}
