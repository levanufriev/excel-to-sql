using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using WebInterface.Data;
using WebInterface.Models;

namespace WebInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            await UploadFile(file);
            TempData["msg"] = "The file was uploaded successfully";
            return View();
        }

        //Upload file on server
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            bool isCopied = false;
            try
            {
                if (file.Length > 0)
                {
                    //Get file name.
                    string fileName = Path.GetFileName(file.FileName);

                    //Set path to the Upload folder.
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));

                    //Create file
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    //File was succesfully copied.
                    isCopied = true;
                }
                else
                {
                    isCopied = false;
                }
            }
            catch (Exception)
            {
                throw;
            }

            await ProcessExcel(Path.Combine(path, Path.GetFileName(file.FileName)));
            return isCopied;
        }

        //Process Excel file.
        private async Task ProcessExcel(string filePath)
        {
            //Connection with excel file.
            Application excel = new Application();
            Workbook workbook = excel.Workbooks.Open(filePath);
            Worksheet worksheet = workbook.Worksheets[1];

            //Add a row to the Bank table.
            Bank bank = new Bank { Name = worksheet.Cells[1, 1].Value };
            await context.Banks.AddAsync(bank);
            await context.SaveChangesAsync();

            string className = "";

            for (int i = 9; worksheet.Cells[i, 1].Value != null; i++)
            {
                //Account number.
                int account;

                //Check only account numbers.
                if (worksheet.Cells[i, 1].Value is not string)
                {
                    continue;
                }

                //Save class name.
                if (!int.TryParse(worksheet.Cells[i, 1].Value, out account) && worksheet.Cells[i, 1].Value!="ПО КЛАССУ" && worksheet.Cells[i, 1].Value!="БАЛАНС")
                {
                    className = worksheet.Cells[i, 1].Value;
                }                
                else if (int.TryParse(worksheet.Cells[i, 1].Value, out account) && account > 100)
                {
                    //Fill the sheet table.
                    Sheet sheet = new Sheet()
                    {
                        Account = account,
                        TwoDigitAccount = account / 100,
                        InputBalanceActive = (decimal)worksheet.Cells[i, 2].Value,
                        InputBalancePassive = (decimal)worksheet.Cells[i, 3].Value,
                        Debit = (decimal)worksheet.Cells[i, 4].Value,
                        Credit = (decimal)worksheet.Cells[i, 5].Value,
                        OutputBalanceActive = (decimal)worksheet.Cells[i, 6].Value,
                        OutputBalancePassive = (decimal)worksheet.Cells[i, 7].Value,
                        ClassName = className,
                        BankId = bank.Id,
                    };

                    await context.Sheets.AddAsync(sheet);
                    await context.SaveChangesAsync();
                }
            }

            workbook.Close();
        }
    }
}