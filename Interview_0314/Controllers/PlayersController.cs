using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Interview_0314.Models;
using Microsoft.AspNetCore.Authorization;

namespace Interview_0314.Controllers
{
    [Authorize]
    public class PlayersController : Controller
    {
        private readonly AppDbContext _context;

        public PlayersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Player.Include(p => p.Position).Include(p => p.Team);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.Position)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,TeamId,PositionId,PhotoPath")] Player player)
        public async Task<IActionResult> Create([Bind("Id,Name,TeamId,PositionId,PhotoPath")] Player player, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();

                if (Photo != null && Photo.Length > 0)
                {
                    // 確保 wwwroot/images 目錄存在
                    var uploadsFolder = Path.Combine("wwwroot", "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // 使用 Player ID 作為檔名，保留原始副檔名
                    var fileExtension = Path.GetExtension(Photo.FileName); // 取得副檔名 (.jpg, .png, etc.)
                    var fileName = player.Id + fileExtension; // 讓檔名與 ID 相同
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(stream);
                    }

                    // 更新 Player 的 PhotoPath
                    player.PhotoPath = "/images/" + fileName;
                    _context.Update(player);
                    await _context.SaveChangesAsync(); // 再次儲存
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("錯誤訊息: " + string.Join(", ", errors));                
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", player.PositionId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", player.PositionId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TeamId,PositionId,PhotoPath")] Player player, IFormFile? Photo)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPlayer = await _context.Player.FindAsync(id);
                    if (existingPlayer == null)
                    {
                        return NotFound();
                    }

                    if (Photo != null && Photo.Length > 0)
                    {
                        // 確保 wwwroot/images 目錄存在
                        var uploadsFolder = Path.Combine("wwwroot", "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // 刪除舊圖片
                        if (!string.IsNullOrEmpty(existingPlayer.PhotoPath))
                        {
                            var oldFilePath = Path.Combine("wwwroot", existingPlayer.PhotoPath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // 使用 Player ID 作為新檔名，保留副檔名
                        var fileExtension = Path.GetExtension(Photo.FileName);
                        var fileName = player.Id + fileExtension;
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Photo.CopyToAsync(stream);
                        }

                        // 更新 PhotoPath
                        existingPlayer.PhotoPath = "/images/" + fileName;
                    }

                    // 更新其他欄位
                    existingPlayer.Name = player.Name;
                    existingPlayer.TeamId = player.TeamId;
                    existingPlayer.PositionId = player.PositionId;

                    _context.Update(existingPlayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", player.PositionId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.Position)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Player.FindAsync(id);
            if (player != null)
            {
                // 刪除圖片
                if (!string.IsNullOrEmpty(player.PhotoPath))
                {
                    var filePath = Path.Combine("wwwroot", player.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Player.Remove(player);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.Id == id);
        }
    }
}
