using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Excel
{
    public class SpreadSheetModel
    {
        public static SpreadsheetDocument SpreadsheetDocument;
        public static WorkbookPart Workbookpart;
        public static WorksheetPart WorksheetPart;
        public static Sheets Sheets;

        public static List<string> ColumnCharacter;

        //create spreadsheet
        public SpreadSheetModel(string path , string worksheetName)
        {
            SpreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            Workbookpart = SpreadsheetDocument.AddWorkbookPart();
            Workbookpart.Workbook = new Workbook();
            WorksheetPart = Workbookpart.AddNewPart<WorksheetPart>();
            WorksheetPart.Worksheet = new Worksheet(new SheetData());

            Columns lstColumns = new Columns();
            
            //lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 25, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 25, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 25, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 20, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 20, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 20, CustomWidth = true }); 
            //lstColumns.Append(new Column() { Min = 8, Max = 8, Width = 20, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 9, Max = 9, Width = 20, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 10, Max = 10, Width = 20, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 11, Max = 11, Width = 20, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 12, Max = 12, Width = 20, CustomWidth = true });

            WorksheetPart.Worksheet.InsertAt(lstColumns, 0);
            InitialWorkstyle();
            

            Sheets sheets = SpreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet()
            {
                Id = SpreadsheetDocument.WorkbookPart.
                GetIdOfPart(WorksheetPart),
                SheetId = 1,
                Name = worksheetName
            };
            sheets.Append(sheet);
            Workbookpart.Workbook.Save();

            
            InitialColumnCharacterIndex();
        }
        private void InitialColumnCharacterIndex()
        {
            ColumnCharacter = new List<string>();
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                ColumnCharacter.Add(Char.ToString(c));
            }
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                for (char cc = 'A'; cc <= 'Z'; ++cc)
                {
                    ColumnCharacter.Add(Char.ToString(c) + Char.ToString(cc));
                }
            }
        }
        private void InitialWorkstyle()
        {
            var stylesPart = SpreadsheetDocument.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = new Stylesheet();

            // blank font list
            stylesPart.Stylesheet.Fonts = new Fonts();
            stylesPart.Stylesheet.Fonts.Count = 1;
            stylesPart.Stylesheet.Fonts.AppendChild(new Font());

            // create fills
            stylesPart.Stylesheet.Fills = new Fills();
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }); // required, reserved by Excel
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }); // required, reserved by Excel
            stylesPart.Stylesheet.Fills.Count = 2;

            // blank border list
            stylesPart.Stylesheet.Borders = new Borders();
            stylesPart.Stylesheet.Borders.Count = 1;
            stylesPart.Stylesheet.Borders.AppendChild(new Border());

            // blank cell format list
            stylesPart.Stylesheet.CellStyleFormats = new CellStyleFormats();
            stylesPart.Stylesheet.CellStyleFormats.Count = 1;
            stylesPart.Stylesheet.CellStyleFormats.AppendChild(new CellFormat());

            // cell format list
            stylesPart.Stylesheet.CellFormats = new CellFormats();
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat());
            stylesPart.Stylesheet.CellFormats.Count = 1;
            stylesPart.Stylesheet.Save();
        }
        public uint InsertBorder(WorkbookPart workbookPart, Border border)
        {
            Borders borders = workbookPart.WorkbookStylesPart.Stylesheet.Elements<Borders>().First();
            borders.Append(border);
            return (uint)borders.Count++;
        }
        public uint InsertCellFormat(WorkbookPart workbookPart, CellFormat cellFormat)
        {
            CellFormats cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.Elements<CellFormats>().First();
            cellFormats.Append(cellFormat);
            return (uint)cellFormats.Count++;
        }
        public Border GenerateBorder()
        {
            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color1 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color1);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color2 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color2);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color3);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color4);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            return border2;
        }
        public void CreateCell(string text , uint row, int col ,CellValues cellValues = CellValues.String , double width = 250.0 ) 
        {
            Cell cell = InsertCellInWorksheet(ColumnCharacter[col], row+1, WorksheetPart);
            CellFormat cellFormat =  new CellFormat();
            cellFormat.BorderId = InsertBorder(Workbookpart, GenerateBorder());
            
            cell.CellValue = new CellValue(text);
            cell.DataType = new EnumValue<CellValues>(cellValues);
            cell.StyleIndex = InsertCellFormat(Workbookpart, cellFormat);

        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }


            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }
                UInt32 colIndex = (uint)ColumnCharacter.FindIndex(c => String.Compare(c, columnName) == 0);
                Columns columns = worksheet.GetFirstChild<Columns>();
                
                if (columns.Elements<Column>().Count() < Int32.Parse(colIndex.ToString()))
                {
                    columns.Append(new Column() { Min = colIndex, Max = colIndex, Width = 23, CustomWidth = true });
                }
                
                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        public void Save()
        {
            WorksheetPart.Worksheet.Save();
        }

        public void Close()
        {
            SpreadsheetDocument.Close();
        }
    }
}
