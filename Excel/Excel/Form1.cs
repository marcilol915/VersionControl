using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using exc = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Excel
{
    public partial class Form1 : Form
    {
        
        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

        RealEstateEntities context = new RealEstateEntities();
        List<Flat> Flats;
        private void LoadData()
        {
            Flats = context.Flats.ToList();
        }
        exc.Application xlApp; 
        exc.Workbook xlWB; 
        exc.Worksheet xlSheet;
        private void CreateTable()
        {
            string[] headers = new string[] {
     "Kód",
     "Eladó",
     "Oldal",
     "Kerület",
     "Lift",
     "Szobák száma",
     "Alapterület (m2)",
     "Ár (mFt)",
     "Négyzetméter ár (Ft/m2)"};
            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }
            object[,] values = new object[Flats.Count, headers.Length];
            int counter = 0;
            foreach (Flat f in Flats)
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;
                values[counter, 4] = f.Elevator;
                values[counter, 5] = f.NumberOfRooms;
                values[counter, 6] = f.FloorArea;
                values[counter, 7] = f.Price;
                values[counter, 8] = (f.Price/f.FloorArea)*1000000;
                counter++;
                xlSheet.get_Range(
            GetCell(2, 1),
            GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;
            }
        }
        private void CreateExcel()
        {
            try
            {
                // Excel elindítása és az applikáció objektum betöltése
                xlApp = new exc.Application();

                // Új munkafüzet
                xlWB = xlApp.Workbooks.Add(Missing.Value);

                // Új munkalap
                xlSheet = xlWB.ActiveSheet;

                // Tábla létrehozása
                CreateTable(); // Ennek megírása a következő feladatrészben következik

                // Control átadása a felhasználónak
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex) // Hibakezelés a beépített hibaüzenettel
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                // Hiba esetén az Excel applikáció bezárása automatikusan
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }
        private void FormatTable()
        {
            string[] headers = new string[] {
     "Kód",
     "Eladó",
     "Oldal",
     "Kerület",
     "Lift",
     "Szobák száma",
     "Alapterület (m2)",
     "Ár (mFt)",
     "Négyzetméter ár (Ft/m2)"};
            exc.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = exc.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = exc.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(exc.XlLineStyle.xlContinuous, exc.XlBorderWeight.xlThick);
        }
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }
    }
}
