﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace MsCrmTools.Translator.AppCode
{
    public class GlobalOptionSetTranslation : BaseTranslation
    {
        public GlobalOptionSetTranslation()
        {
            name = "Global OptionSet values";
        }

        /// <summary>
        ///
        /// </summary>
        /// <example>
        /// OptionSet Id;OptionSet Name;OptionSetValue;Type;LCID1;LCID2;...;LCIDX
        /// </example>
        /// <param name="languages"></param>
        /// <param name="sheet"></param>
        /// <param name="service"></param>
        /// <param name="settings"></param>
        public void Export(List<int> languages, ExcelWorksheet sheet, IOrganizationService service, ExportSettings settings)
        {
            var line = 0;

            AddHeader(sheet, languages);

            var request = new RetrieveAllOptionSetsRequest();
            var response = (RetrieveAllOptionSetsResponse)service.Execute(request);
            var omds = response.OptionSetMetadata;
            if (settings.SolutionId != Guid.Empty)
            {
                var oids = service.GetSolutionComponentObjectIds(settings.SolutionId, 9); // 9 = Global OptionSets
                omds = omds.Where(o => oids.Contains(o.MetadataId ?? Guid.Empty)).ToArray();
            }

            foreach (var omd in omds)
            {
                int cell;
                if (omd is OptionSetMetadata oomd)
                {
                    foreach (var option in oomd.Options.OrderBy(o => o.Value))
                    {
                        if (settings.ExportNames)
                        {
                            line++;
                            cell = 0;
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = (oomd.MetadataId ?? Guid.Empty).ToString("B");
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = oomd.Name;
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = option.Value;
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = "Label";

                            foreach (var lcid in languages)
                            {
                                var label = string.Empty;

                                var optionLabel =
                                    option.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
                                if (optionLabel != null)
                                {
                                    label = optionLabel.Label;
                                }

                                ZeroBasedSheet.Cell(sheet, line, cell++).Value = label;
                            }
                        }

                        if (settings.ExportDescriptions)
                        {
                            line++;
                            cell = 0;
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = (oomd.MetadataId ?? Guid.Empty).ToString("B");
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = oomd.Name;
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = option.Value;
                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = "Description";

                            foreach (var lcid in languages)
                            {
                                var label = string.Empty;

                                var optionDescription =
                                    option.Description.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
                                if (optionDescription != null)
                                {
                                    label = optionDescription.Label;
                                }

                                ZeroBasedSheet.Cell(sheet, line, cell++).Value = label;
                            }
                        }
                    }
                }
                else if (omd is BooleanOptionSetMetadata)
                {
                    var bomd = (BooleanOptionSetMetadata)omd;

                    if (settings.ExportNames)
                    {
                        line++;
                        cell = 0;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = (omd.MetadataId ?? Guid.Empty).ToString("B");
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = omd.Name;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = bomd.FalseOption.Value;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = "Label";

                        foreach (var lcid in languages)
                        {
                            var label = string.Empty;

                            if (bomd.FalseOption.Label != null)
                            {
                                var optionLabel =
                                    bomd.FalseOption.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
                                if (optionLabel != null)
                                {
                                    label = optionLabel.Label;
                                }
                            }

                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = label;
                        }
                    }

                    if (settings.ExportDescriptions)
                    {
                        line++;
                        cell = 0;

                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = (omd.MetadataId ?? Guid.Empty).ToString("B");
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = omd.Name;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = bomd.FalseOption.Value;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = "Description";

                        foreach (var lcid in languages)
                        {
                            var label = string.Empty;

                            if (bomd.FalseOption.Description != null)
                            {
                                var optionLabel =
                                    bomd.FalseOption.Description.LocalizedLabels.FirstOrDefault(l =>
                                        l.LanguageCode == lcid);
                                if (optionLabel != null)
                                {
                                    label = optionLabel.Label;
                                }
                            }

                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = label;
                        }
                    }

                    if (settings.ExportNames)
                    {
                        line++;
                        cell = 0;

                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = (omd.MetadataId ?? Guid.Empty).ToString("B");
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = omd.Name;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = bomd.TrueOption.Value;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = "Label";

                        foreach (var lcid in languages)
                        {
                            var label = string.Empty;

                            if (bomd.TrueOption.Label != null)
                            {
                                var optionLabel =
                                    bomd.TrueOption.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
                                if (optionLabel != null)
                                {
                                    label = optionLabel.Label;
                                }
                            }

                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = label;
                        }
                    }

                    if (settings.ExportDescriptions)
                    {
                        line++;
                        cell = 0;

                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = (omd.MetadataId ?? Guid.Empty).ToString("B");
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = omd.Name;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = bomd.TrueOption.Value;
                        ZeroBasedSheet.Cell(sheet, line, cell++).Value = "Description";

                        foreach (var lcid in languages)
                        {
                            var label = string.Empty;

                            if (bomd.TrueOption.Description != null)
                            {
                                var optionLabel =
                                    bomd.TrueOption.Description.LocalizedLabels.FirstOrDefault(l =>
                                        l.LanguageCode == lcid);
                                if (optionLabel != null)
                                {
                                    label = optionLabel.Label;
                                }
                            }

                            ZeroBasedSheet.Cell(sheet, line, cell++).Value = label;
                        }
                    }
                }
            }

            // Applying style to cells
            for (int i = 0; i < (4 + languages.Count); i++)
            {
                StyleMutator.TitleCell(ZeroBasedSheet.Cell(sheet, 0, i).Style);
            }

            for (int i = 1; i <= line; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    StyleMutator.HighlightedCell(ZeroBasedSheet.Cell(sheet, i, j).Style);
                }
            }
        }

        public void Import(ExcelWorksheet sheet, IOrganizationService service, BackgroundWorker worker)
        {
            var requests = new List<UpdateOptionValueRequest>();

            var rowsCount = sheet.Dimension.Rows;

            for (var rowI = 1; rowI < rowsCount; rowI++)
            {
                UpdateOptionValueRequest request = requests.FirstOrDefault(r => r.OptionSetName == ZeroBasedSheet.Cell(sheet, rowI, 1).Value.ToString()
                    && r.Value == int.Parse(ZeroBasedSheet.Cell(sheet, rowI, 2).Value.ToString()));
                if (request == null)
                {
                    request = new UpdateOptionValueRequest
                    {
                        OptionSetName = ZeroBasedSheet.Cell(sheet, rowI, 1).Value.ToString(),
                        Value = int.Parse(ZeroBasedSheet.Cell(sheet, rowI, 2).Value.ToString()),
                        Label = new Label(),
                        Description = new Label(),
                        MergeLabels = true
                    };

                    int columnIndex = 4;

                    if (ZeroBasedSheet.Cell(sheet, rowI, 3).Value.ToString() == "Label")
                    {
                        // WTF: QUESTIONABLE DELETION: row.Cells.Count() > columnIndex &&
                        while (ZeroBasedSheet.Cell(sheet, rowI, columnIndex) != null && ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value != null)
                        {
                            var sLcid = ZeroBasedSheet.Cell(sheet, 0, columnIndex).Value.ToString();
                            var sLabel = ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value.ToString();

                            if (sLcid.Length > 0 && sLabel.Length > 0)
                            {
                                request.Label.LocalizedLabels.Add(new LocalizedLabel(sLabel, int.Parse(sLcid)));
                            }

                            columnIndex++;
                        }
                    }
                    else if (ZeroBasedSheet.Cell(sheet, rowI, 3).Value.ToString() == "Description")
                    {
                        // WTF: QUESTIONABLE DELETION: row.Cells.Count() > columnIndex &&
                        while (ZeroBasedSheet.Cell(sheet, rowI, columnIndex) != null && ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value != null)
                        {
                            var sLcid = ZeroBasedSheet.Cell(sheet, 0, columnIndex).Value.ToString();
                            var sLabel = ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value.ToString();

                            if (sLcid.Length > 0 && sLabel.Length > 0)
                            {
                                request.Description.LocalizedLabels.Add(new LocalizedLabel(sLabel, int.Parse(sLcid)));
                            }

                            columnIndex++;
                        }
                    }

                    requests.Add(request);
                }
                else
                {
                    int columnIndex = 4;

                    if (ZeroBasedSheet.Cell(sheet, rowI, 3).Value.ToString() == "Label")
                    {
                        // WTF: QUESTIONABLE DELETION: row.Cells.Count() > columnIndex &&
                        while (ZeroBasedSheet.Cell(sheet, rowI, columnIndex) != null && ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value != null)
                        {
                            var sLcid = ZeroBasedSheet.Cell(sheet, 0, columnIndex).Value.ToString();
                            var sLabel = ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value.ToString();

                            if (sLcid.Length > 0 && sLabel.Length > 0)
                            {
                                request.Label.LocalizedLabels.Add(new LocalizedLabel(sLabel, int.Parse(sLcid)));
                            }
                            columnIndex++;
                        }
                    }
                    else if (ZeroBasedSheet.Cell(sheet, rowI, 3).Value.ToString() == "Description")
                    {
                        // WTF: QUESTIONABLE DELETION: row.Cells.Count() > columnIndex &&
                        while (ZeroBasedSheet.Cell(sheet, rowI, columnIndex) != null && ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value != null)
                        {
                            var sLcid = ZeroBasedSheet.Cell(sheet, 0, columnIndex).Value.ToString();
                            var sLabel = ZeroBasedSheet.Cell(sheet, rowI, columnIndex).Value.ToString();

                            if (sLcid.Length > 0 && sLabel.Length > 0)
                            {
                                request.Description.LocalizedLabels.Add(new LocalizedLabel(sLabel, int.Parse(sLcid)));
                            }
                            columnIndex++;
                        }
                    }
                }
            }

            var arg = new TranslationProgressEventArgs { SheetName = sheet.Name };
            foreach (var request in requests)
            {
                AddRequest(request);
                ExecuteMultiple(service, arg);
            }
            ExecuteMultiple(service, arg, true);
        }

        private void AddHeader(ExcelWorksheet sheet, IEnumerable<int> languages)
        {
            var cell = 0;

            ZeroBasedSheet.Cell(sheet, 0, cell++).Value = "OptionSet Id";
            ZeroBasedSheet.Cell(sheet, 0, cell++).Value = "OptionSet Name";
            ZeroBasedSheet.Cell(sheet, 0, cell++).Value = "Value";
            ZeroBasedSheet.Cell(sheet, 0, cell++).Value = "Type";

            foreach (var lcid in languages)
            {
                ZeroBasedSheet.Cell(sheet, 0, cell++).Value = lcid.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}