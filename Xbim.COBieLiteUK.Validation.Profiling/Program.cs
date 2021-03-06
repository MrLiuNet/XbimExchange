﻿using System.IO;
using Xbim.CobieLiteUk.Validation;
using Xbim.CobieLiteUk.Validation.Reporting;

namespace Xbim.CobieLiteUk.Validation.Profiling
{
    class Program
    {
        static void Main(string[] args)
        {
            const string xlsx = @"Lakeside_Restaurant_fabric_only.xlsx";
            string msg;
            var cobie = Facility.ReadCobie(xlsx, out msg);
            var req = Facility.ReadJson(@"003-Lakeside_Restaurant-stage6-COBie.json");
            var validator = new FacilityValidator();
            var result = validator.Validate(req, cobie);

            //create report
            using (var stream = File.Create(@"Lakeside_Restaurant_fabric_only.report.xlsx"))
            {
                var report = new ExcelValidationReport();
                report.Create(result, stream, ExcelValidationReport.SpreadSheetFormat.Xlsx);
                stream.Close();
            }
        }
    }
}
