using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proiect_TW.Models;
using Proiect_TW.Services;

namespace Proiect_TW.Controllers
{
    public class TransferCardController : Controller
    {
        private readonly TransferCardService _transferCardService;

        public TransferCardController(TransferCardService transferCardService)
        {
            _transferCardService = transferCardService;
        }

        public async Task<IActionResult> Index()
        {
            var transfers = await _transferCardService.GetAllTransfersAsync();
            return View(transfers);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransferCard transfer)
        {
            if (ModelState.IsValid)
            {
                await _transferCardService.CreateTransferAsync(transfer);
                return RedirectToAction(nameof(Index));
            }
            return View(transfer);
        }
    }
} 