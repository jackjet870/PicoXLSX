﻿/*
 * PicoXLSX is a small .NET library to generate XLSX (Microsoft Excel 2007 or newer) files in an easy and native way
 * Copyright Raphael Stoeckli © 2015
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PicoXLSX
{
    /// <summary>
    /// Class representing a worksheet of a workbook
    /// </summary>
    public class Worksheet
    {

        /// <summary>
        /// Enum to definte the direction when using AddNextCell method
        /// </summary>
        public enum CellDirection
        {
            /// <summary>The next cell will be on the same row (A1,B1,C1...)</summary>
            ColumnToColum,
            /// <summary>The next cell will be on the same column (A1,A2,A3...)</summary>
            RowToRow
        }

        /// <summary>
        /// Direction when using AddNextCell method
        /// </summary>
        public CellDirection CurrentCellDirection { get; set; }

        private string sheetName;
        private int currentRowNumber;
        private int currentColumnNumber;
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// Name of the worksheet
        /// </summary>
        public string SheetName
        {
            get { return sheetName; }
            set { SetSheetname(value); }
        }
        
        /// <summary>
        /// Internal ID of the sheet
        /// </summary>
        public int SheetID { get; set; }

        /// <summary>
        /// Cells of the worksheet
        /// </summary>
        public Dictionary<string, Cell> Cells
        {
            get { return cells; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Worksheet()
        {
            this.CurrentCellDirection = CellDirection.ColumnToColum;
            this.cells = new Dictionary<string, Cell>();
            this.currentRowNumber = 0;
            this.currentColumnNumber = 0;
        }

        /// <summary>
        /// Constructor with name and sheet ID
        /// </summary>
        /// <param name="name">Name of the worksheet</param>
        /// <param name="id">ID of the worksheet (for internal use)</param>
        public Worksheet(string name, int id) : this()
        {
            SetSheetname(name);
            this.SheetID = id;
        }

#region AddNextCell

        /// <summary>
        /// Adds a string value to the next cell position
        /// </summary>
        /// <param name="value">String value to insert</param>
        public void AddNextCell(string value)
        {
            Cell c = new Cell(value, Cell.CellType.STRING, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Adds a integer value to the next cell position
        /// </summary>
        /// <param name="value">Integer value to insert</param>
        public void AddNextCell(int value)
        {
            Cell c = new Cell(value, Cell.CellType.NUMBER, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Adds a double value to the next cell position
        /// </summary>
        /// <param name="value">Double value to insert</param>
        public void AddNextCell(double value)
        {
            Cell c = new Cell(value, Cell.CellType.NUMBER, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Adds a float value to the next cell position
        /// </summary>
        /// <param name="value">Float value to insert</param>
        public void AddNextCell(float value)
        {
            Cell c = new Cell(value, Cell.CellType.NUMBER, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Adds a DateTime value to the next cell position
        /// </summary>
        /// <param name="value">DateTime value to insert</param>
        public void AddNextCell(DateTime value)
        {
            Cell c = new Cell(value, Cell.CellType.DATE, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Adds a boolean value to the next cell position
        /// </summary>
        /// <param name="value">Boolean value to insert</param>
        public void AddNextCell(bool value)
        {
            Cell c = new Cell(value, Cell.CellType.BOOL, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Adds a formula as string to the next cell position
        /// </summary>
        /// <param name="formula">Formula to insert</param>
        public void AddNextCellFormula(string formula)
        {
            Cell c = new Cell(formula, Cell.CellType.FORMULA, this.currentColumnNumber, this.currentRowNumber);
            AddNextCell(c, true);
        }

        /// <summary>
        /// Method to insert a generic cell to the next cell position
        /// </summary>
        /// <param name="cell">Cell object to insert</param>
        /// <param name="increment">If true, the address value (row or colum) will be incremented, otherwise not</param>
        private void AddNextCell(Cell cell, bool increment)
        {
            string address = cell.GetCellAddress();
            if (this.cells.ContainsKey(address))
            {
                this.cells[address] = cell;
            }
            else
            {
                this.cells.Add(address, cell);
            }
            if (increment == true)
            {
                if (this.CurrentCellDirection == CellDirection.ColumnToColum)
                {
                    this.currentColumnNumber++;
                }
                else
                {
                    this.currentRowNumber++;
                }
            }
            else
            {
                if (this.CurrentCellDirection == CellDirection.ColumnToColum)
                {
                    this.currentColumnNumber = cell.ColumnAddress + 1;
                    this.currentRowNumber = cell.RowAddress;
                }
                else
                {
                    this.currentColumnNumber = cell.ColumnAddress;
                    this.currentRowNumber = cell.RowAddress + 1;
                }
            }
        }

#endregion

#region AddCell
        /// <summary>
        /// Adds a string value to the defined cell address
        /// </summary>
        /// <param name="value">String value to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCell(string value, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(value, Cell.CellType.STRING, columnAddress, rowAddress);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a string value to the defined cell address
        /// </summary>
        /// <param name="value">String value to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCell(string value, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            AddCell(value, column, row);
        }

        /// <summary>
        /// Adds a integer value to the defined cell address
        /// </summary>
        /// <param name="value">Integer value to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCell(int value, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(value, Cell.CellType.NUMBER, columnAddress, rowAddress);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a integer value to the defined cell address
        /// </summary>
        /// <param name="value">Integer value to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCell(int value, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            AddCell(value, column, row);
        }

        /// <summary>
        /// Adds a double value to the defined cell address
        /// </summary>
        /// <param name="value">Double value to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCell(double value, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(value, Cell.CellType.NUMBER, columnAddress, rowAddress);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a double value to the defined cell address
        /// </summary>
        /// <param name="value">Double value to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCell(double value, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            AddCell(value, column, row);
        }

        /// <summary>
        /// Adds a float value to the defined cell address
        /// </summary>
        /// <param name="value">Float value to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCell(float value, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(value, Cell.CellType.NUMBER, columnAddress, rowAddress);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a float value to the defined cell address
        /// </summary>
        /// <param name="value">Float value to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCell(float value, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            AddCell(value, column, row);
        }

        /// <summary>
        /// Adds a DateTime value to the defined cell address
        /// </summary>
        /// <param name="value">DateTime value to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCell(DateTime value, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(value, Cell.CellType.DATE, columnAddress, rowAddress);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a DateTime value to the defined cell address
        /// </summary>
        /// <param name="value">DateTime value to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCell(DateTime value, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            AddCell(value, column, row);
        }

        /// <summary>
        /// Adds a boolean value to the defined cell address
        /// </summary>
        /// <param name="value">Boolean value to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCell(bool value, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(value, Cell.CellType.BOOL, columnAddress, rowAddress);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a boolean value to the defined cell address
        /// </summary>
        /// <param name="value">Boolean value to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCell(bool value, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            AddCell(value, column, row);
        }

#endregion

#region AddCellFormula
        /// <summary>
        /// Adds a cell formula as string to the defined cell address
        /// </summary>
        /// <param name="formula">Forumla to insert</param>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void AddCellFormula(string formula, string address)
        {
            int column, row;
            Cell.ResolveCellCoordinate(address, out column, out row);
            Cell c = new Cell(formula, Cell.CellType.FORMULA, column, row);
            AddNextCell(c, false);
        }

        /// <summary>
        /// Adds a cell formula as string to the defined cell address
        /// </summary>
        /// <param name="formula">Forumla to insert</param>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void AddCellFormula(string formula, int columnAddress, int rowAddress)
        {
            Cell c = new Cell(formula, Cell.CellType.FORMULA, columnAddress, rowAddress);
            AddNextCell(c, false);
        }
#endregion

#region AddCellRange

        /// <summary>
        /// Adds a list of string values to a defined cell range
        /// </summary>
        /// <param name="values">List of string values to insert from the start address to the end address</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        public void AddCellRange(List<string> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            AddCellRangeInternal(values, startAddress, endAddress);
        }

        /// <summary>
        /// Adds a list of string values to a defined cell range
        /// </summary>
        /// <param name="values">List of string values to insert from the start address to the end address</param>
        /// <param name="cellRange">Cell range as string in the format like A1:D1 or X10:X22</param>
        public void AddCellRange(List<string> values, string cellRange)
        {
            Cell.Address start, end;
            Cell.ResolveCellRange(cellRange, out start, out end);
            AddCellRangeInternal(values, start, end);
        }

        /// <summary>
        /// Adds a list of integer values to a defined cell range
        /// </summary>
        /// <param name="values">List of integer values to insert from the start address to the end address</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        public void AddCellRange(List<int> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            AddCellRangeInternal(values, startAddress, endAddress);
        }

        /// <summary>
        /// Adds a list of integer values to a defined cell range
        /// </summary>
        /// <param name="values">List of integer values to insert from the start address to the end address</param>
        /// <param name="cellRange">Cell range as string in the format like A1:D1 or X10:X22</param>
        public void AddCellRange(List<int> values, string cellRange)
        {
            Cell.Address start, end;
            Cell.ResolveCellRange(cellRange, out start, out end);
            AddCellRangeInternal(values, start, end);
        }

        /// <summary>
        /// Adds a list of double values to a defined cell range
        /// </summary>
        /// <param name="values">List of double values to insert from the start address to the end address</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        public void AddCellRange(List<double> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            AddCellRangeInternal(values, startAddress, endAddress);
        }

        /// <summary>
        /// Adds a list of double values to a defined cell range
        /// </summary>
        /// <param name="values">List of double values to insert from the start address to the end address</param>
        /// <param name="cellRange">Cell range as string in the format like A1:D1 or X10:X22</param>
        public void AddCellRange(List<double> values, string cellRange)
        {
            Cell.Address start, end;
            Cell.ResolveCellRange(cellRange, out start, out end);
            AddCellRangeInternal(values, start, end);
        }

        /// <summary>
        /// Adds a list of float values to a defined cell range
        /// </summary>
        /// <param name="values">List of float values to insert from the start address to the end address</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        public void AddCellRange(List<float> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            AddCellRangeInternal(values, startAddress, endAddress);
        }

        /// <summary>
        /// Adds a list of float values to a defined cell range
        /// </summary>
        /// <param name="values">List of float values to insert from the start address to the end address</param>
        /// <param name="cellRange">Cell range as string in the format like A1:D1 or X10:X22</param>
        public void AddCellRange(List<float> values, string cellRange)
        {
            Cell.Address start, end;
            Cell.ResolveCellRange(cellRange, out start, out end);
            AddCellRangeInternal(values, start, end);
        }

        /// <summary>
        /// Adds a list of DateTime values to a defined cell range
        /// </summary>
        /// <param name="values">List of DateTime values to insert from the start address to the end address</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        public void AddCellRange(List<DateTime> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            AddCellRangeInternal(values, startAddress, endAddress);
        }

        /// <summary>
        /// Adds a list of DateTime values to a defined cell range
        /// </summary>
        /// <param name="values">List of DateTime values to insert from the start address to the end address</param>
        /// <param name="cellRange">Cell range as string in the format like A1:D1 or X10:X22</param>
        public void AddCellRange(List<DateTime> values, string cellRange)
        {
            Cell.Address start, end;
            Cell.ResolveCellRange(cellRange, out start, out end);
            AddCellRangeInternal(values, start, end);
        }

        /// <summary>
        /// Adds a list of boolean values to a defined cell range
        /// </summary>
        /// <param name="values">List of boolean values to insert from the start address to the end address</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        public void AddCellRange(List<bool> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            AddCellRangeInternal(values, startAddress, endAddress);
        }

        /// <summary>
        /// Adds a list of boolean values to a defined cell range
        /// </summary>
        /// <param name="values">List of boolean values to insert from the start address to the end address</param>
        /// <param name="cellRange">Cell range as string in the format like A1:D1 or X10:X22</param>
        public void AddCellRange(List<bool> values, string cellRange)
        {
            Cell.Address start, end;
            Cell.ResolveCellRange(cellRange, out start, out end);
            AddCellRangeInternal(values, start, end);
        }
        
        /// <summary>
        /// Internal function to add a generic list of value to the defined cell range
        /// </summary>
        /// <typeparam name="T">Data type of the generic value list</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="startAddress">Start address</param>
        /// <param name="endAddress">End address</param>
        /// <exception cref="OutOfRangeException">Throws a OutOfRangeException if the number of cells differs from the number of passed values</exception>
        private void AddCellRangeInternal<T>(List<T> values, Cell.Address startAddress, Cell.Address endAddress)
        {
            List<Cell.Address> addresses = Cell.GetCellRange(startAddress, endAddress);
            if (values.Count != addresses.Count)
            {
                throw new OutOfRangeException("The number of passed values (" + values.Count.ToString() + ") differs from the number of cells within the range (" + addresses.Count.ToString() + ")");
            }
            List<Cell> list = Cell.ConvertArray<T>(values);
            int len = values.Count;
            for(int i = 0; i < len; i++)
            {
                list[i].RowAddress = addresses[i].Row;
                list[i].ColumnAddress = addresses[i].Column;
                AddNextCell(list[i], false);
            }
        }
#endregion

#region RemoveCell
        /// <summary>
        /// Removes a previous inserted cell at the defined address
        /// </summary>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        /// <returns>Returns true if the cell could be removed (existed), otherwise false (did not exist)</returns>
        public bool RemoveCell(int columnAddress, int rowAddress)
        {
            string address = Cell.ResolveCellAddress(columnAddress, rowAddress);
            if (this.cells.ContainsKey(address))
            {
                this.cells.Remove(address);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a previous inserted cell at the defined address
        /// </summary>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        /// <returns>Returns true if the cell could be removed (existed), otherwise false (did not exist)</returns>
        public bool RemoveCell(string address)
        {
            int row, column;
            Cell.ResolveCellCoordinate(address, out column, out row);
            return RemoveCell(column, row);
        }
#endregion

        /// <summary>
        /// Moves the current position to the next column
        /// </summary>
        public void GoToNextColumn()
        {
            this.currentColumnNumber++;
            this.currentRowNumber = 0;
        }

        /// <summary>
        /// Moves the current position to the next row (use for a new line)
        /// </summary>
        public void GoToNextRow()
        {
            this.currentRowNumber++;
            this.currentColumnNumber = 0;
        }

        /// <summary>
        /// Sets the current row address (row number, zero based)
        /// </summary>
        /// <param name="rowAddress">Row number (zero based)</param>
        /// <exception cref="OutOfRangeException">Throws a OutOfRangeException if the address is out of the valid range. Range is from 0 to 1048575 (1048576 rows)</exception>
        public void SetCurrentRowAddress(int rowAddress)
        {
            if (rowAddress >= 1048576 || rowAddress < 0)
            {
                throw new OutOfRangeException("The row number (" + rowAddress.ToString() + ") is out of range. Range is from 0 to 1048575 (1048576 rows).");
            }
            this.currentRowNumber = rowAddress;
        }

        /// <summary>
        /// Sets the current column address (column number, zero based)
        /// </summary>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <exception cref="OutOfRangeException">Throws a OutOfRangeException if the address is out of the valid range. Range is from 0 to 1048575 (1048576 rows)</exception>
        public void SetCurrentColumnAddress(int columnAddress)
        {
            if (columnAddress >= 16383 || columnAddress < 0)
            {
                throw new OutOfRangeException("The column number (" + columnAddress.ToString() + ") is out of range. Range is from 0 to 16383 (16384 columns).");
            }
            this.currentColumnNumber = columnAddress;
        }

        /// <summary>
        /// Set the current cell address
        /// </summary>
        /// <param name="address">Cell address in the format A1 - XFD16384</param>
        public void SetCurentCellAddress(string address)
        {
            int row, column;
            Cell.ResolveCellCoordinate(address, out column, out row);
            SetCurentCellAddress(column, row);
        }

        /// <summary>
        /// Set the current cell address
        /// </summary>
        /// <param name="columnAddress">Column number (zero based)</param>
        /// <param name="rowAddress">Row number (zero based)</param>
        public void SetCurentCellAddress(int columnAddress, int rowAddress)
        {
            SetCurrentColumnAddress(columnAddress);
            SetCurrentRowAddress(rowAddress);
        }

        /// <summary>
        /// Validates and sets the worksheet name
        /// </summary>
        /// <param name="name">Name to set</param>
        /// <exception cref="FormatException">Throws a FormatException if the sheet name is to long (max. 31) or contains illegal characters [  ]  * ? / \</exception>
        public void SetSheetname(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("The sheet name must be between 1 and 31 characters");
            }
            if (name.Length > 31)
            {
                throw new FormatException("The sheet name must be between 1 and 31 characters");
            }
            Regex rx = new Regex(@"[\[\]\*\?/\\]");
            Match mx = rx.Match(name);
            if (mx.Captures.Count > 0)
            {
                throw new FormatException(@"The sheet name must must not contain the characters [  ]  * ? / \ ");
            }
            this.sheetName = name;
        }
        
    }
}