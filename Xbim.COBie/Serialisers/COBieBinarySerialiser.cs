﻿using System;
using Xbim.COBie.Contracts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Xbim.COBie.Serialisers
{
    public class COBieBinarySerialiser : ICOBieSerialiser
    {
        private string _file;

        public COBieBinarySerialiser(string file)
        {
            _file = file;

        }

        public void Serialise(COBieWorkbook workbook, ICOBieValidationTemplate validationTemplate = null)
        {
            if (workbook == null)
            {
                throw new ArgumentNullException("workbook", "Xbim");
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (Stream stream = new FileStream(_file, FileMode.Create, FileAccess.Write, FileShare.None))
            { 
                formatter.Serialize(stream, workbook);
            }

        }
    }
}
