﻿using System;
using System.Collections.Generic;
using PicoXLSX;


namespace Demo
{
    class Program
    {
        /// <summary>
        /// Method to run all demos 
        /// </summary>
        /// <param name="args">Not used</param>
        static void Main(string[] args)
        {
            basicDemo();
            Demo1();
            Demo2();
            Demo3();
            Demo4();
            Demo5();
            Demo6();
            Demo7();
        }

        /// <summary>
        /// This is a very basic demo (adding three values and save the workbook)
        /// </summary>
        private static void basicDemo()
        {
            Workbook workbook = new Workbook("basic.xlsx", "Sheet1");   // Create new workbook
            workbook.CurrentWorksheet.AddNextCell("Test");              // Add cell A1
            workbook.CurrentWorksheet.AddNextCell("Test2");              // Add cell B1
            workbook.CurrentWorksheet.AddNextCell("Test3");              // Add cell C1
            workbook.Save();
        }

        /// <summary>
        /// This method shows the usage of AddNextCell with several data types and formulas
        /// </summary>
        private static void Demo1()
        {
            Workbook workbook = new Workbook("test1.xlsx", "Sheet1");   // Create new workbook
            workbook.CurrentWorksheet.AddNextCell("Test");              // Add cell A1
            workbook.CurrentWorksheet.AddNextCell(123);                 // Add cell B1
            workbook.CurrentWorksheet.AddNextCell(true);                // Add cell C1
            workbook.CurrentWorksheet.GoToNextRow();                    // Go to Row 2
            workbook.CurrentWorksheet.AddNextCell(123.456d);            // Add cell A2
            workbook.CurrentWorksheet.AddNextCell(123.789f);            // Add cell B2
            workbook.CurrentWorksheet.AddNextCell(DateTime.Now);        // Add cell C2
            workbook.CurrentWorksheet.GoToNextRow();                    // Go to Row 3
            workbook.CurrentWorksheet.AddNextCellFormula("B1*22");      // Add cell A3 as formula (B1 times 22)
            workbook.CurrentWorksheet.AddNextCellFormula("ROUNDDOWN(A2,1)"); // Add cell B3 as formula (Floor A2 with one decimal place)
            workbook.CurrentWorksheet.AddNextCellFormula("PI()");       // Add cell C3 as formula (Pi = 3.14.... )
            workbook.Save();                                            // Save the workbook
        }

        /// <summary>
        /// This demo shows the usage of several data types, the method AddCell, more than one worksheet and the SaveAs method
        /// </summary>
        private static void Demo2()
        {
            Workbook workbook = new Workbook(false);                    // Create new workbook
            workbook.AddWorksheet("Sheet1");                            // Add a new Worksheet and set it as current sheet
            workbook.CurrentWorksheet.AddNextCell("月曜日");            // Add cell A1 (Unicode)
            workbook.CurrentWorksheet.AddNextCell(-987);                // Add cell B1
            workbook.CurrentWorksheet.AddNextCell(false);               // Add cell C1
            workbook.CurrentWorksheet.GoToNextRow();                    // Go to Row 2
            workbook.CurrentWorksheet.AddNextCell(-123.456d);           // Add cell A2
            workbook.CurrentWorksheet.AddNextCell(-123.789f);           // Add cell B2
            workbook.CurrentWorksheet.AddNextCell(DateTime.Now);        // Add cell C3
            workbook.AddWorksheet("Sheet2");                            // Add a new Worksheet and set it as current sheet
            workbook.CurrentWorksheet.AddCell("ABC", "A1");             // Add cell A1
            workbook.CurrentWorksheet.AddCell(779, 2, 1);               // Add cell C2 (zero based addresses: column 2=C, row 1=2)
            workbook.CurrentWorksheet.AddCell(false, 3, 2);             // Add cell D3 (zero based addresses: column 3=D, row 2=3)
            workbook.CurrentWorksheet.AddNextCell(0);                   // Add cell E3 (direction: column to column)
            List<string> values = new List<string>() { "V1", "V2", "V3" }; // Create a List of values
            workbook.CurrentWorksheet.AddCellRange(values, "A4:C4");    // Add a cell range to A4 - C4
            workbook.SaveAs("test2.xlsx");                              // Save the workbook
        }

        /// <summary>
        /// This demo shows the usage of flipped direction when using AddnextCell
        /// </summary>
        private static void Demo3()
        {
            Workbook workbook = new Workbook("test3.xlsx", "Sheet1");   // Create new workbook
            workbook.CurrentWorksheet.CurrentCellDirection = Worksheet.CellDirection.RowToRow;  // Change the cell direction
            workbook.CurrentWorksheet.AddNextCell(1);                   // Add cell A1
            workbook.CurrentWorksheet.AddNextCell(2);                   // Add cell A2
            workbook.CurrentWorksheet.AddNextCell(3);                   // Add cell A3
            workbook.CurrentWorksheet.AddNextCell(4);                   // Add cell A4
            workbook.CurrentWorksheet.GoToNextColumn();                 // Go to Column B
            workbook.CurrentWorksheet.AddNextCell("A");                 // Add cell B1
            workbook.CurrentWorksheet.AddNextCell("B");                 // Add cell B2
            workbook.CurrentWorksheet.AddNextCell("C");                 // Add cell B3
            workbook.CurrentWorksheet.AddNextCell("D");                 // Add cell B4
            workbook.CurrentWorksheet.RemoveCell("A2");                 // Delete cell A2
            workbook.CurrentWorksheet.RemoveCell(1,1);                  // Delete cell B2
            workbook.Save();                                            // Save the workbook
        }

        /// <summary>
        /// This demo shows the usage of several styles, column widths and row heights
        /// </summary>
        private static void Demo4()
        {
            Workbook workbook = new Workbook("test4.xlsx", "Sheet1");                                        // Create new workbook
            List<string> values = new List<string>() { "Header1", "Header2", "Header3" };                    // Create a List of values
            workbook.CurrentWorksheet.AddCellRange(values, new Cell.Address(0,0), new Cell.Address(2,0));    // Add a cell range to A4 - C4
            workbook.CurrentWorksheet.Cells["A1"].SetStyle(Style.BasicStyles.Bold, workbook);                // Assign predefined basic style to cell
            workbook.CurrentWorksheet.Cells["B1"].SetStyle(Style.BasicStyles.Bold, workbook);                // Assign predefined basic style to cell
            workbook.CurrentWorksheet.Cells["C1"].SetStyle(Style.BasicStyles.Bold, workbook);                // Assign predefined basic style to cell
            workbook.CurrentWorksheet.GoToNextRow();                                                         // Go to Row 2
            workbook.CurrentWorksheet.AddNextCell(DateTime.Now);                                             // Add cell A2
            workbook.CurrentWorksheet.AddNextCell(2);                                                        // Add cell B2
            workbook.CurrentWorksheet.AddNextCell(3);                                                        // Add cell B2
            workbook.CurrentWorksheet.GoToNextRow();                                                         // Go to Row 3
            workbook.CurrentWorksheet.AddNextCell(DateTime.Now.AddDays(1));                                  // Add cell B1
            workbook.CurrentWorksheet.AddNextCell("B");                                                      // Add cell B2
            workbook.CurrentWorksheet.AddNextCell("C");                                                      // Add cell B3

            Style s = new Style();                                                                          // Create new style
            s.CurrentFill.SetColor("FF22FF11", Style.Fill.FillType.fillColor);                              // Set fill color
            s.CurrentFont.DoubleUnderline = true;                                                           // Set double underline
            s.CurrentCellXf.HorizontalAlign = Style.CellXf.HorizontalAlignValue.center;                     // Set alignment

            workbook.CurrentWorksheet.Cells["B2"].SetStyle(s, workbook);                                    // Assign style to cell
            workbook.CurrentWorksheet.GoToNextRow();                                                        // Go to Row 3
            workbook.CurrentWorksheet.AddNextCell(DateTime.Now.AddDays(2));                                 // Add cell B1
            workbook.CurrentWorksheet.AddNextCell(true);                                                    // Add cell B2
            workbook.CurrentWorksheet.AddNextCell(false);                                                   // Add cell B3 
            workbook.CurrentWorksheet.Cells["C2"].SetStyle(Style.BasicStyles.BorderFrame, workbook);        // Assign predefined basic style to cell

            Style s2 = new Style();                                                                         // Create new style
            s2.CurrentCellXf.TextRotation = 45;                                                             // Set text rotation
            s2.CurrentCellXf.VerticalAlign = Style.CellXf.VerticallAlignValue.center;                       // Set alignment

            workbook.CurrentWorksheet.Cells["B4"].SetStyle(s2, workbook);                                   // Assign style to cell

            workbook.CurrentWorksheet.SetColumnWidth(0, 20f);                                               // Set column width
            workbook.CurrentWorksheet.SetColumnWidth(1, 15f);                                               // Set column width
            workbook.CurrentWorksheet.SetColumnWidth(2, 25f);                                               // Set column width
            workbook.CurrentWorksheet.SetRowHeight(0, 20);                                                 // Set row height
            workbook.CurrentWorksheet.SetRowHeight(1, 30);                                                 // Set row height
                      
            workbook.Save();                                                                               // Save the workbook
        }

        /// <summary>
        /// This demo shows the usage of cell ranges, adding and removing styles, and meta data 
        /// </summary>
        private static void Demo5()
        {
            Workbook workbook = new Workbook("test5.xlsx", "Sheet1");                                   // Create new workbook
            List<string> values = new List<string>() { "Header1", "Header2", "Header3" };               // Create a List of values
            workbook.CurrentWorksheet.SetActiveStyle(Style.BasicStyles.BorderFrameHeader, workbook);    // Assign predefined basic style as active style
            workbook.CurrentWorksheet.AddCellRange(values, "A1:C1");                                    // Add cell range

            values = new List<string>() { "Cell A2", "Cell B2", "Cell C2" };                            // Create a List of values
            workbook.CurrentWorksheet.SetActiveStyle(Style.BasicStyles.BorderFrame, workbook);          // Assign predefined basic style as active style
            workbook.CurrentWorksheet.AddCellRange(values, "A2:C2");                                    // Add cell range

            values = new List<string>() { "Cell A3", "Cell B3", "Cell C3" };                            // Create a List of values
            workbook.CurrentWorksheet.AddCellRange(values, "A3:C3");                                    // Add cell range

            values = new List<string>() { "Cell A4", "Cell B4", "Cell C4" };                            // Create a List of values
            workbook.CurrentWorksheet.ClearActiveStyle();                                               // Clear the active style 
            workbook.CurrentWorksheet.AddCellRange(values, "A4:C4");                                     // Add cell range

            workbook.WorkbookMetadata.Title = "Test 5";                                                 // Add meta data to workbook
            workbook.WorkbookMetadata.Subject = "This is the 5th PicoXLSX test";                        // Add meta data to workbook
            workbook.WorkbookMetadata.Creator = "PicoXLSX";                                             // Add meta data to workbook
            workbook.WorkbookMetadata.Keywords = "Keyword1;Keyword2;Keyword3";                          // Add meta data to workbook

            workbook.Save();                                                                            // Save the workbook
        }

        /// <summary>
        /// This demo shows the usage of merging cells, protecting cells, worksheet password protection and workbook protection
        /// </summary>
        private static void Demo6()
        {
            Workbook workbook = new Workbook("test6.xlsx", "Sheet1");                                   // Create new workbook
            workbook.CurrentWorksheet.AddNextCell("Mergerd1");                                          // Add cell A1
            workbook.CurrentWorksheet.MergeCells("A1:C1");                                              // Merge cells from A1 to C1
            workbook.CurrentWorksheet.GoToNextRow();                                                    // Go to next row
            workbook.CurrentWorksheet.AddNextCell(false);                                               // Add cell A2
            workbook.CurrentWorksheet.MergeCells("A2:D2");                                              // Merge cells from A2 to D1
            workbook.CurrentWorksheet.GoToNextRow();                                                    // Go to next row
            workbook.CurrentWorksheet.AddNextCell("22.2d");                                             // Add cell A3
            workbook.CurrentWorksheet.MergeCells("A3:E4");                                              // Merge cells from A3 to E4
            workbook.AddWorksheet("Protected");                                                         // Add a new worksheet
            workbook.CurrentWorksheet.AddAllowedActionOnSheetProtection(Worksheet.SheetProtectionValue.sort);               // Allow to sort sheet (worksheet is automatically set as protected)
            workbook.CurrentWorksheet.AddAllowedActionOnSheetProtection(Worksheet.SheetProtectionValue.insertRows);         // Allow to insert rows
            workbook.CurrentWorksheet.AddAllowedActionOnSheetProtection(Worksheet.SheetProtectionValue.selectLockedCells);  // Allow to select cells (locked cells caused automatically to select unlocked cells)
            workbook.CurrentWorksheet.AddNextCell("Cell A1");                                           // Add cell A1
            workbook.CurrentWorksheet.AddNextCell("Cell B1");                                           // Add cell B1
            workbook.CurrentWorksheet.Cells["A1"].SetCellLockedState(false, true, workbook);            // Set the locking state of cell A1 (not locked but value is hidden when cell selected)
            workbook.AddWorksheet("PWD-Protected");                                                     // Add a new worksheet
            workbook.CurrentWorksheet.AddCell("This worksheet is password protected. The password is:",0,0);  // Add cell A1
            workbook.CurrentWorksheet.AddCell("test123", 0, 1);                                         // Add cell A2
            workbook.CurrentWorksheet.SetSheetProtectionPassword("test123");                            // Set the password "test123"
            workbook.SetWorkbookProtection(true, true, true, null);                                     // Set workbook protection (windows locked, structure locked, no password)
            workbook.Save();                                                                            // Save the workbook
        }

        /// <summary>
        /// This demo shows the usage of hiding rows and columns, and auto-filter
        /// </summary>
        private static void Demo7()
        {
            Workbook workbook = new Workbook("test7.xlsx", "Sheet1");                                   // Create new workbook
            Worksheet ws = workbook.CurrentWorksheet;                                                   // Create reference (shortening)
            List<string> values = new List<string>() { "Cell A1", "Cell B1", "Cell C1", "Cell D1" };    // Create a List of values
            ws.AddCellRange(values, "A1:D1");                                                           // Insert cell range
            values = new List<string>() { "Cell A2", "Cell B2", "Cell C2", "Cell D2" };                 // Create a List of values
            ws.AddCellRange(values, "A2:D2");                                                           // Insert cell range
            values = new List<string>() { "Cell A3", "Cell B3", "Cell C3", "Cell D3" };                 // Create a List of values
            ws.AddCellRange(values, "A3:D3");                                                           // Insert cell range
            ws.AddHiddenColumn("C");                                                                    // Hide column C
            ws.AddHiddenRow(1);                                                                         // Hider row 2 (zero-based: 1)
            ws.SetAutoFilter(1, 3);                                                                     // Set auto-filter for column B to D
            workbook.Save();                                                                            // Save the workbook
        }


    }
}
